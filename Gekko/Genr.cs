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
        public static List PrintHelper_31(GekkoSmpl smpl)
        {
            List m32 = new List(); for (int iBankNumber = 0; iBankNumber < 2; iBankNumber++)
            {
                m32.Add(O.Lookup(smpl, null, null, "xx", null, null, false, iBankNumber));
            }
            return m32;
        }
public static void ClearTS(P p) {
}
public static void ClearScalar(P p) {
}
public static void C0(P p) {

GekkoSmpl smpl = O.Smpl();


p.SetText(@"¤0");
for (int iSmpl33 = 0; iSmpl33 < int.MaxValue; iSmpl33++) {
O.Print(smpl, (PrintHelper_31(smpl)));

if (smpl.HasError()) O.TryNewSmpl(smpl, iSmpl33); else break;
}



}


public static void CodeLines(P p)
{
GekkoSmpl smpl = O.Smpl();

C0(p);



}
}
}
