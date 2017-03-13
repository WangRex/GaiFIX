using System;
using System.Collections.Generic;
using System.Linq;
using Apps.Common;
using System.Data;
using Apps.Models;
using Apps.Models.MIS;
using System.Data.Entity.Core.Objects;

namespace Apps.DAL.MIS
{
    public partial class MIS_WebIM_MessageRepository : BaseRepository<MIS_WebIM_Message>
    {

        /// <summary>
        /// 新增消息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="message"></param>
        /// <param name="sender"></param>
        /// <param name="receiver"></param>
        public int CreateMessage(string message, string sender, string receiver,string receiverTitle)
        {
            MIS_WebIM_Message creatMsg = new MIS_WebIM_Message();
            creatMsg.Message = message;
            creatMsg.Sender = sender;
            creatMsg.receiver = receiver;
            creatMsg.receiverTitle = receiverTitle;
            base.Create(creatMsg);
            return creatMsg.Id;
        }

        /// <summary>
        /// 删除发送者的一个消息(物理删除)
        /// </summary>
        /// <param name="id"></param>
        public void DeleteMessageBySender(string id)
        {
            MIS_WebIM_Message delMsg = base.Find(Convert.ToInt32(id));
            base.Delete(delMsg);
        }

        /// <summary>
        /// 删除发送者的所有消息（物理删除）
        /// </summary>
        /// <param name="sender"></param>
        public void DeleteMessageAllBySender(string sender)
        {
            List<MIS_WebIM_Message> delMsg = base.FindList(a => a.Sender == sender).ToList();
            base.Delete(delMsg);
        }
        /// <summary>
        /// 删除发送者的多个消息(非物理删除)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="id"></param>
        public void DeleteMessageNotBySender(string sender, string[] ids)
        {
            using (DbContexts db = new DbContexts())
            {
                IQueryable< MIS_WebIM_Message> entityList=from r in db.MIS_WebIM_Message
                           where ids.Contains(r.Id.ToString())
                           where r.Sender==sender
                           select r;
                if (entityList == null)
                {
                    return;
                }
                foreach (var entity in entityList)
                {
                    entity.State = "true";
                }
                db.SaveChanges();

            }
        }
        /// <summary>
        /// 删除发送者的全部消息(非物理删除)
        /// </summary>
        /// <param name="sender"></param>
        public void DeleteMessageAllNotBySender(string sender)
        {
            using (DbContexts db = new DbContexts())
            {
                IQueryable<MIS_WebIM_Message> entityList = from r in db.MIS_WebIM_Message
                                                     where r.Sender==sender
                                                     select r;
                if (entityList == null)
                {
                    return;
                }
                foreach (var entity in entityList)
                {
                    entity.State = "true";
                }
                db.SaveChanges();

            }
        }
        /// <summary>
        /// 删除接收者的一个消息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="?"></param>
        public void DeleteMessageByReceiver(string id, string receiver)
        {
            base.Delete(Convert.ToInt32(id));
        }
        /// <summary>
        /// 删除接收者的所有消息
        /// </summary>
        /// <param name="receiver"></param>
        public void DeleteMessageAllByReceiver(string receiver)
        {
            List<MIS_WebIM_Message> delMsg = base.FindList(a => a.receiver == receiver).ToList();
            base.Delete(delMsg);
        }
        /// <summary>
        /// 设置接收者的所有未阅读消息为已阅
        /// </summary>
        /// <param name="receiver"></param>
        public void SetMessageHasReadByReceiver(string receiver)
        {
            List<MIS_WebIM_Message> Msgs = base.FindList(a => a.receiver == receiver).ToList();
            foreach( MIS_WebIM_Message msg in Msgs)
            {
                msg.State = "已读";
                base.Update(msg);
            }
     
        }
        public void SetMessageHasReadFromSenderToReceiver(string sender,string receiver)
        {
            List<MIS_WebIM_Message> Msgs = base.FindList(a => a.receiver == receiver && a.Sender == sender).ToList();
            foreach (MIS_WebIM_Message msg in Msgs)
            {
                msg.State = "已读";
                base.Update(msg);
            }
        }
        /// <summary>
        /// 设置接收者的一条未阅读消息为已阅
        /// </summary>
        /// <param name="receiver"></param>
        /// <param name="id"></param>
        public void SetMessageHasReadById(string receiver, string id)
        {
            MIS_WebIM_Message Msg = base.Find(Convert.ToInt32(id));
            Msg.State = "已读";
            base.Update(Msg);
        }
        /// <summary>
        /// 返回发送信息给当前用户的发送者列表及未阅读信息数
        /// </summary>
        /// <param name="receiver"></param>
        /// <returns></returns>
        public List<MIS_WebIM_SenderModel> GetSenderByReceiver(string receiver)
        {
            List<MIS_WebIM_Message> Msgs = base.FindList(a => a.State != "已读").ToList();
            var modelList = from r in Msgs
                            select new MIS_WebIM_SenderModel { 
                                    Sender=r.Sender,
                                    SenderTitle=r.SenderTitle,
                                    MessageCount= Msgs.Where(a => a.Sender == r.Sender).Count()
                            };
                return modelList.ToList();
        }
        /// <summary>
        /// 读取一个发送消息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MIS_WebIM_Message GetMessageBySender(string id)
        {
            return base.Find(Convert.ToInt32(id));
        }

        /// <summary>
        /// 发送者发送的全部消息
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public IQueryable<MIS_WebIM_Message> GetMesasgeAllBySender()
        {
            IQueryable<MIS_WebIM_Message> list = base.DbContext.MIS_WebIM_Message.AsQueryable();
            return list;
        }
        /// <summary>
        /// 返回接收者一个消息
        /// </summary>
        /// <param name="receiver"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public MIS_WebIM_MessageRecModel GetMessageByReceiver(string receiver, string id)
        {
            using (DbContexts db = new DbContexts())
            {
                var model = (from m in db.MIS_WebIM_Message
                             join r in db.MIS_WebIM_Message_Rec on m.Id.ToString() equals r.MessageId
                             where m.Id.ToString() == id
                             where r.receiver == receiver
                             select new MIS_WebIM_MessageRecModel
                             {
                                 Id = m.Id,
                                 Message = m.Message,
                                 Sender = m.Sender,
                                 receiver = r.receiver,
                                 State = r.State,
                                 SendDt = m.SendDt,
                                 RecDt = r.RecDt
                             }).SingleOrDefault();
                return model;
            }
        }
       
        /// <summary>
        /// 返回接收者所有消息
        /// </summary>
        /// <param name="receiver"></param>
        /// <returns></returns>
        public IQueryable<MIS_WebIM_MessageRecModel> GetMessageAllByReceiver(string receiver)
        {
            var modelList = (from m in base.DbContext.MIS_WebIM_Message
                             join r in base.DbContext.MIS_WebIM_Message_Rec on m.Id.ToString() equals r.MessageId
                             where r.receiver == receiver
                             select new MIS_WebIM_MessageRecModel
                             {
                                 Id = m.Id,
                                 Message = m.Message,
                                 Sender = m.Sender,
                                 receiver = r.receiver,
                                 State = r.State,
                                 SendDt = m.SendDt,
                                 RecDt = r.RecDt
                             }).AsQueryable();

            return modelList;

        }
    }
}
