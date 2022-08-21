#region license
// Copyright (c) 2020 Mike Pohatu
//
// This file is part of TsGui.
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

// Arguments.cs - objec to process and store command line arguments

using System;

using Core.Diagnostics;

namespace Hooks
{
    public static class Arguments
    {
        public static bool Install { get; private set; } = false;
        public static bool Uninstall { get; private set; } = false;
        public static string LogFile { get; private set; }
        public static int LoggingLevel { get; private set; }

        internal static void ProcessArguments(string[] Args)
        {
            //string[] args = Environment.GetCommandLineArgs();
            if (Args.Length > 0)
            {
                for (int index = 0; index < Args.Length; index += 1)
                {                  
                    switch (Args[index].ToUpper())
                    {
                        case "-LOG":
                            if (Args.Length < index + 2) { throw new InvalidOperationException("Missing config file after parameter -log"); }
                            LogFile = CompleteFilePath(Args[index + 1]);
                            index++;
                            break;
                        case "-INSTALL":
                            Install = true;
                            break;
                        case "-UNINSTALL":
                            Uninstall = true;
                            break;
                        default:
                            throw new InvalidOperationException("Invalid parameter: " + Args[index]);
                    }                   
                }
            }
        }

        private static string CompleteFilePath(string Input)
        {
            if (!Input.Contains("\\"))
            {
                string exefolder = AppDomain.CurrentDomain.BaseDirectory;
                return exefolder + @Input;
            }
            else { return Input; }
        }
    }
}
