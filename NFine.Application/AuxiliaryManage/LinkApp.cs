using NFine.Code;
using NFine.Domain.Entity.AuxiliaryManage;
using NFine.Domain.IRepository.AuxiliaryManage;
using NFine.Domain.ViewModel;
using NFine.Repository.AuxiliaryManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Application.AuxiliaryManage
{
    public class LinkApp
    {
        private ILinkRepository linkService = new LinkRepository();
        // 根据标题查询数据列表
        public List<LinkEntity> GetDataList(Pagination pagination, string keyword)
        {
            var expression = ExtLinq.True<LinkEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.F_ChineseName.Contains(keyword));
				expression = expression.Or(t => t.F_EnglishName.Contains(keyword));
			}
            return linkService.FindList(expression, pagination);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="keyword"></param> 
        /// <returns></returns>
        public List<LinkEntity> GetDataList(int page, int rows, string keyword, out int total)
        {
            List<LinkEntity> linkList = new List<LinkEntity>();
            linkList = linkService.IQueryable().ToList(); 
            var list = (from link in linkList
                    
                        where link.F_ChineseName.Contains(keyword) || link.F_EnglishName.Contains(keyword)
                        where link.F_DeleteMark == false 
                        orderby link.F_CreatorTime descending
                        select new LinkEntity()
                        {
                            F_Id = link.F_Id,
                            F_ChineseName = link.F_ChineseName,
                            F_EnglishName = link.F_EnglishName,
                            F_Type = link.F_Type,
                            F_Url = link.F_Url,
                            F_Memo = link.F_Memo, 
                            F_CreatorUserId = link.F_CreatorUserId,
                            F_CreatorTime = link.F_CreatorTime,
                            F_DeleteMark = link.F_DeleteMark,
                            F_DeleteUserId = link.F_DeleteUserId,
                            F_DeleteTime = link.F_DeleteTime,
                            F_LastModifyUserId = link.F_LastModifyUserId,
                            F_LastModifyTime = link.F_LastModifyTime
                        }).ToList();
            total = list.Count();
            list = list.Skip((page - 1) * rows).Take(rows).AsQueryable().ToList();
            return list;
        }
        public List<LinkEntity> GetList()
        {
            return linkService.IQueryable().OrderByDescending(x => x.F_CreatorTime).ToList();
        }
         
        // 根据主键ID查询数据对象
        public LinkEntity GetForm(string keyValue)
        {
            return linkService.FindEntity(keyValue);
        }

        // 根据主键删除
        public void DeleteForm(string keyValue)
        {
            linkService.DeleteForm(keyValue);
        }

        // 新建提交
        public void SubmitForm(LinkEntity linkEntity, string keyValue)
        {
            linkService.SubmitForm(linkEntity, keyValue);
        }
    }
}
