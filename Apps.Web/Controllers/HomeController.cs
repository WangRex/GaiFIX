using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Apps.Models.Sys;
using Apps.Common;
using System.Globalization;
using System.Threading;
using System.Text;
using System;
using Apps.Web.Core;
using Apps.Locale;
using Apps.BLL.Sys;
using Apps.BLL;
using Apps.Core.OnlineStat;
using Apps.Models;
using Apps.BLL.MIS;
using Apps.BLL.Flow;
using Apps.Models.Flow;
using Apps.Models.MIS;
using Apps.Models.Enum;
using Apps.Models.LianTong;
using Apps.BLL.LianTong;

namespace Apps.Web.Controllers
{
    public class HomeController : BaseController
    {
        #region UI框架

        public HomeBLL homeBLL = new HomeBLL();
        public SysModuleBLL m_BLL = new SysModuleBLL();
        private SysConfigModel siteConfig = new SysConfigBLL().loadConfig(Utils.GetXmlMapPath("Configpath"));
        ValidationErrors errors = new ValidationErrors();
        public SysUserConfigBLL userConfigBLL = new SysUserConfigBLL();

        public SysUserBLL userBLL = new SysUserBLL();
        public SysStructBLL structBLL = new SysStructBLL();
        public SysAreasBLL areasBLL = new SysAreasBLL();
        public SysUserBLL sysUserBLL = new SysUserBLL();
        public AccountBLL accountBLL = new AccountBLL();
        public MIS_ArticleBLL atr_BLL = new MIS_ArticleBLL();
        public Flow_FormContentBLL formContentBLL = new Flow_FormContentBLL();
        private LianTong_ProjectContractsBLL contractsBLL = new LianTong_ProjectContractsBLL();

        public ActionResult Index()
        {
            if (Session["Account"] != null)
            {
                //获取是否开启WEBIM
                ViewBag.IsEnable = siteConfig.webimstatus;
                //获取信息间隔时间
                ViewBag.NewMesTime = siteConfig.refreshnewmessage;
                //系统名称
                ViewBag.WebName = siteConfig.webname;
                //公司名称
                ViewBag.ComName = siteConfig.webcompany;
                //网站标题
                ViewBag.Title = siteConfig.webtitle;
                //版权
                ViewBag.CopyRight = siteConfig.webcopyright;
                //在线人数
                OnlineUserRecorder recorder = HttpContext.Cache[OnlineHttpModule.g_onlineUserRecorderCacheKey] as OnlineUserRecorder;
                ViewBag.OnlineCount = recorder.GetUserList().Count;
                Account account = new Account();
                account = (Account)Session["Account"];
                return View(account);
            }
            else
            {
                return RedirectToAction("Index", "Account");
            }


        }

        //父ID=0的数据为顶级菜单
        public JsonResult GetTopMenu()
        {
            //加入本地化
            CultureInfo info = Thread.CurrentThread.CurrentCulture;
            string infoName = info.Name;
            if (Session["Account"] != null)
            {
                //加入本地化
                Account account = (Account)Session["Account"];
                List<SysModule> list = m_BLL.GetMenuByPersonId(Convert.ToInt32(account.Id), "0");
                var json = from r in list
                           select new
                           {
                               id = r.Id,
                               text = infoName.IndexOf("zh") > -1 || infoName == "" ? r.Name : r.EnglishName,     //text
                               attributes = (infoName.IndexOf("zh") > -1 || infoName == "" ? "zh-CN" : "en-US") + "/" + r.Url,
                               iconCls = r.Iconic
                           };


                return Json(json);
            }
            else
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }
        }



        /// <summary>
        /// 获取导航菜单
        /// </summary>
        /// <param name="id">所属</param>
        /// <returns>树</returns>
        public JsonResult GetTreeByEasyui(string id)
        {
            //加入本地化
            CultureInfo info = Thread.CurrentThread.CurrentCulture;
            string infoName = info.Name;
            if (Session["Account"] != null)
            {
                //加入本地化
                Account account = (Account)Session["Account"];
                List<SysModule> list = m_BLL.GetMenuByPersonId(Convert.ToInt32(account.Id), id).ToList();
                var json = from r in list
                           select new
                           {
                               id = r.Id,
                               text = infoName.IndexOf("zh") > -1 || infoName == "" ? r.Name : r.EnglishName,     //text
                               attributes = (infoName.IndexOf("zh") > -1 || infoName == "" ? "zh-CN" : "en-US") + "/" + r.Url,
                               //attributes =  "/" + r.Url,
                               iconCls = r.Iconic,
                               state = (m_BLL.exitChild(r.Id.ToString())) ? "closed" : "open"
                           };


                return Json(json);
            }
            else
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult SetThemes(string theme, string menu,bool topmenu)
        {
            SysUserConfig entity = userConfigBLL.GetByUserType("themes", GetUserId());
            if (entity != null)
            {
                entity.Value = theme;
                userConfigBLL._SysUserConfigRepository.Update(entity);
            }
            else
            {   
                entity = new SysUserConfig()
                {
                    Name = "用户自定义主题",
                    Value = theme,
                    Type = "themes",
                    State = "true",
                    UserId = GetUserId()
                };
                userConfigBLL._SysUserConfigRepository.Create(entity);
            }
            Session["themes"] = theme;

            //开启顶部菜单，顶部菜单必须配置多一层
            if (topmenu)
            {
                menu = menu + ",topmenu";
            }
            SysUserConfig entityMenu = userConfigBLL.GetByUserType("menu", GetUserId());
            if (entityMenu != null)
            {
                entityMenu.Value = menu;
                userConfigBLL._SysUserConfigRepository.Update(entityMenu);
            }
            else
            {
                entityMenu = new SysUserConfig()
                {
                    Name = "用户自定义菜单",
                    Value = menu,
                    Type = "menu",
                    State = "true",
                    UserId = GetUserId()
                };
                userConfigBLL._SysUserConfigRepository.Create (entityMenu);

            }

            Session["menu"] = menu;
            return Json("1", JsonRequestBehavior.AllowGet);
        }


        public ActionResult TopInfo()
        {
            if (Session["Account"] != null)
            {
                Account account = new Account();
                account = (Account)Session["Account"];
                return View(account);
            }
            return View();
        }

        #endregion

        #region js配置

        public JavaScriptResult ConfigJs()
        {
            CultureInfo info = Thread.CurrentThread.CurrentCulture;
            StringBuilder sb = new StringBuilder();
            sb.Append("var _YMGlobal;");
            sb.Append("(function(_YMGlobal) {");
            sb.Append("    var Config = (function() {");
            sb.Append("        function Config() {}");
            sb.AppendFormat("  Config.currentCulture = '{0}';", info.Name);
            sb.AppendFormat("  Config.apiUrl = '{0}';", "");
            sb.AppendFormat("  Config.token = '{0}';", "");
            sb.Append("       return Config;");
            sb.Append("   })();");
            sb.Append("   _YMGlobal.Config = Config;");
            sb.Append(" })(_YMGlobal || (_YMGlobal = { }));");

            return JavaScript(sb.ToString());
        }

        #endregion

        #region 我的资料
        public ActionResult Info()
        {
            int Id = Convert.ToInt16(GetUserId());
            SysUser entity = sysUserBLL.m_Rep.Find(Id);
            //防止读取错误
            return View(entity);
        }


        [HttpPost]
        public JsonResult EditPwd(string oldPwd, string newPwd)
        {
            SysUser user = accountBLL.Login(GetAccount().UserName, ValueConvert.MD5(oldPwd));
            if (user == null)
            {
                return Json(JsonHandler.CreateMessage(0, "旧密码不匹配！"), JsonRequestBehavior.AllowGet);
            }
            user.Password = ValueConvert.MD5(newPwd);

            if (userBLL.m_Rep.Update(user))
            {
                LogHandler.WriteServiceLog(GetUserId(), "Id:" + GetUserId() + ",密码:********", "成功", "初始化密码", "用户设置");
                return Json(JsonHandler.CreateMessage(1, Resource.EditSucceed), JsonRequestBehavior.AllowGet);
            }
            else
            {
                string ErrorCol = errors.Error;
                LogHandler.WriteServiceLog(GetUserId(), "Id:" + GetUserId() + ",,密码:********" + ErrorCol, "失败", "初始化密码", "用户设置");
                return Json(JsonHandler.CreateMessage(0, Resource.EditFail + ":"+ErrorCol), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion


        #region webpart

        public WebpartBLL webPartBLL = new WebpartBLL();


        public ActionResult Desktop()
        {
            string userId = GetUserId();
            //获取配置文件里的页面内容，没有就显示默认的。
            SysUserConfig ss = webPartBLL.m_Rep.Find(a => a.UserId == userId && a.State == "true");
            if (ss != null)
            {
                ViewBag.Value = ss.Value;
            }
            else
            {
                ViewBag.Value = "";
            }
            return View();
        }
        [HttpPost]
        public JsonResult GetPartDataByShortcut()
        {

            return Json("", JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetPartDataByMyJob()
        {
            GridPager pager = new GridPager()
            {
                page = 1,
                rows = 5,
                sort = "CreateTime",
                order = "desc"
            };
            List<Flow_FormContent> list = formContentBLL.GeExamineListByUserId(ref pager,"",GetUserId().ToString()).ToList();

            var json = new
            {
                total = pager.totalRows,
                rows = (from r in list
                        select new Flow_FormContent()
                        {
                            Id = r.Id,
                            Title = r.Title,
                            UserId = r.UserId,
                            UserName = r.UserName,
                            FormId = r.FormId,
                            FormLevel = r.FormLevel,
                            CreateTime = r.CreateTime,
                            TimeOut = r.TimeOut,
                            CurrentStep = formContentBLL.GetCurrentFormStep(r),
                            CurrentState = formContentBLL.GetCurrentFormState(r)

                        }).OrderByDescending(a => a.CurrentState).ToArray()

            };
            return Json(json);
        }

        [HttpPost]
        public JsonResult GetPartDataByNotice()
        {

            List<MIS_Article> list = new List<MIS_Article>();
            GridPager pager = new GridPager()
            {
                page = 1,
                rows = 8,
                sort = "Id",
                order = "desc"
            };
            list = atr_BLL.GetList(ref pager, "", "", true, "", 2);

            StringBuilder sb = new StringBuilder("");
            sb.Append("<table style=\"width:100%\">");
            foreach (var i in list)
            {
                sb.AppendFormat("<tr style='height:33px;line-height:33px; padding:5px;'><td>&nbsp;&nbsp;&nbsp;[{0}]&nbsp;<a class=\"color-black\" href=\"javascript:ShowInfo('{1}','{2}')\">{3}</a></td><td class=\"color-black\" style=\"width:75px\">[{4}]</td></tr>", i.CategoryName,i.Title, i.Id, i.Title,Convert.ToDateTime(i.CreateTime).ToShortDateString());
            }
            sb.Append("</table>");
            return Json(sb.ToString(), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetPartDataByData()
        {
            return Json("", JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetPartDataByNote()
        {
            return Json("<span style='color:#b200ff'>语言版本进行大部分翻译，其他未翻译部分需要自行翻译<span>", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetPartData6()
        {

            // 获取在线用户记录器
            OnlineUserRecorder recorder = HttpContext.Cache[OnlineHttpModule.g_onlineUserRecorderCacheKey] as OnlineUserRecorder;
            StringBuilder sb = new StringBuilder("");
            if (recorder == null)
                return Json("在线用户", JsonRequestBehavior.AllowGet);

            //// 绑定在线用户列表
            IList<OnlineUser> userList = recorder.GetUserList();
            sb.AppendFormat("在线用户：<br>");
            foreach (var OnlineUser in userList)
            {
                sb.AppendFormat(OnlineUser.UserName + "<br>");
            }
            return Json(sb.ToString(), JsonRequestBehavior.AllowGet);
        }

        ValidationErrors validationErrors = new ValidationErrors();
        [ValidateInput(false)]
        public JsonResult SaveHtml(string html)
        {
            webPartBLL.SaveHtml(ref validationErrors, GetUserId(), html);
            return Json("保存成功!", JsonRequestBehavior.AllowGet);
        }
        #endregion

        [HttpPost]
        public JsonResult Desktop_MyJobPart()
        {
            GridPager pager = new GridPager()
            {
                page = 1,
                rows = 5,
                sort = "Id",
                order = "desc"
            };
            string DepId = GetAccount().DepId;
            string QuaryCD = structBLL.m_Rep.Find(Convert.ToInt32(DepId)).Remark;
            List<LianTong_ProjectContractsModel> list;
            if ("*".Equals(QuaryCD))
            {
                list = contractsBLL.m_Rep.FindPageList(ref pager, a => (a.history != null && a.history != string.Empty)).ToList();
            }
            else
            {
                list = contractsBLL.m_Rep.FindPageList(ref pager, a => a.department == DepId && (a.history != null && a.history != string.Empty)).ToList();
            }

            var json = new
            {

                total = pager.totalRows,
                rows = (from r in list
                        select new
                        {
                            Id = r.Id,
                            Title = String.IsNullOrEmpty(r.status) ? "待审查" : r.status,
                            contractNum = "合同编号：" + r.contractNum,
                            departmentName = r.departmentName,
                            validDate = r.validDate,
                            CurrentStep =  GetFlowStepStr(r.history),
                            FormLevel = GetFormLevel(r)
                        }).OrderByDescending(a => a.Id).ToArray()

            };
            return Json(json);
        }

        public string GetFlowStepStr(string step)
        {
            string refStr = string.Empty;
            switch (step)
            {
                case "1":
                    refStr = FlowLianTongContracts.送审.ToString();
                    break;
                case "2":
                    refStr = FlowLianTongContracts.补全.ToString();
                    break;
                case "3":
                    refStr = FlowLianTongContracts.审订.ToString();
                    break;
                case "4":
                    refStr = FlowLianTongContracts.开票.ToString();
                    break;
                case "5":
                    refStr = FlowLianTongContracts.回款.ToString();
                    break;
                case "6":
                    refStr = FlowLianTongContracts.完结.ToString();
                    break;
                default:
                    refStr = "未关联";
                    break;
            }
            return refStr;
        }

        public int GetFormLevel(LianTong_ProjectContractsModel _LianTong_ProjectContractsModel)
        {
            int result = FlowFormLevelEnum.Ordinary.GetInt();
            if(!string.IsNullOrEmpty(_LianTong_ProjectContractsModel.validDate))
            {
                DateTime validDate = Convert.ToDateTime(_LianTong_ProjectContractsModel.validDate);
                TimeSpan ts = validDate - DateTime.Now;
                if (ts.Days <= 2 && ts.Days > 0 && !_LianTong_ProjectContractsModel.status.Equals("已完结"))
                {
                    result = FlowFormLevelEnum.Urgent.GetInt();
                } else if(ts.Days < 0)
                {
                    _LianTong_ProjectContractsModel.status = "已过期";
                    contractsBLL.m_Rep.Update(_LianTong_ProjectContractsModel);
                    result = FlowFormLevelEnum.Overdue.GetInt();

                }
            }
            if(_LianTong_ProjectContractsModel.status.Equals("未通过"))
            {
                result = FlowFormLevelEnum.NotPass.GetInt();
            }
            return result;
        }
    }
}