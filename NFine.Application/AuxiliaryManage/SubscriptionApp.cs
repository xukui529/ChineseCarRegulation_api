
using NFine.Code;
using System.Linq;
using System.Collections.Generic;
using NFine.Repository.AuxiliaryManage;
using NFine.Domain.Entity.AuxiliaryManage;
using NFine.Domain.IRepository.AuxiliaryManage;
using System;

namespace NFine.Application.AuxiliaryManage
{
    /// <summary>
    /// SubscriptionApp
    /// </summary>	
    public class SubscriptionApp
    {
        private ISubscriptionRepository service = new SubscriptionRepository();
        private SubscriptionRepository serviceR = new SubscriptionRepository();

        /// <summary>
        /// 获取本期数据  上周数据 没有就新建一个
        /// </summary>
        /// <param name="now">今天的日期</param>
        /// <returns></returns>
        public SubscriptionEntity GetSubDataByTime(DateTime beginTime, DateTime endTime,DateTime time)
        { 
            SubscriptionEntity subscriptionEntity;
            if (!service.IQueryable().Any(x=>x.F_BeginTime<= time && x.F_EndTime >= time))
            {
                //本期没数据，新增一条
                subscriptionEntity = new SubscriptionEntity
                {
                    F_IsSend = false,
                    F_SendNum = GetMaxSendNumList() + 1,
                    F_ExcludeDocIds = "",
                    F_BeginTime = beginTime,
                    F_EndTime = endTime
                };
                SubmitForm(subscriptionEntity, "");
            }
            return service.IQueryable().Where(x => x.F_BeginTime <= time && x.F_EndTime >= time).FirstOrDefault();
        }

        public List<SubscriptionEntity> GetList(Pagination pagination, string keyword)
        {
            return serviceR.FindList(pagination, keyword);
        }
        public int GetMaxSendNumList()
        {
            int maxSendNum = 0;
            if(service.IQueryable().OrderByDescending(x => x.F_SendNum).Any())
            {
                maxSendNum = (int)service.IQueryable().OrderByDescending(x => x.F_SendNum).FirstOrDefault().F_SendNum;
            }
            return maxSendNum;
        }


        public List<SubscriptionEntity> GetList()
        {
            return service.IQueryable().OrderByDescending(x => x.F_CreatorTime).ToList();
        }

        public SubscriptionEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }

        public void Delete(SubscriptionEntity entity)
        {
            service.Delete(entity);
        }

        public void DeleteForm(string keyValue)
        {
            service.DeleteForm(keyValue);
        }

        public void RemoveForm(SubscriptionEntity entity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                service.Update(entity);
            }
        }
        public void SubmitForm(SubscriptionEntity entity, string keyValue)
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

    }
}