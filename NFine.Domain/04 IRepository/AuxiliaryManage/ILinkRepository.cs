using NFine.Data;
using NFine.Domain.Entity.AuxiliaryManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain.IRepository.AuxiliaryManage
{
    public interface ILinkRepository : IRepositoryBase<LinkEntity>
    {
        void DeleteForm(string keyValue);

        void SubmitForm(LinkEntity linkEntity, string keyValue);
    }
}
