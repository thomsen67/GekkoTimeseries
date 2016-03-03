using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.ComponentModel;
using System.IO;

namespace GekkoFlowChart
{
    public class FlowGlobals
    {
        /// <summary>
        /// The list of rectangles that is displayed both in the main window and in the overview window.
        /// </summary>
        public static ObservableCollection<RectangleData> rectangles;

        /// <summary>
        /// The list of arrows that is displayed both in the main window and in the overview window.
        /// </summary>
        public static ObservableCollection<ArrowData> arrows;
    }    
    
    /// <summary>
    /// A simple example of a data-model.  
    /// The purpose of this data-model is to share display data between the main window and overview window.
    /// </summary>
    public class DataModel : INotifyPropertyChanged
    {
        #region Data Members

        /// <summary>
        /// The singleton instance.
        /// This is a singleton for convenience.
        /// </summary>        
        public static DataModel instance = new DataModel(-12345, double.NaN, false);        

        ///
        /// The current scale at which the content is being viewed.
        /// 
        private double contentScale = 1;

        ///
        /// The X coordinate of the offset of the viewport onto the content (in content coordinates).
        /// 
        private double contentOffsetX = 0;

        ///
        /// The Y coordinate of the offset of the viewport onto the content (in content coordinates).
        /// 
        private double contentOffsetY = 0;

        ///
        /// The width of the content (in content coordinates).
        /// 
        public double contentWidth = 2000;

        ///
        /// The heigth of the content (in content coordinates).
        /// 
        //fio:
        public double contentHeight = 2000;

        ///
        /// The width of the viewport onto the content (in content coordinates).
        /// The value for this is actually computed by the main window's ZoomAndPanControl and update in the
        /// data model so that the value can be shared with the overview window.
        /// 
        private double contentViewportWidth = 0;

        ///
        /// The heigth of the viewport onto the content (in content coordinates).
        /// The value for this is actually computed by the main window's ZoomAndPanControl and update in the
        /// data model so that the value can be shared with the overview window.
        /// 
        private double contentViewportHeight = 0;

        #endregion Data Members

        /// <summary>
        /// Retreive the singleton instance.
        /// </summary>
        public static DataModel Instance
        {
            get
            {
                return instance;
            }
        }

        public DataModel(int iterations, double weightMax, bool firstTime)
        {
            if (FruchtermanReingoldLayout.h == null) return;
            //
            // Populate the data model with some example data.
            //            
            if (firstTime || FlowGlobals.rectangles == null)
            {
                FlowGlobals.rectangles = new ObservableCollection<RectangleData>();
                FlowGlobals.arrows = new ObservableCollection<ArrowData>();
            }
            else
            {
                FlowGlobals.rectangles.Clear();
                FlowGlobals.arrows.Clear();
            }
            ObservableCollection<RectangleData> rectangles = FlowGlobals.rectangles;
            ObservableCollection<ArrowData> arrows = FlowGlobals.arrows;

            double width = 90;
            double height = 70;

            Random r = new Random(12345);
            r = new Random(54321);
            r = new Random(11111);
            List<EdgeData> edges = new List<EdgeData>();
            List<NodeData> nodes = new List<NodeData>();

            NodeData startNode = null;

            foreach (FlowArrow fa in FruchtermanReingoldLayout.h.flowArrows)
            {
                if (!double.IsNaN(weightMax) && Math.Abs(fa.weight) < weightMax) continue;
                EdgeData ed = new EdgeData { from = fa.counter1, to = fa.counter2, weight = fa.weight };
                edges.Add(ed);
            }

            foreach (FlowNode fa in FruchtermanReingoldLayout.h.flowNodes)
            {
                NodeData nd = new NodeData { x = r.Next((int)(0.005d * contentWidth), (int)contentWidth), y = r.Next((int)(0.005d * contentWidth), (int)contentWidth), name = fa.varName, labelBig = fa.labelBig, isExogenous = fa.isExogenous, isStartNode = fa.isStartNode, id = fa.id };
                nodes.Add(nd);
                if (nd.isStartNode) startNode = nd;
            }

            Dictionary<int, NodeData> d = new Dictionary<int, NodeData>();
            foreach (NodeData nd in nodes)
            {
                d.Add(nd.id, nd);
            }

            //TODO: The Dijkstra algorithm is a bit sloppy, for instance using List<int> and removing items from this list (use Dictionary instead)
            DijkstraInfo di = new DijkstraInfo();
            di.nodes = nodes;
            di.edges = edges;
            Dijkstra dijkstra = new Dijkstra(nodes.Count, new Dijkstra.InternodeTraversalCost(getInternodeTraversalCost), new Dijkstra.NearbyNodesHint(nearbyNodesHint), di);
            Dijkstra.Results results = dijkstra.Perform(startNode.id, di);
            //int[] minimumPath = dijkstra.GetMinimumPath(0, 102, di);

            //Now we filter out 

            List<EdgeData> edges2 = new List<EdgeData>();
            List<NodeData> nodes2 = new List<NodeData>();

            int large = 1000000000;
            Dictionary<int, int> rename = new Dictionary<int, int>();            
            foreach (NodeData nd in nodes)
            {
                int value = results.MinimumDistance[nd.id];
                if (value < 0) value = large;  //do not minus it: -int.MinValue gives a strange result!
                if (value < large)  //it seems these distances can become = int.MinValue, maybe int.MaxValue+1. So this is to identify those.
                {
                    int number = nodes2.Count;                    
                    rename.Add(nd.id, number);
                    nd.id = number;  //now id numbers in List 'nodes' are inconsistent, but never mind that
                    nodes2.Add(nd);
                }
            }

            foreach (EdgeData ed in edges)
            {
                if (rename.ContainsKey(ed.from) && rename.ContainsKey(ed.to))
                {
                    ed.from = rename[ed.from];
                    ed.to = rename[ed.to];
                    edges2.Add(ed); //now id numbers in List 'edges' are inconsistent, but never mind that
                }
            }
            
            SolveInfo si = new SolveInfo();
            si.width = width;
            si.height = height;
            si.rectangles = rectangles;
            si.arrows = arrows;
            si.edges = edges2;
            si.nodes = nodes2;
            si.iterations = iterations;
            if (si.iterations == -12345) si.iterations = 200;
            si.useSmartRepulse = true;
            si.useWeight = true;
            FruchtermanReingoldLayout.Solve(si);
        }

        private IEnumerable<int> nearbyNodesHint(int startingNode, DijkstraInfo di)
        {
            //TODO: If Dijkstra gets slow
            //This is sloppy in two ways: 
            //(1) It loops through all edges/arrows: could have pointers in the nodes themselves ('parents' and 'children')
            //(2) It creates a new List<int>. If the above is done, we could just point to the 'children' list.
            //See also getInternodeTraversalCost()
            List<int> nearbyNodes = new List<int>();            
            foreach (EdgeData ed in di.edges)
            {
                if (ed.to == startingNode) nearbyNodes.Add(ed.from);  //there should be no dublets in this list
            }
            return nearbyNodes;
        }

        private int getInternodeTraversalCost(int start, int finish, DijkstraInfo di)
        {
            //TODO: If Dijkstra gets slow
            //This is sloppy: 
            //(1) It loops through all edges/arrows: could have pointers in the nodes themselves ('parents' and 'children')
            //    Then we could check the start node and see if it points to finish node. (Or make a hashtable of connections)
            //See also nearbyNodesHint()
            foreach (EdgeData ed in di.edges)
            {
                if (ed.to == start && ed.from == finish) return 1;
            }
            return int.MaxValue;
        }   

        /// <summary>
        /// The list of rectangles that is displayed both in the main window and in the overview window.
        /// </summary>
        public ObservableCollection<RectangleData> Rectangles
        {
            get
            {
                return FlowGlobals.rectangles;
            }
        }

        /// <summary>
        /// The list of arrows that is displayed both in the main window and in the overview window.
        /// </summary>
        public ObservableCollection<ArrowData> Arrows
        {
            get
            {
                return FlowGlobals.arrows;
            }
        }

        ///
        /// The current scale at which the content is being viewed.
        /// 
        public double ContentScale
        {
            get
            {
                return contentScale;
            }
            set
            {
                contentScale = value;

                OnPropertyChanged("ContentScale");
            }
        }

        ///
        /// The X coordinate of the offset of the viewport onto the content (in content coordinates).
        /// 
        public double ContentOffsetX
        {
            get
            {
                return contentOffsetX;
            }
            set
            {
                contentOffsetX = value;

                OnPropertyChanged("ContentOffsetX");
            }
        }

        ///
        /// The Y coordinate of the offset of the viewport onto the content (in content coordinates).
        /// 
        public double ContentOffsetY
        {
            get
            {
                return contentOffsetY;
            }
            set
            {
                contentOffsetY = value;

                OnPropertyChanged("ContentOffsetY");
            }
        }

        ///
        /// The width of the content (in content coordinates).
        /// 
        public double ContentWidth
        {
            get
            {
                return contentWidth;
            }
            set
            {
                contentWidth = value;

                OnPropertyChanged("ContentWidth");
            }
        }

        ///
        /// The heigth of the content (in content coordinates).
        /// 
        public double ContentHeight
        {
            get
            {
                return contentHeight;
            }
            set
            {
                contentHeight = value;

                OnPropertyChanged("ContentHeight");
            }
        }

        ///
        /// The width of the viewport onto the content (in content coordinates).
        /// The value for this is actually computed by the main window's ZoomAndPanControl and update in the
        /// data model so that the value can be shared with the overview window.
        /// 
        public double ContentViewportWidth
        {
            get
            {
                return contentViewportWidth;
            }
            set
            {
                contentViewportWidth = value;

                OnPropertyChanged("ContentViewportWidth");
            }
        }

        ///
        /// The heigth of the viewport onto the content (in content coordinates).
        /// The value for this is actually computed by the main window's ZoomAndPanControl and update in the
        /// data model so that the value can be shared with the overview window.
        /// 
        public double ContentViewportHeight
        {
            get
            {
                return contentViewportHeight;
            }
            set
            {
                contentViewportHeight = value;

                OnPropertyChanged("ContentViewportHeight");
            }
        }

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Raises the 'PropertyChanged' event when the value of a property of the data model has changed.
        /// </summary>
        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        /// <summary>
        /// 'PropertyChanged' event that is raised when the value of a property of the data model has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        

        #endregion
    }
    public class EdgeData
    {
        public int from;
        public int to;
        public double weight;
    }

    public class NodeData
    {
        public double x;
        public double y;
        public string name;
        public double weight;
        public string labelBig;
        public bool isStartNode;
        public bool isExogenous;
        public int id;
        public int distance = -12345;
    }

    public class DijkstraInfo
    {
        public List<NodeData> nodes;
        public List<EdgeData> edges;
    }

    public class SolveInfo
    {
        public double height;        
        public double width;        
        public List<EdgeData> edges;
        public List<NodeData> nodes;
        public ObservableCollection<RectangleData> rectangles;
        public ObservableCollection<ArrowData> arrows;
        public int iterations;
        public bool useSmartRepulse = false;
        public bool useWeight = false;
    }

}
