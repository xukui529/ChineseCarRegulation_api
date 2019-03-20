using NFine.Application.AuxiliaryManage;
using NFine.Application.SystemManage;
using NFine.Code;
using NFine.Domain.Entity.AuxiliaryManage;
using NFine.Domain.Entity.SystemManage;
using NFine.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Software.Web.Areas.SystemManage.Controllers
{
    [HandlerLogin]
    public class MemberUpgradeApplyController : Controller
    {
        private MemberApp memberApp = new MemberApp();
        private MemberDownloadApp memberDownloaApp = new MemberDownloadApp();
        private MemberUpgradeApplyApp memberUpgradeApplyApp = new MemberUpgradeApplyApp();
        [HttpGet]
        public ActionResult Index(string keyword, int page = 1, int take = 10)
        {
            return View();
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public JsonResult GetGridJson(string keyword, int status, int limit, int offset)
        {
            int totalCount;
            Pagination pagination = new Pagination();
            pagination.page = (offset / limit) + 1;
            pagination.rows = limit;
            pagination.sord = "desc";
            pagination.sidx = "F_CreatorTime";
            IList<MemberUpgradeApplyModel> entityList;
            if (status==1|| status == 2)
            { 
                entityList = memberUpgradeApplyApp.GetDataList(offset, limit,out totalCount, keyword, status);

            }
            else
            {
                entityList = memberUpgradeApplyApp.GetDataList(offset, limit, out totalCount, keyword);
            }

            totalCount = entityList.Count();
            var pageSize = entityList.ToList();
            return Json(new { total = totalCount, rows = pageSize }, JsonRequestBehavior.AllowGet);
        }


        [HandlerAuthorize]
        public ActionResult CreateModel(string keyValue)
        {
            ViewData["id"] = string.Empty; 
            MemberUpgradeApplyEntity entity = new MemberUpgradeApplyEntity();
           
            if (!string.IsNullOrEmpty(keyValue))
            {
                ViewData["id"] = keyValue; 
                entity = memberUpgradeApplyApp.GetForm(keyValue);
            }
            return View(entity);
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult Update(DateTime startTime, DateTime endTime, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                MemberUpgradeApplyEntity model = memberUpgradeApplyApp.GetForm(keyValue);
                if (model != null)
                {
                    MemberEntity member = memberApp.GetForm(model.F_MemberId);
                    if (member != null)
                    {
                        //要修改会员信息：会员的状态--改为高级会员 、会员级别-按季度 或者年度
                        //下载次数 要新增 季度15  年度60
                        member.F_Country = model.F_Country;//国家
                        member.F_Account = model.F_Account;//金额
                        member.F_HYType = (int)model.F_HYType;//会员类型
                        member.F_Phone = model.F_Phone;//电话

                        MemberDownloadEntity   entity = memberDownloaApp.GetFormEntity();
                        //下载次数
                        if (member.F_HYType == 1)
                        {
                            if (entity != null)
                            {
                                if (entity.F_EffectiveDate > DateTime.Now)
                                {
                                    member.F_DownAndPrintCount = member.F_DownAndPrintCount + entity.F_OldSeasonNumber;
                                }
                                else
                                {
                                    member.F_DownAndPrintCount = member.F_DownAndPrintCount + entity.F_SeasonNumber;
                                }
                            }
                            //季度  
                        }
                        if (member.F_HYType == 2)
                        {
                            if (entity != null)
                            {
                                if (entity.F_EffectiveDate > DateTime.Now)
                                {
                                    member.F_DownAndPrintCount = member.F_DownAndPrintCount + entity.F_OldYearNumber;
                                }
                                else
                                {
                                    member.F_DownAndPrintCount = member.F_DownAndPrintCount + entity.F_YearNumber;
                                }
                            }
                            //年度 
                        }
                        //首次 多次
                        if (member.F_State == 1)
                        {
                            //普通会员  首次开通
                            member.F_TimesState = 1;
                        }
                        else if (member.F_TimesState == 1)
                        {//已经是首次开通 的 改为多次开通
                            member.F_TimesState = 2;
                        }
                        member.F_State = 3;//高级会员
                        member.F_UseState = 1;//正常
                        member.F_StartDate = startTime;
                        member.F_EndDate = endTime;
                        //开始日期 ==开通日期  结束日期 要计算
                        memberApp.SubmitForm(member, member.F_Id);
                    }
                    memberUpgradeApplyApp.UpdateState(keyValue, 2);//2审核通过
                } 
            }
           
            return Content("操作成功");
        }

        //public ActionResult UpdateState(DateTime startTime, DateTime endTime,string keyValue)
        //{
        //    if (!string.IsNullOrEmpty(keyValue))
        //    {
        //        MemberUpgradeApplyEntity model = memberUpgradeApplyApp.GetForm(keyValue);
        //        if (model != null)
        //        {
        //            MemberEntity member = memberApp.GetForm(model.F_MemberId);
        //            if (member != null)
        //            {
        //                //要修改会员信息：会员的状态--改为高级会员 、会员级别-按季度 或者年度
        //                //下载次数 要新增 季度15  年度60
        //                member.F_Country = model.F_Country;//国家
        //                member.F_Account = model.F_Account;//金额
        //                member.F_HYType = (int)model.F_HYType;//会员类型
        //                member.F_Phone = model.F_Phone;//电话
        //                //下载次数
        //                if (member.F_HYType == 1)
        //                {
        //                    //季度 
        //                    member.F_DownAndPrintCount = member.F_DownAndPrintCount + 15;
        //                }
        //                if (member.F_HYType == 2)
        //                {
        //                    //年度
        //                    member.F_DownAndPrintCount = member.F_DownAndPrintCount + 60;
        //                }
        //                //首次 多次
        //                if(member.F_State == 1)
        //                {
        //                    //普通会员  首次开通
        //                    member.F_TimesState = 1;
        //                }
        //                else if(member.F_TimesState==1)
        //                {//已经是首次开通 的 改为多次开通
        //                    member.F_TimesState = 2;
        //                }
        //                member.F_State = 3;//高级会员
        //                member.F_UseState = 1;//正常
        //                //member.F_StartDate = GetStartTime(member.F_StartDate, member.F_EndDate, member.F_HYType, member.F_UseState);
        //                //member.F_EndDate = GetEndTime(member.F_StartDate, member.F_EndDate, member.F_HYType, member.F_UseState);
        //                //开始日期 ==开通日期  结束日期 要计算
        //                memberApp.SubmitForm(member, member.F_Id);
        //            }
        //            memberUpgradeApplyApp.UpdateState(keyValue, 2);//2审核通过
        //        } 
        //        return Content("操作成功");
        //    }
        //    else
        //    {
        //        return Content("操作失败");
        //    }
        //}
        ///// <summary>
        ///// 根据日期 类型 获取结束日期
        ///// </summary>
        ///// <param name="beginTime"></param>
        ///// <param name="endTime"></param>
        ///// <param name="type"></param>
        ///// <returns></returns>
        //private DateTime GetEndTime(DateTime? startTime, DateTime? endTime, int hYType, int useState)
        //{
        //    DateTime time = DateTime.Now;
        //    if (endTime != null)
        //    {
        //        time = (DateTime)endTime;
        //    } 
           
        //    if (useState != 1)
        //    {
        //        //不正常 超期 逾期 即将逾期 开通日期要修改未当前日期
        //        time =new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day); 
               
        //    }
        //    int oldDay = time.Day;
        //    if (hYType == 1)
        //    {
        //        //季度 
        //        time = time.AddMonths(3);
        //    }
        //    if (hYType == 2)
        //    {
        //        //年度 
        //        time = time.AddMonths(12);
        //    }
        //    if(time.Day!= oldDay)
        //    {
        //        //日期改变后不相等，说明遇到2月份了，就不减一天了
        //    }
        //    else
        //    {
        //        time = time.AddDays(-1);
        //    }
        //    return time;
        //}
        //private DateTime GetStartTime(DateTime? startTime, DateTime? endTime, int hYType, int useState)
        //{
        //    DateTime time = DateTime.Now;
        //    if (startTime != null)
        //    {
        //        time = (DateTime)startTime;
        //    } 
        //    if (useState != 1)
        //    {
        //        //不正常 超期 逾期 即将逾期 开通日期要修改未当前日期
        //        time = DateTime.Now; 
        //    }
        //    else
        //    {
        //        //正常 开通日期不变，只修改结束日期   添加3个月 
        //    }
        //    return time;
        //}
    }
}