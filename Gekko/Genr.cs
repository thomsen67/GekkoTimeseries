using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Gekko.Parser;
namespace Gekko
{
    public class TranslatedCode
    {
        public static GekkoTime globalGekkoTimeIterator = Globals.tNull;
        public static void FunctionDef3()
        {


            //[[splitSTOP]]
                        
            Globals.ufunctions1.Add("f", (GekkoSmpl smpl, IVariable i1) => { G.Writeln2("HEJSAN"); return null; });


            //[[splitSTART]]

        }

        public static void ClearTS(P p)
        {
        }
        public static void ClearScalar(P p)
        {
        }
        public static void C0(P p)
        {

            GekkoSmpl smpl = O.Smpl();


            p.SetText(@"¤1");
            FunctionDef3();

            IVariable z1 = Globals.ufunctions1["f"](null, new ScalarVal(10));
            
        }


        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = O.Smpl();

            C0(p);



        }
    }
}
