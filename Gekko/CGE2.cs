using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using GAMS;

namespace Gekko
{
    class CGE2
    {
        public static string ccCode = null;
        public static string oString = "o";
        public static int idCounter = 0;
        public static StreamWriter sw = null;
        public static string aggString = "agg";
        public static string priceString = "???";
        public static string volumeString = "???";
        public static string costString = "???";
        public static string activityString = "???";
        public static List<string> prices;
        public static List<string> volumes;
        public static List<string> sigmas;
        public static int counterh = 0;
        public static int countere = 0;

        public static bool useFunctions = true;

        public static void Run()
        {

            /*
            
            public static double CES_UC(double p1rel, double p2rel, double theta, double sigma)
        {
            double c = Math.Pow(theta * Math.Pow(p1rel, 1 - sigma) + (1 - theta) * Math.Pow(p2rel, 1 - sigma), 1 / (1 - sigma));
            return c;
        }

        public static double CES_XL(double yrel, double p1rel, double p2rel, double theta, double sigma)
        {
            double uc = CES_UC(p1rel, p2rel, theta, sigma);
            return yrel * Math.Pow(uc / p1rel, sigma);
        }

        public static double CES_XR(double yrel, double p1rel, double p2rel, double theta, double sigma)
        {
            double uc = CES_UC(p1rel, p2rel, theta, sigma);
            return yrel * Math.Pow(uc / p2rel, sigma);
        }

           */

            StreamWriter sw1 = new StreamWriter(@"c:\Thomas\InterACT\estimation_forbrug\forkromet\ces_frml.tsp");

            //======= household demand ==========
            //======= household demand ==========
            //======= household demand ==========

            useFunctions = false;

            sw = sw1;

            prices = new List<string>();
            volumes = new List<string>();
            //sigmas = new List<string>();

            activityString = "u";
            priceString = "pc";
            volumeString = "c";
            costString = "costc";
            ccCode = null;

            CNode n1 = new CNode("pc1", "c1");
            CNode n2 = new CNode("pc2", "c2");
            CNode n3 = new CNode("pc3", "c3");
            CNode n4 = new CNode("pc4", "c4");
            CNode n5 = new CNode("pc5", "c5");
            CNode n6 = new CNode("pc6", "c6");
            CNode n7 = new CNode("pc7", "c7");
            //Careful with the sigma names, that they are distinct. Here they follow n_agg number.
            CNode n_agg1 = new CNode(n1, n2, "sigmac1");
            CNode n_agg2 = new CNode(n_agg1, n3, "sigmac2");
            CNode n_agg4 = new CNode(n4, n5, "sigmac4");
            CNode n_agg3 = new CNode(n_agg2, n_agg4, "sigmac3");
            CNode n_agg6 = new CNode(n6, n7, "sigmac6");
            CNode n_agg5 = new CNode(n_agg3, n_agg6, "sigmac5");            
            CNode n = n_agg5;  //BEWARE: this must be the node at the top 

            idCounter = 0;
            Walk(n);
            Walk1(n);
            Walk2(n);
            ccCode = ccCode.Substring(0, ccCode.Length - 3);

            Writeln("dot 1-" + (2 * counterh) + ";");
            for (int i = 1; i <= countere; i++) {
                Writeln("  eqsub e" + i + " h1-h" + counterh + ";");
            }
            Writeln("enddot;");

            Writeln("dot 1-" + countere + ";");
            Writeln("  frml g. pc. = pc./eff.;");            
            Writeln("enddot;");
            Writeln("dot 1-" + countere + ";");            
            Writeln("  eqsub e. g1-g" + countere + ";");
            Writeln("enddot;");

            // ----------------------------------

            sw1.Flush(); sw1.Close();
            
        }

        static void Walk(CNode node)
        {


            if (!node.IsLeaf())  //could just as well test .rightChild == null
            {
                //Not a leaf node
                Walk(node.leftChild);
                Walk(node.rightChild);

                string costNum = GetCosts(node.leftChild);
                string costDen = GetCosts(node);

                string oLeft;
                string oRight;
                PHelper(node, out oLeft, out oRight);

                //public static double CES_UC(double p1rel, double p2rel, double theta, double sigma)                
                string p1rel = node.leftChild.priceId + node.leftChild.eff + "/" + oLeft;
                string p2rel = node.rightChild.priceId + node.rightChild.eff + "/" + oRight;
                string theta = costNum + " / " + costDen;
                string sigma = node.sigma;

                counterh++;
                if (useFunctions)
                {
                    node.pCode = "frml h" + counterh + " " + node.priceId + " = " + "CES_UC(" + p1rel + ", " + theta + ", " + sigma + ");";
                }
                else
                {
                    node.pCode = "frml h" + counterh + " " + node.priceId + " = " + Generate_CES_UC(p1rel, p2rel, theta, sigma) + ";";
                }
            }
            else
            {
                //Leaf node
                ccCode += node.priceId + " * " + node.volumeId + " + ";
            }

            if (node.parent == null)
            {
            }
            else
            {
                string oLeft;
                string oRight;
                PHelper(node.parent, out oLeft, out oRight);
                string leftOrRight = "CES_XR";
                if (node.isLeftNode) leftOrRight = "CES_XL";
                string xx = GetCosts(node.parent.leftChild);
                string zz = GetCosts(node);
                string yy = GetCosts(node.parent);
                if (node.IsLeaf()) zz = "o" + node.volumeId;
                string ww = node.parent.volumeId;
                string yy2 = yy;
                string qq1 = null;
                string qq2 = null;
                string qq3 = null;
                if (node.parent.parent == null)
                {
                    ww = activityString;
                    yy = oString + activityString;
                }

                string yrel = ww + "/" + yy;
                string p1rel = node.parent.leftChild.priceId + node.parent.leftChild.eff + "/" + oLeft + qq2;
                string p2rel = node.parent.rightChild.priceId + node.parent.rightChild.eff + "/ " + oRight + qq3;
                string theta = xx + " / " + yy2;
                string sigma = node.parent.sigma;

                string right = null;
                if (useFunctions)
                {
                    right = "(" + zz + node.eff + ") * " + leftOrRight + "(" + yrel + " , " + p1rel + " , " + p2rel + " , " + theta + ", " + sigma + ")";
                }
                else
                {
                    string ownPrice = p1rel;
                    if (!node.isLeftNode) ownPrice = p2rel;
                    right = "(" + zz + node.eff + ") * " + "(" + yrel + ") * ((" + Generate_CES_UC(p1rel, p2rel, theta, sigma) + ")/(" + ownPrice + "))**(" + sigma + ")";
                }

                string i = null;
                string eff = null;
                if (node.volumeId.Contains("_"))
                {
                    counterh++;
                    i = "h" + counterh;
                }
                else
                {
                    countere++;
                    i = "e" + countere;
                    eff = " eff" + countere + " * ";
                }

                if (node.volumeId.StartsWith("d") && ww.StartsWith("y"))
                {
                    throw new Exception();
                }
                else
                {
                    if (node.volumeId.Contains("_"))
                    {
                        node.xCode = "frml " + i + " " + node.volumeId + "" + " = " + eff + right + ";";
                    }
                    else
                    {
                        node.xCode = "frml " + i + " l" + node.volumeId + "w" + " = log(" + eff + right + " + gamma" + countere + ");";
                    }                        
                }
            }
        }

        private static string Generate_CES_UC(string p1rel, string p2rel, string theta, string sigma)
        {
            return "((" + theta + ") * (" + p1rel + ")**( 1 - " + sigma + ") + (1 - (" + theta + ")) * (" + p2rel + ")**( 1 - " + sigma + "))**( 1 / (1 - " + sigma + "))";            
        }

        private static void PHelper(CNode node, out string oLeft, out string oRight)
        {
            if (node.IsLeaf())
            {
                oLeft = oString + node.parent.leftChild.priceId;
                oRight = oString + node.parent.rightChild.priceId;
                return;
            }

            if (node.leftChild.IsLeaf()) oLeft = oString + node.leftChild.priceId;  //not a leaf node
            else oLeft = "1.0";

            if (node.rightChild.IsLeaf()) oRight = oString + node.rightChild.priceId;  //not a leaf node
            else oRight = "1.0";

        }

        private static string GetCosts(CNode node)
        {
            string ss = null;
            if (node.IsLeaf())
            {
                ss += "(o" + node.priceId + " * o" + node.volumeId + ")";
            }
            else
            {
                for (int i = 0; i < node.childrenPriceId.Count; i++)
                {
                    ss += "o" + node.childrenPriceId[i] + " * o" + node.childrenVolumeId[i] + " + ";
                }
                ss = ss.Substring(0, ss.Length - 3);
                ss = "(" + ss + ")";
            }
            return ss;
        }

        static void Walk1(CNode node)
        {
            if (!node.IsLeaf())  //could just as well test .rightChild == null
            {
                //Not a leaf node
                Walk1(node.leftChild);
                Walk1(node.rightChild);
            }
            if (node.pCode != null) Writeln(node.pCode);
        }

        static void Walk2(CNode node)
        {
            if (node.xCode != null) Writeln(node.xCode);
            if (!node.IsLeaf())  //could just as well test .rightChild == null
            {
                //Not a leaf node
                Walk2(node.leftChild);
                Walk2(node.rightChild);
            }
        }



        static double CES_XL(double yrel, double p1rel, double p2rel, double theta, double sigma)
        {
            double uc = CES_UC(p1rel, p2rel, theta, sigma);
            return yrel * Math.Pow(uc / p1rel, sigma);
        }

        static double CES_XR(double yrel, double p1rel, double p2rel, double theta, double sigma)
        {
            double uc = CES_UC(p1rel, p2rel, theta, sigma);
            return yrel * Math.Pow(uc / p2rel, sigma);
        }

        static double C(double ca, double p1, double p2, double p1a, double p2a, double theta, double sigma)
        {
            double c = ca * CES_UC(p1 / p1a, p2 / p2a, theta, sigma);
            return c;
        }

        static double CES_UC(double p1rel, double p2rel, double theta, double sigma)
        {
            double c = Math.Pow(theta * Math.Pow(p1rel, 1 - sigma) + (1 - theta) * Math.Pow(p2rel, 1 - sigma), 1 / (1 - sigma));
            return c;
        }

        static double Y(double x1, double x2, double ya, double x1a, double x2a, double theta, double sigma)
        {
            double rho = (sigma - 1) / sigma;
            double y_cal = ya * Math.Pow((theta * Math.Pow(x1 / x1a, rho) + (1 - theta) * Math.Pow(x2 / x2a, rho)), 1 / rho);
            return y_cal;
        }

        // =============================================
        // =============================================
        // =============================================

        static double Y_orig(double x1, double x2, double kappa, double delta, double sigma)
        {
            double rho = (sigma - 1) / sigma;
            double y = kappa * Math.Pow(delta * Math.Pow(x1, rho) + (1 - delta) * Math.Pow(x2, rho), 1 / rho);
            return y;
        }

        static double X1_orig(double y, double p1, double p2, double kappa, double delta, double sigma)
        {
            double x1 = y / kappa * Math.Pow((delta * kappa * AC_orig(p1, p2, kappa, delta, sigma)) / p1, sigma);
            return x1;
        }

        static double X2_orig(double y, double p1, double p2, double kappa, double delta, double sigma)
        {
            double x2 = y / kappa * Math.Pow(((1 - delta) * kappa * AC_orig(p1, p2, kappa, delta, sigma)) / p2, sigma);
            return x2;
        }

        static double C_orig(double y, double p1, double p2, double kappa, double delta, double sigma)
        {
            double c = y * AC_orig(p1, p2, kappa, delta, sigma);
            return c;
        }

        static double AC_orig(double p1, double p2, double kappa, double delta, double sigma)
        {
            double ac = 1 / kappa * Math.Pow(Math.Pow(delta, sigma) * Math.Pow(p1, 1 - sigma) + Math.Pow(1 - delta, sigma) * Math.Pow(p2, 1 - sigma), 1 / (1 - sigma));
            return ac;
        }

        static void Writeln(string s)
        {
            System.Diagnostics.Debug.WriteLine(s);
            sw.WriteLine(s);
        }
    }

    public class CNode
    {
        public CNode parent = null;
        public CNode leftChild = null;
        public CNode rightChild = null;
        public bool isLeftNode = false;
        public string xCode = null;
        public string pCode = null;
        public string priceId = "???";
        public string volumeId = "???";
        public string eff = null;
        public int nodeCounter = 0;
        public string sigma = "???";
        public List<string> childrenPriceId = null;
        public List<string> childrenVolumeId = null;

        public void Initialize(string priceId, string volumeId, bool eff)
        {
            this.volumeId = volumeId;
            if (eff)
            {
                this.eff = "/eff_" + priceId;
            }
            this.priceId = priceId;
            CGE2.prices.Add(priceId);
            CGE2.volumes.Add(volumeId);
        }

        public CNode(string priceId, string volumeId)
        {
            Initialize(priceId, volumeId, false);
        }

        public CNode(string priceId, string volumeId, bool eff)
        {
            Initialize(priceId, volumeId, eff);
        }

        public CNode(CNode left, CNode right, string sigmaName)
        {
            CGE2.idCounter++;
            this.sigma = sigmaName;
            //CGE2.sigmas.Add(this.sigma);
            this.nodeCounter = CGE2.idCounter;
            this.priceId = CGE2.priceString + "_" + CGE2.aggString + this.nodeCounter.ToString();
            this.volumeId = CGE2.volumeString + "_" + CGE2.aggString + this.nodeCounter.ToString();

            this.childrenPriceId = new List<string>();
            if (left.childrenPriceId == null) this.childrenPriceId.Add(left.priceId);
            else this.childrenPriceId.AddRange(left.childrenPriceId);
            if (right.childrenPriceId == null) this.childrenPriceId.Add(right.priceId);
            else this.childrenPriceId.AddRange(right.childrenPriceId);

            this.childrenVolumeId = new List<string>();
            if (left.childrenVolumeId == null) this.childrenVolumeId.Add(left.volumeId);
            else this.childrenVolumeId.AddRange(left.childrenVolumeId);
            if (right.childrenVolumeId == null) this.childrenVolumeId.Add(right.volumeId);
            else this.childrenVolumeId.AddRange(right.childrenVolumeId);

            this.leftChild = left;
            left.parent = this;
            this.rightChild = right;
            right.parent = this;
            this.leftChild.isLeftNode = true;
            this.rightChild.isLeftNode = false;
        }
        public void Error(string s)
        {
            System.Diagnostics.Debug.WriteLine(s);
            throw new Exception();
        }
        public bool IsLeaf()
        {
            if (this.leftChild == null) return true;
            else return false;
        }
    }
}
