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
        private Android.Content.Res.AssetManager mAssetManager { get; set; }

        public MyWebViewClient(Android.Content.Res.AssetManager assetManager)
        {
            mAssetManager = assetManager;
        }

        public static string GetMimeType(String url)
        {
            string type = null;
            string extension = url.Split('.').Last();
            if (extension != null)
            {
                if (extension == "css")
                {
                    return "text/css";
                }
                else if (extension == "js")
                {
                    return "text/javascript";
                }
                else if (extension == "png")
                {
                    return "image/png";
                }
                else if (extension == "gif")
                {
                    return "image/gif";
                }
                else if (extension == "jpg")
                {
                    return "image/jpeg";
                }
                else if (extension == "jpeg")
                {
                    return "image/jpeg";
                }
                else if (extension == "woff")
                {
                    return "application/font-woff";
                }
                else if (extension == "woff2")
                {
                    return "application/font-woff2";
                }
                else if (extension == "ttf")
                {
                    return "application/x-font-ttf";
                }
                else if (extension == "eot")
                {
                    return "application/vnd.ms-fontobject";
                }
                else if (extension == "svg")
                {
                    return "image/svg+xml";
                }

                type = "text/html";
            }

            return type;
        }

        public override bool ShouldOverrideUrlLoading(WebView view, IWebResourceRequest request)
        {
            view.LoadUrl(request.Url.ToString());
            return true;
        }

        public override WebResourceResponse ShouldInterceptRequest(WebView view, IWebResourceRequest request)
        {

            if (request.Url != null && request.Url.Path != null && request.Url.Host == "localhost")
            {
                var mimeType = GetMimeType(request.Url.Path);
                var fileUrl = request.Url.Path
                    .Replace("file://", "")
                    .Replace("android_asset/", "")
                    .Trim('/');

                string extension = request.Url.Path.Split('.').Last(); ;
                string fileEncoding = "application/octet-stream";
                if (extension.EndsWith("html") || extension.EndsWith("js") || extension.EndsWith("css"))
                    fileEncoding = "UTF-8";

                try
                {
                    return new WebResourceResponse(mimeType, fileEncoding, mAssetManager.Open(fileUrl));
                }
                catch
                {
                    // ignore
                }
            }

            return base.ShouldInterceptRequest(view, request);
        }
    }

    
}