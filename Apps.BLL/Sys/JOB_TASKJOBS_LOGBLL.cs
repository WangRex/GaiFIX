using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;
using Apps.DAL;
using Apps.BLL.Core;
using Apps.Common;
using Apps.Models;
using Apps.Locale;
using Apps.Models.JOB;
using Apps.DAL.JOB;

namespace Apps.BLL.JOB
{
    public partial class JOB_TASKJOBS_LOGBLL
    {
        public JOB_TASKJOBS_LOGRepository jOB_TASKJOBS_LOGRepository;
        public JOB_TASKJOBS_LOGBLL()
        {
            jOB_TASKJOBS_LOGRepository = new JOB_TASKJOBS_LOGRepository();
        }
    }
}
