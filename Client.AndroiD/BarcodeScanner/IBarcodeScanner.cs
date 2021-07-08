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

namespace Client.Droid.BarcodeScanner
{
    /// <summary>
    /// V 1.0.2 - 2021-04-30 19:46:51
    /// 可以使用 Android.App.Application.Context 获取上下文, 所以不需使用 Activity 作为参数
    /// 
    /// V 1.0.1 - 2020-3-20 12:00:48
    /// iData_9105 需要使用 activity
    /// 
    /// V 1.0.0 - 2018-8-6 16:58:03
    /// </summary>
    public interface IBarcodeScanner
    {
        void On_Pause();

        void On_Resume();

        void On_Dispose();
    }
}