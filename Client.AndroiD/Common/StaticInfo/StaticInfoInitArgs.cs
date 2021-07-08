using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Common
{
    /// <summary>
    /// V 1.0.0 - 2020-3-21 12:35:17
    /// 首次稳定版标记
    /// </summary>
    public class StaticInfoInitArgs
    {
        /// <summary>
        /// 调试模式
        /// </summary>
        public int? DebugMode { get; set; }

        /// <summary>
        /// 程序名称
        /// </summary>
        public string AppName { get; set; }

        public string IP { get; set; }

        public string Port { get; set; }

        public Uri Uri { get; set; }

        /// <summary>
        /// App Web 服务配置
        /// </summary>
        public WebSetting AppWebSetting { get; set; }

        /// <summary>
        /// 看板服务设置
        /// </summary>
        public WebSetting BillboardWebSetting { get; set; }

        /// <summary>
        /// LBS(地理位置)微服务设置(独立)
        /// </summary>
        public WebSetting LBSWebSetting { get; set; }

        /// <summary>
        /// 短信微服务设置(独立)
        /// </summary>
        public WebSetting SMSWebSetting { get; set; }

        #region 程序内部存储路径 - iOS 与 Android 通用

        public string AppPath { get; set; }

        public string AppFilesPath { get; set; }

        public string AppCachePath { get; set; }

        #endregion

        #region 外部存储路径 - Android 特有

        /// <summary>
        /// 安卓系统外部存储根目录
        /// </summary>
        public string AndroidExternalRoot { get; set; }

        /// <summary>
        /// App外部存储根目录
        /// </summary>
        public string AndroidExternalPath { get; set; }

        /// <summary>
        /// App外部存储的Files
        /// </summary>
        public string AndroidExternalFilesPath { get; set; }

        /// <summary>
        /// App外部存储的cache
        /// </summary>
        public string AndroidExternalCachePath { get; set; }

        #endregion

        /// <summary>
        /// Android 特有 - 有关APK判断能否安装新版
        /// </summary>
        public long AndroidVersionCode { get; set; }

        /// <summary>
        /// Android 特有
        /// </summary>
        public string AndroidVersionName { get; set; }


        /// <summary>
        /// 程序内部SQLite数据库连接字符串
        /// </summary>
        public string InnerSQLiteConnStr { get; set; }

        /// <summary>
        /// 外部存储器SQLite数据库连接字符串
        /// </summary>
        public string ExternalSQLiteConnStr { get; set; }
    }
}
