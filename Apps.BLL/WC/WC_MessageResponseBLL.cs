using Apps.BLL.Core;
using Apps.Common;
using Apps.DAL.WC;
using Apps.Locale;
using Apps.Models;
using Apps.Models.Enum;
using Apps.Models.WC;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Apps.BLL.WC
{
   public partial class WC_MessageResponseBLL
    {
        public WC_MessageResponseRepository m_Rep;
        public WC_MessageResponseBLL()
        {
            m_Rep = new WC_MessageResponseRepository();
        }
        public bool PostData(ref ValidationErrors errors, WC_MessageResponse model)
        {
            try
            {

                WC_MessageResponse entity = new WC_MessageResponse();

                if (m_Rep.IsContains(a => a.KEY_Id == model.KEY_Id))
                {
                    entity = m_Rep.Find(a => a.KEY_Id == model.KEY_Id);
                }

                entity.Id = model.Id;
                entity.OfficalAccountId = model.OfficalAccountId;
                entity.MessageRule = model.MessageRule;
                entity.Category = model.Category;
                entity.MatchKey = model.MatchKey;
                entity.TextContent = model.TextContent;
                entity.ImgTextContext = model.ImgTextContext;
                entity.ImgTextUrl = model.ImgTextUrl;
                entity.ImgTextLink = model.ImgTextLink;
                entity.MeidaUrl = model.MeidaUrl;
                entity.Enable = model.Enable;
                entity.IsDefault = model.IsDefault;
                entity.Remark = model.Remark;
                entity.CreateTime = model.CreateTime;
                entity.CreateBy = model.CreateBy;
                entity.Sort = model.Sort;
                entity.ModifyTime = model.ModifyTime;
                entity.ModifyBy = model.ModifyBy;
                if (m_Rep.Create(entity))
                {
                    return true;
                }
                else
                {
                    errors.Add(Resource.NoDataChange);
                    return false;
                }

            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                ExceptionHander.WriteException(ex);
                return false;
            }
        }

        public List<WC_MessageResponse> GetList(ref GridPager pager, Expression<Func<WC_MessageResponse, bool>> predicate, string queryStr)
        {

            IQueryable<WC_MessageResponse> queryData = null;
            if (!string.IsNullOrWhiteSpace(queryStr))
            {
                queryData = m_Rep.FindList( a => a.MatchKey.Contains(queryStr));
            }
            else
            {
                queryData = m_Rep.FindList();
            }
            queryData = queryData.Where(predicate.Compile()).AsQueryable();
            pager.totalRows = queryData.Count();
            //排序
            queryData = LinqHelper.SortingAndPaging(queryData, pager.sort, pager.order, pager.page, pager.rows);
            return CreateModelList(ref queryData);
        }

        public List<WC_MessageResponse> CreateModelList(ref IQueryable<WC_MessageResponse> queryData)
        {

            List<WC_MessageResponse> modelList = (from r in queryData
                                                       select new WC_MessageResponse
                                                       {
                                                           KEY_Id = r.KEY_Id,
                                                           OfficalAccountId = r.OfficalAccountId,
                                                           MessageRule = r.MessageRule,
                                                           Category = r.Category,
                                                           MatchKey = r.MatchKey,
                                                           TextContent = r.TextContent,
                                                           ImgTextContext = r.ImgTextContext,
                                                           ImgTextUrl = r.ImgTextUrl,
                                                           ImgTextLink = r.ImgTextLink,
                                                           MeidaUrl = r.MeidaUrl,
                                                           MeidaLink = r.MeidaLink,
                                                           Enable = r.Enable,
                                                           IsDefault = r.IsDefault,
                                                           Remark = r.Remark,
                                                           Sort = r.Sort,
                                                           CreateTime = r.CreateTime,
                                                           CreateBy = r.CreateBy,
                                                           ModifyTime = r.ModifyTime,
                                                           ModifyBy = r.ModifyBy,

                                                       }).ToList();

            return modelList;
        }

        public List<WC_MessageResponse> GetListProperty(ref GridPager pager, Expression<Func<WC_MessageResponse, bool>> predicate)
        {

            IQueryable<WC_MessageResponse> queryData = null;
            queryData = m_Rep.FindList().Where(predicate.Compile()).AsQueryable();

            IQueryable<WC_MessageResponse> keys = (from r in queryData group r by new { r.MatchKey,r.Category } into g
                                                        select new WC_MessageResponse() {
                                                            MatchKey = g.Key.MatchKey,
                                                            Category = g.Key.Category,
                                                            CreateTime = g.Max(p=>p.CreateTime)
                                                        });
            pager.totalRows = keys.Count();

            keys = LinqHelper.SortingAndPaging(keys, pager.sort, pager.order, pager.page, pager.rows);

            return keys.ToList();
        }


        /// <summary>
        /// 获取消息自动回复的信息
        /// </summary>
        /// <param name="officalAccountId">请求的公众号</param>
        /// <param name="matchKey">关键字</param>
        /// <returns></returns>
        public WC_MessageResponse GetAutoReplyMessage(string officalAccountId, string matchKey)
        {
            IQueryable<WC_MessageResponse> queryable = m_Rep.FindList();
            //从数据库获取一条记录来回复,完全匹配
            WC_MessageResponse entity = queryable.Where(a => a.OfficalAccountId == officalAccountId 
            && a.MessageRule != WeChatRequestRuleEnum.Default.ToString()
            && a.MessageRule != WeChatRequestRuleEnum.Subscriber.ToString()
            && a.MessageRule != WeChatRequestRuleEnum.Location.ToString()
            && a.Category == WeChatReplyCategory.Equal.ToString()
            && a.MatchKey == matchKey
            ).FirstOrDefault();
            //如果没有符合要求的回复，那么使用包含匹配
            if (entity == null)
            {
               entity = queryable.Where(a => a.OfficalAccountId == officalAccountId
               && a.MessageRule != WeChatRequestRuleEnum.Default.ToString()
               && a.MessageRule != WeChatRequestRuleEnum.Subscriber.ToString()
               && a.MessageRule != WeChatRequestRuleEnum.Location.ToString()
               && a.Category == WeChatReplyCategory.Contain.ToString()
               && a.MatchKey.Contains(matchKey)
               ).FirstOrDefault();
            }

            //如果都没有，使用默认回复
            if (entity == null)
            {
                entity = queryable.Where(a => a.OfficalAccountId == officalAccountId 
                && a.MessageRule == WeChatRequestRuleEnum.Default.ToString()
                && a.IsDefault == "true").FirstOrDefault();
            }
            
            if (entity != null)
            {
                return this.GetById(entity.KEY_Id);
            }
            else
            {
                return null;
            }
           
        }

        public  WC_MessageResponse GetById(string id)
        {
            if (m_Rep.IsContains(a => a.KEY_Id == id))
            {
                WC_MessageResponse entity = m_Rep.GetById(id);
                WC_MessageResponse model = new WC_MessageResponse();
                model.Id = entity.Id;
                model.OfficalAccountId = entity.OfficalAccountId;
                model.MessageRule = entity.MessageRule;
                model.Category = entity.Category;
                model.MatchKey = entity.MatchKey;
                model.TextContent = entity.TextContent;
                model.ImgTextContext = entity.ImgTextContext;
                model.ImgTextUrl = entity.ImgTextUrl;
                model.ImgTextLink = entity.ImgTextLink;
                model.MeidaUrl = entity.MeidaUrl;
                model.MeidaLink = entity.MeidaLink;
                model.Enable = entity.Enable;
                model.IsDefault = entity.IsDefault;
                model.Remark = entity.Remark;
                model.Sort = entity.Sort;
                model.CreateTime = entity.CreateTime;
                model.CreateBy = entity.CreateBy;
                model.ModifyTime = entity.ModifyTime;
                model.ModifyBy = entity.ModifyBy;

                return model;
            }
            else
            {
                return null;
            }
        }

 

        public bool PostData(WC_MessageResponse model)
        {
            //如果所有开关都关掉，证明不启用回复
            if (model.Category == null)
            {
                return true;
            }
            //全部设置为不默认
            m_Rep.ExecuteSqlCommand(string.Format("update [dbo].[WC_MessageResponse] set IsDefault=0 where OfficalAccountId ='{0}' and MessageRule={1}", ResultHelper.Formatstr(model.OfficalAccountId), model.MessageRule));
            //默认回复和订阅回复,且不是图文另外处理，因为他们有3种模式，但是只有一个是默认的
            if (model.Category != WeChatReplyCategory.Image.ToString() && (model.MessageRule == WeChatRequestRuleEnum.Default.ToString() || model.MessageRule == WeChatRequestRuleEnum.Subscriber.ToString()))
            {
                //查看数据库是否存在数据
                var entity = m_Rep.DbContext.WC_MessageResponse.Where(p => p.OfficalAccountId == model.OfficalAccountId && p.MessageRule == model.MessageRule && p.Category == model.Category).FirstOrDefault();
                if (entity != null)
                {
                    //删除原来的
                    m_Rep.DbContext.WC_MessageResponse.Remove(entity);
                }
            }
            //全部设置为默认
            m_Rep.ExecuteSqlCommand(string.Format("update [dbo].[WC_MessageResponse] set IsDefault=1 where OfficalAccountId ='{0}' and MessageRule={1} and Category={2}", ResultHelper.Formatstr(model.OfficalAccountId), model.MessageRule, model.Category));
            //修改
            if (m_Rep.IsContains(a => a.KEY_Id == model.KEY_Id))
            {
                m_Rep.DbContext.Entry<WC_MessageResponse>(model).State = EntityState.Modified;
                return m_Rep.Update(model);
            }
            else
            {
                return m_Rep.Create(model);
            }
        }



        /// <summary>
        /// 获得订阅时候回复的内容
        /// </summary>
        /// <param name="officalAccountId"></param>
        /// <returns></returns>
        public List<WC_MessageResponse> GetSubscribeResponseContent(string officalAccountId)
        {
            return m_Rep.DbContext.WC_MessageResponse.Where(a => a.OfficalAccountId == officalAccountId && a.MessageRule == WeChatRequestRuleEnum.Subscriber.ToString() && a.IsDefault== "true").ToList();
        }

    }
}
