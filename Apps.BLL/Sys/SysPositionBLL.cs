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
    public partial class SysPositionBLL
    {
        public SysPositionRepository m_Rep;
        public SysPositionBLL()
        {
            m_Rep = new SysPositionRepository();
        }

    }
}
