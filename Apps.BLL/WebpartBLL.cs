using System;
using Microsoft.Practices.Unity;
using Apps.BLL.Core;
using Apps.Common;
using Apps.Models;
using System.Collections.Generic;
using Apps.Models.Sys;
using Apps.DAL;
using System.Data.Entity;

namespace Apps.BLL
{
    public class WebpartBLL
    {
        public WebpartRepository m_Rep;

        public WebpartBLL()
        {
            m_Rep = new WebpartRepository();
        }

        /// <summary>
        /// 保存HTML
        /// </summary>
        /// <param name="html"></param>
        public bool SaveHtml(ref ValidationErrors errors, string userId, string html)
        {
            try
            {
                if (SaveHtml(userId, html))
                {
                    return true;
                }
                else
                {
                    errors.Add("保存失败！");
                    return false;
                }
            }
            catch(Exception ex)
            {
                errors.Add(ex.Message);
                ExceptionHander.WriteException(ex);
                return false;
            }

        }
        /// <summary>
        /// 新建或修改HTML
        /// </summary>
        /// <param name="html"></param>
        public bool SaveHtml(string userId, string html)
        {
            SysUserConfig ss = m_Rep.Find(a => a.Name == "webpart" && a.UserId == userId);

            if (ss == null)
            {
                ss = new SysUserConfig();
                ss.UserId = userId;
                ss.Value = html;
                ss.Type = "webpart";
                ss.State = "true";
                ss.Name = "webpart";
                return m_Rep.Create(ss);
            }
            else
            {
                ss.Value = html;
                ss.Type = "webpart";
                ss.State = "true";
                return m_Rep.Update (ss);
            }
        }
    }
}
