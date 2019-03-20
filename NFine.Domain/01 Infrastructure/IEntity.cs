using NFine.Code;
using System;

namespace NFine.Domain
{
    public class IEntity<TEntity>
    {
        public void Create()
        {
            var entity = this as ICreationAudited;
            entity.F_Id = Common.GuId();
            try
            {
                var LoginInfo = OperatorProvider.Provider.GetCurrent();
                if (LoginInfo != null)
                {
                    entity.F_CreatorUserId = LoginInfo.UserId;
                }
            }
            catch (Exception)
            {

            } 
            entity.F_CreatorTime = DateTime.Now;
        }
        public void Modify(string keyValue)
        {
            var entity = this as IModificationAudited;
            entity.F_Id = keyValue;
            try
            {
                var LoginInfo = OperatorProvider.Provider.GetCurrent();
                if (LoginInfo != null)
                {
                    entity.F_LastModifyUserId = LoginInfo.UserId;
                }
            }
            catch (Exception)
            {

            } 
            entity.F_LastModifyTime = DateTime.Now;
        }
        public void Remove()
        {
            var entity = this as IDeleteAudited;
            try
            {
                var LoginInfo = OperatorProvider.Provider.GetCurrent();
                if (LoginInfo != null)
                {
                    entity.F_DeleteUserId = LoginInfo.UserId;
                }
            }
            catch (Exception)
            {

            } 
            entity.F_DeleteTime = DateTime.Now;
            entity.F_DeleteMark = true;
        }
    }
}
