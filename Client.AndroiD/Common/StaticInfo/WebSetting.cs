using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Common
{
    /// <summary>
    /// V 1.0.2 2021-04-03 18:11:17 Uri 属性
    /// V 1.0.1 2019-09-16 16:47:45 增加属性 IsIndependent
    /// V 1.0.0 2018-06-04 17:37:19 创建 WebSetting 类, 用于定义 .asmx, .ashx, web api 等 Web 应用
    /// </summary>
    public class WebSetting
    {
        /// <summary>
        /// 是独立的
        /// 若 '是', 执行 GetUri 只用回自身的 IP 与 Port, 不跟随 StaticInfo 的 IP, Port
        /// 若 '否', 则跟随
        /// </summary>
        public bool IsIndependent { get; set; }

        /// <summary>
        /// 服务名称，配置文件对应的名称
        /// </summary>        
        public string ServiceSettingName { get; set; }

        /// <summary>
        /// 应用程序
        /// </summary>
        public string AppName { get; set; }

        public Uri Uri { get; set; }

        public WebSetting()
        {

        }

        public WebSetting(string serviceSettingName, string uriStr, string appName, bool isIndependent = false)
        {
            this.IsIndependent = isIndependent;
            this.ServiceSettingName = serviceSettingName;
            this.AppName = appName;
            this.Uri = new Uri(uriStr).Combine(appName);
        }

        public Uri GetUri()
        {
            string r = string.Empty;

            if (IsIndependent)
            {
                r = this.Uri.ToString();
            }
            else
            {
                r = StaticInfo.Uri.Combine(this.AppName).ToString();
            }

            return new Uri(r);
        }
    }
}
