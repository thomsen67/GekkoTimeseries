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
            p.SetStack(@"¤122"); O.InitSmpl(smpl, p);
            O.Assignment o0 = new O.Assignment();
            o0.opt_trace = @"LIST #kfrem =
// Alle på #var tages med
AFDR,
AFSG,
ATPST,
BQULB,
BTGB,
BTGCOV,
BTGH,
BTGIH,
BTGIOB,
BTGIOM,
BTGIPB,
BTGIPM,
BTGQ,
BTGYBX,
BTGYH,
BTPPK,
BTIU_H,
BTYPK,
BUAB,
D7734,
DMIMS,
DRAD,
DRAL,
DTYE_,
EFKRKS,
ERHFRK,
EUSD,
FBZZ,
FEY,
FIT,
FIY,
FMY,
IB30,
IBMIN00,
IBODEM,
IBZ,
IDI,
IDP,
ILO,
IMM,
IUSD,
JTEN_,
KFXE,
KOBAL,
KOMPE,
KVDEM,
KVUSD,
OWPK,
OWPP,
TAX,
TDU,
TG,
TMBRA,
TMVX,
TMY,
TPKCOV,
TPKQ,
TPKYBX,
TRB,
TRIPM,
TSUIH,
VARG,
// Alle som manuelt fremskrives uændret tages med
ALBA, 
ALBM, 
BSDA, 
D100Q1, 
D78Q4, 
DUMUEL, 
DUMUL, 
FIOB, 
MAXTID, 
MAXTID2, 
OTIME, 
QBY_ORL, 
QFTJ, 
QLA, 
SAK, 
SDR, 
SDV, 
SIQV, 
SSATS, 
TENOU,
TOI, 
TYPRI, 
ZBDPC,
ZBN,
ZBT,
ZDPC_,
ZIBZ,
ZMDPC,
ZMN,
ZMT,
ZTAX,
ZZBBYG_,
ZZMMASK_,
// Yderligere tilføjet
ARBSATS, 
ATPA,
ATPSATS, 
D8081, 
D88Q1,
DB, 
DEUSIM, 
DM_, 
DPYBW, 
DTYO_, 
DUM903, 
DUMDPC, 
DUMMY1973Q4, 
DUMMY1991Q1,
DUMMY1992Q1, 
DUMMY7193,
DUMMY7199,
FEOL, 
KSDR, 
ORLOV, 
RENTEML,
TER, 
TTTT";


            Globals.precedentsSeries = null;
            Action assign_3236 = () => {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar3235 = O.FlattenIVariablesSeq(true, new List(new List<IVariable> { new ScalarString("AFDR"), null, new ScalarString("AFSG"), null, new ScalarString("ATPST"), null, new ScalarString("BQULB"), null, new ScalarString("BTGB"), null, new ScalarString("BTGCOV"), null, new ScalarString("BTGH"), null, new ScalarString("BTGIH"), null, new ScalarString("BTGIOB"), null, new ScalarString("BTGIOM"), null, new ScalarString("BTGIPB"), null, new ScalarString("BTGIPM"), null, new ScalarString("BTGQ"), null, new ScalarString("BTGYBX"), null, new ScalarString("BTGYH"), null, new ScalarString("BTPPK"), null, new ScalarString("BTIU_H"), null, new ScalarString("BTYPK"), null, new ScalarString("BUAB"), null, new ScalarString("D7734"), null, new ScalarString("DMIMS"), null, new ScalarString("DRAD"), null, new ScalarString("DRAL"), null, new ScalarString("DTYE_"), null, new ScalarString("EFKRKS"), null, new ScalarString("ERHFRK"), null, new ScalarString("EUSD"), null, new ScalarString("FBZZ"), null, new ScalarString("FEY"), null, new ScalarString("FIT"), null, new ScalarString("FIY"), null, new ScalarString("FMY"), null, new ScalarString("IB30"), null, new ScalarString("IBMIN00"), null, new ScalarString("IBODEM"), null, new ScalarString("IBZ"), null, new ScalarString("IDI"), null, new ScalarString("IDP"), null, new ScalarString("ILO"), null, new ScalarString("IMM"), null, new ScalarString("IUSD"), null, new ScalarString("JTEN_"), null, new ScalarString("KFXE"), null, new ScalarString("KOBAL"), null, new ScalarString("KOMPE"), null, new ScalarString("KVDEM"), null, new ScalarString("KVUSD"), null, new ScalarString("OWPK"), null, new ScalarString("OWPP"), null, new ScalarString("TAX"), null, new ScalarString("TDU"), null, new ScalarString("TG"), null, new ScalarString("TMBRA"), null, new ScalarString("TMVX"), null, new ScalarString("TMY"), null, new ScalarString("TPKCOV"), null, new ScalarString("TPKQ"), null, new ScalarString("TPKYBX"), null, new ScalarString("TRB"), null, new ScalarString("TRIPM"), null, new ScalarString("TSUIH"), null, new ScalarString("VARG"), null, new ScalarString("ALBA"), null, new ScalarString("ALBM"), null, new ScalarString("BSDA"), null, new ScalarString("D100Q1"), null, new ScalarString("D78Q4"), null, new ScalarString("DUMUEL"), null, new ScalarString("DUMUL"), null, new ScalarString("FIOB"), null, new ScalarString("MAXTID"), null, new ScalarString("MAXTID2"), null, new ScalarString("OTIME"), null, new ScalarString("QBY_ORL"), null, new ScalarString("QFTJ"), null, new ScalarString("QLA"), null, new ScalarString("SAK"), null, new ScalarString("SDR"), null, new ScalarString("SDV"), null, new ScalarString("SIQV"), null, new ScalarString("SSATS"), null, new ScalarString("TENOU"), null, new ScalarString("TOI"), null, new ScalarString("TYPRI"), null, new ScalarString("ZBDPC"), null, new ScalarString("ZBN"), null, new ScalarString("ZBT"), null, new ScalarString("ZDPC_"), null, new ScalarString("ZIBZ"), null, new ScalarString("ZMDPC"), null, new ScalarString("ZMN"), null, new ScalarString("ZMT"), null, new ScalarString("ZTAX"), null, new ScalarString("ZZBBYG_"), null, new ScalarString("ZZMMASK_"), null, new ScalarString("ARBSATS"), null, new ScalarString("ATPA"), null, new ScalarString("ATPSATS"), null, new ScalarString("D8081"), null, new ScalarString("D88Q1"), null, new ScalarString("DB"), null, new ScalarString("DEUSIM"), null, new ScalarString("DM_"), null, new ScalarString("DPYBW"), null, new ScalarString("DTYO_"), null, new ScalarString("DUM903"), null, new ScalarString("DUMDPC"), null, new ScalarString("DUMMY1973Q4"), null, new ScalarString("DUMMY1991Q1"), null, new ScalarString("DUMMY1992Q1"), null, new ScalarString("DUMMY7193"), null, new ScalarString("DUMMY7199"), null, new ScalarString("FEOL"), null, new ScalarString("KSDR"), null, new ScalarString("ORLOV"), null, new ScalarString("RENTEML"), null, new ScalarString("TER"), null, new ScalarString("TTTT"), null }));
                O.AdjustT0(smpl, 2);
                O.Lookup(smpl, null, null, "#kfrem", null, ivTmpvar3235, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.List, o0)
                ;
            };
            Func<bool> check_3236 = () => {
                O.AdjustT0(smpl, -2);
                IVariable ivTmpvar3235 = O.FlattenIVariablesSeq(true, new List(new List<IVariable> { new ScalarString("AFDR"), null, new ScalarString("AFSG"), null, new ScalarString("ATPST"), null, new ScalarString("BQULB"), null, new ScalarString("BTGB"), null, new ScalarString("BTGCOV"), null, new ScalarString("BTGH"), null, new ScalarString("BTGIH"), null, new ScalarString("BTGIOB"), null, new ScalarString("BTGIOM"), null, new ScalarString("BTGIPB"), null, new ScalarString("BTGIPM"), null, new ScalarString("BTGQ"), null, new ScalarString("BTGYBX"), null, new ScalarString("BTGYH"), null, new ScalarString("BTPPK"), null, new ScalarString("BTIU_H"), null, new ScalarString("BTYPK"), null, new ScalarString("BUAB"), null, new ScalarString("D7734"), null, new ScalarString("DMIMS"), null, new ScalarString("DRAD"), null, new ScalarString("DRAL"), null, new ScalarString("DTYE_"), null, new ScalarString("EFKRKS"), null, new ScalarString("ERHFRK"), null, new ScalarString("EUSD"), null, new ScalarString("FBZZ"), null, new ScalarString("FEY"), null, new ScalarString("FIT"), null, new ScalarString("FIY"), null, new ScalarString("FMY"), null, new ScalarString("IB30"), null, new ScalarString("IBMIN00"), null, new ScalarString("IBODEM"), null, new ScalarString("IBZ"), null, new ScalarString("IDI"), null, new ScalarString("IDP"), null, new ScalarString("ILO"), null, new ScalarString("IMM"), null, new ScalarString("IUSD"), null, new ScalarString("JTEN_"), null, new ScalarString("KFXE"), null, new ScalarString("KOBAL"), null, new ScalarString("KOMPE"), null, new ScalarString("KVDEM"), null, new ScalarString("KVUSD"), null, new ScalarString("OWPK"), null, new ScalarString("OWPP"), null, new ScalarString("TAX"), null, new ScalarString("TDU"), null, new ScalarString("TG"), null, new ScalarString("TMBRA"), null, new ScalarString("TMVX"), null, new ScalarString("TMY"), null, new ScalarString("TPKCOV"), null, new ScalarString("TPKQ"), null, new ScalarString("TPKYBX"), null, new ScalarString("TRB"), null, new ScalarString("TRIPM"), null, new ScalarString("TSUIH"), null, new ScalarString("VARG"), null, new ScalarString("ALBA"), null, new ScalarString("ALBM"), null, new ScalarString("BSDA"), null, new ScalarString("D100Q1"), null, new ScalarString("D78Q4"), null, new ScalarString("DUMUEL"), null, new ScalarString("DUMUL"), null, new ScalarString("FIOB"), null, new ScalarString("MAXTID"), null, new ScalarString("MAXTID2"), null, new ScalarString("OTIME"), null, new ScalarString("QBY_ORL"), null, new ScalarString("QFTJ"), null, new ScalarString("QLA"), null, new ScalarString("SAK"), null, new ScalarString("SDR"), null, new ScalarString("SDV"), null, new ScalarString("SIQV"), null, new ScalarString("SSATS"), null, new ScalarString("TENOU"), null, new ScalarString("TOI"), null, new ScalarString("TYPRI"), null, new ScalarString("ZBDPC"), null, new ScalarString("ZBN"), null, new ScalarString("ZBT"), null, new ScalarString("ZDPC_"), null, new ScalarString("ZIBZ"), null, new ScalarString("ZMDPC"), null, new ScalarString("ZMN"), null, new ScalarString("ZMT"), null, new ScalarString("ZTAX"), null, new ScalarString("ZZBBYG_"), null, new ScalarString("ZZMMASK_"), null, new ScalarString("ARBSATS"), null, new ScalarString("ATPA"), null, new ScalarString("ATPSATS"), null, new ScalarString("D8081"), null, new ScalarString("D88Q1"), null, new ScalarString("DB"), null, new ScalarString("DEUSIM"), null, new ScalarString("DM_"), null, new ScalarString("DPYBW"), null, new ScalarString("DTYO_"), null, new ScalarString("DUM903"), null, new ScalarString("DUMDPC"), null, new ScalarString("DUMMY1973Q4"), null, new ScalarString("DUMMY1991Q1"), null, new ScalarString("DUMMY1992Q1"), null, new ScalarString("DUMMY7193"), null, new ScalarString("DUMMY7199"), null, new ScalarString("FEOL"), null, new ScalarString("KSDR"), null, new ScalarString("ORLOV"), null, new ScalarString("RENTEML"), null, new ScalarString("TER"), null, new ScalarString("TTTT"), null }));
                O.AdjustT0(smpl, 2);
                if (ivTmpvar3235.Type() != EVariableType.Series) return false;
                O.Dynamic1(smpl);
                O.Lookup(smpl, null, null, "#kfrem", null, ivTmpvar3235, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.List, o0)
                ;
                return O.Dynamic2(smpl);
            };
            O.RunAssigmentMaybeDynamic(smpl, assign_3236, check_3236, o0, p);

            //[[commandEnd]]0
        }



        public static void CodeLines(P p)
        {
            GekkoSmpl smpl = new GekkoSmpl(); O.InitSmpl(smpl, p);

            C0(smpl, p);



        }
    }
}