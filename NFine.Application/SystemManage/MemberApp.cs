using NFine.Code;
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
    public class MemberApp
    {
        private IMemberRepository service = new MemberRepository();
        public List<MemberEntity> GetList(Pagination pagination, string keyword, int type)
        {
            var expression = ExtLinq.True<MemberEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.F_Email.Contains(keyword));
                expression = expression.Or(t => t.F_Phone.Contains(keyword));
            }
            if (type ==1)
            {
                expression = expression.And(t => t.F_State == 1);
            }
            if (type == 2)
            {
                expression = expression.And(t => t.F_State == 2);
            }
            if (type == 3)
            {
                expression = expression.And(t => t.F_State == 3);
            }
            if (type == 4)
            {
                expression = expression.And(t => t.F_State == 4);
                expression = expression.And(t => t.F_UseState == 4);
            }
            return service.FindList(expression, pagination);
        }
        public List<MemberEntity> GetDataList(int page, int rows, string keyword, int type, out int total)
        {
            List<MemberEntity> modelList = new List<MemberEntity>();
            modelList = service.IQueryable().ToList();
            var list = (from model in modelList

                        where model.F_Email.Contains(keyword) || model.F_Phone.Contains(keyword)
                        where model.F_State == type
                        where (type==4)?model.F_UseState==4 : model.F_State == type
                        where model.F_DeleteMark == false
                        orderby model.F_CreatorTime descending
                        select new MemberEntity()
                        {
                            F_Id = model.F_Id,
                            F_HYType = model.F_HYType,
                            F_Email = model.F_Email,
                            F_Password = model.F_Password,
                            F_Phone = model.F_Phone,
                            F_Account = model.F_Account,
                            F_StartDate = model.F_StartDate,
                            F_EndDate = model.F_EndDate,
                            F_TimesState = model.F_TimesState,
                            F_UseState = model.F_UseState,
                            F_DownAndPrintCount = model.F_DownAndPrintCount,
                            F_State = model.F_State,
                            F_IsSubscription = model.F_IsSubscription,
                            F_SubscriptionLanguage = model.F_SubscriptionLanguage,
                            F_IsDeputy = model.F_IsDeputy, 
                            F_SuperUId = model.F_SuperUId,
                            F_SecretKey = model.F_SecretKey,
                            F_SecretKeyPastDue = model.F_SecretKeyPastDue,
                            F_Country = model.F_Country,  
                            F_CreatorUserId = model.F_CreatorUserId,
                            F_CreatorTime = model.F_CreatorTime,
                            F_DeleteMark = model.F_DeleteMark,
                            F_DeleteUserId = model.F_DeleteUserId,
                            F_DeleteTime = model.F_DeleteTime,
                            F_LastModifyUserId = model.F_LastModifyUserId,
                            F_LastModifyTime = model.F_LastModifyTime
                        }).ToList();
            total = list.Count();
            list = list.Skip((page - 1) * rows).Take(rows).AsQueryable().ToList();
            return list;
        }
        public MemberEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }

        public void SubmitForm(MemberEntity entity, string keyValue)
        {
            service.SubmitForm(entity, keyValue);
        }

        public void UpdateState(string keyValue)
        {
            service.UpdateState(keyValue);
        }
        public MemberEntity GetFormByEmail(string email)
        {
            return service.IQueryable().Where(x => x.F_Email == email && x.F_DeleteMark == false).FirstOrDefault();
        }

        public bool CheckUserByEmail(string email)
        {
            return service.IQueryable().Any(x => x.F_Email == email && x.F_DeleteMark == false);
        }

        public int GetDeputyAllCount(string superUId)
        {
            return service.IQueryable().Where(x => x.F_SuperUId == superUId && x.F_DeleteMark == false).Count();
        }
        public void DeleteForm(string keyValue)
        {
            service.DeleteForm(keyValue);
        }
        public List<MemberEntity> GetListSubscription()
        {
            return service.IQueryable().Where(x => x.F_IsSubscription == true && x.F_DeleteMark == false).OrderByDescending(x => x.F_CreatorTime).ToList();
        }

        public List<MemberEntity> GetDeputyMemberListByUserId(string userid)
        {
            return service.IQueryable().Where(x => x.F_IsDeputy == true && x.F_SuperUId == userid && x.F_DeleteMark==false).OrderByDescending(x => x.F_CreatorTime).ToList();
        }
    }
}
