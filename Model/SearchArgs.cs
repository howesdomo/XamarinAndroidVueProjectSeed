using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class SearchArgs
    {
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string ScanAccount { get; set; }

        public string HotfixTest = "Model.dll = 1.0.0.0";

        public bool SearchProductScan_IsEnabled()
        {
            return 
                StartDate.HasValue ||
                EndDate.HasValue ||
                ScanAccount.IsNullOrWhiteSpace() == false;
        }
    }
}
