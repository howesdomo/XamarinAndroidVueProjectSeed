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

using Util.XamariN;

namespace Client.Droid.BarcodeScanner
{
    /// <summary>
    /// V 1.0.1 - 2021-04-30 19:38:26
    /// 1. 使用单例模式
    /// 2. 注册注销广播接收器增加判断时机
    /// 
    /// V 1.0.0 - 2021-04-15 16:20:11
    /// 首次接触这款 PDA, 编写读取扫描头的方法
    /// </summary>
    public sealed class BarcodeScanner_IWRIST : IBarcodeScanner, IHardwareBarcodeScanner
    {
        #region Props

        Android.Content.IntentFilter mFilter { get; set; }

        BarcodeScanner.BroadcastReceiver_IWRIST mReceiver { get; set; }

        IWRIST_Scanner mScanner { get; set; }

        /// <summary>
        /// 已注册广播接收器
        /// </summary>
        bool mIsRegistered { get; set; }

        #endregion

        #region 单例模式

        private static BarcodeScanner_IWRIST _Instance_ { get; set; }

        private static readonly object _LOCK_ = new object();

        public static BarcodeScanner_IWRIST GetInstance()
        {
            if (_Instance_ == null)
            {
                lock (_LOCK_)
                {
                    if (_Instance_ == null)
                    {
                        _Instance_ = new BarcodeScanner_IWRIST();
                    }
                }
            }

            return _Instance_;
        }

        #endregion

        private BarcodeScanner_IWRIST()
        {
            mScanner = new IWRIST_Scanner();
            // 扫描功能
            mScanner.Open();
            mScanner.SetOutputMode(2); // 使用广播模式
            mScanner.EnablePlayBeep(false); // 关闭扫描声音
            mScanner.DefaultSetting();

            mFilter = new IntentFilter("com.android.server.scannerservice.broadcast");
            mReceiver = new BroadcastReceiver_IWRIST();
        }

        #region 实现接口 IBarcodeScanner

        public void On_Pause()
        {
            if (mIsRegistered == true)
            {
                Android.App.Application.Context.UnregisterReceiver(mReceiver);
                mIsRegistered = false;
            }
        }

        public void On_Resume()
        {
            if (mIsRegistered == false)
            {
                Android.App.Application.Context.RegisterReceiver(mReceiver, mFilter);
                mIsRegistered = true;
            }
        }

        public void On_Dispose()
        {
            if (mIsRegistered == true)
            {
                Android.App.Application.Context.UnregisterReceiver(mReceiver);
                mIsRegistered = false;
            }
        }

        #endregion

        #region 实现接口 Util.XamariN.IHardwareBarcodeScanner

        public void EnabledScanner()
        {
            mScanner.Open();
        }

        public void DisabledScanner()
        {
            mScanner.Close();
        }

        #endregion
    }

    public class BroadcastReceiver_IWRIST : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            string data = intent.GetStringExtra("scannerdata");

            Common.BarcodeScanModel scanModel = new Common.BarcodeScanModel
            (
                _BarcodeContent: data,
                _RawBarcodeContent: null,
                _BarcodeType: "IWRIST Unknown",
                _ScanTime: DateTime.Now
            );

            // 发出扫描成功音效
            App.AudioPlayer.PlayBeep();

            // ** 核心 ** 调用统一接口
            Common.BarcodeScanner.OnBarcodeScan(this, scanModel);
        }
    }

    public class IWRIST_Scanner
    {
        void scannerEnabled(bool a)
        {
            Intent intent = new Intent("com.android.scanner.ENABLED");
            intent.PutExtra("enabled", a);

            Android.App.Application.Context.SendBroadcast(intent);
        }

        public void Open()
        {
            scannerEnabled(true);
        }

        public void Close()
        {
            scannerEnabled(false);
        }

        /// <summary>
        /// <para>0 - 焦点录入</para>
        /// <para>1 - 焦点录入，覆盖已有内容</para>
        /// <para>2 - 广播</para>
        /// <para>3 - 剪贴板</para>
        /// </summary>
        /// <param name="mode"></param>
        public void SetOutputMode(int i)
        {
            string mode = string.Empty;

            switch (i)
            {
                case 0: mode = "FOCUS"; break;
                case 1: mode = "FOCUS_OVERWRITE"; break;
                case 2: mode = "BROADCAST"; break;
                case 3: mode = "CLIPBOARD"; break;
                default: mode = "BROADCAST"; break;
            }

            Intent intent = new Intent("com.android.scanner.service_settings");
            intent.PutExtra("barcode_send_mode", mode);

            Android.App.Application.Context.SendBroadcast(intent);
        }

        public void EnablePlayBeep(bool a)
        {
            Intent intent = new Intent("com.android.scanner.service_settings");
            intent.PutExtra("sound_play", a);

            Android.App.Application.Context.SendBroadcast(intent);
        }

        /// <summary>
        /// 程序接管扫描头, 将配置设置为默认
        /// </summary>
        public void DefaultSetting()
        {
            // 清除前缀
            Intent intent = new Intent("com.android.scanner.service_settings");
            intent.PutExtra("prefix", "");

            Android.App.Application.Context.SendBroadcast(intent);

            // 清除后缀
            intent = new Intent("com.android.scanner.service_settings");
            intent.PutExtra("suffix", "");

            Android.App.Application.Context.SendBroadcast(intent);

            // 结束符设为 NONE
            intent = new Intent("com.android.scanner.service_settings");
            intent.PutExtra("endchar", "NONE"); // NONE / SPACE / TAB / ENTER

            Android.App.Application.Context.SendBroadcast(intent);

            // 关闭震动
            intent = new Intent("com.android.scanner.service_settings");
            intent.PutExtra("viberate", false);

            Android.App.Application.Context.SendBroadcast(intent);

        }
    }

}