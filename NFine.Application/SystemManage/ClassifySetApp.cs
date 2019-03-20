using NFine.Code;
using NFine.Data;
using NFine.Domain.Entity.SystemManage;
using NFine.Domain.IRepository.SystemManage;
using NFine.Repository.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Application.SystemManage
{
	public class ClassifySetApp
	{
		private IClassifySetRepository service = new ClassifySetRepository();

		public List<ClassifySetEntity> GetDataList(Pagination pagination, string keyword)
		{
			var expression = ExtLinq.True<ClassifySetEntity>();
			//if (!string.IsNullOrEmpty(keyword))
			//{
				//expression = expression.And(t => t.F_ChineseName.Contains(keyword));
			//}
			return service.FindList(expression, pagination);
		}
     
        public bool CheckName(string name)
		{
			string sql = String.Format("select name from Sys_ClassifySet where name={0}", name);
			var expression = ExtLinq.True<ClassifySetEntity>();
			expression = expression.And(t => t.Name==name.Trim());
			List<ClassifySetEntity> list = service.FindList(sql);
			if(list.Count() > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public ClassifySetEntity GetForm(string keyValue)
		{
			return service.FindEntity(keyValue);
		}

		public void SubmitForm(ClassifySetEntity entity, string keyValue)
		{
			service.SubmitForm(entity, keyValue);
		}
	}
}
