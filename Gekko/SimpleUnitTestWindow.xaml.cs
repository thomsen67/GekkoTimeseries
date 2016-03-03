using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Gekko
{
    /// <summary>
    /// Interaction logic for SimpleUnitTestWindow.xaml
    /// </summary>
    public partial class SimpleUnitTestWindow : Window
    {
        public SimpleUnitTestWindow()
        {
            InitializeComponent();
        }        

        public void WriteLine(string s) {
            UnitTestWindowStuff(s, true, false);            
        }

        public void Write(string s)
        {
            UnitTestWindowStuff(s, false, false);            
        }

        public void Clear()
        {
            UnitTestWindowStuff(null, false, true);
        }

        //This is how delegates can be done for WPF
        //weird delegate pattern, but it works!
        //NOTE: needs to set thread to STA when WPF window is created, else this will not work!
        delegate void UnitTestWindowStuffCallback(string text, bool nl, bool cls);
        public void UnitTestWindowStuff(string text, bool nl, bool cls)
        {
            if (this.richTextBox1.Dispatcher.Thread == System.Threading.Thread.CurrentThread)
            {
                if (cls) this.richTextBox1.Document.Blocks.Clear();
                else
                {
                    if (nl) this.richTextBox1.AppendText(text + G.NL);
                    else this.richTextBox1.AppendText(text);
                    this.richTextBox1.ScrollToEnd();
                }
                // It's on the same thread, no need for Invoke         
            }
            else
            {
                // It's on a different thread, so use Invoke.                
                this.richTextBox1.Dispatcher.Invoke(new UnitTestWindowStuffCallback(UnitTestWindowStuff), new object[] { text, nl, cls });                               
            }
        }
    }
}
