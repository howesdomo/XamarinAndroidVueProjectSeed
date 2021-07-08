using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class DownloadFileInfo
    {
        /// <summary>
        /// 下载地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 下载到
        /// </summary>
        public string DownloadFilePath { get; set; }

        /// <summary>
        /// 最后存放到
        /// </summary>
        public string TargetFilePath { get; set; }

        /// <summary>
        /// 下载文件期望的大小值
        /// </summary>
        public long AspectFileLen { get; set; }

        /// <summary>
        /// 下载文件期望的MD5
        /// </summary>
        public string AspectMD5 { get; set; }
    }
}
