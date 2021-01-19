using System;
using System.Collections.Generic;
using System.Text;
using Antlr.Runtime;
using Antlr.Runtime.Tree;
using Antlr.Runtime.Debug;
using Microsoft.CSharp;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Reflection;
using CT = Antlr.Runtime.Tree.CommonTree;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Collections;
using System.Security.Cryptography;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace Gekko.Parser.Frm
{
    class ParserFrmCompileAST
    {
        public static void ParserFrmOrderAndCompileAST(ECompiledModelType modelType, bool isCalledFromModelStatement, bool isFix)
        {
            //seems modelType is not used at all
            bool newM2 = false;
            DateTime t0 = DateTime.Now;
            string cacheKey = GetCacheKey(isFix);

            if (Program.model.modelGekko.m2cache.lru.ContainsKey(cacheKey))  //MODEL statement should always issue a real compile, because in that case, Program.model.modelGekko.m2 is newly created
            {
                Program.model.modelGekko.m2 = (Model2)Program.model.modelGekko.m2cache.lru[cacheKey];
                G.WritelnGray("¤¤¤ Got in cache: " + cacheKey);
            }
            else
            {

                G.WritelnGray("¤¤¤ Has to do .m2 stuff: " + cacheKey);
                newM2 = true;
                Program.model.modelGekko.m2 = new Model2();  //deleting everything here, this is most safe rather than reusing the object

                //this runs very fast
                if (Globals.stackedPrintTimings) G.Writeln2("EndogenizeExogenizeStuff start");
                Program.EndogenizeExogenizeStuff(isFix); //depends upon which endo/exo variables are set
                if (Globals.stackedPrintTimings) G.Writeln2("EndogenizeExogenizeStuff end");
                //takes about 0.6 sec on dec09
                if (Globals.stackedPrintTimings) G.Writeln2("FeedbackOrderingStuff start");
                Program.FeedbackOrderingStuff(modelType, isCalledFromModelStatement); //depends upon which endo/exo variables are set
                if (Globals.stackedPrintTimings) G.Writeln2("FeedbackOrderingStuff end");
            }

            //The .m2 object is in principle recreated each time this method is called (for instance because of ENDO/EXO statement),
            //or with a MODEL statement. But since there is a cache, it will often be found there if it is because of ENDO/EXO.
            //For each modelType (Gauss, GaussFailSafe, Res, Newton, After, Unknown) there is a dedicated .dll in .m2. If
            //this is missing, it will be made in the method below.
            //The way this is done now is more robust, since it will be impossible to obtain a .dll the does not have the
            //corresponding ENDO/EXO vars set. This could be a problem before, for instance doing a ENDO/EXO, and then
            //afterwards Gauss-simulation with failsafe on.

            if (modelType == ECompiledModelType.GaussFailSafe)
            {
                EmitCsCodeAndCompileModel(ECompiledModelType.Gauss, isCalledFromModelStatement);  //This method is only called from here
            }

            //if (Globals.stackedPrintTimings) G.Writeln2("Emit start");
            EmitCsCodeAndCompileModel(modelType, isCalledFromModelStatement);  //This method is only called from here
            //if (Globals.stackedPrintTimings) G.Writeln2("Emit end");

            G.WritelnGray("¤¤¤ Hash: " + cacheKey);

            if (isCalledFromModelStatement) PrintInfoFilesCreateVarsEtc(isCalledFromModelStatement);  //so the "endogenous" are endogenous in original model without ENDO/EXO.

            if (newM2) Program.model.modelGekko.m2cache.lru.Add(cacheKey, Program.model.modelGekko.m2);
        }

        public static void ParserFrmMakeProtobuf()
        {
            try //not the end of world if it fails (should never be done if model is read from zipped protobuffer (would be waste of time))
            {
                DateTime dt1 = DateTime.Now;

                PutListsIntoModelListHelper();

                //May take a little time to create: so use static serializer if doing serialize on a lot of small objects
                RuntimeTypeModel serializer = TypeModel.Create();
                serializer.UseImplicitZeroDefaults = false;  //otherwise an int that has default constructor value -12345 but is set to 0 will reappear as a -12345 (instead of 0). For int, 0 is default, false for bools etc.


                // ----- SERIALIZE
                //string outputPath = Globals.localTempFilesLocation;
                //DeleteFolder(outputPath);
                //Directory.CreateDirectory(outputPath);
                string protobufFileName = Globals.gekkoVersion + "_" + Program.model.modelGekko.modelHashTrue + ".mdl";
                string pathAndFilename = Globals.localTempFilesLocation + "\\" + protobufFileName;
                using (FileStream fs = Program.WaitForFileStream(pathAndFilename, Program.GekkoFileReadOrWrite.Write))
                {
                    //Serializer.Serialize(fs, m);
                    serializer.Serialize(fs, Program.model.modelGekko);
                }
                //Program.WaitForZipWrite(outputPath, Globals.localTempFilesLocation + "\\" + protobufFileName);
                G.WritelnGray("Created model cache file in " + G.SecondsFormat((DateTime.Now - dt1).TotalMilliseconds));
            }
            catch (Exception e)
            {
                //do nothing, not the end of the world if it fails
            }
        }

    }
}
