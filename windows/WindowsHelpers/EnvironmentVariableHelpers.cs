using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsHelpers
{
    public static class EnvironmentVariableHelpers
    {
        public enum Type { System, User, Process}

        public static string AddPath(string path, Type type)
        {
            string current = GetVariableValue("PATH", type);
            if (string.IsNullOrEmpty(path)) { return current; }

            string newpath = path.ToLower();
            if (newpath.EndsWith("\\"))
            {
                newpath = newpath.Substring(0, newpath.Length - 1);
            }

            string[] paths = current.Split(';');
            foreach (string p in paths)
            {
                string subpath = p;
                if (subpath.EndsWith("\\"))
                {
                    subpath = subpath.Substring(0, subpath.Length - 1);
                    //if already in path, return without adding
                    if (subpath.ToLower() == newpath) { return current; }
                }
            }

            string finalpath = path;
            if (current.EndsWith(";")) { finalpath = current + path; }
            else { finalpath = current + ";" + path; }

            SetVariable("PATH", finalpath, type);
            return finalpath;
        }

        public static void SetVariable(string variable, string value, Type type)
        {
            if (string.IsNullOrEmpty(variable)) { return; }
            if (type == Type.Process)
            {
                Environment.SetEnvironmentVariable(variable, value, EnvironmentVariableTarget.Process);
            }

            //try user variables
            if (type == Type.User)
            {
                Environment.SetEnvironmentVariable(variable, value, EnvironmentVariableTarget.User);
            }

            //try computer variables
            if (type == Type.System)
            {
                Environment.SetEnvironmentVariable(variable, value, EnvironmentVariableTarget.Machine);
            }
        }

        public static string GetVariableValue(string Variable, Type type)
        {
            if (Variable == null) { return null; }
            string s;

            //try ts env 
            //try process variables
            if (type == Type.Process)
            {
                s = Environment.GetEnvironmentVariable(Variable, EnvironmentVariableTarget.Process);
                if (!string.IsNullOrEmpty(s)) { return s; }
            }

            //try user variables
            if (type == Type.User)
            {
                s = Environment.GetEnvironmentVariable(Variable, EnvironmentVariableTarget.User);
                if (!string.IsNullOrEmpty(s)) { return s; }
            }

            //try computer variables
            if (type == Type.System)
            {
                s = Environment.GetEnvironmentVariable(Variable, EnvironmentVariableTarget.Machine);
                if (!string.IsNullOrEmpty(s)) { return s; }
            }

            //not found. return null
            return null;

        }
    }
}
