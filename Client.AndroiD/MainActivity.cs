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

            initUI();
        }

        //public void LaunchBrowserView(string authorizationServerUrl, WebView webView)
        //{
        //    try
        //    {
        //        webView.Settings.JavaScriptEnabled = true;
        //        webView.Settings.DomStorageEnabled = true;
        //        //web_view.Settings.= true;
        //        //web_view.Settings.AllowContentAccess = true;
        //        webView.SetWebViewClient(new MyBrowser());
        //        webView.Settings.LoadsImagesAutomatically = true;
        //        webView.LoadUrl(authorizationServerUrl);
        //    }
        //    catch (System.Exception ex)
        //    {
        //    }
        //}

        void initUI()
        {

            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;


            // Add By Howe
            mWebView = FindViewById<Android.Webkit.WebView>(Resource.Id.webView0);
            initWebView();
        }

        void initWebView()
        {
            var webSettings = mWebView.Settings;
            webSettings.JavaScriptEnabled = true;
            // webSettings.setJavaScriptEnabled(true);


            //其他细节操作            
            // webSettings.CacheMode = Android.Webkit.CacheModes.CacheElseNetwork;
            webSettings.CacheMode = Android.Webkit.CacheModes.NoCache;

            //webSettings.setAllowFileAccess(true); //设置可以访问文件
            webSettings.AllowFileAccess = true;
            webSettings.JavaScriptCanOpenWindowsAutomatically = true; // 支持通过JS打开新窗口
            webSettings.LoadsImagesAutomatically = true; // 支持自动加载图片
            webSettings.DefaultTextEncodingName = "utf-8";

            mWebView.SetWebViewClient(new Android.Webkit.WebViewClient());

            string url = "http://192.168.90.104:8077/";
            //String url = "file:///android_asset/vue/index.html";
            mWebView.LoadUrl(url);
            // webView.addJavascriptInterface(new WebAppInterface(this, webView), "android");
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            //View view = (View) sender;
            //Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
            //    .SetAction("Action", (View.IOnClickListener)null).Show();

            // mWebView.LoadUrl("https://www.bing.com");
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
