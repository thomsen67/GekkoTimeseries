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


O.Assignment o0 = new O.Assignment();
O.AdjustT0(smpl, -2);
IVariable ivTmpvar768 = i769;
O.AdjustT0(smpl, 2);
O.Lookup(smpl, null, null, "%grad", null, ivTmpvar768, new  LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, o0)
;

//[[commandEnd]]0
}
public static void C1(GekkoSmpl smpl, P p) {
//[[commandStart]]2
p.SetText(@"¤3"); O.InitSmpl(smpl, p);


O.Assignment o2 = new O.Assignment();
smpl.t0 = Globals.globalPeriodStart;
smpl.t1 = Globals.globalPeriodStart;
smpl.t2 = Globals.globalPeriodEnd;
smpl.t3 = Globals.globalPeriodEnd;

o2.opt_n = "yes";


O.AdjustT0(smpl, -2);
IVariable ivTmpvar771 = O.MatrixCol(O.MatrixRow(i772, i773, i774, i775, i776, i777, i778), O.MatrixRow(i779, i780, i781, i782, i783, i784, i785), O.MatrixRow(i786, i787, i788, i789, i790, i791, i792));
O.AdjustT0(smpl, 2);
O.Lookup(smpl, null, null, "#r", null, ivTmpvar771, new  LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Matrix, null)
;

//[[commandEnd]]2
}
public static void C2(GekkoSmpl smpl, P p) {
//[[commandStart]]4
p.SetText(@"¤6"); O.InitSmpl(smpl, p);


O.Assignment o4 = new O.Assignment();
smpl.t0 = Globals.globalPeriodStart;
smpl.t1 = Globals.globalPeriodStart;
smpl.t2 = Globals.globalPeriodEnd;
smpl.t3 = Globals.globalPeriodEnd;

o4.opt_n = "yes";


O.AdjustT0(smpl, -2);
IVariable ivTmpvar794 = O.MatrixCol(O.MatrixRow(i795, i796, i797, i798, i799, i800, i801), O.MatrixRow(i802, i803, i804, i805, i806, i807, i808), O.MatrixRow(i809, i810, i811, i812, i813, i814, i815), O.MatrixRow(i816, i817, i818, i819, i820, i821, i822));
O.AdjustT0(smpl, 2);
O.Lookup(smpl, null, null, "#r", null, ivTmpvar794, new  LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Matrix, null)
;

//[[commandEnd]]4
}
public static void C3(GekkoSmpl smpl, P p) {
//[[commandStart]]6
p.SetText(@"¤9"); O.InitSmpl(smpl, p);


O.Assignment o6 = new O.Assignment();
smpl.t0 = Globals.globalPeriodStart;
smpl.t1 = Globals.globalPeriodStart;
smpl.t2 = Globals.globalPeriodEnd;
smpl.t3 = Globals.globalPeriodEnd;

o6.opt_n = "yes";


O.AdjustT0(smpl, -2);
IVariable ivTmpvar824 = O.MatrixCol(O.MatrixRow(i825, i826, i827, i828, i829, i830, i831), O.MatrixRow(i832, i833, i834, i835, i836, i837, i838), O.MatrixRow(i839, i840, i841, i842, i843, i844, i845), O.MatrixRow(i846, i847, i848, i849, i850, i851, i852), O.MatrixRow(i853, i854, i855, i856, i857, i858, i859));
O.AdjustT0(smpl, 2);
O.Lookup(smpl, null, null, "#r", null, ivTmpvar824, new  LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Matrix, null)
;

//[[commandEnd]]6
}
public static void C4(GekkoSmpl smpl, P p) {
//[[commandStart]]7
p.SetText(@"¤12"); O.InitSmpl(smpl, p);


O.Assignment o7 = new O.Assignment();
smpl.t0 = Globals.globalPeriodStart;
smpl.t1 = Globals.globalPeriodStart;
smpl.t2 = Globals.globalPeriodEnd;
smpl.t3 = Globals.globalPeriodEnd;

o7.opt_n = "yes";


O.AdjustT0(smpl, -2);
IVariable ivTmpvar860 = O.MatrixCol(O.MatrixRow(i861, i862, i863, i864, i865, i866, i867), O.MatrixRow(i868, i869, i870, i871, i872, i873, i874));
O.AdjustT0(smpl, 2);
O.Lookup(smpl, null, null, "#r", null, ivTmpvar860, new  LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Matrix, null)
;

//[[commandEnd]]7
}


public static readonly ScalarVal i769 = new ScalarVal(2d);
public static readonly ScalarVal i770 = new ScalarVal(4d);
public static readonly ScalarVal i772 = new ScalarVal(1d);
public static readonly ScalarVal i773 = new ScalarVal(0d);
public static readonly ScalarVal i774 = new ScalarVal(0d);
public static readonly ScalarVal i775 = new ScalarVal(0d);
public static readonly ScalarVal i776 = new ScalarVal(0d);
public static readonly ScalarVal i777 = new ScalarVal(0d);
public static readonly ScalarVal i778 = new ScalarVal(0d);
public static readonly ScalarVal i779 = new ScalarVal(1d);
public static readonly ScalarVal i780 = new ScalarVal(2d);
public static readonly ScalarVal i781 = new ScalarVal(3d);
public static readonly ScalarVal i782 = new ScalarVal(4d);
public static readonly ScalarVal i783 = new ScalarVal(5d);
public static readonly ScalarVal i784 = new ScalarVal(0d);
public static readonly ScalarVal i785 = new ScalarVal(0d);
public static readonly ScalarVal i786 = new ScalarVal(0d);
public static readonly ScalarVal i787 = new ScalarVal(0d);
public static readonly ScalarVal i788 = new ScalarVal(0d);
public static readonly ScalarVal i789 = new ScalarVal(0d);
public static readonly ScalarVal i790 = new ScalarVal(1d);
public static readonly ScalarVal i791 = new ScalarVal(0d);
public static readonly ScalarVal i792 = new ScalarVal(0d);
public static readonly ScalarVal i793 = new ScalarVal(3d);
public static readonly ScalarVal i795 = new ScalarVal(1d);
public static readonly ScalarVal i796 = new ScalarVal(0d);
public static readonly ScalarVal i797 = new ScalarVal(0d);
public static readonly ScalarVal i798 = new ScalarVal(0d);
public static readonly ScalarVal i799 = new ScalarVal(0d);
public static readonly ScalarVal i800 = new ScalarVal(0d);
public static readonly ScalarVal i801 = new ScalarVal(0d);
public static readonly ScalarVal i802 = new ScalarVal(1d);
public static readonly ScalarVal i803 = new ScalarVal(2d);
public static readonly ScalarVal i804 = new ScalarVal(3d);
public static readonly ScalarVal i805 = new ScalarVal(4d);
public static readonly ScalarVal i806 = new ScalarVal(5d);
public static readonly ScalarVal i807 = new ScalarVal(0d);
public static readonly ScalarVal i808 = new ScalarVal(0d);
public static readonly ScalarVal i809 = new ScalarVal(0d);
public static readonly ScalarVal i810 = new ScalarVal(0d);
public static readonly ScalarVal i811 = new ScalarVal(0d);
public static readonly ScalarVal i812 = new ScalarVal(0d);
public static readonly ScalarVal i813 = new ScalarVal(1d);
public static readonly ScalarVal i814 = new ScalarVal(0d);
public static readonly ScalarVal i815 = new ScalarVal(0d);
public static readonly ScalarVal i816 = new ScalarVal(0d);
public static readonly ScalarVal i817 = new ScalarVal(0d);
public static readonly ScalarVal i818 = new ScalarVal(0d);
public static readonly ScalarVal i819 = new ScalarVal(1d);
public static readonly ScalarVal i820 = new ScalarVal(0d);
public static readonly ScalarVal i821 = new ScalarVal(0d);
public static readonly ScalarVal i822 = new ScalarVal(0d);
public static readonly ScalarVal i823 = new ScalarVal(2d);
public static readonly ScalarVal i825 = new ScalarVal(1d);
public static readonly ScalarVal i826 = new ScalarVal(0d);
public static readonly ScalarVal i827 = new ScalarVal(0d);
public static readonly ScalarVal i828 = new ScalarVal(0d);
public static readonly ScalarVal i829 = new ScalarVal(0d);
public static readonly ScalarVal i830 = new ScalarVal(0d);
public static readonly ScalarVal i831 = new ScalarVal(0d);
public static readonly ScalarVal i832 = new ScalarVal(1d);
public static readonly ScalarVal i833 = new ScalarVal(2d);
public static readonly ScalarVal i834 = new ScalarVal(3d);
public static readonly ScalarVal i835 = new ScalarVal(4d);
public static readonly ScalarVal i836 = new ScalarVal(5d);
public static readonly ScalarVal i837 = new ScalarVal(0d);
public static readonly ScalarVal i838 = new ScalarVal(0d);
public static readonly ScalarVal i839 = new ScalarVal(0d);
public static readonly ScalarVal i840 = new ScalarVal(0d);
public static readonly ScalarVal i841 = new ScalarVal(0d);
public static readonly ScalarVal i842 = new ScalarVal(0d);
public static readonly ScalarVal i843 = new ScalarVal(1d);
public static readonly ScalarVal i844 = new ScalarVal(0d);
public static readonly ScalarVal i845 = new ScalarVal(0d);
public static readonly ScalarVal i846 = new ScalarVal(0d);
public static readonly ScalarVal i847 = new ScalarVal(0d);
public static readonly ScalarVal i848 = new ScalarVal(0d);
public static readonly ScalarVal i849 = new ScalarVal(1d);
public static readonly ScalarVal i850 = new ScalarVal(0d);
public static readonly ScalarVal i851 = new ScalarVal(0d);
public static readonly ScalarVal i852 = new ScalarVal(0d);
public static readonly ScalarVal i853 = new ScalarVal(0d);
public static readonly ScalarVal i854 = new ScalarVal(0d);
public static readonly ScalarVal i855 = new ScalarVal(1d);
public static readonly ScalarVal i856 = new ScalarVal(0d);
public static readonly ScalarVal i857 = new ScalarVal(0d);
public static readonly ScalarVal i858 = new ScalarVal(0d);
public static readonly ScalarVal i859 = new ScalarVal(0d);
public static readonly ScalarVal i861 = new ScalarVal(1d);
public static readonly ScalarVal i862 = new ScalarVal(0d);
public static readonly ScalarVal i863 = new ScalarVal(0d);
public static readonly ScalarVal i864 = new ScalarVal(0d);
public static readonly ScalarVal i865 = new ScalarVal(0d);
public static readonly ScalarVal i866 = new ScalarVal(0d);
public static readonly ScalarVal i867 = new ScalarVal(0d);
public static readonly ScalarVal i868 = new ScalarVal(1d);
public static readonly ScalarVal i869 = new ScalarVal(2d);
public static readonly ScalarVal i870 = new ScalarVal(3d);
public static readonly ScalarVal i871 = new ScalarVal(4d);
public static readonly ScalarVal i872 = new ScalarVal(5d);
public static readonly ScalarVal i873 = new ScalarVal(0d);
public static readonly ScalarVal i874 = new ScalarVal(0d);

public static void CodeLines(P p)
{
GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

C0(smpl, p);


//[[commandSpecial]]1
if(O.IsTrue(smpl, O.Equals(smpl, O.Lookup(smpl, null, null, "%grad", null, null, new  LookupSettings(), EVariableType.Var, null),i770))) {
C1(smpl, p);

}else {
//[[commandSpecial]]3
if(O.IsTrue(smpl, O.Equals(smpl, O.Lookup(smpl, null, null, "%grad", null, null, new  LookupSettings(), EVariableType.Var, null),i793))) {
C2(smpl, p);

}else {
//[[commandSpecial]]5
if(O.IsTrue(smpl, O.Equals(smpl, O.Lookup(smpl, null, null, "%grad", null, null, new  LookupSettings(), EVariableType.Var, null),i823))) {
C3(smpl, p);

}else {
C4(smpl, p);

}
//[[commandEnd]]5

}
//[[commandEnd]]3

}
//[[commandEnd]]1



}
}
}
