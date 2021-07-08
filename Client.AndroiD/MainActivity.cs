using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Snackbar;
using Android.Content.PM;
using System.IO;


namespace Client.AndroiD
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        public Android.Webkit.WebView mWebView { get; set; }

        public Droid.BarcodeScanner.IBarcodeScanner mBarcodeScanner { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            init();
            initWebView();
        }

        private void init()
        {
            #region 初始化 Common.StaticInfo

            var staticInfoInitArgs = new Common.StaticInfoInitArgs();

            staticInfoInitArgs.AppName = "XAVPS - Xamarin.Android Vue Project Seed";

            // 获取 AndroidManifest.xml 配置的 VersionCode VersionName
            PackageInfo packageInfo = this.PackageManager.GetPackageInfo(this.PackageName, 0);
            staticInfoInitArgs.AndroidVersionName = Application.Context.ApplicationContext.PackageManager.GetPackageInfo(Application.Context.ApplicationContext.PackageName, 0).VersionName;
            try
            {
                staticInfoInitArgs.AndroidVersionCode = packageInfo.LongVersionCode; // (int)VersionCode已过时, 现在推荐使用 (long)LongVersionCode                
            }
            catch (Exception)
            {
                // 已测试 android 7.1 没有 LongVersionCode, 这里用回被弃用的 VersionCode 属性
                staticInfoInitArgs.AndroidVersionCode = packageInfo.VersionCode;
            }

            #region 程序内部存储路径 - iOS 与 Android 通用 

            var di = new System.IO.DirectoryInfo(Xamarin.Essentials.FileSystem.AppDataDirectory);

            staticInfoInitArgs.AppPath = di.Parent.FullName;
            staticInfoInitArgs.AppFilesPath = Xamarin.Essentials.FileSystem.AppDataDirectory;
            staticInfoInitArgs.AppCachePath = Xamarin.Essentials.FileSystem.CacheDirectory;

            #endregion

            #region 安卓项目路径赋值

            // 安卓系统外部存储绝对路径
            staticInfoInitArgs.AndroidExternalRoot = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;

            // App外部存储的Files -- /{安卓系统外部存储路径}/Android/data/{appPackageName}/files
            foreach (var item in this.GetExternalFilesDirs(string.Empty))
            {
                staticInfoInitArgs.AndroidExternalFilesPath = item.AbsolutePath;
                break;
            }

            // App外部存储根目录
            if (staticInfoInitArgs.AndroidExternalFilesPath.IsNullOrWhiteSpace() == false)
            {
                staticInfoInitArgs.AndroidExternalPath = new DirectoryInfo(staticInfoInitArgs.AndroidExternalFilesPath).Parent.FullName;
            }

            // App外部存储的cache -- /{安卓系统外部存储路径}/Android/data/{appPackageName}/cache
            foreach (var item in this.GetExternalCacheDirs())
            {
                staticInfoInitArgs.AndroidExternalCachePath = item.AbsolutePath;
                break;
            }

            #endregion

            #region 服务器配置

            string pathServiceSettings = Common.ServiceSettingsUtils.GetConfigFilePath(argsDirPath: staticInfoInitArgs.AndroidExternalFilesPath);

            if (System.IO.File.Exists(pathServiceSettings) == false)
            {
                // 检查到没有网络服务配置文件, 执行初始化
                Common.ServiceSettingsUtils.InitConfig(pathServiceSettings);
            }
            // 读取网络服务配置文件
            // 1. 读取 Uri
            staticInfoInitArgs.Uri = Common.ServiceSettingsUtils.ReadUri(pathServiceSettings);
            // 2. 读取 WebServiceSettingList            
            var webServiceSettingList = Common.ServiceSettingsUtils.ReadWebServiceSettingList(pathServiceSettings);
            Common.ServiceSettingsUtils.EveryAppDefine(webServiceSettingList, staticInfoInitArgs);

            #endregion

            #region 本机配置

            string pathNativeSettings = Common.NativeSettingsUtils.GetConfigFilePath(argsDirPath: staticInfoInitArgs.AndroidExternalFilesPath);

            if (System.IO.File.Exists(pathNativeSettings) == false)
            {
                Common.NativeSettingsUtils.InitConfig(pathNativeSettings);
            }

            Common.NativeSettingsUtils.ReadConfig(pathNativeSettings, staticInfoInitArgs);

            #endregion

            #region SQLite

            //staticInfoInitArgs.InnerSQLiteConnStr = System.IO.Path.Combine
            //(
            //    System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),
            //    Util.Principle.DatabaseName_SQLite
            //);

            //staticInfoInitArgs.ExternalSQLiteConnStr = System.IO.Path.Combine
            //(
            //    staticInfoInitArgs.AndroidExternalFilesPath,
            //    Util.Principle.DatabaseName_SQLite
            //);

            #endregion

            Common.StaticInfo.Init(staticInfoInitArgs);

            #endregion

            // 实现IOutput接口 - 用 Logcat 来实现
            // App.Output = new MyOutput();

            //// 屏幕方向
            //App.Screen = Util.XamariN.AndroiD.MyScreen.GetInstance(this);

            //// 初始化百度定位
            //App.LBS = new BaiduLBS(Android.App.Application.Context);

            //// 初始化Audio
            //App.AudioPlayer = Util.XamariN.AndroiD.MyAudioPlayer.GetInstance();

            //// 初始化TTS
            //App.TTS = Util.XamariN.AndroiD.MyTTS.GetInstance();

            //// 初始化IR
            //// App.IR = MyIR.GetInstance(ApplicationContext);            

            //// 初始化Bluetooth
            //App.Bluetooth = Util.XamariN.AndroiD.MyBluetooth.GetInstance(this);

            //// Aspose.Cells
            //App.ExcelUtils_AsposeCells = new Util.Excel.ExcelUtils_Aspose_XamarinAndroid();

            //App.PowerManager = new MyPowerManager();

            #region 安卓特有

            //// 访问 Assets 资源
            //App.AndroidAssetsUtils = Util.XamariN.AndroiD.MyAndroidAssetsUtils.GetInstance(this);

            //// 初始化 Intent 工具类
            //App.AndroidIntentUtils = Util.XamariN.AndroiD.MyAndroidIntentUtils.GetInstance(this);
            //string auth = $"{this.Application.PackageName}.fileprovider";
            //App.AndroidIntentUtils.SetFileProvider_Authority(auth);

            //App.MyShareUtils = new Util.XamariN.AndroiD.MyShareUtils();

            //// 文件管理器 ShareUtils 赋值具体
            //Util.XamariN.FileExplorer.MyFileExplorer.ShareUtils = new Util.XamariN.AndroiD.MyShareUtils();

            //// 1)实现封装好的接口

            //// [弃用] 初始化动态权限, 改用 Xamarin.Essentials 里面的权限控制
            //// App.AndroidPermissionUtils = Util.XamariN.AndroiD.MyAndroidPermission.GetInstance(this);

            //// 初始化截屏
            //App.AndroidScreenshot = Util.XamariN.AndroiD.MyAndroidScreenshot.GetInstance(this);

            //// 初始化录屏
            //App.AndroidScreenRecord = Util.XamariN.AndroiD.MyAndroidScreenRecord.GetInstance(this);

            #endregion

            #region 收集 DeviceInfo ( 方便调试 初始化扫描枪代码 )

            try
            {
                // dynamic deviceInfo = new System.Dynamic.ExpandoObject();
                string Platform = Xamarin.Essentials.DeviceInfo.Platform.ToString();
                string Manufacturer = Xamarin.Essentials.DeviceInfo.Manufacturer;
                string Model = Xamarin.Essentials.DeviceInfo.Model;
                string Idiom = Xamarin.Essentials.DeviceInfo.Idiom.ToString();
                string DeviceName = Xamarin.Essentials.DeviceInfo.Name;
                string DeviceType = Xamarin.Essentials.DeviceInfo.DeviceType.ToString();

                string VersionInfo = Xamarin.Essentials.DeviceInfo.VersionString;
                int VersionMajor = Xamarin.Essentials.DeviceInfo.Version.Major;
                int VersionMinor = Xamarin.Essentials.DeviceInfo.Version.Minor;
                int VersionBuild = Xamarin.Essentials.DeviceInfo.Version.Build;
                int VersionRevision = Xamarin.Essentials.DeviceInfo.Version.Revision;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.GetInfo());
                System.Diagnostics.Debugger.Break();
            }

            #endregion

            #region 获取显示信息

            var metrics = new Android.Util.DisplayMetrics();
            this.WindowManager.DefaultDisplay.GetMetrics(metrics);
            float d = metrics.Density;
            Android.Util.DisplayMetricsDensity dpi = metrics.DensityDpi;

            #endregion

            #region 初始化第三方 DLL 库

            //// 初始化 Acr.UserDialogs
            //Acr.UserDialogs.UserDialogs.Init(this);

            //// 初始化条码扫描器
            //ZXing.Net.Mobile.Forms.Android.Platform.Init();

            //// FFImage
            //FFImageLoading.Forms.Platform.CachedImageRenderer.Init(enableFastRenderer: true);

            #endregion

            #region 初始化扫描枪

            // Zebra TC-20 = "Zebra", Zebra TC-25 = "Zebra Technologies"
            if (Xamarin.Essentials.DeviceInfo.Manufacturer.Contains("Zebra", StringComparison.CurrentCultureIgnoreCase))
            {
                try
                {
                    // 优先尝试使用 EMDK for Xamarin 的方式获取扫描内容
                    var tc_Serial = Droid.BarcodeScanner.BarcodeScanner_Zebra_TC_Series.GetInstance();
                    mBarcodeScanner = tc_Serial;
                    App.HardwareBarcodeScanner = tc_Serial; // 用于控制扫描头硬开关
                }
                catch (Droid.BarcodeScanner.NotSuppertEMDKXamarinException)
                {
                    // 使用获取 DataWedge 广播的方式读取扫描内容
                    var zebra_DataWedge_Broadcast = Droid.BarcodeScanner.BarcodeScanner_Zebra_Broadcast.GetInstance();
                    mBarcodeScanner = zebra_DataWedge_Broadcast;
                    // App.HardwareBarcodeScanner = zebraBBB; // TODO
                }
            }
            //else if
            //(
            //    Xamarin.Essentials.DeviceInfo.Manufacturer.Equals("Android", StringComparison.CurrentCultureIgnoreCase) &&
            //    Xamarin.Essentials.DeviceInfo.Name.Equals("50 Series", StringComparison.CurrentCultureIgnoreCase)
            //)
            //{
            //    // iData 9105
            //    var idata9105 = BarcodeScanner.BarcodeScanner_iData_iScan.GetInstance();
            //    mBarcodeScanner = idata9105;
            //    App.HardwareBarcodeScanner = idata9105; // 用于控制扫描头硬开关
            //}
            //else if
            //(
            //    Xamarin.Essentials.DeviceInfo.Manufacturer.Equals("IWRIST", StringComparison.CurrentCultureIgnoreCase)
            //)
            //{
            //    // 测试成功的型号 IWRIST i7
            //    var i = BarcodeScanner.BarcodeScanner_IWRIST.GetInstance();
            //    mBarcodeScanner = i;
            //    App.HardwareBarcodeScanner = i;
            //}
            else
            {
                var fakeBarcodeScanner = Droid.BarcodeScanner.FakeBarcodeScanner.GetInstance();
                mBarcodeScanner = fakeBarcodeScanner;
                App.HardwareBarcodeScanner = fakeBarcodeScanner; // 用于控制扫描头硬开关
            }

            #endregion

            //// android 设置全局字体
            //setDefaultFont("Fonts/FZFSJ.ttf");
        }

        void initWebView()
        {
            mWebView = FindViewById<Android.Webkit.WebView>(Resource.Id.webView0);

            var webSettings = mWebView.Settings;
            webSettings.JavaScriptEnabled = true;
            webSettings.DomStorageEnabled = true; // 遇到的坑 关键 不加上不能进入 第二页
            webSettings.AllowFileAccess = true;

            webSettings.SetAppCacheEnabled(true);
            webSettings.SetAppCachePath(this.Application.BaseContext.ExternalCacheDir.AbsolutePath);


            // webSettings.CacheMode = Android.Webkit.CacheModes.CacheElseNetwork;
            webSettings.CacheMode = Android.Webkit.CacheModes.NoCache;

            //webSettings.setAllowFileAccess(true); //设置可以访问文件

            webSettings.JavaScriptCanOpenWindowsAutomatically = true; // 支持通过JS打开新窗口
            webSettings.LoadsImagesAutomatically = true; // 支持自动加载图片
            webSettings.DefaultTextEncodingName = "utf-8";

            mWebView.SetWebViewClient(new MyWebViewClient(this.Assets));
            mWebView.SetWebChromeClient(new MyWebChromeClient());

            string url = "http://192.168.90.104:8077/";
            //String url = "file:///android_asset/vue/index.html";
            mWebView.LoadUrl(url);

            mWebView.AddJavascriptInterface(new JSInterface(this, mWebView), name: "Android");
        }

        public override void OnBackPressed()
        {
            if (mWebView.CanGoBack())
            {
                mWebView.GoBack();
            }
            else
            {
                // base.OnBackPressed();
            }
        }



        protected override void OnPause()
        {
            mBarcodeScanner.On_Pause();
            base.OnPause();
        }

        protected override void OnResume()
        {
            mBarcodeScanner.On_Resume();
            base.OnResume();
        }

        protected override void OnDestroy()
        {
            mBarcodeScanner.On_Dispose();
            base.OnDestroy();
        }



        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
