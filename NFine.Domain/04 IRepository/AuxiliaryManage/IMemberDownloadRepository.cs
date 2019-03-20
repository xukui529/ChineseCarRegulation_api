using NFine.Data;
using NFine.Domain.Entity.AuxiliaryManage;
using NFine.Domain.Entity.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain.IRepository.AuxiliaryManage
{
    public interface IMemberDownloadRepository : IRepositoryBase<MemberDownloadEntity>
    {
        void SubmitForm(MemberDownloadEntity entity, string keyValue);

        MemberDownloadEntity GetFormEntity();
    }
}
