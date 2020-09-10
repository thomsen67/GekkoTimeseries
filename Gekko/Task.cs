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
    public class GekkoTask : INotifyPropertyChanged
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
        int i;  //number in collection
        string pivot_text;
        string pivot_buttonVisible1;
        string pivot_buttonVisible2;
        string pivot_buttonVisible3;
        string pivot_fontWeight;
        WindowDecomp.TaskType pivot_taskType;          
        ObservableCollection<string> pivot_sublist;
        public List<string> pivot_filterSelected;
        // ----------------------------------------------
        public DecompOptions2 decompOptions2;

        //pivot
        public GekkoTask(string text, string rowColor, string visible3, string visible1, string visible2, string fontWeight, WindowDecomp.TaskType taskType, int i, ObservableCollection<string> sublist, List<string> filterSelected, DecompOptions2 decompOptions2)
        {
            this.pivot_text = text;
            this.pivot_buttonVisible1 = visible1;
            this.pivot_buttonVisible2 = visible2;
            this.pivot_buttonVisible3 = visible3;
            this.pivot_fontWeight = fontWeight;
            this.lineColor = "LightGray";            
            this.pivot_taskType = taskType;
            this.i = i;
            this.pivot_sublist = sublist;
            this.rowColor = rowColor;
            this.pivot_filterSelected = filterSelected;
            this.decompOptions2 = decompOptions2;
        }

        //non-pivot
        public GekkoTask(string AliasName, string FileName, string FileNameWithPath, string Size, string Period, string Info1, string Date, string RowColor, string prot, int i)
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
            this.lineColor = "LightGray";
            this.number = "";
            if (i == 0) this.number = "1";
            else if (i == 1) this.number = "REF";
            else if (i > 1) this.number = i.ToString();
            if (G.Equal(this.aliasName, "Local")) this.number = "";
            if (G.Equal(this.aliasName, "Global")) this.number = "";
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
            set
            {
                this.finished = value;
                OnPropertyChanged("Finished");
            }
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

        public int I
        {
            get { return this.i; }      
            set { this.i = value; }
        }

        public ObservableCollection<string> Pivot_Sublist
        {
            get
            {
                return this.pivot_sublist;
            }
            set
            {
                this.pivot_sublist = value;
                OnPropertyChanged("Pivot_Sublist");
            }
        }

        public string Pivot_Text
        {
            get { return this.pivot_text; }
            set { this.pivot_text = value; }
        }

        public string Pivot_ButtonVisible1
        {
            get { return this.pivot_buttonVisible1; }
        }

        public string Pivot_ButtonVisible2
        {
            get { return this.pivot_buttonVisible2; }
        }

        public string Pivot_ButtonVisible3
        {
            get { return this.pivot_buttonVisible3; }
            set
            {
                this.pivot_buttonVisible3 = value;
                OnPropertyChanged(Pivot_ButtonVisible3);
            }
        }

        public string Pivot_FontWeight
        {
            get { return this.pivot_fontWeight; }
        }

        public WindowDecomp.TaskType Pivot_TaskType
        {
            get { return this.pivot_taskType; }
            set { this.pivot_taskType = value; }
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

    //public class ListTask : INotifyPropertyChanged
    //{
    //    public event PropertyChangedEventHandler PropertyChanged;

    //    string aliasName;
    //    string fileName;
    //    string fileNameWithPath;
    //    string size;
    //    string period;
    //    string info1;
    //    string date;
    //    string originalAliasName;
    //    string rowColor;
    //    string lineColor;
    //    bool finished;
    //    string number;
    //    string prot;


    //    public ListTask(string AliasName, string FileName, string FileNameWithPath, string Size, string Period, string Info1, string Date, string RowColor, string prot, int i)
    //    {
    //        this.aliasName = AliasName;
    //        this.fileName = FileName;
    //        this.fileNameWithPath = FileNameWithPath;
    //        this.size = Size;
    //        this.period = Period;
    //        this.info1 = Info1;
    //        this.date = Date;
    //        this.rowColor = RowColor;
    //        this.originalAliasName = AliasName;
    //        //if (i == 1) this.lineColor = "Black";
    //        //else this.lineColor = "LightGray";
    //        this.lineColor = "LightGray";
    //        this.number = "";
    //        if (i == 0) this.number = "1";
    //        else if (i == 1) this.number = "REF";
    //        else if (i > 1) this.number = i.ToString();
    //        if (G.Equal(this.aliasName, "Local")) this.number = "";
    //        if (G.Equal(this.aliasName, "Global")) this.number = "";
    //        this.prot = prot;
    //    }

    //    public string Number
    //    {
    //        get { return this.number; }
    //        set
    //        {
    //            this.number = value;
    //            OnPropertyChanged("Number");
    //        }
    //    }

    //    public bool Finished
    //    {
    //        get { return this.finished; }
    //        set { this.finished = value; }
    //    }

    //    public string AliasName
    //    {
    //        get { return this.aliasName; }
    //        set
    //        {
    //            this.aliasName = value;
    //            OnPropertyChanged("AliasName");

    //            //System.ComponentModel.notify em.RaisePropertyChanged("AliasName");
    //        }
    //    }

    //    public string Prot
    //    {
    //        get { return this.prot; }
    //        set
    //        {
    //            this.prot = value;
    //            OnPropertyChanged("Prot");
    //        }
    //    }

    //    public string FileName
    //    {
    //        get { return this.fileName; }
    //    }

    //    public string FileNameWithPath
    //    {
    //        get { return this.fileNameWithPath; }
    //    }

    //    public string Size
    //    {
    //        get { return this.size; }
    //    }

    //    public string Period
    //    {
    //        get { return this.period; }
    //    }

    //    public string Info1
    //    {
    //        get { return this.info1; }
    //    }

    //    public string OriginalAliasName
    //    {
    //        get { return this.originalAliasName; }
    //    }

    //    public string Date
    //    {
    //        get { return this.date; }
    //    }

    //    public string RowColor
    //    {
    //        get { return this.rowColor; }
    //        set
    //        {
    //            this.rowColor = value;
    //            OnPropertyChanged("RowColor");
    //        }
    //    }

    //    public string LineColor
    //    {
    //        get { return this.lineColor; }
    //        set
    //        {
    //            this.lineColor = value;
    //            OnPropertyChanged("LineColor");
    //        }
    //    }

    //    // Create the OnPropertyChanged method to raise the event
    //    protected void OnPropertyChanged(string name)
    //    {
    //        PropertyChangedEventHandler handler = PropertyChanged;
    //        if (handler != null)
    //        {
    //            handler(this, new PropertyChangedEventArgs(name));
    //        }
    //    }

    //    //public override string ToString()
    //    //{
    //    //    return "hello";
    //    //}
    //}
}
