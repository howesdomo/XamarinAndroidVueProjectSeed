using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class HotfixCheckUpdateInfo
    {
        public bool HasHotfix { get; set; }

        public List<Item> Items { get; set; }

        public class Item
        {
            public string FileName { get; set; }

            public long FileLen { get; set; }

            public string MD5 { get; set; }

            /// <summary>
            /// 来源路径
            /// </summary>
            public string Url { get; set; }

            /// <summary>
            /// 目标路径
            /// </summary>
            public List<string> LocalPaths { get; set; }

            public Version ServerVersion { get; set; }

            // [Obsolete("xml无法成功转换Version, 临时用")]
            public string ServerVersionStr { get; set; }

            public Version LocalVersion { get; set; }
        }
    }
}
