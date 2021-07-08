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

namespace Client.Droid.BarcodeScanner
{
    public class BarcodeScanner_Zebra_Broadcast : IBarcodeScanner, Util.XamariN.IHardwareBarcodeScanner
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

        private static BarcodeScanner_Zebra_Broadcast _Instance_ { get; set; }

        private static readonly object _LOCK_ = new object();

        public static BarcodeScanner_Zebra_Broadcast GetInstance()
        {
            if (_Instance_ == null)
            {
                lock (_LOCK_)
                {
                    if (_Instance_ == null)
                    {
                        _Instance_ = new BarcodeScanner_Zebra_Broadcast();
                    }
                }
            }

            return _Instance_;
        }

        #endregion

        private BarcodeScanner_Zebra_Broadcast()
        {
            //mScanner = new IWRIST_Scanner();
            //// 扫描功能
            //mScanner.Open();
            //mScanner.SetOutputMode(2); // 使用广播模式
            //mScanner.EnablePlayBeep(false); // 关闭扫描声音
            //mScanner.DefaultSetting();

            // mFilter = new IntentFilter("com.android.server.scannerservice.broadcast");
            mFilter = new IntentFilter("barcodescanner.RECVR");
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
                
                bool isUpdated = Xamarin.Essentials.Preferences.Get("datawedge_updated", false);
                if (!isUpdated)
                {
                    CreateProfile();
                    UpdateProfile();

                    // Xamarin.Essentials.Preferences.Set("datawedge_updated", true);
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
            // mScanner.Open();
        }

        public void DisabledScanner()
        {
            // mScanner.Close();
        }

        #endregion


        // 配置wedge

        #region MyRegion

        static string PROFILE_NAME = "XamAndroidDataWedge";

        public static string ACTION_DATAWEDGE_FROM_6_2 = "com.symbol.datawedge.api.ACTION";
        public static string EXTRA_CREATE_PROFILE = "com.symbol.datawedge.api.CREATE_PROFILE";
        public static string EXTRA_ENABLE_DATAWEDGE = "com.symbol.datawedge.api.ENABLE_DATAWEDGE";
        public static string EXTRA_SWITCH_TO_PROFILE = "com.symbol.datawedge.api.SWITCH_TO_PROFILE";
        public static string EXTRA_SET_CONFIG = "com.symbol.datawedge.api.SET_CONFIG";
        public static string DATAWEDGE_EXTRA_KEY_SCANNER_TRIGGER_CONTROL = "com.symbol.datawedge.api.SOFT_SCAN_TRIGGER";
        public static string DATAWEDGE_EXTRA_VALUE_TOGGLE_SCANNER = "TOGGLE_SCANNING";

        private void EnableDataWedge()
        {
            Intent i = new Intent();
            i.SetAction(ACTION_DATAWEDGE_FROM_6_2);
            i.PutExtra(EXTRA_ENABLE_DATAWEDGE, true);
            Android.App.Application.Context.SendBroadcast(i);
        }



        private void CreateProfile()
        {
            Intent i = new Intent();
            i.SetAction(ACTION_DATAWEDGE_FROM_6_2);
            i.PutExtra(EXTRA_CREATE_PROFILE, PROFILE_NAME);
            Android.App.Application.Context.SendBroadcast(i);
        }

        private void UpdateProfile()
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
            intentProps.PutString("intent_action", "barcodescanner.RECVR");
            intentProps.PutString("intent_delivery", "2");
            intentConfig.PutBundle("PARAM_LIST", intentProps);
            profileConfig.PutBundle("PLUGIN_CONFIG", intentConfig);
            SendDataWedgeIntentWithExtra(ACTION_DATAWEDGE_FROM_6_2, EXTRA_SET_CONFIG, profileConfig);
        }


        private void SwitchToProfile()
        {
            Intent i = new Intent();
            i.SetAction(ACTION_DATAWEDGE_FROM_6_2);
            i.PutExtra(EXTRA_SWITCH_TO_PROFILE, PROFILE_NAME);
            Android.App.Application.Context.SendBroadcast(i);
        }

        private static void SendDataWedgeIntentWithExtra(String action, String extraKey, Bundle extraValue)
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


        #endregion
    }

    public class BroadcastReceiver_Zebra : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            try
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

                // string bs = intent.GetStringExtra("com.symbol.datawedge.decode_data"); // 获取到 null 值

                // String scanResult = intent.getStringExtra("scannerdata").trim();
                //if (scanResult.equals("") || scanResult == null) return;
                //String js = String.format("javascript:callJsFunction('%s')", scanResult);
                //webView.loadUrl(js);
            }
            catch (Exception e)
            {
                // showErr(e.toString());
            }
        }
    }
}