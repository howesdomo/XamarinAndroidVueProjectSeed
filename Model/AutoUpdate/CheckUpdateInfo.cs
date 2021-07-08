using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    /// <summary>
    /// 客户端将版本信息提交到服务器, 用于检查是否需要更新
    /// </summary>
    public class CheckUpdateInfo
    {
        public UpdatePlatform UpdatePlatform { get; set; }
        
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.VersionConverter))]
        public Version Version { get; set; }

        /// <summary>
        /// 安卓版本号
        /// </summary>
        public const string VersionTemplate = "1.0.0.{0}";

    }
}
