using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class WebServerTest
    {
        public const string _ProjectName_ = "HoweProjectSeed";

        /// <summary>
        /// <para>项目名称</para>
        /// <para>项目名或 Guid 限定住不让程序与服务错误匹配, 防止出现类似考泰斯KOS项目KMMS项目相互混乱</para>
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 服务器当前时间
        /// </summary>
        public DateTime ServerDateTime { get; set; }
    }
}
