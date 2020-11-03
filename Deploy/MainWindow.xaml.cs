using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Reflection;
using System.Diagnostics;


namespace Deploy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //public static string zip = @"c:\Thomas\Gekko\GekkoCS\Gekko\bin\Debug\zip\7z.dll";
        //public static string zip = @"c:\Thomas\Gekko\GekkoCS\Diverse\FilesUsedForDeployment\7z.dll";
        //For some reason, the full 7z files need to be present in the folder, whereas for the Gekko installation, this
        //is not necessary. A mystery ... so we had to install z-zip to make it work. Never mind.
        //public static string zip = @"c:\Program Files\7-Zip\7z.dll";
        public static string tools = @"c:\tools\tmp\Gekko_files";

        public MainWindow()
        {
            InitializeComponent();
            this.Top = 10;
            this.Left = 10;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            File.Copy(@"c:\Thomas\Gekko\dok\user\Gekko.chm", @"c:\Thomas\Gekko\GekkoCS\Gekko\bin\Debug\helpfiles\Gekko.chm", true);
            MessageBox.Show("Copied Gekko.chm");
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FileInfo fi = new FileInfo(@"c:\Thomas\Gekko\GekkoCS\InstallerForGekko\Release32bit\InstallerForGekko.msi");
                double min = (DateTime.Now - fi.LastWriteTime).TotalMinutes;
                if (min > 15)
                {
                    MessageBox.Show("ERROR: the .msi file seems too old (> 15 minutes)");
                    return;
                }
                System.Diagnostics.Process.Start(@"c:\Thomas\Gekko\GekkoCS\InstallerForGekko\Release32bit\InstallerForGekko.msi");                
            }
            catch
            {
                MessageBox.Show("*** ERROR: Installation failed");
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            try
            {                
                File.Copy(@"c:\Thomas\Gekko\GekkoCS\InstallerForGekko\Release32bit\InstallerForGekko.msi", tools + @"\InstallerForGekko.msi", true);
                File.Copy(@"c:\Thomas\Gekko\GekkoCS\InstallerForGekko\Release32bit\Setup.exe", tools + @"\Setup.exe", true);
                MessageBox.Show(@"Copying 2 files ok");
            }
            catch
            {
                MessageBox.Show("*** ERROR: Copying 2 files failed");
            }           
            
        }

        public void IsJit()
        {
            var HasDebuggableAttribute = false;
            var IsJITOptimized = false;
            var IsJITTrackingEnabled = false;
            var BuildType = "";
            var DebugOutput = "";
            var ReflectedAssembly = Assembly.LoadFile(@"c:\Program Files (x86)\Gekko\Gekko.exe");

            //	var ReflectedAssembly = Assembly.LoadFile(@"path to the dll you are testing");
            object[] attribs = ReflectedAssembly.GetCustomAttributes(typeof(DebuggableAttribute), false);

            // If the 'DebuggableAttribute' is not found then it is definitely an OPTIMIZED build
            if (attribs.Length > 0)
            {
                // Just because the 'DebuggableAttribute' is found doesn't necessarily mean
                // it's a DEBUG build; we have to check the JIT Optimization flag
                // i.e. it could have the "generate PDB" checked but have JIT Optimization enabled
                DebuggableAttribute debuggableAttribute = attribs[0] as DebuggableAttribute;
                if (debuggableAttribute != null)
                {
                    HasDebuggableAttribute = true;
                    IsJITOptimized = !debuggableAttribute.IsJITOptimizerDisabled;

                    // IsJITTrackingEnabled - Gets a value that indicates whether the runtime will track information during code generation for the debugger.
                    IsJITTrackingEnabled = debuggableAttribute.IsJITTrackingEnabled;
                    BuildType = debuggableAttribute.IsJITOptimizerDisabled ? "Debug" : "Release";

                    // check for Debug Output "full" or "pdb-only"
                    DebugOutput = (debuggableAttribute.DebuggingFlags &
                                    DebuggableAttribute.DebuggingModes.Default) !=
                                    DebuggableAttribute.DebuggingModes.None
                                    ? "Full" : "pdb-only";
                }
            }
            else
            {
                IsJITOptimized = true;
                BuildType = "Release";
            }

            Console.WriteLine($"{nameof(HasDebuggableAttribute)}: {HasDebuggableAttribute}");
            Console.WriteLine($"{nameof(IsJITOptimized)}: {IsJITOptimized}");
            Console.WriteLine($"{nameof(IsJITTrackingEnabled)}: {IsJITTrackingEnabled}");
            Console.WriteLine($"{nameof(BuildType)}: {BuildType}");
            Console.WriteLine($"{nameof(DebugOutput)}: {DebugOutput}");
        }

        private string GetVersion()
        {
            string version = null;
            string[] ss = textBox2.Text.Split('.');
            int x1, x2, x3 = 0;
            if (ss.Length == 3 && int.TryParse(ss[0], out x1) && int.TryParse(ss[1], out x2) && int.TryParse(ss[2], out x3))
            {
                version = x1 + "_" + x2 + "_" + x3;
            }
            else
            {
                MessageBox.Show("Version number illegal");
                version = null;
            }
            return version;
        }
                
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                File.Copy(@"c:\Program Files (x86)\Gekko\Gekko.exe", @"c:\Thomas\Gekko\GekkoCS\Diverse\RAMLargeAware\Gekko.exe", true);
                System.Diagnostics.Process.Start(@"c:\Thomas\Gekko\GekkoCS\Diverse\RAMLargeAware\editbin.exe", @"/LARGEADDRESSAWARE c:\Thomas\Gekko\GekkoCS\Diverse\RAMLargeAware\Gekko.exe");
                                             
                System.Diagnostics.ProcessStartInfo processInfo = new System.Diagnostics.ProcessStartInfo();
                processInfo.Verb = "runas";
                processInfo.FileName = @"c:\tools\ramaware.bat";
                try
                {
                    System.Diagnostics.Process.Start(processInfo);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("*** ERROR: RAM aware failed");
                }

                MessageBox.Show(@"RAM aware ok -- has copied from c:\Thomas\Gekko\GekkoCS\Diverse\RAMLargeAware\Gekko.exe   TO c:\Program Files (x86)\Gekko\Gekko.exe");
            }
            catch (Exception error)
            {
                MessageBox.Show(" *** ERROR: RAM aware failed");
            }
        }


        private void button4_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string dir = @"c:\Program Files (x86)\Gekko\";
                string zip = tools + @"\Gekko.zip";
                File.Delete(zip);
                ZipFile.CreateFromDirectory(dir, zip);
            }
            catch
            {
                MessageBox.Show("Zipping of Gekko.zip failed");
            }
            MessageBox.Show("Finished zipping of Gekko.zip");
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {

            string version = GetVersion();
            if (version == null)
            {
                MessageBox.Show("Problem with version number, SHA1 aborted");
                return;
            }

            File.Delete(tools + @"\sha.txt"); //for safety

            string sha1 = ComputeSha1(tools + @"\InstallerForGekko.msi");
            string sha2 = ComputeSha1(tools + @"\Setup.exe");
            string sha3 = ComputeSha1(tools + @"\Gekko.zip");

            if (sha1 == null || sha2 == null || sha3 == null)
            {
                MessageBox.Show("Problem with SHA1, not computed, aborting...");
                return;
            }

            string txt = null;
            txt += "<strong>Gekko " + version + " </strong>" + "\r\n";
            txt += "<ul>" + "\r\n";
            txt += "  <li>" + sha1 + "    " + "InstallerForGekko.msi" + "</li>" + "\r\n";
            txt += "  <li>" + sha2 + "    " + "Setup.exe" + "</li>" + "\r\n";
            txt += "  <li>" + sha3 + "    " + "Gekko.zip" + "</li>" + "\r\n";
            txt += "</ul>" + "\r\n";

            System.IO.File.WriteAllText(tools + @"\sha.txt", txt);

            this.textBoxSha1.Text = sha1 + " = " + "InstallerForGekko.msi" + "\n";
            this.textBoxSha1.Text += sha2 + " = " + "Setup.exe" + "\n";
            this.textBoxSha1.Text += sha3 + " = " + "Gekko.zip";

            MessageBox.Show(@"SHA ok, copied to " + tools);

        }

        private static string ComputeSha1(string fileName)
        {
            string ssha1 = null;
            try
            {

                if (!File.Exists(fileName))
                {
                    MessageBox.Show("File " + fileName + " does not exist, aborting...");
                    return null;
                }
                using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                using (BufferedStream bs = new BufferedStream(fs))
                {
                    using (System.Security.Cryptography.SHA1Managed sha1 = new System.Security.Cryptography.SHA1Managed())
                    {
                        byte[] hash = sha1.ComputeHash(bs);
                        StringBuilder formatted = new StringBuilder(2 * hash.Length);
                        foreach (byte b in hash)
                        {
                            formatted.AppendFormat("{0:X2}", b);
                        }
                        ssha1 = formatted.ToString().ToLower();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Could not get SHA1 of this: " + fileName);
            }
            return ssha1;
        }

        private void button6_Click(object sender, RoutedEventArgs e)
        {
            string version = GetVersion();
            if (version == null)
            {
                MessageBox.Show("Could not zip source, illegal version number");
                return;
            }
            try
            {
                string dir = @"c:\Thomas\Gekko\GekkoCS\";
                string zip = @"c:\Thomas\Gekko\" + version + ".zip";
                if(File.Exists(zip))
                {
                    MessageBox.Show("File " + zip + " already exists, aborting");
                    return;
                }                
                ZipHelper.CreateFromDirectory(dir, zip, fileName => !(fileName.Contains(@"\.vs\") || fileName.Contains(@"\.git\") || fileName.Contains(@"\TestResults\")));
            }
            catch (Exception ee)
            {                
                MessageBox.Show("Zipping of " + version + ".zip failed -- exists already?");
            }
            MessageBox.Show("Finished zipping of " + version + ".zip");
        }

        private void textBox9_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void button7_Click(object sender, RoutedEventArgs e)
        {
            int ii = 0;

            //string file = @"c:\Thomas\Gekko\GekkoCS\ANTLR\Cmd2.g";
            //string s = GetTextFromFileWithWait(file);
            //using (FileStream temp = Program.WaitForFileStream(file + "_", Program.GekkoFileReadOrWrite.Write))
            //using (StreamWriter tempFs = G.GekkoStreamWriter(temp))
            //{
            //    tempFs.Write(s);
            //}
                        
            string[] sss = textBox13.Text.Trim().ToUpper().Split(',');

            List<string> output1 = new List<string>();
            List<string> output2 = new List<string>();
            List<string> output3 = new List<string>();
            string file = @"c:\Thomas\Gekko\GekkoCS\ANTLR\Cmd3.g";
            string[] txt2 = File.ReadAllLines(file, Encoding.GetEncoding(1252));  //why 1252, but otherwise ¤ and æøå go wrong. Do not write with 1252 though...

            List<string> txt = new List<string>(txt2);
            int i1 = 0, i2 = 0, i3 = 0;
            foreach (string s in txt)
            {
                if (s.Contains("--- tokens1 start ---")) i1++;
                if (s.Contains("--- tokens1 end ---")) i1++;
                if (s.Contains("--- tokens2 start ---")) i2++;
                if (s.Contains("--- tokens2 end ---")) i2++;
                if (s.Contains("--- tokens3 start ---")) i3++;
                if (s.Contains("--- tokens3 end ---")) i3++;
            }
            if (i1 != 2 || i2 != 2 || i3 != 2) throw new Exception();


            foreach (string ss2 in sss)
            {

                string ss = ss2.Trim().ToUpper();

                bool dublet = false;
                                
                
                bool started = false;
                foreach (string s in txt)
                {
                    if (s.Contains("--- tokens1 start ---"))
                    {
                        started = true;
                    }
                    if (s.Contains("--- tokens1 end ---"))
                    {
                        break;
                    }
                    if (started && s.Trim().ToUpper().StartsWith(ss))
                    {
                        ii++;
                        dublet = true;
                    }
                }

                if (!dublet)
                {

                    foreach (string s in txt)
                    {
                        //output.Add(s);
                        if (s.Contains("--- tokens1 start ---"))
                        {
                            output1.Add("            " + ss + " = '" + ss + "';");
                        }
                        else if (s.Contains("--- tokens2 start ---"))
                        {
                            output2.Add("            d.Add(\"" + ss + "\", " + ss + ");");
                        }
                        else if (s.Contains("--- tokens3 start ---"))
                        {
                            output3.Add("            " + ss + "|");
                        }
                    }
                }
            }

            MessageBox.Show("Output is in extra.g, " + ii + " items were dublets");

            List<string> output = new List<string>();
            output.AddRange(output1);
            output.AddRange(output2);
            output.AddRange(output3);

            File.WriteAllLines(@"c:\Thomas\Gekko\GekkoCS\ANTLR\extra.g", output, Encoding.UTF8);
        }

        private void button8_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(@"c:\Tools\uninstall_gekko.bat");
        }

        private void textBox2_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void textBox12_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            System.IO.DirectoryInfo di = new DirectoryInfo(tools + @"\");
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
            MessageBox.Show("Folder " + tools + " is now wiped (empty)");
        }

        public static class ZipHelper
        {
            //Note that the three following are equivalent:         
            // ----------------------------------------------------------------------------
            //ZipFile.CreateFromDirectory(dir, zip);
            //ZipFile.CreateFromDirectory(dir, zip, CompressionLevel.Optimal, false);
            //ZipHelper.CreateFromDirectory(dir, zip, fileName => true)
            // ----------------------------------------------------------------------------

            public static void CreateFromDirectory(
                string sourceDirectoryName
            , string destinationArchiveFileName
            , CompressionLevel compressionLevel
            , bool includeBaseDirectory
            , Encoding entryNameEncoding
            , Predicate<string> filter // Add this parameter
            )
            {
                if (string.IsNullOrEmpty(sourceDirectoryName))
                {
                    throw new ArgumentNullException("sourceDirectoryName");
                }
                if (string.IsNullOrEmpty(destinationArchiveFileName))
                {
                    throw new ArgumentNullException("destinationArchiveFileName");
                }
                var filesToAdd = Directory.GetFiles(sourceDirectoryName, "*", SearchOption.AllDirectories);
                var entryNames = GetEntryNames(filesToAdd, sourceDirectoryName, includeBaseDirectory);
                using (var zipFileStream = new FileStream(destinationArchiveFileName, FileMode.Create))
                {
                    using (var archive = new ZipArchive(zipFileStream, ZipArchiveMode.Create))
                    {
                        for (int i = 0; i < filesToAdd.Length; i++)
                        {
                            // Add the following condition to do filtering:
                            if (!filter(filesToAdd[i]))
                            {
                                continue;
                            }
                            archive.CreateEntryFromFile(filesToAdd[i], entryNames[i], compressionLevel);
                        }
                    }
                }
            }

            //Default without parameters is: ZipFile.CreateFromDirectory(dir, zip) = ZipFile.CreateFromDirectory(dir, zip, CompressionLevel.Optimal, false);
            public static void CreateFromDirectory(string sourceDirectoryName, string destinationArchiveFileName, Predicate<string> filter)
            {                
                ZipHelper.CreateFromDirectory(sourceDirectoryName, destinationArchiveFileName, CompressionLevel.Optimal, false, Encoding.UTF8, filter);
            }

            //Default without parameters is: ZipFile.CreateFromDirectory(dir, zip) = ZipFile.CreateFromDirectory(dir, zip, CompressionLevel.Optimal, false);
            public static void CreateFromDirectory(string sourceDirectoryName, string destinationArchiveFileName, CompressionLevel compressionLevel, Predicate<string> filter)
            {
                ZipHelper.CreateFromDirectory(sourceDirectoryName, destinationArchiveFileName, compressionLevel, false, Encoding.UTF8, filter);
            }

            private static string[] GetEntryNames(string[] names, string sourceFolder, bool includeBaseName)
            {
                if (names == null || names.Length == 0)
                    return new string[0];

                if (includeBaseName)
                    sourceFolder = System.IO.Path.GetDirectoryName(sourceFolder);

                int length = string.IsNullOrEmpty(sourceFolder) ? 0 : sourceFolder.Length;
                if (length > 0 && sourceFolder != null && sourceFolder[length - 1] != System.IO.Path.DirectorySeparatorChar && sourceFolder[length - 1] != System.IO.Path.AltDirectorySeparatorChar)
                    length++;

                var result = new string[names.Length];
                for (int i = 0; i < names.Length; i++)
                {
                    result[i] = names[i].Substring(length);
                }

                return result;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
                        
            //string sha = ComputeSha1(s);

            string path = @"c:\Program Files (x86)\Gekko\";           
            
            Helper list = new Helper();

            //seems ok, sha1 exists on net
            list.Add("f8e4147d9b68dd9917253f1266b5c42764401045", path + @"Antlr3.Runtime.dll", @"C:\Thomas\Gekko\GekkoCS\Diverse\ExternalDllFiles\Antlr3.Runtime.dll");

            //same as 2.4.2
            list.Add("f145630ea51af460770a81351919032b3efc9219", path + @"GAMS.net4.dll", @"C:\Thomas\Gekko\GekkoCS\Diverse\ExternalDllFiles\GAMS.net4.dll");

            //same as 2.4.2
            //list.Add("45ca532a64d10c1f56635a11ec7973ee8bc7cf73", path + @"SevenZipSharp.dll", @"C:\Thomas\Gekko\GekkoCS\Diverse\ExternalDllFiles\SevenZipSharp.dll");

            //seems ok, sha1 exists on net
            //list.Add("774584ff54b38da5d3b3ee02e30908dacab175c5", path + @"zip\7z.dll", @"C:\Thomas\Gekko\GekkoCS\Diverse\FilesUsedForDeployment\7z.dll");

            //ok, taken from this EViews link: http://www.eviews.com/download/older/censusx12.html
            list.Add("2fae4c913ff299e61bbbc0f1bfe9d34a52465b50", path + @"X12A.EXE", @"C:\Thomas\Gekko\GekkoCS\Diverse\FilesUsedForDeployment\X12A.EXE");

            //same as 2.4.2
            list.Add("5fbaa5eef965a7df1985b3e48aa377c53c6d2b59", path + @"EPPlus.dll", @"C:\Thomas\Gekko\GekkoCS\packages\EPPlus.4.5.2.1\lib\net40\EPPlus.dll");

            //ok, taken from here: https://www.nuget.org/packages/protobuf-net/2.0.0.594
            //but the file in \packages is newer.
            list.Add("24c2c7a0d6c9918f037393c2a17e28a49d340df1", path + @"protobuf-net.dll", @"C:\Thomas\Gekko\GekkoCS\packages\protobuf-net.2.4.4\lib\net40\protobuf-net.dll");

            //same as 2.4.2
            list.Add("bf8056fd232c261f75c5dec7fd81dcfb4e5ab0a6", path + @"wshom.ocx", @"C:\Windows\SysWOW64\wshom.ocx");

            // -----

            //same as 2.4.2
            list.Add("7e23879c4950f3e26491d319bfadf1205ddde958", path + @"gnuplot\intl.dll", @"C:\Thomas\Gekko\GekkoCS\Gekko\bin\Debug\gnuplot\intl.dll");

            //same as 2.4.2
            list.Add("ef43fb9e1b652375a5de8a2dd7fb2fd9fdf30407", path + @"gnuplot\libcairo-2.dll", @"C:\Thomas\Gekko\GekkoCS\Gekko\bin\Debug\gnuplot\libcairo-2.dll");

            //same as 2.4.2
            list.Add("cf436334dd73053e77305428507fc8c70da33a74", path + @"gnuplot\libffi-6.dll", @"C:\Thomas\Gekko\GekkoCS\Gekko\bin\Debug\gnuplot\libffi-6.dll");

            //same as 2.4.2
            list.Add("125a2fcf210d4abb78fc8b43c6360c94c74917ab", path + @"gnuplot\libglib-2.0-0.dll", @"C:\Thomas\Gekko\GekkoCS\Gekko\bin\Debug\gnuplot\libglib-2.0-0.dll");

            //same as 2.4.2
            list.Add("428ddf59ca447dfa379085b2f55c93d1dabc8395", path + @"gnuplot\libgmodule-2.0-0.dll", @"C:\Thomas\Gekko\GekkoCS\Gekko\bin\Debug\gnuplot\libgmodule-2.0-0.dll");

            //same as 2.4.2
            list.Add("e6b7b69c7bfb8e6863335ab1f0db2ad96c6212a4", path + @"gnuplot\libgobject-2.0-0.dll", @"C:\Thomas\Gekko\GekkoCS\Gekko\bin\Debug\gnuplot\libgobject-2.0-0.dll");

            //same as 2.4.2
            list.Add("e47fc318b894553016c471d0141554e185f93d79", path + @"gnuplot\libiconv-2.dll", @"C:\Thomas\Gekko\GekkoCS\Gekko\bin\Debug\gnuplot\libiconv-2.dll");

            //same as 2.4.2
            list.Add("e7bd3cc831e063f1f1e0610505541dd1dbcb352d", path + @"gnuplot\libpango-1.0-0.dll", @"C:\Thomas\Gekko\GekkoCS\Gekko\bin\Debug\gnuplot\libpango-1.0-0.dll");

            //same as 2.4.2
            list.Add("bd2d99f7ce9620c2df101d9f64aed5629829ab25", path + @"gnuplot\libpangocairo-1.0-0.dll", @"C:\Thomas\Gekko\GekkoCS\Gekko\bin\Debug\gnuplot\libpangocairo-1.0-0.dll");

            //same as 2.4.2
            list.Add("6c2ed1be5ec1c71ccfd8c6be7371b32891964d57", path + @"gnuplot\libpangowin32-1.0-0.dll", @"C:\Thomas\Gekko\GekkoCS\Gekko\bin\Debug\gnuplot\libpangowin32-1.0-0.dll");

            //same as 2.4.2
            list.Add("ef4b4a15387f98b6695adf8615f243f3535e2891", path + @"gnuplot\libpng14-14.dll", @"C:\Thomas\Gekko\GekkoCS\Gekko\bin\Debug\gnuplot\libpng14-14.dll");

            //same as 2.4.2
            list.Add("81fe3c0c24277725723e1dbc63ce10fd235ebf60", path + @"gnuplot\wgnuplot.exe", @"C:\Thomas\Gekko\GekkoCS\Diverse\FilesUsedForDeployment\wgnuplot.exe");

            //same as 2.4.2
            list.Add("e771b8b319bfe5c85588b4817b50294a39d218db", path + @"gnuplot\wgnuplot51.exe", @"C:\Thomas\Gekko\GekkoCS\Gekko\bin\Debug\gnuplot\wgnuplot51.exe");

            //same as 2.4.2
            list.Add("cbbce727fd8447487c7fc68051b24df17d043649", path + @"gnuplot\zlib1.dll", @"C:\Thomas\Gekko\GekkoCS\Gekko\bin\Debug\gnuplot\zlib1.dll");

            // -----

            //same as 2.4.2
            list.Add("8f00ce31b60e5d6fdf70e29eefe61fb08c5bd180", path + @"XmlNotepad\FontBuilder.dll", @"C:\Thomas\Gekko\GekkoCS\Gekko\bin\Debug\XmlNotepad\FontBuilder.dll");

            //same as 2.4.2
            list.Add("617a4eece6cee8707cc7adda24dee9c648938797", path + @"XmlNotepad\Microsoft.XmlNotepad.dll", @"C:\Thomas\Gekko\GekkoCS\Gekko\bin\Debug\XmlNotepad\Microsoft.XmlNotepad.dll");

            //same as 2.4.2
            list.Add("61575949a7ef494ecac64b450045872e522c870f", path + @"XmlNotepad\XmlDiffPatch.dll", @"C:\Thomas\Gekko\GekkoCS\Gekko\bin\Debug\XmlNotepad\XmlDiffPatch.dll");

            //same as 2.4.2
            list.Add("e52b237d30be81cb9227d7d63894e5652808b08a", path + @"XmlNotepad\XmlDiffPatch.View.dll", @"C:\Thomas\Gekko\GekkoCS\Gekko\bin\Debug\XmlNotepad\XmlDiffPatch.View.dll");

            //same as 2.4.2
            list.Add("8d8b81f6311eac9623762fee5ba2031a815b9ae1", path + @"XmlNotepad\XmlNotepad.exe", @"C:\Thomas\Gekko\GekkoCS\Gekko\bin\Debug\XmlNotepad\XmlNotepad.exe");

            //same as 2.4.2
            list.Add("dbdbb8aaa6b17493c45193f33db3d02375bbcf59", path + @"XmlNotepad\XmlNotepadRegistration.exe", @"C:\Thomas\Gekko\GekkoCS\Gekko\bin\Debug\XmlNotepad\XmlNotepadRegistration.exe");

            int errors = 0;
            StringBuilder sb = new StringBuilder();
            
            foreach (string[] ss in list.storage)
            {
                string shaGekkoDir = ComputeSha1(ss[1]);                
                if (ss[0] != shaGekkoDir)
                {                    
                    errors++;
                    sb.AppendLine(ss[1]);
                }

                string shaSource = ComputeSha1(ss[2]);
                if (ss[0] != shaSource)
                {                    
                    errors++;
                    sb.AppendLine(ss[2]);
                }
            }

            MessageBox.Show("Checked " + list.storage.Count + " exe/dll file signatures, there were " + errors + " errors" + "\n" + sb.ToString());
        }
    }

    public class Helper
    {
        public List<string[]> storage = new List<string[]>();
        public void Add(string s1, string s2, string s3)
        {
            string[] temp = new string[] { s1, s2, s3 };
            storage.Add(temp);
        }
    }
}
