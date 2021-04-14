using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.IO.Compression;

namespace Deploy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {        
        public static string tools = @"c:\tools\tmp\Gekko_files";
        public bool installerIs64Bit = false;
        public static string ifile = @"c:\Thomas\Gekko\GekkoCS\InstallerForGekko\InstallerForGekko.vdproj";

        public MainWindow()
        {
            InitializeComponent();
            this.Top = 10;
            this.Left = 10;
            InstallerBitness(ifile);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            File.Copy(@"c:\Thomas\Gekko\dok\user\Gekko.chm", @"c:\Thomas\Gekko\GekkoCS\Gekko\bin\Debug\helpfiles\Gekko.chm", true);
            MessageBox.Show("Copied Gekko.chm");
        }

        private string bit32Or64()
        {
            if (installerIs64Bit) return "64";
            else return "32";
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string file = null;
                if (installerIs64Bit) file = @"c:\Thomas\Gekko\GekkoCS\InstallerForGekko\Release\InstallerForGekko.msi";
                else file = @"c:\Thomas\Gekko\GekkoCS\InstallerForGekko\Release32bit\InstallerForGekko.msi";

                FileInfo fi = new FileInfo(file);
                double min = (DateTime.Now - fi.LastWriteTime).TotalMinutes;
                if (min > 15)
                {
                    MessageBox.Show("ERROR: the .msi file seems too old (> 15 minutes)");
                    return;
                }
                System.Diagnostics.Process.Start(file);                
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
                if (installerIs64Bit)
                {
                    File.Copy(@"c:\Thomas\Gekko\GekkoCS\InstallerForGekko\Release\InstallerForGekko.msi", tools + @"\" + bit32Or64() + @"\InstallerForGekko.msi", true);
                    File.Copy(@"c:\Thomas\Gekko\GekkoCS\InstallerForGekko\Release\Setup.exe", tools + @"\" + bit32Or64() + @"\Setup.exe", true);
                }
                else
                {
                    File.Copy(@"c:\Thomas\Gekko\GekkoCS\InstallerForGekko\Release32bit\InstallerForGekko.msi", tools + @"\" + bit32Or64() + @"\InstallerForGekko.msi", true);
                    File.Copy(@"c:\Thomas\Gekko\GekkoCS\InstallerForGekko\Release32bit\Setup.exe", tools + @"\" + bit32Or64() + @"\Setup.exe", true);
                }
                    
                MessageBox.Show(@"Copying 2 files ok");
            }
            catch
            {
                MessageBox.Show("*** ERROR: Copying 2 files failed");
            }           
            
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
                string dir = null;
                if (installerIs64Bit)
                {
                    dir = @"c:\Program Files\Gekko\";
                }
                else
                {
                    dir = @"c:\Program Files (x86)\Gekko\";                    
                }

                FileInfo fi = new FileInfo(dir + "gekko.exe");
                double min = (DateTime.Now - fi.LastWriteTime).TotalMinutes;
                if (min > 15)
                {
                    MessageBox.Show("ERROR: the gekko.exe file seems too old (> 15 minutes) for zipping Gekko" + bit32Or64() + ".zip");
                    return;
                }

                string zip = tools + @"\" + bit32Or64() + @"\Gekko.zip";
                File.Delete(zip);
                ZipFile.CreateFromDirectory(dir, zip);
            }
            catch
            {
                MessageBox.Show("Zipping of Gekko" + bit32Or64() + ".zip failed");
            }
            MessageBox.Show("Finished zipping of Gekko" + bit32Or64() + ".zip");
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {

            string version = GetVersion();
            if (version == null)
            {
                MessageBox.Show("Problem with version number, SHA1 aborted");
                return;
            }

            File.Delete(tools + @"\"+ bit32Or64() + @"\sha.txt"); //for safety

            string sha1_32 = ComputeSha1(tools + @"\" + "32" + @"\InstallerForGekko.msi");
            string sha2_32 = ComputeSha1(tools + @"\"+ "32" + @"\Setup.exe");
            string sha3_32 = ComputeSha1(tools + @"\"+ "32" + @"\Gekko.zip");
            string sha4_32 = ComputeSha1(tools + @"\" + "Gekcel" + @"\" + "32" + @"\Gekcel.zip");
            string sha1_64 = ComputeSha1(tools + @"\" + "64" + @"\InstallerForGekko.msi");
            string sha2_64 = ComputeSha1(tools + @"\" + "64" + @"\Setup.exe");
            string sha3_64 = ComputeSha1(tools + @"\" + "64" + @"\Gekko.zip");
            string sha4_64 = ComputeSha1(tools + @"\" + "Gekcel" + @"\" + "64" + @"\Gekcel.zip");

            if (sha1_32 == null || sha2_32 == null || sha3_32 == null || sha4_32 == null || sha1_64 == null || sha2_64 == null || sha3_64 == null || sha4_64 == null)
            {
                MessageBox.Show("Problem with SHA1, not computed, aborting...");
                return;
            }

            string txt = null;
            txt += "<strong>Gekko " + version + " </strong>" + "\r\n";
            txt += "<ul>" + "\r\n";
            txt += "  <li>" + sha1_32 + "    " + "InstallerForGekko.msi (32-bit)" + "</li>" + "\r\n";
            txt += "  <li>" + sha1_64 + "    " + "InstallerForGekko.msi (64-bit)" + "</li>" + "\r\n";
            txt += "  <li>" + sha2_32 + "    " + "Setup.exe (32-bit)" + "</li>" + "\r\n";
            txt += "  <li>" + sha2_64 + "    " + "Setup.exe (64-bit)" + "</li>" + "\r\n";
            txt += "  <li>" + sha3_32 + "    " + "Gekko.zip (32-bit)" + "</li>" + "\r\n";
            txt += "  <li>" + sha3_64 + "    " + "Gekko.zip (64-bit)" + "</li>" + "\r\n";
            txt += "  <li>" + sha4_32 + "    " + "Gekcel.zip (32-bit)" + "</li>" + "\r\n";
            txt += "  <li>" + sha4_64 + "    " + "Gekcel.zip (64-bit)" + "</li>" + "\r\n";
            txt += "</ul>" + "\r\n";

            System.IO.File.WriteAllText(tools + @"\sha.txt", txt);

            MessageBox.Show(@"SHA ok, copied to: " + tools);

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
            Directory.CreateDirectory(tools + @"\" + @"32");
            Directory.CreateDirectory(tools + @"\" + @"64");
            Directory.CreateDirectory(tools + @"\" + @"Gekcel");
            Directory.CreateDirectory(tools + @"\" + @"Gekcel\32");
            Directory.CreateDirectory(tools + @"\" + @"Gekcel\64");            
            MessageBox.Show("Folder " + tools + " is now wiped (empty), with 3 subfolders.");
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

            public static void CreateZipFromFiles(string fileName, List<string> files)
            {
                // Create and open a new ZIP file
                var zip = ZipFile.Open(fileName, ZipArchiveMode.Create);
                foreach (var file in files)
                {
                    // Add the entry for each file, cf. #89u3258572345
                    string entryName = Path.GetFileName(file);
                    if (entryName == "Gekcel64.xll") entryName = "Gekcel.xll";
                    else if (entryName == "Gekcel64.dna") entryName = "Gekcel.dna";
                    zip.CreateEntryFromFile(file, entryName, CompressionLevel.Optimal);
                }
                // Dispose of the object when we are done
                zip.Dispose();
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

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            
            string text = File.ReadAllText(ifile);
            if (text.Contains("\"TargetPlatform\" = \"3:0\""))
            {
                MessageBoxResult m = MessageBox.Show("Installer bitness is 32-bit. Would you like to CHANGE to 64-bit?", "", MessageBoxButton.YesNoCancel);
                if (m == MessageBoxResult.Yes)
                {
                    text = text.Replace("\"TargetPlatform\" = \"3:0\"", "\"TargetPlatform\" = \"3:1\"");
                    text = text.Replace("\"DefaultLocation\" = \"8:[ProgramFilesFolder]\\\\[ProductName]\"", "\"DefaultLocation\" = \"8:[ProgramFiles64Folder]\\\\[ProductName]\"");
                    File.WriteAllText(ifile, text);
                    MessageBox.Show("Installer bitness set to 64-bit");
                }
                else
                {
                    MessageBox.Show("Installer bitness kept at 32-bit");
                }
            }
            else if (text.Contains("\"TargetPlatform\" = \"3:1\""))
            {
                MessageBoxResult m = MessageBox.Show("Installer bitness is 64-bit. Would you like to CHANGE to 32-bit?", "", MessageBoxButton.YesNoCancel);
                if (m == MessageBoxResult.Yes)
                {
                    text = text.Replace("\"TargetPlatform\" = \"3:1\"", "\"TargetPlatform\" = \"3:0\"");
                    text = text.Replace("\"DefaultLocation\" = \"8:[ProgramFiles64Folder]\\\\[ProductName]\"", "\"DefaultLocation\" = \"8:[ProgramFilesFolder]\\\\[ProductName]\"");
                    File.WriteAllText(ifile, text);
                    MessageBox.Show("Installer bitness set to 32-bit");
                }
                else
                {
                    MessageBox.Show("Installer bitness kept at 64-bit");
                }
            }
            else
            {
                MessageBox.Show("Installer does not seem to be either 32-bit or 64-bit -- strange, aborting...");
            }

            InstallerBitness(ifile);
        }

        private void InstallerBitness(string file)
        {
            string text2 = File.ReadAllText(file);
            if (text2.Contains("\"TargetPlatform\" = \"3:0\"")) installerIs64Bit = false;
            else if (text2.Contains("\"TargetPlatform\" = \"3:1\"")) installerIs64Bit = true;
            else
            {
                MessageBox.Show("Installer does not seem to be either 32-bit or 64-bit -- strange, aborting...");
            }
            if (installerIs64Bit) label_Bitness.Content = "Installer: 64-bit";
            else label_Bitness.Content = "Installer: 32-bit";

            if (installerIs64Bit)
            {
                if (!text2.Contains("\"DefaultLocation\" = \"8:[ProgramFiles64Folder]\\\\[ProductName]\""))
                {
                    MessageBox.Show("*** ERROR: for 64-bit: expected vdproj file to contain [ProgramFiles64Folder]");
                    return;
                }
            }
            else
            {
                if (!text2.Contains("\"DefaultLocation\" = \"8:[ProgramFilesFolder]\\\\[ProductName]\""))
                {
                    MessageBox.Show("*** ERROR: for 32-bit: expected vdproj file to contain [ProgramFilesFolder]");
                    return;
                }
            }
        }

        public static bool CheckExpiry(string file)
        {
            FileInfo fi = new FileInfo(file);
            double min = (DateTime.Now - fi.LastWriteTime).TotalMinutes;
            if (min > 15)
            {
                MessageBox.Show("ERROR: the file '" + file + "' seems too old (> 15 minutes)");
                return true;
            }
            return false;
        }        

        private void Button_Click_4a(object sender, RoutedEventArgs e)
        {
            GekcelZipHelper(32);
        }

        private void Button_Click_4b(object sender, RoutedEventArgs e)
        {
            GekcelZipHelper(64);
        }

        private void Button_Click_44a(object sender, RoutedEventArgs e)
        {
            GekcelFileHelper(32);
        }

        private void Button_Click_44b(object sender, RoutedEventArgs e)
        {
            GekcelFileHelper(64);
        }

        private static void GekcelFileHelper(int bitness)
        {
            string path1 = null;
            if (bitness == 32)
            {
                path1 = @"c:\Thomas\Gekko\GekkoCS\Gekko\bin\x86\Debug\";
            }
            else
            {
                path1 = @"c:\Thomas\Gekko\GekkoCS\Gekko\bin\x64\Debug\";
            }
            string path2 = @"c:\Thomas\Gekko\GekkoCS\Gekcel\Gekcel\Diverse\ExternalDllFiles\";

            bool fail = false;

            //see also list here: #8907520357238
            List<string> files = new List<string>();
            files.Add("Gekko.exe");
            files.Add("Gekko.pdb");
            files.Add("ANTLR.dll");
            foreach (string s in files)
            {
                bool b = CheckExpiry(path1 + s);  //only check on these, not the rest
                if(b)
                {
                    fail = true;
                    break;
                }
            }
            if (fail) return;
            files.Add("protobuf-net.dll");
            files.Add("GAMS.net4.dll");
            files.Add("EPPlus.dll");
            files.Add("Antlr3.Runtime.dll");

            foreach (string s in files)
            {
                File.Copy(path1 + s, path2 + s, true);  //overwrite
            }
            MessageBox.Show("Files copied");
        }

        private static void GekcelZipHelper(int bitness)
        {
            string path1 = @"c:\Thomas\Gekko\GekkoCS\Gekcel\Gekcel\Diverse\ExternalDllFiles\";
            string path2 = @"c:\Thomas\Gekko\GekkoCS\Gekcel\Gekcel\bin\Debug\";

            bool fail = false;

            //see also list here: #8907520357238
            List<string> files = new List<string>();
            files.Add(path1 + "Gekko.exe");
            files.Add(path1 + "Gekko.pdb");
            files.Add(path1 + "ANTLR.dll");
            if(bitness == 32) files.Add(path2 + "Gekcel.xlsm");  //note: path2 --> this file must be newly createdj for Gekcel 32-bit: good to check that here.
            foreach (string s in files)
            {
                bool b = CheckExpiry(s);  //only check on these, not the rest
                if (b)
                {
                    fail = true;
                    break;
                }
            }
            if (fail) return;
            files.Add(path1 + "protobuf-net.dll");
            files.Add(path1 + "GAMS.net4.dll");
            files.Add(path1 + "EPPlus.dll");
            files.Add(path1 + "Antlr3.Runtime.dll");
            //hmmmmmmmmmmm what about all the other files??
            files.Add(path1 + "demo.gbk");
            // -----            
            files.Add(path2 + "ExcelDna.Integration.dll");
            files.Add(path2 + "ExcelDna.IntelliSense.dll");
            files.Add(path2 + "Gekcel.dll");            
            files.Add(path2 + "Gekcel.pdb");
            
            if (bitness == 64)
            {
                files.Add(path2 + "Gekcel.xlsm");   //when packaging Gekcel 64-bit, the file may be > 15 minutes if we are slow...
                files.Add(path2 + "Gekcel64.dna");  //will have 64 removed later on, see #89u3258572345
                files.Add(path2 + "Gekcel64.xll");  //will have 64 removed later on, see #89u3258572345
            }
            else
            {
                files.Add(path2 + "Gekcel.dna");
                files.Add(path2 + "Gekcel.xll");
            }

            string zip = tools + @"\Gekcel\" + bitness + @"\Gekcel.zip";
            if (File.Exists(zip)) File.Delete(zip);
            ZipHelper.CreateZipFromFiles(zip, files);

            //about 64-bit see: https://colinlegg.wordpress.com/2016/09/07/my-first-c-net-udf-using-excel-dna-and-visual-studio/";

            //s = "If there are problems injecting VBA, see Gekcel/Program.cs for trust stuff" + "\n";
            //s += "1. Double-click Gekcel.xll" + "\n";
            //s += "2. If there is a security warning, click 'activate'" + "\n";
            //s += "3. Now there should be a 'Gekko' tab on the ribbon. Click this tab and click the 'Setup' button'" + "\n";
            //s += "   You should get a message that the Gekko environment is set up." + "\n";
            //s += "4. Run the macro 'Demo' " + "\n";
            //MessageBox.Show(s);
            //s = "Now package files in Gekcel32.zip, see what files were in last time" + "\n";

            MessageBox.Show("Finished packing a Gekcel.zip");
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
