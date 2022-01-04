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

namespace Client.Droid
{
    public class App
    {
        public static Util.XamariN.IScreen Screen { get; set; }

        public static Util.XamariN.IAudioPlayer AudioPlayer { get; set; }

        //public static Util.XamariN.ITTS TTS { get; set; }

        //public static Util.XamariN.IBluetooth Bluetooth { get; set; }

        //public static Util.XamariN.IShareUtils MyShareUtils { get; set; }


        //public static Client.Common.ILBS LBS { get; set; }

        //public static Client.Common.I_IR IR { get; set; }

        //public static Client.Common.IOutput Output { get; set; }

        //public static Util.Excel.IExcelUtils ExcelUtils_AsposeCells { get; set; }

        public static Util.XamariN.IHardwareBarcodeScanner HardwareBarcodeScanner { get; set; }

        public static Client.Common.IPowerManager PowerManager { get; set; }

        public static ZXing.Mobile.MobileBarcodeScanner Scanner {get;set;}
    }
}