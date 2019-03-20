using NFine.Data;
using NFine.Domain.Entity.AuxiliaryManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain.IRepository.AuxiliaryManage
{
    public interface IMessageRepository : IRepositoryBase<MessageEntity>
    {
        void DeleteForm(string keyValue);

        void SubmitForm(MessageEntity messageEntity, string keyValue);
    }
}
