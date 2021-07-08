using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.Common
{
    // TODO 整合或迁移
    public interface IPowerManager
    {
        void AppRestart();

        void DeviceReboot();

        void DeviceShutdown();
    }
}

namespace Client.Droid
{
    // TODO 整合或迁移
    public class MyPowerManager : Client.Common.IPowerManager
    {
        public void AppRestart()
        {
            // 已验证, 能成功重启
            // 但如果用在Hotfix时, 拷贝新的 Client.dll 后再执行本方法会报错

            // System.BadImageFormatException: 'Method has zero rva'
            // System.TypeLoadException: 'Could not resolve type with token 010000b6 from typeref
            // (expected class 'Client.Common.StaticInfo' in assembly 'Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null')'
            // 因为已经无法在关联到旧版本
            Intent intent = new Intent(Android.App.Application.Context, typeof(MainActivity));
            intent.SetFlags(ActivityFlags.ClearTask | ActivityFlags.NewTask);
            Android.App.Application.Context.StartActivity(intent);
        }



        public void DeviceReboot()
        {
            // 未验证
            Android.OS.PowerManager s = Android.OS.PowerManager.FromContext(Android.App.Application.Context);
            s.Reboot("hotfix");
        }

        public void RootDeviceReboot()
        {
            // 未验证
            Java.Lang.Runtime.GetRuntime().Exec(new String[] { "/system/xbin/su", "-c", "reboot now" });
        }




        public void DeviceShutdown()
        {
            // 未验证
            Intent intent = new Intent();
            intent.SetAction("android.intent.action.ACTION_SHUTDOWN");
            Android.App.Application.Context.SendBroadcast(intent);
        }

        public void RootDeviceShutdown()
        {
            // 未验证
            // If your device is rooted, you can use this code for shutdown
            Java.Lang.Runtime.GetRuntime().Exec(new String[] { "/system/xbin/su", "-c", "reboot -p" });
        }


    }
}