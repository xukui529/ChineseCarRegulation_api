using NFine.Data;
using NFine.Domain.Entity.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain.IRepository.SystemManage
{
    public interface IMemberRepository : IRepositoryBase<MemberEntity>
    {
        void DeleteForm(string keyValue);
        void SubmitForm(MemberEntity entity, string keyValue);

        void UpdateState(string keyValue);
    }
}
