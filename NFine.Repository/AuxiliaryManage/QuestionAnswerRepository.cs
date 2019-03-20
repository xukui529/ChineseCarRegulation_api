using NFine.Code;
using NFine.Data;
using NFine.Domain.Entity.AuxiliaryManage;
using NFine.Domain.IRepository.AuxiliaryManage;
using NFine.Domain.ViewModel;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace NFine.Repository.AuxiliaryManage
{
	public class QuestionAnswerRepository : RepositoryBase<QuestionAnswerEntity>, IQuestionAnswerRepository
	{
		//public List<QuestionAnswerModel> GetItemList(Pagination pagination, string keyword)
		//{
		//	List<QuestionAnswerModel> list = new List<QuestionAnswerModel>();
		//	StringBuilder strSql = new StringBuilder();
		//	strSql.Append(@"SELECT  d.*
		//                          FROM    NFine_Sys_ItemsDetail d
		//                                  INNER  JOIN NFine_Sys_Items i ON i.F_Id = d.F_ItemId
		//                          WHERE   1 = 1
		//                                  AND i.F_EnCode = @enCode
		//                                  AND d.F_EnabledMark = 1
		//                                  AND d.F_DeleteMark = 0
		//                          ORDER BY d.F_SortCode ASC");
		//	DbParameter[] parameter =
		//	{
		//		 new SqlParameter("@enCode",keyword)
		//	};
		//	list = FindList(strSql.ToString(), parameter);
		//}
		public void DeleteForm(string keyValue)
		{
			using (var db = new RepositoryBase().BeginTrans())
			{
				db.Delete<QuestionAnswerEntity>(t => t.F_Id == keyValue);
				db.Commit();
			}
		}

		public void SubmitForm(QuestionAnswerEntity entity, string keyValue)
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
	}
}
