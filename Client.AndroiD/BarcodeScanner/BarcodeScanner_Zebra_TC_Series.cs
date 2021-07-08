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

using Symbol.XamarinEMDK;
using Symbol.XamarinEMDK.Barcode;

namespace Client.Droid.BarcodeScanner
{
    /// <summary>
    /// V 1.0.4 - 2021-04-30 21:05:03
    /// 1. 使用单例模式
    /// 2. 注册注销广播接收器增加判断时机
    /// 
    /// V 1.0.3 - 2020-9-3 17:05:46
    /// 由 BarcodeScanner_Zebra_TC20 更名为 BarcodeScanner_Zebra_TC_Series
    /// 
    /// V 1.0.2 - 2020-3-20 12:00:26
    /// 兼容新版 IBarcodeScanner 接口方法
    /// 
    /// V 1.0.1 - 2020-3-19 15:19:42
    /// 实现接口 IHardwareBarcodeScanner, 实现了扫描设备硬启动与硬关闭。
    /// 扫描设备的硬关闭， 使触发扫描按钮无法发出红光, 提醒用户有异常信息未处理。
    /// 
    /// V 1.0.0
    /// 首次创建
    /// </summary>
    public class BarcodeScanner_Zebra_TC_Series : Activity, EMDKManager.IEMDKListener, IBarcodeScanner, Util.XamariN.IHardwareBarcodeScanner
    {
        #region Props

        EMDKManager mEMDKManager { get; set; }

        BarcodeManager mBarcodeManager { get; set; }

        Scanner mScanner { get; set; }

        #endregion

        #region 单例模式

        private static BarcodeScanner_Zebra_TC_Series _Instance_ { get; set; }

        private static readonly object _LOCK_ = new object();

        public static BarcodeScanner_Zebra_TC_Series GetInstance()
        {
            if (_Instance_ == null)
            {
                lock (_LOCK_)
                {
                    if (_Instance_ == null)
                    {
                        _Instance_ = new BarcodeScanner_Zebra_TC_Series();
                    }
                }
            }

            return _Instance_;
        }

        #endregion

        private BarcodeScanner_Zebra_TC_Series()
        {
            EMDKResults results = EMDKManager.GetEMDKManager(Android.App.Application.Context, this);

            if (results.StatusCode != EMDKResults.STATUS_CODE.Success)
            {
                System.Diagnostics.Debug.WriteLine("Status: EMDKManager object creation failed.");
                System.Diagnostics.Debug.WriteLine("请到以下网址查看本设备支持使用 EMDK for Xamarin.Android");
                System.Diagnostics.Debug.WriteLine("https://www.zebra.com/us/en/support-downloads/software/developer-tools/emdk-for-xamarin.html");

                System.Diagnostics.Debugger.Break();

                throw new NotSuppertEMDKXamarinException();
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Status: EMDKManager object creation succeeded.");
            }
        }

        #region IEMDKListener Members

        /// <summary>
        /// 实现接口
        /// </summary>
        void EMDKManager.IEMDKListener.OnClosed()
        {
            // This callback will be issued when the EMDK closes unexpectedly.

            if (mEMDKManager != null)
            {
                if (mBarcodeManager != null)
                {
                    // Remove connection listener
                    mBarcodeManager.Connection -= barcodeManager_Connection;
                    mBarcodeManager = null;
                }

                // Release all the resources
                mEMDKManager.Release();
                mEMDKManager = null;
            }

            // textViewStatus.Text = "Status: EMDK closed unexpectedly! Please close and restart the application.";
            System.Diagnostics.Debug.WriteLine("Status: EMDK closed unexpectedly! Please close and restart the application.");

        }

        /// <summary>
        /// 实现接口
        /// </summary>
        /// <param name="emdkManagerInstance"></param>
        void EMDKManager.IEMDKListener.OnOpened(EMDKManager emdkManagerInstance)
        {
            System.Diagnostics.Debug.WriteLine("Status: EMDK open success.");

            this.mEMDKManager = emdkManagerInstance;

            try
            {
                mBarcodeManager = (BarcodeManager)mEMDKManager.GetInstance(EMDKManager.FEATURE_TYPE.Barcode);
                if (mBarcodeManager != null)
                {
                    // Add connection listener
                    mBarcodeManager.Connection += barcodeManager_Connection;
                }

                initScanner();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Status: BarcodeManager object creation failed.");
                System.Diagnostics.Debug.WriteLine("Exception:" + e.StackTrace);
            }
        }

        void barcodeManager_Connection(object sender, BarcodeManager.ScannerConnectionEventArgs e)
        {
            // 这个方法貌似为了用蓝牙连接而写的, 故不需要实现

            string msg = "barcodeManager_Connection";
            System.Diagnostics.Debug.WriteLine(msg);
        }

        #endregion

        public void initScanner()
        {
            try
            {
                var deviceList = mBarcodeManager.SupportedDevicesInfo;
                mScanner = mBarcodeManager.GetDevice(deviceList[0]); // 默认取第一个条码扫描设备

                // *** 定义Scanner事件 ***
                mScanner.Data += scanner_Data;
                mScanner.Status += scanner_Status;

                mScanner.Enable();
                mScanner.TriggerType = Scanner.TriggerTypes.Hard; // 按键触发                

                // Scanner 配置               
                //ScannerConfig config = mScanner.GetConfig();
                //config.ScanParams.AudioStreamType = ScannerConfig.AudioStreamType.Alarms;
                //config.ScanParams.DecodeAudioFeedbackMode = ScannerConfig.DecodeAudioFeedbackMode.Remote;

                #region (不在这里进行过滤) 过滤的条码码制

                //config.DecoderParams.QrCode.Enabled = true;
                //config.DecoderParams.Ean8.Enabled = true;
                //config.DecoderParams.Ean13.Enabled = true;
                //config.DecoderParams.Code39.Enabled = true;
                //config.DecoderParams.Code128.Enabled = true;

                #endregion

                //mScanner.SetConfig(config);

                // *** 核心 开启扫描头 *** 
                mScanner.Read();

            }
            catch (Exception e)
            {
                string msg = e.GetInfo();
                System.Diagnostics.Debug.WriteLine(msg);
            }
        }

        #region Scanner 注册事件

        void scanner_Data(object sender, Scanner.DataEventArgs e)
        {
            Common.BarcodeScanModel scanModel = null;

            ScanDataCollection scanDataCollection = e.P0;

            if ((scanDataCollection != null) && (scanDataCollection.Result == ScannerResults.Success))
            {
                IList<ScanDataCollection.ScanData> scanData = scanDataCollection.GetScanData();

                foreach (ScanDataCollection.ScanData data in scanData)
                {
                    try
                    {
                        scanModel = new Common.BarcodeScanModel
                        (
                            _BarcodeContent: data.Data,
                            _RawBarcodeContent: data.GetRawData(),
                            _BarcodeType: data.LabelType.Name(),
                            _ScanTime: DateTime.Now
                        );
                    }
                    catch (Exception ex)
                    {
                        scanModel = new Common.BarcodeScanModel
                        (
                            _ExceptionInfo: ex.GetInfo()
                        );
                    }

                    if (scanModel != null)
                    {
                        break;
                    }
                }
            }

            // ** 核心 ** 调用统一接口
            Common.BarcodeScanner.OnBarcodeScan(this, scanModel);
        }

        void scanner_Status(object sender, Scanner.StatusEventArgs e)
        {
            #region (弃用) Copy From Demo -- 仅供参考

            //StatusData statusData = e.P0;
            //StatusData.ScannerStates state = e.P0.State;

            //if (state == StatusData.ScannerStates.Idle) // Idle 闲置的
            //{
            //    statusString = "Status: " + statusData.FriendlyName + " is enabled and idle...";
            //    RunOnUiThread(() => textViewStatus.Text = statusString);

            //    if (isContinuousMode)
            //    {
            //        try
            //        {
            //            // An attempt to use the scanner continuously and rapidly (with a delay < 100 ms between scans) 
            //            // may cause the scanner to pause momentarily before resuming the scanning. 
            //            // Hence add some delay (>= 100ms) before submitting the next read.
            //            try
            //            {
            //                Thread.Sleep(100);
            //            }
            //            catch (Exception ex)
            //            {
            //                Console.WriteLine(ex.StackTrace);
            //            }

            //            // Submit another read to keep the continuation
            //            scanner.Read();
            //        }
            //        catch (ScannerException ex)
            //        {
            //            statusString = "Status: " + ex.Message;
            //            RunOnUiThread(() => textViewStatus.Text = statusString);
            //            Console.WriteLine(ex.StackTrace);
            //        }
            //        catch (NullReferenceException ex)
            //        {
            //            statusString = "Status: An error has occurred.";
            //            RunOnUiThread(() => textViewStatus.Text = statusString);
            //            Console.WriteLine(ex.StackTrace);
            //        }
            //    }

            //    RunOnUiThread(() => EnableUIControls(true));
            //}

            //if (state == StatusData.ScannerStates.Waiting)
            //{
            //    statusString = "Status: Scanner is waiting for trigger press...";
            //    RunOnUiThread(() =>
            //    {
            //        textViewStatus.Text = statusString;
            //        EnableUIControls(false);
            //    });
            //}

            //if (state == StatusData.ScannerStates.Scanning)
            //{
            //    statusString = "Status: Scanning...";
            //    RunOnUiThread(() =>
            //    {
            //        textViewStatus.Text = statusString;
            //        EnableUIControls(false);
            //    });
            //}

            //if (state == StatusData.ScannerStates.Disabled)
            //{
            //    statusString = "Status: " + statusData.FriendlyName + " is disabled.";
            //    RunOnUiThread(() =>
            //    {
            //        textViewStatus.Text = statusString;
            //        EnableUIControls(true);
            //    });
            //}

            //if (state == StatusData.ScannerStates.Error)
            //{
            //    statusString = "Status: An error has occurred.";
            //    RunOnUiThread(() =>
            //    {
            //        textViewStatus.Text = statusString;
            //        EnableUIControls(true);
            //    });
            //}
            //RunOnUiThread(() => EnableButtonText());

            #endregion

            StatusData statusData = e.P0;
            StatusData.ScannerStates state = e.P0.State;

            string statusString = string.Empty;

            if (state == StatusData.ScannerStates.Disabled)
            {
                statusString = "Status: " + statusData.FriendlyName + " is disabled.";
                System.Diagnostics.Debug.WriteLine(statusString);
            }

            if (state == StatusData.ScannerStates.Error)
            {
                statusString = "Status: An error has occurred.";
                System.Diagnostics.Debug.WriteLine(statusString);
            }

            // 扫描条码成功后, 状态变为闲置, 继续激活扫描
            if (state == StatusData.ScannerStates.Idle) // Idle 闲置的
            {
                statusString = "Status: " + statusData.FriendlyName + " is enabled and idle...";
                try
                {
                    System.Threading.Thread.Sleep(100);
                    mScanner.Read();
                }
                catch (Exception ex)
                {
                    string msg = ex.GetInfo();
                    System.Diagnostics.Debug.WriteLine(msg);
                }
            }
        }

        #endregion

        void IDisposable.Dispose()
        {
            DeInitScanner();
            if (mBarcodeManager != null)
            {
                // Remove connection listener
                mBarcodeManager.Connection -= barcodeManager_Connection;
                mBarcodeManager = null;
            }

            if (mEMDKManager != null)
            {
                mEMDKManager.Release();
                mEMDKManager = null;
            }
        }

        private void DeInitScanner()
        {
            if (mScanner != null)
            {
                try
                {
                    mScanner.CancelRead();
                    mScanner.Disable();
                }
                catch (ScannerException ex)
                {
                    string msg = ex.GetInfo();
                    System.Diagnostics.Debug.WriteLine(msg);
                }

                mScanner.Data -= scanner_Data;
                mScanner.Status -= scanner_Status;

                try
                {
                    mScanner.Release();
                }
                catch (ScannerException ex)
                {
                    string msg = ex.GetInfo();
                    System.Diagnostics.Debug.WriteLine(msg);
                }

                mScanner = null;
            }
        }

        #region 实现接口 IBarcodeScanner

        public void On_Pause()
        {
            //// Save the current state of continuous mode flag
            //isContinuousModeSaved = isContinuousMode;

            //// Reset continuous flag 
            //isContinuousMode = false;

            // De-initialize scanner
            DeInitScanner();

            if (mBarcodeManager != null)
            {
                // Remove connection listener
                mBarcodeManager.Connection -= barcodeManager_Connection;
                mBarcodeManager = null;
            }

            // Release the barcode manager resources
            if (mEMDKManager != null)
            {
                mEMDKManager.Release(EMDKManager.FEATURE_TYPE.Barcode);
            }
        }

        public void On_Resume()
        {
            // The application is in foreground 

            // Restore continuous mode flag
            //isContinuousMode = isContinuousModeSaved;

            // Acquire the barcode manager resources
            if (mEMDKManager != null)
            {
                try
                {
                    mBarcodeManager = (BarcodeManager)mEMDKManager.GetInstance(EMDKManager.FEATURE_TYPE.Barcode);

                    if (mBarcodeManager != null)
                    {
                        // Add connection listener
                        mBarcodeManager.Connection += barcodeManager_Connection;
                    }

                    // EnumerateScanners();
                    initScanner();
                }
                catch (Exception e)
                {
                    string msg = "Status: BarcodeManager object creation failed.";
                    System.Diagnostics.Debug.WriteLine(msg);

                    Console.WriteLine("Exception: " + e.StackTrace);
                    System.Diagnostics.Debug.WriteLine(e.GetInfo());
                }
            }
        }

        public void On_Dispose()
        {
            this.Dispose();
        }

        #endregion

        #region 实现接口 Util.XamariN.IHardwareBarcodeScanner

        public void EnabledScanner()
        {
            mScanner.Enable();
        }

        public void DisabledScanner()
        {
            mScanner.Disable();
        }

        #endregion
    }
}