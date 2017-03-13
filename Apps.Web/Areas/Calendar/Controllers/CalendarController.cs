using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Apps.Common;
using Apps.Web.Core;
using Apps.Models.Calendar;
using Apps.BLL.Calendar;
using Newtonsoft.Json;

namespace Apps.Web.Areas.Calendar.Controllers
{
    public class CalendarController : BaseController
    {
        private CalendarBLL _calendarBLL = new CalendarBLL();

        // GET: Calendar/Calendar
        public ActionResult Calendar()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Calendar_LoadEvent(Task _task)
        {
            List<Task> queryData = _calendarBLL.FindList(GetAccount().Id, _task.start, _task.end).ToList();
            string sstemp = JsonConvert.SerializeObject(queryData);
            sstemp.Replace("id", "_id");
            return Json(sstemp);
        }

        [HttpPost]
        public JsonResult Calendar_AddEvent(Task _task)
        {
            string roles = GetAccount().RoleName;
            if (_task.className == "label-danger" || _task.className == "label-success")
            {
                if (roles.Contains("system"))
                {
                    _task.UserId = "system";
                }
                else
                {
                    return Json(JsonHandler.CreateMessage(0, "权限不足，无法使用系统级标签"), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                _task.UserId = GetAccount().Id;
            }
            if (_task.id > 0)
            {
                var _taskInfo = _calendarBLL.m_Rep.Find(_task.id);
                //_taskInfo.className = _task.className;
                _taskInfo.title = _task.title;
                //_taskInfo.start = _task.start;
                //_taskInfo.end = _task.end;
                //_taskInfo.allDay = _task.allDay;
                _calendarBLL.m_Rep.Update(_taskInfo);
            }
            else
            {
                if (_task.allDay && _task.end == null)
                {
                    _task.end = _task.start;
                }
                _calendarBLL.m_Rep.Create(_task);
            }
            return Json(JsonHandler.CreateMessage(1, ""));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public JsonResult Calendar_DeleteEvent(int id)
        {
            var _Calendar = _calendarBLL.m_Rep.Find(id);
            if (_Calendar == null)
            {
                return Json(JsonHandler.CreateMessage(0, "数据异常"));
            }
            string roles = GetAccount().RoleName;
            if (!roles.Contains("system") && (_Calendar.className == "label-danger" || _Calendar.className == "label-success"))
            {
                return Json(JsonHandler.CreateMessage(0, "权限不足，无法删除系统级标签"));
            }
            _calendarBLL.m_Rep.Delete(id);
            return Json(JsonHandler.CreateMessage(1, ""));
        }

        public JsonResult Calendar_DropEvent(Task _task)
        {
            string roles = GetAccount().RoleName;
            if (_task.className == "label-danger" || _task.className == "label-success")
            {
                if (roles.Contains("system"))
                {
                    _task.UserId = "system";
                }
                else
                {
                    return Json(JsonHandler.CreateMessage(0, "权限不足，无法移动系统级标签"));
                }
            }
            else
            {
                _task.UserId = GetAccount().Id;
            }
            if (_task.id > 0)
            {
                var _taskInfo = _calendarBLL.m_Rep.Find(_task.id);
                if (_taskInfo != null)
                {
                    //_taskInfo.className = _task.className;
                    //_taskInfo.title = _task.title;
                    _taskInfo.start = _task.start;
                    _taskInfo.end = _task.end;
                    _taskInfo.allDay = _task.allDay;
                    _calendarBLL.m_Rep.Update(_taskInfo);
                }

            }
            return Json(JsonHandler.CreateMessage(1, ""));
        }

    }
}