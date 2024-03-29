﻿#region license
// Copyright (c) 2022 20Road Limited
//
// This file is part of Hooks.
//
// TsGui is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, version 3 of the License.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
//
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Diagnostics;
using Core.Logging;
using WindowsHelpers;

namespace Hooks
{
    internal class Program
    {
        static void Main(string[] args)
        {
            LoggingHelpers.InitLogging();
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += OnUnhandledException;
            Arguments.ProcessArguments(args);

            if (Arguments.Install) { EnvironmentVariableHelpers.AddPath(AppDomain.CurrentDomain.BaseDirectory, EnvironmentVariableHelpers.Type.System); }
            else if (Arguments.Uninstall) { EnvironmentVariableHelpers.RemovePath(AppDomain.CurrentDomain.BaseDirectory, EnvironmentVariableHelpers.Type.System); }

            throw new KnownException("test","test");
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
            Console.WriteLine(Message);
            Environment.Exit(1);
        }
    }
}
