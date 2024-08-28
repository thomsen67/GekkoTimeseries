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
            p.SetStack(@"¤1"); O.InitSmpl(smpl, p);

            O.Decomp2 o0 = new O.Decomp2();
            o0.type = @"ASTDECOMP3";
            o0.label = @"decomp <2028 2035 m> qBNP from E_qBNP endo qBNP rows vars, lags cols time";
            o0.t1 = O.ConvertToDate(i3, O.GetDateChoices.FlexibleStart);
            ;
            o0.t2 = O.ConvertToDate(i4, O.GetDateChoices.FlexibleEnd);
            ;

            o0.opt_prtcode = O.ConvertToString((new ScalarString("m")));



            o0.select.Add(O.FlattenIVariablesSeq(false, new List(new List<IVariable> { new ScalarString("qBNP") })));

            o0.from.Add(O.FlattenIVariablesSeq(false, new List(new List<IVariable> { new ScalarString("E_qBNP") })));

            o0.endo.Add(O.FlattenIVariablesSeq(false, new List(new List<IVariable> { new ScalarString("qBNP") })));



            o0.rows.Add(O.FlattenIVariablesSeq(false, new List(new List<IVariable> { new ScalarString("vars"), new ScalarString("lags") })));

            o0.cols.Add(O.FlattenIVariablesSeq(false, new List(new List<IVariable> { new ScalarString("time") })));

            o0.Exe();

            //[[commandEnd]]0
        }


        public static readonly ScalarVal i3 = new ScalarVal(2028d, 0);
        public static readonly ScalarVal i4 = new ScalarVal(2035d, 0);

        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}
