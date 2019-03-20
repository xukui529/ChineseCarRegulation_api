using NFine.Data;
using NFine.Domain.Entity.AuxiliaryManage;
using NFine.Domain.IRepository.AuxiliaryManage;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Repository.AuxiliaryManage
{
    public class MemberDownloadRepository : RepositoryBase<MemberDownloadEntity>, IMemberDownloadRepository
    {
        public void SubmitForm(MemberDownloadEntity entity, string keyValue)
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
        public MemberDownloadEntity GetFormEntity()
        {
            MemberDownloadEntity entity = new MemberDownloadEntity();
            List<MemberDownloadEntity> list = new List<MemberDownloadEntity>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT * FROM  Auxiliary_MemberDownload WHERE 1 = 1  AND F_DeleteMark = 0");
            list= this.FindList(strSql.ToString());
            if (list.Count() > 0)
            {
                entity = list[0];
            }
            return entity;
        }


    }
}
