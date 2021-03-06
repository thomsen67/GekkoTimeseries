﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Gekko
{
    /// <summary>
    /// Contains helper methods called by dynamic code. This is the part of the O.class that deals with lookup code, to keep this in one file. The rest of (and the 
    /// largest part) is found in O.cs.
    /// </summary>
    public static partial class O
    {
        // --------------------------------------------------------------------------------------------------------------------------------------------
        // LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START
        // LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START
        // LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START
        // LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START
        // LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START
        // LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START LOOKUPS START
        // --------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Lookup, if $-condition is present.
        /// </summary>
        /// <param name="logical"></param>
        /// <param name="smpl"></param>
        /// <param name="map"></param>
        /// <param name="dbName"></param>
        /// <param name="varname"></param>
        /// <param name="freq"></param>
        /// <param name="rhsExpression"></param>
        /// <param name="isLeftSideVariable"></param>
        /// <param name="type"></param>
        /// <param name="options"></param>
        //NOTE: Must have same signature as Lookup(), #89075234532
        public static void DollarLookup(IVariable logical, GekkoSmpl smpl, Map map, string dbName, string varname, string freq, IVariable rhsExpression, LookupSettings isLeftSideVariable, EVariableType type, O.Assignment options)
        {
            //Only encountered on the LHS
            if (logical == null)
            {
                Lookup(smpl, map, dbName, varname, freq, rhsExpression, isLeftSideVariable, type, options);
            }
            if (logical.Type() == EVariableType.Val)
            {
                if (IsTrue(((ScalarVal)logical).val))
                {
                    Lookup(smpl, map, dbName, varname, freq, rhsExpression, isLeftSideVariable, type, options);
                }
                else
                {
                    //skip it!
                }
            }
            else if (logical.Type() == EVariableType.Series)
            {
                //This deviates a bit from GAMS: when logical is 0 here, a 0 will also be set for the LHS, it is not just skipped.
                //See also #6238454
                IVariable y = Conditional1Of3(smpl, rhsExpression, logical);
                Lookup(smpl, map, dbName, varname, freq, y, isLeftSideVariable, type, options);
            }
            else
            {
                DollarLHSError();
            }
        }

        /// <summary>
        /// Overload.
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="map"></param>
        /// <param name="x"></param>
        /// <param name="rhsExpression"></param>
        /// <param name="isLeftSideVariable"></param>
        /// <param name="type"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IVariable Lookup(GekkoSmpl smpl, Map map, IVariable x, IVariable rhsExpression, LookupSettings isLeftSideVariable, EVariableType type, O.Assignment options)
        {
            //overload
            return Lookup(smpl, map, x, rhsExpression, isLeftSideVariable, type, true, options);
        }

        /// <summary>
        /// NameLookup() is used in some places, for instance ENDO/EXO command. Perhaps naked
        /// lists could be used instead, instead of NameLookup() complicated code.
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="map"></param>
        /// <param name="x"></param>
        /// <param name="rhsExpression"></param>
        /// <param name="isLeftSideVariable"></param>
        /// <param name="type"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IVariable NameLookup(GekkoSmpl smpl, Map map, IVariable x, IVariable rhsExpression, LookupSettings isLeftSideVariable, EVariableType type, O.Assignment options)
        {
            return x;
        }

        /// <summary>
        /// Overload.
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="map"></param>
        /// <param name="x"></param>
        /// <param name="rhsExpression"></param>
        /// <param name="settings"></param>
        /// <param name="type"></param>
        /// <param name="errorIfNotFound"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        //NOTE: Must have same signature as DollarLookup(), #89075234532
        public static IVariable Lookup(GekkoSmpl smpl, Map map, IVariable x, IVariable rhsExpression, LookupSettings settings, EVariableType type, bool errorIfNotFound, O.Assignment options)
        {
            //This calls the more general Lookup(GekkoSmpl smpl, Map map, string dbName, string varname, string freq, IVariable rhsExpression)

            if (x.Type() == EVariableType.String)
            {

                IVariable rv = null;
                string x_string = (x as ScalarString).string2;


                string dbName, varName, freq; string[] indexes; char firstChar;
                if (Program.IsListfileArtificialName(x_string))
                {
                    dbName = null; varName = x_string; freq = null; indexes = null; firstChar = varName[0];
                }
                else
                {
                    Chop(x_string, out dbName, out varName, out freq, out indexes);
                }

                LookupSettings settingsTemp = settings;
                if (indexes != null)
                {
                    settingsTemp = new LookupSettings(); //normal abort if array-super-series is not found, cannot just be created
                    settingsTemp.depth = settings.depth;  //no recursion for #alias
                }

                IVariable iv = Lookup(smpl, map, dbName, varName, freq, rhsExpression, settingsTemp, type, errorIfNotFound, options);

                if (indexes != null)
                {
                    Series iv_series = iv as Series;
                    if (iv_series == null || iv_series.type != ESeriesType.ArraySuper)
                    {
                        new Error("Expected array-series variable");
                        //throw new GekkoException();
                    }

                    rv = iv_series.FindArraySeries(smpl, Stringlist.GetListOfIVariablesFromListOfStrings(indexes), false, false, settings);  //last arg. not used

                }
                else
                {
                    rv = iv;
                }

                return rv;
            }
            else if (x.Type() == EVariableType.List)
            {
                //for instance PRT {('a', 'b')}. A controlled unfold like PRT {#m} will not get here.
                List x_list = x as List;
                string[] items = Stringlist.GetListOfStringsFromListOfIvariables(x_list.list.ToArray());
                if (items == null)
                {
                    new Error("The list contains non-string elements");
                    //throw new GekkoException();
                }
                else
                {
                    List<IVariable> rv = new List<IVariable>();
                    foreach (string s in items)
                    {
                        IVariable iv = GetIVariableFromString(s, ECreatePossibilities.NoneReportError, true);
                        rv.Add(iv);
                    }
                    List m = new List(rv);
                    return m;
                }
            }
            else
            {
                new Error("Expected variable name to be a string, but it is of " + G.GetTypeString(x) + " type");
                //throw new GekkoException();
            }
            return x;
        }

        /// <summary>
        /// Overload.
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="map"></param>
        /// <param name="dbName"></param>
        /// <param name="varname"></param>
        /// <param name="freq"></param>
        /// <param name="rhsExpression"></param>
        /// <param name="isLeftSideVariable"></param>
        /// <param name="type"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IVariable Lookup(GekkoSmpl smpl, Map map, string dbName, string varname, string freq, IVariable rhsExpression, LookupSettings isLeftSideVariable, EVariableType type, O.Assignment options)
        {
            //overload
            return Lookup(smpl, map, dbName, varname, freq, rhsExpression, isLeftSideVariable, type, true, options);
        }

        /// <summary>
        /// NameLookup() is used in some places, for instance ENDO/EXO command. Perhaps naked
        /// lists could be used instead, instead of NameLookup() complicated code.
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="map"></param>
        /// <param name="dbName"></param>
        /// <param name="varname"></param>
        /// <param name="freq"></param>
        /// <param name="rhsExpression"></param>
        /// <param name="isLeftSideVariable"></param>
        /// <param name="type"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IVariable NameLookup(GekkoSmpl smpl, Map map, string dbName, string varname, string freq, IVariable rhsExpression, LookupSettings isLeftSideVariable, EVariableType type, O.Assignment options)
        {
            if (dbName != null || freq != null)
            {
                new Error("Expected a simple variable name without bank or frequency");
                //throw new GekkoException();
            }

            return new ScalarString(varname);
        }

        /// <summary>
        /// This is the central Lookup() hub, where everything regarding an expression or an assigment passes through.
        /// So whenever an IVariable is passed around in an expression, it takes place here. The easiest part is if it 
        /// is a variable that is fetched on the right-hand side (RHS), and this will call LookupHelperRightside().
        /// But variables that are going to be assigned to 
        /// on the left-hand side (LHS) also go through this hub (will call LookupHelperLeftside()). If indexers are used
        /// on the left-hand side, this is dealt with in special code. Note that this method will also handle variables inside
        /// maps (maps are in reality kinds of mini-databanks).
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="map"></param>
        /// <param name="dbName"></param>
        /// <param name="varname"></param>
        /// <param name="freq"></param>
        /// <param name="rhsExpression"></param>
        /// <param name="settings"></param>
        /// <param name="type"></param>
        /// <param name="errorIfNotFound"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        //Also see #8093275432098
        public static IVariable Lookup(GekkoSmpl smpl, Map map, string dbName, string varname, string freq, IVariable rhsExpression, LookupSettings settings, EVariableType type, bool errorIfNotFound, O.Assignment options)
        {
            // =============================================================================================
            // =============================================================================================
            // =============================================================================================
            // =============================================================================================
            // ==================== THIS IS THE CENTRAL LOOKUP METHOD ======================================
            // ====================== everything passes through here =======================================
            // =============================================================================================
            // =============================================================================================
            // =============================================================================================

            //map != null:             the variable is found in the MAP, otherwise, the variable is found in a databank
            //rhsExpression != null:   it is an assignment of the left-hand side

            //only adds freq if not there. No sigil is added for lhs vars here.
            string varnameWithFreq = G.AddFreq(varname, freq, type, settings.type);

            if (Program.options.interface_alias)
            {
                bool foundAlias = true;

                if (Program.alias == null)
                {
                    IVariable alias2 = Program.databanks.GetGlobal().GetIVariable("#alias");

                    if (alias2 == null)
                    {
                        foundAlias = false;
                    }
                    else
                    {

                        if (alias2 == null || alias2.Type() != EVariableType.List)
                        {
                            new Error("No global:#alias list was found, even though. OPTION interface alias = yes.");

                            //throw new GekkoException();
                        }

                        GekkoDictionary<string, string> alias3 = new GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                        List<IVariable> alias_list = (alias2 as List).list;
                        foreach (IVariable iv in alias_list)
                        {
                            if (iv.Type() != EVariableType.List)
                            {
                                new Error("global:#alias must be a list of lists");
                                //throw new GekkoException();
                            }
                            List<IVariable> element_list = (iv as List).list;
                            if (element_list.Count != 2)
                            {
                                new Error("the elements of global:#alias must contain two strings");
                                //throw new GekkoException();
                            }
                            string s1 = G.Chop_AddFreq(O.ConvertToString(element_list[0]), Program.options.freq);
                            string s2 = G.Chop_AddFreq(O.ConvertToString(element_list[1]), Program.options.freq);

                            if (alias3.ContainsKey(s1))
                            {
                                new Error("the string " + s1 + " appears several times in global:#alias");
                                //throw new GekkoException();
                            }
                            alias3.Add(s1, s2);
                        }
                        Program.alias = alias3;  //we wait until global:#alias has finished looping, so that only if global:#alias is ok, will Program.alias be != null
                    }
                }

                if (foundAlias)
                {
                    if (settings.depth == 0)
                    {
                        //use the dict
                        string var2 = null; Program.alias.TryGetValue(varnameWithFreq, out var2);
                        if (var2 != null)
                        {
                            varnameWithFreq = var2;
                            //varname = G.Chop_RemoveFreq(varnameWithFreq);

                            settings.depth++;  //will be 1
                            return O.Lookup(smpl, map, new ScalarString(varnameWithFreq), rhsExpression, settings, type, options);

                        }
                    }
                    else
                    {

                    }
                }

            }

            if (settings.type == ELookupType.LeftHandSide)
            {
                //ASSIGNMENT OF LEFT-HAND SIDE
                //ASSIGNMENT OF LEFT-HAND SIDE
                //ASSIGNMENT OF LEFT-HAND SIDE
                //ASSIGNMENT OF LEFT-HAND SIDE
                //ASSIGNMENT OF LEFT-HAND SIDE

                IBank ib = null;
                if (map != null)
                {
                    ib = map;
                }
                else
                {
                    Databank db = null;

                    if (IsAllSpecialDatabank(dbName))
                    {
                        new Error("all:x = ... is not supported, use first:x = ... instead to circumvent LOCAL/GLOBAL<all>.");
                    }
                    else
                    {

                        if (dbName == null)
                        {
                            LocalGlobal.ELocalGlobalType lg = Program.databanks.localGlobal.GetValue(varname);  //varname is always without freq 
                            db = HandleLocalGlobalBank(lg);
                        }
                        else
                        {
                            db = Program.databanks.GetDatabank(dbName, true);
                        }
                    }
                    ib = db;
                }

                if (rhsExpression != null)
                {
                    //direct assignment, like x = 5, or %s = 'a'
                    //in these cases, the LHS can be created if it is not already existing
                    //ScalarString ss = rhsExpression as ScalarString;                    
                    LookupHelperLeftside(smpl, ib, varnameWithFreq, freq, rhsExpression, type, options);
                    return null;
                }
                else
                {
                    //indexers on lhs, for instance x['a'] = ... or x[2000] = 5 or #x.%s = ...
                    //in this case, the x variable must exist
                    //NOTE: no databank search is allowed!
                    //NOTE: sigils cannot be omitted here. VAL x['v'] = 100 or VAL x.v = 100 will not access a %v variable.

                    IVariable ivar2 = ib.GetIVariable(varnameWithFreq);
                    if (ivar2 == null)
                    {
                        new Error("Could not find variable " + G.GetNameAndFreqPretty(varnameWithFreq, true) + " for use in dot- or []-indexing");
                        return null;
                        //throw new GekkoException();
                    }
                    else
                    {
                        return ivar2;
                    }
                }
            }
            else
            {
                //SIMPLE LOOKUP ON RIGHT-HAND SIDE
                //SIMPLE LOOKUP ON RIGHT-HAND SIDE
                //SIMPLE LOOKUP ON RIGHT-HAND SIDE
                //SIMPLE LOOKUP ON RIGHT-HAND SIDE
                //SIMPLE LOOKUP ON RIGHT-HAND SIDE

                //NOTE: databank search may be allowed!
                return LookupHelperRightside(smpl, map, dbName, varnameWithFreq, varname, settings);
            }
        }

        /// <summary>
        /// NameLookup() is used in some places, for instance ENDO/EXO command. Perhaps naked
        /// lists could be used instead, instead of NameLookup() complicated code.
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="map"></param>
        /// <param name="dbName"></param>
        /// <param name="varname"></param>
        /// <param name="freq"></param>
        /// <param name="rhsExpression"></param>
        /// <param name="isLeftSideVariable"></param>
        /// <param name="type"></param>
        /// <param name="errorIfNotFound"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        //Also see #8093275432098
        public static string NameLookup(GekkoSmpl smpl, Map map, string dbName, string varname, string freq, IVariable rhsExpression, LookupSettings isLeftSideVariable, EVariableType type, bool errorIfNotFound, O.Assignment options)
        {
            return varname;
        }

        /// <summary>
        /// Important helper method regarding Lookup() and right-hand side (RHS) variable 
        /// </summary>
        /// <param name="smpl"></param>
        /// <param name="map"></param>
        /// <param name="dbName"></param>
        /// <param name="varnameWithFreq"></param>
        /// <param name="varname"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        private static IVariable LookupHelperRightside(GekkoSmpl smpl, Map map, string dbName, string varnameWithFreq, string varname, LookupSettings settings)
        {
            //varname is used for local/global stuff, faster than chopping up varnameWithFreq up now
            //Can either look up stuff in a Map, or in a databank

            bool isAllSpecialDatabank = IsAllSpecialDatabank(dbName); // all:x should be understood as just x, circumventing any local<all> or global<all>

            bool errorIfNotFound = settings.create == ECreatePossibilities.NoneReportError;  //else it will return null

            IVariable rv = null;
            string frombank = null;

            if (Program.CheckIfLooksLikeWildcard2(dbName) || Program.CheckIfLooksLikeWildcard(varnameWithFreq))
            {
                //a pattern like {'a*'} or rather {'a*!a'} is caught here

                if (dbName != null)
                {
                    varnameWithFreq = G.Chop_AddBank(varnameWithFreq, dbName);
                }

                List<string> names = Program.Search(new List(new List<string>() { varnameWithFreq }), frombank, EVariableType.Var);

                if (Globals.fixWildcardLabel && smpl != null)
                {
                    smpl.labelRecordedPieces = new List<RecordedPieces>();
                    for (int i = 0; i < names.Count; i++)
                    {
                        RecordedPieces r = new RecordedPieces("wildcard", new ScalarString(names[i]));
                        smpl.labelRecordedPieces.Add(r);
                    }
                }

                List<IVariable> names2 = new List<IVariable>();
                foreach (string name in names)
                {
                    names2.Add(O.GetIVariableFromString(name, ECreatePossibilities.NoneReportError));
                }

                rv = new List(names2);

            }
            else
            {
                if (map == null)
                {
                    //It must be a databank then
                    if (dbName == null || isAllSpecialDatabank)
                    {
                        //No explicit databank name is provided, or an all:x

                        if (Program.IsListfileArtificialName(varname))
                        {
                            //special case: #(listfile m)

                            List ml = ReadListFile(varname);
                            return ml;
                        }
                        else
                        {

                            //databank name not given, for instance "PRT x", or it is given as "PRT all:x"
                            //Searching only if:
                            //  (1) OPTION databank search = yes
                            //  (2) settings.canSearch = true (deactivated for commands like DOC, TRUNCATE and others)
                            //  (3) the name is not found in Local or Global databanks
                            //  (4) it is on the right-hand side (which is the case here)
                            //  With Program.options.databank_search == false, it will skip banks opened with OPEN (but search local/global)

                            if (smpl != null && smpl.bankNumber == 1 && !G.StartsWithSigil(varnameWithFreq))
                            {
                                //Ref lookup, also overrules local/global
                                Databank db = null;
                                db = Program.databanks.GetRef();
                                rv = LookupHelperFindVariableInSpecificBank(varnameWithFreq, settings, db);
                            }
                            else
                            {
                                //non-Ref lookup         

                                LocalGlobal.ELocalGlobalType lg = Program.databanks.localGlobal.GetValue(varname);  //varname is always without freq

                                if (lg != LocalGlobal.ELocalGlobalType.None && !isAllSpecialDatabank)
                                {
                                    //the variable x has been stated with LOCAL or GLOBAL keyword, and all:x is not used
                                    //Really should be handled under lookup in specific bank, since the logic
                                    //is just as if local:x had been stated, and not just x.

                                    Databank db = HandleLocalGlobalBank(lg);
                                    rv = LookupHelperFindVariableInSpecificBank(varnameWithFreq, settings, db);
                                    if (rv == null)
                                    {
                                        //this error message is perhaps already done in LookupHelperFindVariableInSpecificBank(), but for safety it is here, too
                                        new Error("Could not find variable " + G.GetNameAndFreqPretty(varnameWithFreq) + " in databank '" + db.name + "'");
                                        //throw new GekkoException();
                                    }
                                }
                                else
                                {

                                    bool canSearch = Program.options.databank_search && settings.canSearch;
                                    rv = Program.databanks.GetVariableWithSearch(varnameWithFreq, canSearch);

                                    if (rv == null)
                                    {
                                        if (settings.create == ECreatePossibilities.NoneReportError)
                                        {
                                            //     Local
                                            //  0. Work
                                            //  1. Ref
                                            //  2. OPEN1
                                            //  3. OPEN2
                                            //     Global
                                            //
                                            string s = null;
                                            string ss = null;
                                            string sss = null;
                                            if (!canSearch)
                                            {
                                                if (Program.databanks.GetLocal().storage.Count > 0 || Program.databanks.GetGlobal().storage.Count > 0) sss = " (or Local/Global)";
                                                ss = "the first-position" + sss + " databank";
                                            }
                                            else
                                            {
                                                ss = "any open databank";
                                                if (Program.databanks.GetRef().storage.Count() > 0) s = " (excluding Ref)";
                                            }
                                            new Error("Could not find variable " + G.GetNameAndFreqPretty(varnameWithFreq) + " in " + ss + s);
                                            //throw new GekkoException();
                                        }
                                        else if (settings.create == ECreatePossibilities.Can || settings.create == ECreatePossibilities.Must)
                                        {                                            
                                            //it is probably ok to create it here like this                                            
                                            //never mind...
                                            rv = new Series(G.ConvertFreq(G.Chop_GetFreq(varnameWithFreq)), varnameWithFreq);  //brand new
                                            Program.databanks.GetFirst().AddIVariableWithOverwrite(rv);
                                        }
                                        else
                                        {
                                            //just return the null
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        //We have an explicit databank given, like "PRT bank1:x"
                        //If we have bankNumber = 1 (Ref bank, used for PRT), we put in the Ref bank instead
                        //In that way, "MULPRT x" and "MULPRT work:x" will give the same (as it should).
                        if (smpl != null && smpl.bankNumber == 1 && !G.StartsWithSigil(varnameWithFreq))
                        {
                            //only for series type
                            rv = LookupHelperFindVariableInSpecificBank(varnameWithFreq, settings, Program.databanks.GetRef());
                        }
                        else
                        {
                            //databank name is given explicitly, and we are not doing bankNumber stuff
                            Databank db = Program.databanks.GetDatabank(dbName, true); //we know that dbName is not null
                            rv = LookupHelperFindVariableInSpecificBank(varnameWithFreq, settings, db);
                        }
                    }
                }
                else
                {
                    //We use the IBank interface here
                    rv = LookupHelperRightside2(map, dbName, varnameWithFreq);
                    if (rv == null)
                    {
                        new Error("Could not find variable " + G.GetNameAndFreqPretty(varnameWithFreq) + " in map collection");
                        //throw new GekkoException();
                    }
                }
            }
            return rv;
        }

        private static IVariable LookupHelperRightside2(Map map, string dbName, string varnameWithFreq)
        {
            IVariable rv;
            IBank ib = null;
            if (map != null) ib = map;
            else ib = Program.databanks.GetDatabank(dbName, true);
            rv = ib.GetIVariable(varnameWithFreq);
            if (rv == null)
            {

                new Error("Could not find variable " + G.GetNameAndFreqPretty(varnameWithFreq) + " in " + ib.Message());
                //throw new GekkoException();
            }
            return rv;
        }

        public static void LookupHelperLeftside(GekkoSmpl smpl, IBank ib, string varnameWithFreq, string freq, IVariable rhs, EVariableType type, O.Assignment options)
        {
            //normal use
            LookupHelperLeftside(smpl, ib, varnameWithFreq, freq, rhs, null, type, options);
        }

        public static void LookupHelperLeftside(GekkoSmpl smpl, Series arraySubSeries, IVariable rhs, EVariableType type, O.Assignment options)
        {
            //use for array-series, for instance xx['a'] = ...
            LookupHelperLeftside(smpl, null, null, null, rhs, arraySubSeries, type, options);
        }

        private static void LookupHelperLeftside(GekkoSmpl smpl, IBank ib, string varnameWithFreq, string freq, IVariable rhs, Series arraySubSeries, EVariableType lhsType, O.Assignment o)
        {
            //This is an assignment, for instance %x = 5, or x = (1, 2, 3), or bank:x = bank:y, or #m.x = (1, 2, 3).
            //Assignment is the hardest part of Lookup()

            bool isArraySubSeries = false;
            if (arraySubSeries != null) isArraySubSeries = true;

            if (smpl.lhsAssignmentType == assignmantTypeLhs.Active)
            {
                //active
                if (isArraySubSeries)
                {
                    //in this case, varnameWithFreq will be null, but we are sure it is a series
                    smpl.lhsAssignmentType = assignmantTypeLhs.Series;
                }
                else
                {
                    if (G.Chop_HasSigil(varnameWithFreq)) smpl.lhsAssignmentType = assignmantTypeLhs.Nonseries;
                    else smpl.lhsAssignmentType = assignmantTypeLhs.Series;
                }
                return;  //is just a probe on the type of the lhs, so we return without changing anything!
            }

            bool isListfile = Program.IsListfileArtificialName(varnameWithFreq);

            if (!isListfile && ib != null && ib.BankType() == EBankType.Normal)
            {
                //ib can be == null with an indexer on the lhs, like #m.#n.%s
                Databank ib_databank = ib as Databank;
                if (!ib_databank.editable) Program.ProtectError("You cannot add/change a variable in non-editable databank ('" + ib_databank.name + "'), see OPEN<edit> or UNLOCK");
                ib_databank.isDirty = true;
            }

            if (rhs.Type() == EVariableType.List)
            {
                List rhs_list = rhs as List;
            }

            if (isListfile)
            {
                WriteListFile(varnameWithFreq, rhs);
            }
            else
            {
                IVariable lhs = null;
                if (ib != null)
                {
                    //ib can be == null with an indexer on the lhs, like #m.#n.%s
                    lhs = ib.GetIVariable(varnameWithFreq); //may return null
                }

                //We divide into three groups depending on LHS name:
                //  A. LHS variable starts with '%'
                //  B. LHS variable starts with '#'                
                //  C. LHS variable has no sigil/symbol as first character (or isArraySubSeries == true)

                //  For each A, B, C, we also have the 7 possible types of the RHS, for instace ... = 2012q1 (date type)
                //  And for each of these 7 types, we may have a LHS type indicator, for instance DATE %d = ...  (should become date)
                //  Note: on the RHS, a series may be normal series, timeless series, array-series.

                //The following is hard to refactor, but the switches keeps it modularized.

                if (!isArraySubSeries && varnameWithFreq[0] == Globals.symbolScalar)
                {
                    // -----------------------------------------------------------------------------------
                    // A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A
                    // A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A
                    // A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A
                    // A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A
                    // LHS variable starts with '%'
                    // A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A
                    // A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A
                    // A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A
                    // A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A
                    // -----------------------------------------------------------------------------------
                    // Starts with '%'

                    //smpl.omitDynamicSeries = true;

                    if (lhsType == EVariableType.Val || lhsType == EVariableType.String || lhsType == EVariableType.Date || lhsType == EVariableType.Var)
                    {
                        //good
                    }
                    else
                    {
                        new Error("Name '" + varnameWithFreq + "' with '" + Globals.symbolScalar + "' symbol cannot be of " + lhsType.ToString().ToUpper() + " type");
                        //throw new GekkoException();
                    }

                    switch (rhs.Type())
                    {
                        case EVariableType.Series:
                            {
                                //---------------------------------------------------------
                                //%x = SERIES
                                //---------------------------------------------------------

                                Series rhsExpression_series = rhs as Series;
                                switch (rhsExpression_series.type)
                                {
                                    case ESeriesType.Timeless:
                                        {
                                            //---------------------------------------------------------
                                            // %x = Series Timeless
                                            //---------------------------------------------------------
                                            if (lhsType == EVariableType.Val || lhsType == EVariableType.Var)
                                            {
                                                // VAL %x = Series Timeless
                                                IVariable lhsNew = new ScalarVal(rhsExpression_series.GetTimelessData());
                                                AddIvariableWithOverwrite(ib, varnameWithFreq, lhs != null, lhsNew);
                                                G.ServiceMessage("VAL " + varnameWithFreq + " updated ", smpl.p);
                                            }
                                            else
                                            {
                                                ReportTypeError(varnameWithFreq, rhs, lhsType);
                                            }
                                        }
                                        break;
                                    default:
                                        {
                                            //---------------------------------------------------------
                                            // %x = Series Normal
                                            //---------------------------------------------------------                                        
                                            ReportTypeError(varnameWithFreq, rhs, lhsType);
                                        }
                                        break;
                                }
                            }
                            break;
                        case EVariableType.Val:
                            {
                                //---------------------------------------------------------
                                // %x = VAL
                                //---------------------------------------------------------
                                //TODO: date %d = 2010.

                                if (lhsType == EVariableType.Val || lhsType == EVariableType.Var)
                                {
                                    IVariable lhsNew = new ScalarVal(((ScalarVal)rhs).val);
                                    AddIvariableWithOverwrite(ib, varnameWithFreq, lhs != null, lhsNew);
                                    G.ServiceMessage("VAL " + varnameWithFreq + " updated ", smpl.p);
                                }
                                else if (lhsType == EVariableType.Date)
                                {
                                    IVariable lhsNew = new ScalarDate(rhs.ConvertToDate(GetDateChoices.Strict));
                                    AddIvariableWithOverwrite(ib, varnameWithFreq, lhs != null, lhsNew);
                                    G.ServiceMessage("DATE " + varnameWithFreq + " updated ", smpl.p);
                                }
                                else
                                {
                                    //STRING command will fail
                                    ReportTypeError(varnameWithFreq, rhs, lhsType);
                                }
                            }
                            break;
                        case EVariableType.String:
                            {
                                //---------------------------------------------------------
                                // %x = STRING
                                //---------------------------------------------------------                            

                                if (lhsType == EVariableType.String || lhsType == EVariableType.Var)
                                {
                                    IVariable lhsNew = new ScalarString(((ScalarString)rhs).string2);
                                    AddIvariableWithOverwrite(ib, varnameWithFreq, lhs != null, lhsNew);
                                    G.ServiceMessage("STRING " + varnameWithFreq + " updated ", smpl.p);
                                }
                                else
                                {
                                    //DATE and VAL commands will fail
                                    ReportTypeError(varnameWithFreq, rhs, lhsType);
                                }

                            }
                            break;
                        case EVariableType.Date:
                            {
                                //---------------------------------------------------------
                                // %x = DATE
                                //---------------------------------------------------------

                                if (lhsType == EVariableType.Date || lhsType == EVariableType.Var)
                                {
                                    IVariable lhsNew = new ScalarDate(((ScalarDate)rhs).date);
                                    AddIvariableWithOverwrite(ib, varnameWithFreq, lhs != null, lhsNew);
                                    G.ServiceMessage("DATE " + varnameWithFreq + " updated ", smpl.p);
                                }
                                else
                                {
                                    //STRING and VAL commands will fail
                                    ReportTypeError(varnameWithFreq, rhs, lhsType);
                                }

                            }
                            break;
                        case EVariableType.List:
                            {
                                //---------------------------------------------------------
                                // %x = LIST
                                //---------------------------------------------------------
                                ReportTypeError(varnameWithFreq, rhs, lhsType);
                            }
                            break;
                        case EVariableType.Map:
                            {
                                //---------------------------------------------------------
                                // %x = MAP
                                //---------------------------------------------------------

                                ReportTypeError(varnameWithFreq, rhs, lhsType);

                            }
                            break;
                        case EVariableType.Matrix:
                            {
                                //---------------------------------------------------------
                                // %x = MATRIX
                                //---------------------------------------------------------                            
                                if (lhsType == EVariableType.Val || lhsType == EVariableType.Var)
                                {
                                    IVariable lhsNew = new ScalarVal(rhs.ConvertToVal());  //only 1x1 matrix will become VAL
                                    AddIvariableWithOverwrite(ib, varnameWithFreq, lhs != null, lhsNew);
                                    G.ServiceMessage("VAL " + varnameWithFreq + " updated ", smpl.p);
                                }
                                else
                                {
                                    ReportTypeError(varnameWithFreq, rhs, lhsType);
                                }
                            }
                            break;
                        case EVariableType.Null:
                            {
                                //---------------------------------------------------------
                                // %x = NULL
                                //---------------------------------------------------------                            
                                new Error("Null-value on right-hand side");
                                //throw new GekkoException();
                            }
                            break;
                        default:
                            {
                                new Error("Expected variable to be series, val, date, string, list, map or matrix");
                                //throw new GekkoException();
                            }
                            break;
                    }
                }
                else if (!isArraySubSeries && varnameWithFreq[0] == Globals.symbolCollection)
                {
                    // ---------------------------------------------------------------------------------------
                    // B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B
                    // B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B
                    // B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B
                    // B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B
                    // LHS variable starts with '#'
                    // B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B
                    // B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B
                    // B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B
                    // B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B B
                    // ---------------------------------------------------------------------------------------
                    // Starts with '#'

                    //smpl.omitDynamicSeries = true;

                    if (lhsType == EVariableType.List || lhsType == EVariableType.Matrix || lhsType == EVariableType.Map || lhsType == EVariableType.Var)
                    {
                        //good
                    }
                    else
                    {
                        new Error("Name '" + varnameWithFreq + "' with '" + Globals.symbolCollection + "' symbol cannot be of " + lhsType.ToString().ToUpper() + " type");
                        //throw new GekkoException();
                    }

                    switch (rhs.Type())
                    {
                        case EVariableType.Series:
                            {
                                Series rhs_series = rhs as Series;
                                switch (rhs_series.type)
                                {
                                    case ESeriesType.Normal:
                                        {
                                            //---------------------------------------------------------
                                            // #x = Series Normal --> not allowed, but MATRIX #m = Series Normal is ok
                                            //---------------------------------------------------------

                                            //if (lhsType == EVariableType.Matrix || lhsType == EVariableType.Var)
                                            if (lhsType == EVariableType.Matrix)
                                            {

                                                // array    smpl          destination
                                                // source
                                                //         
                                                //           o   i1=-1    y 0             --> will become NaN
                                                //   x 0     o            y 1
                                                //   x 1     o            y 2
                                                //   x 2     o            y 3
                                                //   x 3     o            y 4
                                                //           o   i2 = 4   y 5             --> will become NaN
                                                //                                        

                                                //method will only work if smpl freq is same as series freq
                                                int n = smpl.Observations12();
                                                //int i1 = rhs_series.FromGekkoTimeToArrayIndex(smpl.t1);
                                                //int i2 = rhs_series.FromGekkoTimeToArrayIndex(smpl.t2);                                                
                                                //double[] source = rhs_series.GetDataArray();

                                                int i1; int i2;
                                                double[] source = rhs_series.GetDataSequenceUnsafePointerReadOnlyBEWARE(out i1, out i2, smpl.t1, smpl.t2);

                                                Matrix m = new Matrix(1, n);
                                                double[,] destination = m.data;

                                                int destinationStart = 0;

                                                Buffer.BlockCopy(source, 8 * i1, destination, 8 * destinationStart, 8 * (i2 - i1 + 1));
                                                IVariable lhsNew = m;

                                                if (Series.MissingZero(rhs_series)) G.ReplaceNaNWith0(m.data);

                                                AddIvariableWithOverwrite(ib, varnameWithFreq, lhs != null, lhsNew);

                                                G.ServiceMessage("MATRIX " + varnameWithFreq + " updated ", smpl.p);
                                            }
                                            else
                                            {
                                                ReportTypeError(varnameWithFreq, rhs, lhsType);
                                            }
                                        }
                                        break;
                                    case ESeriesType.Light:
                                        {

                                            //---------------------------------------------------------
                                            // #x = Series Light --> not allowed, but MATRIX #m = Series Light is ok
                                            //---------------------------------------------------------

                                            //if (lhsType == EVariableType.Matrix || lhsType == EVariableType.Var)
                                            if (lhsType == EVariableType.Matrix)
                                            {

                                                //method will only work if smpl freq is same as series freq
                                                int n = smpl.Observations12();
                                                Matrix m = new Matrix(1, n);
                                                int ii1 = rhs_series.FromGekkoTimeToArrayIndex(smpl.t1);
                                                int ii2 = rhs_series.FromGekkoTimeToArrayIndex(smpl.t2);

                                                int tooSmall = 0; int tooLarge = 0;
                                                rhs_series.TooSmallOrTooLarge(ii1, ii2, out tooSmall, out tooLarge);
                                                if (tooSmall > 0 || tooLarge > 0)
                                                {
                                                    if (smpl.gekkoError == null) smpl.gekkoError = new GekkoError(tooSmall, tooLarge);
                                                    return;
                                                }

                                                int destinationStart = 0;
                                                double[,] destination = m.data;
                                                double[] source = rhs_series.GetDataSequenceUnsafePointerReadOnlyBEWARE();
                                                //see #0985324985237
                                                Buffer.BlockCopy(source, 8 * ii1, destination, 8 * destinationStart, 8 * (ii2 - ii1 + 1));
                                                IVariable lhsNew = m;
                                                //if (Series.MissingZero()) G.ReplaceNaNWith0(m.data); --> NO! Series light do not get replacement
                                                AddIvariableWithOverwrite(ib, varnameWithFreq, lhs != null, lhsNew);
                                                G.ServiceMessage("MATRIX " + varnameWithFreq + " updated ", smpl.p);
                                            }
                                            else
                                            {
                                                ReportTypeError(varnameWithFreq, rhs, lhsType);
                                            }
                                        }
                                        break;
                                    case ESeriesType.Timeless:
                                        {
                                            //---------------------------------------------------------
                                            // #x = Series Timeless --> not allowed, but MATRIX #m = Series Timeless is ok
                                            //---------------------------------------------------------

                                            //if (lhsType == EVariableType.Matrix || lhsType == EVariableType.Var)
                                            if (lhsType == EVariableType.Matrix)
                                            {
                                                int n = smpl.Observations12();
                                                double d = rhs_series.GetDataSequenceUnsafePointerAlterBEWARE()[0];
                                                if (Series.MissingZero(rhs_series) && G.isNumericalError(d)) d = 0d;
                                                Matrix m = new Matrix(1, n, d);  //expanded as if it was a real timeseries                                       
                                                AddIvariableWithOverwrite(ib, varnameWithFreq, lhs != null, m);
                                                G.ServiceMessage("MATRIX " + varnameWithFreq + " updated ", smpl.p);
                                            }
                                            else
                                            {
                                                ReportTypeError(varnameWithFreq, rhs, lhsType);
                                            }
                                        }
                                        break;
                                    case ESeriesType.ArraySuper:
                                        {
                                            //---------------------------------------------------------
                                            // #x = Series Array Super
                                            //---------------------------------------------------------
                                            {
                                                ReportTypeError(varnameWithFreq, rhs, lhsType);
                                            }
                                        }
                                        break;
                                    default:
                                        {
                                            new Error("Expected SERIES to be 1 of 4 types");
                                            //throw new GekkoException();
                                        }
                                        break;
                                }
                            }
                            break;
                        case EVariableType.Val:
                            {
                                //---------------------------------------------------------
                                // #x = VAL
                                //---------------------------------------------------------
                                ReportTypeError(varnameWithFreq, rhs, lhsType);
                            }
                            break;
                        case EVariableType.String:
                            {
                                //---------------------------------------------------------
                                // #x = STRING
                                //---------------------------------------------------------

                                ReportTypeError(varnameWithFreq, rhs, lhsType);

                            }
                            break;
                        case EVariableType.Date:
                            {
                                //---------------------------------------------------------
                                // #x = DATE
                                //---------------------------------------------------------

                                ReportTypeError(varnameWithFreq, rhs, lhsType);

                            }
                            break;
                        case EVariableType.List:
                            {
                                //---------------------------------------------------------
                                // #x = LIST
                                //---------------------------------------------------------         
                                if (lhsType == EVariableType.List || lhsType == EVariableType.Var)
                                {
                                    AddIvariableWithOverwrite(ib, varnameWithFreq, lhs != null, rhs.DeepClone(null));
                                    G.ServiceMessage("LIST " + varnameWithFreq + " updated ", smpl.p);
                                }
                                else
                                {
                                    ReportTypeError(varnameWithFreq, rhs, lhsType);
                                }
                            }
                            break;
                        case EVariableType.Map:
                            {
                                //---------------------------------------------------------
                                // #x = MAP
                                //---------------------------------------------------------

                                if (lhsType == EVariableType.Map || lhsType == EVariableType.Var)
                                {
                                    AddIvariableWithOverwrite(ib, varnameWithFreq, lhs != null, rhs.DeepClone(null));
                                    G.ServiceMessage("MAP " + varnameWithFreq + " updated ", smpl.p);
                                }
                                else
                                {
                                    ReportTypeError(varnameWithFreq, rhs, lhsType);
                                }
                            }
                            break;
                        case EVariableType.Matrix:
                            {
                                //---------------------------------------------------------
                                // #x = MATRIX
                                //---------------------------------------------------------
                                if (lhsType == EVariableType.Matrix || lhsType == EVariableType.Var)
                                {
                                    Matrix m = rhs.DeepClone(null) as Matrix;
                                    if (o.opt_colnames != null) m.colnames = new List<string>(Stringlist.GetListOfStringsFromListOfIvariables(O.ConvertToList(o.opt_colnames).ToArray()));
                                    if (o.opt_rownames != null) m.rownames = new List<string>(Stringlist.GetListOfStringsFromListOfIvariables(O.ConvertToList(o.opt_rownames).ToArray()));
                                    AddIvariableWithOverwrite(ib, varnameWithFreq, lhs != null, m);
                                    G.ServiceMessage("MATRIX " + varnameWithFreq + " updated ", smpl.p);
                                }
                                else
                                {
                                    ReportTypeError(varnameWithFreq, rhs, lhsType);
                                }
                            }
                            break;
                        default:
                            {
                                new Error("Expected IVariable to be 1 of 7 types");
                                //throw new GekkoException();
                            }
                            break;
                    }
                }
                else
                {
                    // -------------------------------------------------------------------------------
                    // C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C
                    // C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C
                    // C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C
                    // C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C
                    // LHS variable has no sigil/symbol as first character (or isArraySubSeries == true)
                    // C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C
                    // C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C
                    // C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C
                    // C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C C
                    // -------------------------------------------------------------------------------
                    //name is of series type (no sigils), or we have that isArraySubSeries == true (or both)

                    if (lhs == null && !isArraySubSeries && !varnameWithFreq.StartsWith("xx", StringComparison.OrdinalIgnoreCase))
                    {
                        //nonexisting series
                        if (!Program.options.databank_create_auto)
                        {
                            //#07549843254
                            using (Error e = new Error())
                            {
                                e.MainAdd("Cannot auto-create series " + varnameWithFreq + ". See the CREATE command.");
                                e.MainAdd("You may change the settings with the following option:");
                                e.MainNewLineTight();
                                e.MainAdd("OPTION databank create auto = yes;");
                                e.MainNewLineTight();
                                e.MainAdd("Alternatively, use 'MODE data;' or 'MODE mixed;'.");
                            }                            
                        }
                    }

                    //The indicated LHS type can only be series or var type, for instance SERIES x = ...  or VAR x = ...  or x = ...  . 
                    if (lhsType == EVariableType.Series || lhsType == EVariableType.Var)
                    {
                        //good
                    }
                    else
                    {
                        new Error("Name '" + varnameWithFreq + "' without '" + Globals.symbolScalar + "' or '" + Globals.symbolCollection + "' symbol cannot be of " + lhsType.ToString().ToUpper() + " type");
                        //throw new GekkoException();
                    }

                    //Now we know that it is either SERIES x = ...  or VAR x = ...  or x = ...   

                    bool removeFirst = true;

                    Series lhs_series = null;
                    if (isArraySubSeries) lhs_series = arraySubSeries;
                    else lhs_series = lhs as Series;

                    //TODO: error if more than 1 is set
                    ESeriesUpdTypes operatorType = GetOperatorType(o);
                    bool keep = false; if (o != null && G.Equal(o.opt_keep, "p")) keep = true;

                    Series original = null;
                    if (keep || false)
                    {
                        original = (Series)lhs_series.DeepClone(null);
                    }

                    bool create = CreateSeriesIfNotExisting(varnameWithFreq, freq, ref lhs_series);

                    if (!isArraySubSeries)
                    {
                        lhs_series.meta.stamp = Program.GetDateStamp();
                        if (o?.opt_label != null) lhs_series.meta.label = O.ConvertToString(o.opt_label);
                        if (o?.opt_source != null) lhs_series.meta.source = O.ConvertToString(o.opt_source);
                        if (o?.opt_units != null) lhs_series.meta.units = O.ConvertToString(o.opt_units);
                        if (o?.opt_stamp != null) lhs_series.meta.stamp = O.ConvertToString(o.opt_stamp);  //will override

                        if (!G.NullOrEmpty(lhs_series.meta.source))
                        {
                            if (lhs_series.meta.source.StartsWith("<[code]>"))
                            {
                                lhs_series.meta.source = smpl.t1.ToString() + "-" + smpl.t2.ToString() + ": " + lhs_series.meta.source.Replace("<[code]>", "");
                            }
                        }
                    }

                    switch (rhs.Type())
                    {
                        case EVariableType.Series:
                            {
                                Series rhs_series_beware = rhs as Series;
                                string freq_rhs = G.ConvertFreq(rhs_series_beware.freq);
                                if (varnameWithFreq != null && !varnameWithFreq.ToLower().EndsWith(Globals.freqIndicator + freq_rhs))  //null if it is a subseries under an array-superseries
                                {
                                    new Error("Frequency: illegal series name '" + varnameWithFreq + "', should end with '" + Globals.freqIndicator + freq_rhs + "'");
                                    //throw new GekkoException();
                                }

                                if (Program.options.series_dyn_check)
                                {
                                    if (CheckDyn2(o))
                                    {
                                        //Neither <dyn> nor BLOCK series dyn have not been set
                                        //options can be == null, in that case there is no <...>-field
                                        if (Globals.precedentsSeries != null)
                                        {
                                            if (Globals.precedentsSeries.ContainsKey(lhs_series))
                                            {
                                                int obs = smpl.Observations12();
                                                if (obs > 1)
                                                {
                                                    G.Writeln2("*** ERROR: It seems the left-hand side variable appears with a lag on the right-hand side.");
                                                    G.Writeln("           When 'OPTION series dyn check = yes' (default), in such dynamic statements you", Color.Red);
                                                    G.Writeln("           must use <dyn> or <dyn = no> tags, or put the expression inside a", Color.Red);
                                                    G.Writeln("           'BLOCK series dyn = yes|no'.", Color.Red);
                                                    G.Writeln();
                                                    Action<GAO> a = (gao) =>
                                                    {
                                                        O.Help("i_dynamic_statements");
                                                    };
                                                    G.Writeln("           Read more about this error " + G.GetLinkAction("here", new GekkoAction(EGekkoActionTypes.Unknown, null, a)) + ". If you are uprading from a Gekko version < 3.1.7 to a", Color.Red);
                                                    G.Writeln("           Gekko version >= 3.1.7, this error may come out of the blue. In that case, see the", Color.Red);
                                                    G.Writeln("           'Backwards incompatibility, or how to ignore' section in the above link.", Color.Red);
                                                    G.Writeln();
                                                    throw new GekkoException();
                                                }
                                            }
                                        }
                                    }
                                }

                                switch (rhs_series_beware.type)
                                {

                                    case ESeriesType.Normal:
                                    case ESeriesType.Light:
                                        {
                                            //---------------------------------------------------------
                                            // x = Series Normal or Light
                                            //---------------------------------------------------------

                                            if (operatorType == ESeriesUpdTypes.none || operatorType == ESeriesUpdTypes.n)
                                            {
                                                //this runs fast

                                                GekkoTime tt1 = GekkoTime.tNull;
                                                GekkoTime tt2 = GekkoTime.tNull;
                                                GekkoTime.ConvertFreqs(G.ConvertFreq(freq, true), smpl.t1, smpl.t2, ref tt1, ref tt2);  //converts smpl.t1 and smpl.t2 to tt1 and tt2 in freq frequency
                                                                                                                                        //bool create = CreateSeriesIfNotExisting(varnameWithFreq, freq, ref lhs_series);
                                                                                                                                        //Now the smpl window runs from tt1 to tt2
                                                                                                                                        //We copy in from that window
                                                if (lhs_series.freq != rhs_series_beware.freq)
                                                {
                                                    new Error("Frequency mismatch. Left-hand series is " + G.GetFreqPretty(lhs_series.freq) + ", whereas right-hand series is " + G.GetFreqPretty(lhs_series.freq));

                                                    //throw new GekkoException();
                                                }

                                                if (rhs_series_beware.type == ESeriesType.Light)
                                                {
                                                    int tooSmall = 0; int tooLarge = 0;
                                                    rhs_series_beware.TooSmallOrTooLarge(rhs_series_beware.GetArrayIndex(tt1), rhs_series_beware.GetArrayIndex(tt2), out tooSmall, out tooLarge);
                                                    if (tooSmall > 0 || tooLarge > 0)
                                                    {
                                                        if (smpl.gekkoError == null) smpl.gekkoError = new GekkoError(tooSmall, tooLarge);
                                                        return;
                                                    }
                                                }

                                                int index1, index2;
                                                //may enlarge the array with NaNs first and last
                                                double[] data_beware_do_not_alter = rhs_series_beware.GetDataSequenceUnsafePointerReadOnlyBEWARE(out index1, out index2, tt1, tt2);
                                                lhs_series.SetDataSequence(tt1, tt2, data_beware_do_not_alter, index1, Series.MissingZero(rhs_series_beware));
                                            }
                                            else
                                            {
                                                //not so fast running, could be improved
                                                OperatorHelperSeries(smpl, lhs_series, rhs_series_beware, operatorType);
                                            }
                                            //G.ServiceMessage("SERIES " + G.GetNameAndFreqPretty(varnameWithFreq, false) + " updated " + smpl.t1 + "-" + smpl.t2 + " ", smpl.p);
                                            LookupHelperLeftside_message(smpl, lhs_series.freq, varnameWithFreq);
                                        }
                                        break;
                                    case ESeriesType.Timeless:
                                        {
                                            //---------------------------------------------------------
                                            // x = Series Timeless
                                            //---------------------------------------------------------
                                            // stuff below also handles array-timeseries just fine  

                                            if (create)
                                            {
                                                lhs_series = rhs_series_beware.DeepClone(null) as Series;  //so that it becomes timeless, too                                                
                                                lhs_series.name = varnameWithFreq; ;
                                                double[] temp = lhs_series.GetDataSequenceUnsafePointerAlterBEWARE();  //sets dirty, but it *is* dirty
                                                if (Series.MissingZero(rhs_series_beware) && G.isNumericalError(temp[0]))
                                                {
                                                    temp[0] = 0d;
                                                }
                                            }
                                            else
                                            {
                                                double d = double.NaN;
                                                if (rhs_series_beware.GetDataSequenceUnsafePointerReadOnlyBEWARE() != null) d = rhs_series_beware.GetDataSequenceUnsafePointerReadOnlyBEWARE()[0];
                                                if (Series.MissingZero(rhs_series_beware) && G.isNumericalError(d))
                                                {
                                                    d = 0d;
                                                }

                                                if (operatorType == ESeriesUpdTypes.none || operatorType == ESeriesUpdTypes.n)
                                                {
                                                    if (O.UseFlexFreq(smpl.t1, lhs_series.freq))
                                                    {
                                                        foreach (GekkoTime t in smpl.Iterate12(lhs_series.freq))
                                                        {
                                                            lhs_series.SetData(t, d);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        foreach (GekkoTime t in smpl.Iterate12())
                                                        {
                                                            lhs_series.SetData(t, d);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    OperatorHelperScalar(smpl, lhs_series, operatorType, d);
                                                }
                                            }
                                            //G.ServiceMessage("SERIES " + G.GetNameAndFreqPretty(varnameWithFreq, false) + " updated " + smpl.t1 + "-" + smpl.t2 + " ", smpl.p);
                                            LookupHelperLeftside_message(smpl, lhs_series.freq, varnameWithFreq);

                                        }
                                        break;
                                    case ESeriesType.ArraySuper:
                                        {
                                            //---------------------------------------------------------
                                            // x = Series Array Super
                                            //---------------------------------------------------------

                                            create = true;  //always create a fresh one, if there is an array-series on the RHS. Does not make sense to merge into existing array-series

                                            if (isArraySubSeries)
                                            {
                                                new Error("You cannot put an array-series inside an array-series");
                                                //throw new GekkoException();
                                            }

                                            if (operatorType != ESeriesUpdTypes.none && operatorType != ESeriesUpdTypes.n)
                                            {
                                                new Error("Operators cannot be used for array-series (yet)");
                                                //throw new GekkoException();
                                            }

                                            lhs_series = rhs.DeepClone(null) as Series;
                                            lhs_series.name = varnameWithFreq;
                                            //!we need to make all the subseries point to the superseries, this pointer is used in DECOMP and other places
                                            foreach (KeyValuePair<MultidimItem, IVariable> kvp in lhs_series.dimensionsStorage.storage)
                                            {
                                                kvp.Key.parent = lhs_series;
                                                (kvp.Value as Series).mmi.parent = lhs_series;
                                            }
                                            removeFirst = lhs != null;
                                            //lhs_series = clone;
                                            //AddIvariableWithOverwrite(ib, varnameWithFreq, lhs != null, clone);
                                            //G.ServiceMessage("SERIES " + G.GetNameAndFreqPretty(varnameWithFreq, false) + " updated " + smpl.t1 + "-" + smpl.t2 + " ", smpl.p);
                                            LookupHelperLeftside_message(smpl, lhs_series.freq, varnameWithFreq);
                                        }
                                        break;
                                    default:
                                        {
                                            new Error("Expected SERIES to be 1 of 4 types");
                                            //throw new GekkoException();
                                        }
                                        break;
                                }
                            }
                            break;
                        case EVariableType.Val:
                            {
                                //---------------------------------------------------------
                                // x = VAL
                                //---------------------------------------------------------       
                                // stuff below also handles array-timeseries just fine                     
                                double d = ((ScalarVal)rhs).val;
                                //bool create = CreateSeriesIfNotExisting(varnameWithFreq, freq, ref lhs_series);

                                if (operatorType == ESeriesUpdTypes.none || operatorType == ESeriesUpdTypes.n)
                                {
                                    //this is very similar to the same code regarding 1 x 1 MATRIX
                                    if (O.UseFlexFreq(smpl.t1, lhs_series.freq))
                                    {
                                        //different freqs, for instance x!q = 2 when global freq is !a                                        
                                        foreach (GekkoTime t in smpl.Iterate12(lhs_series.freq)) lhs_series.SetData(t, d);
                                    }
                                    else
                                    {
                                        //same freq
                                        foreach (GekkoTime t in smpl.Iterate12()) lhs_series.SetData(t, d);
                                    }
                                }
                                else
                                {
                                    OperatorHelperScalar(smpl, lhs_series, operatorType, d);
                                }

                                //G.ServiceMessage("SERIES " + G.GetNameAndFreqPretty(varnameWithFreq, false) + " updated " + smpl.t1 + "-" + smpl.t2 + " ", smpl.p);
                                LookupHelperLeftside_message(smpl, lhs_series.freq, varnameWithFreq);

                            }
                            break;

                        case EVariableType.Date:
                            {
                                //---------------------------------------------------------
                                // x = DATE
                                //---------------------------------------------------------
                                {
                                    ReportTypeError(varnameWithFreq, rhs, lhsType);
                                }
                            }
                            break;
                        case EVariableType.String:
                            {
                                //---------------------------------------------------------
                                // x = STRING
                                //---------------------------------------------------------
                                {
                                    ReportTypeError(varnameWithFreq, rhs, lhsType, 1);
                                }
                            }
                            break;
                        case EVariableType.List:
                            {
                                //---------------------------------------------------------
                                // x = LIST
                                //---------------------------------------------------------
                                // stuff below also handles array-timeseries just fine 

                                List rhs_list = rhs as List;

                                HelperListdata(smpl, lhs_series, operatorType, rhs_list);

                                //G.ServiceMessage("SERIES " + G.GetNameAndFreqPretty(varnameWithFreq, false) + " updated " + smpl.t1 + "-" + smpl.t2 + " ", smpl.p);
                                LookupHelperLeftside_message(smpl, lhs_series.freq, varnameWithFreq);
                            }
                            break;
                        case EVariableType.Map:
                            {
                                //---------------------------------------------------------
                                // x = MAP
                                //---------------------------------------------------------
                                {
                                    ReportTypeError(varnameWithFreq, rhs, lhsType);
                                }
                            }
                            break;
                        case EVariableType.Matrix:
                            {
                                //---------------------------------------------------------
                                // x = MATRIX
                                //---------------------------------------------------------

                                // stuff below also handles array-timeseries just fine     

                                Matrix rhs_matrix = rhs as Matrix;

                                if (rhs_matrix.data.Length == 1)
                                {
                                    double d = rhs.ConvertToVal();  //will fail with error if not 1x1                            

                                    if (operatorType == ESeriesUpdTypes.none || operatorType == ESeriesUpdTypes.n)
                                    {
                                        //this is very similar to the same code regarding VAL
                                        if (O.UseFlexFreq(smpl.t1, lhs_series.freq))
                                        {
                                            //different freqs, for instance x!q = 2 when global freq is !a                                        
                                            foreach (GekkoTime t in smpl.Iterate12(lhs_series.freq)) lhs_series.SetData(t, d);
                                        }
                                        else
                                        {
                                            //same freq
                                            foreach (GekkoTime t in smpl.Iterate12()) lhs_series.SetData(t, d);
                                        }

                                    }
                                    else
                                    {
                                        OperatorHelperScalar(smpl, lhs_series, operatorType, d);
                                    }
                                }
                                else
                                {
                                    GekkoTime t1 = smpl.t1;
                                    GekkoTime t2 = smpl.t2;
                                    if (O.UseFlexFreq(t1, lhs_series.freq)) O.Helper_Convert12(smpl, lhs_series.freq, out t1, out t2);
                                    int n = GekkoTime.Observations(t1, t2);

                                    if (n != rhs_matrix.data.GetLength(0) || 1 != rhs_matrix.data.GetLength(1))
                                    {
                                        new Error("Expected " + n + " x 1 matrix, got " + rhs_matrix.data.GetLength(0) + " x " + rhs_matrix.data.GetLength(1));
                                        //throw new GekkoException();
                                    }

                                    if (operatorType == ESeriesUpdTypes.none || operatorType == ESeriesUpdTypes.n)
                                    {
                                        for (int i = 0; i < rhs_matrix.data.GetLength(0); i++)
                                        {
                                            lhs_series.SetData(t1.Add(i), rhs_matrix.data[i, 0]);
                                        }
                                    }
                                    else
                                    {
                                        //rhs_matrix.data[i, 0]

                                        //int offset = 1;                                    
                                        double[] rhsData = new double[n + Globals.smplOffset];
                                        for (int i = 0; i < n; i++)
                                        {
                                            rhsData[i + Globals.smplOffset] = rhs_matrix.data[i, 0];
                                        }
                                        for (int i = 0; i < Globals.smplOffset; i++)
                                        {
                                            //just safety, probably not necessary
                                            rhsData[i] = double.NaN;
                                        }
                                        OperatorHelperSequence(smpl, lhs_series, rhsData, operatorType);
                                    }
                                }

                                LookupHelperLeftside_message(smpl, lhs_series.freq, varnameWithFreq);

                            }
                            break;
                        default:
                            {
                                new Error("Expected IVariable to be 1 of 7 types");
                                //throw new GekkoException();
                            }
                            break;
                    }  //end switch

                    if (create)
                    {
                        AddIvariableWithOverwrite(ib, varnameWithFreq, removeFirst, lhs_series);
                    }
                    else
                    {
                        //nothing to do, either already existing in bank/map or array-subseries
                    }

                    if (keep)
                    {
                        GekkoTime tLast = lhs_series.GetRealDataPeriodLast();

                        GekkoTime t3 = smpl.t3; //why t3 and not t2? Never mind, t2 and t3 are equal most of the time
                        if (O.UseFlexFreq(t3, lhs_series.freq)) t3 = GekkoTime.ConvertFreqsLast(lhs_series.freq, t3);

                        foreach (GekkoTime t in new GekkoTimeIterator(t3.Add(1), tLast))
                        {
                            //runs after the <...> period or globals period until data ends
                            //so the updates outside of sample.
                            double rel = original.GetData(smpl, t) / original.GetData(smpl, t.Add(-1));
                            lhs_series.SetData(t, lhs_series.GetData(smpl, t.Add(-1)) * rel);
                        }
                    }

                    if (Program.options.series_failsafe)
                    {
                        //only for debugging                        
                        ReportSeriesMissingValue(lhs_series, smpl.t1, smpl.t2);
                    }
                }
            }

            return;

        }

        private static void LookupHelperLeftside_message(GekkoSmpl smpl, EFreq lhs_series_freq, string varnameWithFreq)
        {
            string s;
            GekkoTime t1 = smpl.t1;
            GekkoTime t2 = smpl.t2;
            if (O.UseFlexFreq(t1, lhs_series_freq)) O.Helper_Convert12(smpl, lhs_series_freq, out t1, out t2);
            s = t1 + "-" + t2;
            G.ServiceMessage("SERIES " + G.GetNameAndFreqPretty(varnameWithFreq, false) + " updated " + s + " ", smpl.p);
        }

        /// <summary>
        /// Find an IVariable in a particular databank.
        /// </summary>
        /// <param name="varnameWithFreq"></param>
        /// <param name="settings"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        private static IVariable LookupHelperFindVariableInSpecificBank(string varnameWithFreq, LookupSettings settings, Databank db)
        {
            bool create = false;
            IVariable rv = db.GetIVariable(varnameWithFreq);
            if (rv == null)
            {
                if (settings.create == ECreatePossibilities.NoneReturnNull) return rv;
                if (settings.create == ECreatePossibilities.NoneReportError)
                {
                    new Error("Could not find variable " + G.GetNameAndFreqPretty(varnameWithFreq) + " in databank '" + db.name + "'");
                    //throw new GekkoException();
                }
                else if (settings.create == ECreatePossibilities.Must || settings.create == ECreatePossibilities.Can)
                {
                    if (G.Chop_HasSigil(varnameWithFreq))
                    {
                        new Error("Internal error #982437532");
                        //throw new GekkoException();
                    }
                    else
                    {
                        //series
                        create = true;
                    }
                }
            }
            else
            {
                if (settings.create == ECreatePossibilities.Must) create = true;
            }
            if (create)
            {
                rv = new Series(G.ConvertFreq(G.Chop_GetFreq(varnameWithFreq)), varnameWithFreq);  //brand new
                db.AddIVariableWithOverwrite(rv);
            }
            return rv;
        }

        /// <summary>
        /// Helper regarding Global and Local databank, for use with Lookup() code.
        /// </summary>
        /// <param name="lg"></param>
        /// <returns></returns>
        private static Databank HandleLocalGlobalBank(LocalGlobal.ELocalGlobalType lg)
        {
            Databank db;
            if (lg == LocalGlobal.ELocalGlobalType.None)
            {
                db = Program.databanks.GetFirst();
            }
            else if (lg == LocalGlobal.ELocalGlobalType.Local)
            {
                db = Program.databanks.local;
            }
            else if (lg == LocalGlobal.ELocalGlobalType.Global)
            {
                db = Program.databanks.global;
            }
            else
            {
                new Error("#8097432857"); return null;
                //throw new GekkoException();
            }

            return db;
        }

        private static void HelperListdata(GekkoSmpl smpl, Series lhs_series, ESeriesUpdTypes operatorType, List rhs_list)
        {
            GekkoTime t1 = smpl.t1;
            GekkoTime t2 = smpl.t2;
            if (O.UseFlexFreq(t1, lhs_series.freq)) O.Helper_Convert12(smpl, lhs_series.freq, out t1, out t2);

            bool lastElementStar = false;
            IVariable last = rhs_list.list[rhs_list.list.Count - 1];
            ScalarVal last_val = last as ScalarVal;
            if (last_val != null)
            {
                lastElementStar = last_val.hasRepStar;
            }

            int n = GekkoTime.Observations(t1, t2);

            if (rhs_list.list.Count < n)
            {
                //lacking elements
                if (!lastElementStar)
                {
                    new Error("Expected " + n + " list items, got " + rhs_list.list.Count);
                    //throw new GekkoException();
                }
            }
            else if (rhs_list.list.Count > n)
            {
                new Error("Expected " + n + " list items, got " + rhs_list.list.Count);
                //throw new GekkoException();
            }

            //int offset = 1;
            double[] rhs_data = new double[n + Globals.smplOffset];
            for (int i = 0; i < rhs_list.list.Count; i++)
            {
                rhs_data[i + Globals.smplOffset] = rhs_list.list[i].ConvertToVal();
            }
            for (int i = 0; i < Globals.smplOffset; i++)
            {
                rhs_data[i] = double.NaN;
            }

            if (rhs_list.list.Count < n)
            {
                //then lastElementStar = true
                for (int i = rhs_list.list.Count; i < n; i++)
                {
                    rhs_data[i + Globals.smplOffset] = rhs_list.list[rhs_list.list.Count - 1].ConvertToVal();
                }
            }

            if (operatorType == ESeriesUpdTypes.none || operatorType == ESeriesUpdTypes.n)
            {
                for (int i = 0; i < n; i++)
                {
                    lhs_series.SetData(t1.Add(i), rhs_data[i + Globals.smplOffset]);
                }
            }
            else
            {
                OperatorHelperSequence(smpl, lhs_series, rhs_data, operatorType);
            }
        }

        private static bool CreateSeriesIfNotExisting(string varnameWithFreq, string freq, ref Series lhs_series)
        {
            bool create = false;
            if (lhs_series != null && (lhs_series.type == ESeriesType.Normal || lhs_series.type == ESeriesType.Timeless))
            {
                //do nothing, use it
            }
            else
            {
                //create it
                create = true;
                lhs_series = new Series(ESeriesType.Normal, G.ConvertFreq(freq, true), varnameWithFreq);
            }

            return create;
        }

        private static void ReportTypeError(string varnameWithFreq, IVariable rhs, EVariableType type)
        {
            ReportTypeError(varnameWithFreq, rhs, type, 0);
        }

        private static void ReportTypeError(string varnameWithFreq, IVariable rhs, EVariableType type, int extra)
        {
            string s = null;
            if (extra == 1)
            {
                s+= Globals.stringConversionNote;
            }
            new Error("" + type.ToString().ToUpper() + " " + varnameWithFreq + " has a " + rhs.Type().ToString().ToUpper() + " on right-hand side. " + s);
        }

        private static bool IsAllSpecialDatabank(string dbName)
        {
            return G.Equal(dbName, Globals.All);
        }

        // --------------------------------------------------------------------------------------------------------------------------------------------
        // LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END 
        // LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END 
        // LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END 
        // LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END 
        // LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END 
        // LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END LOOKUP END 
        // --------------------------------------------------------------------------------------------------------------------------------------------

    }
}
