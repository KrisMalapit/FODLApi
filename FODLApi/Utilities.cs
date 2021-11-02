using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace FODLApi
{
    public class Utilities
    {
    }
    public static class Extensions
    {
        public static void WriteLog(this string str)
        {
            StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\fodllogs.txt", true);
            sw.WriteLine(str + " " + DateTime.Now);
            sw.Close();

        }
    }
}