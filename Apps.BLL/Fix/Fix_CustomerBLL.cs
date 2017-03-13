/**
* 命名空间: Apps.BLL.Fix
*
* 功 能： N/A
* 类 名： Fix_CustomerBLL
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
* V0.01 2017-3-12 10:55:12 王仁禧 初版
*
* Copyright (c) 2017 Lir Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：大连安琪科技有限公司 　　　　　　　　　　　　　　       │
*└──────────────────────────────────┘
*/
using Apps.Common;
using Apps.DAL.Fix;
using Apps.Models;
using Apps.Models.FIX;
using LinqToExcel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Apps.BLL.Fix
{
    public partial class Fix_CustomerBLL
    {
        public Fix_CustomerRepository m_Rep;
        public Fix_MatchingRepository matching_Rep;
        public Fix_CustomerBLL()
        {
            m_Rep = new Fix_CustomerRepository();
            matching_Rep = new Fix_MatchingRepository();
        }

        #region 判断客户是否存在
        /// <summary>
        /// 客户是否存在
        /// </summary>
        /// <param name="Name">姓名</param>
        /// <param name="Tel">座机</param>
        /// <param name="Address">地址</param>
        /// <param name="ONU">ONU</param>
        /// <returns>0：不存在；1：存在</returns>
        public int IsCustomerExist(string Name, string Tel, string Address, string ONU)
        {
            //获取实体列表
            IQueryable<FIX_CustomerModel> customers = m_Rep.FindList().Where(cm => cm.Name == Name && cm.Tel == Tel && cm.Address == Address && cm.ONU == ONU);
            if (customers.Count() > 0)
            {
                return 1;
            }
            return 0;
        }
        #endregion

        #region 校验Excel数据
        /// <summary>
        /// 校验Excel数据
        /// </summary>
        public bool CheckImportData(string fileName, List<FIX_CustomerModel> customerList, ref ValidationErrors errors)
        {

            var targetFile = new FileInfo(fileName);

            if (!targetFile.Exists)
            {

                errors.Add("导入的数据文件不存在");
                return false;
            }

            var excelFile = new ExcelQueryFactory(fileName);

            var sheetList = excelFile.GetWorksheetNames();
            foreach (var sheet in sheetList)
            {
                //获得sheet对应的数据
                var data = excelFile.WorksheetNoHeader(sheet).ToList();
                //sheet里不为空再去获取值
                if (data.Count > 2)
                {
                    for (var i = 3; i < data.Count; i++)
                    {
                        var Tel = data[i][4];
                        var Name = data[i][11];
                        var Address = data[i][12];
                        var ONU = data[i][18];
                        if (!string.IsNullOrEmpty(Address) && this.IsCustomerExist(Name, Tel, Address, ONU) == 0)
                        {
                            FIX_CustomerModel customer = new FIX_CustomerModel();
                            customer.CreatePerson = "System";
                            customer.CreateTime = DateTime.Now.ToShortDateString();
                            customer.Name = Name;
                            customer.Tel = Tel;
                            customer.Address = Address;
                            customer.ONU = ONU;
                            customerList.Add(customer);
                        }
                    }
                }
            }
            if (errors.Count > 0)
            {
                return false;
            }
            return true;
        }
        #endregion

        #region 保存数据
        /// <summary>
        /// 保存数据
        /// </summary>
        public void SaveImportData(IEnumerable<FIX_CustomerModel> customerList)
        {
            try
            {
                var matchAddressList = new List<string>();
                DbContexts db = new DbContexts();
                foreach (var model in customerList)
                {
                    FIX_CustomerModel entity = new FIX_CustomerModel();
                    entity.Name = model.Name;
                    entity.Phone = model.Phone;
                    entity.Tel = model.Tel;
                    entity.Address = model.Address;
                    entity.ONU = model.ONU;
                    entity.CreateTime = ResultHelper.NowTime.ToString("yyyy-MM-dd HH:mm:ss");
                    db.FIX_CustomerModel.Add(entity);

                    var matchingAddress = entity.Address.ToString().Substring(0, entity.Address.ToString().IndexOf("号") + 1);

                    if (!matchAddressList.Contains(matchingAddress))
                    {
                        FIX_MatchingModel matching = new FIX_MatchingModel();
                        matching.CreatePerson = "System";
                        matching.CreateTime = DateTime.Now.ToShortDateString();
                        matching.Address = matchingAddress;
                        matching.OTM = "0";
                        matchAddressList.Add(matchingAddress);
                        db.FIX_MatchingModel.Add(matching);
                    }
                }
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
    }
}
