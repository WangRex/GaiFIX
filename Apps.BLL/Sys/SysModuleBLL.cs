using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Apps.Models;
using Apps.BLL.Core;
using Apps.Common;
using System.Transactions;
using Apps.Models.Sys;
using Apps.Locale;
using Apps.DAL;
using Apps.DAL.Sys;

namespace Apps.BLL.Sys
{
    public partial class SysModuleBLL
    {
        public SysModuleRepository m_Rep;
        public SysModuleBLL()
        {
            m_Rep = new SysModuleRepository();
        }

        public List<SysModule> GetMenuByPersonId(int personId, string moduleId)
        {

            // var menus = m_Rep.FindList(a => a.ParentId == moduleId);
            if (personId == 1)
            {
                return m_Rep.FindList(a => a.ParentId == moduleId).OrderBy(a => a.Sort).ToList();
            }
            if (moduleId == "0")
            {
                List<SysModule> refMenus = new List<SysModule>();
                List<SysModule> menusTop = m_Rep.FindList(a => a.ParentId == moduleId).ToList();
                foreach (SysModule subMenu in menusTop)
                {
                    List<SysModule> menus =
                      (
                          from m in m_Rep.DbContext.SysModule
                          join rl in m_Rep.DbContext.SysRight
                          on m.Id.ToString() equals rl.ModuleId
                          join r in
                              (from r in m_Rep.DbContext.SysRoleSysUser
                               join u in m_Rep.DbContext.SysUser
                               on r.SysUserId equals u.Id.ToString()
                               where u.Id == personId
                               select r)
                          on rl.RoleId equals r.SysRoleId
                          where m.ParentId == subMenu.Id.ToString()
                          select m
                                ).Distinct().OrderBy(a => a.Sort).ToList();
                    if (menus.Count > 0)
                    {
                        refMenus.Add(subMenu);
                    }
                }

                return refMenus;
            }
            else
            {
                var menus =
                  (
                      from m in m_Rep.DbContext.SysModule
                      join rl in m_Rep.DbContext.SysRight
                      on m.Id.ToString() equals rl.ModuleId
                      join r in
                          (from r in m_Rep.DbContext.SysRoleSysUser
                           join u in m_Rep.DbContext.SysUser
                           on r.SysUserId equals u.Id.ToString()
                           where u.Id == personId
                           select r)
                      on rl.RoleId equals r.SysRoleId
                      where m.ParentId == moduleId
                      select m
                            ).Distinct().OrderBy(a => a.Sort);
                return menus.ToList();
            }
           
            
        }

        //public IQueryable<SysModule> GetPageList(ref queryData,string moduleId)
        //{

        //    var menus = m_Rep.FindList(a => a.ParentId == moduleId);
        //    //(
        //    //    from m in m_Rep.DbContext.SysModule
        //    //    join rl in m_Rep.DbContext.SysRight
        //    //    on m.Id.ToString() equals rl.ModuleId
        //    //    join r in
        //    //        (from r in m_Rep.DbContext.SysRole
        //    //         from u in r.SysUser
        //    //         where u.Id  == personId
        //    //         select r)
        //    //    on rl.RoleId equals r.Id.ToString()
        //    //    where rl.Rightflag == 
        //    //    where m.ParentId == moduleId
        //    //    where m.Id != 0
        //    //    select m
        //    //          ).Distinct().OrderBy(a => a.Sort);
        //    return menus;
        //}

        public Boolean exitChild(string ParentId)
        {
            if (ParentId == null)
                ParentId = "0";
            List<SysModule> list = m_Rep.FindList(a => a.ParentId == ParentId).ToList();
            if (list.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
