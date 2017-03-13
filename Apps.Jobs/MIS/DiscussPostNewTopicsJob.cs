using Apps.Common;
using Apps.BLL.MIS;
using Apps.DAL.MIS;
using Apps.Models;
using Apps.Models.JOB;
using Apps.Models.MIS;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Jobs.MIS
{
    public class DiscussPostNewTopicsJob : ITaskJob
    {

        public string RunJob(ref JobDataMap dataMap, string jobName, string id, string taskName)
        {

            MIS_ArticleBLL discussArticleBLL = new MIS_ArticleBLL();

            MIS_Article model = discussArticleBLL.m_Rep.Find(Convert.ToInt32(id));
            string retResult = "";
            if (model == null)
            {
                retResult = "文章不存在";
                return retResult;
            }
            model.CheckFlag ="0";

            ValidationErrors validationErrors = new ValidationErrors();

            discussArticleBLL.m_Rep.Update(model);

            if (validationErrors.Count > 0)
            {
                return validationErrors.Error;
            }
            retResult = "修改成功";
            return retResult;
        }

        public string RunJobBefore(Job jobModel)
        {
            Log.Write("RunJobBefor", jobModel.taskName,"运行");
            ValidationErrors validationErrors = new ValidationErrors();
            MIS_ArticleBLL discussArticleBLL = new MIS_ArticleBLL();
            var model = discussArticleBLL.m_Rep.Find(Convert.ToInt32(jobModel.id));
            if (model == null)
            {

                return "参数不能为空!";
            }

            model.CheckFlag = "1";
            if (discussArticleBLL.m_Rep.Update(model))
            {
                return null;
            }
            else
            {
                return validationErrors.Error;
            }


        }


        public string CloseJob(Job jobModel)
        {
            Log.Write("CloseJob", jobModel.taskName,"关闭");
            ValidationErrors validationErrors = new ValidationErrors();
            MIS_ArticleBLL discussArticleBLL = new MIS_ArticleBLL();
            var model = discussArticleBLL.m_Rep.Find(Convert.ToInt32(jobModel.id));
            if (model == null)
            {

                return "参数不能为空!";
            }

            model.CheckFlag = "1";
            if (discussArticleBLL.m_Rep.Update(model))
            {
                return null;
            }
            else
            {
                return validationErrors.Error;
            }

        }
    }
}
