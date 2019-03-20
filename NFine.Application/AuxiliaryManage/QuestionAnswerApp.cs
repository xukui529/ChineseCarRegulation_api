using NFine.Code;
using NFine.Domain.Entity.AuxiliaryManage;
using NFine.Domain.Entity.SystemManage;
using NFine.Domain.IRepository.AuxiliaryManage;
using NFine.Domain.IRepository.SystemManage;
using NFine.Domain.ViewModel;
using NFine.Repository.AuxiliaryManage;
using NFine.Repository.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Application.AuxiliaryManage
{
   public class QuestionAnswerApp
    {
        private IQuestionAnswerRepository questionService = new QuestionAnswerRepository();
		private IUserRepository userService = new UserRepository();
        // 根据标题查询问答数据列表
        //public List<QuestionAnswerModel> GetDataList(Pagination pagination, string keyword)
        //{
        //    List<QuestionAnswerEntity> questionAnswersList = new List<QuestionAnswerEntity>();
        //    List<UserEntity> usersList = new List<UserEntity>();
        //    questionAnswersList = questionService.IQueryable().ToList();
        //    usersList = userService.IQueryable().ToList();
        //    var list = (from question in questionAnswersList
        //                join user in usersList on question.F_CreatorUserId equals user.F_Id
        //                where  ( (question.F_ChTitle != null && question.F_ChTitle.Contains(keyword)  )
        //                ||(question.F_EnTitle != null && question.F_EnTitle.Contains(keyword)))
        //                where question.F_DeleteMark == false
        //                where user.F_DeleteMark == false
        //                orderby question.F_CreatorTime descending
        //                select new QuestionAnswerModel()
        //                {
        //                    F_Id = question.F_Id,
        //                    F_EnTitle = question.F_EnTitle,
        //                    F_ChTitle = question.F_ChTitle,
        //                    F_Type = question.F_Type,
        //                    F_EnAnswer = question.F_EnAnswer,
        //                    F_ChAnswer = question.F_ChAnswer,
        //                    F_CreatorUserName = user.F_RealName,
        //                    F_CreatorUserId = question.F_CreatorUserId,
        //                    F_CreatorTime = question.F_CreatorTime,
        //                    F_DeleteMark = question.F_DeleteMark,
        //                    F_DeleteUserId = question.F_DeleteUserId,
        //                    F_DeleteTime = question.F_DeleteTime,
        //                    F_LastModifyUserId = question.F_LastModifyUserId,
        //                    F_LastModifyTime = question.F_LastModifyTime
        //                }).ToList();
        //    list = list.Skip(pagination.page).Take(pagination.rows).AsQueryable().ToList();
        //    return list;

        //}
        public List<QuestionAnswerModel> GetDataList(int page, int rows, string keyword, out int total)
        {
            List<QuestionAnswerEntity> questionAnswersList = new List<QuestionAnswerEntity>();
            List<UserEntity> usersList = new List<UserEntity>();
            questionAnswersList = questionService.IQueryable().ToList();
            usersList = userService.IQueryable().ToList();
            var list = (from question in questionAnswersList
                        join user in usersList on question.F_CreatorUserId equals user.F_Id
                        where ((question.F_ChTitle != null && question.F_ChTitle.Contains(keyword))
                        || (question.F_EnTitle != null && question.F_EnTitle.Contains(keyword)))
                        where question.F_DeleteMark == false
                        where user.F_DeleteMark == false
                        orderby question.F_CreatorTime descending
                        select new QuestionAnswerModel()
                        {
                            F_Id = question.F_Id,
                            F_EnTitle = question.F_EnTitle,
                            F_ChTitle = question.F_ChTitle,
                            F_Type = question.F_Type,
                            F_EnAnswer = question.F_EnAnswer,
                            F_ChAnswer = question.F_ChAnswer,
                            F_CreatorUserName = user.F_RealName,
                            F_CreatorUserId = question.F_CreatorUserId,
                            F_CreatorTime = question.F_CreatorTime,
                            F_DeleteMark = question.F_DeleteMark,
                            F_DeleteUserId = question.F_DeleteUserId,
                            F_DeleteTime = question.F_DeleteTime,
                            F_LastModifyUserId = question.F_LastModifyUserId,
                            F_LastModifyTime = question.F_LastModifyTime
                        }).ToList();
            total = list.Count();
            list = list.Skip((page - 1) * rows).Take(rows).AsQueryable().ToList();
            return list;
        }

        //public List<QuestionAnswerModel> GetDataList(int page, int rows, string keyword, string type)
        //{
        //    List<QuestionAnswerEntity> questionAnswersList = new List<QuestionAnswerEntity>();
        //    List<UserEntity> usersList = new List<UserEntity>();
        //    questionAnswersList = questionService.IQueryable().ToList();
        //    usersList = userService.IQueryable().ToList();
        //    var list = (from question in questionAnswersList
        //                join user in usersList on question.F_CreatorUserId equals user.F_Id
        //                where  (question.F_ChTitle != null && question.F_ChTitle.Contains(keyword) )
        //                || (question.F_EnTitle != null && question.F_EnTitle.Contains(keyword))
        //                || (question.F_EnAnswer != null && question.F_EnAnswer.Contains(keyword))
        //                || (question.F_ChAnswer != null && question.F_ChAnswer.Contains(keyword))
        //                where question.F_Type == type
        //                where question.F_DeleteMark == false
        //                where user.F_DeleteMark == false
        //                orderby question.F_CreatorTime descending
        //                select new QuestionAnswerModel()
        //                {
        //                    F_Id = question.F_Id,
        //                    F_EnTitle = question.F_EnTitle,
        //                    F_ChTitle = question.F_ChTitle,
        //                    F_Type = question.F_Type,
        //                    F_EnAnswer = question.F_EnAnswer,
        //                    F_ChAnswer = question.F_ChAnswer,
        //                    F_CreatorUserName = user.F_RealName,
        //                    F_CreatorUserId = question.F_CreatorUserId,
        //                    F_CreatorTime = question.F_CreatorTime,
        //                    F_DeleteMark = question.F_DeleteMark,
        //                    F_DeleteUserId = question.F_DeleteUserId,
        //                    F_DeleteTime = question.F_DeleteTime,
        //                    F_LastModifyUserId = question.F_LastModifyUserId,
        //                    F_LastModifyTime = question.F_LastModifyTime
        //                }).ToList();
        //    list = list.Skip((page-1)*rows).Take(rows).AsQueryable().ToList();
        //    return list;
        //}
        public List<QuestionAnswerModel> GetDataList(string type)
        {
            List<QuestionAnswerEntity> questionAnswersList = new List<QuestionAnswerEntity>();

            questionAnswersList = questionService.IQueryable().ToList();
            var list = (from question in questionAnswersList
                        where question.F_Type == type
                        where question.F_DeleteMark == false
                        orderby question.F_CreatorTime descending
                        select new QuestionAnswerModel()
                        {
                            F_Id = question.F_Id,
                            F_EnTitle = question.F_EnTitle,
                            F_ChTitle = question.F_ChTitle,
                            F_Type = question.F_Type,
                            F_EnAnswer = question.F_EnAnswer,
                            F_ChAnswer = question.F_ChAnswer,
                            F_CreatorUserId = question.F_CreatorUserId,
                            F_CreatorTime = question.F_CreatorTime,
                            F_DeleteMark = question.F_DeleteMark,
                            F_DeleteUserId = question.F_DeleteUserId,
                            F_DeleteTime = question.F_DeleteTime,
                            F_LastModifyUserId = question.F_LastModifyUserId,
                            F_LastModifyTime = question.F_LastModifyTime
                        }).ToList();
            return list;
        }

        // 根据主键ID查询数据对象
        public QuestionAnswerEntity GetForm(string keyValue)
        {
            return questionService.FindEntity(keyValue);
        }

        // 根据主键删除问答数据
        public void DeleteForm(string keyValue)
        {
            questionService.DeleteForm(keyValue);
        }

        // 新建提交
        public void SubmitForm(QuestionAnswerEntity entity, string keyValue)
        {
            questionService.SubmitForm(entity, keyValue);
        }
    }
}
