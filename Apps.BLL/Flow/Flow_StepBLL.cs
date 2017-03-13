using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;
using Apps.Models;
using Apps.Common;
using System.Transactions;
using Apps.Models.Flow;
using Apps.DAL.Flow;

using Apps.BLL.Core;
using Apps.Locale;

namespace Apps.BLL.Flow
{
    public partial class Flow_StepBLL
    {
        public Flow_StepRepository m_Rep;
        public Flow_StepBLL()
        {
            m_Rep = new Flow_StepRepository();
        }
      
    }
}
