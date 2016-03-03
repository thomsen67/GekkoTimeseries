using System;
using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Gekko
{
    public class Task : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        string aliasName;
        string fileName;
        string fileNameWithPath;
        string size;
        string period;
        string info1;
        string date;
        string originalAliasName;
        string rowColor;
        string lineColor;
        bool finished;
        string number;
        string prot;
        

        public Task(string AliasName, string FileName, string FileNameWithPath, string Size, string Period, string Info1, string Date, string RowColor, string prot, int i)
        {
            this.aliasName = AliasName;
            this.fileName = FileName;
            this.fileNameWithPath = FileNameWithPath;
            this.size = Size;
            this.period = Period;
            this.info1 = Info1;
            this.date = Date;
            this.rowColor = RowColor;
            this.originalAliasName = AliasName;
            if (i == 1) this.lineColor = "Black";
            else this.lineColor = "LightGray";
            this.number = "";
            if (i == 0) this.number = "1";
            else if (i == 1) this.number = "REF";
            else if (i > 1) this.number = (i + 0).ToString();
            this.prot = prot;
        }
        
        public string Number
        {
            get { return this.number; }
            set
            {
                this.number = value;
                OnPropertyChanged("Number");
            }
        }

        public bool Finished
        {
            get { return this.finished; }
            set { this.finished = value; }
        }

        public string AliasName
        {            
            get { return this.aliasName; }
            set 
            { 
                this.aliasName = value;
                OnPropertyChanged("AliasName");
                
                //System.ComponentModel.notify em.RaisePropertyChanged("AliasName");
            }
        }

        public string Prot
        {
            get { return this.prot; }
            set
            {
                this.prot = value;
                OnPropertyChanged("Prot");
            }
        }

        public string FileName
        {
            get { return this.fileName; }
        }

        public string FileNameWithPath
        {
            get { return this.fileNameWithPath; }
        }

        public string Size
        {
            get { return this.size; }
        }

        public string Period
        {
            get { return this.period; }
        }

        public string Info1
        {
            get { return this.info1; }
        }

        public string OriginalAliasName
        {
            get { return this.originalAliasName; }
        }

        public string Date
        {
            get { return this.date; }
        }

        public string RowColor
        {
            get { return this.rowColor; }
            set { 
                this.rowColor = value;
                OnPropertyChanged("RowColor");
            }
        }

        public string LineColor
        {
            get { return this.lineColor; }
            set
            {
                this.lineColor = value;
                OnPropertyChanged("LineColor");
            }
        }

        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
