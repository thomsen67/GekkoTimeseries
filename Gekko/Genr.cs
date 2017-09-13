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
public static readonly ScalarVal i4 = new ScalarVal(100d);
public static void ClearTS(P p) {
}
public static void ClearScalar(P p) {
}
public static void C0(P p) {

GekkoSmpl smpl = O.Smpl();

           

            Map tmpmap = new Map();
            O.Lookup(smpl, tmpmap, null, "%d", null, true, new ScalarVal(2d));
            O.Lookup(smpl, tmpmap, null, "%s", null, true, new ScalarString("a"));

            O.Lookup(smpl, null, null, "#m", null, true, tmpmap);



        }

        public static void C1(P p) {

GekkoSmpl smpl = O.Smpl();





}


public static void CodeLines(P p)
{
GekkoSmpl smpl = O.Smpl();

C0(p);

return;

C1(p);



}
}
}
