
using Apps.DAL.Calendar;
using Apps.DAL.LianTong;
using Apps.Models.Calendar;
using Apps.Models.LianTong;
using System;
using System.Linq;

namespace Apps.BLL.Calendar
{
    public partial class CalendarBLL
    {
        public CalendarRepository m_Rep;
        public CalendarBLL()
        {
            m_Rep = new CalendarRepository();
        }

        public IQueryable<Task> FindList(string UserId, Nullable<DateTime> fromDate, Nullable<DateTime> toDate)
        {
            //获取实体列表
            IQueryable<Task> _TaskInfos = m_Rep.FindList(cm => (cm.UserId == UserId || cm.UserId == "system"));
            int testt;
            testt = _TaskInfos.Count();
            if (fromDate != null) _TaskInfos = _TaskInfos.Where(cm => cm.start >= fromDate);
            int testt2;
            testt2 = _TaskInfos.Count();
            if (toDate != null) _TaskInfos = _TaskInfos.Where(cm => cm.start <= toDate);
            int testt3;
            testt3 = _TaskInfos.Count();

            _TaskInfos = _TaskInfos.OrderBy(w => w.id);
            return _TaskInfos.AsQueryable();
        }
    }
}
