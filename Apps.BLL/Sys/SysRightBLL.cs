using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apps.Models;
using Microsoft.Practices.Unity;
using Apps.Common;
using Apps.BLL.Core;
using Apps.Models.Sys;
using Apps.Locale;
using Apps.DAL.Sys;

namespace Apps.BLL.Sys
{
    public partial class SysRightBLL
    {
        public SysRightRepository _SysRightRepository;
        public SysRightBLL()
        {
            _SysRightRepository = new SysRightRepository();
        }


    }
}
