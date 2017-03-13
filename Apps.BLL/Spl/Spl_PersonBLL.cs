using Apps.Common;
using Apps.DAL.Spl;
using Apps.Models;
using Apps.Models.Spl;
using LinqToExcel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.BLL.Spl
{
    public partial class Spl_PersonBLL
    {
        public Spl_PersonRepository m_Rep;
        public Spl_PersonBLL()
        {
            m_Rep = new Spl_PersonRepository();
        }

        /// <summary>
        /// 校验Excel数据
        /// </summary>
        public bool CheckImportData(string fileName, List<Spl_Person> personList,ref ValidationErrors errors )
        {
          
            var targetFile = new FileInfo(fileName);

            if (!targetFile.Exists)
            {

                errors.Add("导入的数据文件不存在");
                return false;
            }

            var excelFile = new ExcelQueryFactory(fileName);

            //对应列头
            excelFile.AddMapping<Spl_Person>(x => x.Name, "Name");
            excelFile.AddMapping<Spl_Person>(x => x.Sex, "Sex");
            excelFile.AddMapping<Spl_Person>(x => x.Age, "Age");
            excelFile.AddMapping<Spl_Person>(x => x.IDCard, "IDCard");
            excelFile.AddMapping<Spl_Person>(x => x.Phone, "Phone");
            excelFile.AddMapping<Spl_Person>(x => x.Email, "Email");
            excelFile.AddMapping<Spl_Person>(x => x.Address, "Address");
            excelFile.AddMapping<Spl_Person>(x => x.Region, "Region");
            excelFile.AddMapping<Spl_Person>(x => x.Category, "Category");
            //SheetName
            var excelContent = excelFile.Worksheet<Spl_Person>(0);

            int rowIndex = 1;

            //检查数据正确性
            foreach (var row in excelContent)
            {
                var errorMessage = new StringBuilder();
                var person = new Spl_Person();

                person.KEY_Id = "";
                person.Name = row.Name;
                person.Sex = row.Sex;
                person.Age = row.Age;
                person.IDCard = row.IDCard;
                person.Phone = row.Phone;
                person.Email = row.Email;
                person.Address = row.Address;
                person.Region = row.Region;
                person.Category = row.Category;

                if (string.IsNullOrWhiteSpace(row.Name))
                {
                    errorMessage.Append("Name - 不能为空. ");
                }

                if (string.IsNullOrWhiteSpace(row.IDCard))
                {
                    errorMessage.Append("IDCard - 不能为空. ");
                }

                //=============================================================================
                if (errorMessage.Length > 0)
                {
                    errors.Add(string.Format(
                        "第 {0} 列发现错误：{1}{2}",
                        rowIndex,
                        errorMessage,
                        "<br/>"));
                }
                personList.Add(person);
                rowIndex += 1;
            }
            if (errors.Count > 0)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 校验Excel数据
        /// </summary>
        public bool CheckImportBatchData(string fileName, List<Spl_Person> personList, ref ValidationErrors errors)
        {

            var targetFile = new FileInfo(fileName);

            if (!targetFile.Exists)
            {

                errors.Add("导入的数据文件不存在");
                return false;
            }
            int rowIndex = 1;
            var excelFile = new ExcelQueryFactory(fileName);
            //获取所有的worksheet
            var sheetList = excelFile.GetWorksheetNames();
            //检查数据正确性
            foreach (var sheet in sheetList)
            {
                var errorMessage = new StringBuilder();

                //获得sheet对应的数据
                var data = excelFile.WorksheetNoHeader(sheet).ToList();
           
                //判断信息是否齐全
                if (data[1][2].Value.ToString() == "")
                {
                    errorMessage.Append("姓名不能为空");
                }
             

                var person = new Spl_Person();
                person.KEY_Id = "";
                person.Name = data[1][2].Value.ToString();
                person.Sex = data[2][2].Value.ToString();
                person.Age = Convert.ToInt32(data[3][2].Value).ToString ();
                person.IDCard = data[4][2].Value.ToString();
                person.Phone = data[5][2].Value.ToString();
                person.Email = data[6][2].Value.ToString();
                person.Address = data[7][2].Value.ToString();
                person.Region = data[8][2].Value.ToString();
                person.Category = data[9][2].Value.ToString();

                //集合错误
                if (errorMessage.Length > 0)
                {
                    errors.Add(string.Format(
                        "在Sheet {0} 发现错误：{1}{2}",
                        sheet,
                        errorMessage,
                        "<br/>"));
                }
                personList.Add(person);
                rowIndex += 1;
            }
            if (errors.Count > 0)
            {
                return false;
            }
            return true;
        }




        /// <summary>
        /// 保存数据
        /// </summary>
        public void SaveImportData(IEnumerable<Spl_Person> personList)
        {
            try
            {
                DbContexts db = new DbContexts();
                foreach (var model in personList)
                {
                    Spl_Person entity = new Spl_Person();
                    entity.KEY_Id = ResultHelper.NewId;
                    entity.Name = model.Name;
                    entity.Sex = model.Sex;
                    entity.Age = model.Age;
                    entity.IDCard = model.IDCard;
                    entity.Phone = model.Phone;
                    entity.Email = model.Email;
                    entity.Address = model.Address;
                    entity.CreateTime = ResultHelper.NowTime.ToString("yyyy-MM-dd HH:mm:ss");
                    entity.Region = model.Region;
                    entity.Category = model.Category;
                    db.Spl_Person.Add(entity);
                }
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
