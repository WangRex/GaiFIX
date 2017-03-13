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
    public partial class SysRoleSysUserBLL
    {
        public SysRoleSysUserRepository m_Rep;
        public SysRoleRepository roleRepository;
        public SysUserRepository userRepository;
        public SysRoleSysUserBLL()
        {
            m_Rep = new SysRoleSysUserRepository();
            roleRepository = new SysRoleRepository();
            userRepository = new SysUserRepository();
        }

        public bool UpdateRolesByUserId(string userId, string roleIds)
        {
            string[] arr = roleIds.Split(',');
            var UpdateUser = userRepository.Find(Convert.ToInt32(userId));
            if (UpdateUser == null)
            {
                return false;
            }
            else
            {
                UpdateUser.RoleName = string.Empty;
                var Roles = m_Rep.FindList(a => a.SysUserId == userId);
                if (Roles != null)
                {
                    m_Rep.Delete(Roles);
                }
            }
            foreach (string roleid in arr)
            {
                if (!string.IsNullOrWhiteSpace(roleid))
                {
                    SysRoleSysUser Insert = new SysRoleSysUser();
                    Insert.SysRoleId = roleid;
                    Insert.SysUserId = userId;
                    var role = roleRepository.Find(Convert.ToInt32(roleid));
                    if (role != null)
                    {
                        UpdateUser.RoleName = UpdateUser.RoleName + "[" + role.Name + "]";
                        Insert.SysRoleName  = role.Name;
                    }
                    m_Rep.Create(Insert);
                }
            }
            userRepository.Update(UpdateUser);
            return true;
        }

        public bool UpdateUsersByRoleId(string roleId, string userIds)
        {
            if (!string.IsNullOrWhiteSpace(userIds))
            {
                string[] arr = userIds.Split(',');
                foreach (string userid in arr)
                {
                    var Roles = m_Rep.Find(a => a.SysUserId == userid && a.SysRoleId == roleId);
                    var User = userRepository.Find(Convert.ToInt32(userid));
                    if (Roles == null && User != null)
                    {
                        SysRoleSysUser Insert = new SysRoleSysUser();
                        Insert.SysRoleId = roleId;
                        Insert.SysUserId = userid;

                        var role = roleRepository.Find(Convert.ToInt32(roleId));
                        if (role != null)
                        {
                            User.RoleName = User.RoleName + "[" + role.Name + "]";
                            Insert.SysRoleName = role.Name;
                        }
                        m_Rep.Create(Insert);
                        userRepository.Update(User);
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
          
        }

    }
}




