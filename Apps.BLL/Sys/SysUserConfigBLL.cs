﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由T4模板自动生成
//	   生成时间 2012-12-07 20:55:35 by App
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;
using Apps.Models;
using Apps.Common;
using Apps.BLL.Core;
using System.Transactions;
using Apps.Models.Sys;
using Apps.Locale;
using Apps.DAL.Sys;

namespace Apps.BLL.Sys
{
    public partial class SysUserConfigBLL
    {
        public SysUserConfigRepository _SysUserConfigRepository;
        public SysUserConfigBLL()
        {
            _SysUserConfigRepository = new SysUserConfigRepository();
        }

        public SysUserConfig GetByUserType(string type, string userid)
        {
            return _SysUserConfigRepository.Find(a => a.Type == type && a.UserId == userid);
        }
    }
}