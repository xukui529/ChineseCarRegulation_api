using NFine.Data;
using NFine.Domain.Entity.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain.IRepository.SystemManage
{
	public interface IClassifySetRepository : IRepositoryBase<ClassifySetEntity>
	{
		void SubmitForm(ClassifySetEntity entity, string keyValue);
	}
}
