using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.AndroiD
{
    public class MyWebViewClient : Android.Webkit.WebViewClient
    {
        //public bool shouldOverrideUrlLoading(WebView view, String url)
        //{
        //    view.loadUrl(url);
        //    return true;
        //}


        //public override bool ShouldOverrideUrlLoading(WebView view, string url)
        //{
        //    view.LoadUrl(url);
        //    return true;
        //}

        public override bool ShouldOverrideUrlLoading(WebView view, IWebResourceRequest request)
        {
            view.LoadUrl(request.Url.ToString());
            return true;
        }
    }
}