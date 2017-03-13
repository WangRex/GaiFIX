using Apps.Common;
using Apps.BLL.WC;
using Apps.DAL.WC;
using Apps.Models;
using Apps.Models.JOB;
using Apps.Models.WC;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apps.DAL;

namespace Apps.Jobs.WC
{
    public class WeChatGetTokenJob : ITaskJob
    {

        public string RunJob(ref JobDataMap dataMap, string jobName, string id, string taskName)
        {

            WC_OfficalAccountsRepository m_Rep = new WC_OfficalAccountsRepository();

            IQueryable<WC_OfficalAccounts> queryable = m_Rep.FindList();
            ValidationErrors validationErrors = new ValidationErrors();
            foreach (var entity in queryable)
            {
                if (!string.IsNullOrEmpty(entity.AppId) && !string.IsNullOrEmpty(entity.AppSecret))
                {
                    entity.AccessToken = Senparc.Weixin.MP.CommonAPIs.CommonApi.GetToken(entity.AppId, entity.AppSecret).access_token;
                    entity.ModifyTime = ResultHelper.NowTime.ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
            if(queryable.Count()>0)
            {
                TaskJob.UpdateState(ref validationErrors, jobName, 1, "成功");
                m_Rep.SaveChanges();
            }

            return "批量更新Access_Token！";
        }

        public string RunJobBefore(Job jobModel)
        {
            Log.Write("RunJobBefor", jobModel.taskName,"运行");
            ValidationErrors validationErrors = new ValidationErrors();

            WC_OfficalAccountsRepository m_Rep = new WC_OfficalAccountsRepository();
            IQueryable<WC_OfficalAccounts> queryable = m_Rep.FindList();
                int count = queryable.Count();
                if (count < 1)
                {
                    return "没有符合获取Access_Token的数据！";
                }
                return null;
          
        }


        public string CloseJob(Job jobModel)
        {
            ValidationErrors validationErrors = new ValidationErrors();

            Log.Write("CloseJob", jobModel.taskName,"关闭");
            TaskJob.UpdateState(ref validationErrors, jobModel.id, 3, "挂起");
            return "关闭获取Access_Token任务";
           
        }
    }
}
