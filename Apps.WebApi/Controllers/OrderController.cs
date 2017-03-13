using Apps.BLL.Fix;
using Apps.BLL.Sys;
using Apps.DAL;
using Apps.Models.FIX;
using Apps.Models.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Apps.WebApi.Controllers
{
    /// <summary>  
    /// 订单信息  
    /// </summary>  
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class OrderController : WebAPI2BaseController
    {
        private Fix_CustomerBLL customer_BLL = new Fix_CustomerBLL();
        private Fix_MatchingBLL matching_BLL = new Fix_MatchingBLL();
        private Fix_OrderBLL order_BLL = new Fix_OrderBLL();
        private Fix_OrderHistoryBLL orderHistory_BLL = new Fix_OrderHistoryBLL();
        private Fix_AssessmentBLL assessment_BLL = new Fix_AssessmentBLL();
        private SysUserBLL user_BLL = new SysUserBLL();
        log4net.ILog log = log4net.LogManager.GetLogger("GaiFix.Logging");

        /// <summary>  
        /// 用户创建订单
        /// </summary>  
        /// <param name="order">订单详情</param>
        [HttpPost]
        public HttpResponseMessage CreateOrder(FIX_OrderModel order)
        {
            log.Debug("OrderController.CreateOrder Start!");
            log.Debug("Address is " + order.Address + ", MatchingAddress is " + order.MatchingAddress + ", Phone is " + order.Phone + ", Tel is " + order.Tel + ", Type is " + order.Type + ", Description is " + order.Description);
            var response = new Response();
            response.Code = 0;
            response.Message = "添加订单成功！";
            FIX_CustomerModel customer = new FIX_CustomerModel();
            customer.CreatePerson = order.Phone;
            customer.CreateTime = DateTime.Now.ToShortDateString();
            customer.UpdateTime = DateTime.Now.ToShortDateString();
            customer.Name = order.Name;
            customer.Phone = order.Phone;
            customer.Tel = order.Tel;
            customer.Address = order.Province + order.City + order.Area + order.Address;
            if (customer_BLL.IsCustomerExist(customer.Name, customer.Tel, customer.Address, null) == 0)
            {
                customer_BLL.m_Rep.Create(customer);
            }
            order.CreatePerson = "Mobile";
            order.CreateTime = DateTime.Now.ToShortDateString();
            order.UpdateTime = DateTime.Now.ToShortDateString();
            order.Assessment_ID = 0;
            order.OTM_ID = matching_BLL.getOTM_IDByAddress(order.MatchingAddress).ToString();
            if (order.OTM_ID == "0")
            {
                order.Status = "新订单";
                response.Data = "暂无外线员";
                if (!string.IsNullOrEmpty(order.MatchingAddress))
                {
                    if (matching_BLL.isAddressNotExist(order.MatchingAddress))
                    {
                        FIX_MatchingModel matching = new FIX_MatchingModel();
                        matching.CreateTime = DateTime.Now.ToShortDateString();
                        matching.UpdateTime = DateTime.Now.ToShortDateString();
                        matching.CreatePerson = "System";
                        matching.Address = order.MatchingAddress;
                        matching.OTM = "0";
                        matching_BLL.m_Rep.Create(matching);
                    }
                }
            }
            else
            {
                order.Status = "待接单";
                response.Code = 1;
                Dictionary<string, object> dic = new Dictionary<string, object>();
                SysUser user = user_BLL.m_Rep.Find(Convert.ToInt32(order.OTM_ID));
                dic.Add("OutsideTroubleMan_ID", user.Id);
                dic.Add("Name", user.TrueName);
                dic.Add("Phone", user.MobileNumber);
                dic.Add("EmployeeNo", user.EmployeeNo);
                dic.Add("WorkYear", user.WorkYear);
                dic.Add("Business", user.Business);
                dic.Add("ResponsibleAreaBrief", user.ResponsibleAreaBrief);
                response.Data = dic;
            }
            order_BLL.m_Rep.Create(order);
            FIX_OrderHistoryModel orderHistory = new FIX_OrderHistoryModel();
            orderHistory.CreatePerson = "System";
            orderHistory.CreateTime = DateTime.Now.ToShortDateString();
            orderHistory.Order_ID = order.Id;
            orderHistory.Status = order.Status;
            orderHistory.UpdateTime = DateTime.Now.ToShortDateString();
            orderHistory_BLL.m_Rep.Create(orderHistory);
            return toJson(response);
        }

        /// <summary>  
        /// 外线员获取订单列表  
        /// </summary>  
        /// <param name="OTM_ID">外线员ID</param>
        /// <param name="status">订单状态(待接单/处理中/待评价/已完成)</param>
        [HttpGet]
        public HttpResponseMessage GetOrdersByOTM(int OTM_ID, string status)
        {
            log.Debug("OrderController.GetOrdersByOTM Start!");
            log.Debug("OTM_ID is " + OTM_ID + ", status is " + status);
            var response = new Response();
            response.Code = 0;
            response.Message = "获取订单列表成功！";
            response.Data = order_BLL.getOrdersByOTM(OTM_ID, status);
            return toJson(response);
        }

        /// <summary>  
        /// 获取订单
        /// </summary>  
        /// <param name="Order_ID">订单ID</param>
        [HttpGet]
        public HttpResponseMessage GetOrder(int Order_ID)
        {
            log.Debug("OrderController.AcceptOrderByOTM Start!");
            log.Debug("Order_ID is " + Order_ID);
            var response = new Response();
            response.Code = 0;
            response.Message = "获取订单成功！";
            Dictionary<string, object> dic = new Dictionary<string, object>();
            FIX_OrderModel order = order_BLL.m_Rep.Find(Order_ID);
            if(null != order)
            {
                FIX_AssessmentModel assessment = assessment_BLL.m_Rep.Find(order.Assessment_ID);
                dic.Add("Name", order.Name);
                dic.Add("Province", order.Province);
                dic.Add("City", order.City);
                dic.Add("Area", order.Area);
                dic.Add("Time", order.Time);
                dic.Add("Address", order.Address);
                dic.Add("MatchingAddress", order.MatchingAddress);
                dic.Add("Phone", order.Phone);
                dic.Add("Tel", order.Tel);
                dic.Add("Type", order.Type);
                dic.Add("Status", order.Status);
                dic.Add("Description", order.Description);
                dic.Add("OTM_ID", order.OTM_ID);
                dic.Add("Assessment_ID", order.Assessment_ID);
                if (null != assessment)
                {
                    dic.Add("Content", assessment.Content);
                    dic.Add("QuestionStar", assessment.QuestionStar);
                    dic.Add("AttitudeStar", assessment.AttitudeStar);
                    dic.Add("ComprehensiveStar", assessment.ComprehensiveStar);
                }
            } else
            {
                response.Code = 1;
                response.Message = "获取订单失败！";
                dic.Add("Content", "没有订单Id为: " + Order_ID + " 的订单！");
            }
            response.Data = dic;
            return toJson(response);
        }

        /// <summary>  
        /// 外线员接受订单
        /// </summary>  
        /// <param name="OTM_ID">外线员ID</param>
        /// <param name="Order_ID">订单ID</param>
        [HttpGet]
        public HttpResponseMessage AcceptOrderByOTM(int OTM_ID, int Order_ID)
        {
            log.Debug("OrderController.AcceptOrderByOTM Start!");
            log.Debug("OTM_ID is " + OTM_ID + ", Order_ID is " + Order_ID);
            var response = new Response();
            response.Code = 0;
            response.Message = "接单成功！";
            FIX_OrderModel order = order_BLL.m_Rep.Find(Order_ID);
            order.UpdatePerson = user_BLL.m_Rep.Find(OTM_ID).TrueName;
            order.UpdateTime = DateTime.Now.ToShortDateString();
            order.Status = "处理中";
            order_BLL.m_Rep.Update(order);
            FIX_OrderHistoryModel orderHistory = new FIX_OrderHistoryModel();
            orderHistory.CreatePerson = order.UpdatePerson;
            orderHistory.CreateTime = DateTime.Now.ToShortDateString();
            orderHistory.Order_ID = order.Id;
            orderHistory.Status = order.Status;
            orderHistory.UpdateTime = DateTime.Now.ToShortDateString();
            orderHistory_BLL.m_Rep.Create(orderHistory);
            response.Data = true;
            return toJson(response);
        }

        /// <summary>  
        /// 外线员完成订单
        /// </summary>  
        /// <param name="OTM_ID">外线员ID</param>
        /// <param name="Order_ID">订单ID</param>
        [HttpGet]
        public HttpResponseMessage CompleteOrderByOTM(int OTM_ID, int Order_ID)
        {
            log.Debug("OrderController.CompleteOrderByOTM Start!");
            log.Debug("OTM_ID is " + OTM_ID + ", Order_ID is " + Order_ID);
            var response = new Response();
            response.Code = 0;
            response.Message = "外线员完成订单成功！";
            FIX_OrderModel order = order_BLL.m_Rep.Find(Order_ID);
            order.UpdatePerson = user_BLL.m_Rep.Find(OTM_ID).TrueName;
            order.UpdateTime = DateTime.Now.ToShortDateString();
            order.Status = "待评价";
            order_BLL.m_Rep.Update(order);
            FIX_OrderHistoryModel orderHistory = new FIX_OrderHistoryModel();
            orderHistory.CreatePerson = order.UpdatePerson;
            orderHistory.CreateTime = DateTime.Now.ToShortDateString();
            orderHistory.Order_ID = order.Id;
            orderHistory.Status = order.Status;
            orderHistory.UpdateTime = DateTime.Now.ToShortDateString();
            orderHistory_BLL.m_Rep.Create(orderHistory);
            response.Data = true;
            return toJson(response);
        }

        /// <summary>  
        /// 用户获取订单列表
        /// </summary>  
        /// <param name="Phone">用户手机号</param>
        /// <param name="Status">状态（待评价/已完成）</param>
        [HttpGet]
        public HttpResponseMessage GetAssessmentOrders(string Phone, string Status)
        {
            log.Debug("OrderController.GetWaitingAssessmentOrderByPhone Start!");
            log.Debug("Phone is " + Phone + ",Status is " + Status);
            var response = new Response();
            response.Code = 0;
            response.Message = "获取" + Status + "订单列表成功！";
            response.Data = order_BLL.getWaitingAssessmentOrders(Phone, Status);
            return toJson(response);
        }

        /// <summary>  
        /// 用户获取待评价订单详情
        /// </summary>  
        /// <param name="Phone">用户手机号</param>
        /// <param name="Order_ID">订单ID</param>
        [HttpGet]
        public HttpResponseMessage GetWaitingAssessmentOrder(string Phone, int Order_ID)
        {
            log.Debug("OrderController.GetWaitingAssessmentOrder Start!");
            log.Debug("Phone is " + Phone + ", Order_ID is " + Order_ID);
            var response = new Response();
            response.Code = 0;
            response.Message = "用户获取待评价订单成功！";
            FIX_OrderModel order = order_BLL.m_Rep.Find(Order_ID);
            SysUser user = user_BLL.m_Rep.Find(Convert.ToInt32(order.OTM_ID));
            var json = new
            {
                Order_ID = Order_ID,
                OTM_ID = order.OTM_ID,
                Name = user.TrueName,
                Phone = user.MobileNumber,
                EmployeeNo = user.EmployeeNo,
                WorkYear = user.WorkYear,
                Business = user.Business,
                ResponsibleAreaBrief = user.ResponsibleAreaBrief,
                Description = order.Description
            };
            response.Data = json;
            return toJson(response);
        }

        /// <summary>  
        /// 用户获取评已价订单详情
        /// </summary>  
        /// <param name="Phone">用户手机号</param>
        /// <param name="Order_ID">订单ID</param>
        [HttpGet]
        public HttpResponseMessage GetAssessmentOrder(string Phone, int Order_ID)
        {
            log.Debug("OrderController.GetAssessmentOrder Start!");
            log.Debug("Phone is " + Phone + ", Order_ID is " + Order_ID);
            var response = new Response();
            response.Code = 0;
            response.Message = "用户获取已评价订单成功！";
            FIX_OrderModel order = order_BLL.m_Rep.Find(Order_ID);
            SysUser user = user_BLL.m_Rep.Find(Convert.ToInt32(order.OTM_ID));
            FIX_AssessmentModel assessment = assessment_BLL.m_Rep.Find(order.Assessment_ID);
            var json = new
            {
                Name = user.TrueName,
                Phone = user.MobileNumber,
                EmployeeNo = user.EmployeeNo,
                WorkYear = user.WorkYear,
                Business = user.Business,
                ResponsibleAreaBrief = user.ResponsibleAreaBrief,
                Description = order.Description,
                Content = assessment.Content,
                QuestionStar = assessment.QuestionStar,
                AttitudeStar = assessment.AttitudeStar,
                ComprehensiveStar = assessment.ComprehensiveStar
            };
            response.Data = json;
            return toJson(response);
        }

        /// <summary>  
        /// 用户评价订单
        /// </summary>  
        /// <param name="assessment">用户评价详情</param>
        [HttpPost]
        public HttpResponseMessage AssessmentOrder(FIX_AssessmentModel assessment)
        {
            log.Debug("OrderController.GetWaitingAssessmentOrderByPhone Start!");
            log.Debug("Phone is " + assessment.Phone + ", Order_ID is " + assessment.Order_ID + ", OTM_ID is " + assessment.OTM_ID + ", ComprehensiveStar is " + assessment.ComprehensiveStar + ", AttitudeStar is " + assessment.AttitudeStar + ", QuestionStar is " + assessment.QuestionStar + ", Content is " + assessment.Content);
            var response = new Response();
            response.Code = 0;
            response.Message = "用户评价订单成功！";
            assessment.CreatePerson = assessment.Phone;
            assessment.CreateTime = DateTime.Now.ToShortDateString();
            assessment.UpdateTime = DateTime.Now.ToShortDateString();
            assessment_BLL.m_Rep.Create(assessment);
            FIX_OrderModel order = order_BLL.m_Rep.Find(assessment.Order_ID);
            order.UpdatePerson = assessment.Phone;
            order.UpdateTime = DateTime.Now.ToShortDateString();
            order.Status = "已完成";
            order.Assessment_ID = assessment.Id;
            order_BLL.m_Rep.Update(order);
            FIX_OrderHistoryModel orderHistory = new FIX_OrderHistoryModel();
            orderHistory.CreatePerson = order.UpdatePerson;
            orderHistory.CreateTime = DateTime.Now.ToShortDateString();
            orderHistory.Order_ID = order.Id;
            orderHistory.Status = order.Status;
            orderHistory.UpdateTime = DateTime.Now.ToShortDateString();
            orderHistory_BLL.m_Rep.Create(orderHistory);
            response.Data = true;
            return toJson(response);
        }
    }
}