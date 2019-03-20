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
    public class MemberDownloadApp
    {
        private IMemberDownloadRepository service = new MemberDownloadRepository();
        public void SubmitForm(MemberDownloadEntity entity, string keyValue)
        {
            service.SubmitForm(entity, keyValue);
        }

        public MemberDownloadEntity GetFormEntity()
        {
            return service.GetFormEntity();
        }
    }
}
