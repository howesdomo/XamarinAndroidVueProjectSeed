using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Snackbar;


namespace Client.AndroiD
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        public Android.Webkit.WebView mWebView { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            initWebView();
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

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
