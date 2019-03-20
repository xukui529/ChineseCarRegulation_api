using NFine.Data;
using NFine.Domain.Entity.SystemManage;
using NFine.Domain.IRepository.SystemManage;
using NFine.Domain.ViewModel;
using NFine.Repository.SystemManage;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using System.Linq;

namespace NFine.Repository.SystemManage
{
    public class ItemsDetailRepository : RepositoryBase<ItemsDetailEntity>, IItemsDetailRepository
    {
        public List<ItemsDetailEntity> GetItemList(string enCode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  d.*
                            FROM    NFine_Sys_ItemsDetail d
                                    INNER  JOIN NFine_Sys_Items i ON i.F_Id = d.F_ItemId
                            WHERE   1 = 1
                                    AND i.F_EnCode = @enCode
                                    AND d.F_EnabledMark = 1
                                    AND d.F_DeleteMark = 0
                            ORDER BY d.F_SortCode ASC");
            DbParameter[] parameter =
            {
                 new SqlParameter("@enCode",enCode)
            };
            return this.FindList(strSql.ToString(), parameter);
        }
        public List<ItemsDetailEntity> GetItemAll()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  d.*
                            FROM    NFine_Sys_ItemsDetail d
                                    INNER  JOIN NFine_Sys_Items i ON i.F_Id = d.F_ItemId
                            WHERE   1 = 1
                                    AND d.F_EnabledMark = 1
                                    AND d.F_DeleteMark = 0
                            ORDER BY d.F_SortCode ASC");
            return this.FindList(strSql.ToString());
        }
         
        public List<ItemAndItemDetailModel> GetItemAndItemDetailAll()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT d.F_Id Id,d.F_ItemName ItemName,d.F_ItemEnName ItemEnName, d.F_ItemCode ItemCode ,d.F_ItemId ItemId,i.F_EnCode EnCode
                            FROM    NFine_Sys_ItemsDetail d
                                    INNER  JOIN NFine_Sys_Items i ON i.F_Id = d.F_ItemId
                            WHERE   1 = 1
                                    AND d.F_EnabledMark = 1
                                    AND d.F_DeleteMark = 0
                            ORDER BY d.F_SortCode ASC");

            return dbcontext.Database.SqlQuery<ItemAndItemDetailModel>(strSql.ToString()).ToList(); 
        }
    }
}