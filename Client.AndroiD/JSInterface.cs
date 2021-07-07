﻿using Android.App;
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
    public class JSInterface : Java.Lang.Object
    {
        Context mContext;

        Android.Webkit.WebView mWebView { get; set; }

        public JSInterface(Context context, Android.Webkit.WebView webView)
        {
            this.mContext = context;
            this.mWebView = webView;
        }

        [Android.Webkit.JavascriptInterface]
        [Java.Interop.Export]
        public void ShowToast(string msg)
        {
            Toast.MakeText(mContext, msg, ToastLength.Short).Show();
        }

        [Android.Webkit.JavascriptInterface]
        public void ShowScan()
        {
            string script = string.Format("javascript:getContactList('%s','%s')", 'A', 'B');
            mWebView.EvaluateJavascript(script, null);
        }
    }
}