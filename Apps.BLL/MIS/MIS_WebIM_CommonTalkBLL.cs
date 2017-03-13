using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;
using Apps.Models;
using Apps.Common;
using System.Transactions;
using Apps.Models.MIS;
using Apps.BLL.Core;
using Apps.Locale;
using Apps.DAL.MIS;

namespace Apps.BLL.MIS
{
    public partial class MIS_WebIM_CommonTalkBLL
    {
        public MIS_WebIM_CommonTalkRepository m_Rep;
        public MIS_WebIM_CommonTalkBLL()
        {
            m_Rep = new MIS_WebIM_CommonTalkRepository();
        }

    }
}
