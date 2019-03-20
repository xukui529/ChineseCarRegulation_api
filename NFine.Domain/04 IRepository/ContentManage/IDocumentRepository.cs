using NFine.Data;
using NFine.Domain.Entity.ContentManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain.IRepository.ContentManage
{
	public interface IDocumentRepository: IRepositoryBase<DocumentEntity>
	{
		void DeleteForm(string keyValue);

		void SubmitForm(DocumentEntity entity, string keyValue);

		void UpdateState(string keyValue, int state);
	}
}
