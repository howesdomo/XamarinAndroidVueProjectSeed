using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Common
{
    /// <summary>
    /// V 1.0.0 - 2020-3-21 12:35:17
    /// 首次稳定版标记
    /// </summary>
    public class StaticInfo
    {
        public static void Init(StaticInfoInitArgs args)
        {
            if (args.DebugMode.HasValue == true && args.DebugMode.Value >= 0)
            {
                _DebugMode = args.DebugMode.Value;
            }

            if (args.AppName.IsNullOrWhiteSpace() == false)
            {
                _AppName = args.AppName;
            }

            // 服务器配置
            InitWebServiceSettingList(args);

            #region 程序内部存储路径 - iOS 与 Android 通用

            if (args.AppPath.IsNullOrWhiteSpace() == false)
            {
                _AppPath = args.AppPath;
            }

            if (args.AppFilesPath.IsNullOrWhiteSpace() == false)
            {
                _AppFilesPath = args.AppFilesPath;
            }

            if (args.AppCachePath.IsNullOrWhiteSpace() == false)
            {
                _AppCachePath = args.AppCachePath;
            }

            #endregion

            // Android 特有
            if (Xamarin.Essentials.DeviceInfo.Platform == Xamarin.Essentials.DevicePlatform.Android)
            {
                #region 外部存储路径 - Android 特有

                // 安卓系统外部存储根目录
                if (args.AndroidExternalRoot.IsNullOrWhiteSpace() == false)
                {
                    _AndroidExternalRoot = args.AndroidExternalRoot;
                }

                // 程序外部存储绝对路径
                if (args.AndroidExternalPath.IsNullOrWhiteSpace() == false)
                {
                    _AndroidExternalPath = args.AndroidExternalPath;
                }

                // 程序外部存储cache -- /{安卓系统外部存储路径}/Android/data/{appPackageName}/cache
                if (args.AndroidExternalCachePath.IsNullOrWhiteSpace() == false)
                {
                    _AndroidExternalCachePath = args.AndroidExternalCachePath;
                }

                // 程序外部存储files -- /{安卓系统外部存储路径}/Android/data/{appPackageName}/files
                if (args.AndroidExternalFilesPath.IsNullOrWhiteSpace() == false)
                {
                    _AndroidExternalFilesPath = args.AndroidExternalFilesPath;
                }

                #endregion

                StaticInfo.AndroidVersionCode = args.AndroidVersionCode;
                StaticInfo.AndroidVersionName = args.AndroidVersionName;
            }

            #region InnerSQLite

            //if (args.InnerSQLiteConnStr.IsNullOrWhiteSpace() == false)
            //{
            //    StaticInfo.InnerSQLiteConnStr = args.InnerSQLiteConnStr;
            //}

            #endregion

            #region ExternalSQLite

            //if (args.ExternalSQLiteConnStr.IsNullOrWhiteSpace() == false)
            //{
            //    StaticInfo.ExternalSQLiteConnStr = args.ExternalSQLiteConnStr;
            //}

            #endregion
        }

        public static void InitWebServiceSettingList(StaticInfoInitArgs args)
        {
            if (args.Uri != null)
            {
                _Uri = args.Uri;
            }

            if (args.AppWebSetting != null)
            {
                AppWebSetting = args.AppWebSetting;
            }

            if (args.BillboardWebSetting != null)
            {
                BillboardWebSetting = args.BillboardWebSetting;
            }

            if (args.LBSWebSetting != null)
            {
                LBSWebSetting = args.LBSWebSetting;
            }

            if (args.SMSWebSetting != null)
            {
                SMSWebSetting = args.SMSWebSetting;
            }
        }

        private static int _DebugMode = 0;
        public static int DebugMode
        {
            get
            {
                return _DebugMode;
            }
            set
            {
                _DebugMode = value;
            }
        }

        private static String _AppName = "HoweApp";
        /// <summary>
        /// 程序名称
        /// </summary>
        public static string AppName
        {
            get
            {
                return _AppName;
            }
        }


        /// <summary>
        /// 当前登录用户
        /// </summary>
        public static SecurityModel.User CurrentUser { get; set; }


        //private static Util.XamariN.Essentials.DeviceInfo _DeviceInfo;
        ///// <summary>
        ///// 当前运行设备信息
        ///// </summary>
        //public static Util.XamariN.Essentials.DeviceInfo DeviceInfo
        //{
        //    get
        //    {
        //        if (_DeviceInfo == null)
        //        {
        //            //Util.XamariN.Essentials.IDeviceInfoUtils match = Xamarin.Forms.DependencyService.Get<Util.XamariN.Essentials.IDeviceInfoUtils>();
        //            //if (match == null)
        //            //{
        //            //    throw new Exception("未能找到实现 Util.XamariN.Essentials.IDeviceInfoUtils 的依赖");
        //            //}
        //            //else
        //            //{
        //            //    _DeviceInfo = match.GetDeviceInfo(); // 全局捕获异常后，执行此处会导致闪退，在启动程序的时候就执行一次
        //            //                                         // 让变量不为空值就没有问题了
        //            //}

        //            _DeviceInfo = new Util.XamariN.Essentials.DeviceInfo();
        //        }
        //        return _DeviceInfo;
        //    }
        //}

        /// <summary>
        /// 当前运行设备信息（字符串）
        /// </summary>
        public static string DeviceInfoStr
        {
            get
            {
                int totalWidth = 20;
                char paddingChar = ' ';
                string temp = $"{"Model".PadRight(totalWidth, paddingChar)}:{{0}}\r\n{"Manufacturer".PadRight(totalWidth, paddingChar)}:{{1}}\r\n{"DeviceName".PadRight(totalWidth, paddingChar)}:{{2}}\r\n{"Version".PadRight(totalWidth, paddingChar)}:{{3}}\r\n{"Platform".PadRight(totalWidth, paddingChar)}:{{4}}\r\n{"Idiom".PadRight(totalWidth, paddingChar)}:{{5}}\r\n{"DeviceType".PadRight(totalWidth, paddingChar)}:{{6}}";

                return string.Format
                (
                    temp,
                    Xamarin.Essentials.DeviceInfo.Model,
                    Xamarin.Essentials.DeviceInfo.Manufacturer,
                    Xamarin.Essentials.DeviceInfo.Name, // DeviceName
                    Xamarin.Essentials.DeviceInfo.Version, // VersionInfo
                    Xamarin.Essentials.DeviceInfo.Platform,
                    Xamarin.Essentials.DeviceInfo.Idiom,
                    Xamarin.Essentials.DeviceInfo.DeviceType
                );
            }
        }

        /// <summary>
        /// 当前运行设备平台
        /// </summary>
        public static string DeviceInfo_Platform
        {
            get
            {
                //if (DeviceInfo == null)
                //{
                //    return string.Empty;
                //}
                //else
                //{
                //    return DeviceInfo.Platform.ToUpper();
                //}

                return Xamarin.Essentials.DeviceInfo.Platform.ToString().ToUpper();
            }
        }

        ///// <summary>
        ///// 当前运行设备显示信息
        ///// </summary>
        //public static Util.XamariN.Essentials.DisplayInfo DisplayInfo
        //{
        //    get
        //    {
        //        //// 采用依赖注入接口方式处理 Android / iOS 的调用问题
        //        //// 具体如何实现接口请参考 CoreUtil.XamariN.AndroiD , DisplayInfoUtils

        //        //// 由于屏幕会旋转, 所以每次都获取最新信息
        //        //Util.XamariN.Essentials.IDisplayInfoUtils match = Xamarin.Forms.DependencyService.Get<Util.XamariN.Essentials.IDisplayInfoUtils>();
        //        //if (match == null)
        //        //{
        //        //    throw new Exception("未能找到实现 Util.XamariN.Essentials.IDisplayInfoUtils 的依赖");
        //        //}
        //        //else
        //        //{
        //        //    return match.GetDisplayInfo();
        //        //}

        //        // 由于屏幕会旋转, 所以每次都获取最新信息
        //        return new Util.XamariN.Essentials.DisplayInfo();
        //    }
        //}

        public static Version AppVersion
        {
            get
            {
                var r = Xamarin.Essentials.AppInfo.Version;
                
                if (Xamarin.Essentials.DeviceInfo.Platform == Xamarin.Essentials.DevicePlatform.Android)
                {
                    // 如果是安卓平台, 读取 StaticInfo.AndroidVersionCode (数据类型 long 或者 int),
                    // 然后在 VersionTemplate 中 ( 1.0.0.X ) 替换掉 X
                    // 使用 Version 的原因是方便比较
                    r = new Version(string.Format(Model.CheckUpdateInfo.VersionTemplate, StaticInfo.AndroidVersionCode));
                }

                return r;
            }
        }

        public static string AppVersionInfo
        {
            get
            {
                string r = string.Empty;

                if (Xamarin.Essentials.DeviceInfo.Platform == Xamarin.Essentials.DevicePlatform.Android)
                {
                    r = AndroidVersionCode.ToString();
                }
                else
                {
                    // System.Diagnostics.Debug.WriteLine("未能测试其他版本");
                    // System.Diagnostics.Debugger.Break();
                    r = AppVersion.ToString();
                }

                return r;
            }
        }

        #region 捕获全局异常 + 记录异常日志

        /// <summary>
        /// 错误日志文件夹 DirectoryInfo
        /// </summary>
        public static System.IO.DirectoryInfo ErrorsDirectoryInfo
        {
            get
            {
                System.IO.DirectoryInfo r = null;

                string folder = "Errors";

                if (Xamarin.Essentials.DeviceInfo.Platform == Xamarin.Essentials.DevicePlatform.Android)
                {
                    string filePath = System.IO.Path.Combine(StaticInfo.AndroidExternalFilesPath, folder);
                    r = new System.IO.DirectoryInfo(filePath);
                }
                else if (Xamarin.Essentials.DeviceInfo.Platform == Xamarin.Essentials.DevicePlatform.iOS)
                {
                    string filePath = System.IO.Path.Combine(StaticInfo.AppFilesPath, folder);
                    r = new System.IO.DirectoryInfo(filePath);
                }
                else
                {
                    throw new NotImplementedException();
                }

                return r;
            }
        }

        /// <summary>
        /// 致命异常（闪退）日志文件 FileInfo
        /// </summary>
        public static System.IO.FileInfo FatalLogFileInfo
        {
            get
            {
                System.IO.FileInfo r = null;

                string errorFolder = StaticInfo.ErrorsDirectoryInfo.FullName;
                string fileName = "fatal.txt";

                if (Xamarin.Essentials.DeviceInfo.Platform == Xamarin.Essentials.DevicePlatform.Android)
                {
                    string filePath = System.IO.Path.Combine(errorFolder, fileName);
                    r = new System.IO.FileInfo(filePath);
                }
                else if (Xamarin.Essentials.DeviceInfo.Platform == Xamarin.Essentials.DevicePlatform.iOS)
                {
                    string filePath = System.IO.Path.Combine(errorFolder, fileName);
                    r = new System.IO.FileInfo(filePath);
                }
                else
                {
                    throw new NotImplementedException();
                }

                return r;
            }
        }

        /// <summary>
        /// 记录致命异常（闪退）日志文件
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="exFrom"></param>
        /// <returns></returns>
        public static bool SaveFatalLog(Exception ex, string exFrom)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                if (CurrentUser == null) // 没有登陆的用户
                {
                    CurrentUser = new SecurityModel.User()
                    {
                        Id = Guid.Empty,
                        LoginAccount = "Unlogin",
                        UserName = "Unlogin"
                    };
                }

                sb.AppendLine(StaticInfo.CurrentUser.Id.ToString());
                sb.AppendLine(exFrom);
                sb.AppendLine("异常信息:");
                sb.AppendLine(ex.GetInfo());
                sb.AppendLine();
                sb.AppendLine($"程序版本:{StaticInfo.AppVersionInfo}");
                sb.AppendLine();
                sb.AppendLine("设备信息:");
                sb.AppendLine(StaticInfo.DeviceInfoStr);
                sb.AppendLine();
                sb.AppendLine("账号信息:");
                sb.AppendLine(StaticInfo.CurrentUser.LoginAccount);
                sb.AppendLine(StaticInfo.CurrentUser.UserName);

                if (FatalLogFileInfo.Directory.Exists == false)
                {
                    FatalLogFileInfo.Directory.Create();
                }

                System.IO.File.AppendAllText(FatalLogFileInfo.FullName, sb.ToString());

                return true;
            }
            catch (Exception subEx)
            {
                string error = subEx.GetInfo();
                System.Diagnostics.Debugger.Break();
                return false;
            }
        }

        #endregion

        public static Uri _Uri;
        public static Uri Uri
        {
            get { return _Uri; }
            set
            {
                _Uri = value;
                ServiceSettingsUtils.UpdateUri(value);
            }
        }


        /// <summary>
        /// App Web 服务配置
        /// </summary>
        public static WebSetting AppWebSetting { get; set; }

        /// <summary>
        /// 看板服务设置
        /// </summary>
        public static WebSetting BillboardWebSetting { get; set; }

        /// <summary>
        /// LBS(地理位置)微服务设置(独立)
        /// </summary>
        public static WebSetting LBSWebSetting { get; set; }

        /// <summary>
        /// 短信微服务设置(独立)
        /// </summary>
        public static WebSetting SMSWebSetting { get; set; }



        #region 程序内部存储路径 - iOS 与 Android 通用

        private static string _AppPath;
        public static string AppPath
        {
            get { return _AppPath; }
        }

        private static string _AppFilesPath;
        public static string AppFilesPath
        {
            get { return _AppFilesPath; }
        }

        private static string _AppCachePath;
        public static string AppCachePath
        {
            get { return _AppCachePath; }
        }

        #endregion

        #region 外部存储路径 - Android 特有

        private static string _AndroidExternalRoot;
        /// <summary>
        /// 安卓系统外部存储根目录
        /// </summary>
        public static string AndroidExternalRoot
        {
            get { return _AndroidExternalRoot; }
        }

        private static string _AndroidExternalPath;
        /// <summary>
        /// App外部存储根目录
        /// </summary>
        public static string AndroidExternalPath
        {
            get { return _AndroidExternalPath; }
        }

        private static string _AndroidExternalFilesPath;
        /// <summary>
        /// App外部存储的Files
        /// </summary>
        public static string AndroidExternalFilesPath
        {
            get { return _AndroidExternalFilesPath; }
        }

        private static string _AndroidExternalCachePath;
        /// <summary>
        /// App外部存储的cache
        /// </summary>
        public static string AndroidExternalCachePath
        {
            get { return _AndroidExternalCachePath; }
        }

        #endregion

        /// <summary>
        /// Android 特有 - 有关APK判断能否安装新版
        /// </summary>
        public static long AndroidVersionCode { get; set; }

        /// <summary>
        /// Android 特有
        /// </summary>
        public static string AndroidVersionName { get; set; }

        #region InnerSQLite

        ///// <summary>
        ///// 程序内部SQLite数据库连接字符串
        ///// </summary>
        //public static string InnerSQLiteConnStr { get; private set; }

        //private static Client.Data.SQLiteDB _InnerSQLiteDB;

        ///// <summary>
        ///// 程序内部SQLite数据库
        ///// </summary>
        //public static Client.Data.SQLiteDB InnerSQLiteDB
        //{
        //    get
        //    {
        //        if (_InnerSQLiteDB == null)
        //        {
        //            if (StaticInfo.InnerSQLiteConnStr.IsNullOrWhiteSpace() == false)
        //            {
        //                _InnerSQLiteDB = new Client.Data.SQLiteDB(Util.Data_SQLite.LocationEnum.Inner, InnerSQLiteConnStr);
        //            }
        //        }
        //        return _InnerSQLiteDB;
        //    }
        //}

        #endregion

        #region ExternalSQLite

        //public static string ExternalSQLiteConnStr { get; private set; }

        //private static Client.Data.SQLiteDB _ExternalSQLiteDB;

        ///// <summary>
        ///// 外部存储器SQLite数据库
        ///// </summary>
        //public static Client.Data.SQLiteDB ExternalSQLiteDB
        //{
        //    get
        //    {
        //        if (_ExternalSQLiteDB == null)
        //        {
        //            if (StaticInfo.ExternalSQLiteConnStr.IsNullOrWhiteSpace() == false)
        //            {

        //                var fileInfo = new System.IO.FileInfo(StaticInfo.ExternalSQLiteConnStr);
        //                if (System.IO.Directory.Exists(fileInfo.DirectoryName) == false)
        //                {
        //                    System.IO.Directory.CreateDirectory(fileInfo.DirectoryName);
        //                }

        //                _ExternalSQLiteDB = new Client.Data.SQLiteDB(Util.Data_SQLite.LocationEnum.External, ExternalSQLiteConnStr);
        //            }
        //        }
        //        return _ExternalSQLiteDB;
        //    }
        //}

        #endregion
    }
}