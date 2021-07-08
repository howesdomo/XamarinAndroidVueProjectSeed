using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// 本文件用于设置 最通用 & 最基本的配置 ( 方便拷贝到其他项目中使用 )
/// 请将项目中较为特殊的设置写在对应的 XxxSettingUtils.cs 内
/// </summary>
namespace Client.Common
{

    /// <summary>
    /// V 1.0.0 - 2019-9-17 15:37:39 编写 ServiceSettingsUtils 基础代码种子
    /// </summary>
    public partial class ServiceSettingsUtils
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public static string ConfigFileName
        {
            get
            {
                return "ServiceSettings.config";
            }
        }

        /// <summary>
        /// 获取文件路径
        /// </summary>
        /// <param name="argsDirPath">传入文件夹路径(可空)</param>
        /// <returns>配置文件路径</returns>
        public static string GetConfigFilePath(string argsDirPath = "")
        {
            if (argsDirPath.IsNullOrEmpty())
            {
                if (Xamarin.Essentials.DeviceInfo.Platform == Xamarin.Essentials.DevicePlatform.Android)
                {
                    return System.IO.Path.Combine(StaticInfo.AndroidExternalFilesPath, ConfigFileName);
                }
                else
                {
                    return System.IO.Path.Combine(StaticInfo.AppFilesPath, ConfigFileName);
                }
            }
            else
            {
                return System.IO.Path.Combine(argsDirPath, ConfigFileName);
            }
        }

        [Obsolete]
        public static void UpdateIPOrAddress(string newValue)
        {
            string name = "IP";
            System.Xml.Linq.XDocument xDoc = System.Xml.Linq.XDocument.Load(GetConfigFilePath());
            ConfigUtils.GetElement(xDoc, name).Value = newValue;
            xDoc.Save(GetConfigFilePath());
        }

        [Obsolete]
        public static void UpdatePort(string newValue)
        {
            string name = "Port";
            System.Xml.Linq.XDocument xDoc = System.Xml.Linq.XDocument.Load(GetConfigFilePath());
            ConfigUtils.GetElement(xDoc, name).Value = newValue;
            xDoc.Save(GetConfigFilePath());
        }

        public static void UpdateUri(Uri newValue)
        {
            saveXML(newValue);
            reloadWebServiceSettingListForStaticInfo();
        }

        /// <summary>
        /// <para>1.更新服务器Uri值; </para>
        /// <para>2.为非独立的WebService更新最新的Uri;</para>
        /// <para>3.保存XML</para>
        /// </summary>
        /// <param name="newValue"></param>
        static void saveXML(Uri newValue)
        {
            System.Xml.Linq.XDocument xDoc = System.Xml.Linq.XDocument.Load(GetConfigFilePath());
            ConfigUtils.Update(xDoc, "Uri", value: newValue.ToString()); // 更新 Uri

            List<WebSetting> webSettingList = ConfigUtils.GetList<WebSetting>(xDoc: xDoc, descendantsName: "WebSetting");
            for (int i = 0; i < webSettingList.Count; i++)
            {
                WebSetting item = webSettingList[i];

                if (item.IsIndependent == true)
                {
                    continue;
                }

                item.Uri = newValue.Combine(item.AppName); // 为非独立的WebService更新最新的Uri

                foreach (var element in xDoc.Descendants("WebSetting"))
                {
                    string value = element.Value;
                    var nameAttr = element.Attribute("name");
                    if (nameAttr.Value == item.ServiceSettingName)
                    {
                        element.Value = Util.JsonUtils.SerializeObjectWithFormatted(item);
                        break;
                    }
                }
            }

            xDoc.Save(GetConfigFilePath());
        }

        /// <summary>
        /// <para>1. 读取配置文件的Uri</para>
        /// <para>2. 读取配置文件的各个WebServiceSetting</para>
        /// <para>3. 将以上读取的值更新到 StaticInfo 中</para>
        /// </summary>
        static void reloadWebServiceSettingListForStaticInfo()
        {
            var args = new Common.StaticInfoInitArgs();

            string configFilePath = GetConfigFilePath();

            // 1. 读取配置文件的Uri
            args.Uri = Common.ServiceSettingsUtils.ReadUri(configFilePath);

            // 2. 读取配置文件的各个WebServiceSetting
            var list = Common.ServiceSettingsUtils.ReadWebServiceSettingList(configFilePath);
            Common.ServiceSettingsUtils.EveryAppDefine(list, args);

            // 3. 将以上读取的值更新到 StaticInfo 中
            Common.StaticInfo.InitWebServiceSettingList(args);
        }
    }

    /// <summary>
    /// V 1.0.0 - 2019-9-17 15:37:39 编写 NativeSettingsUtils 基础代码种子
    /// </summary>
    public partial class NativeSettingsUtils
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public static string ConfigFileName
        {
            get
            {
                return "NativeSettings.config";
            }
        }

        /// <summary>
        /// 获取文件路径
        /// </summary>
        /// <param name="argsDirPath">传入文件夹路径(可空)</param>
        /// <returns>配置文件路径</returns>
        public static string GetConfigFilePath(string argsDirPath = "")
        {
            if (argsDirPath.IsNullOrEmpty() == true)
            {
                return System.IO.Path.Combine(StaticInfo.AppFilesPath, ConfigFileName);
            }
            else
            {
                return System.IO.Path.Combine(argsDirPath, ConfigFileName);
            }
        }

        public static void UpdateDebugMode(string newValue)
        {
            string name = "DebugMode";
            System.Xml.Linq.XDocument xDoc = System.Xml.Linq.XDocument.Load(GetConfigFilePath());
            ConfigUtils.GetElement(xDoc, name).Value = newValue;
            xDoc.Save(GetConfigFilePath());
        }
    }
}
