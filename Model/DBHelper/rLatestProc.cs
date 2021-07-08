using System;
using System.Collections.Generic;
using System.Text;

namespace Model.DBHelper
{
    /// <summary>
    /// 最近修改的存储过程
    /// </summary>
    public class rLatestProc
    {
        public string Name { get; set; }

        public DateTime CreateDateTime { get; set; }

        public DateTime ModifyDateTime { get; set; }
    }
}
