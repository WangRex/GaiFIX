using System;
using System.Linq;
using Apps.Models.Flow;
using Apps.Models;
using System.Data;

namespace Apps.DAL.Flow
{
    public partial class Flow_StepRepository : BaseRepository<Flow_Step>
    {
        public int Delete(string id)
        {

            Flow_Step entity = base.DbContext.Flow_Step.SingleOrDefault(a => a.Id == Convert.ToInt32(id));
                if (entity != null)
                {
                    IQueryable<Flow_StepRule> collection = from f in base.DbContext.Flow_StepRule
                                                           where f.StepId==id
                                                           select f;
                    foreach (var deleteItem in collection)
                    {
                    base.DbContext.Flow_StepRule.Remove(deleteItem);
                    }
                base.DbContext.Flow_Step.Remove(entity);
                }
                return this.SaveChanges();
        }

 

    }
}
