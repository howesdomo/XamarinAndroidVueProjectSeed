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
    /// V 1.0.0 - 2021-07-09 10:55:38
    /// 首次创建, 为解决某些型号在某些安卓版本上无法使用 EMDK 的方式读取条码信息
    /// 请到以下网址查看本设备支持使用 EMDK for Xamarin.Android
    /// https://www.zebra.com/us/en/support-downloads/software/developer-tools/emdk-for-xamarin.html 
    /// </summary>
    public class BarcodeScanner_Zebra_DataWedge : IBarcodeScanner, IHardwareBarcodeScanner
    {
        #region Props

        Android.Content.IntentFilter mFilter { get; set; }

        BroadcastReceiver_Zebra mReceiver { get; set; }

        // IWRIST_Scanner mScanner { get; set; }

        /// <summary>
        /// 已注册广播接收器
        /// </summary>
        bool mIsRegistered { get; set; }

        #endregion

        #region 单例模式

        private static BarcodeScanner_Zebra_DataWedge _Instance_ { get; set; }

        private static readonly object _LOCK_ = new object();

        public static BarcodeScanner_Zebra_DataWedge GetInstance()
        {
            if (_Instance_ == null)
            {
                lock (_LOCK_)
                {
                    if (_Instance_ == null)
                    {
                        _Instance_ = new BarcodeScanner_Zebra_DataWedge();
                    }
                }
            }

            return _Instance_;
        }

        #endregion

        const string FilterAction = "cn.com.howe";

        private BarcodeScanner_Zebra_DataWedge()
        {
            mFilter = new IntentFilter(FilterAction);
            mReceiver = new BroadcastReceiver_Zebra();
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
                EnableDataWedge();

                
                int localProfileVersion = Xamarin.Essentials.Preferences.Get(Profile_Key, defaultValue: 0);
                
                if (localProfileVersion == 0)
                {
                    CreateProfile();
                }

                if (localProfileVersion < LatestProfileVersion)
                {                    
                    UpdateProfile();
                    Xamarin.Essentials.Preferences.Set(Profile_Key, LatestProfileVersion);
                }                

                SwitchToProfile();

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
            EnableDataWedge();
        }

        public void DisabledScanner()
        {
            DisableDataWedge();
        }

        #endregion

        #region 本程序向 DataWedge 新增配置文件, 然后启用配置文件

        public const string Profile_Key = "DataWedge_ProfileVersion";        
        static string PROFILE_NAME = "XPFS"; // TODO 如何动态地获取 MainActivity 的 Label

        public static string ACTION_DATAWEDGE_FROM_6_2 = "com.symbol.datawedge.api.ACTION";
        public static string EXTRA_CREATE_PROFILE = "com.symbol.datawedge.api.CREATE_PROFILE";
        public static string EXTRA_ENABLE_DATAWEDGE = "com.symbol.datawedge.api.ENABLE_DATAWEDGE";
        public static string EXTRA_SWITCH_TO_PROFILE = "com.symbol.datawedge.api.SWITCH_TO_PROFILE";
        public static string EXTRA_SET_CONFIG = "com.symbol.datawedge.api.SET_CONFIG";
        public static string DATAWEDGE_EXTRA_KEY_SCANNER_TRIGGER_CONTROL = "com.symbol.datawedge.api.SOFT_SCAN_TRIGGER";
        public static string DATAWEDGE_EXTRA_VALUE_TOGGLE_SCANNER = "TOGGLE_SCANNING";

        /// <summary>
        /// DataWedge 设置为启用状态
        /// </summary>
        void EnableDataWedge()
        {
            Intent i = new Intent();
            i.SetAction(ACTION_DATAWEDGE_FROM_6_2);
            i.PutExtra(EXTRA_ENABLE_DATAWEDGE, true);
            Android.App.Application.Context.SendBroadcast(i);
        }

        /// <summary>
        /// DataWedge 设置为关闭状态
        /// </summary>
        void DisableDataWedge()
        {
            Intent i = new Intent();
            i.SetAction(ACTION_DATAWEDGE_FROM_6_2);
            i.PutExtra(EXTRA_ENABLE_DATAWEDGE, false);
            Android.App.Application.Context.SendBroadcast(i);
        }

        /// <summary>
        /// 在 DataWedge 中新建配置文件
        /// </summary>
        void CreateProfile()
        {
            Intent i = new Intent();
            i.SetAction(ACTION_DATAWEDGE_FROM_6_2);
            i.PutExtra(EXTRA_CREATE_PROFILE, PROFILE_NAME);
            Android.App.Application.Context.SendBroadcast(i);
        }
                
        public const int LatestProfileVersion = 1; // 当配置代码有更新, Version + 1

        /// <summary>
        /// 更新配置, 更新代码后请 LatestProfileVersion + 1
        /// </summary>
        void UpdateProfile()
        {
            Bundle profileConfig = new Bundle();
            profileConfig.PutString("PROFILE_NAME", PROFILE_NAME); // Set the profile name
            profileConfig.PutString("PROFILE_ENABLED", "true"); // Enable the profile
            profileConfig.PutString("CONFIG_MODE", "UPDATE");
            Bundle barcodeConfig = new Bundle();
            barcodeConfig.PutString("PLUGIN_NAME", "BARCODE");
            barcodeConfig.PutString("RESET_CONFIG", "true"); // This is the default but never hurts to specify

            // Here you define scanner properties
            Bundle barcodeProps = new Bundle();

            barcodeProps.PutString("scanner_input_enabled", "true");
            barcodeProps.PutString("scanner_selection", "auto"); // Could also specify a number here, the id returned from ENUMERATE_SCANNERS.
                                                                 // Do NOT use "Auto" here (with a capital 'A'), it must be lower case.
            barcodeProps.PutString("decoder_ean8", "true");
            barcodeProps.PutString("decoder_ean13", "true");
            barcodeProps.PutString("decoder_code39", "true");
            barcodeProps.PutString("decoder_code128", "true");
            barcodeProps.PutString("decoder_upca", "true");
            barcodeProps.PutString("decoder_upce0", "true");
            barcodeProps.PutString("decoder_upce1", "true");
            barcodeProps.PutString("decoder_d2of5", "true");
            barcodeProps.PutString("decoder_i2of5", "true");
            barcodeProps.PutString("decoder_aztec", "true");
            barcodeProps.PutString("decoder_pdf417", "true");
            barcodeProps.PutString("decoder_qrcode", "true");

            
            barcodeProps.PutString("decode_audio_feedback_uri", ""); // 去掉扫描成功音效

            barcodeConfig.PutBundle("PARAM_LIST", barcodeProps);
            profileConfig.PutBundle("PLUGIN_CONFIG", barcodeConfig);

            Bundle appConfig = new Bundle();
            appConfig.PutString("PACKAGE_NAME", Android.App.Application.Context.PackageName); // Associate the profile with this app
            appConfig.PutStringArray("ACTIVITY_LIST", new String[] { "*" });
            profileConfig.PutParcelableArray("APP_LIST", new Bundle[] { appConfig });
            SendDataWedgeIntentWithExtra(ACTION_DATAWEDGE_FROM_6_2, EXTRA_SET_CONFIG, profileConfig);
            // You can only configure one plugin at a time, we have done the barcode input, now do the intent output
            profileConfig.Remove("PLUGIN_CONFIG");
            Bundle intentConfig = new Bundle();
            intentConfig.PutString("PLUGIN_NAME", "INTENT");
            intentConfig.PutString("RESET_CONFIG", "true");
            Bundle intentProps = new Bundle();
            intentProps.PutString("intent_output_enabled", "true");
            // intentProps.PutString("intent_action", DataWedgeReceiver.IntentAction); // We can use this when we're going to define the DataWedgeReceiver class
            intentProps.PutString("intent_action", FilterAction);
            intentProps.PutString("intent_delivery", "2");            
            intentConfig.PutBundle("PARAM_LIST", intentProps);
            profileConfig.PutBundle("PLUGIN_CONFIG", intentConfig);
            SendDataWedgeIntentWithExtra(ACTION_DATAWEDGE_FROM_6_2, EXTRA_SET_CONFIG, profileConfig);
        }

        void SendDataWedgeIntentWithExtra(String action, String extraKey, Bundle extraValue)
        {
            Intent dwIntent = new Intent();
            dwIntent.SetAction(action);
            dwIntent.PutExtra(extraKey, extraValue);
            //  Could also specify 'COMPLETE_RESULT' if you want the result from every plugin you specify in the same call
            dwIntent.PutExtra("SEND_RESULT", "TRUE");
            //dwIntent.putExtra("COMMAND_IDENTIFIER", "123");
            dwIntent.PutExtra("SEND_RESULT", "LAST_RESULT");  //  7.1+ only
            Android.App.Application.Context.SendBroadcast(dwIntent);
        }

        void SwitchToProfile()
        {
            Intent i = new Intent();
            i.SetAction(ACTION_DATAWEDGE_FROM_6_2);
            i.PutExtra(EXTRA_SWITCH_TO_PROFILE, PROFILE_NAME);
            Android.App.Application.Context.SendBroadcast(i);
        }

        #endregion
    }

    public class BroadcastReceiver_Zebra : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            // TODO 获取 byte[] rawData 
            var rawData = intent.GetSerializableExtra("com.symbol.datawedge.decode_data");

            var scanModel = new Common.BarcodeScanModel
            (
                _BarcodeContent: intent.GetStringExtra("com.symbol.datawedge.data_string"),
                _RawBarcodeContent: null,
                _BarcodeType: intent.GetStringExtra("com.symbol.datawedge.label_type"),
                _ScanTime: DateTime.Now
            );

            // 发出扫描成功音效
            App.AudioPlayer.PlayBeep();

            // ** 核心 ** 调用统一接口
            Common.BarcodeScanner.OnBarcodeScan(this, scanModel);
        }
    }
}