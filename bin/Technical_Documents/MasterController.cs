
using eSales.Service.Models;
using eSales.Service.Utils;
using HQFramework.DAL;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using eSales.ProcessDb;
using eSales.Process;
using eSales.Service.Helper;

namespace eSales.Service.Controllers
{
    public class MasterController : ApiController
    {
        private AppEntities _app = DbHelper.CreateObjectContext<AppEntities>(false);
        private static ILog log = LogManager.GetLogger(typeof(MasterController));

        public MasterController()
        {
            _app.Database.CommandTimeout = 600;
        }
        public InventorySync GetInventory(string userName, string branchID,[FromUri] List<TableVersion> tableVersions, string sessionID)
        {
            try
            {
                InventorySync inventory = new InventorySync();
                inventory.Inventories = _app.PPC_GetIN_Inventory(userName, branchID, GetVersion("IN_Inventory", tableVersions), sessionID).ToList();
                inventory.ItemLocs = _app.PPC_GetIN_ItemLocV2(branchID, userName, GetVersion("IN_ItemLoc", tableVersions), sessionID).ToList();
                inventory.ProductClasses = _app.PPC_GetIN_ProductClassV2(userName, branchID, GetVersion("IN_ProductClass", tableVersions), sessionID).ToList();
                inventory.Conversions = _app.PPC_GetIN_UnitConversionV2(userName, branchID,  GetVersion("IN_UnitConversion", tableVersions), sessionID).ToList();
                inventory.PosmAllocs = _app.PPC_GetIN_PosmAlloc(branchID, userName, GetVersion("IN_PosmAlloc", tableVersions), sessionID).ToList();
                inventory.PosmInventories = _app.PPC_GetIN_PosmInventory(branchID, userName, GetVersion("IN_PosmInventory", tableVersions), sessionID).ToList();
                inventory.Kits = _app.PPC_GetIN_Kit(branchID, userName, GetVersion("IN_Kit", tableVersions), sessionID).ToList();
                inventory.Components = _app.PPC_GetIN_Component(branchID, userName, GetVersion("IN_Component", tableVersions), sessionID).ToList();

                return inventory;
            }
            catch (Exception ex)
            {
                throw Request.CreateResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        public InventorySync GetItemLoc(string userName, string branchID, [FromUri] List<TableVersion> tableVersions, string sessionID)
        {
            try
            {
                InventorySync inventory = new InventorySync();
                inventory.ItemLocs = _app.PPC_GetIN_ItemLocV2(branchID, userName, GetVersion("IN_ItemLoc", tableVersions), sessionID).ToList();
                inventory.PosmAllocs = _app.PPC_GetIN_PosmAlloc(branchID, userName, GetVersion("IN_PosmAlloc", tableVersions), sessionID).ToList();
                return inventory;
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
                general.BranchSettings = _app.PPC_GetBranchSetting(branchID, userName, 0, sessionID).ToList();
                general.Channels = _app.PPC_GetAR_Channel(branchID, userName, GetVersion("AR_Channel", tableVersions), sessionID).ToList();
                general.ShopTypes = _app.PPC_GetAR_ShopTypeV2(branchID, userName, GetVersion("AR_ShopType", tableVersions), sessionID).ToList();
                general.Taxes = _app.PPC_GetSI_TaxV2(branchID, userName, GetVersion("SI_Tax", tableVersions), sessionID).ToList();
                general.Territories = _app.PPC_GetAR_Territory(branchID, userName, GetVersion("AR_Territory", tableVersions), sessionID).ToList();
                general.States = _app.PPC_GetSI_StateV2(branchID, userName, GetVersion("SI_State", tableVersions), sessionID).ToList();
                general.Districts = _app.PPC_GetSI_DistrictV2(branchID, userName, GetVersion("SI_District", tableVersions), sessionID).ToList();
                general.DisplayLocations = _app.PPC_GetOM_DisplayLocation(branchID, userName, GetVersion("OM_DisplayLocation", tableVersions), sessionID).ToList();
                general.Displays = _app.PPC_GetOM_Display(branchID, userName, GetVersion("OM_Display", tableVersions), sessionID).ToList();
                general.DisplayCusts = _app.PPC_GetOM_DisplayCust(branchID, userName, GetVersion("OM_DisplayCust", tableVersions), sessionID).ToList();
                general.Advertises = _app.PPC_GetOM_AdvertiseV2(branchID, userName, GetVersion("OM_Advertise", tableVersions), sessionID).ToList();
                general.Reasons = _app.PPC_GetPPC_ReasonCode(branchID, userName, GetVersion("PPC_ReasonCode", tableVersions), sessionID, langID).ToList();
                general.CustClasses = _app.PPC_GetAR_CustClassV2(branchID, userName, GetVersion("AR_CustClass", tableVersions), sessionID).ToList();
                general.Locations = _app.PPC_GetAR_Location(branchID, userName, GetVersion("AR_Location", tableVersions), sessionID).ToList();
                // Begin Sync Dashboard ---------------------------------
                general.DashboardKpis = _app.PPC_GetPPC_DashboardKpi(branchID, userName, GetVersion("PPC_DashboardKpi", tableVersions), sessionID, langID).ToList();
				general.DashboardKpiDetails = _app.PPC_GetPPC_DashboardKpiDetails(branchID, userName, GetVersion("PPC_DashboardKpiDetails", tableVersions), sessionID, langID).ToList();
				general.DashboardDows = _app.PPC_GetPPC_DashboardDow(branchID, userName, GetVersion("PPC_DashboardDow", tableVersions), sessionID, langID).ToList();
                general.DashboardWeeks = _app.PPC_GetPPC_DashboardWeek(branchID, userName, GetVersion("PPC_DashboardWeek", tableVersions), sessionID, langID).ToList();
				general.DashboardMultiColumns = _app.PPC_GetPPC_DashboardMultiColumn(branchID, userName, GetVersion("PPC_DashboardMultiColumn", tableVersions), sessionID, langID).ToList();
				general.DashboardMultiColumnDetails = _app.PPC_GetPPC_DashboardMultiColumn_Details(branchID, userName, GetVersion("PPC_DashboardMultiColumn_Details", tableVersions), sessionID, langID).ToList();
                general.DashboardMetros = _app.PPC_GetPPC_DashboardMetro(branchID, userName, GetVersion("PPC_DashboardMetro", tableVersions), sessionID, langID).ToList();
                // End Sync Dashboard ------------------------------------------
                general.SlsFreq = _app.PPC_GetPPC_SlsFreq(branchID, userName, GetVersion("PPC_SlsFreq", tableVersions), sessionID, langID).ToList();
                general.WeekofVisitMCL = _app.PPC_GetPPC_WeekofVisitMCL(branchID, userName, GetVersion("PPC_WeekofVisitMCL", tableVersions), sessionID).ToList();
                general.SalesDetailHistories = _app.PPC_GetPPC_SalesDetailHistory(branchID, userName, GetVersion("PPC_SalesDetailHistory", tableVersions), sessionID).ToList();
                general.SalesHeaderHistories = _app.PPC_GetPPC_SalesHeaderHistory(branchID, userName, GetVersion("PPC_SalesHeaderHistory", tableVersions), sessionID).ToList();
                general.ProgramSalesItems = _app.PPC_GetPPC_ProgramSalesItem(branchID, userName, GetVersion("PPC_ProgramSalesItem", tableVersions), sessionID).ToList();
                general.SalesOrdHistories = _app.PPC_GetOM_SalesOrdHistory(branchID, userName, GetVersion("OM_SalesOrdHistory", tableVersions), sessionID).ToList();
                general.SalesOrdDetHistories = _app.PPC_GetOM_SalesOrdDetHistory(branchID, userName, GetVersion("OM_SalesOrdDetHistory", tableVersions), sessionID).ToList();
                general.StockOutletHistories = _app.PPC_GetPPC_StockOutletHistory(branchID, userName, GetVersion("PPC_StockOutletHistory", tableVersions), sessionID).ToList();
                general.PictureTypeNewCustomers = _app.PPC_GetPPC_PictureTypeNewCustomer(branchID, userName, GetVersion("PPC_PictureTypeNewCustomer", tableVersions), sessionID).ToList();
                general.PosmCustomers = _app.PPC_GetPPC_PosmCustomer(branchID, userName, GetVersion("PPC_PosmCustomer", tableVersions), sessionID).ToList();
                general.SalesDisplayHistory = _app.PPC_GetPPC_SalesDisplayHistory(branchID, userName, GetVersion("PPC_SalesDisplayHistory", tableVersions), sessionID).ToList();           
                general.Areas = _app.PPC_GetAR_Area(branchID, userName, GetVersion("AR_Area", tableVersions), sessionID).ToList();
                general.SubTerritories = _app.PPC_GetSI_SubTerritory(branchID, userName, GetVersion("SI_SubTerritory", tableVersions), sessionID).ToList();
                general.OutSideCheckings = _app.PPC_GetPPC_OutSideChecking(branchID, userName, GetVersion("PPC_OutSideChecking", tableVersions), sessionID).ToList();
                general.Wards = _app.PPC_GetSI_Ward(branchID, userName, GetVersion("SI_Ward", tableVersions), sessionID).ToList();
                general.CustomerSalesDetHistories = _app.PPC_GetRPT_CustomerSalesDetHistory(branchID, userName, GetVersion("CustomerSalesDetHistory", tableVersions), sessionID, langID).ToList();
                general.CustomerSalesOrdHistories = _app.PPC_GetRPT_CustomerSalesOrdHistory(branchID, userName, GetVersion("CustomerSalesOrdHistory", tableVersions), sessionID, langID).ToList();
                general.PriceClasses = _app.PPC_GetOM_PriceClassV2(branchID, userName, GetVersion("OM_PriceClass", tableVersions), sessionID).ToList();
                general.Notifies = _app.PPC_GetPPC_Notify(branchID, userName, GetVersion("PPC_Notify", tableVersions), sessionID, langID).ToList();
                general.ProgramStamps = _app.PPC_GetOM_ProgramStamp(branchID, userName, GetVersion("OM_ProgramStamp", tableVersions), sessionID).ToList();
                general.ProgramStampDetails = _app.PPC_GetOM_ProgramStampDetail(branchID, userName, GetVersion("OM_ProgramStampDetail", tableVersions), sessionID).ToList();
                general.Albums = _app.PPC_GetOM_Album(branchID, userName, GetVersion("OM_Album", tableVersions), sessionID).ToList();
                general.DisplayRewards = _app.PPC_GetOM_DisplayReward(branchID, userName, GetVersion("OM_DisplayReward", tableVersions), sessionID).ToList();
                general.DiscBreakQuotas = _app.PPC_GetOM_DiscBreakQuota(branchID, userName, GetVersion("OM_DiscBreakQuota", tableVersions), sessionID).ToList();
                general.SalesRoutePlan = _app.PPC_GetSalesRoutePlan(branchID, userName, GetVersion("PPC_SalesRoutePlan", tableVersions), sessionID).ToList();
                general.TimeKeeping = _app.PPC_GetTimeKeeping(branchID, userName, GetVersion("PPC_TimeKeeping", tableVersions), sessionID).ToList();
                general.MustStocks = _app.PPC_GetPPC_MustStock(branchID, userName, GetVersion("PPC_MustStock", tableVersions), sessionID).ToList();
                general.Accumulated = _app.PPC_GetOM_Accumulated(branchID, userName, GetVersion("OM_Accumulated", tableVersions), sessionID).ToList();
                general.AccumulatedRegis = _app.PPC_GetOM_AccumulatedRegis(branchID, userName, GetVersion("OM_AccumulatedRegis", tableVersions), sessionID).ToList();
                general.SalesRoutePlanCustomer = _app.PPC_GetPPC_SalesRoutePlanCustomer(branchID, userName, GetVersion("AR_Customer", tableVersions), sessionID).ToList();
                general.SampleImages = _app.PPC_GetOM_SampleImages(branchID, userName, GetVersion("OM_SampleImages", tableVersions), sessionID).ToList();
                general.SubRoute = _app.PPC_GetOM_SubRoute(branchID, userName, GetVersion("OM_SubRoute", tableVersions), sessionID).ToList();
                general.InSite = _app.PPC_GetIN_Site_Stock(branchID, userName, GetVersion("IN_Site", tableVersions), sessionID).ToList();
                general.Sizes = _app.PPC_GetSI_Size(branchID, userName, GetVersion("SI_Size", tableVersions), sessionID).ToList();
                general.Stand = _app.PPC_GetSI_Stand(branchID, userName, GetVersion("SI_Stand", tableVersions), sessionID).ToList();
                general.Brand = _app.PPC_GetSI_Brand(branchID, userName, GetVersion("SI_Brand", tableVersions), sessionID).ToList();
                general.Display = _app.PPC_GetSI_Display(branchID, userName, GetVersion("SI_Display", tableVersions), sessionID).ToList();
				general.Hierarchy = _app.PPC_GetSI_Hierarchy(branchID, userName, GetVersion("SI_Hierarchy", tableVersions), sessionID).ToList();
                general.Ounits = _app.PPC_GetAR_OUnit(branchID, userName, GetVersion("AR_OUnit", tableVersions), sessionID).ToList();
                general.StockCheckTypes = _app.PPC_GetStockCheckType(branchID, userName, GetVersion("PPC_CheckTypeInventory", tableVersions), sessionID).ToList();
                general.StockOutletTargets = _app.PPC_GetPPC_StockOutletTarget(branchID, userName, GetVersion("PPC_StockOutletTarget", tableVersions), sessionID).ToList();
                general.StockScanHistories = _app.PPC_GetPPC_StockScanHistory(branchID, userName, GetVersion("PPC_StockScanHistory", tableVersions), sessionID).ToList();              
                general.Markets = _app.PPC_GetSI_Market(branchID, userName, GetVersion("SI_Market", tableVersions), sessionID).ToList();
                general.BudgetTrades = _app.PPC_GetOM_BudgetTrade(branchID, userName, GetVersion("OM_BudgetTrade(", tableVersions), sessionID).ToList();
				general.InventoryInfors = _app.PPC_GetIN_InventoryInfor(sessionID, GetVersion("IN_InventoryInfor", tableVersions), branchID, userName).ToList();
				general.ProgramSalesTypes = _app.PPC_GetPPC_ProgramSalesType(sessionID, GetVersion("PPC_ProgramSalesType", tableVersions), branchID, userName, langID).ToList();
                general.WorkStations = _app.PPC_GetOM_WorkStation(branchID, userName, GetVersion("OM_WorkStation", tableVersions), sessionID).ToList();
                general.WorkingPlans = _app.PPC_GetOM_WorkingPlan(branchID, userName, GetVersion("OM_WorkingPlan", tableVersions), sessionID).ToList();
				general.CompetitorSurveyHeaders = _app.PPC_GetOM_CompetitorSurveyHeader(sessionID, GetVersion("OM_CompetitorSurveyHeader", tableVersions), branchID, userName).ToList();
				general.CompetitorSurveyInvts = _app.PPC_GetOM_CompetitorSurveyInvt(sessionID, GetVersion("OM_CompetitorSurveyInvt", tableVersions), branchID, userName).ToList();
				general.CompetitorSurveyCriterias = _app.PPC_GetOM_CompetitorSurveyCriteria(sessionID, GetVersion("OM_CompetitorSurveyCriteria", tableVersions), branchID, userName).ToList();
                general.SalesTraces = _app.PPC_GetOM_SalesTrace(branchID, userName, GetVersion("OM_SalesTrace", tableVersions), sessionID).ToList();
                // Begin Sync Work With ---------------------------------
                general.TrainingPlan = _app.PPC_GetTrainingPlan(branchID, userName, 0, sessionID).ToList();
                general.WorkWithCriteria = _app.PPC_GetWorkWithCriteriaResult(branchID, userName, 0, sessionID).ToList();
                general.WorkWithChanceResult = _app.PPC_GetWorkWithChanceResult(branchID, userName, 0, sessionID).ToList();
                // End Sync Work With ---------------------------------
                general.AdvBookings = _app.PPC_GetOM_AdvBooking(branchID, userName, 0, sessionID).ToList();
                general.BookingSalesDets = _app.PPC_GetOM_BookingSalesDet(branchID, userName, 0, sessionID).ToList();
				// Begin Sync VietUc_eSales ---------------------------------
                general.QuestLists = _app.PPC_GetSI_QuestList(branchID, userName, GetVersion("SI_QuestList", tableVersions), sessionID).ToList();
                general.Questions = _app.PPC_GetSI_Question(branchID, userName, GetVersion("SI_Question", tableVersions), sessionID).ToList();
                general.Answers = _app.PPC_GetSI_Answer(branchID, userName, GetVersion("SI_Answer", tableVersions), sessionID).ToList();
                general.QuestListCpnys = _app.PPC_GetSI_QuestListCpny(branchID, userName, GetVersion("SI_QuestListCpny", tableVersions), sessionID).ToList();
                general.TestLists = _app.PPC_GetSI_TestList(branchID, userName, GetVersion("SI_TestList", tableVersions), sessionID).ToList();
                general.TestQuests = _app.PPC_GetSI_TestQuest(branchID, userName, GetVersion("SI_TestQuest", tableVersions), sessionID).ToList();
                // End Sync VietUc_eSales ---------------------------------
                general.AccumulatedRewards = _app.PPC_GetOM_AccumulatedReward(branchID, userName, GetVersion("OM_AccumulatedReward", tableVersions), sessionID).ToList();

                _app.PPC_SyncClear(branchID, userName, 0, sessionID);

                return general;
            }
            catch (Exception ex)
            {
                throw Request.CreateResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }

        }
        public PriceSync GetSalesPrice(string userName, string branchID, int version, string sessionID)
        {
            try
            {
                PriceSync price = new PriceSync();
                price.Details = _app.PPC_GetOM_SalesPrice(branchID, userName, version, sessionID).ToList();
                price.Custs = _app.PPC_GetOM_SalesPriceCust(branchID, userName, version, sessionID).ToList();
                return price;
            }
            catch (Exception ex)
            {
                throw Request.CreateResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }

        }
        public CustomerSync GetCustomer(string userName, string branchID, int version, string sessionID)
        {
            try
            {
                CustomerSync customer = new CustomerSync();
                customer.Salespersons = _app.PPC_GetAR_Salesperson(branchID, userName, version, sessionID).ToList();
                customer.RouteDets = _app.PPC_GetOM_SalesRouteDetV2(branchID, userName, version, sessionID).ToList();
                customer.Customers = _app.PPC_GetAR_CustomerV2(branchID, userName, version, sessionID).ToList();
                customer.Addresses = _app.PPC_GetAR_SOAddressV2(branchID, userName, version, sessionID).ToList();
                customer.BillAddresses = _app.PPC_GetAR_BillAddress(branchID, userName, version, sessionID).ToList();
				customer.KeItemStatuses = _app.PPC_GetKE_ItemStatus(branchID, userName, version, sessionID).ToList();
                customer.CustomerEditReasons = _app.PPC_GetAR_CustomerEditReason(branchID, userName, version, sessionID).ToList();
                customer.DisplayTarget = _app.PPC_GetPPC_DisplayTarget(branchID, userName, version, sessionID).ToList();
                customer.CustomerLevel = _app.PPC_GetAR_CustomerLevel(branchID, userName, version, sessionID).ToList();
                customer.RegisterSalesRoute = _app.PPC_GetPPC_RegisterSalesRoute(branchID, userName, version, sessionID).ToList();
                customer.CustomerDebt = _app.PPC_GetPPC_CustomerDebt(branchID, userName, version, sessionID).ToList();
				customer.Doces = _app.PPC_GetAR_Doc(sessionID, version, branchID, userName).ToList();
				customer.Balances = _app.PPC_GetAR_Balances(sessionID, version, branchID, userName).ToList();

				return customer;
            }
            catch (Exception ex)
            {
                throw Request.CreateResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
          
        }
        public API_StormLotSync CheckScanStampQRCodePac(string userName, string branchID, int version, string sessionID, string data)
        {
            try
            {
                API_StormLotSync stormLotSync = new API_StormLotSync();
                stormLotSync.StormLots = _app.PPC_ProcessCheckScanStampQRCodePac(branchID, userName, sessionID, data).ToList();
               
                return stormLotSync;
            }
            catch (Exception ex)
            {
                throw Request.CreateResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }

        }
        public DiscountSync GetDiscount(string userName, string branchID, int version, string sessionID)
        {
            try
            {
                DiscountSync discount = new DiscountSync();
                // Auto Discount
                //discount.Seqs = _app.PPC_GetOM_DiscSeq(branchID, userName, version, sessionID).ToList();
                //discount.Items = _app.PPC_GetOM_DiscItemV2(branchID, userName, version, sessionID).ToList();

                discount.Seqs = new List<PPC_GetOM_DiscSeq_Result>();
                discount.Items = new List<PPC_GetOM_DiscItem_Result>();
                discount.ItemClasses = new List<PPC_GetOM_DiscItemClass_Result>();
                discount.Custs = new List<PPC_GetOM_DiscCust_Result>();
                discount.CustClasses = new List<PPC_GetOM_DiscCustClass_Result>();
                discount.FreeItems = new List<PPC_GetOM_DiscFreeItem_Result>();
                discount.Breaks = new List<PPC_GetOM_DiscBreak_Result>();
                discount.Budgets = new List<PPC_GetOM_PPCpny_Result>();
                discount.CustCates = new List<PPC_GetOM_DiscCustCate_Result>();
                discount.Channels = new List<PPC_GetOM_DiscChannel_Result>();
                discount.SubBreakItems = new List<PPC_GetOM_DiscSubBreakItem_Result>();
                // Manual Discount
                discount.DiscDescrs = new List<PPC_GetOM_DiscDescr_Result>();
                discount.DiscDescrCusts = new List<PPC_GetOM_DiscDescrCust_Result>();
                discount.DiscDescrFreeItems = new List<PPC_GetOM_DiscDescrFreeItem_Result>();



                //discount.Seqs = _app.PPC_GetOM_DiscSeq(branchID, userName, version, sessionID).ToList();
                //discount.Items = _app.PPC_GetOM_DiscItemV2(branchID, userName, version, sessionID).ToList();
                //discount.ItemClasses = _app.PPC_GetOM_DiscItemClassV2(branchID, userName, version, sessionID).ToList();
                //discount.Custs = _app.PPC_GetOM_DiscCustV2(branchID, userName, version, sessionID).ToList();
                //discount.CustClasses = _app.PPC_GetOM_DiscCustClassV2(branchID, userName, version, sessionID).ToList();
                //discount.FreeItems = _app.PPC_GetOM_DiscFreeItem(branchID, userName, version, sessionID).ToList();
                //discount.Breaks = _app.PPC_GetOM_DiscBreak(branchID, userName, version, sessionID).ToList();
                //discount.Budgets = _app.PPC_GetOM_PPCpny(branchID, userName, version, sessionID).ToList();
                //discount.CustCates = _app.PPC_GetOM_DiscCustCate(branchID, userName, version, sessionID).ToList();
                //discount.Channels = _app.PPC_GetOM_DiscChannel(branchID, userName, version, sessionID).ToList();
                //discount.SubBreakItems = _app.PPC_GetOM_DiscSubBreakItem(branchID, userName, version, sessionID).ToList();
                //// Manual Discount
                //discount.DiscDescrs = _app.PPC_GetOM_DiscDescrV2(branchID, userName, version, sessionID).ToList();
                //discount.DiscDescrCusts = _app.PPC_GetOM_DiscDescrCust(branchID, userName, version, sessionID).ToList();
                //discount.DiscDescrFreeItems = _app.PPC_GetOM_DiscDescrFreeItem(branchID, userName, version, sessionID).ToList();

                return discount;
            }
            catch(Exception ex)
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
        
        public ReportSKUSync GetReportSKU(string userName, string branchID, int version, string sessionID, string fromDate, string toDate)
        {
            try
            {
                ReportSKUSync reportSKU = new ReportSKUSync();
                reportSKU.QtySKUTotal =  _app.PPC_GetRPT_QtySKUTotal(branchID, userName, 0, sessionID,fromDate,toDate).ToList();
                return reportSKU;
            }
            catch (Exception ex)
            {
                throw Request.CreateResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }
        public ReportMonthlyHTSync GetReportMonthlyHT(string userName, string branchID, int version, string sessionID, string fromMonth, string toMonth)
        {
            try
            {
                ReportMonthlyHTSync report = new ReportMonthlyHTSync();
                report.Rptmonthlytotal_ht = _app.PPC_GetRPT_MonthlyTotal_HT(branchID, userName, version, sessionID, fromMonth, toMonth).ToList();
                report.Rptmonthlyproduct_ht = _app.PPC_GetRPT_MonthlyProduct_HT(branchID, userName, version, sessionID, fromMonth, toMonth).ToList();
                report.Rptmonthlycustomer_ht = _app.PPC_GetRPT_MonthlyCustomer_HT(branchID, userName, version, sessionID, fromMonth, toMonth).ToList();
                return report;
            }
            catch (Exception ex)
            {
                throw Request.CreateResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }

        }
        public ReportWorkingTimeSync GetReportWorkingTime(string userName, string branchID, int version, string sessionID, string fromMonth, string toMonth)
        {
            try
            {
                ReportWorkingTimeSync report = new ReportWorkingTimeSync();
                report.RptWorkingTimes = _app.PPC_GetRPT_WorkingTime(branchID, userName, version, sessionID, fromMonth, toMonth).ToList();
                return report;
            }
            catch (Exception ex)
            {
                throw Request.CreateResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }

        }
        public ApplockSync GetApplock(string userName, string branchID, int version, string sessionID, string imei)
        {
            try
            {
                ApplockSync applock = new ApplockSync();
                applock.ApplicationList = _app.PPC_GetPPC_Application(branchID, userName, version, sessionID, imei).ToList();
                applock.ApplicationKnoxBlackList = _app.PPC_GetPPC_ApplicationKnoxBlackList(branchID, userName, version, sessionID, imei).ToList();
                return applock;
            }
            catch (Exception ex)
            {
                throw Request.CreateResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }

        }
        public KnoxSync GetKnox(string userName, string branchID, int version, string sessionID, string imei)
        {
            try
            {
                KnoxSync knox = new KnoxSync();
                knox.Device_rules = _app.PPC_GetDevice_Rules(userName , branchID, version, sessionID, imei).ToList();
                return knox;
            }
            catch (Exception ex)
            {
                throw Request.CreateResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }

        }

        public ConfigSync GetConfig(string userName, string branchID, [FromUri] List<TableVersion> tableVersions, string sessionID, string langID)
        {
            try
            {
                ConfigSync config = new ConfigSync();
                config.Languages = _app.PPC_GetPPC_Language(branchID, userName, GetVersion("PPC_Language", tableVersions), sessionID, langID).ToList();
                config.ScreenConfigs = _app.PPC_GetScreenConfig(branchID, userName, GetVersion("PPC_ScreenConfig", tableVersions), sessionID).ToList();
                config.SalesSteps = _app.PPC_GetPPC_SalesStep(branchID, userName, GetVersion("PPC_SalesStep", tableVersions), sessionID).ToList();
                config.SalesStepSeconds = _app.PPC_GetPPC_SalesStepSecond(branchID, userName, GetVersion("PPC_SalesStepSecond", tableVersions), sessionID).ToList();
                config.SalesStepOtherInfors = _app.PPC_GetPPC_SalesStepOtherInfor(branchID, userName, GetVersion("PPC_SalesStepOtherInfor", tableVersions), sessionID).ToList();
                config.Reports = _app.PPC_GetPPC_Report(branchID, userName, GetVersion("PPC_Report", tableVersions), sessionID).ToList();
                return config;
            }
            catch (Exception ex)
            {
                throw Request.CreateResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }

        }
        public OrderSync GetOrder(string userName, string branchID, int version, string sessionID)
        {
            try
            {
                OrderSync data = new OrderSync();
                data.SalesOrdHistories = _app.PPC_GetOM_SalesOrdHistory(branchID, userName, 0, sessionID).ToList();
                data.SalesOrdDetHistories = _app.PPC_GetOM_SalesOrdDetHistory(branchID, userName, 0, sessionID).ToList();
                return data;
            }
            catch (Exception ex)
            {
                throw Request.CreateResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }

        }
		public ReportOrderSync GetReportOrder(string userName, string branchID, int version, string sessionID)
		{
			try
			{
				ReportOrderSync data = new ReportOrderSync();
				data.SalesOrdHistories = _app.PPC_GetOM_SalesOrdHistory(branchID, userName, 0, sessionID).ToList();
				data.SalesOrdDetHistories = _app.PPC_GetOM_SalesOrdDetHistory(branchID, userName, 0, sessionID).ToList();
				data.SalesOrdStatusHistories = _app.PPC_GetRPT_SalesOrdStatusHistory(branchID, userName, 0, sessionID).ToList();
				return data;
			}
			catch (Exception ex)
			{
				throw Request.CreateResponse(HttpStatusCode.InternalServerError, ex.ToString());
			}
		}

		public ApprovalReasonCheckInSync GetApprovalReasonCheckIn(string userName, string branchID, int version, string sessionID, string visitDate)
        {
            try
            {
                ApprovalReasonCheckInSync approval = new ApprovalReasonCheckInSync();
                approval.ApprovalReasonCheckInList = _app.PPC_GetApprovalReasonCheckIn(branchID, userName, version, sessionID, Convert.ToDateTime(visitDate)).ToList();
                return approval;
            }
            catch (Exception ex)
            {
                throw Request.CreateResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }

        }
        [HttpPost]
        public String CheckScanStampQRCode(string userName, string branchID, string sessionID, string data)
        {
            try
            {
                _app.Configuration.AutoDetectChangesEnabled = false;
                _app.Configuration.EnsureTransactionsForFunctionsAndCommands = false;

                var user = _app.PPC_ProcessCheckScanStampQRCode(branchID, userName, sessionID, data).FirstOrDefault();

                return user.ToString();
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
        public int GetVersion(string name, List<TableVersion> tableVersions)
        {
            TableVersion tableVersion = tableVersions.FirstOrDefault(p => p.Name.ToLower() == name.ToLower());
            if (tableVersion != null) return tableVersion.Version;
            return 0;
        }

        // support build cu
        [HttpPost]
        public bool SyncEnd(string userName, string branchID, string sessionID, [FromBody] EndSync data)
        {
            SyncEndResult(userName, branchID, sessionID, data, "vi");
            return true;
        }

        [HttpPost]
        public EndSyncResult SyncEndResult(string userName, string branchID, string sessionID,[FromBody] EndSync data, string langID)
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
                    item.ReasonCode = item.ReasonCode ?? "";
                    item.Note = item.Note ?? ""; // build cu
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
                    item.SlsperID = item.SlsperID ?? userName; // build cu SlsperID = null nghia la build sales, build mowi co gan slsperid

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
                    item.SlsperID = item.SlsperID ?? userName; // build cu SlsperID = null nghia la build sales, build mowi co gan slsperid

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

                if(data.OrderManualDiscs != null)
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

                if (data.SalesRoutePlan != null)
                {
                    foreach (var item in data.SalesRoutePlan)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;
                        item.SlsPerID = userName;
                        _app.PPC_SyncSalesRoutePlan.Add(item);
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

                if (data.Competitor != null)
                {
                    foreach (var item in data.Competitor)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;
                        item.SlsperID = userName;
                        _app.PPC_SyncOM_Competitor.Add(item);
                    }
                }
                if (data.StockScanLots != null)
                {
                    foreach (var item in data.StockScanLots)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;
                        item.SlsPerID = userName;

                        _app.PPC_SyncStockScanLot.Add(item);
                    }
                }
                if (data.StockScans != null)
                {
                    foreach (var item in data.StockScans)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;
                        item.SlsPerID = userName;

                        _app.PPC_SyncStockScan.Add(item);
                    }
                }
                if (data.StockScanDets != null)
                {
                    foreach (var item in data.StockScanDets)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;
                        item.SlsPerID = userName;

                        _app.PPC_SyncStockScanDet.Add(item);
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
				if (data.FeedBackCustomers != null)
                {
                    foreach (var item in data.FeedBackCustomers)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;
                        item.SlsperID = userName;

                        _app.PPC_SyncFeedBackCustomer.Add(item);
                    }
                }

                if (data.FeedBackCustomerImages != null)
                {
                    foreach (var item in data.FeedBackCustomerImages)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;
                        item.SlsperID = userName;
                        item.ImageName = branchID + @"/" + userName + @"/" + Convert.ToDateTime(item.CreateDate).ToString("yyyyMM") + @"/" + item.ImageName;
                        _app.PPC_SyncFeedBackCustomerImage.Add(item);
                    }
                }

                if (data.RateStaffServices != null)
                {
                    foreach (var item in data.RateStaffServices)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;
                        item.SlsperID = userName;

                        _app.PPC_SyncRateStaffService.Add(item);
                    }
                }

                if (data.RateStaffServiceImages != null)
                {
                    foreach (var item in data.RateStaffServiceImages)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;
                        item.SlsperID = userName;
                        item.ImageName = branchID + @"/" + userName + @"/" + Convert.ToDateTime(item.CreateDate).ToString("yyyyMM") + @"/" + item.ImageName;
                        _app.PPC_SyncRateStaffServiceImage.Add(item);
                    }
                }

                if (data.RecordsFeedBackCustomers != null)
                {
                    foreach (var item in data.RecordsFeedBackCustomers)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;
                        item.SlsperID = userName;
                        item.FileNameRecording = branchID + @"/" + userName + @"/" + Convert.ToDateTime(item.CreateDate).ToString("yyyyMM") + @"/" + item.FileNameRecording;
                   
                        _app.PPC_SyncRecordsFeedBackCustomer.Add(item);
                    }
                }

                if (data.CompetitorSurveyResults != null)
				{
					foreach (var item in data.CompetitorSurveyResults)
					{
						item.SessionID = sessionID;
						item.BranchID = branchID;
						//item.SlsperID = userName;

						_app.PPC_SyncCompetitorSurveyResult.Add(item);
					}
				}

				if (data.CompetitorSurveyResultIMGs != null)
				{
					foreach (var item in data.CompetitorSurveyResultIMGs)
					{
						item.SessionID = sessionID;
						item.BranchID = branchID;
						//item.SlsperID = userName;
						item.ImageName = branchID + @"/" + userName + @"/" + Convert.ToDateTime(item.CreateDate).ToString("yyyyMM") + @"/" + item.ImageName;

						_app.PPC_SyncCompetitorSurveyResultIMG.Add(item);
					}
				}

                if (data.RecordInfos != null)
                {
                    foreach (var item in data.RecordInfos)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;

                        _app.PPC_SyncRecordInfo.Add(item);
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

                if (data.BookingSalesRefs != null)
                {
                    foreach (var item in data.BookingSalesRefs)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;
                        item.SlsperID = userName;
                      
                        _app.PPC_SyncBookingSalesRef.Add(item);
                    }
                }
				
				if (data.Surveys != null)
                {
                    foreach (var item in data.Surveys)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;
                        item.SlsperID = userName;
                        _app.PPC_SyncSurvey.Add(item);
                    }
                }
				
                _app.SaveChanges();

                _app.PPC_ProcessSyncEnd(branchID,userName,sessionID);

                EndSyncResult result = new EndSyncResult();
                result.VanSalesReleaseResults = ProcessVanSales(_app, data.Orders, langID);

                return result;
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
        public bool SyncStamp(string userName, string branchID, string sessionID, [FromBody] StampSync data)
        {
            try
            {
                _app.Configuration.AutoDetectChangesEnabled = false;
                _app.Configuration.EnsureTransactionsForFunctionsAndCommands = false;

               
                foreach (var item in data.ProgramStampRemarks)
                {
                    item.SessionID = sessionID;
                    item.BranchID = branchID;
                    item.SlsperID = userName;

                    _app.PPC_SyncProgramStampRemark.Add(item);
                }
               

                foreach (var item in data.ProgramStampRemarkImages)
                {
                    item.SessionID = sessionID;
                    item.BranchID = branchID;
                    item.SlsperID = userName;
                    item.ImageName = branchID + @"/" + userName + @"/" + Convert.ToDateTime(item.RemarkTime).ToString("yyyyMM") + @"/" + item.ImageName;
                    _app.PPC_SyncProgramStampRemarkImage.Add(item);
                }
                

                _app.SaveChanges();

                _app.PPC_ProcessSyncStamp(branchID, userName, sessionID);

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
        public bool SyncInvestigation(string userName, string branchID, string sessionID, [FromBody] InvestigationSync data)
        {
            try
            {
                _app.Configuration.AutoDetectChangesEnabled = false;
                _app.Configuration.EnsureTransactionsForFunctionsAndCommands = false;

                if (data.ExpertisePermanentAddresses != null)
                {
                    foreach (var item in data.ExpertisePermanentAddresses)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;
                        item.SlsperID = userName;

                        _app.PPC_SyncExpertisePermanentAddress.Add(item);
                    }

                }
                if (data.ExpertiseCurrentAddresses != null)
                {
                    foreach (var item in data.ExpertiseCurrentAddresses)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;
                        item.SlsperID = userName;

                        _app.PPC_SyncExpertiseCurrentAddress.Add(item);
                    }

                }

                if (data.ExpertiseBranches != null)
                {
                    foreach (var item in data.ExpertiseBranches)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;
                        item.SlsperID = userName;  

                        _app.PPC_SyncExpertiseBranch.Add(item);
                    }

                }
                if (data.ExpertiseCompanies != null)
                {
                    foreach (var item in data.ExpertiseCompanies)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;
                        item.SlsperID = userName;

                        _app.PPC_SyncExpertiseCompany.Add(item);
                    }

                }
                if (data.ExpertiseOthers != null)
                {
                    foreach (var item in data.ExpertiseOthers)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;
                        item.SlsperID = userName;

                        _app.PPC_SyncExpertiseOther.Add(item);
                    }

                }
                if (data.InvestigationImages != null)
                {
                    foreach (var item in data.InvestigationImages)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;
                        item.SlsperID = userName;
                        if (!string.IsNullOrEmpty(item.ImageName))
                        {
                            item.ImageName = branchID + @"/" + userName + @"/" + Convert.ToDateTime(item.VisitDate).ToString("yyyyMM") + @"/" + item.ImageName;
                        }

                        _app.PPC_SyncInvestigationImage.Add(item);
                    }
                }

                if (data.RecordsInvestigations != null)
                {
                    foreach (var item in data.RecordsInvestigations)
                    {
                        item.SessionID = sessionID;
                        item.BranchID = branchID;
                        item.SlsperID = userName;
                        item.FileNameRecording = branchID + @"/" + userName + @"/" + Convert.ToDateTime(item.CreateDate).ToString("yyyyMM") + @"/" + item.FileNameRecording;

                        _app.PPC_SyncInvestigationRecord.Add(item);
                    }
                }

                _app.SaveChanges();

                _app.PPC_ProcessSyncEnd_Investigation(branchID, userName, sessionID);

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
        public bool SyncLocation(string userName, string branchID, string sessionID, [FromBody] LocationSync data)
        {
            try
            {
                _app.Configuration.AutoDetectChangesEnabled = false;
                _app.Configuration.EnsureTransactionsForFunctionsAndCommands = false;

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

                _app.SaveChanges();

                _app.PPC_ProcessSyncLocation(branchID, userName, sessionID);

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
        public bool SyncOutSide(string userName, string branchID, string sessionID, [FromBody] OutSideSync data)
        {
            try
            {
                _app.Configuration.AutoDetectChangesEnabled = false;
                _app.Configuration.EnsureTransactionsForFunctionsAndCommands = false;


                PPC_SyncOutsideChecking item = data.outsideChecking;
                item.SessionID = sessionID;
                item.BranchID = branchID;
                item.SlsperID = userName;
                item.ImageName = "";
                item.Note = item.Note ?? "";
                item.DeviationType = item.DeviationType ?? "";
                item.ShiftID = item.ShiftID ?? "";

                _app.PPC_SyncOutsideChecking.Add(item);


                PPC_SyncSalesTrace traceItem = data.salesTrace;
                traceItem.SessionID = sessionID;
                traceItem.BranchID = branchID;
                traceItem.SlsperID = userName;
                //suport build cu
                traceItem.ReasonCode = traceItem.ReasonCode ?? "";
                traceItem.CustID = traceItem.CustID ?? "";

                _app.PPC_SyncSalesTrace.Add(traceItem);

                _app.SaveChanges();

                _app.PPC_ProcessSyncOutSide(branchID, userName, sessionID);

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
        public bool SyncApprovalReasonCheckIn(string userName, string branchID, string sessionID, [FromBody] ApprovalReasonCheckInSyncEnd data)
        {
            try
            {
                _app.Configuration.AutoDetectChangesEnabled = false;
                _app.Configuration.EnsureTransactionsForFunctionsAndCommands = false;


                PPC_SyncApprovalReasonCheckIn item = data.approvalReasonChecking;
                item.SessionID = sessionID;
                item.BranchID = branchID;
                item.SlsperID = userName;
                _app.PPC_SyncApprovalReasonCheckIn.Add(item);

				if (data.approvalReasonCheckInDetail != null) {
					foreach (var details in data.approvalReasonCheckInDetail)
					{
						details.SessionID = sessionID;
						details.BranchID = branchID;
						details.SlsperID = userName;

						_app.PPC_SyncApprovalReasonCheckInDetail.Add(details);
					}
				}
				
				_app.SaveChanges();

                _app.PPC_ProcessSyncApprovalReasonCheckIn(branchID, userName, sessionID);

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
        public bool SyncUpdateCust(string userName, string branchID, string sessionID, [FromBody] ApprovalSyncCustImage data)
        {
            try
            {
                _app.Configuration.AutoDetectChangesEnabled = false;
                _app.Configuration.EnsureTransactionsForFunctionsAndCommands = false;


                PPC_SyncApprovalEditCust item = data.ApprovalUpdateCust;
                item.SessionID = sessionID;
                item.BranchID = branchID;
                item.SlsperID = userName;
                _app.PPC_SyncApprovalEditCust.Add(item);

                _app.SaveChanges();

                _app.PPC_ProcessSyncApprovalUpdateCust(branchID, userName, sessionID);

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
        public bool SyncRegisterWorkingPlan(string userName, string branchID, string sessionID, [FromBody] RegisterWorkingPlanSync data)
        {
            try
            {
                _app.Configuration.AutoDetectChangesEnabled = false;
                _app.Configuration.EnsureTransactionsForFunctionsAndCommands = false;

                foreach (var item in data.RegisterWorkingPlanList)
                {
                    item.SessionID = sessionID;
                    item.BranchID = branchID;
                    item.SlsperID = userName;
                    _app.PPC_SyncRegisterWorkingPlan.Add(item);
                }

                _app.SaveChanges();

                _app.PPC_ProcessSyncRegisterWorkingPlan(branchID, userName, sessionID);

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
        public bool SyncWorkingPlan(string userName, string branchID, string sessionID, [FromBody] WorkingPlanSync data)
        {
            try
            {
                _app.Configuration.AutoDetectChangesEnabled = false;
                _app.Configuration.EnsureTransactionsForFunctionsAndCommands = false;


                //PPC_SyncSalesRoutePlan item = data.SalesRoutePlan;
                foreach (var item in data.SalesRoutePlan)
                {
                    item.SessionID = sessionID;
                    item.BranchID = branchID;
                    item.SlsPerID = userName;
                    _app.PPC_SyncSalesRoutePlan.Add(item);
                }

                _app.SaveChanges();

                _app.PPC_ProcessSyncWorkingPlan(branchID, userName, sessionID);

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
        public static List<VanSalesReleaseResult> ProcessVanSales(AppEntities app, List<PPC_SyncSalesOrd> orderList, string langID)
        {
            List<VanSalesReleaseResult> resultList = new List<VanSalesReleaseResult>();
            foreach (var item in orderList)
            {
                if (item.OrderType == "DO")
                {
                    DataAccess dal = DbHelper.Dal();
                    OrderProcess orderProcess = new OrderProcess(item.SlsperID, "SFA", dal);
                    try
                    {
                        dal.BeginTrans(System.Data.IsolationLevel.ReadUncommitted);
                       
                        orderProcess.ReleaseVanSales(item.BranchID, item.SalesID, item.SlsperID, item.OrderDate, item.OrderDate);

                        dal.CommitTrans();

                        resultList.Add(new VanSalesReleaseResult() { OrderNbr = item.OrderNbr, ErrorMessage = "", IsSuccess = true });
                    }
                    catch (Exception ex)
                    {
                        dal.RollbackTrans();
                        if (ex is ProcessException)
                        {
                            ProcessException procExp = (ProcessException)ex;
                            resultList.Add(new VanSalesReleaseResult() { OrderNbr = item.OrderNbr, ErrorMessage = LangHelper.Get(app, langID, procExp.Code, procExp.Parm), IsSuccess = false });
                        }
                        else
                        {
                            resultList.Add(new VanSalesReleaseResult() { OrderNbr = item.OrderNbr, ErrorMessage = ex.Message, IsSuccess = false });
                        }
                        orderProcess.DeleteVanSales(item.BranchID, item.SalesID);
                    }
                }
            }
            return resultList;
        }

		[HttpPost]
		public ReportRequestSync SyncGetRPT_KE_Request(string userName, string branchID, int version, string sessionID, string fromDate, string toDate, [FromBody] ReportRequestSyncEnd data)
        {
            try
            {
				_app.Configuration.AutoDetectChangesEnabled = false;
				_app.Configuration.EnsureTransactionsForFunctionsAndCommands = false;

				if (data.ke_RequestHeaders != null) {
					foreach (var item in data.ke_RequestHeaders)
					{
						item.SessionID = sessionID;
						item.BranchID = branchID;
						_app.PPC_SyncKE_RequestHeader.Add(item);
					}
				}

				if (data.ke_RequestDetails != null)
				{
					foreach (var item in data.ke_RequestDetails)
					{
						item.SessionID = sessionID;
						item.BranchID = branchID;
						_app.PPC_SyncKE_RequestDetail.Add(item);
					}
				}

				if (data.ke_RequestImage != null)
				{
					foreach (var item in data.ke_RequestImage)
					{
						item.SessionID = sessionID;
						item.BranchID = branchID;
						item.ImageName = branchID + @"/" + userName + @"/" + Convert.ToDateTime(item.CreateDate).ToString("yyyyMM") + @"/" + item.ImageName;
						_app.PPC_SyncKE_RequestImage.Add(item);
					}
				}

				_app.SaveChanges();

				_app.PPC_ProcessSyncEndKERequest(branchID, userName, sessionID);
				
                ReportRequestSync report = new ReportRequestSync();
				report.RequestHeaders = _app.PPC_GetRPT_KE_RequestHeader(branchID, userName, version, sessionID, fromDate, toDate).ToList();
				report.RequestDetails = _app.PPC_GetRPT_KE_RequestDetail(branchID, userName, version, sessionID, fromDate, toDate).ToList();
				report.RequestImages = _app.PPC_GetRPT_KE_RequestImage(branchID, userName, version, sessionID, fromDate, toDate).ToList();

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

                _app.PPC_ProcessSyncEnd_TDisplayCustomer(branchID, userName, sessionID);

                RegisterDisplayCustomerSync register = new RegisterDisplayCustomerSync();
                //Customer
                register.Salespersons = _app.PPC_GetAR_Salesperson(branchID, userName, version, sessionID).ToList();
                register.RouteDets = _app.PPC_GetOM_SalesRouteDetV2(branchID, userName, version, sessionID).ToList();
                register.Customers = _app.PPC_GetAR_CustomerV2(branchID, userName, version, sessionID).ToList();
                register.Addresses = _app.PPC_GetAR_SOAddressV2(branchID, userName, version, sessionID).ToList();
                //Display Customer
                register.Displays = _app.PPC_GetOM_Display(branchID, userName, version, sessionID).ToList();
                register.DisplayCusts = _app.PPC_GetOM_DisplayCust(branchID, userName, version, sessionID).ToList();
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
                        item.SalesID = item.SalesID?? "";
                        item.BudgetID = item.BudgetID?? "";
                        _app.PPC_SyncAccumulatedRegis.Add(item);
                    }
                }

                _app.SaveChanges();

                _app.PPC_ProcessSyncEnd_AccumulatedRegister(branchID, userName, sessionID);

                RegisterAccumulationSync register = new RegisterAccumulationSync();
                //Customer
                register.Salespersons = _app.PPC_GetAR_Salesperson(branchID, userName, version, sessionID).ToList();
                register.RouteDets = _app.PPC_GetOM_SalesRouteDetV2(branchID, userName, version, sessionID).ToList();
                register.Customers = _app.PPC_GetAR_CustomerV2(branchID, userName, version, sessionID).ToList();
                register.Addresses = _app.PPC_GetAR_SOAddressV2(branchID, userName, version, sessionID).ToList();
                //Accumulation
                register.Accumulated = _app.PPC_GetOM_Accumulated(branchID, userName, version, sessionID).ToList();
                register.AccumulatedRegis = _app.PPC_GetOM_AccumulatedRegis(branchID, userName, version, sessionID).ToList();
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
        public RegisterSalesRouteSync SyncGetRegisterSalesRoute(string userName, string branchID, int version, string sessionID, [FromBody] RegisterSalesRouteSyncEnd data)
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

                _app.PPC_ProcessSyncEnd_RegisterSalesRoute(branchID, userName, sessionID);

                RegisterSalesRouteSync registerSalesRouteSync = new RegisterSalesRouteSync();
                registerSalesRouteSync.RegisterSalesRoute = _app.PPC_GetPPC_RegisterSalesRoute(branchID, userName, version, sessionID).ToList();
                return registerSalesRouteSync;
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
        public DebtSync SyncGetDebt(string userName, string branchID, int version, string sessionID)
        {
            try
            {
                DebtSync data = new DebtSync();
                data.Doces = _app.PPC_GetAR_Doc(sessionID, version, branchID, userName).ToList();
                data.Balances = _app.PPC_GetAR_Balances(sessionID, version, branchID, userName).ToList();

                return data;
            }
            catch (Exception ex)
            {
                throw Request.CreateResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }


        [HttpGet]
        public SearchProductInformationSync GetSearchProductInformation(string userName, string branchID, int version, string sessionID, string invtID)
        {
            try
            {
                SearchProductInformationSync data = new SearchProductInformationSync();
                data.SearchProductInformationPromotions = _app.PPC_GetSearchProductInformationPromotion(branchID , userName , version , sessionID, invtID).ToList();
                data.SearchProductInformationDisplays = _app.PPC_GetSearchProductInformationDisplay(branchID, userName, version, sessionID, invtID).ToList();
                data.SearchProductInformationAccumulations = _app.PPC_GetSearchProductInformationAccumulation(branchID, userName, version, sessionID, invtID).ToList();

                return data;
            }
            catch (Exception ex)
            {
                throw Request.CreateResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [HttpGet]
        public ReportDisplaySync GetReportDisplay(string userName, string branchID, int version, string sessionID, string displayID)
        {
            try
            {
                ReportDisplaySync data = new ReportDisplaySync();
                data.DisplayRewards = _app.PPC_GetRPT_DisplayReward(branchID, userName, version, sessionID, displayID).ToList();
               

                return data;
            }
            catch (Exception ex)
            {
                throw Request.CreateResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [HttpGet]
        public ReportAccumulatedSync GetReportAccumulation(string userName, string branchID, int version, string sessionID, string accumulateID)
        {
            try
            {
                ReportAccumulatedSync data = new ReportAccumulatedSync();
                data.AccumulatedRewards = _app.PPC_GetRPT_AccumulatedReward(branchID, userName, version, sessionID, accumulateID).ToList();
                return data;
            }
            catch (Exception ex)
            {
                throw Request.CreateResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [HttpPost]
        public CheckBranchTimeKeepingSync CheckBranchTimeKeeping(string userName, string branchID, string visitDate, int version, string sessionID)
        {

            try
            {
                CheckBranchTimeKeepingSync data = new CheckBranchTimeKeepingSync();
                data.CheckBranchTimeKeepings = _app.PPC_CheckBranchTimeKeeping(branchID, userName, visitDate, version, sessionID).ToList();
                return data;
            }
            catch (Exception ex)
            {
                throw Request.HandleError(ex);
            }
            
        }
    }



}
