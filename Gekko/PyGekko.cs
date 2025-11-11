using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;

namespace Gekko
{
    /// <summary>
    /// Used for Python.NET, calling C# (Gekko) directly from Python.
    /// </summary>
    public class PyGekko  //Cannot be static, because then it cannot be instantiated in Python.NET
    {
        public PyGekko()
        {
            PyGekko.PreparePython();
        }        
        
        /// <summary>
        /// Issue Gekko statement(s).
        /// </summary>
        /// <param name="s"></param>
        public static string Run(string s)
        {
            string output = null;
            try
            {
                Globals.gekkoOutputRecorder = new StringBuilder();
                Program.RunGekkoCommands(s, "", 0, new P());
                if (Globals.gekkoOutputRecorder != null) output = Globals.gekkoOutputRecorder.ToString();
            }
            finally
            {
                Globals.gekkoOutputRecorder = null;
            }
            return output;
        }

        /// <summary>
        /// Run Gekko command file (.gcm)
        /// </summary>
        /// <param name="s"></param>
        public static void RunFile(string s)
        {
            Program.RunGekkoCommands("", s, 0, new P());
        }

        public static void Decomp(string name, string eq = null, int[] t = null, string op = null)
        {
            string xeq = "null";
            string xt = "null";
            string xop = "null";
            if (eq != null) xeq = eq;
            if (t != null) xt = t[0] + " " + t[1];
            if (op != null) xop = op;
            new Writeln("Gekko received: name=" + name + " -- eq=" + xeq + " -- t=" + xt + " -- op=" + xop);
        }        

        /// <summary>
        /// Helper method (hook) for Python. BEWARE: Do not change name or signature without changing in 
        /// Python files (that call Gekko), too!! Ok that it is not referenced: is used from Python.
        /// </summary>        
        public static void PreparePython()
        {
            //See similar code used in in GuiStuff(), see: #09785932405            
            Globals.batchType = EBatchType.PyGekko;            
            Program.SetupGekkoForNonGuiUse();
        }

        public static void Wait()
        {
            System.Threading.Thread.Sleep(int.MaxValue);
        }

        public static void Stdout(bool b)
        {
            Globals.pyGekkoStdout = b;
        }
    }
}
