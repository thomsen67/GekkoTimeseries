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
using System.Net;
using SevenZip;

namespace Deploy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string startupPath = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        public void SetStartupPath(string s)
        {
            startupPath = s;
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
                double min = (DateTime.Now - fi.CreationTime).TotalMinutes;
                if (min > 60)
                {
                    MessageBox.Show("ERROR: the .msi file seems too old");
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
                string version = GetVersion();
                
                System.IO.DirectoryInfo di = new DirectoryInfo(@"c:\tmp\Gekko_files\");
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }
                
                File.Copy(@"c:\Thomas\Gekko\GekkoCS\InstallerForGekko\Release32bit\InstallerForGekko.msi", @"c:\tmp\Gekko_files\InstallerForGekko.msi", true);
                File.Copy(@"c:\Thomas\Gekko\GekkoCS\InstallerForGekko\Release32bit\Setup.exe", @"c:\tmp\Gekko_files\Setup.exe", true);
                MessageBox.Show("Copying files ok -- now manually FTP them to " + version + @" from C:\tmp\Gekko_files");
            }
            catch
            {
                MessageBox.Show("*** ERROR: Copying files failed");
            }
            //string user = "user";
            //string pwd = textBox7.Text;
            //if (pwd == null || pwd == "")
            //{
            //    MessageBox.Show("No FTP pwd");
            //    return;
            //}

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
                throw new Exception();
            }
            return version;
        }
                
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //c:\Thomas\Gekko\GekkoCS\Diverse\RAMLargeAware\   c:\Program Files (x86)\Gekko\
                File.Copy(@"c:\Program Files (x86)\Gekko\Gekko.exe", @"c:\Thomas\Gekko\GekkoCS\Diverse\RAMLargeAware\Gekko.exe", true);
                //System.Diagnostics.Process.Start(@"c:\Thomas\Gekko\GekkoCS\Diverse\RAMLargeAware\xx.bat");

                System.Diagnostics.Process.Start(@"c:\Thomas\Gekko\GekkoCS\Diverse\RAMLargeAware\editbin.exe", @"/LARGEADDRESSAWARE c:\Thomas\Gekko\GekkoCS\Diverse\RAMLargeAware\Gekko.exe");

                System.Diagnostics.Process.Start(@"c:\Tools\ramaware.lnk");

                //File.Copy(@"c:\Thomas\Gekko\GekkoCS\Diverse\RAMLargeAware\Gekko.exe", @"c:\Program Files (x86)\Gekko\Gekko.exe", true);
                
                //File.Copy(@"c:\Thomas\Gekko\GekkoCS\Diverse\RAMLargeAware\Gekko.exe", @"c:\Program Files (x86)\Gekko\Gekko.exe", true);
                                
                MessageBox.Show(@"RAM aware ok -- has copied from c:\Thomas\Gekko\GekkoCS\Diverse\RAMLargeAware\Gekko.exe   TO c:\Program Files (x86)\Gekko\Gekko.exe");
            }
            catch (Exception error)
            {
                MessageBox.Show(" * ** ERROR: RAM aware failed");
            }
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string sevenzPath = startupPath + "\\zip\\7z.dll";
                SevenZipExtractor.SetLibraryPath(sevenzPath);
                SevenZipCompressor tmp = new SevenZipCompressor();
                tmp.ArchiveFormat = OutArchiveFormat.Zip;
                tmp.CompressionLevel = CompressionLevel.Normal;                
                tmp.CompressDirectory(@"c:\Program Files (x86)\Gekko\", @"c:\tmp\Gekko_files\Gekko.zip", true);
                MessageBox.Show("Zipping of Gekko program dir ok");
            }
            catch
            {
                MessageBox.Show("*** ERROR: Zipping of Gekko program dir failed");
            }
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                File.Copy(@"c:\tmp\Gekko_files\InstallerForGekko.msi", @"c:\Thomas\Gekko\GekkoCS\Diverse\SHA1\InstallerForGekko.msi", true);
                File.Copy(@"c:\tmp\Gekko_files\Setup.exe", @"c:\Thomas\Gekko\GekkoCS\Diverse\SHA1\Setup.exe", true);
                File.Copy(@"c:\tmp\Gekko_files\Gekko.zip", @"c:\Thomas\Gekko\GekkoCS\Diverse\SHA1\Gekko.zip", true);

                System.Diagnostics.Process.Start(@"c:\Thomas\Gekko\GekkoCS\Diverse\SHA1\sha.bat");

                //MessageBox.Show(@"Go to c:\Thomas\Gekko\GekkoCS\Diverse\SHA1\ and run sha.bat now");

                //System.Diagnostics.Process.Start(@"c:\Thomas\Gekko\GekkoCS\Diverse\SHA1\fciv.exe", @"c:\Thomas\Gekko\GekkoCS\Diverse\SHA1\InstallerForGekko.msi -sha1 > sha1.txt");
                //System.Diagnostics.Process.Start(@"c:\Thomas\Gekko\GekkoCS\Diverse\SHA1\fciv.exe", @"c:\Thomas\Gekko\GekkoCS\Diverse\SHA1\setup.exe -sha1 > sha2.txt");
                //System.Diagnostics.Process.Start(@"c:\Thomas\Gekko\GekkoCS\Diverse\SHA1\fciv.exe", @"c:\Thomas\Gekko\GekkoCS\Diverse\SHA1\Gekko.zip -sha1 > sha3.txt");

                //System.Diagnostics.Process.Start(@"c:\Thomas\Gekko\GekkoCS\Diverse\SHA1\sha.bat");

                //string ss = File.ReadAllText(@"c:\Thomas\Gekko\GekkoCS\Diverse\SHA1\sha1.txt") + " " + File.ReadAllText(@"c:\Thomas\Gekko\GekkoCS\Diverse\SHA1\sha2.txt") + " " + File.ReadAllText(@"c:\Thomas\Gekko\GekkoCS\Diverse\SHA1\sha3.txt");


                string[] ss1 = File.ReadAllLines(@"c:\Thomas\Gekko\GekkoCS\Diverse\SHA1\sha1.txt");
                string[] ss2 = File.ReadAllLines(@"c:\Thomas\Gekko\GekkoCS\Diverse\SHA1\sha2.txt");
                string[] ss3 = File.ReadAllLines(@"c:\Thomas\Gekko\GekkoCS\Diverse\SHA1\sha3.txt");

                string[] filtered1 = ss1.Where(s => !s.Trim().StartsWith("//")).ToArray();
                string[] filtered2 = ss2.Where(s => !s.Trim().StartsWith("//")).ToArray();
                string[] filtered3 = ss3.Where(s => !s.Trim().StartsWith("//")).ToArray();

                string t1 = filtered1[0].Replace(@" c:\thomas\gekko\gekkocs\diverse\sha1\", "    ");
                string t2 = filtered2[0].Replace(@" c:\thomas\gekko\gekkocs\diverse\sha1\", "    ");
                string t3 = filtered3[0].Replace(@" c:\thomas\gekko\gekkocs\diverse\sha1\", "    ");

                string txt = null;

                txt += "<strong>Gekko " + GetVersion() + " </strong>" + "\r\n";
                txt += "<ul>" + "\r\n";
                txt += "  <li>" + t1 + "</li>" + "\r\n";
                txt += "  <li>" + t2 + "</li>" + "\r\n";
                txt += "  <li>" + t3 + "</li>" + "\r\n";
                txt += "</ul>" + "\r\n";

                System.IO.File.WriteAllText(@"c:\tmp\Gekko_files\sha.txt", t1 + t2 + t3);
                MessageBox.Show(@"SHA ok, copied to c:\tmp\Gekko_files");
            }
            catch (Exception exception)
            {
                MessageBox.Show("*** ERROR: SHA failed");
            }
        }

        private void button6_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string sevenzPath = startupPath + "\\zip\\7z.dll";
                SevenZipExtractor.SetLibraryPath(sevenzPath);
                SevenZipCompressor tmp = new SevenZipCompressor();
                tmp.ArchiveFormat = OutArchiveFormat.Zip;
                tmp.CompressionLevel = CompressionLevel.Normal;
                tmp.CompressDirectory(@"c:\Thomas\Gekko\GekkoCS\", @"c:\Thomas\Gekko\" + GetVersion() + ".zip", true);
                MessageBox.Show("Zipping of Gekko " + GetVersion() + ".zip" + "  ok -- REMOVE .git and TestResults folders!");
            }
            catch
            {
                MessageBox.Show("*** ERROR: Zipping of Gekko source failed");
            }
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
            string file = @"c:\Thomas\Gekko\GekkoCS\ANTLR\Cmd2.g";
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
    }
}
