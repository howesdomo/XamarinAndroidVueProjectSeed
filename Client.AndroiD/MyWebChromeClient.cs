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

namespace Client.AndroiD
{
    // 参考demo网站
    // Chromium WebView Samples
    // https://github.com/googlearchive/chromium-webview-samples
    // 
    // WebChromeClient是辅助WebView处理Javascript的对话框，网站图标，网站title，加载进度等
    public class MyWebChromeClient : Android.Webkit.WebChromeClient
    {
        public override void OnProgressChanged(Android.Webkit.WebView view, int newProgress)
        {
            base.OnProgressChanged(view, newProgress);
        }

        public override bool OnJsAlert(Android.Webkit.WebView view, string url, string message, Android.Webkit.JsResult result)
        {
            return base.OnJsAlert(view, url, message, result);
        }
    }
}