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
public static void C0(GekkoSmpl smpl, P p) {
//[[commandStart]]0
p.SetText(@"¤1"); O.InitSmpl(smpl, p);


O.Open o0 = new O.Open();
o0.openFileNames2 = O.ReplaceSlash(O.ExplodeIvariables(new List(new List<IVariable> {O.ReplaceSlash(O.HandleString(new ScalarString(@"xx"))), O.ReplaceSlash(O.HandleString(new ScalarString(@"yy")))})));
o0.openFileNamesAs2 = null;

o0.Exe();

//[[commandEnd]]0
}



public static void CodeLines(P p)
{
GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

C0(smpl, p);



}
}
}
