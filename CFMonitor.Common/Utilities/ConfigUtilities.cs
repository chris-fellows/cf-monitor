using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.Utilities
{
    public static class ConfigUtilities
    {
        /// <summary>
        /// Max size of image that can be uploaded
        /// </summary>
        public static long MaxUploadImageSize = 1024 * 300;

        /// <summary>
        /// Items per page on list pages (Audit Events, Issues etc0
        /// </summary>
        public static int ItemsPerListPage = 20;


        //public static string UserImageLocalFolder => Path.Combine(Environment.CurrentDirectory, "images", "users");
        public static string UserImageLocalFolder = "D:\\Data\\Dev\\C#\\cf-monitor\\CFMonitor.UI\\wwwroot\\images\\users";

        /// <summary>
        /// Local root folder where uploaded images are temporarily stored
        /// </summary>
        public static string ImageTempFilesRootFolder = Path.Combine(Path.GetTempPath(), "images");
    }
}
