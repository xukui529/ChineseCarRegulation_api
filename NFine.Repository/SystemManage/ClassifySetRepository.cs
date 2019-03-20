using NFine.Data;
using NFine.Domain.Entity.SystemManage;
using NFine.Domain.IRepository.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Repository.SystemManage
{
	public class ClassifySetRepository : RepositoryBase<ClassifySetEntity>, IClassifySetRepository
	{
		public void SubmitForm(ClassifySetEntity entity, string keyValue)
		{
			using (var db = new RepositoryBase().BeginTrans())
			{
				if (!string.IsNullOrEmpty(keyValue))
				{
					entity.Modify(keyValue);
					db.Update(entity);
				}
				else
				{
					entity.Create();
					db.Insert(entity);
				}
				db.Commit();
			}
		}
	}
}
