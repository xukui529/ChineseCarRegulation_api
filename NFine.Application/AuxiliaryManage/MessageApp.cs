using NFine.Code;
using NFine.Domain.Entity.AuxiliaryManage;
using NFine.Domain.IRepository.AuxiliaryManage;
using NFine.Repository.AuxiliaryManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Application.AuxiliaryManage
{
	public class MessageApp
	{
		private IMessageRepository messageService = new MessageRepository();
		// 根据标题查询数据列表
		public List<MessageEntity> GetDataList(Pagination pagination, string keyword)
		{
			var expression = ExtLinq.True<MessageEntity>();
			if (!string.IsNullOrEmpty(keyword))
			{
				expression = expression.And(t => t.F_Type == keyword);
			}
			return messageService.FindList(expression, pagination);
		}

        public List<MessageEntity> GetDataList(int page, int rows, string keyword, out int total)
        {
            List<MessageEntity> messageList = new List<MessageEntity>();

            messageList = messageService.IQueryable().ToList();
         
            var list = (from model in messageList
                      
                        where model.F_Type== keyword
                        where model.F_DeleteMark == false
                    
                        orderby model.F_CreatorTime descending
                        select new MessageEntity()
                        {
                            F_Id = model.F_Id,
                            F_Title = model.F_Title,
                            F_Email = model.F_Email,
                            F_Type = model.F_Type,
                            F_Memo = model.F_Memo,
                            F_UserId = model.F_UserId,  
                              
                            F_State = model.F_State, 
                            F_CreatorUserId = model.F_CreatorUserId,
                            F_CreatorTime = model.F_CreatorTime,
                            F_DeleteMark = model.F_DeleteMark,
                            F_DeleteUserId = model.F_DeleteUserId,
                            F_DeleteTime = model.F_DeleteTime,
                            F_LastModifyUserId = model.F_LastModifyUserId,
                            F_LastModifyTime = model.F_LastModifyTime
                        }).ToList();
            total = list.Count();
            list = list.Skip((page - 1) * rows).Take(rows).AsQueryable().ToList();
            return list;
        }


        // 根据主键ID查询数据对象
        public MessageEntity GetForm(string keyValue)
		{
			return messageService.FindEntity(keyValue);
		}

		// 根据主键删除
		public void DeleteForm(string keyValue)
		{
			messageService.DeleteForm(keyValue);
		}

		// 新建提交
		public void SubmitForm(MessageEntity messageEntity, string keyValue)
		{
			messageService.SubmitForm(messageEntity, keyValue);
		}

		// 更改状态
		public void UpdateStateForm(string keyValue)
		{
			MessageEntity messageEntity = new MessageEntity();
			messageEntity = messageService.FindEntity(keyValue);
			messageEntity.F_State = true;
			messageService.SubmitForm(messageEntity, keyValue);
		}
	}
}
