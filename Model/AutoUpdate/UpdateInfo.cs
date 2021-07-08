using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Model
{
    [DataContract]
    public class UpdateInfo
    {
        /// <summary>
        /// 服务器含有新版本
        /// </summary>
        [DataMember]
        public bool HasNewVersion { get; set; }

        /// <summary>
        /// 强制用户更新新版
        /// </summary>
        [DataMember]
        public bool IsForceUpdate { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public Version Version { get; set; }

        /// <summary>
        /// 版本字符串信息
        /// </summary>
        [DataMember]
        public string VersionStr
        {
            get
            {
                string r = null;
                if (this.Version != null)
                {
                    r = this.Version.ToString();
                }
                return r;
            }
            set
            {
                try
                {
                    this.Version = new Version(value);
                }
                catch (Exception)
                {
                    System.Diagnostics.Debugger.Break();
                }
            }
        }

        /// <summary>
        /// 更新内容项
        /// </summary>
        [DataMember]
        public List<Item> Items { get; set; }

        [DataContract]
        public class Item
        {
            /// <summary>
            /// 来源路径
            /// </summary>
            [DataMember]
            public string Url { get; set; }

            /// <summary>
            /// 目标路径
            /// </summary>
            [DataMember]
            public List<string> LocalPaths { get; set; }

            /// <summary>
            /// 文件长度
            /// </summary>
            [DataMember]
            public long FileLen { get; set; }

            /// <summary>
            /// MD5
            /// </summary>
            [DataMember]
            public string MD5 { get; set; }
        }
    }
}