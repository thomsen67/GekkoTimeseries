using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Media;


namespace GekkoFlowChart
{
    public class FruchtermanReingoldLayout
    {
        public static Helper h;
        
        public static void Solve(SolveInfo si)
        {
            /*
             Rectangle w, h
             fArea = w*h
             m_fC = 1
             k = m_fC * Sqrt(fArea / #nodes) =ca= avg. distance between nodes * m_fC
             temperature = w / 10             
             
             Repulse:
             tDisplacementX += tDeltaX * (k*k / distance_nodes) / distance_nodes)
             tDisplacementY += tDeltaY * (k*k / distance_nodes) / distance_nodes)
                
             Attract:
             faOverDelta = 
             tFactorX = tDeltaX * (distance_nodes * distance_nodes) / k / distance_nodes)
             tFactorY = tDeltaY * (distance_nodes * distance_nodes) / k / distance_nodes)
             displacement[oEdge.from].x += tFactorX;
             displacement[oEdge.from].y += tFactorY;
             displacement[oEdge.to].x -= tFactorX;
             displacement[oEdge.to].y -= tFactorY;              
              
             For some reason, the forces are both divided by distance_nodes.
             Distance between two nodes would be k if only 2 connected nodes in whole graph.
             
             repulse: k*k/d
             attract: d*d/k
              
             Equilibrium: k*k/d = d*d/k --> d = k
             
             Alternative: 
             d*d/k = k*k*k/(d*d) --> hmmmm is that better?
             
             
              
             
             */

            int iterstop = si.iterations;
            double factor = 50;
            int m_iIterations = 200;
            FRRectangle oRectangle= new FRRectangle() { Width = 200, Height = 200 };
            bool skipBarriers = false;
            int counter = -1;            
            double m_fC = 1.0d;

            int iVertices = si.nodes.Count;

            List<Displacement> displacement = new List<Displacement>();
            for (int i = 0; i < iVertices; i++) displacement.Add(new Displacement());

            // If the graph has already been laid out, use the current vertex
            // locations as initial values.

            //randomize locations here!        

            double fArea = oRectangle.Width * oRectangle.Height;

            // The algorithm in Figure 1 of the Fruchterman-Reingold paper doesn't
            // include the constant C, but it is included in the calculation for k
            // under the "Modelling the forces" section.

            double k = m_fC * Math.Sqrt(fArea / (double)iVertices);

            // The rectangle is guaranteed to have non-zero width and height, so
            // k should never be zero.        

            // Use the simple cooling algorithm suggested in the Fruchterman-
            // Reingold paper.

            double fTemperature = oRectangle.Width / 10d;

            double fT = fTemperature;

            double fTemperatureDecrement = fTemperature / (double)m_iIterations;

            while (true)
            {
                bool skip = false;
                if (skipBarriers && (counter % 5 == 3))  skip = true;

                counter++;                

                // Calculate the attractive and repulsive forces between the
                // vertices.  The results get written to metadata on the vertices.

                CalculateRepulsiveForces(si, displacement, k, factor, skip);
                CalculateAttractiveForces(si, displacement, k);

                double fNextTemperature = fTemperature - fTemperatureDecrement;

                // Set the unbounded location of each vertex based on the vertex's
                // current location and the calculated forces.

                //check if fnexttemp > 0.......
                SetUnboundedLocations(si.nodes, displacement, fTemperature);

                // Decrease the temperature.

                fTemperature = fNextTemperature;

                // For graphs with many edges, telling NodeXLControl to refresh
                // its window (which is the end result of calling
                // FireLayOutGraphIterationCompleted()) can significantly slow down
                // performance, so don't do it.
                //
                // For example, a random graph with 1,000 vertices and 10,000 edges
                // took 3.6 seconds to lay out when the window was repeatedly
                // refreshed, but only 2.1 seconds with no refreshes.
                //
                // The performance improvement is much smaller for graphs with a
                // large number of vertices, where the O(V-squared) behavior in
                // CalculateRepulsiveForces() dominates the layout time.
                                

                if (counter >= iterstop || fTemperature <= 0d)
                {
                    double contentWidth = 2000;  //This corresonds to the 2000, 2000 used as width and height of GUI window
                    double contentHeight = 2000;
                    
                    double minX = double.MaxValue;
                    double maxX = double.MinValue;
                    double minY = double.MaxValue;
                    double maxY = double.MinValue;                    
                    foreach (NodeData xx in si.nodes)
                    {
                        if (xx.x < minX) minX = xx.x;
                        if (xx.y < minY) minY = xx.y;
                        if (xx.x > maxX) maxX = xx.x;
                        if (xx.y > maxY) maxY = xx.y;
                    }                                                           

                    minX += -contentWidth / 20d;
                    minY += -contentWidth / 20d;
                    maxX += contentWidth / 20d;
                    maxY += contentWidth / 20d;

                    double longest = Math.Max(maxX - minX, maxY - minY);
                    double scale = Math.Min(1d, contentWidth / longest);  //don't scale up!
                    if (contentWidth != contentHeight) throw new Exception();

                    si.width *= scale;
                    si.height *= scale;
                    
                    foreach (NodeData xx in si.nodes)
                    {
                        xx.x = (xx.x - minX) * scale;
                        xx.y = (xx.y - minY) * scale;
                    }

                    minX = double.MaxValue;
                    maxX = double.MinValue;
                    minY = double.MaxValue;
                    maxY = double.MinValue;
                    foreach (NodeData xx in si.nodes)
                    {
                        if (xx.x < minX) minX = xx.x;
                        if (xx.y < minY) minY = xx.y;
                        if (xx.x > maxX) maxX = xx.x;
                        if (xx.y > maxY) maxY = xx.y;
                    }
                    double midX = (minX + maxX) / 2d;
                    double midY = (minY + maxY) / 2d;
                    double addX = contentWidth / 2d - midX;
                    double addY = contentWidth / 2d - midY;
                    foreach (NodeData xx in si.nodes)
                    {
                        xx.x += addX;
                        xx.y += addY;                        
                    }
                    
                    int counter2 = -1;
                    foreach (NodeData xx in si.nodes)
                    {
                        counter2++;                 
                        Color c = Colors.LightGray;
                        if (xx.isExogenous) c = Colors.White;
                        if (xx.isStartNode) c = Colors.Tomato;
                        si.rectangles.Add(new RectangleData(xx.x - si.width / 2d, xx.y - si.height / 2d, si.width, si.height, " " + xx.name, xx.labelBig, c));
                    }

                    //si.rectangles.Add(new RectangleData(0, 0, si.width, si.height, "!!!", "", Colors.Blue));
                    //si.rectangles.Add(new RectangleData(contentWidth - si.width, contentHeight - si.height, si.width, si.height, "!!!", "", Colors.Blue));
                    //si.rectangles.Add(new RectangleData(contentWidth, contentHeight, si.width, si.height, "!!!", "", Colors.Red));
                    //si.arrows.Add(new ArrowData(100d, 100d, k * scale, 0d, Colors.Green, 5d));

                    foreach (EdgeData xx in si.edges)
                    {
                        if (xx.from == 19 && xx.to == 18) 
                            Console.WriteLine();
                        
                        double x1 = si.nodes[(int)xx.from].x;
                        double y1 = si.nodes[(int)xx.from].y;
                        double x2 = si.nodes[(int)xx.to].x;
                        double y2 = si.nodes[(int)xx.to].y;
                        double w = Math.Min(1d, Math.Abs(xx.weight));
                        //si.arrows.Add(new ArrowData(x1, y1, x2 - x1, y2 - y1, Colors.Red, xx.weight * 4d * scale));
                        double r = 255;
                        double g = 0;
                        double b = 0;
                        if (xx.weight > 0)
                        {
                            r = 0;
                            g = 255;
                            b = 0;
                        }
                        double w2 = Math.Max(w, 0.05d);
                        double a = Angle(x1, -y1, x2, -y2);                        
                        if (a < 0d || a > 360d) throw new Exception();
                        //TODO: do this as minimization of real angle between 
                        //arrows and boxes.
                        double off_x1 = 0d;
                        double off_y1 = 0d;
                        double off_x2 = 0d;
                        double off_y2 = 0d;
                        if (a < 45)
                        {
                            off_x1 = si.width / 2d;
                            off_x2 = -si.width / 2d;
                        }
                        else if (a < 135)
                        {
                            off_y1 = -si.height / 2d;
                            off_y2 = si.height / 2d;
                        }
                        else if (a < 225)
                        {
                            off_x1 = -si.width / 2d;
                            off_x2 = si.width / 2d;
                        }
                        else if (a < 315)
                        {
                            off_y1 = si.height / 2d;
                            off_y2 = -si.height / 2d;
                        }
                        else
                        {
                            off_x1 = si.width / 2d;
                            off_x2 = -si.width / 2d;
                        }
                        
                        si.arrows.Add(new ArrowData(x1 + off_x1, y1 + off_y1, x2 + off_x2 - (x1 + off_x1), y2 + off_y2 - (y1 + off_y1), Color.FromArgb(180, (byte)(r), (byte)(g), (byte)(b)), 8d * scale * w2));
                    }                    
                    break;
                }
            }
        }

        public static double Angle(double px1, double py1, double px2, double py2)
        {
            // Negate X and Y values
            double pxRes = px2 - px1;
            double pyRes = py2 - py1;
            double angle = 0.0;
            // Calculate the angle
            if (pxRes == 0.0)
            {
                if (pxRes == 0.0)
                    angle = 0.0;
                else if (pyRes > 0.0) angle = System.Math.PI / 2.0;
                else
                    angle = System.Math.PI * 3.0 / 2.0;
            }
            else if (pyRes == 0.0)
            {
                if (pxRes > 0.0)
                    angle = 0.0;
                else
                    angle = System.Math.PI;
            }
            else
            {
                if (pxRes < 0.0)
                    angle = System.Math.Atan(pyRes / pxRes) + System.Math.PI;
                else if (pyRes < 0.0) angle = System.Math.Atan(pyRes / pxRes) + (2 * System.Math.PI);
                else
                    angle = System.Math.Atan(pyRes / pxRes);
            }
            // Convert to degrees
            angle = angle * 180 / System.Math.PI; return angle;

        }

        public static  void  CalculateAttractiveForces(SolveInfo si, List<Displacement> displacement, double k)
        {
            List<EdgeData> edgesToLayOut = si.edges;
            List<NodeData> verticesToLayOut = si.nodes;

            foreach (EdgeData oEdge in edgesToLayOut)
            {
                if (oEdge.from == oEdge.to)
                {
                    // A vertex isn't attracted to itself.
                    continue;
                }

                // Get the edge's vertices.


                double xFrom = verticesToLayOut[oEdge.from].x;
                double yFrom = verticesToLayOut[oEdge.from].y;
                double xTo = verticesToLayOut[oEdge.to].x;
                double yTo = verticesToLayOut[oEdge.to].y;

                double tDeltaX =
                    xTo - xFrom;

                double tDeltaY =
                    yTo - yFrom;

                double tDelta = (double)Math.Sqrt(
                    (tDeltaX * tDeltaX) + (tDeltaY * tDeltaY)
                    );

                //displacement here: for nodes in exact same position???
                //displacement here: for nodes in exact same position???
                //displacement here: for nodes in exact same position???                

                // (Note that there is an obvious typo in the Fruchterman-Reingold
                // paper for computing the attractive force.  The function fa(z) at
                // the top of Figure 1 is defined as x squared over k.  It should
                // read z squared over k.)

                double fa = (tDelta * tDelta) / (double)k;

                if (si.useWeight)
                {
                    double w = Math.Pow(Math.Abs(oEdge.weight), 0.5d);  //easing the effect a little
                    if (w <= 1d)
                    {
                        fa = fa * w;  //may be negative flow, 3 compensates for avg. < 1 force
                    }
                    else fa = fa * 1d;  //may be negative flow, 3 compensates for avg. < 1 force
                    fa *= 3d;
                }

                if (tDelta == 0)
                {
                    // TODO: Is this the correct way to handle vertices in the same
                    // location?  See the notes in CalculateRepulsiveForces().                    
                    //throw new Exception();
                    displacement[oEdge.from].x += 1;
                    displacement[oEdge.from].y += 1;
                    displacement[oEdge.to].x -= 1;
                    displacement[oEdge.to].y -= 1;
                }
                else
                {

                    double faOverDelta = fa / tDelta;

                    double tFactorX = tDeltaX * faOverDelta;
                    double tFactorY = tDeltaY * faOverDelta;

                    displacement[oEdge.from].x += tFactorX;
                    displacement[oEdge.from].y += tFactorY;
                    displacement[oEdge.to].x -= tFactorX;
                    displacement[oEdge.to].y -= tFactorY;
                }
            }
        }

        public static void CalculateRepulsiveForces(SolveInfo si,  List<Displacement> displacement, double k, double factor, bool skipBarriers)
        {
            List<EdgeData> edgesToLayOut = si.edges;
            List<NodeData> verticesToLayOut = si.nodes;

            double tkSquared = (double)(k * k);

            int vCounter = -1;
            foreach (NodeData oVertexV in verticesToLayOut)
            {
                vCounter++;
                // Retrieve the object that stores calculated values for the
                // vertex.                

                double tDisplacementX = 0;
                double tDisplacementY = 0;

                foreach (NodeData oVertexU in verticesToLayOut)
                {
                    if (oVertexU == oVertexV)
                    {
                        continue;
                    }

                    double tDeltaX =
                        oVertexV.x - oVertexU.x;

                    double tDeltaY =
                        oVertexV.y - oVertexU.y;

                    double tDelta = (double)Math.Sqrt(
                        (tDeltaX * tDeltaX) + (tDeltaY * tDeltaY)
                        );

                    // The Fruchterman-Reingold paper says this about vertices in
                    // the same location:
                    //
                    // "A special case occurs when vertices are in the same
                    // position: our implementation acts as though the two vertices
                    // are a small distance apart in a randomly chosen orientation:
                    // this leads to a violent repulsive effect separating them."
                    //
                    // Handle this case by arbitrarily setting a small
                    // displacement.

                    if (tDelta == 0)
                    {
                        tDisplacementX += 1;
                        tDisplacementY += 1;
                    }
                    else
                    {
                        if (si.useSmartRepulse)
                        {
                            double fr = double.NaN;
                            double kk = 30d;  //proportional to #nodes???
                            fr = k * k / Math.Pow(tDelta, 1.0d) - k * k / Math.Pow(k, 1.0d) / kk;
                            double frOverDelta = fr / tDelta;
                            tDisplacementX += tDeltaX * frOverDelta * factor;
                            tDisplacementY += tDeltaY * frOverDelta * factor;
                        }
                        else
                        {
                            double fr = tkSquared / tDelta;
                            double frOverDelta = fr / tDelta;
                            tDisplacementX += tDeltaX * frOverDelta * factor;
                            tDisplacementY += tDeltaY * frOverDelta * factor;
                        }
                    }
                }

                // Save the results for VertexV.

                displacement[vCounter].x = tDisplacementX;
                displacement[vCounter].y = tDisplacementY;

                if (skipBarriers)
                {
                    displacement[vCounter].x = 0d;
                    displacement[vCounter].y = 0d;
                }

            }
        }


        public static void SetUnboundedLocations(List<NodeData> verticesToLayOut, List<Displacement> displacement, double fTemperature)
        {
            // The following variables define the unbounded rectangle.

            double tMinLocationX = double.MaxValue;
            double tMaxLocationX = double.MinValue;

            double tMinLocationY = double.MaxValue;
            double tMaxLocationY = double.MinValue;

            int counter = -1;
            foreach (NodeData oVertex in verticesToLayOut)
            {
                counter++;
                // Retrieve the object that stores calculated values for the
                // vertex.  We need the vertex's current unbounded location and
                // the displacement created by the repulsive and attractive forces
                // on the vertex.



                double tUnboundedLocationX = oVertex.x;
                double tUnboundedLocationY = oVertex.y;

                double tDisplacementX = displacement[counter].x;
                double tDisplacementY = displacement[counter].y;

                CalcDisplacement(fTemperature, ref tUnboundedLocationX, ref tUnboundedLocationY, tDisplacementX, tDisplacementY);

                // Update the vertex's unbounded location.

                oVertex.x = tUnboundedLocationX;
                oVertex.y = tUnboundedLocationY;

                // Expand the unbounded rectangle if necessary.

                tMinLocationX = Math.Min(tUnboundedLocationX, tMinLocationX);
                tMaxLocationX = Math.Max(tUnboundedLocationX, tMaxLocationX);

                tMinLocationY = Math.Min(tUnboundedLocationY, tMinLocationY);
                tMaxLocationY = Math.Max(tUnboundedLocationY, tMaxLocationY);
            }


        }

        private static void CalcDisplacement(double fTemperature, ref double tUnboundedLocationX, ref double tUnboundedLocationY, double tDisplacementX, double tDisplacementY)
        {
            double tDisplacement = (double)Math.Sqrt(
                (tDisplacementX * tDisplacementX) +
                (tDisplacementY * tDisplacementY)
                );

            if (tDisplacement != 0)
            {
                // Calculate a new unbounded location, limited by the current
                // temperature.

                tUnboundedLocationX += (tDisplacementX / tDisplacement) *
                    Math.Min(tDisplacement, (double)fTemperature);

                tUnboundedLocationY += (tDisplacementY / tDisplacement) *
                    Math.Min(tDisplacement, (double)fTemperature);
            }
        }


    }

    public class FRRectangle {
        public double Width;
        public double Height;
    }

    public class Displacement
    {
        public double x = 0;
        public double y = 0;
    }

    public class FlowNode
    {
        public string varName;
        public int counter;
        public string labelBig;
        public bool isExogenous;
        public bool isStartNode;
        public int id;
    }

    public class FlowArrow
    {
        public string varName1;
        public int counter1;
        public string varName2;
        public int counter2;
        public double weight;
    }




}
