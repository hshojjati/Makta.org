using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Makta.Areas._Setting.Models
{
    public static class WebSiteSetting
    {
        public static CommonSetting Common { get; set; }
        public static EmailSetting EmailSetting { get; set; }
        public static string ProductVersion = $"{Assembly.GetExecutingAssembly().GetName().Version}-{Assembly.GetExecutingAssembly().GetLinkerTime()}";

        private static string GetLinkerTime(this Assembly assembly)
        {
            DateTime buildDate = new FileInfo(Assembly.GetExecutingAssembly().Location).LastWriteTime;
            return buildDate.ToString("yyyy-MM-dd");
        }
    }
}
