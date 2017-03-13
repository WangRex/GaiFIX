using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;
using Apps.Models;
using Apps.Common;
using System.Transactions;
using Apps.Models.Sys;
using Apps.BLL.Core;
using Apps.Locale;
using Apps.DAL.Sys;

namespace Apps.BLL.Sys
{
    public partial class SysModuleOperateBLL
    {
        public SysModuleOperateRepository m_Rep;
        public SysModuleOperateBLL()
        {
            m_Rep = new SysModuleOperateRepository();
        }
    }
}
