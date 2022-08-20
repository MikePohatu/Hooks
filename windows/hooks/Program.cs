using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Diagnostics;
using Core.Logging;

namespace hooks
{
    internal class Program
    {
        static void Main(string[] args)
        {
            LoggingHelpers.InitLogging();
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(OnUnhandledException);

        }

        public static void OnUnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;
            Log.Fatal("OnUnhandledException:" + e.Message);
            HandleException(sender, e, e.StackTrace);
        }

        private static void HandleException(object sender, Exception e, string AdditionalText)
        {
            if (e is KnownException) { ShowErrorMessageAndClose((KnownException)e); }
            else
            {
                string s = "Source: " + sender.ToString() + Environment.NewLine + Environment.NewLine + "Exception: " + e.Message + Environment.NewLine;
                if (!string.IsNullOrEmpty(AdditionalText)) { s = s + Environment.NewLine + AdditionalText; }
                ShowErrorMessageAndClose(s);
            }

        }

        private static void ShowErrorMessageAndClose(KnownException e)
        {
            string msg = e.CustomMessage + Environment.NewLine + Environment.NewLine + "Full error message:" + Environment.NewLine + e.Message;
            ShowErrorMessageAndClose(msg);
        }

        private static void ShowErrorMessageAndClose(string Message)
        {
            Log.Fatal("Closing Hooks. Error message: " + Message);
            string msg = Message;
            //Director.Instance.CloseWithError("Application Runtime Exception", msg);
        }
    }
}
