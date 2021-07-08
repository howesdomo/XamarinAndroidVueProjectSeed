using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Client.Common
{
    /// <summary>
    /// ServiceSettingsUtils 各个项目自由编写区域
    /// </summary>
    public partial class ServiceSettingsUtils
    {
        public static void InitConfig(string path)
        {
            // 新版 XML+Json 的配置文件
            List<WebSetting> list = new List<WebSetting>();

            // Step 1 修改 IP Port            
#if DEBUG
            Uri u = new Uri($"https://192.168.1.215:44391/");
#elif RELEASE
            Uri u = new Uri($"https://192.168.1.215:44391/");
#endif

            // Step 2 修改 WebSetting
            #region 各个程序自行配置

            //list.Add(new WebSetting
            //(
            //    isIndependent: false,
            //    serviceSettingName: WebServer.Tag,
            //    uriStr: u.ToString(),
            //    appName: string.Empty
            //));

            //list.Add(new WebSetting
            //(
            //    isIndependent: false,
            //    serviceSettingName: BillboardServer.Tag,
            //    uriStr: u.ToString(),
            //    appName: "Billboard"
            //));

            //list.Add(new WebSetting
            //(
            //    isIndependent: true,
            //    serviceSettingName: LBSServer.Tag,
            //    uriStr: "https://lbs.baidu.com/",
            //    appName: string.Empty
            //));

            //list.Add(new WebSetting
            //(
            //    isIndependent: true,
            //    serviceSettingName: SMSServer.Tag,
            //    uriStr: "https://sms.baidu.com/",
            //    appName: string.Empty
            //));

            #endregion

            XDocument xDoc = new XDocument
            (
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement
                (
                    "configuration",

                    new XComment("通用Uri"),
                    new XElement
                    (
                        "Uri", u.ToString()
                    ),

                    new XComment("服务器配置集合"),
                    new XElement
                    (
                        "WebSettings",
                        list.Select
                        (
                            i => new XElement
                            (
                                "WebSetting",
                                new XAttribute("name", i.ServiceSettingName), // attr name
                                new XAttribute("class", i.GetType().FullName), // class attr
                                Util.JsonUtils.SerializeObjectWithFormatted(i) // value
                            )
                        )
                    )
                )
            );

            xDoc.Save(path);
        }

        public static Uri ReadUri(string path)
        {
            // 新版 XML+Json 的配置文件
            XDocument xDoc = XDocument.Load(path);
            var uriStr = ConfigUtils.GetObject<string>(xDoc: xDoc, descendantsName: "Uri");
            return new Uri(uriStr);
        }

        public static List<WebSetting> ReadWebServiceSettingList(string path)
        {
            // 新版 XML+Json 的配置文件
            XDocument xDoc = XDocument.Load(path);
            return ConfigUtils.GetList<WebSetting>(xDoc: xDoc, descendantsName: "WebSetting");
        }

        /// <summary>
        /// 各个程序自行配置
        /// </summary>
        /// <param name="webSettingList"></param>
        /// <param name="staticInfoInitArgs"></param>
        public static void EveryAppDefine(List<WebSetting> webSettingList, Common.StaticInfoInitArgs staticInfoInitArgs)
        {
            //staticInfoInitArgs.AppWebSetting = webSettingList.FirstOrDefault(i => i.ServiceSettingName == WebServer.Tag);
            //staticInfoInitArgs.BillboardWebSetting = webSettingList.FirstOrDefault(i => i.ServiceSettingName == BillboardServer.Tag);
            //staticInfoInitArgs.LBSWebSetting = webSettingList.FirstOrDefault(i => i.ServiceSettingName == LBSServer.Tag);
            //staticInfoInitArgs.SMSWebSetting = webSettingList.FirstOrDefault(i => i.ServiceSettingName == SMSServer.Tag);
        }
    }
}
