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
using System.Data.SqlClient;

namespace Apps.BLL.Sys
{
    public partial class SysRoleBLL
    {
        public SysRoleRepository _SysRoleRepository;
        public SysRoleBLL()
        {
            _SysRoleRepository = new SysRoleRepository();
        }

        public List<UserView> GetRoleListByUser(string roleId, string depId)
        {
            String sql = "select a.Id,a.UserName,a.TrueName,ISNULL(b.SysUserId, '0') as Flag from SysUser a left join SysRoleSysUser b on cast(a.Id as varchar) = b.SysUserId and b.SysRoleId = @SysRoleId Where a.DepId =  @DepId order by b.SysUserId desc";
            SqlParameter[] sqlParameters = { new SqlParameter { ParameterName = "SysRoleId", Value = roleId }, new SqlParameter { ParameterName = "DepId", Value = depId } };
            DbContexts DbContext = new DbContexts();
            return DbContext.Database.SqlQuery<UserView>(sql, sqlParameters).ToList();
        }

    }
}




