using System;
using System.Linq;
using Apps.Models.Flow;
using Apps.Models;
using System.Data;

namespace Apps.DAL.Flow
{
    public partial class Flow_FormContentRepository : BaseRepository<Flow_FormContent>
    {
        public IQueryable<Flow_FormContent> GeExamineListByUserId(string userId)
        {
            IQueryable<Flow_FormContent> list = (from a in DbContext.Flow_FormContent
                                                 join b in DbContext.Flow_Step
                                                on a.FormId equals b.FormId
                                                 join c in DbContext.Flow_FormContentStepCheck
                                                on b.Id.ToString() equals c.StepId
                                                 join d in DbContext.Flow_FormContentStepCheckState
                                                on c.Id.ToString() equals d.StepCheckId
                                                 where d.UserId == userId && a.IsDelete != "true"
                                                 select a).Distinct();
            return list;
        }

        public IQueryable<Flow_FormContent> GeExamineList()
        {
            IQueryable<Flow_FormContent> list = (from a in DbContext.Flow_FormContent
                                                 join b in DbContext.Flow_Step
                                                 on a.FormId equals b.FormId
                                                 join c in DbContext.Flow_FormContentStepCheck
                                                 on b.Id.ToString() equals c.StepId
                                                 join d in DbContext.Flow_FormContentStepCheckState
                                                 on c.Id.ToString() equals d.StepCheckId
                                                 select a).Distinct();
            return list;
        }
    }
}
