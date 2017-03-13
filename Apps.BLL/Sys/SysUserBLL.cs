using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apps.BLL.Core;
using Microsoft.Practices.Unity;
using Apps.Models.Sys;
using Apps.Common;
using Apps.Models;
using System.Transactions;
using Apps.Locale;
using Apps.DAL.Sys;
using System.Data.SqlClient;

namespace Apps.BLL.Sys
{
    public partial class SysUserBLL
    {
        public SysUserRepository m_Rep;

        public SysUserBLL()
        {
            m_Rep = new SysUserRepository();
        }

        public SysRoleRepository _SysRoleRepository = new SysRoleRepository();
        public SysRoleSysUserRepository _SysRoleSysUserRepository = new SysRoleSysUserRepository();
        public SysPositionRepository _SysPositionRepository = new SysPositionRepository();
        public SysStructRepository _SysStructRepository = new SysStructRepository();

        #region 获取用户权限
        public List<perm> GetPermission(string accountid, string controller)
        {
            List <perm>  perms=
            (
            from rt in m_Rep.DbContext.SysRight
            join sm in m_Rep.DbContext.SysModule
            on rt.ModuleId equals sm.Id.ToString()
            join r in
                (from r in m_Rep.DbContext.SysRoleSysUser
                 join u in m_Rep.DbContext.SysUser
                 on r.SysUserId equals u.Id.ToString()
                 where u.Id.ToString() == accountid
                 select r)
            on rt.RoleId equals r.SysRoleId
            where sm.Url.ToLower() == controller
            select new perm
            {
                KeyCode = rt.KeyCode,
                IsValid = rt.IsValid
            }).ToList();
            return perms;
        }
        #endregion

        #region 获取用户列表
        public List<SysUser> GetList(ref GridPager pager, string queryStr)
        {
            List<SysUser> query = null;
            IQueryable<SysUser> list = m_Rep.FindList();
            pager.totalRows = list.Count();
            if (!string.IsNullOrWhiteSpace(queryStr))
            {
                list = list.Where(a => a.UserName.Contains(queryStr) || a.TrueName.Contains(queryStr));
                pager.totalRows = list.Count();
            }
            else
            {
                pager.totalRows = list.Count();
            }
            if (pager.order == "desc")
            {
                query = list.OrderBy(c => c.Id).Skip((pager.page - 1) * pager.rows).Take(pager.rows).ToList();
            }
            else
            {
                if (pager.order == "UserName")
                {
                    query = list.OrderByDescending(c => c.UserName).Skip((pager.page - 1) * pager.rows).Take(pager.rows).ToList();
                }
                else
                {
                    query = list.OrderByDescending(c => c.Id).Skip((pager.page - 1) * pager.rows).Take(pager.rows).ToList();
                }
            }
            return query;
        }
        #endregion

        #region 获取角色
        public string GetRefSysRole(string userId)
        {
            string RoleName = "";
            var roleList = _SysRoleSysUserRepository.FindList(a => a.SysUserId == userId);
            if (roleList != null)
            {
                foreach (var role in roleList)
                {
                    RoleName += "[" + role.SysRoleName + "] ";
                }
            }
            return RoleName;
        }
        #endregion

        #region 获取用户角色
        public List<SysRole> GetRoleListByUser(string userId)
        {
            String sql = "select a.Id,a.KEY_Id,a.Name,a.Description,a.CreateTime,a.CreatePerson,a.UserName,a.Description,ISNULL(b.SysUserId, '0') as Flag from SysRole a left join SysRoleSysUser b on cast(a.Id as varchar) = b.SysRoleId and b.SysUserId = @SysUserId order by b.SysUserId desc";
            SqlParameter[] sqlParameters = { new SqlParameter { ParameterName = "SysUserId", Value = userId } };
            return  _SysRoleRepository.SqlQuery(sql, sqlParameters).ToList();
        }
        #endregion

        #region 修改用户角色
        public SysUser UpdateUserRoleByUserId(string userId, string roleIds)
        {
            string[] arr = roleIds.Split(',');
            var UpdateUser = m_Rep.Find(Convert.ToInt32(userId));
            if (UpdateUser == null)
            {
                return null;
            }
            else
            {
                UpdateUser.RoleName = string.Empty;
            }
            foreach (string roleid in arr)
            {
                if (!string.IsNullOrWhiteSpace(roleid))
                {
                    var role = _SysRoleRepository.Find(a => a.Id == Convert.ToInt32(roleid));
                    if (role != null)
                    {
                        UpdateUser.RoleName = UpdateUser.RoleName + role.Name;
                    }
                }
            }
            return UpdateUser;
        }
        #endregion

        #region 外线员登录
        /// <summary>
        /// 外线员登陆
        /// </summary>
        /// <param name="EmployeeNo"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public SysUser login(string EmployeeNo, string Password)
        {
            SysUser user = new SysUser();
            //获取实体列表
            IQueryable<SysUser> users = m_Rep.FindList().Where(otm => otm.EmployeeNo == EmployeeNo && otm.Password == Password);
            if (users.Count() > 0)
            {
                user = users.First();
            }
            return user;
        }
        #endregion
    }
}
