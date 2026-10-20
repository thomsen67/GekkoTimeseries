using System;
using System.Threading;
using System.Windows;

namespace Gekko
{
    /// <summary>
    /// Shows the errors collected in a dlink file loop (see DlinkLoopErrors.Report), all at once in a
    /// scrollable text box. Same look as WindowDlinkGitHook.
    /// </summary>
    public partial class WindowDlinkErrors : Window
    {
        public WindowDlinkErrors(string title, string headline, string text)
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(title)) this.Title = title;
            if (!string.IsNullOrEmpty(headline)) headlineLabel.Text = headline;
            textBox1.Text = text ?? "";
            textBox1.ScrollToHome();
        }

        /// <summary>
        /// Shows the window modally and returns when the user clicks OK. Safe to call from any thread: a WPF window
        /// needs an STA thread, so from a non-STA thread the window is shown on a temporary STA thread.
        /// </summary>
        public static void ShowErrors(string title, string headline, string text)
        {
            if (Thread.CurrentThread.GetApartmentState() == ApartmentState.STA)
            {
                new WindowDlinkErrors(title, headline, text).ShowDialog();
                return;
            }
            Thread thread = new Thread(delegate ()
            {
                new WindowDlinkErrors(title, headline, text).ShowDialog();
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
