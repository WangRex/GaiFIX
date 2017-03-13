using System;
using System.Linq;
using Apps.Models.Flow;
using Apps.Models;
using System.Data;
using System.Collections.Generic;

namespace Apps.DAL.Flow
{
    public partial class Flow_FormContentStepCheckRepository : BaseRepository<Flow_FormContentStepCheck>
    {
        public IQueryable<Flow_FormContentStepCheck> GetListByFormId(string formId, string contentId)
        {
            IQueryable<Flow_FormContentStepCheck> list = from a in DbContext.Flow_FormContentStepCheck
                                                         join b in DbContext.Flow_Step
                                                         on a.StepId equals b.Id.ToString()
                                                         where b.FormId == formId & a.ContentId == contentId
                                                         select a;
            return list;
        }

        public void ResetCheckStateByFormCententId(string stepCheckId, string contentId, string checkState, string checkFlag)
        {
            //Context.P_Flow_ResetCheckStepState(stepCheckId, contentId, checkState, checkFlag);
            //重新设置当前表单步骤的状态
            //update Flow_FormContentStepCheck set State = @CheckState where ContentId = @ContentId and id!= @stepCheckId
            Flow_FormContentStepCheckStateRepository _Flow_FormContentStepCheckStateRepository = new Flow_FormContentStepCheckStateRepository();
            List<Flow_FormContentStepCheck> upModelList = FindList(a => a.ContentId == contentId && a.Id.ToString() != stepCheckId).ToList();
            foreach (Flow_FormContentStepCheck upm in upModelList)
            {
                //根据表单步骤设置其子下步骤分解的状态
                Flow_FormContentStepCheckState upInfo = _Flow_FormContentStepCheckStateRepository.Find(Convert.ToInt32(upm.Id));
                upInfo.CheckFlag = checkFlag;
                _Flow_FormContentStepCheckStateRepository.Update(upInfo);
            }
        }
    }
}
