using NFine.Data;
using NFine.Domain.Entity.AuxiliaryManage;
using NFine.Domain.IRepository.AuxiliaryManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Repository.AuxiliaryManage
{
    public class MessageRepository : RepositoryBase<MessageEntity>, IMessageRepository
    {
        public void DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                db.Delete<MessageEntity>(t => t.F_Id == keyValue);
                db.Commit();
            }
        }

        public void SubmitForm(MessageEntity entity, string keyValue)
        {
			using (var db = new RepositoryBase().BeginTrans())
			{
				if (string.IsNullOrEmpty(keyValue))
				{
					entity.Create();
					db.Insert(entity);
				}
				else
				{
					entity.Modify(keyValue);
					db.Update(entity);
				}
				db.Commit();
			}
        }
    }
}
