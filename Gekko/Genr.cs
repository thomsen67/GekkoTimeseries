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
        public static GekkoTime globalGekkoTimeIterator = GekkoTime.tNull;
        public static int labelCounter;
        public static void C0(GekkoSmpl smpl, P p)
        {
            //[[commandStart]]0
            p.SetText(@"¤1"); O.InitSmpl(smpl, p);

            var Evalcode10 = new List<Func<GekkoSmpl, IVariable>>(); 
            foreach (IVariable listloop_s9 in new O.GekkoListIterator(O.DecompLooper("#s")))
            {
                Evalcode10.Add((smpl11) => { return O.Subtract(smpl11, O.Indexer(O.Indexer2(smpl11, O.EIndexerType.None, listloop_s9), smpl11, O.EIndexerType.None, O.Lookup(smpl11, null, null, "vY", null, null, new LookupSettings(), EVariableType.Var, null), listloop_s9), O.Multiply(smpl11, O.Indexer(O.Indexer2(smpl11, O.EIndexerType.None, listloop_s9), smpl11, O.EIndexerType.None, O.Lookup(smpl11, null, null, "pY", null, null, new LookupSettings(), EVariableType.Var, null), listloop_s9), O.Indexer(O.Indexer2(smpl11, O.EIndexerType.None, listloop_s9), smpl11, O.EIndexerType.None, O.Lookup(smpl11, null, null, "qY", null, null, new LookupSettings(), EVariableType.Var, null), listloop_s9))); });
            }
            Globals.expressionText = @"vY[#s] - (pY[#s] * qY[#s])";
            Globals.expressions = Evalcode10;

            //[[commandEnd]]0
        }



        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}

