using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using GAMS;

namespace Gekko
{
    class CGE
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

        public static void Run()
        {            
            
            StreamWriter sw1 = new StreamWriter(@"c:\Thomas\InterACT\CES\CES\ces.frm");
            StreamWriter sw2 = new StreamWriter(@"c:\Thomas\InterACT\CES\CES\ces.gcm");

            int nGoods = 10;

            string sigmaExport = "-0.30";  //must be negative!!            
            string sigmaImport = "0.30";

            string alpha = "0.15";  //share
            string beta = "0.15";  //share


            sw = sw2;
            Writeln("reset; cls;");
            Writeln("option print fields pdec 4;");

            Writeln("model ces;");
            Writeln("create #all;");
            Writeln("time 2000 2000;");

            //======= household demand ==========                 
            //======= household demand ==========                 
            //======= household demand ==========                 

            if (true)
            {

                sw = sw1;

                prices = new List<string>();
                volumes = new List<string>();
                sigmas = new List<string>();

                activityString = "u";
                priceString = "pc";
                volumeString = "c";
                costString = "costc";
                ccCode = null;

                CesNode n1 = new CesNode("pc1", "c1");
                CesNode n2 = new CesNode("pc2", "c2");
                CesNode n3 = new CesNode("pc3", "c3");
                CesNode n4 = new CesNode("pc4", "c4");
                CesNode n5 = new CesNode("pc5", "c5");
                CesNode n6 = new CesNode("pc6", "c6");
                CesNode n7 = new CesNode("pc7", "c7");
                CesNode n8 = new CesNode("pc8", "c8");
                CesNode n9 = new CesNode("pc9", "c9");
                CesNode n10 = new CesNode("pc10", "c10");
                CesNode n_agg1 = new CesNode(n1, n2);
                CesNode n_agg2 = new CesNode(n_agg1, n3);
                CesNode n_agg3 = new CesNode(n4, n5);
                CesNode n_agg4 = new CesNode(n_agg2, n_agg3);
                CesNode n_agg5 = new CesNode(n6, n7);
                CesNode n_agg6 = new CesNode(n_agg4, n_agg5);
                CesNode n_agg7 = new CesNode(n8, n9);
                CesNode n_agg8 = new CesNode(n_agg6, n_agg7);
                CesNode n_agg9 = new CesNode(n_agg8, n10);
                CesNode n = n_agg9;
                idCounter = 0;
                Walk(n);
                Walk1(n);
                Walk2(n);
                ccCode = ccCode.Substring(0, ccCode.Length - 3);
                Writeln("frml _i   " + costString + " = " + ccCode + ";");
                Writeln("frml _i   pu = " + costString + " / u;");
                Writeln("");

                // ----------------------------------

                sw = sw2;
                Writeln("series " + activityString + " = 1000; series " + oString + activityString + " = 1000;");
                foreach (string s in volumes) Writeln("series " + s + " = 100; series " + oString + s + " = 100;");
                foreach (string s in prices) Writeln("series " + s + " = 1; series " + oString + s + " = 1;");
                foreach (string s in sigmas) Writeln("series " + s + " = 0.5;");
                Writeln("series sigmac1 = 1.5;");
                Writeln("series sigmac2 = 2.5;");
                Writeln("series sigmac3 = 0.5;");
                Writeln("series sigmac4 = 0.5;");
                Writeln("series sigmac5 = 0.5;");
                Writeln("series sigmac6 = 0.5;");
                Writeln("series sigmac7 = 0.5;");
                Writeln("series sigmac8 = 0.5;");
                Writeln("series sigmac9 = 0.5;");
                
            }

            //if (true)
            //{

            //    sw = sw1;

            //    prices = new List<string>();
            //    volumes = new List<string>();
            //    sigmas = new List<string>();

            //    activityString = "u";
            //    priceString = "pc";
            //    volumeString = "c";
            //    costString = "costc";
            //    ccCode = null;

            //    CesNode n1 = new CesNode("pc1", "c1");
            //    CesNode n2 = new CesNode("pc2", "c2");
            //    CesNode n3 = new CesNode("pc3", "c3");                
            //    CesNode n_agg1 = new CesNode(n1, n2);
            //    CesNode n_agg2 = new CesNode(n_agg1, n3);;
            //    CesNode n = n_agg2;
            //    idCounter = 0;
            //    Walk(n);
            //    Walk1(n);
            //    Walk2(n);
            //    ccCode = ccCode.Substring(0, ccCode.Length - 3);
            //    Writeln("frml _i   " + costString + " = " + ccCode + ";");
            //    Writeln("frml _i   pu = " + costString + " / u;");
            //    Writeln("");

            //    // ----------------------------------

            //    sw = sw2;
            //    Writeln("series " + activityString + " = 1000; series " + oString + activityString + " = 1000;");
            //    foreach (string s in volumes) Writeln("series " + s + " = 100; series " + oString + s + " = 100;");
            //    foreach (string s in prices) Writeln("series " + s + " = 1; series " + oString + s + " = 1;");
            //    foreach (string s in sigmas) Writeln("series " + s + " = 0.5;");
            //    Writeln("series sigmac1 = 1.5;");
            //    Writeln("series sigmac2 = 2.5;");
            //    Writeln("series sigmac3 = 0.5;");
            //    Writeln("series sigmac4 = 0.5;");
            //    Writeln("series sigmac5 = 0.5;");
            //    Writeln("series sigmac6 = 0.5;");
            //    Writeln("series sigmac7 = 0.5;");
            //    Writeln("series sigmac8 = 0.5;");
            //    Writeln("series sigmac9 = 0.5;");

            //}

            //======= production sectors ==========                 
            //======= production sectors ==========                 
            //======= production sectors ==========   

            for (int i = 1; i <= nGoods; i++)
            {

                sw = sw1;

                prices = new List<string>();
                volumes = new List<string>();
                sigmas = new List<string>();

                activityString = "y" + i;
                priceString = "py" + i;
                volumeString = "y" + i;
                costString = "costy" + i;
                ccCode = null;

                CesNode n1 = new CesNode("pk" + i, "k" + i, true);
                Writeln("frml _i   pk" + i + " = pk;");
                CesNode n2 = new CesNode("pl" + i, "l" + i, true);
                Writeln("frml _i   pl" + i + " = pl;");
                CesNode n3 = new CesNode("pve" + i, "ve" + i, true);
                Writeln("frml _i   pve" + i + " = pve;");
                CesNode n4 = new CesNode("pvm" + i, "vm" + i, true);
                Writeln("frml _i   pvm" + i + " = pvm;");
                CesNode n_agg1 = new CesNode(n1, n2);
                CesNode n_agg2 = new CesNode(n_agg1, n3);
                CesNode n_agg3 = new CesNode(n_agg2, n4);
                CesNode n = n_agg3;
                idCounter = 0;
                Walk(n);
                Walk1(n);
                Walk2(n);
                ccCode = ccCode.Substring(0, ccCode.Length - 3);
                Writeln("frml _i   " + costString + " = " + ccCode + ";");
                Writeln("");

                // ----------------------------------

                sw = sw2;
                Writeln("series " + activityString + " = 100; series " + oString + activityString + " = 100;");
                foreach (string s in volumes) Writeln("series " + s + " = 25; series " + oString + s + " = 25;");
                foreach (string s in prices) Writeln("series " + s + " = 1; series " + oString + s + " = 1;");
                foreach (string s in sigmas) Writeln("series " + s + " = 0.5;");
                Writeln("series eff_pk" + i + " = 1;");
                Writeln("series eff_pl" + i + " = 1;");
                Writeln("series eff_pve" + i + " = 1;");
                Writeln("series eff_pvm" + i + " = 1;");


            }

            //======= import ==========                 
            //======= import ==========                 
            //======= import ==========                                         

            for (int i = 1; i <= nGoods; i++)
            {

                sw = sw1;

                prices = new List<string>();
                volumes = new List<string>();
                sigmas = new List<string>();

                activityString = "c" + i;
                priceString = "pc" + i;
                volumeString = "extra_c" + i;
                costString = "costc" + i;
                ccCode = null;

                CesNode n1 = new CesNode("pd" + i, "d" + i);
                CesNode n2 = new CesNode("pfx", "m" + i);
                CesNode n_agg1 = new CesNode(n1, n2);
                CesNode n = n_agg1;
                idCounter = 0;
                Walk(n);
                Walk1(n);
                Walk2(n);
                ccCode = ccCode.Substring(0, ccCode.Length - 3);
                Writeln("frml _i   " + costString + " = " + ccCode + ";");
                Writeln("");

                // ----------------------------------

                sw = sw2;
                Writeln("series " + activityString + " = 100; series " + oString + activityString + " = 100;");
                foreach (string s in volumes)
                {
                    string ss = beta;
                    if (s.StartsWith("d")) ss = "(1-" + beta + ")";
                    Writeln("series " + s + " = " + beta + "; series " + oString + s + " = " + ss + " * 100;");
                }
                foreach (string s in prices) Writeln("series " + s + " = 1; series " + oString + s + " = 1;");
                foreach (string s in sigmas) Writeln("series " + s + " = " + sigmaImport + ";");

            }

            //======= export ==========                 
            //======= export ==========                 
            //======= export ==========                                        

            for (int i = 1; i <= nGoods; i++)
            {

                sw = sw1;

                prices = new List<string>();
                volumes = new List<string>();
                sigmas = new List<string>();

                activityString = "y" + i;
                priceString = "extra_py" + i;
                volumeString = "extra_y" + i;
                costString = "extra_costy" + i;
                ccCode = null;

                CesNode n1 = new CesNode("pd" + i, "d" + i);
                CesNode n2 = new CesNode("pfx", "e" + i);
                CesNode n_agg1 = new CesNode(n1, n2);
                CesNode n = n_agg1;
                idCounter = 0;
                Walk(n);
                Walk1(n);
                Walk2(n);
                ccCode = ccCode.Substring(0, ccCode.Length - 3);
                Writeln("frml _i   " + costString + " = " + ccCode + ";");
                Writeln("");

                // ----------------------------------

                sw = sw2;
                Writeln("series " + activityString + " = 100; series " + oString + activityString + " = 100;");
                foreach (string s in volumes)
                {
                    string ss = alpha;
                    if (s.StartsWith("d")) ss = "(1-" + alpha + ")";
                    Writeln("series " + s + " = " + alpha + "; series " + oString + s + " = " + ss + " * 100;");
                }
                foreach (string s in prices) Writeln("series " + s + " = 1; series " + oString + s + " = 1;");
                foreach (string s in sigmas) Writeln("series " + s + " = " + sigmaExport + ";");

            }

            //======= finishing ==========                 
            //======= finishing ==========                 
            //======= finishing ==========                 

            sw = sw1;
            string sumK = null;
            string sumL = null;
            string sumVE = null;
            string sumVM = null;
            string sumE = null;
            string sumM = null;

            for (int i = 1; i <= nGoods; i++)
            {
                Writeln("frml _i   py" + i + " = costy" + i + "/y" + i + ";");
                Writeln("frml _i   pd" + i + " = pd" + i + " + py" + i + " - (extra_costy" + i + "/y" + i + ");");
                Writeln("frml _i   pc" + i + " = costc" + i + "/c" + i + ";");
                //Writeln("frml _i   y" + i + " = c" + i + ";");
                sumK += "k" + i + " + ";
                sumL += "l" + i + " + ";
                sumVE += "ve" + i + " + ";
                sumVM += "vm" + i + " + ";
                sumE += "e" + i + " + ";
                sumM += "m" + i + " + ";

            }
            Writeln("frml _i   k = " + sumK.Substring(0, sumK.Length - 3) + ";");
            Writeln("frml _i   l = " + sumL.Substring(0, sumL.Length - 3) + ";");  //leisure c11 can be added here
            Writeln("frml _i   ve = " + sumVE.Substring(0, sumVE.Length - 3) + ";");
            Writeln("frml _i   vm = " + sumVM.Substring(0, sumVM.Length - 3) + ";");
            Writeln("frml _i   e = " + sumE.Substring(0, sumE.Length - 3) + ";");
            Writeln("frml _i   m = " + sumM.Substring(0, sumM.Length - 3) + ";");

            Writeln("frml _i   pfx = pfx + e - m;");

            sw = sw2;
            Writeln("series k = 250; series pk = 1;");
            Writeln("series l = 250; series pl = 1;");
            Writeln("series ve = 250; series pve = 1;");
            Writeln("series vm = 250; series pvm = 1;");

            Writeln("endo pk, u, pve, pvm;");
            Writeln("exo k, l, ve, vm;");

            Writeln("sim<fix>;");
            Writeln("write data0;");
            Writeln("clone;");            

            Writeln("ser eff_pk1 * 1.01;");
            Writeln("ser eff_pl1 * 1.01;");
            Writeln("ser eff_pve1 * 1.01;");
            Writeln("ser eff_pvm1 * 1.01;");

            Writeln("sim<fix>;");
            Writeln("write data1;");

            Writeln("prt<q>py1, pd1, pc1, pfx;");
            Writeln("prt<q>py2, pd2, pc2, pfx;");

            Writeln("prt<q>y1, d1, c1, m1, e1;");
            Writeln("prt<q>y2, d2, c2, m2, e2;");

            Writeln("prt<q>k1, l1, ve1, vm1;");
            Writeln("prt<q>k2, l2, ve2, vm2;");

            Writeln("tell'';");
            Writeln("tell'y = e + d,   c = d + m   --> c = y + m - e';");
            Writeln("tell'';");
            Writeln("tell'py1 * y1 = pk1 * k1 + pl1 * l1 + pve1 * ve1 + pvm1 * vm1';");
            Writeln("tell'py1 * y1 = pd1 * d1 + pfx * e1    ==> vha pd1';");
            Writeln("tell'pc1 * c1 = pd1 * d1 + pfx * m1';");

            Writeln("tell'                    U';");
            Writeln("tell'                   /';");
            Writeln("tell'                  /';");
            Writeln("tell'                 C';");
            Writeln("tell'                /\\';");
            Writeln("tell'               /  \\';");
            Writeln("tell'              M    D    E';");
            Writeln("tell'                    \\  /';");
            Writeln("tell'                     \\/';");
            Writeln("tell'                     Y';");
            Writeln("tell'                    / \\';");
            Writeln("tell'                   /   \\';");
            Writeln("tell'                  K     L ...';");

            Writeln("prt<q dec=8 width=20>u;");            

            sw1.Flush(); sw1.Close();
            sw2.Flush(); sw2.Close();
        }
        
        static void Walk(CesNode node)
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

                node.pCode = "frml _i   " + node.priceId + " = " + "CES_UC(" + node.leftChild.priceId + node.leftChild.eff + "/" + oLeft + ", " + node.rightChild.priceId + node.rightChild.eff + "/" + oRight + ", " + costNum + " / " + costDen + ", " + node.sigma + ");";
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
                string s = "CES_XR";
                if (node.isLeftNode) s = "CES_XL";
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
                    //qq1 = "/" + node.volumeId.Replace("d", "eff");  //HACK HACK
                    //if (!node.parent.leftChild.priceId.Contains(aggString)) qq2 = qq1;
                    //if (!node.parent.rightChild.priceId.Contains(aggString)) qq3 = qq1;
                }

                string right = zz + node.eff + " * " + s + "(" + ww + "/" + yy + " , " + node.parent.leftChild.priceId + node.parent.leftChild.eff + "/" + oLeft + qq2 + " , " + node.parent.rightChild.priceId + node.parent.rightChild.eff + "/ " + oRight + qq3 + " , " + xx + " / " + yy2 + ", " + node.parent.sigma + ");";
                if (node.volumeId.StartsWith("d") && ww.StartsWith("y"))
                {
                    string uuu = node.volumeId.Replace("d", "y");  //HACK HACK
                    node.xCode = "frml _i   " + uuu + "=" + uuu + "+" + node.volumeId + " - " + right;
                }
                else
                    node.xCode = "frml _i   " + node.volumeId + " = " + right;
            }
        }

        private static void PHelper(CesNode node, out string oLeft, out string oRight)
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

        private static string GetCosts(CesNode node)
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

        static void Walk1(CesNode node)
        {
            if (!node.IsLeaf())  //could just as well test .rightChild == null
            {
                //Not a leaf node
                Walk1(node.leftChild);
                Walk1(node.rightChild);
            }
            if (node.pCode != null) Writeln(node.pCode);
        }

        static void Walk2(CesNode node)
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

    public class CesNode
    {
        public CesNode parent = null;
        public CesNode leftChild = null;
        public CesNode rightChild = null;
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
            CGE.prices.Add(priceId);
            CGE.volumes.Add(volumeId);
        }

        public CesNode(string priceId, string volumeId)
        {
            Initialize(priceId, volumeId, false);
        }

        public CesNode(string priceId, string volumeId, bool eff)
        {
            Initialize(priceId, volumeId, eff);
        }

        public CesNode(CesNode left, CesNode right)
        {
            CGE.idCounter++;
            this.sigma = "sigma" + CGE.volumeString + CGE.idCounter;
            CGE.sigmas.Add(this.sigma);
            this.nodeCounter = CGE.idCounter;
            this.priceId = CGE.priceString + "_" + CGE.aggString + this.nodeCounter.ToString();
            this.volumeId = CGE.volumeString + "_" + CGE.aggString + this.nodeCounter.ToString();

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
