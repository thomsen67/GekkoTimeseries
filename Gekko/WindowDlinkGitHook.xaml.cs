using System;
using System.ComponentModel;
using System.Text;
using System.Windows;

namespace Gekko
{    
    
    public partial class WindowDlinkGitHook : Window
    {
        private readonly StringBuilder _log = new StringBuilder();
        private volatile bool _finished;

        public WindowDlinkGitHook(string title)
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(title)) this.Title = title;
        }

        /// <summary>
        /// Appends a line to the log text box and scrolls to it. Safe to call from any thread.
        /// </summary>
        public void AppendLine(string text)
        {
            RunOnUiThread(delegate
            {
                if (_log.Length > 0) _log.Append(Environment.NewLine);
                _log.Append(text);
                textBox1.Text = _log.ToString();
                textBox1.ScrollToEnd();
            });
        }

        /// <summary>
        /// Updates the progress bar and the label above it, e.g. "Processing 12 of 340: x.csv".
        /// Safe to call from any thread.
        /// </summary>
        public void ReportProgress(int current, int total, string currentItemLabel)
        {            
            Dispatcher.BeginInvoke((Action)delegate
            {
                int percent = total > 0 ? (int)(100.0 * current / total) : 0;
                if (percent < 0) percent = 0;
                if (percent > 100) percent = 100;
                progressBar.Value = percent;
                progressLabel.Text = "Processing " + current + " of " + total + (string.IsNullOrEmpty(currentItemLabel) ? "" : (": " + currentItemLabel));
            });
        }

        /// <summary>
        /// Call once the background work is done. Replaces the log with the final report text
        /// (pass null/empty to just leave the running log as it is), fills the progress bar, and
        /// enables the OK button so the user can dismiss the window. Safe to call from any thread.
        /// </summary>
        public void Finish(string finalReportText)
        {
            RunOnUiThread(delegate
            {
                if (!string.IsNullOrEmpty(finalReportText))
                {
                    _log.Clear();
                    _log.Append(finalReportText);
                    textBox1.Text = _log.ToString();
                    textBox1.ScrollToHome();
                }
                progressBar.Value = 100;
                progressLabel.Text = "Done.";
                _finished = true;
                button1.IsEnabled = true;
            });
        }

        private void RunOnUiThread(Action action)
        {
            if (Dispatcher.CheckAccess()) action();
            else Dispatcher.Invoke(action);
        }

        // Refuses Alt+F4/the system close button while the sync is still running -- otherwise
        // ShowDialog() in DlinkSyncFiles would return early while the worker thread is still
        // mutating shared state (getFilesNew/getFilesOverwrite/putFiles/errors) behind it.
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (!_finished) e.Cancel = true;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        
    }
}
