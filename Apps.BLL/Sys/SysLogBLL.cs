using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Apps.BLL.Core;
using Apps.Common;
using Apps.Models.Sys;
using Apps.Models;
using System.Transactions;
using Apps.Locale;
using Apps.DAL.Sys;

namespace Apps.BLL.Sys
{
    public partial class SysLogBLL
    {
        public SysLogRepository _SysLogRepository;
        public SysLogBLL()
        {
            _SysLogRepository = new SysLogRepository();
        }

    }
}
