
using System;

namespace AppData.Model
{
    public class Store
    {
        public static string ScreenshotDirectory { get; set; }


        public string GetFileName(string name, string username)
        {
            return ScreenshotDirectory + "\\" + DateTime.Now.ToString("yyyyMMdd_HHmmss tt") + name +" "+ username + ".jpeg";
        }
    }
}