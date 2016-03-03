/* 
    Gekko Timeseries Software (www.t-t.dk/gekko).
    Copyright (C) 2016, Thomas Thomsen, T-T Analyse.

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program (see the file COPYING in the root folder).
    Else, see <http://www.gnu.org/licenses/>.        
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;


namespace WA
{
    class Program
    {
        static void Main(string[] args)
        {
            //args = new string[1];
            //args[0] = "C:\\warem32";

            //This program is an AREMOS functionality not directly related to Gekko            
            
            if (args.Length != 1)
            {
                MessageBox.Show("*** ERROR: Gekko/WA.exe should be called with an argument indicating the AREMOS program folder");
                return;
            }
            string aremosPath = args[0];
            string callingPath = System.IO.Directory.GetCurrentDirectory();

            List<string> temp = null;

            if (File.Exists(aremosPath + "\\aremos.opt"))
            {

                temp = new List<string>();
                StreamReader inputFileStringReader = null;
                try
                {
                    inputFileStringReader = new StreamReader(aremosPath + "\\aremos.opt");
                }
                catch (Exception e)
                {
                    MessageBox.Show("*** ERROR: Gekko/WA.exe could not read AREMOS-option file " + aremosPath + "\\aremos.opt \nIs it opened in another program?");
                    return;
                }
                
                List<string> output = new List<string>();
                while (true)
                {
                    string aLine = null;
                    try
                    {
                        aLine = inputFileStringReader.ReadLine();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("*** ERROR: Gekko/WA.exe could not read AREMOS-option file " + aremosPath + "\\aremos.opt \nIs it opened in another program?");
                        return;
                    }
                    if (aLine != null)
                    {
                        output.Add(aLine);
                    }
                    else
                    {
                        break;
                    }
                }

                foreach (string line in output)
                {
                    if (line.ToLower().Trim().StartsWith("set file dir"))
                    {
                        //ignore such lines
                    }
                    else
                    {
                        temp.Add(line);
                    }
                }
                temp.Add("set file dir \"" + callingPath + "\";");
                inputFileStringReader.Close();
            }
            else
            {
                MessageBox.Show("*** ERROR: Gekko/WA.exe could not find AREMOS-option file " + aremosPath + "\\aremos.opt");
                return;
            }

            try
            {
                File.Delete(aremosPath + "\\aremos.opt");  //do not use WaitForFileDelete() here
            }
            catch (Exception e)
            {
                MessageBox.Show("*** ERROR: Gekko/WA.exe could not change AREMOS-option file " + aremosPath + "\\aremos.opt \nIs it opened in another program?");
                return;
            }
            StreamWriter sw = new StreamWriter(aremosPath + "\\aremos.opt");
            foreach (string line in temp)
            {
                sw.WriteLine(line);
            }

            sw.Flush();
            sw.Close();

        }
    }
}
