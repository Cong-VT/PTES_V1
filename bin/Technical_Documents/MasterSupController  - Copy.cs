
using eSales.Service.Models;
using eSales.Service.Utils;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace eSales.Service.Controllers
{
    public class MasterSupController : ApiController
    {
        private AppEntities _app = DbHelper.CreateObjectContext<AppEntities>(false);
        private static ILog log = LogManager.GetLogger(typeof(MasterSupController));

        public MasterSupController()
        {
            _app.Database.CommandTimeout = 600;
        }
      
      
        public CustomerSupSync GetCustomer(string userName, string branchID, int version, string sessionID)
        {
            try
            {
                CustomerSupSync customer = new CustomerSupSync();
                customer.Salespersons = _app.PPC_GetAR_Salesperson(branchID, userName, version, sessionID).ToList();
                customer.RouteDets = _app.PPC_GetOM_SalesRouteDetSup(branchID, userName, version, sessionID).ToList();
                customer.Customers = _app.PPC_GetAR_CustomerSup(branchID, userName, version, sessionID).ToList();
                customer.Addresses = _app.PPC_GetAR_SOAddressV2(branchID, userName, version, sessionID).ToList();
                customer.BillAddresses = _app.PPC_GetAR_BillAddress(branchID, userName, version, sessionID).ToList();
                customer.RegisterSalesRoute = _app.PPC_GetPPC_RegisterSalesRouteSup(branchID, userName, version, sessionID).ToList();
                return customer;
            }
            catch (Exception ex)
            {
                throw Request.CreateResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
          
        }

        public TrainingSync GetTraining(string userName, string branchID, int version, string sessionID,String date)
        {
            try
            {
                TrainingSync training = new TrainingSync();
                training.CriteriaDetailt = _app.PPC_GetCO_CriteriaDetail(branchID, userName, version, sessionID).ToList();
                training.CriteriaHeader = _app.PPC_GetCO_CriteriaHeader(branchID, userName, version, sessionID).ToList();
                training.PlanHeader = _app.PPC_GetCO_PlanHeader(branchID, userName, version, sessionID, Convert.ToDateTime(date)).ToList();
                training.PlanSaleperson = _app.PPC_GetCO_PlanSalePerson(branchID, userName, version, sessionID).ToList();
                training.PlanTarget = _app.PPC_GetCO_PlanTarget(branchID, userName, version, sessionID).ToList();
                training.ResultTraining = _app.PPC_GetCO_ResultTraining(branchID, userName, version, sessionID).ToList();
                return training;
            }
            catch (Exception ex)
            {
                throw Request.CreateResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }

        }

        public ReportSync GetReport(string userName, string branchID, int version, string sessionID, string fromMonth, string toMonth)
        {
            try
            {
                ReportSync report = new ReportSync();
                report.Rptmonthlytotal = _app.PPC_GetRPT_MonthlyTotal(branchID, userName, version, sessionID, fromMonth, toMonth).ToList();
                report.Rptmonthlyproduct = _app.PPC_GetRPT_MonthlyProduct(branchID, userName, version, sessionID, fromMonth, toMonth).ToList();
                report.Rptmonthlycustomer = _app.PPC_GetRPT_MonthlyCustomer(branchID, userName, version, sessionID, fromMonth, toMonth).ToList();
                return report;
            }
            catch (Exception ex)
            {
                throw Request.CreateResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }

        }
    
        public ReportSupSync GetReportSup(string userName, string branchID, int version, string sessionID, string fromMonth, string toMonth)
        {
            try
            {
                ReportSupSync report = new ReportSupSync();
                report.MonthlyTotalSups = _app.PPC_GetRPT_MonthlyTotalSup(branchID, userName, version, sessionID, fromMonth, toMonth).ToList();
                report.MonthlyProductSups = _app.PPC_GetRPT_MonthlyProductSup(branchID, userName, version, sessionID, fromMonth, toMonth).ToList();
                report.MonthlyCustomerSups = _app.PPC_GetRPT_MonthlyCustomerSup(branchID, userName, version, sessionID, fromMonth, toMonth).ToList();
                return report;
            }
            catch (Exception ex)
            {
                throw Request.CreateResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }

        }

        public SupportTaskSync getSupportTask(string userName, string branchID, int version, string sessionID, int dateSync)
        {
            try
            {
                SupportTaskSync report = new SupportTaskSync();
                report.SupportTasks = _app.PPC_GetSupportTask(branchID, userName, version, sessionID, dateSync).ToList();
                return report;
            }
            catch (Exception ex)
            {
                throw Request.CreateResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }

        } 
        // support build cu
        public GeneralSync GetGeneral(string userName, string branchID, [FromUri] List<TableVersion> tableVersions, string sessionID)
        {
            return GetGeneral(userName, branchID, tableVersions, sessionID, "vi");
        }
        public GeneralSync GetGeneral(string userName, string branchID, [FromUri] List<TableVersion> tableVersions, string sessionID, string langID)
        {
            try
            {
                GeneralSync general = new GeneralSync();

                general.BranchSettings = _app.PPC_GetBranchSettingSup(branchID, userName, 0, sessionID).ToList();
                general.Channels = _app.PPC_GetAR_Channel(branchID, userName, GetVersion("AR_Channel", tableVersions), sessionID).ToList();
                general.ShopTypes = _app.PPC_GetAR_ShopTypeV2(branchID, userName, GetVersion("AR_ShopType", tableVersions), sessionID).ToList();
                general.Taxes = //_app.PPC_GetSI_TaxV2(branchID, userName, GetVersion("SI_Tax", tableVersions), sessionID).ToList();           
                general.Territories = _app.PPC_GetAR_Territory(branchID, userName, GetVersion("AR_Territory", tableVersions), sessionID).ToList();
                general.States = _app.PPC_GetSI_StateV2(branchID, userName, GetVersion("SI_State", tableVersions), sessionID).ToList();
                general.Districts = _app.PPC_GetSI_DistrictV2(branchID, userName, GetVersion("SI_District", tableVersions), sessionID).ToList();
                general.DisplayLocations = //_app.PPC_GetOM_DisplayLocation(branchID, userName, GetVersion("OM_DisplayLocation", tableVersions), sessionID).ToList();
                general.Displays = //_app.PPC_GetOM_DisplaySup(branchID, userName, GetVersion("OM_Display", tableVersions), sessionID).ToList();
                general.DisplayCusts = //_app.PPC_GetOM_DisplayCustSup(branchID, userName, GetVersion("OM_DisplayCust", tableVersions), sessionID).ToList();
                general.Advertises = _app.PPC_GetOM_AdvertiseV2(branchID, userName, GetVersion("OM_Advertise", tableVersions), sessionID).ToList();
                //TODO Xóa khi build mới hơn
                general.Reasons = _app.PPC_GetPPC_ReasonCode(branchID, userName, GetVersion("PPC_ReasonCode", tableVersions), sessionID, langID).ToList();
                general.CustClasses = _app.PPC_GetAR_CustClassV2(branchID, userName, GetVersion("AR_CustClass", tableVersions), sessionID).ToList();
                general.Locations = _app.PPC_GetAR_Location(branchID, userName, GetVersion("AR_Location", tableVersions), sessionID).ToList();
                // Begin Sync Dashboard ---------------------------------
                general.DashboardKpis = _app.PPC_GetPPC_DashboardKpi(branchID, userName, GetVersion("PPC_DashboardKpi", tableVersions), sessionID, langID).ToList();
                general.DashboardDows = _app.PPC_GetPPC_DashboardDow(branchID, userName, GetVersion("PPC_DashboardDow", tableVersions), sessionID, langID).ToList();
                general.DashboardWeeks = _app.PPC_GetPPC_DashboardWeek(branchID, userName, GetVersion("PPC_DashboardWeek", tableVersions), sessionID, langID).ToList();
                general.DashboardMetros = _app.PPC_GetPPC_DashboardMetro(branchID, userName, GetVersion("PPC_DashboardMetro", tableVersions), sessionID, langID).ToList();
                // End Sync Dashboard ------------------------------------------
                general.SlsFreq = _app.PPC_GetPPC_SlsFreq(branchID, userName, GetVersion("PPC_SlsFreq", tableVersions), sessionID, langID).ToList();
                general.WeekofVisitMCL = _app.PPC_GetPPC_WeekofVisitMCL(branchID, userName, GetVersion("PPC_WeekofVisitMCL", tableVersions), sessionID).ToList();
                general.SalesDetailHistories = //_app.PPC_GetPPC_SalesDetailHistory(branchID, userName, GetVersion("PPC_SalesDetailHistory", tableVersions), sessionID).ToList();
                general.SalesHeaderHistories = //_app.PPC_GetPPC_SalesHeaderHistory(branchID, userName, GetVersion("PPC_SalesHeaderHistory", tableVersions), sessionID).ToList();
                general.ProgramSalesItems = //_app.PPC_GetPPC_ProgramSalesItem(branchID, userName, GetVersion("PPC_ProgramSalesItem", tableVersions), sessionID).ToList();
                general.SalesOrdHistories = //_app.PPC_GetOM_SalesOrdHistorySup(branchID, userName, GetVersion("OM_SalesOrdHistory", tableVersions), sessionID).ToList();
                general.SalesOrdDetHistories = //_app.PPC_GetOM_SalesOrdDetHistorySup(branchID, userName, GetVersion("OM_SalesOrdDetHistory", tableVersions), sessionID).ToList();
                general.StockOutletHistories = //_app.PPC_GetPPC_StockOutletHistory(branchID, userName, GetVersion("PPC_StockOutletHistory", tableVersions), sessionID).ToList();
                general.PictureTypeNewCustomers = //_app.PPC_GetPPC_PictureTypeNewCustomer(branchID, userName, GetVersion("PPC_PictureTypeNewCustomer", tableVersions), sessionID).ToList();
                general.PosmCustomers = //_app.PPC_GetPPC_PosmCustomer(branchID, userName, GetVersion("PPC_PosmCustomer", tableVersions), sessionID).ToList();
                general.SalesDisplayHistory = //_app.PPC_GetPPC_SalesDisplayHistory(branchID, userName, GetVersion("PPC_SalesDisplayHistory", tableVersions), sessionID).ToList();           
                general.Areas = _app.PPC_GetAR_Area(branchID, userName, GetVersion("AR_Area", tableVersions), sessionID).ToList();
                general.SubTerritories = _app.PPC_GetSI_SubTerritory(branchID, userName, GetVersion("SI_SubTerritory", tableVersions), sessionID).ToList();
                general.OutSideCheckings = _app.PPC_GetPPC_OutSideCheckingSup(branchID, userName, GetVersion("PPC_OutSideChecking", tableVersions), sessionID).ToList();
                general.Wards = _app.PPC_GetSI_Ward(branchID, userName, GetVersion("SI_Ward", tableVersions), sessionID).ToList();
                general.CustomerSalesDetHistories = //_app.PPC_GetRPT_CustomerSalesDetHistory(branchID, userName, GetVersion("CustomerSalesDetHistory", tableVersions), sessionID, langID).ToList();
                general.CustomerSalesOrdHistories = //_app.PPC_GetRPT_CustomerSalesOrdHistory(branchID, userName, GetVersion("CustomerSalesOrdHistory", tableVersions), sessionID, langID).ToList();
                general.PriceClasses = //_app.PPC_GetOM_PriceClassV2(branchID, userName, GetVersion("OM_PriceClass", tableVersions), sessionID).ToList();
                general.Notifies = _app.PPC_GetPPC_Notify(branchID, userName, GetVersion("PPC_Notify", tableVersions), sessionID, langID).ToList();
                general.ProgramStamps = _app.PPC_GetOM_ProgramStamp(branchID, userName, GetVersion("OM_ProgramStamp", tableVersions), sessionID).ToList();
                general.ProgramStampDetails = _app.PPC_GetOM_ProgramStampDetail(branchID, userName, GetVersion("OM_ProgramStampDetail", tableVersions), sessionID).ToList();
                general.Albums = //_app.PPC_GetOM_Album(branchID, userName, GetVersion("OM_Album", tableVersions), sessionID).ToList();
                general.DisplayRewards = //_app.PPC_GetOM_DisplayReward(branchID, userName, GetVersion("OM_DisplayReward", tableVersions), sessionID).ToList();
                general.DiscBreakQuotas = //_app.PPC_GetOM_DiscBreakQuota(branchID, userName, GetVersion("OM_DiscBreakQuota", tableVersions), sessionID).ToList();
                general.LeaveShifts = //_app.PPC_GetLeaveShift(branchID, userName, GetVersion("PPC_LeaveShift", tableVersions), sessionID).ToList();
                general.Sizes = //_app.PPC_GetSI_Size(branchID, userName, GetVersion("SI_Size", tableVersions), sessionID).ToList();
                general.TimeKeeping = _app.PPC_GetTimeKeeping(branchID, userName, GetVersion("PPC_TimeKeeping", tableVersions), sessionID).ToList();
                general.MustStocks = _app.PPC_GetPPC_MustStock(branchID, userName, GetVersion("PPC_MustStock", tableVersions), sessionID).ToList();
                general.Stand = //_app.PPC_GetSI_Stand(branchID, userName, GetVersion("SI_Stand", tableVersions), sessionID).ToList();
                general.Brand = //_app.PPC_GetSI_Brand(branchID, userName, GetVersion("SI_Brand", tableVersions), sessionID).ToList();
                general.Display = //_app.PPC_GetSI_Display(branchID, userName, GetVersion("SI_Display", tableVersions), sessionID).ToList();
                general.Markets = //_app.PPC_GetSI_Market(branchID, userName, GetVersion("SI_Market", tableVersions), sessionID).ToList();
                general.InSite = _app.PPC_GetIN_Site_Stock(branchID, userName, GetVersion("IN_Site", tableVersions), sessionID).ToList();
                general.WorkStations = //_app.PPC_GetOM_WorkStation(branchID, userName, GetVersion("OM_WorkStation", tableVersions), sessionID).ToList();
                general.WorkingPlans = //_app.PPC_GetOM_WorkingPlan(branchID, userName, GetVersion("OM_WorkingPlan", tableVersions), sessionID).ToList();
                general.Hierarchy = _app.PPC_GetSI_HierarchySup(branchID, userName, GetVersion("SI_Hierarchy", tableVersions), sessionID).ToList();
                 // Begin Sync Work With ---------------------------------
				general.SalesPersonTraining = _app.PPC_GetSalesPersonTraining(branchID, userName, GetVersion("AR_Salesperson", tableVersions), sessionID).ToList();
                general.WorkWithCriteriaHeader = _app.PPC_GetOM_WorkWithCriteriaHeader(branchID, userName, GetVersion("OM_WorkWithCriteriaHeader", tableVersions), sessionID).ToList();
                general.WorkWithCriteriaDetail = _app.PPC_GetOM_WorkWithCriteriaDetail(branchID, userName, GetVersion("OM_WorkWithCriteriaDetail", tableVersions), sessionID).ToList();
                general.TargetBusinessMonthTraining = _app.PPC_GetTargetBusinessMonth(branchID, userName, 0, sessionID).ToList();
                general.TrainingHistory = _app.PPC_GetTrainingHistory(branchID, userName, 0, sessionID).ToList();
                general.TrainingHistoryResult = _app.PPC_GetTrainingHistoryResult(branchID, userName, 0, sessionID).ToList();
                general.TrainingPrepareAppraise = _app.PPC_GetTrainingPrepareAppraise(branchID, userName, 0, sessionID).ToList();
                general.TrainingTargetDay = _app.PPC_GetTrainingTargetDay(branchID, userName, 0, sessionID).ToList();
                general.WorkWithStep = _app.PPC_GetPPC_WorkWithStep(branchID, userName, GetVersion("PPC_WorkWithStep", tableVersions), sessionID).ToList();
                general.WorkWithKPI = _app.PPC_GetOM_WorkWithKPI(branchID, userName, GetVersion("OM_WorkWithKPI", tableVersions), sessionID).ToList();
                general.WorkWithMSL = _app.PPC_GetOM_WorkWithMSL(branchID, userName, GetVersion("OM_WorkWithMSL", tableVersions), sessionID).ToList();
                general.WorkWithChance = _app.PPC_GetOM_WorkWithChance(branchID, userName, GetVersion("OM_WorkWithChance", tableVersions), sessionID).ToList();
                general.WorkWithTypeFinish = _app.PPC_GetOM_WorkWithTypeFinish(branchID, userName, GetVersion("OM_WorkWithTypeFinish", tableVersions), sessionID).ToList();
                general.WorkWithSummaryChance = _app.PPC_GetOM_WorkWithSummaryChance(branchID, userName, GetVersion("OM_WorkWithSummaryChance", tableVersions), sessionID).ToList();
                //DMS -> SFA
                general.TrainingPrepareAppraiseResults = _app.PPC_GetTrainingPrepareAppraiseResult(branchID, userName, GetVersion("PPC_TrainingPrepareAppraise", tableVersions), sessionID).ToList();
                general.TrainingTargetDayResults = _app.PPC_GetTrainingTargetDayResult(branchID, userName, GetVersion("PPC_TrainingTargetDayResult", tableVersions), sessionID).ToList();
                general.WorkWithChanceResults = _app.PPC_GetPPC_WorkWithChanceResult(branchID, userName, GetVersion("PPC_WorkWithChanceResult", tableVersions), sessionID).ToList();
                general.WorkWithChanceStatusPresentResults = _app.PPC_GetWorkWithChanceStatusPresentResult(branchID, userName, GetVersion("PPC_WorkWithChanceStatusPresentResult", tableVersions), sessionID).ToList();
                general.WorkWithCriteriaHeaderResults = _app.PPC_GetWorkWithCriteriaHeaderResult(branchID, userName, GetVersion("PPC_WorkWithCriteriaHeaderResult", tableVersions), sessionID).ToList();
                general.WorkWithCriteriaDetailResults = _app.PPC_GetWorkWithCriteriaDetailResult(branchID, userName, GetVersion("PPC_WorkWithCriteriaDetailResult", tableVersions), sessionID).ToList();
                general.WorkWithCriteriaRateResults = _app.PPC_GetWorkWithCriteriaRateResult(branchID, userName, GetVersion("PPC_WorkWithCriteriaRateResult", tableVersions), sessionID).ToList();
                general.WorkWithDetermineTargetResults = _app.PPC_GetWorkWithDetermineTargetResult(branchID, userName, GetVersion("PPC_WorkWithDetermineTargetResults", tableVersions), sessionID).ToList();
                general.WorkWithDisplayResults = _app.PPC_GetWorkWithDisplayResult(branchID, userName, GetVersion("PPC_WorkWithDisplayResult", tableVersions), sessionID).ToList();
                general.WorkWithFinishs = _app.PPC_GetWorkWithFinish(branchID, userName, GetVersion("PPC_WorkWithFinish", tableVersions), sessionID).ToList();
                general.WorkWithKPIResults = _app.PPC_GetWorkWithKPIResult(branchID, userName, GetVersion("PPC_WorkWithKPIResult", tableVersions), sessionID).ToList();
                general.WorkWithMSLResults = _app.PPC_GetWorkWithMSLResult(branchID, userName, GetVersion("PPC_WorkWithMSLResult", tableVersions), sessionID).ToList();
                general.WorkWithOrderResults = _app.PPC_GetWorkWithOrderResult(branchID, userName, GetVersion("PPC_WorkWithOrderResult", tableVersions), sessionID).ToList();
                general.WorkWithRemarkResults = _app.PPC_GetWorkWithRemarkResult(branchID, userName, GetVersion("PPC_WorkWithRemarkResult", tableVersions), sessionID).ToList();      
                general.WorkWithSummaryChanceResults = _app.PPC_GetWorkWithSummaryChanceResult(branchID, userName, GetVersion("PPC_WorkWithSummaryChanceResult", tableVersions), sessionID).ToList();
                general.WorkWithSummaryKPIResults = _app.PPC_GetWorkWithSummaryKPIResult(branchID, userName, GetVersion("PPC_WorkWithSummaryKPIResult", tableVersions), sessionID).ToList();
                general.WorkWithTypeFinishResults = _app.PPC_GetWorkWithTypeFinishResult(branchID, userName, GetVersion("PPC_WorkWithTypeFinishResult", tableVersions), sessionID).ToList();

                general.TaskLists = _app.PPC_GetOM_TaskList(branchID, userName, GetVersion("OM_TaskList", tableVersions), sessionID).ToList();
                general.WorkSchedules = _app.PPC_GetOM_WorkSchedule(branchID, userName, GetVersion("OM_WorkSchedule", tableVersions), sessionID).ToList();
                general.WorkScheduleDetails = _app.PPC_GetOM_WorkScheduleDetail(branchID, userName, GetVersion("OM_WorkScheduleDetail", tableVersions), sessionID).ToList();
                general.OmWorkWithCategory = _app.PPC_GetOM_WorkWithCategory(branchID, userName, GetVersion("OM_WorkWithCategory", tableVersions), sessionID).ToList();
                general.OmWorkWithCriteria = _app.PPC_GetOM_WorkWithCriteria(branchID, userName, GetVersion("OM_WorkWithCriteria", tableVersions), sessionID).ToList();

                // End Sync Work With ------------------------------------------
                general.Accumulated = //_app.PPC_GetOM_AccumulatedSup(branchID, userName, GetVersion("OM_Accumulated", tableVersions), sessionID).ToList();
                general.AccumulatedRegis = //_app.PPC_GetOM_AccumulatedRegisSup(branchID, userName, GetVersion("OM_AccumulatedRegis", tableVersions), sessionID).ToList();
                general.BudgetTrades = //_app.PPC_GetOM_BudgetTrade(branchID, userName, GetVersion("OM_BudgetTrade", tableVersions), sessionID).ToList();
                general.CheckItemLocs = //_app.PPC_GetPPC_CheckItemLoc(branchID, userName, GetVersion("PPC_CheckItemLoc", tableVersions), sessionID).ToList();
                general.CheckItemLocDets = //_app.PPC_GetPPC_CheckItemLocDet(branchID, userName, GetVersion("PPC_CheckItemLocDet", tableVersions), sessionID).ToList();

                _app.PPC_SyncClear(branchID, userName, 0, sessionID);

                return general;
            }
            catch (Exception ex)
            {
                throw Request.CreateResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }

        }
        public int GetVersion(string name, List<TableVersion> tableVersions)
        {
            TableVersion tableVersion = tableVersions.FirstOrDefault(p => p.Name.ToLower() == name.ToLower());
            if (tableVersion != null) return tableVersion.Version;
            return 0;
        }

        [HttpPost]
        public WorkingPlanScheduleSync SyncGetWorkingPlanSchedule(string userName, string branchID, int version, string sessionID, [FromBody] WorkingPlanScheduleSyncEnd data)
        {
            try
            {
                _app.Configuration.AutoDetectChangesEnabled = false;
                _app.Configuration.EnsureTransactionsForFunctionsAndCommands = false;

                if (data.WorkingPlans != null)
                {
                    foreach (var item in data.WorkingPlans)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;
                        _app.PPC_SyncWorkingPlan.Add(item);
                    }
                }

                _app.SaveChanges();

                _app.PPC_ProcessSyncEnd_WorkingPlan(branchID, userName, sessionID);

                WorkingPlanScheduleSync workingPlan = new WorkingPlanScheduleSync();
                workingPlan.WorkingPlans = _app.PPC_GetOM_WorkingPlan(branchID, userName, version, sessionID).ToList();
                return workingPlan;
            }
            catch (DbEntityValidationException ex)
            {
                string message = "";
                foreach (var info in ex.EntityValidationErrors)
                {
                    message += String.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:\n",
                        info.Entry.Entity.GetType().Name, info.Entry.State);
                    foreach (var ve in info.ValidationErrors)
                    {
                        message += string.Format("- Property: \"{0}\", Error: \"{1}\"\n",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                log.Error(ex);
                throw Request.CreateResponse(HttpStatusCode.InternalServerError, message);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw Request.CreateResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }
        //support build cu
        [HttpPost]
        public bool SyncEnd(string userName, string branchID, string sessionID, [FromBody] EndSupSync data)
        {
            SyncEndResult(userName, branchID, sessionID, data, "vi");
            return true;
        }
        [HttpPost]
        public EndSyncResult SyncEndResult(string userName, string branchID, string sessionID,[FromBody] EndSupSync data, string langID)
        {
            try
            {
                _app.Configuration.AutoDetectChangesEnabled = false;
                _app.Configuration.EnsureTransactionsForFunctionsAndCommands = false;

                foreach (var item in data.OutsideCheckings)
                {
                    item.SessionID = sessionID;
                    item.BranchID = branchID;
                    item.SlsperID = userName;
                    item.Note = item.Note ?? "";
                    item.DeviationType = item.DeviationType ?? "";
                    item.ShiftID = item.ShiftID ?? "";
                    if (!string.IsNullOrEmpty(item.ImageName)) {
                        item.ImageName = branchID + @"/" + userName + @"/" + Convert.ToDateTime(item.VisitDate).ToString("yyyyMM") + @"/" + item.ImageName;
                    }                        

                    _app.PPC_SyncOutsideChecking.Add(item);
                }

                foreach (var item in data.Orders)
                {
                    item.SessionID = sessionID;
                    item.BranchID = branchID;

                    item.FieldString1 = item.FieldString1 ?? "";
                    item.FieldString2 = item.FieldString2 ?? "";
                    item.FieldString3 = item.FieldString3 ?? "";

                    item.FieldDate1 = item.FieldDate1.Year == 1 ? DateTime.Now : item.FieldDate1;
                    item.FieldDate2 = item.FieldDate2.Year == 1 ? DateTime.Now : item.FieldDate2;
                    item.FieldDate3 = item.FieldDate3.Year == 1 ? DateTime.Now : item.FieldDate3;

                    item.BillToID = item.BillToID ?? "";
                    if (item.Remark.Length > 200)
                        item.Remark = item.Remark.Substring(0, 200);//Cắt 200 ký tự
                    else
                        item.Remark = item.Remark;

                    _app.PPC_SyncSalesOrd.Add(item);
                }

                foreach (var item in data.OrderDetails)
                {
                    item.SessionID = sessionID;
                    item.BranchID = branchID;

                    item.DisplayID = item.DisplayID ?? "";
                    item.DisplayPeriodID = item.DisplayPeriodID ?? "";
                    item.KitLineRef = item.KitLineRef ?? "";
                    item.AccumulateID = item.AccumulateID ?? "";

                    _app.PPC_SyncSalesOrdDet.Add(item);
                }

                foreach (var item in data.OrderDiscs)
                {
                    item.SessionID = sessionID;
                    item.BranchID = branchID;

                    _app.PPC_SyncOrdDisc.Add(item);
                }

                if (data.OrderDiscItemApplies != null)
                {
                    foreach (var item in data.OrderDiscItemApplies)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;

                        _app.PPC_SyncOrdDiscItemApply.Add(item);
                    }
                }


                if (data.OrderManualDiscs != null)
                {
                    foreach (var item in data.OrderManualDiscs)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;

                        _app.PPC_SyncOrdManualDisc.Add(item);
                    }
                }


                if (data.OrderAccumulates != null)
                {
                    foreach (var item in data.OrderAccumulates)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;

                        _app.PPC_SyncOrdAccumulate.Add(item);
                    }
                }

                foreach (var item in data.NewCustomers)
                {
                    item.SessionID = sessionID;
                    item.BranchID = branchID;
                    item.SlsperID = userName;

                    if (!string.IsNullOrEmpty(item.ImageFileName))
                    {
                       item.ImageFileName = branchID + @"/" + userName + @"/" + Convert.ToDateTime(item.Crtd_Datetime).ToString("yyyyMM") + @"/" + item.ImageFileName;
                    }
               
                    _app.PPC_SyncNewCustomer.Add(item);
                }

                // support sync cho build cu 
                if (data.NewCustomersImage != null)
                {
                    foreach (var item in data.NewCustomersImage)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;
                        item.SlsperID = userName;

                        if (!string.IsNullOrEmpty(item.ImageName))
                        {
                            item.ImageName = branchID + @"/" + userName + @"/" + Convert.ToDateTime(item.VisitDate).ToString("yyyyMM") + @"/" + item.ImageName;
                        }

                        _app.PPC_SyncNewCustomerImges.Add(item);
                    }
                }
               
                foreach (var item in data.NewLocations)
                {
                    item.SessionID = sessionID;
                    item.BranchID = branchID;
                    item.SlsperID = userName;
                    _app.PPC_SyncNewLocation.Add(item);
                }

                foreach (var item in data.CustomerDontBuys)
                {
                    item.SessionID = sessionID;
                    item.BranchID = branchID;
                    item.SlsperID = userName;
                    _app.PPC_SyncCustomerDontBuy.Add(item);
                }

                foreach (var item in data.Logs)
                {
                    item.SessionID = sessionID;
                     _app.PPC_SyncLog.Add(item);
                }

                foreach (var item in data.LocationRealtimes)
                {
                    item.SessionID = sessionID;
                    item.BranchID = branchID;
                    item.SlsperID = userName;

                    _app.PPC_SyncLocationRealtime.Add(item);
                }

                foreach (var item in data.StockOutletDets)
                {
                    item.SessionID = sessionID;
                    item.BranchID = branchID;
                    item.SlsPerID = userName;

                    _app.PPC_SyncStockOutletDet.Add(item);
                }

                foreach (var item in data.StockOutlets)
                {
                    item.SessionID = sessionID;
                    item.BranchID = branchID;
                    item.SlsPerID = userName;

                    _app.PPC_SyncStockOutlet.Add(item);
                }

                foreach (var item in data.SalesTraces)
                {
                    item.SessionID = sessionID;
                    item.BranchID = branchID;
                    item.SlsperID = userName;
                    //suport build cu
                    item.ReasonCode = item.ReasonCode ?? "";
                    item.CustID = item.CustID ?? "";

                    _app.PPC_SyncSalesTrace.Add(item);
                }

                if (data.Posms != null)
                {
                    foreach (var item in data.Posms)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;
                        item.SlsperID = userName;

                        _app.PPC_SyncPosm.Add(item);
                    }
                }
               
                if(data.PosmImages!=null)
                {
                    foreach (var item in data.PosmImages)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;
                        item.SlsperID = userName;
                        item.ImageName = branchID + @"/" + userName + @"/" + Convert.ToDateTime(item.VisitDate).ToString("yyyyMM") + @"/" + item.ImageName;
                        _app.PPC_SyncPosmImage.Add(item);
                    }
                }

                if (data.DisplayRemarkImages != null)
                {
                    foreach (var item in data.DisplayRemarkImages)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;
                        item.SlsperID = userName;
                        item.ImageID = item.ImageID ?? "";
                        item.ImageName = branchID + @"/" + userName + @"/" + Convert.ToDateTime(item.VisitDate).ToString("yyyyMM") + @"/" + item.ImageName;
                        _app.PPC_SyncDisplayRemarkImage.Add(item);
                    }
                }

                if (data.DisplayRemarks != null)
                {
                    foreach (var item in data.DisplayRemarks)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;
                        item.SlsperID = userName;

                        _app.PPC_SyncDisplayRemark.Add(item);
                    }
                }

                if (data.DisplayCustomers != null)
                {
                    foreach (var item in data.DisplayCustomers)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;
                        item.SlsperID = userName;
                        item.SalesID = item.SalesID ?? "";
                        item.BudgetID = item.BudgetID ?? "";
                        _app.PPC_SyncTDisplayCustomer.Add(item);
                    }
                }

                if (data.AccumulatedRegis != null)
                {
                    foreach (var item in data.AccumulatedRegis)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;
                        item.SlsperID = userName;
                        item.SalesID = item.SalesID ?? "";
                        item.BudgetID = item.BudgetID ?? "";
                        _app.PPC_SyncAccumulatedRegis.Add(item);
                    }
                }
                if (data.CustomerTargets != null)
                {
                    foreach (var item in data.CustomerTargets)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;
                        item.SlsperID = userName;

                        _app.PPC_SyncCustomerTarget.Add(item);
                    }
                }

                if (data.Applications != null)
                {
                    foreach (var item in data.Applications)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;
                        item.SlsperId = userName;

                        _app.PPC_SyncApplication.Add(item);
                    }
                }


                if (data.ProgramStampRemarks != null)
                {
                    foreach (var item in data.ProgramStampRemarks)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;
                        item.SlsperID = userName;

                        _app.PPC_SyncProgramStampRemark.Add(item);
                    }
                }

                if (data.ProgramStampRemarkImages != null)
                {
                    foreach (var item in data.ProgramStampRemarkImages)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;
                        item.SlsperID = userName;
                        item.ImageName = branchID + @"/" + userName + @"/" + Convert.ToDateTime(item.RemarkTime).ToString("yyyyMM") + @"/" + item.ImageName;
                        _app.PPC_SyncProgramStampRemarkImage.Add(item);
                    }
                }

                if (data.AlbumImages != null)
                {
                    foreach (var item in data.AlbumImages)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;
                        item.SlsperID = userName;
                        item.ImageName = branchID + @"/" + userName + @"/" + Convert.ToDateTime(item.ImageCreateDate).ToString("yyyyMM") + @"/" + item.ImageName;
                        _app.PPC_SyncAlbumImage.Add(item);
                    }
                }

                if (data.CallPhoneRecordses != null)
                {
                    foreach (var item in data.CallPhoneRecordses)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;
                        item.SlsperID = userName;
                        item.FileNameRecording = branchID + @"/" + userName + @"/" + Convert.ToDateTime(item.StartDateTime).ToString("yyyyMM") + @"/" + item.FileNameRecording;
                        _app.PPC_SyncCallPhoneRecords.Add(item);
                    }
                }

                if (data.CallPhoneRecordses != null)
                {
                    foreach (var item in data.CallPhoneRecordses)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;
                        item.SlsperID = userName;
                        item.FileNameRecording = branchID + @"/" + userName + @"/" + Convert.ToDateTime(item.StartDateTime).ToString("yyyyMM") + @"/" + item.FileNameRecording;
                        _app.PPC_SyncCallPhoneRecords.Add(item);
                    }
                }

                if (data.ResultTrainings != null)
                {
                    foreach (var item in data.ResultTrainings)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;
                        //item.SlsPerID = userName;
                        _app.PPC_SyncResultTraining.Add(item);
                    }
                }

                if (data.SupportTasks != null)
                {
                    foreach (var item in data.SupportTasks)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;
                        item.SlsperID = userName;
                        _app.PPC_SyncSupportTask.Add(item);
                    }
                }

                if (data.TimeKeepings != null)
                {
                    foreach (var item in data.TimeKeepings)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;
                        item.SlsperID = userName;
                        item.ImageName = branchID + @"/" + userName + @"/" + Convert.ToDateTime(item.CheckTime).ToString("yyyyMM") + @"/" + item.ImageName;
                        _app.PPC_SyncTimeKeeping.Add(item);
                    }
                }

                if (data.LeaveRequestForms != null)
                {
                    foreach (var item in data.LeaveRequestForms)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;
                        item.SlsperID = userName;
                        _app.PPC_SyncLeaveRequestForm.Add(item);
                    }
                }
				if (data.OutsideCheckingAudits != null)
                {
	                foreach (var item in data.OutsideCheckingAudits)
	                {
	                    item.SessionID = sessionID;
	                    item.BranchID = branchID;
	                    item.SlsperID = userName;
	                    item.Note = item.Note ?? "";
	                    if (!string.IsNullOrEmpty(item.ImageName))
	                    {
	                        item.ImageName = branchID + @"/" + userName + @"/" + Convert.ToDateTime(item.VisitDate).ToString("yyyyMM") + @"/" + item.ImageName;
	                    }

	                    _app.PPC_SyncOutsideCheckingAudit.Add(item);
	                }
				}				
                if (data.WorkingPlans != null)
                {
                    foreach (var item in data.WorkingPlans)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;
                        _app.PPC_SyncWorkingPlan.Add(item);
                    }
                }

                if (data.RegisterSalesRoute != null)
                {
                    foreach (var item in data.RegisterSalesRoute)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID??"";
                        item.SlsperID = userName;
                        _app.PPC_SyncRegisterSalesRoute.Add(item);
                    }
                }

                if (data.WorkWithCriteriaHeaderResults != null)
                {
                    foreach (var item in data.WorkWithCriteriaHeaderResults)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;
                        item.SupID = userName;
                        _app.PPC_SyncWorkWithCriteriaHeaderResult.Add(item);
                    }
                }

                if (data.WorkWithCriteriaDetailResults != null)
                {
                    foreach (var item in data.WorkWithCriteriaDetailResults)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;
                        item.SupID = userName;
                        _app.PPC_SyncWorkWithCriteriaDetailResult.Add(item);
                    }
                }

                if (data.WorkWithCriteriaRateResults != null)
                {
                    foreach (var item in data.WorkWithCriteriaRateResults)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;
                        item.SupID = userName;
                        _app.PPC_SyncWorkWithCriteriaRateResult.Add(item);
                    }
                }

                if (data.WorkWithOrderResults != null)
                {
                    foreach (var item in data.WorkWithOrderResults)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;
                        item.SupID = userName;
                        _app.PPC_SyncWorkWithOrderResult.Add(item);
                    }
                }

                if (data.WorkWithKPIResults != null)
                {
                    foreach (var item in data.WorkWithKPIResults)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;
                        item.SupID = userName;
                        _app.PPC_SyncWorkWithKPIResult.Add(item);
                    }
                }

                if (data.WorkWithCriteriaKPIResults != null)
                {
                    foreach (var item in data.WorkWithCriteriaKPIResults)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;
                        item.SupID = userName;
                        _app.PPC_SyncWorkWithCriteriaKPIResult.Add(item);
                    }
                }

                if (data.WorkWithMSLResults != null)
                {
                    foreach (var item in data.WorkWithMSLResults)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;
                        item.SupID = userName;
                        _app.PPC_SyncWorkWithMSLResult.Add(item);
                    }
                }

                if (data.WorkWithChanceStatusPresentResults != null)
                {
                    foreach (var item in data.WorkWithChanceStatusPresentResults)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;
                        item.SupID = userName;
                        _app.PPC_SyncWorkWithChanceStatusPresentResult.Add(item);
                    }
                }

                if (data.WorkWithChanceResults != null)
                {
                    foreach (var item in data.WorkWithChanceResults)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;
                        item.SupID = userName;
                        item.LevelID = item.LevelID ??"";
                        _app.PPC_SyncWorkWithChanceResult.Add(item);
                    }
                }

                if (data.WorkWithDisplayResults != null)
                {
                    foreach (var item in data.WorkWithDisplayResults)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;
                        item.SupID = userName;
                        _app.PPC_SyncWorkWithDisplayResult.Add(item);
                    }
                }

                if (data.WorkWithRemarkResults != null)
                {
                    foreach (var item in data.WorkWithRemarkResults)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;
                        item.SupID = userName;
                        _app.PPC_SyncWorkWithRemarkResult.Add(item);
                    }
                }

                if (data.WorkWithTypeFinishResults != null)
                {
                    foreach (var item in data.WorkWithTypeFinishResults)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;
                        item.SupID = userName;
                        _app.PPC_SyncWorkWithTypeFinishResult.Add(item);
                    }
                }

                if (data.WorkWithDetermineTargetResult != null)
                {
                    foreach (var item in data.WorkWithDetermineTargetResult)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;
                        item.SupID = userName;
                        _app.PPC_SyncWorkWithDetermineTargetResult.Add(item);
                    }
                }

                if (data.WorkWithSummaryKPIResult != null)
                {
                    foreach (var item in data.WorkWithSummaryKPIResult)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;
                        item.SupID = userName;
                        _app.PPC_SyncWorkWithSummaryKPIResult.Add(item);
                    }
                }

                if (data.WorkWithSummaryChanceResult != null)
                {
                    foreach (var item in data.WorkWithSummaryChanceResult)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;
                        item.SupID = userName;
                        _app.PPC_SyncWorkWithSummaryChanceResult.Add(item);
                    }
                }

                if (data.WorkWithFinish != null)
                {
                    foreach (var item in data.WorkWithFinish)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;
                        item.SupID = userName;
                        item.ImageSupPath = branchID + @"/" + userName + @"/" + Convert.ToDateTime(item.VisitDate).ToString("yyyyMM") + @"/" + item.ImageSupName;
                        item.ImageSalespersonPath = branchID + @"/" + userName + @"/" + Convert.ToDateTime(item.VisitDate).ToString("yyyyMM") + @"/" + item.ImageSalespersonName;
                        _app.PPC_SyncWorkWithFinish.Add(item);
                    }
                }
				
				if (data.TrainingPrepareAppraiseResults != null)
                {
                    foreach (var item in data.TrainingPrepareAppraiseResults)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;
                        item.SupID = userName;
                        _app.PPC_SyncTrainingPrepareAppraiseResult.Add(item);
                    }
                }

                if (data.TrainingTargetDayResults != null)
                {
                    foreach (var item in data.TrainingTargetDayResults)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;
                        item.SupID = userName;
                        _app.PPC_SyncTrainingTargetDayResult.Add(item);
                    }
                }

                if (data.CheckItemLocs != null)
                {
                    foreach (var item in data.CheckItemLocs)
                    {
                        item.SessionID = sessionID;
                        _app.PPC_SyncCheckItemLoc.Add(item);
                    }
                }

                if (data.CheckItemLocDets != null)
                {
                    foreach (var item in data.CheckItemLocDets)
                    {
                        item.SessionID = sessionID;
                        _app.PPC_SyncCheckItemLocDet.Add(item);
                    }
                }

                if (data.LicenseKnoxes != null)
                {
                    foreach (var item in data.LicenseKnoxes)
                    {
                        item.SessionID = sessionID;

                        _app.PPC_SyncLicenseKnox.Add(item);
                    }
                }
                _app.SaveChanges();

                _app.PPC_ProcessSyncEndSup(branchID,userName,sessionID);

                return new EndSyncResult();
            }
            catch (DbEntityValidationException ex)
            {
                string message = "";
                foreach (var info in ex.EntityValidationErrors)
                {
                    message += String.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:\n",
                        info.Entry.Entity.GetType().Name, info.Entry.State);
                    foreach (var ve in info.ValidationErrors)
                    {
                        message += string.Format("- Property: \"{0}\", Error: \"{1}\"\n",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                log.Error(ex);
                throw Request.CreateResponse(HttpStatusCode.InternalServerError, message);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw Request.CreateResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_app != null)
                {
                    _app.Dispose();
                }
            }
            base.Dispose(disposing);
        }

		[HttpPost]
		public RequestSUPSync SyncGetKE_Request(string userName, string branchID, int version, string sessionID, string fromDate, string toDate, [FromBody] RequestSUPSyncEnd data)
		{
			try
			{
				_app.Configuration.AutoDetectChangesEnabled = false;
				_app.Configuration.EnsureTransactionsForFunctionsAndCommands = false;

				if (data.ke_RequestHeaders != null)
				{
					foreach (var item in data.ke_RequestHeaders)
					{
						item.SessionID = sessionID;
						item.BranchID = branchID;
						_app.PPC_SyncKE_RequestHeader.Add(item);
					}
				}

				_app.SaveChanges();

				_app.PPC_ProcessSyncEndSupKERequest(branchID, userName, sessionID);

				RequestSUPSync report = new RequestSUPSync();
				report.RequestHeaderSUPs = _app.PPC_GetKE_RequestHeaderSUP(branchID, userName, version, sessionID, fromDate, toDate).ToList();
				report.RequestDetailSUPs = _app.PPC_GetKE_RequestDetailSUP(branchID, userName, version, sessionID, fromDate, toDate).ToList();
				report.RequestImageSUPs = _app.PPC_GetKE_RequestImageSUP(branchID, userName, version, sessionID, fromDate, toDate).ToList();

				return report;
			}
			catch (DbEntityValidationException ex)
			{
				string message = "";
				foreach (var info in ex.EntityValidationErrors)
				{
					message += String.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:\n",
						info.Entry.Entity.GetType().Name, info.Entry.State);
					foreach (var ve in info.ValidationErrors)
					{
						message += string.Format("- Property: \"{0}\", Error: \"{1}\"\n",
							ve.PropertyName, ve.ErrorMessage);
					}
				}
				log.Error(ex);
				throw Request.CreateResponse(HttpStatusCode.InternalServerError, message);
			}
			catch (Exception ex)
			{
				log.Error(ex);
				throw Request.CreateResponse(HttpStatusCode.InternalServerError, ex.ToString());
			}
		}

		[HttpGet]
		public ApprovalReasonCheckInSUPSync SyncGetPPC_ApprovalReasonCheckIn(string userName, string branchID, int version, string sessionID)
		{
			try
			{
				ApprovalReasonCheckInSUPSync data = new ApprovalReasonCheckInSUPSync();
				data.approvalReasonCheckInSUPList = _app.PPC_GetPPC_ApprovalReasonCheckInSUP(sessionID, version, branchID, userName).ToList();

				return data;
			}
			catch (Exception ex)
			{
				throw Request.CreateResponse(HttpStatusCode.InternalServerError, ex.ToString());
			}
		}

		[HttpPost]
		public Boolean SyncApprovalReasonCheckIn(string userName, string branchID, int version, string sessionID, [FromBody] ApprovalReasonCheckInSUPSyncEnd data)
		{
			try
			{
				_app.Configuration.AutoDetectChangesEnabled = false;
				_app.Configuration.EnsureTransactionsForFunctionsAndCommands = false;

				if (data.approvalReasonCheckInList != null)
				{
					foreach (var item in data.approvalReasonCheckInList)
					{
						item.SessionID = sessionID;
						item.BranchID = branchID; // khong duoc gan gia tri SlsperID
						_app.PPC_SyncApprovalReasonCheckIn.Add(item);
					}
				}

				_app.SaveChanges();

				_app.PPC_ProcessSyncApprovalReasonCheckInSUP(sessionID, version, branchID, userName);				

				return true;
			}
			catch (DbEntityValidationException ex)
			{
				string message = "";
				foreach (var info in ex.EntityValidationErrors)
				{
					message += String.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:\n",
						info.Entry.Entity.GetType().Name, info.Entry.State);
					foreach (var ve in info.ValidationErrors)
					{
						message += string.Format("- Property: \"{0}\", Error: \"{1}\"\n",
							ve.PropertyName, ve.ErrorMessage);
					}
				}
				log.Error(ex);
				throw Request.CreateResponse(HttpStatusCode.InternalServerError, message);
			}
			catch (Exception ex)
			{
				log.Error(ex);
				throw Request.CreateResponse(HttpStatusCode.InternalServerError, ex.ToString());
			}
		}
   
        [HttpPost]
        public ApproveSalesRouteSync SyncGetApproveSalesRoute(string userName, string branchID, int version, string sessionID, [FromBody] ApproveSalesRouteSyncEnd data)
        {
            try
            {
                _app.Configuration.AutoDetectChangesEnabled = false;
                _app.Configuration.EnsureTransactionsForFunctionsAndCommands = false;

                if (data.RegisterSalesRoute != null)
                {
                    foreach (var item in data.RegisterSalesRoute)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;
                        item.SlsperID = userName;
                        _app.PPC_SyncRegisterSalesRoute.Add(item);
                    }
                }

                _app.SaveChanges();

                _app.PPC_ProcessSyncEndSup_ApproveRegisterSalesRoute(branchID, userName, sessionID);

                ApproveSalesRouteSync approveSalesRouteSync = new ApproveSalesRouteSync();
                approveSalesRouteSync.RegisterSalesRoute = _app.PPC_GetPPC_RegisterSalesRouteSup(branchID, userName, version, sessionID).ToList();
                return approveSalesRouteSync;
            }
            catch (DbEntityValidationException ex)
            {
                string message = "";
                foreach (var info in ex.EntityValidationErrors)
                {
                    message += String.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:\n",
                        info.Entry.Entity.GetType().Name, info.Entry.State);
                    foreach (var ve in info.ValidationErrors)
                    {
                        message += string.Format("- Property: \"{0}\", Error: \"{1}\"\n",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                log.Error(ex);
                throw Request.CreateResponse(HttpStatusCode.InternalServerError, message);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw Request.CreateResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [HttpPost]
        public RegisterDisplayCustomerSync SyncGetRegisterDisplayCustomer(string userName, string branchID, int version, string sessionID, [FromBody] RegisterDisplayCustomerSyncEnd data)
        {
            try
            {
                _app.Configuration.AutoDetectChangesEnabled = false;
                _app.Configuration.EnsureTransactionsForFunctionsAndCommands = false;

                if (data.DisplayCustomers != null)
                {
                    foreach (var item in data.DisplayCustomers)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;
                        item.SlsperID = userName;
                        item.SalesID = item.SalesID ?? "";
                        item.BudgetID = item.BudgetID ?? "";
                        _app.PPC_SyncTDisplayCustomer.Add(item);
                    }
                }

                _app.SaveChanges();

                _app.PPC_ProcessSyncEndSup_TDisplayCustomer(branchID, userName, sessionID);

                RegisterDisplayCustomerSync register = new RegisterDisplayCustomerSync();
                //Customer
                register.Salespersons = _app.PPC_GetAR_Salesperson(branchID, userName, version, sessionID).ToList();
                register.RouteDets = _app.PPC_GetOM_SalesRouteDetSup(branchID, userName, version, sessionID).ToList();
                register.Customers = _app.PPC_GetAR_CustomerSup(branchID, userName, version, sessionID).ToList();
                register.Addresses = _app.PPC_GetAR_SOAddressV2(branchID, userName, version, sessionID).ToList();
                //Displays Customer
                register.Displays = _app.PPC_GetOM_DisplaySup(branchID, userName, version, sessionID).ToList();
                register.DisplayCusts = _app.PPC_GetOM_DisplayCustSup(branchID, userName, version, sessionID).ToList();
                register.BudgetTrades = _app.PPC_GetOM_BudgetTrade(branchID, userName, version, sessionID).ToList();
                return register;
            }
            catch (DbEntityValidationException ex)
            {
                string message = "";
                foreach (var info in ex.EntityValidationErrors)
                {
                    message += String.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:\n",
                        info.Entry.Entity.GetType().Name, info.Entry.State);
                    foreach (var ve in info.ValidationErrors)
                    {
                        message += string.Format("- Property: \"{0}\", Error: \"{1}\"\n",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                log.Error(ex);
                throw Request.CreateResponse(HttpStatusCode.InternalServerError, message);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw Request.CreateResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [HttpPost]
        public RegisterAccumulationSync SyncGetRegisterAccumulation(string userName, string branchID, int version, string sessionID, [FromBody] RegisterAccumulationSyncEnd data)
        {
            try
            {
                _app.Configuration.AutoDetectChangesEnabled = false;
                _app.Configuration.EnsureTransactionsForFunctionsAndCommands = false;

                if (data.AccumulatedRegis != null)
                {
                    foreach (var item in data.AccumulatedRegis)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;
                        item.SlsperID = userName;
                        item.SalesID = item.SalesID ?? "";
                        item.BudgetID = item.BudgetID ?? "";
                        _app.PPC_SyncAccumulatedRegis.Add(item);
                    }
                }

                _app.SaveChanges();

                _app.PPC_ProcessSyncEndSup_AccumulatedRegister(branchID, userName, sessionID);

                RegisterAccumulationSync register = new RegisterAccumulationSync();
                //Customer
                register.Salespersons = _app.PPC_GetAR_Salesperson(branchID, userName, version, sessionID).ToList();
                register.RouteDets = _app.PPC_GetOM_SalesRouteDetSup(branchID, userName, version, sessionID).ToList();
                register.Customers = _app.PPC_GetAR_CustomerSup(branchID, userName, version, sessionID).ToList();
                register.Addresses = _app.PPC_GetAR_SOAddressV2(branchID, userName, version, sessionID).ToList();
                //Accumulation
                register.Accumulated = _app.PPC_GetOM_AccumulatedSup(branchID, userName, version, sessionID).ToList();
                register.AccumulatedRegis = _app.PPC_GetOM_AccumulatedRegisSup(branchID, userName, version, sessionID).ToList();
                register.BudgetTrades = _app.PPC_GetOM_BudgetTrade(branchID, userName, version, sessionID).ToList();
                return register;
            }
            catch (DbEntityValidationException ex)
            {
                string message = "";
                foreach (var info in ex.EntityValidationErrors)
                {
                    message += String.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:\n",
                        info.Entry.Entity.GetType().Name, info.Entry.State);
                    foreach (var ve in info.ValidationErrors)
                    {
                        message += string.Format("- Property: \"{0}\", Error: \"{1}\"\n",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                log.Error(ex);
                throw Request.CreateResponse(HttpStatusCode.InternalServerError, message);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw Request.CreateResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }
        [HttpPost]
        public CheckStockItemLocSync GetCheckStockItemLoc(string userName, string branchID, int version, string sessionID, [FromBody] CheckStockItemLocSyncEnd data)
        {
            try
            {
                _app.Configuration.AutoDetectChangesEnabled = false;
                _app.Configuration.EnsureTransactionsForFunctionsAndCommands = false;

                if (data.CheckItemLocs != null)
                {
                    foreach (var item in data.CheckItemLocs)
                    {
                        item.SessionID = sessionID;
                        _app.PPC_SyncCheckItemLoc.Add(item);
                    }
                }

                if (data.CheckItemLocDets != null)
                {
                    foreach (var item in data.CheckItemLocDets)
                    {
                        item.SessionID = sessionID;
                        _app.PPC_SyncCheckItemLocDet.Add(item);
                    }
                }
                _app.SaveChanges();

                _app.PPC_ProcessSyncEndSup_CheckItemLoc(branchID, userName, sessionID);

                CheckStockItemLocSync checkStockItemLocSync = new CheckStockItemLocSync();
                checkStockItemLocSync.CheckItemLocs = _app.PPC_GetPPC_CheckItemLoc(branchID, userName, version, sessionID).ToList();
                checkStockItemLocSync.CheckItemLocDets = _app.PPC_GetPPC_CheckItemLocDet(branchID, userName, version, sessionID).ToList();
                return checkStockItemLocSync;
            }
            catch (DbEntityValidationException ex)
            {
                string message = "";
                foreach (var info in ex.EntityValidationErrors)
                {
                    message += String.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:\n",
                        info.Entry.Entity.GetType().Name, info.Entry.State);
                    foreach (var ve in info.ValidationErrors)
                    {
                        message += string.Format("- Property: \"{0}\", Error: \"{1}\"\n",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                log.Error(ex);
                throw Request.CreateResponse(HttpStatusCode.InternalServerError, message);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw Request.CreateResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

    }
}
