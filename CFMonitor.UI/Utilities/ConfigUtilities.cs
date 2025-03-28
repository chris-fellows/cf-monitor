namespace CFMonitor.UI.Utilities
{
    public static class ConfigUtilities
    {  /// <summary>
       /// Max size of image that can be uploaded
       /// </summary>
        public static long MaxUploadImageSize = 1024 * 300;

        public static long MaxUploadFileObjectSize = 1024 * 100000;

        /// <summary>
        /// Items per page on list pages (Audit Events etc)
        /// </summary>
        public static int ItemsPerListPage = 20;

        //public static string ActionItemTypeImageLocalFolder => Path.Combine(Environment.CurrentDirectory, "images", "action_item_types");
        public static string ActionItemTypeImageLocalFolder = "D:\\Data\\Dev\\C#\\cf-monitor\\CFMonitor.UI\\wwwroot\\images\\action_item_types";

        //public static string AuditEventTypeImageLocalFolder => Path.Combine(Environment.CurrentDirectory, "images", "audit_event_types");
        public static string AuditEventTypeImageLocalFolder = "D:\\Data\\Dev\\C#\\cf-monitor\\CFMonitor.UI\\wwwroot\\images\\audit_event_types";

        //public static string UserImageLocalFolder => Path.Combine(Environment.CurrentDirectory, "images", "users");
        public static string UserImageLocalFolder = "D:\\Data\\Dev\\C#\\cf-monitor\\CFMonitor.UI\\wwwroot\\images\\users";

        /// <summary>
        /// Local root folder where uploaded images are temporarily stored
        /// </summary>
        public static string ImageTempFilesRootFolder = Path.Combine(Path.GetTempPath(), "images");
    }
}
