
using NFine.Code;
using System.Linq;
using System.Collections.Generic;
using NFine.Repository.AuxiliaryManage;
using NFine.Domain.Entity.AuxiliaryManage;
using NFine.Domain.IRepository.AuxiliaryManage;
using NFine.Domain.ViewModel;
using NFine.Domain.Entity.SystemManage;
using NFine.Repository.SystemManage;

namespace NFine.Application.AuxiliaryManage
{
    /// <summary>
    /// MemberUpgradeApplyApp
    /// </summary>	
    public class MemberUpgradeApplyApp
    {
        private IMemberUpgradeApplyRepository service = new MemberUpgradeApplyRepository();
        private MemberUpgradeApplyRepository serviceR = new MemberUpgradeApplyRepository();
        private MemberRepository memberService = new MemberRepository();
        public List<MemberUpgradeApplyEntity> GetList(Pagination pagination, string keyword, int state)
        {
            var expression = ExtLinq.True<MemberUpgradeApplyEntity>();
            if (!string.IsNullOrEmpty(keyword))
            { 
                expression = expression.Or(t => t.F_Phone.Contains(keyword));
            }
            if (state == 1)
            {
                expression = expression.And(t => t.F_Status == 1);
            }
            if (state == 2)
            {
                expression = expression.And(t => t.F_Status == 2);
            } 
            return service.FindList(expression, pagination);
        }
        public List<MemberUpgradeApplyEntity> GetList(Pagination pagination, string keyword)
        {
            return serviceR.FindList(pagination, keyword);
        }

        public List<MemberUpgradeApplyEntity> GetList()
        {
            return service.IQueryable().OrderByDescending(x => x.F_CreatorTime).ToList();
        }

        public MemberUpgradeApplyEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }

        public void Delete(MemberUpgradeApplyEntity entity)
        {
            service.Delete(entity);
        }

        public void DeleteForm(string keyValue)
        {
            service.DeleteForm(keyValue);
        }

        public void RemoveForm(MemberUpgradeApplyEntity entity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                service.Update(entity);
            }
        }
        public void SubmitForm(MemberUpgradeApplyEntity entity, string keyValue)
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

        public void UpdateState(string keyValue, int status)
        {
            service.UpdateState(keyValue,   status);
        }

        public List<MemberUpgradeApplyModel> GetDataList(int page, int rows,  out int total, string keyword, int status)
        {
            List<MemberUpgradeApplyEntity> memberUpgradeApplyList = new List<MemberUpgradeApplyEntity>();
            List<MemberEntity> memberList = new List<MemberEntity>();
            memberUpgradeApplyList = service.IQueryable().ToList();
            memberList = memberService.IQueryable().ToList();
            var list = (from table1 in memberUpgradeApplyList
                        join member in memberList on table1.F_MemberId equals member.F_Id
                        where table1.F_Status== status
                        where (keyword == "" ? true : (table1.F_Phone.Contains(keyword) || member.F_Email.Contains(keyword))) 
                        where member.F_DeleteMark == false 
                        where table1.F_DeleteMark == false
                        orderby table1.F_CreatorTime descending
                        select new MemberUpgradeApplyModel()
                        {
                            F_Id = table1.F_Id,
                            F_MemberId = table1.F_MemberId,
                             F_Email = member.F_Email,
                            F_Country = table1.F_Country,
                            F_Phone = table1.F_Phone,
                            F_Account = table1.F_Account,
                            F_HYType = table1.F_HYType,
                            F_OpenUpTime = table1.F_OpenUpTime,
                            F_Remark = table1.F_Remark,
                            F_Status = table1.F_Status,
                            F_CreatorUserId = table1.F_CreatorUserId,
                            F_CreatorTime = table1.F_CreatorTime,
                            F_DeleteMark = table1.F_DeleteMark,
                            F_DeleteUserId = table1.F_DeleteUserId,
                            F_DeleteTime = table1.F_DeleteTime,
                            F_LastModifyUserId = table1.F_LastModifyUserId,
                            F_LastModifyTime = table1.F_LastModifyTime,

 
                        }).ToList();
            total = list.Count();
            list = list.Skip((page - 1) * rows).Take(rows).AsQueryable().ToList();
            return list;
        }

        public List<MemberUpgradeApplyModel> GetDataList(int page, int rows, out int total, string keyword)
        {
            List<MemberUpgradeApplyEntity> memberUpgradeApplyList = new List<MemberUpgradeApplyEntity>();
            List<MemberEntity> memberList = new List<MemberEntity>();
            memberUpgradeApplyList = service.IQueryable().ToList();
            memberList = memberService.IQueryable().ToList();
            var list = (from table1 in memberUpgradeApplyList
                        join member in memberList on table1.F_MemberId equals member.F_Id
                        where (keyword == "" ? true : (table1.F_Phone.Contains(keyword) || member.F_Email.Contains(keyword)))
                        where member.F_DeleteMark == false
                        where table1.F_DeleteMark == false
                        orderby table1.F_CreatorTime descending
                        select new MemberUpgradeApplyModel()
                        {
                            F_Id = table1.F_Id,
                            F_MemberId = table1.F_MemberId,
                            F_Email = member.F_Email,
                            F_Country = table1.F_Country,
                            F_Phone = table1.F_Phone,
                            F_Account = table1.F_Account,
                            F_HYType = table1.F_HYType,
                            F_OpenUpTime = table1.F_OpenUpTime,
                            F_Remark = table1.F_Remark,
                            F_Status = table1.F_Status,
                            F_CreatorUserId = table1.F_CreatorUserId,
                            F_CreatorTime = table1.F_CreatorTime,
                            F_DeleteMark = table1.F_DeleteMark,
                            F_DeleteUserId = table1.F_DeleteUserId,
                            F_DeleteTime = table1.F_DeleteTime,
                            F_LastModifyUserId = table1.F_LastModifyUserId,
                            F_LastModifyTime = table1.F_LastModifyTime,


                        }).ToList();
            total = list.Count();
            list = list.Skip((page - 1) * rows).Take(rows).AsQueryable().ToList();
            return list;
        }
    }
}