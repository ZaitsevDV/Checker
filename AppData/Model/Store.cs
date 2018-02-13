
using System;
using System.Collections.Generic;

namespace AppData.Model
{
    public class Store
    {
        public static string Directory { get; set; }
        public static string SourceFile { get; set; }
        public static List<User> Users { get; set; }


        public string LogFileName(string name)
        {
            return Directory + "\\" + DateTime.Now.ToString("yyyyMMdd tt") + name + " " + "log.txt";
        }

        public string ScreenShotFileName(string name, string username)
        {
            return Directory + "\\" + DateTime.Now.ToString("yyyyMMdd_HHmmss tt") + name +" "+ username + ".jpeg";
        }
    }
}