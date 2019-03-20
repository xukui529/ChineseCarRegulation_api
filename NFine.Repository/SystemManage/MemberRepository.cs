using NFine.Data;
using NFine.Domain.Entity.SystemManage;
using NFine.Domain.IRepository.SystemManage;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Repository.SystemManage
{
    public class MemberRepository : RepositoryBase<MemberEntity>, IMemberRepository
    {
        public void DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                db.Delete<MemberEntity>(t => t.F_Id == keyValue);
                db.Commit();
            }
        }
        /// <summary>
        ///  新增/编辑 会员
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="keyValue"></param>
        public void SubmitForm(MemberEntity entity, string keyValue)
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

        public void UpdateState(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                MemberEntity entity = new MemberEntity();
                entity = db.FindEntity<MemberEntity>(keyValue);
                entity.F_State = 3;
                db.Update(entity);
                db.Commit();
            }
        }
    }
}
