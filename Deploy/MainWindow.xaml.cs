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
//using System.IO.Compression.FileSystem;
using System.Net;
using SevenZip;

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
        public static string zip = @"c:\Program Files\7-Zip\7z.dll";
        public static string tools = @"c:\tools\tmp\Gekko_files";

        public MainWindow()
        {
            InitializeComponent();
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
                MessageBox.Show("Installation ok");
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
            
            //string version = null;
            //string[] ss = textBox2.Text.Split('.');
            //int x1, x2, x3 = 0;
            //if (ss.Length == 3 && int.TryParse(ss[0], out x1) && int.TryParse(ss[1], out x2) && int.TryParse(ss[2], out x3))
            //{
            //    version = x1 + "_" + x2 + "_" + x3;
            //}
            //else
            //{
            //    MessageBox.Show("Version number illegal");
            //    return;
            //}

            //WebRequest request = WebRequest.Create(@"http://www.t-t.dk/gekko/downloads/" + version);
            //request.Method = WebRequestMethods.Ftp.MakeDirectory;
            //request.Credentials = new NetworkCredential(user, pwd);
            //using (var resp = (FtpWebResponse)request.GetResponse())
            //{
            //    //Console.WriteLine(resp.StatusCode);
            //}

            //using (WebClient client = new WebClient())
            //{
            //    client.Credentials = new NetworkCredential(user, pwd);
            //    client.UploadFile(@"http://www.t-t.dk/gekko/downloads/" + version + "/InstallerForGekko.msi", "STOR", @"c:\Thomas\Gekko\GekkoCS\InstallerForGekko\Release32bit\InstallerForGekko.msi");
            //}
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
                ZipFile.CreateFromDirectory(dir, zip); //deflate: ZipFile.ExtractToDirectory()
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
            if (!File.Exists(fileName))
            {
                MessageBox.Show("File " + fileName + " does not exist, aborting...");
                return null;
            }
            using (FileStream fs = new FileStream(fileName, FileMode.Open))
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
                //File.Delete(zip);  //let it fail, could type wrong number
                ZipFile.CreateFromDirectory(dir, zip); //deflate: ZipFile.ExtractToDirectory()
            }
            catch
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
    }
}
