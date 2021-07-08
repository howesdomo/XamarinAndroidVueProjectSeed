using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

// 调用Jar包
using Com.Android.Barcodescandemo;

namespace Client.Droid.BarcodeScanner
{
    /// <summary>
    /// V 1.0.2 - 2021-04-30 19:38:26
    /// 1. 使用单例模式
    /// 2. 注册注销广播接收器增加判断时机
    /// 
    /// V 1.0.1 
    /// 实现接口 Util.XamariN.IHardwareBarcodeScanner
    /// 扫描设备的硬关闭， 使触发扫描按钮无法发出红光, 提醒用户有异常信息未处理。
    /// 
    /// V 1.0.0 - 2020-3-20 12:03:44
    /// 首次创建
    /// </summary>
    public sealed class BarcodeScanner_iData_iScan : IBarcodeScanner, Util.XamariN.IHardwareBarcodeScanner
    {
        #region Props

        Android.Content.IntentFilter mFilter { get; set; }

        Com.Android.Barcodescandemo.ScannerInerface mScanner { get; set; }

        BarcodeScanner.BroadcastReceiver_iData_9105 mReceiver { get; set; }

        /// <summary>
        /// 已注册广播接收器标记
        /// </summary>
        bool mIsRegistered { get; set; }

        #endregion

        #region 单例模式

        private static BarcodeScanner_iData_iScan _Instance_ { get; set; }

        private static readonly object _LOCK_ = new object();

        public static BarcodeScanner_iData_iScan GetInstance()
        {
            if (_Instance_ == null)
            {
                lock (_LOCK_)
                {
                    if (_Instance_ == null)
                    {
                        _Instance_ = new BarcodeScanner_iData_iScan();
                    }
                }
            }

            return _Instance_;
        }

        #endregion

        private BarcodeScanner_iData_iScan()
        {
            mScanner = new ScannerInerface(Android.App.Application.Context);
            // 扫描功能
            mScanner.Open();
            mScanner.SetOutputMode(1);//使用广播模式
            mScanner.EnablePlayBeep(false); // 关闭扫描声音

            mFilter = new IntentFilter("android.intent.action.SCANRESULT");
            mReceiver = new BroadcastReceiver_iData_9105();
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

    public class BroadcastReceiver_iData_9105 : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            // final String data = intent.getStringExtra("value");
            string data = intent.GetStringExtra("value");

            Common.BarcodeScanModel scanModel = new Common.BarcodeScanModel
            (
                _BarcodeContent: data,
                _RawBarcodeContent: null,
                _BarcodeType: "iData Unknown",
                _ScanTime: DateTime.Now
            );

            // 发出扫描成功音效
            App.AudioPlayer.PlayBeep();

            // ** 核心 ** 调用统一接口
            Common.BarcodeScanner.OnBarcodeScan(this, scanModel);
        }
    }
}