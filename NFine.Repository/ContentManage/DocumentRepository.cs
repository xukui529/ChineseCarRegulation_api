using NFine.Data;
using NFine.Domain.Entity.ContentManage;
using NFine.Domain.IRepository.ContentManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Repository.ContentManage
{
	public class DocumentRepository : RepositoryBase<DocumentEntity>, IDocumentRepository
	{
		public void DeleteForm(string keyValue)
		{
			using (var db = new RepositoryBase().BeginTrans())
			{
				db.Delete<DocumentEntity>(t => t.F_Id == keyValue);
				db.Commit();
			}
		}

		public void SubmitForm(DocumentEntity entity, string keyValue)
		{
			using (var db = new RepositoryBase().BeginTrans())
			{
				if (string.IsNullOrEmpty(keyValue))
                {
                    entity.F_State = 0;
                    entity.Create();
					entity.F_DeleteMark = false;
					entity.F_LastModifyTime = DateTime.Now;
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

		public void UpdateState(string keyValue,int state)
		{
			using (var db = new RepositoryBase().BeginTrans())
			{
				DocumentEntity entity = new DocumentEntity();
				entity = db.FindEntity<DocumentEntity>(keyValue);
				entity.F_State = state;
				db.Update(entity);
				db.Commit();
			}
		}
	}
}
