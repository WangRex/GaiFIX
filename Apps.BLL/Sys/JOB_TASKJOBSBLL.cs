using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;
using Apps.DAL;
using System.Text.RegularExpressions;
using Apps.BLL.Core;
using Apps.Common;
using Apps.Models;
using Apps.Locale;
using System.Transactions;
using Apps.Models.JOB;
using Apps.DAL.JOB;

namespace Apps.BLL.JOB
{
    public partial class JOB_TASKJOBSBLL
    {
        public JOB_TASKJOBSRepository _JOB_TASKJOBSRepository;
        public JOB_TASKJOBSBLL()
        {
            _JOB_TASKJOBSRepository = new JOB_TASKJOBSRepository();
        }
    }
}
