
using NFine.Code;
using System.Linq;
using System.Collections.Generic;
using NFine.Repository.AuxiliaryManage;
using NFine.Domain.Entity.AuxiliaryManage;
using NFine.Domain.IRepository.AuxiliaryManage;
namespace NFine.Application.AuxiliaryManage
{
    /// <summary>
    /// SysMessageInfoApp
    /// </summary>	
    public class SysMessageInfoApp
    {
        private ISysMessageInfoRepository service = new SysMessageInfoRepository();
        private SysMessageInfoRepository serviceR = new SysMessageInfoRepository();

        public List<SysMessageInfoEntity> GetList(Pagination pagination, string keyword)
        {
            return serviceR.FindList(pagination, keyword);
        }

        public List<SysMessageInfoEntity> GetList()
        {
            return service.IQueryable().OrderByDescending(x => x.F_CreatorTime).ToList();
        }

        public SysMessageInfoEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }

        public void Delete(SysMessageInfoEntity entity)
        {
            service.Delete(entity);
        }

        public void DeleteForm(string keyValue)
        {
            service.DeleteForm(keyValue);
        }

        public void RemoveForm(SysMessageInfoEntity entity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                service.Update(entity);
            }
        }
        public void SubmitForm(SysMessageInfoEntity entity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                service.Update(entity);
            }
            else
            {
                entity.F_DeleteMark = false;
                entity.Create();
                service.Insert(entity);
            }
        }
        public List<SysMessageInfoEntity> GetDataList(int page, int rows, string userid,out int total )
        {
            List<SysMessageInfoEntity> sysMessageInfoEntity = new List<SysMessageInfoEntity>();

            sysMessageInfoEntity = service.IQueryable().ToList();
          
            var list = (from table1 in sysMessageInfoEntity 
                        where table1.F_UserId == userid
                        where table1.F_DeleteMark == false 
                        orderby table1.F_CreatorTime descending
                        select new SysMessageInfoEntity()
                        {
                            F_Id = table1.F_Id,
                            F_UserId = table1.F_UserId, 
                            F_EnTitle = table1.F_EnTitle,
                            F_ChTitle = table1.F_ChTitle,
                            F_CreatorUserId = table1.F_CreatorUserId,
                            F_CreatorTime = table1.F_CreatorTime,
                            F_DeleteMark = table1.F_DeleteMark,
                            F_DeleteUserId = table1.F_DeleteUserId,
                            F_DeleteTime = table1.F_DeleteTime,
                            F_LastModifyUserId = table1.F_LastModifyUserId,
                            F_LastModifyTime = table1.F_LastModifyTime,
                        }).ToList();
            total = list.Count();
            list = list.Skip((page-1)*rows).Take(rows).AsQueryable().ToList();
            return list;
        }
    }
}