using NFine.Code;
using NFine.Domain.Entity.SystemManage;
using NFine.Domain.IRepository.SystemManage;
using NFine.Repository.SystemManage;
using System;
using System.Linq;
using System.Collections.Generic;

namespace NFine.Application.SystemManage
{
    public class UserApp
    {
        private IUserRepository service = new UserRepository();
        private UserLogOnApp userLogOnApp = new UserLogOnApp();

        //public List<UserEntity> GetList(Pagination pagination, string keyword)
        //{
        //    var expression = ExtLinq.True<UserEntity>();
        //    if (!string.IsNullOrEmpty(keyword))
        //    {
        //        expression = expression.And(t => t.F_Account.Contains(keyword));
        //        expression = expression.Or(t => t.F_RealName.Contains(keyword));
        //        expression = expression.Or(t => t.F_MobilePhone.Contains(keyword));
        //    }
        //    expression = expression.And(t => t.F_Account != "admin");
        //    return service.FindList(expression, pagination);
        //}
        public List<UserEntity> GetDataList(int page, int rows, string keyword, out int total)
        {
            List<UserEntity> modelList = new List<UserEntity>();
            modelList = service.IQueryable().ToList();
            var list = (from model in modelList

                        where model.F_Account.Contains(keyword) || model.F_RealName.Contains(keyword) || model.F_MobilePhone.Contains(keyword)
                        where model.F_Account != "admin"
                        where model.F_DeleteMark == false
                        orderby model.F_CreatorTime descending
                        select new UserEntity()
                        {
                            F_Id = model.F_Id,
                            F_Account = model.F_Account,
                            F_RealName = model.F_RealName,
                            F_NickName = model.F_NickName,
                            F_HeadIcon = model.F_HeadIcon,
                            F_Gender = model.F_Gender,
                            F_Birthday = model.F_Birthday,
                            F_MobilePhone = model.F_MobilePhone,
                            F_Email = model.F_Email,
                            F_WeChat = model.F_WeChat,
                            F_ManagerId = model.F_ManagerId,
                            F_SecurityLevel = model.F_SecurityLevel,
                            F_Signature = model.F_Signature,
                            F_OrganizeId = model.F_OrganizeId,
                            F_DepartmentId = model.F_DepartmentId,
                            F_RoleId = model.F_RoleId,
                            F_DutyId = model.F_DutyId,
                            F_IsAdministrator = model.F_IsAdministrator,
                            F_SortCode = model.F_SortCode, 
                            F_EnabledMark = model.F_EnabledMark,
                            F_Description = model.F_Description, 
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
        public UserEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            service.DeleteForm(keyValue);
        }
        public void SubmitForm(UserEntity userEntity, UserLogOnEntity userLogOnEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                userEntity.Modify(keyValue);
            }
            else
            {
                userEntity.Create();
            }
            service.SubmitForm(userEntity, userLogOnEntity, keyValue);
        }
        public void UpdateForm(UserEntity userEntity)
        {
            service.Update(userEntity);
        }
        public UserEntity CheckLogin(string username, string password)
        {
            UserEntity userEntity = service.FindEntity(t => t.F_Account == username);
            if (userEntity != null)
            {
                if (userEntity.F_EnabledMark == true)
                {
                    UserLogOnEntity userLogOnEntity = userLogOnApp.GetForm(userEntity.F_Id);
                    string dbPassword = Md5.md5(DESEncrypt.Encrypt(password.ToLower(), userLogOnEntity.F_UserSecretkey).ToLower(), 32).ToLower();
                    if (dbPassword == userLogOnEntity.F_UserPassword)
                    {
                        DateTime lastVisitTime = DateTime.Now;
                        int LogOnCount = (userLogOnEntity.F_LogOnCount).ToInt() + 1;
                        if (userLogOnEntity.F_LastVisitTime != null)
                        {
                            userLogOnEntity.F_PreviousVisitTime = userLogOnEntity.F_LastVisitTime.ToDate();
                        }
                        userLogOnEntity.F_LastVisitTime = lastVisitTime;
                        userLogOnEntity.F_LogOnCount = LogOnCount;
                        userLogOnApp.UpdateForm(userLogOnEntity);
                        return userEntity;
                    }
                    else
                    {
                        throw new Exception("用户名或密码不正确，请重新输入");
                    }
                }
                else
                {
                    throw new Exception("账户被系统锁定,请联系管理员");
                }
            }
            else
            {
                throw new Exception("账户不存在，请重新输入");
            }
        }

		public UserEntity WebCheckLogin(string username, string password)
		{
			UserEntity userEntity = service.FindEntity(t => t.F_Account == username || t.F_Email == username);
			if (userEntity != null)
			{
				if (userEntity.F_EnabledMark == true)
				{
					UserLogOnEntity userLogOnEntity = userLogOnApp.GetForm(userEntity.F_Id);
					if (userLogOnEntity == null)
					{
						userEntity.F_Description = "用户不存在";
						return userEntity;
					}
					string dbPassword = Md5.md5(DESEncrypt.Encrypt(password.ToLower(), userLogOnEntity.F_UserSecretkey).ToLower(), 32).ToLower();
					if (dbPassword == userLogOnEntity.F_UserPassword)
					{
						DateTime lastVisitTime = DateTime.Now;
						int LogOnCount = (userLogOnEntity.F_LogOnCount).ToInt() + 1;
						if (userLogOnEntity.F_LastVisitTime != null)
						{
							userLogOnEntity.F_PreviousVisitTime = userLogOnEntity.F_LastVisitTime.ToDate();
						}
						userLogOnEntity.F_LastVisitTime = lastVisitTime;
						userLogOnEntity.F_LogOnCount = LogOnCount;
						userLogOnApp.UpdateForm(userLogOnEntity);
						return userEntity;
					}
					else
					{
						userEntity.F_Description = "用户名或密码不正确，请重新输入";
						return userEntity;
					}
				}
				else
				{
					userEntity.F_Description = "账户被系统锁定,请联系管理员";
					return userEntity;
				}
			}
			else
			{
				userEntity.F_Description = "账户不存在，请重新输入";
				return userEntity;
			}
		}
	}
}
