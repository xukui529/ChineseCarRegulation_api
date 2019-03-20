using NFine.Code;
using NFine.Domain.Entity.AuxiliaryManage;
using NFine.Domain.Entity.ContentManage;
using NFine.Domain.Entity.SystemManage;
using NFine.Domain.IRepository.AuxiliaryManage;
using NFine.Domain.IRepository.ContentManage;
using NFine.Domain.IRepository.SystemManage;
using NFine.Domain.ViewModel;
using NFine.Repository.AuxiliaryManage;
using NFine.Repository.ContentManage;
using NFine.Repository.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Application.ContentManage
{
	public class DocumentApp
	{
		private IDocumentRepository documentService = new DocumentRepository();
		private IUserRepository userService = new UserRepository(); 
        private IDocumentContentRepository documentContentService = new DocumentContentRepository();



        /// <summary>
        /// 查询本期要发送的订阅邮件 发布时间是 上周一到上周日
        /// </summary> 
        /// <returns></returns>
        public List<DocumentEntity> GetSubscriptionDataList(DateTime beginTime,DateTime endTime)
        { 
            List<DocumentEntity> documentList = new List<DocumentEntity>(); 
            documentList = documentService.IQueryable().ToList(); 
            var list = (from document in documentList 
                        where document.F_DeleteMark == false 
                        where document.F_State ==(int) NFine.Code.Enum.DocumentStatus.已发布 
                        where (document.F_Type == (int)NFine.Code.Enum.DocumentType.标准|| document.F_Type == (int)NFine.Code.Enum.DocumentType.法规)
                        where document.F_ReleaseDate >= beginTime && document.F_ReleaseDate <= endTime
                        orderby document.F_CreatorTime descending
                        select new DocumentEntity()
                        {
                            F_Id = document.F_Id,
                            F_ChineseTitle = document.F_ChineseTitle,
                            F_EnglishTitle = document.F_EnglishTitle,
                            F_FileState = document.F_FileState,
                            F_Domain = document.F_Domain,
                            F_AcquisitionStandard = document.F_AcquisitionStandard,
                            F_Label = document.F_Label,
                            F_Publisher = document.F_Publisher,
                            F_Direction = document.F_Direction,
                            F_ReleaseDate = document.F_ReleaseDate,
                            F_ImplementDate = document.F_ImplementDate,
                            F_HtmlUrl = document.F_HtmlUrl,
                            F_ChineseUrl = document.F_ChineseUrl,
                            F_EnglishUrl = document.F_EnglishUrl,
                            F_TotalUrl = document.F_TotalUrl,
                            F_InfoContent = document.F_InfoContent,
                            F_State = document.F_State,
                            F_IsActivateVip = document.F_IsActivateVip,
                            F_IsActivate = document.F_IsActivate,
                            F_Type = document.F_Type, 
                            F_CreatorUserId = document.F_CreatorUserId,
                            F_CreatorTime = document.F_CreatorTime,
                            F_DeleteMark = document.F_DeleteMark,
                            F_DeleteUserId = document.F_DeleteUserId,
                            F_DeleteTime = document.F_DeleteTime,
                            F_LastModifyUserId = document.F_LastModifyUserId,
                            F_LastModifyTime = document.F_LastModifyTime
                        }).ToList(); 
            return list;
        }

        public List<DocumentEntity> GetDataListAll()
        { 
            return documentService.IQueryable().ToList();
        }


        /// <summary>
        /// 根据标题查询数据列表--审核页面专用
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="keyword"></param>
        /// <param name="index">区别法规，标准，咨询</param>
        /// <param name="type">区别发布状态</param>
        /// <returns></returns>
        public List<DocumentModel> GetAuditDataList(int page, int rows, string keyword, int index, int type, out int total)
        {
            List<DocumentEntity> documentList = new List<DocumentEntity>();
            List<UserEntity> usersList = new List<UserEntity>();
            documentList = documentService.IQueryable().ToList();
            usersList = userService.IQueryable().ToList();
            var list = (from document in documentList
                        join user in usersList on document.F_CreatorUserId equals user.F_Id
                        where document.F_ChineseTitle.Contains(keyword) || document.F_EnglishTitle.Contains(keyword)
                        where document.F_DeleteMark == false
                        where user.F_DeleteMark == false
                        where type == 1 ? document.F_State == 1 : type == 2 ? document.F_State== 2 : document.F_State == 4
                        //1      2   4
                        where document.F_Type == index
                        orderby document.F_CreatorTime descending
                        select new DocumentModel()
                        {
                            F_Id = document.F_Id,
                            F_ChineseTitle = document.F_ChineseTitle,
                            F_EnglishTitle = document.F_EnglishTitle,
                            F_FileState = document.F_FileState,
                            F_Domain = document.F_Domain,
                            F_AcquisitionStandard = document.F_AcquisitionStandard,
                            F_Label = document.F_Label,
                            F_Publisher = document.F_Publisher,
                            F_Direction = document.F_Direction,
                            F_ReleaseDate = document.F_ReleaseDate,
                            F_ImplementDate = document.F_ImplementDate,
                            F_HtmlUrl = document.F_HtmlUrl,
                            F_ChineseUrl = document.F_ChineseUrl,
                            F_EnglishUrl = document.F_EnglishUrl,
                             F_TotalUrl= document.F_TotalUrl,
                              F_InfoContent=document.F_InfoContent,
                            F_State = document.F_State,
                            F_IsActivateVip = document.F_IsActivateVip,
                            F_IsActivate = document.F_IsActivate,
                            F_Type = document.F_Type,
                            F_CreatorUserName = user.F_RealName,
                            F_CreatorUserId = document.F_CreatorUserId,
                            F_CreatorTime = document.F_CreatorTime,
                            F_DeleteMark = document.F_DeleteMark,
                            F_DeleteUserId = document.F_DeleteUserId,
                            F_DeleteTime = document.F_DeleteTime,
                            F_LastModifyUserId = document.F_LastModifyUserId,
                            F_LastModifyTime = document.F_LastModifyTime
                        }).ToList();
            total = list.Count();
            list = list.Skip((page-1)*rows).Take(rows).AsQueryable().ToList();
            return list;
        }


        /// <summary>
        /// 根据标题查询数据列表 新增页面专用
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="keyword"></param>
        /// <param name="index">区别法规，标准，咨询</param>
        /// <param name="type">区别发布状态</param>
        /// <returns></returns>
        public List<DocumentModel> GetDataList(int page, int rows, string keyword, int index, int type, out int total)
		{
			List<DocumentEntity> documentList = new List<DocumentEntity>();
			List<UserEntity> usersList = new List<UserEntity>();
			documentList = documentService.IQueryable().ToList();
			usersList = userService.IQueryable().ToList();
			var list = (from document in documentList
						join user in usersList on document.F_CreatorUserId equals user.F_Id
						where document.F_ChineseTitle.Contains(keyword) || document.F_EnglishTitle.Contains(keyword)
						where document.F_DeleteMark == false
						where user.F_DeleteMark == false
						where type == 1 ? document.F_State == 0 : type == 2 ? (document.F_State > 0 && document.F_State < 4) : document.F_State == 4
                        //1 最新最新编辑 0  2 审核状态 123 3 已发布 4
						where document.F_Type == index
						orderby document.F_CreatorTime descending
						select new DocumentModel()
						{
							F_Id = document.F_Id,
							F_ChineseTitle = document.F_ChineseTitle,
							F_EnglishTitle = document.F_EnglishTitle,
							F_FileState = document.F_FileState,
							F_Domain = document.F_Domain,
							F_AcquisitionStandard = document.F_AcquisitionStandard,
							F_Label = document.F_Label,
							F_Publisher = document.F_Publisher,
							F_Direction = document.F_Direction,
							F_ReleaseDate = document.F_ReleaseDate,
							F_ImplementDate = document.F_ImplementDate,
							F_HtmlUrl = document.F_HtmlUrl,
							F_ChineseUrl = document.F_ChineseUrl,
							F_EnglishUrl = document.F_EnglishUrl,
                            F_TotalUrl = document.F_TotalUrl, 
                            F_State = document.F_State,
							F_IsActivateVip = document.F_IsActivateVip,
							F_IsActivate=document.F_IsActivate,
							F_Type = document.F_Type,
							F_CreatorUserName = user.F_RealName,
							F_CreatorUserId = document.F_CreatorUserId,
							F_CreatorTime = document.F_CreatorTime,
							F_DeleteMark = document.F_DeleteMark,
							F_DeleteUserId = document.F_DeleteUserId,
							F_DeleteTime = document.F_DeleteTime,
							F_LastModifyUserId = document.F_LastModifyUserId,
							F_LastModifyTime = document.F_LastModifyTime
						}).ToList();
            total = list.Count();
            list = list.Skip((page-1)*rows).Take(rows).AsQueryable().ToList();
			return list;
		}

		/// <summary>
		/// 根据标题查询数据列表
		/// </summary>
		/// <param name="pagination"></param>
		/// <param name="keyword"></param>
		/// <param name="index">区别法规，标准，咨询</param>
		/// <param name="type">区别发布状态</param>
		/// <returns></returns>
		public List<DocumentModel> GetDataListApi(int page, int rows, string keyword, int index, int type)
        {
            

            List<DocumentEntity> documentList = new List<DocumentEntity>();
			List<UserEntity> usersList = new List<UserEntity>();
			documentList = documentService.IQueryable().ToList();
			usersList = userService.IQueryable().ToList();
			var list = (from document in documentList
						join user in usersList on document.F_CreatorUserId equals user.F_Id into temp
						from data in temp.DefaultIfEmpty()
						where document.F_ChineseTitle.Contains(keyword) || document.F_EnglishTitle.Contains(keyword)
						where document.F_DeleteMark == false
						where data.F_DeleteMark == false
						where document.F_State == (int)Code.Enum.DocumentStatus.已发布
                        where document.F_Type == index
						orderby document.F_CreatorTime descending
						select new DocumentModel()
						{
							F_Id = document.F_Id,
							F_ChineseTitle = document.F_ChineseTitle,
							F_EnglishTitle = document.F_EnglishTitle,
							F_FileState = document.F_FileState,
							F_Domain = document.F_Domain,
							F_AcquisitionStandard = document.F_AcquisitionStandard,
							F_Label = document.F_Label,
							F_Publisher = document.F_Publisher,
							F_Direction = document.F_Direction,
							F_ReleaseDate = document.F_ReleaseDate,
							F_ImplementDate = document.F_ImplementDate,
							F_HtmlUrl = document.F_HtmlUrl,
							F_ChineseUrl = document.F_ChineseUrl,
							F_EnglishUrl = document.F_EnglishUrl,
							F_State = document.F_State,
							F_IsActivateVip = document.F_IsActivateVip,
							F_IsActivate = document.F_IsActivate,
							F_Type = document.F_Type,
							F_CreatorUserName = data.F_RealName,
							F_CreatorUserId = document.F_CreatorUserId,
							F_CreatorTime = document.F_CreatorTime,
							F_DeleteMark = document.F_DeleteMark,
							F_DeleteUserId = document.F_DeleteUserId,
							F_DeleteTime = document.F_DeleteTime,
							F_LastModifyUserId = document.F_LastModifyUserId,
							F_LastModifyTime = document.F_LastModifyTime
						}).ToList();
			list = list.Skip(page * rows).Take(rows).AsQueryable().ToList();
			return list;
		}

		// 根据标题查询数据列表
		/// <summary>
		/// 
		/// </summary>
		/// <param name="pagination"></param>
		/// <param name="keyword"></param>
		/// <param name="index">区别法规，标准，咨询</param>
		/// <param name="type">区别发布状态</param>
		/// <returns></returns>
		public List<DocumentModel> GetDataListByCondition(int page, int rows, string chinesetitle, string englishtitle, string domain, string filestate, string acquisitionstandard, int state, int type)
		{
			List<DocumentEntity> documentList = new List<DocumentEntity>();
			List<UserEntity> usersList = new List<UserEntity>();
			documentList = documentService.IQueryable().ToList();
			usersList = userService.IQueryable().ToList();
			var list = (from document in documentList
						join user in usersList on document.F_CreatorUserId equals user.F_Id
						where string.IsNullOrEmpty(chinesetitle) || document.F_ChineseTitle.Contains(chinesetitle)
						where string.IsNullOrEmpty(englishtitle) || document.F_EnglishTitle.Contains(englishtitle)
						where string.IsNullOrEmpty(domain) || document.F_Domain.Contains(domain)
						where string.IsNullOrEmpty(filestate) || document.F_Domain.Contains(filestate)
						where string.IsNullOrEmpty(acquisitionstandard) || document.F_Domain.Contains(acquisitionstandard)
						where document.F_DeleteMark == false
						where user.F_DeleteMark == false
						where document.F_State == (int)Code.Enum.DocumentStatus.已发布
                        where document.F_Type == type
						orderby document.F_CreatorTime descending
						select new DocumentModel()
						{
							F_Id = document.F_Id,
							F_ChineseTitle = document.F_ChineseTitle,
							F_EnglishTitle = document.F_EnglishTitle,
							F_FileState = document.F_FileState,
							F_Domain = document.F_Domain,
							F_AcquisitionStandard = document.F_AcquisitionStandard,
							F_Label = document.F_Label,
							F_Publisher = document.F_Publisher,
							F_Direction = document.F_Direction,
							F_ReleaseDate = document.F_ReleaseDate,
							F_ImplementDate = document.F_ImplementDate,
							F_HtmlUrl = document.F_HtmlUrl,
							F_ChineseUrl = document.F_ChineseUrl,
							F_EnglishUrl = document.F_EnglishUrl,
							F_State = document.F_State,
							F_IsActivateVip = document.F_IsActivateVip,
							F_Type = document.F_Type,
							F_CreatorUserName = user.F_RealName,
							F_CreatorUserId = document.F_CreatorUserId,
							F_CreatorTime = document.F_CreatorTime,
							F_DeleteMark = document.F_DeleteMark,
							F_DeleteUserId = document.F_DeleteUserId,
							F_DeleteTime = document.F_DeleteTime,
							F_LastModifyUserId = document.F_LastModifyUserId,
							F_LastModifyTime = document.F_LastModifyTime
						}).ToList();
			list = list.Skip((page-1)*rows).Take(rows).AsQueryable().ToList();
			return list;
		}
        /// <summary>
        ///  
        /// </summary>
        /// <returns></returns>
		public DocumentTotalModel GetDataTotal()
		{
			DocumentTotalModel model = new DocumentTotalModel();
			List<DocumentEntity> documentList = new List<DocumentEntity>();
			documentList = documentService.IQueryable().ToList();
            model.WaitSubmitTotal = (from a in documentList
                                     where a.F_State == 0 
                                     where a.F_DeleteMark == false
                                     select a.F_Id).ToList().Count(); 
            
            model.UnauditedTotal = (from a in documentList
									where a.F_State == 1 
									where a.F_DeleteMark == false select a.F_Id ).ToList().Count();

            model.WaitPublishe = (from a in documentList
                                  where a.F_State == 2
                                  where a.F_DeleteMark == false
                                  select a.F_Id).ToList().Count();

            model.AuditTotal = (from a in documentList
								where a.F_State == 3
								where a.F_DeleteMark == false
								select a.F_Id).ToList().Count();
			model.PublishedTotal = (from a in documentList
									where a.F_State == 4
									where a.F_DeleteMark == false
									select a.F_Id).ToList().Count();
			return model;
		}


		// 根据主键ID查询数据对象
		public DocumentEntity GetForm(string keyValue)
		{
			return documentService.FindEntity(keyValue);
		}

		// 根据主键删除
		public void DeleteForm(string keyValue)
		{
			documentService.DeleteForm(keyValue);
		}

		// 新建提交
		public void SubmitForm(DocumentEntity entity, string keyValue)
		{
			documentService.SubmitForm(entity, keyValue);
		}

		// 新建提交
		public void UpdateState(string keyValue, int state)
		{
			documentService.UpdateState(keyValue, state);
		}

        /// <summary>
        /// 根据标题查询数据列表
        /// </summary> 
        /// <param name="keyword"></param> 
        /// <returns></returns>
        public List<DocumentModel> SearchForIndexByContent(string keyword)
        { 
            List<DocumentEntity> documentList = new List<DocumentEntity>();

            documentList = documentService.IQueryable().ToList();
            var list1 = (from document in documentList
                         where (document.F_Label!=null && document.F_Label.Contains(keyword))
                         where document.F_DeleteMark == false
                         where document.F_State == (int)Code.Enum.DocumentStatus.已发布
                         orderby document.F_CreatorTime descending
                         select new DocumentModel()
                         {
                             F_Id = document.F_Id,
                             F_ChineseTitle = document.F_ChineseTitle,
                             F_EnglishTitle = document.F_EnglishTitle,
                             F_FileState = document.F_FileState,
                             F_Domain = document.F_Domain,
                             F_AcquisitionStandard = document.F_AcquisitionStandard,
                             F_Label = document.F_Label,
                             F_Publisher = document.F_Publisher,
                             F_Direction = document.F_Direction,
                             F_ReleaseDate = document.F_ReleaseDate,
                             F_ImplementDate = document.F_ImplementDate,
                             F_HtmlUrl = document.F_HtmlUrl,
                             F_ChineseUrl = document.F_ChineseUrl,
                             F_EnglishUrl = document.F_EnglishUrl,
                             F_State = document.F_State,
                             F_IsActivateVip = document.F_IsActivateVip,
                             F_Type = document.F_Type,

                             F_CreatorUserId = document.F_CreatorUserId,
                             F_CreatorTime = document.F_CreatorTime,
                             F_DeleteMark = document.F_DeleteMark,
                             F_DeleteUserId = document.F_DeleteUserId,
                             F_DeleteTime = document.F_DeleteTime,
                             F_LastModifyUserId = document.F_LastModifyUserId,
                             F_LastModifyTime = document.F_LastModifyTime
                         }).ToList();


            var list2 = (from document in documentList
                         where(document.F_ChineseTitle != null && document.F_ChineseTitle.Contains(keyword) )|| 
                         (document.F_EnglishTitle != null && document.F_EnglishTitle.Contains(keyword))
                         where document.F_State == (int)Code.Enum.DocumentStatus.已发布
                         where document.F_DeleteMark == false
                         orderby document.F_CreatorTime descending
                         select new DocumentModel()
                         {
                             F_Id = document.F_Id,
                             F_ChineseTitle = document.F_ChineseTitle,
                             F_EnglishTitle = document.F_EnglishTitle,
                             F_FileState = document.F_FileState,
                             F_Domain = document.F_Domain,
                             F_AcquisitionStandard = document.F_AcquisitionStandard,
                             F_Label = document.F_Label,
                             F_Publisher = document.F_Publisher,
                             F_Direction = document.F_Direction,
                             F_ReleaseDate = document.F_ReleaseDate,
                             F_ImplementDate = document.F_ImplementDate,
                             F_HtmlUrl = document.F_HtmlUrl,
                             F_ChineseUrl = document.F_ChineseUrl,
                             F_EnglishUrl = document.F_EnglishUrl,
                             F_State = document.F_State,
                             F_IsActivateVip = document.F_IsActivateVip,
                             F_Type = document.F_Type,

                             F_CreatorUserId = document.F_CreatorUserId,
                             F_CreatorTime = document.F_CreatorTime,
                             F_DeleteMark = document.F_DeleteMark,
                             F_DeleteUserId = document.F_DeleteUserId,
                             F_DeleteTime = document.F_DeleteTime,
                             F_LastModifyUserId = document.F_LastModifyUserId,
                             F_LastModifyTime = document.F_LastModifyTime
                         }).ToList();

            var list = list1.Union(list2).ToList(); 
            list = list.MyDistinct(s => s.F_Id).ToList();
            return list;
        }



        /// <summary>
        /// 根据标题查询数据列表
        /// </summary> 
        /// <param name="keyword"></param> 
        /// <returns></returns>
        public List<DocumentModel> SearchForIndexByContent()
        {
            List<DocumentEntity> documentList = new List<DocumentEntity>();

            documentList = documentService.IQueryable().ToList();
            var list1 = (from document in documentList 
                         where document.F_DeleteMark == false
                         where document.F_State == (int)Code.Enum.DocumentStatus.已发布
                         orderby document.F_CreatorTime descending
                         select new DocumentModel()
                         {
                             F_Id = document.F_Id,
                             F_ChineseTitle = document.F_ChineseTitle,
                             F_EnglishTitle = document.F_EnglishTitle,
                             F_FileState = document.F_FileState,
                             F_Domain = document.F_Domain,
                             F_AcquisitionStandard = document.F_AcquisitionStandard,
                             F_Label = document.F_Label,
                             F_Publisher = document.F_Publisher,
                             F_Direction = document.F_Direction,
                             F_ReleaseDate = document.F_ReleaseDate,
                             F_ImplementDate = document.F_ImplementDate,
                             F_HtmlUrl = document.F_HtmlUrl,
                             F_ChineseUrl = document.F_ChineseUrl,
                             F_EnglishUrl = document.F_EnglishUrl,
                             F_State = document.F_State,
                             F_IsActivateVip = document.F_IsActivateVip,
                             F_Type = document.F_Type,

                             F_CreatorUserId = document.F_CreatorUserId,
                             F_CreatorTime = document.F_CreatorTime,
                             F_DeleteMark = document.F_DeleteMark,
                             F_DeleteUserId = document.F_DeleteUserId,
                             F_DeleteTime = document.F_DeleteTime,
                             F_LastModifyUserId = document.F_LastModifyUserId,
                             F_LastModifyTime = document.F_LastModifyTime
                         }).ToList();


            var list2 = (from document in documentList 
                         where document.F_State == (int)Code.Enum.DocumentStatus.已发布
                         where document.F_DeleteMark == false
                         orderby document.F_CreatorTime descending
                         select new DocumentModel()
                         {
                             F_Id = document.F_Id,
                             F_ChineseTitle = document.F_ChineseTitle,
                             F_EnglishTitle = document.F_EnglishTitle,
                             F_FileState = document.F_FileState,
                             F_Domain = document.F_Domain,
                             F_AcquisitionStandard = document.F_AcquisitionStandard,
                             F_Label = document.F_Label,
                             F_Publisher = document.F_Publisher,
                             F_Direction = document.F_Direction,
                             F_ReleaseDate = document.F_ReleaseDate,
                             F_ImplementDate = document.F_ImplementDate,
                             F_HtmlUrl = document.F_HtmlUrl,
                             F_ChineseUrl = document.F_ChineseUrl,
                             F_EnglishUrl = document.F_EnglishUrl,
                             F_State = document.F_State,
                             F_IsActivateVip = document.F_IsActivateVip,
                             F_Type = document.F_Type,

                             F_CreatorUserId = document.F_CreatorUserId,
                             F_CreatorTime = document.F_CreatorTime,
                             F_DeleteMark = document.F_DeleteMark,
                             F_DeleteUserId = document.F_DeleteUserId,
                             F_DeleteTime = document.F_DeleteTime,
                             F_LastModifyUserId = document.F_LastModifyUserId,
                             F_LastModifyTime = document.F_LastModifyTime
                         }).ToList();

            var list = list1.Union(list2).ToList();
            list= list.MyDistinct(s => s.F_Id).ToList() ;
            return list;
        }



        /// <summary>
        /// 根据标题查询数据列表
        /// </summary> 
        /// <param name="keyword"></param> 
        /// <returns></returns>
        public List<DocumentModel> SearchForIndexByContent(int type, string fileState, string domain, string acquisitionStandard
            , string publisher, string direction ,string keyword)
        {
            List<DocumentEntity> documentList = new List<DocumentEntity>();

            documentList = documentService.IQueryable().ToList();
            var list1 = (from document in documentList

                         where document.F_Type == type
                         where document.F_State == (int)Code.Enum.DocumentStatus.已发布
                         where (fileState == "" ? true : document.F_FileState == fileState)


                         where (publisher == "" ? true:(document.F_Publisher != null&&(document.F_Publisher.Contains(publisher))))
                         where (direction == "" ? true : (document.F_Direction != null && (document.F_Direction.Contains(direction))))
                         where (domain == "" ? true : (document.F_Domain != null && (document.F_Domain.Contains(domain))))
                         where (acquisitionStandard == "" ? true : (document.F_AcquisitionStandard != null && (document.F_AcquisitionStandard.Contains(acquisitionStandard))))


                      
                         //where (document.F_Publisher!=null &&(publisher == "" ? true : document.F_Publisher.Contains(publisher)))
                         //where (document.F_Direction != null && (direction == "" ? true : document.F_Direction.Contains(direction)))
                         //where (document.F_Domain != null && (domain == "" ? true : document.F_Domain.Contains(domain)))
                         //where (document.F_AcquisitionStandard != null && (acquisitionStandard == "" ? true : document.F_AcquisitionStandard.Contains(acquisitionStandard)))

                         where (document.F_Label != null && document.F_Label.Contains(keyword))
                         where document.F_DeleteMark == false 
                         orderby document.F_CreatorTime descending
                         select new DocumentModel()
                         {
                             F_Id = document.F_Id,
                             F_ChineseTitle = document.F_ChineseTitle,
                             F_EnglishTitle = document.F_EnglishTitle,
                             F_FileState = document.F_FileState,
                             F_Domain = document.F_Domain,
                             F_AcquisitionStandard = document.F_AcquisitionStandard,
                             F_Label = document.F_Label,
                             F_Publisher = document.F_Publisher,
                             F_Direction = document.F_Direction,
                             F_ReleaseDate = document.F_ReleaseDate,
                             F_ImplementDate = document.F_ImplementDate,
                             F_HtmlUrl = document.F_HtmlUrl,
                             F_ChineseUrl = document.F_ChineseUrl,
                             F_EnglishUrl = document.F_EnglishUrl,
                             F_State = document.F_State,
                             F_IsActivateVip = document.F_IsActivateVip,
                             F_Type = document.F_Type,

                             F_CreatorUserId = document.F_CreatorUserId,
                             F_CreatorTime = document.F_CreatorTime,
                             F_DeleteMark = document.F_DeleteMark,
                             F_DeleteUserId = document.F_DeleteUserId,
                             F_DeleteTime = document.F_DeleteTime,
                             F_LastModifyUserId = document.F_LastModifyUserId,
                             F_LastModifyTime = document.F_LastModifyTime
                         }).ToList();


            var list2 = (from document in documentList

                         where document.F_Type == type
                         where document.F_State == (int)Code.Enum.DocumentStatus.已发布
                         where (fileState == "" ? true : document.F_FileState == fileState)


                         where (publisher == "" ? true : (document.F_Publisher != null && (document.F_Publisher.Contains(publisher))))
                         where (direction == "" ? true : (document.F_Direction != null && (document.F_Direction.Contains(direction))))
                         where (domain == "" ? true : (document.F_Domain != null && (document.F_Domain.Contains(domain))))
                         where (acquisitionStandard == "" ? true : (document.F_AcquisitionStandard != null && (document.F_AcquisitionStandard.Contains(acquisitionStandard))))






                         //where (document.F_Publisher != null && (publisher == "" ? true : document.F_Publisher.Contains(publisher)))
                         //where (document.F_Direction != null && (direction == "" ? true : document.F_Direction.Contains(direction)))
                         //where (document.F_Domain != null && (domain == "" ? true : document.F_Domain.Contains(domain)))
                         //where (document.F_AcquisitionStandard != null && (acquisitionStandard == "" ? true : document.F_AcquisitionStandard.Contains(acquisitionStandard)))

                         where (document.F_ChineseTitle != null && document.F_ChineseTitle.Contains(keyword)) ||
                         (document.F_EnglishTitle != null && document.F_EnglishTitle.Contains(keyword))
                          
                         where document.F_DeleteMark == false
                         orderby document.F_CreatorTime descending
                         select new DocumentModel()
                         {
                             F_Id = document.F_Id,
                             F_ChineseTitle = document.F_ChineseTitle,
                             F_EnglishTitle = document.F_EnglishTitle,
                             F_FileState = document.F_FileState,
                             F_Domain = document.F_Domain,
                             F_AcquisitionStandard = document.F_AcquisitionStandard,
                             F_Label = document.F_Label,
                             F_Publisher = document.F_Publisher,
                             F_Direction = document.F_Direction,
                             F_ReleaseDate = document.F_ReleaseDate,
                             F_ImplementDate = document.F_ImplementDate,
                             F_HtmlUrl = document.F_HtmlUrl,
                             F_ChineseUrl = document.F_ChineseUrl,
                             F_EnglishUrl = document.F_EnglishUrl,
                             F_State = document.F_State,
                             F_IsActivateVip = document.F_IsActivateVip,
                             F_Type = document.F_Type,

                             F_CreatorUserId = document.F_CreatorUserId,
                             F_CreatorTime = document.F_CreatorTime,
                             F_DeleteMark = document.F_DeleteMark,
                             F_DeleteUserId = document.F_DeleteUserId,
                             F_DeleteTime = document.F_DeleteTime,
                             F_LastModifyUserId = document.F_LastModifyUserId,
                             F_LastModifyTime = document.F_LastModifyTime
                         }).ToList();

            var list = list1.Union(list2).ToList();
            list = list.MyDistinct(s => s.F_Id).ToList();
            return list;
        }



        /// <summary>
        /// 根据标题查询数据列表
        /// </summary> 
        /// <param name="keyword"></param> 
        /// <returns></returns>
        public List<DocumentModel> SearchForIndexByContent(int type, string fileState, string domain, string acquisitionStandard
            , string publisher, string direction   )
        {
            List<DocumentEntity> documentList = new List<DocumentEntity>();

            documentList = documentService.IQueryable().ToList();
            var list1 = (from document in documentList
                         where document.F_DeleteMark == false

                         where document.F_Type == type
                         where document.F_State == (int)Code.Enum.DocumentStatus.已发布
                         where (fileState == "" ? true : document.F_FileState == fileState)



                         where (publisher == "" ? true : (document.F_Publisher != null && (document.F_Publisher.Contains(publisher))))
                         where (direction == "" ? true : (document.F_Direction != null && (document.F_Direction.Contains(direction))))
                         where (domain == "" ? true : (document.F_Domain != null && (document.F_Domain.Contains(domain))))
                         where (acquisitionStandard == "" ? true : (document.F_AcquisitionStandard != null && (document.F_AcquisitionStandard.Contains(acquisitionStandard))))



                         //where (publisher == "" ? true : document.F_Publisher.Contains(publisher))
                         //where (direction == "" ? true : document.F_Direction.Contains(direction))
                         //where (domain == "" ? true : document.F_Domain.Contains(domain))
                         //where (acquisitionStandard == "" ? true : document.F_AcquisitionStandard.Contains(acquisitionStandard))

                         orderby document.F_CreatorTime descending
                         select new DocumentModel()
                         {
                             F_Id = document.F_Id,
                             F_ChineseTitle = document.F_ChineseTitle,
                             F_EnglishTitle = document.F_EnglishTitle,
                             F_FileState = document.F_FileState,
                             F_Domain = document.F_Domain,
                             F_AcquisitionStandard = document.F_AcquisitionStandard,
                             F_Label = document.F_Label,
                             F_Publisher = document.F_Publisher,
                             F_Direction = document.F_Direction,
                             F_ReleaseDate = document.F_ReleaseDate,
                             F_ImplementDate = document.F_ImplementDate,
                             F_HtmlUrl = document.F_HtmlUrl,
                             F_ChineseUrl = document.F_ChineseUrl,
                             F_EnglishUrl = document.F_EnglishUrl,
                             F_State = document.F_State,
                             F_IsActivateVip = document.F_IsActivateVip,
                             F_Type = document.F_Type,

                             F_CreatorUserId = document.F_CreatorUserId,
                             F_CreatorTime = document.F_CreatorTime,
                             F_DeleteMark = document.F_DeleteMark,
                             F_DeleteUserId = document.F_DeleteUserId,
                             F_DeleteTime = document.F_DeleteTime,
                             F_LastModifyUserId = document.F_LastModifyUserId,
                             F_LastModifyTime = document.F_LastModifyTime
                         }).ToList();


            var list2 = (from document in documentList

                         where document.F_Type == type
                         where document.F_State == (int)Code.Enum.DocumentStatus.已发布
                         where (fileState == "" ? true : document.F_FileState == fileState)



                         where (publisher == "" ? true : (document.F_Publisher != null && (document.F_Publisher.Contains(publisher))))
                         where (direction == "" ? true : (document.F_Direction != null && (document.F_Direction.Contains(direction))))
                         where (domain == "" ? true : (document.F_Domain != null && (document.F_Domain.Contains(domain))))
                         where (acquisitionStandard == "" ? true : (document.F_AcquisitionStandard != null && (document.F_AcquisitionStandard.Contains(acquisitionStandard))))


                         //where (publisher == "" ? true : document.F_Publisher.Contains(publisher))
                         //where (direction == "" ? true : document.F_Direction.Contains(direction))
                         //where (domain == "" ? true : document.F_Domain.Contains(domain))
                         //where (acquisitionStandard == "" ? true : document.F_AcquisitionStandard.Contains(acquisitionStandard))




                         where document.F_DeleteMark == false
                         orderby document.F_CreatorTime descending
                         select new DocumentModel()
                         {
                             F_Id = document.F_Id,
                             F_ChineseTitle = document.F_ChineseTitle,
                             F_EnglishTitle = document.F_EnglishTitle,
                             F_FileState = document.F_FileState,
                             F_Domain = document.F_Domain,
                             F_AcquisitionStandard = document.F_AcquisitionStandard,
                             F_Label = document.F_Label,
                             F_Publisher = document.F_Publisher,
                             F_Direction = document.F_Direction,
                             F_ReleaseDate = document.F_ReleaseDate,
                             F_ImplementDate = document.F_ImplementDate,
                             F_HtmlUrl = document.F_HtmlUrl,
                             F_ChineseUrl = document.F_ChineseUrl,
                             F_EnglishUrl = document.F_EnglishUrl,
                             F_State = document.F_State,
                             F_IsActivateVip = document.F_IsActivateVip,
                             F_Type = document.F_Type,

                             F_CreatorUserId = document.F_CreatorUserId,
                             F_CreatorTime = document.F_CreatorTime,
                             F_DeleteMark = document.F_DeleteMark,
                             F_DeleteUserId = document.F_DeleteUserId,
                             F_DeleteTime = document.F_DeleteTime,
                             F_LastModifyUserId = document.F_LastModifyUserId,
                             F_LastModifyTime = document.F_LastModifyTime
                         }).ToList();

            var list = list1.Union(list2).ToList();
            list = list.MyDistinct(s => s.F_Id).ToList();
            return list;
        }
         
        public List<DocumentModel> SearchByType(string fileState, string domain, string acquisitionStandard
            , string publisher, string direction,
            int type, int page, int rows, out int total)
        {
            List<DocumentEntity> documentList = new List<DocumentEntity>();

            documentList = documentService.IQueryable().ToList();
            var list = (from document in documentList
                        where document.F_Type == type
                        where document.F_State == (int)Code.Enum.DocumentStatus.已发布
                        where (fileState == "" ? true : document.F_FileState == fileState)



                        //where (publisher == "" ? true : document.F_Publisher.Contains(publisher)  )
                        //where (direction == "" ? true : document.F_Direction.Contains(direction))
                        //where (domain == "" ? true : document.F_Domain.Contains(domain))
                        //where (acquisitionStandard == "" ? true : document.F_AcquisitionStandard.Contains(acquisitionStandard))



                        where (publisher == "" ? true : (document.F_Publisher != null && (document.F_Publisher.Contains(publisher))))
                        where (direction == "" ? true : (document.F_Direction != null && (document.F_Direction.Contains(direction))))
                        where (domain == "" ? true : (document.F_Domain != null && (document.F_Domain.Contains(domain))))
                        where (acquisitionStandard == "" ? true : (document.F_AcquisitionStandard != null && (document.F_AcquisitionStandard.Contains(acquisitionStandard))))



                        where document.F_DeleteMark == false
                        orderby document.F_CreatorTime descending
                        select new DocumentModel()
                        {
                            F_Id = document.F_Id,
                            F_ChineseTitle = document.F_ChineseTitle,
                            F_EnglishTitle = document.F_EnglishTitle,
                            F_FileState = document.F_FileState,
                            F_Domain = document.F_Domain,
                            F_AcquisitionStandard = document.F_AcquisitionStandard,
                            F_Label = document.F_Label,
                            F_Publisher = document.F_Publisher,
                            F_Direction = document.F_Direction,
                            F_ReleaseDate = document.F_ReleaseDate,
                            F_ImplementDate = document.F_ImplementDate,
                            F_HtmlUrl = document.F_HtmlUrl,
                            F_ChineseUrl = document.F_ChineseUrl,
                            F_EnglishUrl = document.F_EnglishUrl,
                            F_State = document.F_State,
                            F_IsActivateVip = document.F_IsActivateVip,
                            F_Type = document.F_Type,

                            F_CreatorUserId = document.F_CreatorUserId,
                            F_CreatorTime = document.F_CreatorTime,
                            F_DeleteMark = document.F_DeleteMark,
                            F_DeleteUserId = document.F_DeleteUserId,
                            F_DeleteTime = document.F_DeleteTime,
                            F_LastModifyUserId = document.F_LastModifyUserId,
                            F_LastModifyTime = document.F_LastModifyTime
                        }).ToList();
            total = list.Count();
            list = list.Skip((page-1)*rows).Take(rows).AsQueryable().ToList();
            return list;
        }

        public List<DocumentModel> SearchDocAll(int page, int rows)
        {
            List<DocumentEntity> documentList = new List<DocumentEntity>();

            documentList = documentService.IQueryable().ToList();
            var list = (from document in documentList
                        where document.F_State == (int)Code.Enum.DocumentStatus.已发布
                        where (document.F_Type == 1 || document.F_Type == 2)
                        where document.F_DeleteMark == false
                        orderby document.F_CreatorTime descending
                        select new DocumentModel()
                        {
                            F_Id = document.F_Id,
                            F_ChineseTitle = document.F_ChineseTitle,
                            F_EnglishTitle = document.F_EnglishTitle,
                            F_FileState = document.F_FileState,
                            F_Domain = document.F_Domain,
                            F_AcquisitionStandard = document.F_AcquisitionStandard,
                            F_Label = document.F_Label,
                            F_Publisher = document.F_Publisher,
                            F_Direction = document.F_Direction,
                            F_ReleaseDate = document.F_ReleaseDate,
                            F_ImplementDate = document.F_ImplementDate,
                            F_HtmlUrl = document.F_HtmlUrl,
                            F_ChineseUrl = document.F_ChineseUrl,
                            F_EnglishUrl = document.F_EnglishUrl,
                            F_State = document.F_State,
                            F_IsActivateVip = document.F_IsActivateVip,
                            F_Type = document.F_Type,

                            F_CreatorUserId = document.F_CreatorUserId,
                            F_CreatorTime = document.F_CreatorTime,
                            F_DeleteMark = document.F_DeleteMark,
                            F_DeleteUserId = document.F_DeleteUserId,
                            F_DeleteTime = document.F_DeleteTime,
                            F_LastModifyUserId = document.F_LastModifyUserId,
                            F_LastModifyTime = document.F_LastModifyTime
                        }).ToList();
            list = list.Skip((page-1)*rows).Take(rows).AsQueryable().ToList();
            return list;
        }


        public List<DocumentModel> SearchDynamicByType(int type, int page, int rows)
        {
            List<DocumentEntity> documentList = new List<DocumentEntity>();

            documentList = documentService.IQueryable().ToList();
            var list = (from document in documentList
                        where document.F_Type == type
                        where document.F_State == (int)Code.Enum.DocumentStatus.已发布
                        where document.F_DeleteMark == false
                        orderby document.F_CreatorTime descending
                        select new DocumentModel()
                        {
                            F_Id = document.F_Id,
                            F_ChineseTitle = document.F_ChineseTitle,
                            F_EnglishTitle = document.F_EnglishTitle,
                            F_FileState = document.F_FileState,
                            F_Domain = document.F_Domain,
                            F_AcquisitionStandard = document.F_AcquisitionStandard,
                            F_Label = document.F_Label,
                            F_Publisher = document.F_Publisher,
                            F_Direction = document.F_Direction,
                            F_ReleaseDate = document.F_ReleaseDate,
                            F_ImplementDate = document.F_ImplementDate,
                            F_HtmlUrl = document.F_HtmlUrl,
                            F_ChineseUrl = document.F_ChineseUrl,
                            F_EnglishUrl = document.F_EnglishUrl,
                            F_State = document.F_State,
                            F_IsActivateVip = document.F_IsActivateVip,
                            F_Type = document.F_Type,

                            F_CreatorUserId = document.F_CreatorUserId,
                            F_CreatorTime = document.F_CreatorTime,
                            F_DeleteMark = document.F_DeleteMark,
                            F_DeleteUserId = document.F_DeleteUserId,
                            F_DeleteTime = document.F_DeleteTime,
                            F_LastModifyUserId = document.F_LastModifyUserId,
                            F_LastModifyTime = document.F_LastModifyTime
                        }).ToList();
            list = list.Skip((page-1)*rows).Take(rows).AsQueryable().ToList();
            return list;
        }
       

        public bool CheckIsHaveNoSubscription(DateTime dateTime)
        {
            return documentService.IQueryable().Any(x => x.F_CreatorTime > dateTime);
        }
        public List<DocumentEntity> GetList(int page, int rows, int type, out int total)
        {
            List<DocumentEntity> documentList = new List<DocumentEntity>();

            documentList = documentService.IQueryable().ToList();
            var list = (from document in documentList
                        where document.F_DeleteMark == false
                        where document.F_State == (int)Code.Enum.DocumentStatus.已发布
                        where document.F_Type == type
                        orderby document.F_CreatorTime descending
                        select new DocumentEntity()
                        {
                            F_Id = document.F_Id,
                            F_ChineseTitle = document.F_ChineseTitle,
                            F_EnglishTitle = document.F_EnglishTitle,
                            F_FileState = document.F_FileState,
                            F_Domain = document.F_Domain,
                            F_AcquisitionStandard = document.F_AcquisitionStandard,
                            F_Label = document.F_Label,
                            F_Publisher = document.F_Publisher,
                            F_Direction = document.F_Direction,
                            F_ReleaseDate = document.F_ReleaseDate,
                            F_ImplementDate = document.F_ImplementDate,
                            F_HtmlUrl = document.F_HtmlUrl,
                            F_ChineseUrl = document.F_ChineseUrl,
                            F_EnglishUrl = document.F_EnglishUrl,
                            F_State = document.F_State,
                            F_IsActivateVip = document.F_IsActivateVip,
                            F_Type = document.F_Type,

                            F_CreatorUserId = document.F_CreatorUserId,
                            F_CreatorTime = document.F_CreatorTime,
                            F_DeleteMark = document.F_DeleteMark,
                            F_DeleteUserId = document.F_DeleteUserId,
                            F_DeleteTime = document.F_DeleteTime,
                            F_LastModifyUserId = document.F_LastModifyUserId,
                            F_LastModifyTime = document.F_LastModifyTime
                        }).ToList();
            list = list.Skip((page-1)*rows).Take(rows).AsQueryable().ToList();
            total = list.Count();
            return list;
        }
        /// <summary>
        /// 资讯查询，带内容
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param> 
        /// <param name="total"></param> 
        /// <returns></returns>
        public List<DocumentContentModel> GetInformationListIncludeContent(int page, int rows, out int total)
        {
            List<DocumentEntity> documentList = new List<DocumentEntity>();
            List<DocumentContentEntity> contentList = new List<DocumentContentEntity>();
            documentList = documentService.IQueryable().ToList();
            contentList = documentContentService.IQueryable().ToList();

            var list = (from document in documentList
                        join content in contentList on document.F_Id equals content.F_DocumentId
                        where document.F_DeleteMark == false
                        where document.F_State == (int)Code.Enum.DocumentStatus.已发布
                        where document.F_Type == (int)Code.Enum.DocumentType.资讯 
                        orderby document.F_CreatorTime descending
                        select new DocumentContentModel()
                        {
                            F_Id = document.F_Id,
                            F_ChineseTitle = document.F_ChineseTitle,
                            F_EnglishTitle = document.F_EnglishTitle,
                            F_Type = document.F_Type,
                            F_DocumentId = document.F_Id,
                            F_ChineseContent = content.F_ChineseContent,
                            F_EnglishContent = content.F_EnglishContent,
                            F_CreatorTime =document.F_CreatorTime,
                            F_Label = document.F_Label,
                            F_FileState = document.F_FileState,
                            F_Domain = document.F_Domain,
                            F_AcquisitionStandard = document.F_AcquisitionStandard,
                            F_Publisher = document.F_Publisher,
                            F_Direction = document.F_Direction,
                            F_ReleaseDate = document.F_ReleaseDate,
                            F_ImplementDate = document.F_ImplementDate

                        }).ToList();
            total = list.Count();
            list = list.Skip((page-1)*rows).Take(rows).AsQueryable().ToList();
            return list;
        }

        public List<DocumentEntity> GetNewestList(int page, int rows, out int total)
        {
            List<DocumentEntity> documentList = new List<DocumentEntity>();

            documentList = documentService.IQueryable().ToList();
            var list = (from document in documentList
                        where document.F_DeleteMark == false
                        where document.F_State == (int)Code.Enum.DocumentStatus.已发布
                        where (document.F_Type == (int)Code.Enum.DocumentType.标准 || document.F_Type == (int)Code.Enum.DocumentType.法规)
                        orderby document.F_CreatorTime descending
                        select new DocumentEntity()
                        {
                            F_Id = document.F_Id,
                            F_ChineseTitle = document.F_ChineseTitle,
                            F_EnglishTitle = document.F_EnglishTitle,
                            F_FileState = document.F_FileState,
                            F_Domain = document.F_Domain,
                            F_AcquisitionStandard = document.F_AcquisitionStandard,
                            F_Label = document.F_Label,
                            F_Publisher = document.F_Publisher,
                            F_Direction = document.F_Direction,
                            F_ReleaseDate = document.F_ReleaseDate,
                            F_ImplementDate = document.F_ImplementDate,
                            F_HtmlUrl = document.F_HtmlUrl,
                            F_ChineseUrl = document.F_ChineseUrl,
                            F_EnglishUrl = document.F_EnglishUrl,
                            F_State = document.F_State,
                            F_IsActivateVip = document.F_IsActivateVip,
                            F_Type = document.F_Type,

                            F_CreatorUserId = document.F_CreatorUserId,
                            F_CreatorTime = document.F_CreatorTime,
                            F_DeleteMark = document.F_DeleteMark,
                            F_DeleteUserId = document.F_DeleteUserId,
                            F_DeleteTime = document.F_DeleteTime,
                            F_LastModifyUserId = document.F_LastModifyUserId,
                            F_LastModifyTime = document.F_LastModifyTime
                        }).ToList();
            total = list.Count();
            list = list.Skip((page-1)*rows).Take(rows).AsQueryable().ToList();
            return list;
        }

    }
}
