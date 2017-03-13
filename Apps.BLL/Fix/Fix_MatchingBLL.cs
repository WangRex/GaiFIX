
using Apps.Common;
/**
* 命名空间: Apps.BLL.Fix
*
* 功 能： N/A
* 类 名： Fix_MatchingBLL
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
* V0.01 2017/3/9 14:43:28 王仁禧 初版
*
* Copyright (c) 2017 Lir Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：大连安琪科技有限公司 　　　　　　　　　　　　　　       │
*└──────────────────────────────────┘
*/
using Apps.DAL.Fix;
using Apps.DAL.Sys;
using Apps.Models;
using Apps.Models.FIX;
using Apps.Models.Sys;
using LinqToExcel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace Apps.BLL.Fix
{
    public class Fix_MatchingBLL
    {
        public Fix_MatchingRepository m_Rep;
        public SysUserRepository user_Rep;
        public Fix_MatchingBLL()
        {
            m_Rep = new Fix_MatchingRepository();
            user_Rep = new SysUserRepository();
        }

        #region 获取匹配人员信息
        public List<UserView> GetMatchingListByUser(string matchingId, string depId, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                name = string.Empty;
            }
            if (string.IsNullOrEmpty(depId))
            {
                depId = string.Empty;
            }
            //String sql = "select a.Id,a.UserName,a.TrueName,ISNULL(b.OTM, '0') as Flag from SysUser a left join FIX_MatchingModel b on cast(a.Id as varchar) = b.OTM and b.Id = @matchingId Where a.DepId =  @DepId and a.TrueName = '@name' order by b.OTM desc";
            String sql = "select a.Id,a.UserName,a.TrueName,ISNULL(b.OTM, '0') as Flag from SysUser a left join FIX_MatchingModel b on cast(a.Id as varchar) = b.OTM and b.Id = @matchingId Where a.DepId =  @DepId order by b.OTM desc";
            //SqlParameter[] sqlParameters = { new SqlParameter { ParameterName = "matchingId", Value = matchingId }, new SqlParameter { ParameterName = "DepId", Value = depId }, new SqlParameter { ParameterName = "name", Value = name } };
            SqlParameter[] sqlParameters = { new SqlParameter { ParameterName = "matchingId", Value = matchingId }, new SqlParameter { ParameterName = "DepId", Value = depId } };
            DbContexts DbContext = new DbContexts();
            return DbContext.Database.SqlQuery<UserView>(sql, sqlParameters).ToList();
        }
        #endregion

        #region 绑定/解绑外线员
        public Boolean BindingUser(string matchingId, string userId, string updatePerson)
        {
            var matching = m_Rep.Find(Convert.ToInt32(matchingId));
            if (matching == null)
            {
                return false;
            }
            if (!string.IsNullOrWhiteSpace(userId))
            {
                var user = user_Rep.Find(Convert.ToInt32(userId));
                if (user != null)
                {
                    matching.OTM = user.Id.ToString();
                    matching.OTM_Name = user.TrueName;
                    matching.Status = "已匹配";
                    matching.UpdatePerson = updatePerson;
                    matching.UpdateTime = DateTime.Now.ToShortDateString();
                    m_Rep.Update(matching);
                }
                else
                {
                    return false;
                }
            } else
            {
                matching.OTM = "0";
                matching.OTM_Name = string.Empty;
                matching.Status = "未匹配";
                matching.UpdatePerson = updatePerson;
                matching.UpdateTime = DateTime.Now.ToShortDateString();
                m_Rep.Update(matching);
            }
            return true;
        }
        #endregion

        #region 根据地址获取外线员
        /// <summary>
        /// 根据地址获取外线员
        /// </summary>
        /// <param name="matchingAddress">匹配地址</param>
        /// <returns></returns>
        public UserOTM getOTM(string matchingAddress)
        {
            List<FIX_MatchingModel> list = m_Rep.FindList().Where(m => m.Address == matchingAddress).ToList();
            UserOTM _UserOTM = null;
            if(list.Count > 0)
            {
                FIX_MatchingModel _FIX_MatchingModel = list.First();
                _UserOTM = new UserOTM();
                _UserOTM.Id = _FIX_MatchingModel.OTM;
                _UserOTM.UserName = _FIX_MatchingModel.OTM_Name;
            }
            return _UserOTM;
        }
        #endregion

        #region 根据地址获取外线员ID
        public int getOTM_IDByAddress(string matchingAddress)
        {
            int Id = 0;
            IQueryable<FIX_MatchingModel> matchings = m_Rep.FindList().Where(mm => mm.Address == matchingAddress);
            if (matchings.Count() > 0)
            {
                Id = matchings.First().Id;
            }
            return Id;
        }
        #endregion

        #region 判断地址是否存在
        public bool isAddressNotExist(string matchingAddress)
        {
            bool result = true;
            IQueryable<FIX_MatchingModel> matchings = m_Rep.FindList().Where(mm => mm.Address == matchingAddress);
            if (matchings.Count() > 0)
            {
                result = false;
            }
            return result;
        }
        #endregion
    }
}
