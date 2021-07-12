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

namespace Client.Droid
{
    /// <summary>
    /// V 1.0.2 2021-04-15 20:08:08
    /// 1) 优化全局捕获异常流程
    /// step1 捕获到未处理到异常时，记录异常日志到 fatal.txt； 
    /// step2 重启程序 ( 无法处理的异常就闪退；不闪退的执行 PowerManager.Shutdown方法 )
    /// step3 在 AppStart中检查 fatal.txt 是否存在, 若存在读取fatal文件内容后，删除fatal.txt文件，
    ///       将异常日志内容拷贝到日志文件中。最后显示Confirm对话框，询问用户是否上传信息到服务器。
    /// 2）重新测试各情况下抛出异常在 Android 10 的情况
    ///
    /// V 1.0.1 2019-9-18 14:35:23
    /// 1) 修复bug 记录日志路径
    /// 2) 优化 记录日志到 外部存储空间
    /// 
    /// 处理这些异常的时候，应用程序已经崩溃且无法恢复，这时android系统已经杀死了应用程序，
    /// 唯一能做的就是：记录异常、场景等重要信息，在合适的时候发送到服务器，以供错误分析
    /// </summary>
    [Application(Label = "@string/app_name")] // Label 的值影响 apk安装包在安装时显示的名称 若这里不填写任何值 则显示 Client.Android
    public class MyApplication : Android.App.Application
    {
        /// <summary>
        /// 当捕获到异常时，在调试模式中是否进入断点
        /// </summary>
        bool IsEnabledDebug
        {
            get { return true; }
        }

        // TODO 为了与 iOS 保持相同的异常表现，设置让程序闪退
        /// <summary>
        /// 当捕获到异常时，将程序关闭
        /// </summary>
        bool IsEnabledShutdown
        {
            get { return true; }
        }

        public MyApplication(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        // 捕获异常流向图
        // AndroidEnvironment.UnhandledExceptionRaiser ===》 AppDomain.CurrentDomain.UnhandledException
        // System.Threading.Tasks.TaskScheduler.UnobservedTaskException ===> AppDomain.CurrentDomain.UnhandledException

        public override void OnCreate()
        {
            base.OnCreate();

            // 注册未处理异常事件
            AndroidEnvironment.UnhandledExceptionRaiser += AndroidEnvironment_UnhandledExceptionRaiser;
            System.Threading.Tasks.TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            // 不再使用以下的异常处理，上面的处理已涵盖主线程和Task发出的异常
            // AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException; // 实际最终都会流向此处
        }

        protected override void Dispose(bool disposing)
        {
            AndroidEnvironment.UnhandledExceptionRaiser -= AndroidEnvironment_UnhandledExceptionRaiser;
            System.Threading.Tasks.TaskScheduler.UnobservedTaskException -= TaskScheduler_UnobservedTaskException;

            base.Dispose(disposing);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject != null)
            {
                if (IsEnabledDebug)
                {
                    System.Diagnostics.Debugger.Break();
                }

                Exception ex = e.ExceptionObject as Exception;
                HandleException(ex, "CurrentDomain_UnhandledException");
            }
        }

        void AndroidEnvironment_UnhandledExceptionRaiser(object sender, RaiseThrowableEventArgs e)
        {
            if (IsEnabledDebug)
            {
                System.Diagnostics.Debugger.Break();
            }

            HandleException(e.Exception, "AndroidEnvironment_UnhandledExceptionRaiser");

            // Toast 提示需要分开情况处理，在主线程中捕获的异常直接 Toast 即可
            Toast.MakeText(this, "程序捕获到异常。", ToastLength.Long).Show();

            if (IsEnabledShutdown)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true; // 非 debug 状况下，android 启用 e.Handled = true 可以阻止闪退
            }
        }

        void TaskScheduler_UnobservedTaskException(object sender, System.Threading.Tasks.UnobservedTaskExceptionEventArgs e)
        {
            string msg = e.Exception.GetInfo();


            if (msg.IsNullOrWhiteSpace() == false
                && msg.StartsWith(@"A Task's exception(s) were not observed either by Waiting on the Task or accessing its Exception property. As a result, the unobserved exception was rethrown by the finalizer thread.") == true)
            {
                System.Diagnostics.Debug.WriteLine(msg);
                System.Diagnostics.Debugger.Break();

                // 以往的Xamarin.Forms 4.3 版本经常会无端端出现这个异常
                // 暂时未能找出为什么经常会捕获到此异常的原因, 暂时忽略此错误
                return;
            }

            if (IsEnabledDebug)
            {
                System.Diagnostics.Debugger.Break();
            }

            HandleException(e.Exception, "TaskScheduler_UnobservedTaskException");

            if (e.Observed == false)
            {
                e.SetObserved(); // 设置为已观察到异常
            }

            // Toast 提示需要分开情况处理, 需要在主线程中调用 Toast
            Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(() =>
            {
                Toast.MakeText(this, "程序捕获到异常。", ToastLength.Long).Show();
                if (IsEnabledShutdown)
                {
                    App.PowerManager.DeviceShutdown();
                }
            });
        }

        private void HandleException(Exception ex, string exFrom)
        {
            Android.Util.Log.Error(exFrom, ex.GetInfo());
            Client.Common.StaticInfo.SaveFatalLog(ex, exFrom);
        }
    }
}