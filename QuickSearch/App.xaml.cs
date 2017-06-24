using System;
using System.IO;
using System.Windows;
using System.Windows.Threading;

namespace QuickSearch
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App() : base()
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(AppUnhandledException);
        }

        private void AppUnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            var e = (Exception)args.ExceptionObject;
            Log(String.Format("{0} is happend in {1}", e.Message, e.StackTrace));
        }

        void Log(string logMessage)
        {
            using (StreamWriter w = File.AppendText("logs.txt"))
            {
                w.WriteLine(logMessage);
            }
        }
    }
}
