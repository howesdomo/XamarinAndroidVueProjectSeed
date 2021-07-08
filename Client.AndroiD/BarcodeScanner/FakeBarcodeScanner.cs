using Android.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.Droid.BarcodeScanner
{
    /// <summary>
    /// V 1.0.3 - 2021-04-30 20:53:21
    /// 1. 使用单例模式
    /// 2. 注册注销广播接收器增加判断时机
    /// 
    /// V 1.0.2 - 2020-3-20 12:03:04
    /// 兼容新版 IBarcodeScanner 接口方法
    /// 
    /// V 1.0.1 - 2020-3-19 15:19:42
    /// 统一实现接口 IHardwareBarcodeScanner
    /// 
    /// V 1.0.0
    /// 首次创建, 用于无扫码设备中
    /// </summary>
    public class FakeBarcodeScanner : IBarcodeScanner, Util.XamariN.IHardwareBarcodeScanner
    {
        #region 单例模式

        private static FakeBarcodeScanner _Instance_ { get; set; }

        private static readonly object _LOCK_ = new object();

        public static FakeBarcodeScanner GetInstance()
        {
            if (_Instance_ == null)
            {
                lock (_LOCK_)
                {
                    if (_Instance_ == null)
                    {
                        _Instance_ = new FakeBarcodeScanner();
                    }
                }
            }

            return _Instance_;
        }

        #endregion

        private FakeBarcodeScanner() { }

        #region 实现接口 IBarcodeScanner

        public void On_Pause()
        {
            System.Diagnostics.Debug.WriteLine("On_Pause");
        }

        public void On_Resume()
        {
            System.Diagnostics.Debug.WriteLine("On_Resume");
        }

        public void On_Dispose()
        {
            System.Diagnostics.Debug.WriteLine("On_Dispose");
        }

        #endregion

        #region 实现接口 Util.XamariN.IHardwareBarcodeScanner

        public void EnabledScanner()
        {
            System.Diagnostics.Debug.WriteLine("On_Enabled");
        }

        public void DisabledScanner()
        {
            System.Diagnostics.Debug.WriteLine("On_Disabled");
        }

        #endregion
    }
}