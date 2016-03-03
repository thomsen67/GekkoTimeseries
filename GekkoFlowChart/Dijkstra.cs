using System;
using System.Collections.Generic;
using System.Text;

namespace GekkoFlowChart
{

    /// <summary> 
    /// Implements a generalized Dijkstra's algorithm to calculate 
    /// both minimum distance and minimum path. 
    /// </summary> 
    /// <remarks> 
    /// For this algorithm, all nodes should be provided, and handled 
    /// in the delegate methods, including the start and finish nodes. 
    /// </remarks> 
    public class Dijkstra
    {
        /// <summary> 
        /// An optional delegate that can help optimize the algorithm 
        /// by showing it a subset of nodes to consider. Very useful 
        /// for limited connectivity graphs. (like pixels on a screen!) 
        /// </summary> 
        /// <param name="startingNode"> 
        /// The node that is being traveled away FROM. 
        /// </param> 
        /// <returns> 
        /// An array of nodes that might be reached from the  
        /// <paramref name="startingNode"/>. 
        /// </returns> 
        public delegate IEnumerable<int> NearbyNodesHint(int startingNode, DijkstraInfo di);

        /// <summary> 
        /// Determines the cost of moving from a given node to another given node. 
        /// </summary> 
        /// <param name="start"> 
        /// The node being moved away from. 
        /// </param> 
        /// <param name="finish"> 
        /// The node that may be moved to. 
        /// </param> 
        /// <returns> 
        /// The cost of the transition from <paramref name="start"/> to 
        /// <paramref name="finish"/>, or <see cref="Int32.MaxValue"/> 
        /// if the transition is impossible (i.e. there is no edge between  
        /// the two nodes). 
        /// </returns>
        ///
        public delegate int InternodeTraversalCost(int start, int finish, DijkstraInfo di);

        /// <summary> 
        /// Creates an instance of the <see cref="Dijkstra"/> class. 
        /// </summary> 
        /// <param name="totalNodeCount"> 
        /// The total number of nodes in the graph. 
        /// </param> 
        /// <param name="traversalCost"> 
        /// The delegate that can provide the cost of a transition between 
        /// any two nodes. 
        /// </param> 
        /// <param name="hint"> 
        /// An optional delegate that can provide a small subset of nodes 
        /// that a given node may be connected to. 
        /// </param> 
        public Dijkstra(int totalNodeCount, InternodeTraversalCost traversalCost, NearbyNodesHint hint, DijkstraInfo di)
        {
            if (totalNodeCount < 3) throw new ArgumentOutOfRangeException("totalNodeCount", totalNodeCount, "Expected a minimum of 3.");
            if (traversalCost == null) throw new ArgumentNullException("traversalCost");
            Hint = hint;
            TraversalCost = traversalCost;
            TotalNodeCount = totalNodeCount;
        }

        protected readonly NearbyNodesHint Hint;
        protected readonly InternodeTraversalCost TraversalCost;
        protected readonly int TotalNodeCount;

        /// <summary> 
        /// The composite product of a Dijkstra algorithm. 
        /// </summary> 
        public struct Results
        {
            /// <summary> 
            /// Prepares a Dijkstra results package. 
            /// </summary> 
            /// <param name="minimumPath"> 
            /// The minimum path array, where each array element index corresponds  
            /// to a node designation, and the array element value is a pointer to 
            /// the node that should be used to travel to this one. 
            /// </param> 
            /// <param name="minimumDistance"> 
            /// The minimum distance from the starting node to the given node. 
            /// </param> 
            public Results(int[] minimumPath, int[] minimumDistance)
            {
                MinimumDistance = minimumDistance;
                MinimumPath = minimumPath;
            }

            /// <summary> 
            /// The minimum path array, where each array element index corresponds  
            /// to a node designation, and the array element value is a pointer to 
            /// the node that should be used to travel to this one. 
            /// </summary> 
            public readonly int[] MinimumPath;

            /// <summary> 
            /// The minimum distance from the starting node to the given node. 
            /// </summary> 
            public readonly int[] MinimumDistance;
        }

        /// <summary> 
        /// Performs the Dijkstra algorithm on the data provided when the  
        /// <see cref="Dijkstra"/> object was instantiated. 
        /// </summary> 
        /// <param name="start"> 
        /// The node to use as a starting location. 
        /// </param> 
        /// <returns> 
        /// A struct containing both the minimum distance and minimum path 
        /// to every node from the given <paramref name="start"/> node. 
        /// </returns> 
        public virtual Results Perform(int start, DijkstraInfo di)
        {
            // Initialize the distance to every node from the starting node. 
            int[] d = GetStartingTraversalCost(start, di);
            // Initialize best path to every node as from the starting node. 
            int[] p = GetStartingBestPath(start, di);
            ICollection<int> c = GetChoices();

            c.Remove(start); // take starting node out of the list of choices 

            //Debug.WriteLine("Step v C D P"); 
            //Debug.WriteLine(string.Format("init - {{{0}}} [{1}] [{2}]", 
            // ArrayToString<int>(",", c), ArrayToString<int>(",", d), ArrayToString<int>(",", p))); 
            //int step = 0; 

            // begin greedy loop 
            while (c.Count > 1)
            {
                // Find element v in c, that minimizes d[v] 
                int v = FindMinimizingDinC(d, c);
                c.Remove(v); // remove v from the list of future solutions 
                // Consider all unselected nodes and consider their cost from v. 
                foreach (int w in (Hint != null ? Hint(v, di) : c))
                {
                    if (!c.Contains(w)) continue; // discard pixels not in c 
                    // At this point, relative(Index) points to a candidate pixel,  
                    // that has not yet been selected, and lies within our area of interest. 
                    // Consider whether it is now within closer reach. 
                    int cost = TraversalCost(v, w, di);
                    if (cost < int.MaxValue && d[v] + cost < d[w]) // don't let wrap-around negatives slip by 
                    {
                        // We have found a better way to get at relative 
                        d[w] = d[v] + cost; // record new distance 
                        // Record how we came to this new pixel 
                        p[w] = v;
                    }
                }
                //Debug.WriteLine(string.Format("{4} {3} {{{0}}} [{1}] [{2}]", 
                // ArrayToString<int>(",", c), ArrayToString<int>(",", d), ArrayToString<int>(",", p), v + 1, ++step)); 
            }

            return new Results(p, d);
        }

        /// <summary> 
        /// Uses the Dijkstra algorithhm to find the minimum path 
        /// from one node to another. 
        /// </summary> 
        /// <param name="start"> 
        /// The node to use as a starting location. 
        /// </param> 
        /// <param name="finish"> 
        /// The node to use as a finishing location. 
        /// </param> 
        /// <returns> 
        /// A struct containing both the minimum distance and minimum path 
        /// to every node from the given <paramref name="start"/> node. 
        /// </returns> 
        public virtual int[] GetMinimumPath(int start, int finish, DijkstraInfo di)
        {
            Results results = Perform(start, di);
            return GetMinimumPath(start, finish, results.MinimumPath, di);
        }

        /// <summary> 
        /// Finds an array of nodes that provide the shortest path 
        /// from one given node to another. 
        /// </summary> 
        /// <param name="start"> 
        /// The starting node. 
        /// </param> 
        /// <param name="finish"> 
        /// The finishing node. 
        /// </param> 
        /// <param name="shortestPath"> 
        /// The P array of the completed algorithm. 
        /// </param> 
        /// <returns> 
        /// The list of nodes that provide the one step at a time path  
        /// from <paramref name="start"/> to <paramref name="finish"/> nodes. 
        /// </returns> 
        protected virtual int[] GetMinimumPath(int start, int finish, int[] shortestPath, DijkstraInfo di)
        {
            Stack<int> path = new Stack<int>();
            do
            {
                path.Push(finish);
                finish = shortestPath[finish]; // step back one step toward the start point 
            }
            while (finish != start);
            return path.ToArray();
        }

        /// <summary> 
        /// Initializes the P array for the algorithm. 
        /// </summary> 
        /// <param name="startingNode"> 
        /// The node that has been designated the starting node for the entire algorithm. 
        /// </param> 
        /// <returns> 
        /// The new P array. 
        /// </returns> 
        /// <remarks> 
        /// A fresh P array will set every single node's source node to be  
        /// the starting node, including the starting node itself. 
        /// </remarks> 
        protected virtual int[] GetStartingBestPath(int startingNode, DijkstraInfo di)
        {
            int[] p = new int[TotalNodeCount];
            for (int i = 0; i < p.Length; i++)
                p[i] = startingNode;
            return p;
        }

        /// <summary> 
        /// Finds the yet-unconsidered node that has the least cost to reach. 
        /// </summary> 
        /// <param name="d"> 
        /// The cost of reaching any node. 
        /// </param> 
        /// <param name="c"> 
        /// The nodes that are still available for picking. 
        /// </param> 
        /// <returns> 
        /// The node that is closest (has the shortest special path). 
        /// </returns> 
        protected virtual int FindMinimizingDinC(int[] d, ICollection<int> c)
        {
            int bestIndex = -1;
            foreach (int ci in c)
                if (bestIndex == -1 || d[ci] < d[bestIndex])
                    bestIndex = ci;
            return bestIndex;
        }

        /// <summary> 
        /// Initializes an collection of all nodes not yet considered. 
        /// </summary> 
        /// <returns> 
        /// The initialized collection. 
        /// </returns> 
        protected virtual ICollection<int> GetChoices()
        {
            ICollection<int> choices = new List<int>(TotalNodeCount);
            for (int i = 0; i < TotalNodeCount; i++)
                choices.Add(i);
            return choices;
        }

        /// <summary> 
        /// Initializes the D array for the start of the algorithm. 
        /// </summary> 
        /// <param name="start"> 
        /// The starting node. 
        /// </param> 
        /// <returns> 
        /// The contents of the new D array. 
        /// </returns> 
        /// <remarks> 
        /// The traversal cost for every node will be set to impossible 
        /// (int.MaxValue) unless a connecting edge is found between the 
        /// <paramref name="start"/>ing node and the node in question. 
        /// </remarks> 
        protected virtual int[] GetStartingTraversalCost(int start, DijkstraInfo di)
        {
            int[] subset = new int[TotalNodeCount];
            for (int i = 0; i < subset.Length; i++)
                subset[i] = int.MaxValue; // all are unreachable 
            subset[start] = 0; // zero cost from start to start 
            foreach (int nearby in Hint(start, di))
                subset[nearby] = TraversalCost(start, nearby, di);
            return subset;
        }

        /// <summary> 
        /// Joins the elements of an array into a string, using 
        /// a given separator. 
        /// </summary> 
        /// <typeparam name="T">The type of element in the array.</typeparam> 
        /// <param name="separator">The seperator to insert between each element.</param> 
        /// <param name="array">The array.</param> 
        /// <returns>The resulting string.</returns> 
        /// <remarks> 
        /// This is very much like <see cref="string.Join"/>, except 
        /// that it works on arrays of non-strings. 
        /// </remarks> 
        protected string ArrayToString<T>(string separator, IEnumerable<int> array)
        {
            StringBuilder sb = new StringBuilder();
            foreach (int t in array)
                sb.AppendFormat("{0}{1}", t < int.MaxValue ? t + 1 : t, separator);
            sb.Length -= separator.Length;
            return sb.ToString();
        }
    }
}
