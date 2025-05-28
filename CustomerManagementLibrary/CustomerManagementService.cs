using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using CustomerManagementLibrary.Models;
using DatabaseManager;
using Newtonsoft.Json;

namespace CustomerManagementLibrary
{
    public class CustomerManagementService : ICustomerManagementService
    {
        
        public async Task<ResponseModel> GetCompany()
        {
            ResponseModel responseModal = new ResponseModel();

            ArrayList arrList = new ArrayList();

            try
            {
                DALOR.spArgumentsCollection(arrList, "@p_flag", "I", "Char", "I", 1);




                DALOR.spArgumentsCollection(arrList, "@ret", "", "VARCHAR", "O");

                DALOR.spArgumentsCollection(arrList, "@errormsg", "", "VARCHAR", "O", 4000);
                DALOR.spArgumentsCollection(arrList, "g_ResultSet", "", "REFCURSOR", "O");

                DataSet ds = new DataSet();
                var res = DALOR.RunStoredProcedureDsRetError("sp_GetCompany", arrList, ds);

                responseModal.code = res.Ret;
                responseModal.msg = res.ErrorMsg;

                if (res.Ret > 0 && ds != null && ds.Tables.Count > 0)
                {
                    //responseModal.data = JsonSerializer.Serialize(ds.Tables[0]);
                    responseModal.data = JsonConvert.SerializeObject(ds.Tables[0]);
                }
                else
                {
                    responseModal.data = string.Empty;
                }
            }
            catch (Exception ex)
            {
                responseModal.code = -1;
                responseModal.msg = "Error Occurred, Could Not Get Company Details";
                responseModal.data = string.Empty;
            }

            return responseModal;
        }

        public async Task<ResponseModel> GetOrderDetailsMaster(string customercode, string companycode)
        {
            ResponseModel responseModel = new ResponseModel();

            try
            {
                if (string.IsNullOrEmpty(customercode) || string.IsNullOrEmpty(companycode))
                {
                    responseModel.code = -1;
                    responseModel.msg = "Customer Code and Company Code is required.";
                }
                else
                {

                    DataSet ds = new DataSet();
                    ArrayList arrList = new ArrayList();
                    DALOR.spArgumentsCollection(arrList, "@p_flag", "M", "Char", "I", 1);
                    DALOR.spArgumentsCollection(arrList, "g_CustomerCode", customercode, "CHAR", "I", 5);
                    
                    DALOR.spArgumentsCollection(arrList, "g_Company_Code", companycode != null ? companycode : "0", "INT", "I");
                    DALOR.spArgumentsCollection(arrList, "g_OrdeNumber", "0", "INT", "I");



                    DALOR.spArgumentsCollection(arrList, "@ret", "", "VARCHAR", "O");

                    DALOR.spArgumentsCollection(arrList, "@errormsg", "", "VARCHAR", "O", 4000);
                    DALOR.spArgumentsCollection(arrList, "g_ResultSet", "", "REFCURSOR", "O");
                    var res = DALOR.RunStoredProcedureDsRetError("sp_GetOrderDetails", arrList, ds);

                    if (res.Ret > 0)
                    {
                        if (res.Ret == 1 && ds != null && ds.Tables.Count > 0)
                        {
                            //responseModal.data = JsonSerializer.Serialize(ds.Tables[0]);
                            responseModel.code = res.Ret;
                            responseModel.msg = res.ErrorMsg;
                            responseModel.data = JsonConvert.SerializeObject(ds.Tables[0]);
                        }
                        

                        
                        else
                        {
                            responseModel.code = 2;
                            responseModel.data = string.Empty;
                            responseModel.msg = "User order details are empty";
                        }
                    }

                    else
                    {
                        responseModel.code = -2;
                        responseModel.msg = "Invalid Operation";
                    }

                }
            }
            catch (Exception ex)
            {
                responseModel.code = -1;
                responseModel.msg = $"An error occurred while processing the request: {ex.Message}";
            }
            return responseModel;
        }
        public async Task<ResponseModel> GetOrderDetailsItem(string customercode, string companycode, string ordercode)
        {
            ResponseModel responseModel = new ResponseModel();

            try
            {
                if (string.IsNullOrEmpty(customercode) || string.IsNullOrEmpty(companycode) || string.IsNullOrEmpty(ordercode))
                {
                    responseModel.code = -1;
                    responseModel.msg = "Customer Code, order number and Company Code is required.";
                }
                else
                {

                    DataSet ds = new DataSet();
                    ArrayList arrList = new ArrayList();
                    DALOR.spArgumentsCollection(arrList, "@p_flag", "I", "Char", "I", 1);
                    DALOR.spArgumentsCollection(arrList, "g_CustomerCode", customercode, "CHAR", "I", 5);

                    DALOR.spArgumentsCollection(arrList, "g_Company_Code", companycode != null ? companycode : "0", "INT", "I");
                    DALOR.spArgumentsCollection(arrList, "g_OrdeNumber", ordercode != null ? ordercode : "0", "INT", "I");



                    DALOR.spArgumentsCollection(arrList, "@ret", "", "VARCHAR", "O");

                    DALOR.spArgumentsCollection(arrList, "@errormsg", "", "VARCHAR", "O", 4000);
                    DALOR.spArgumentsCollection(arrList, "g_ResultSet", "", "REFCURSOR", "O");
                    var res = DALOR.RunStoredProcedureDsRetError("sp_GetOrderDetails", arrList, ds);

                    if (res.Ret > 0)
                    {
                        if (res.Ret == 1 && ds != null && ds.Tables.Count > 0)
                        {
                            //responseModal.data = JsonSerializer.Serialize(ds.Tables[0]);
                            responseModel.code = res.Ret;
                            responseModel.msg = res.ErrorMsg;
                            responseModel.data = JsonConvert.SerializeObject(ds.Tables[0]);
                        }



                        else
                        {
                            responseModel.code = 2;
                            responseModel.data = string.Empty;
                            responseModel.msg = "User order details are empty";
                        }
                    }

                    else
                    {
                        responseModel.code = -2;
                        responseModel.msg = "Invalid Operation";
                    }

                }
            }
            catch (Exception ex)
            {
                responseModel.code = -1;
                responseModel.msg = $"An error occurred while processing the request: {ex.Message}";
            }
            return responseModel;
        }

        public async Task<ResponseModel> GetAllItems(string companycode)
        {
            ResponseModel responseModal = new ResponseModel();

            ArrayList arrList = new ArrayList();

            try
            {
                //DALOR.spArgumentsCollection(arrList, "@p_flag", "I", "Char", "I", 1);
                DALOR.spArgumentsCollection(arrList, "g_Company_Code", companycode != null ? companycode : "0", "INT", "I");



                DALOR.spArgumentsCollection(arrList, "@ret", "", "VARCHAR", "O");

                DALOR.spArgumentsCollection(arrList, "@errormsg", "", "VARCHAR", "O", 4000);
                DALOR.spArgumentsCollection(arrList, "g_ResultSet", "", "REFCURSOR", "O");

                DataSet ds = new DataSet();
                var res = DALOR.RunStoredProcedureDsRetError("sp_GetAllItems", arrList, ds);

                responseModal.code = res.Ret;
                responseModal.msg = res.ErrorMsg;

                if (res.Ret > 0 && ds != null && ds.Tables.Count > 0)
                {
                    //responseModal.data = JsonSerializer.Serialize(ds.Tables[0]);
                    responseModal.data = JsonConvert.SerializeObject(ds.Tables[0]);
                }
                else
                {
                    responseModal.data = string.Empty;
                }
            }
            catch (Exception ex)
            {
                responseModal.code = -1;
                responseModal.msg = "Error Occurred, Could Not Get Item Details";
                responseModal.data = string.Empty;
            }

            return responseModal;
        }


        public async Task<ResponseModel> CreateOrder(OrderDetailModel odm)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                //byte[] fileBytes = Convert.FromBase64String(odm.AttachFile);
                //req.Password = await encDcService.Encrypt(req.Password);

                ArrayList arrList = new ArrayList();
                DALOR.spArgumentsCollection(arrList, "p_flag", "M", "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_COMPANY_CODE", odm.CompanyCode, "INT", "I");
                DALOR.spArgumentsCollection(arrList, "p_SUPP_CUST_CODE", odm.SuppCustCode, "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_ATTACH_FILE", odm.AttachFile, "BLOB", "I");

                DALOR.spArgumentsCollection(arrList, "p_CONTENT_TYPE", odm.ContentType, "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_FILE_NAME", odm.FileName, "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_SHIP_TO_PARTY", odm.ShipToParty, "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_SHIP_TO_ADDRESS", odm.ShipToAddress, "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_TRANSPORTER_NAME", odm.TransporterName, "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_BOOKING_STATION", odm.BookingStation, "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_CUSTOMER_PO_NO", odm.CustomerPoNo, "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_SCHOOL_NAME", odm.SchoolName, "VARCHAR", "I");

                DALOR.spArgumentsCollection(arrList, "p_ITEM_CODE", "", "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_ITEM_QTY", "0", "INT", "I");

                DALOR.spArgumentsCollection(arrList, "p_SerialNum", "0", "INT", "I");
                DALOR.spArgumentsCollection(arrList, "p_insertedValue", "0", "INT", "I");

                DALOR.spArgumentsCollection(arrList, "@ret", "", "VARCHAR", "O");
                DALOR.spArgumentsCollection(arrList, "@errormsg", "", "VARCHAR", "O");
                //DALOR.spArgumentsCollection(arrList, "g_ResultSet", "", "REFCURSOR", "O");
                var res = DALOR.RunStoredProcedureDsRetError("sp_CreateUpdateDeleteOrder", arrList);
                response.code = res.Ret;
                response.msg = res.ErrorMsg;
                if (response.code > 0 )
                {
                    int i = 0;
                    foreach (var item in odm.Items)
                    {
                        i += 1;
                        arrList = new ArrayList();
                        DALOR.spArgumentsCollection(arrList, "p_flag", "I", "VARCHAR", "I");
                        //DALOR.spArgumentsCollection(arrList, "p_COMPANY_CODE", odm.CompanyCode, "INT", "I");
                        DALOR.spArgumentsCollection(arrList, "p_COMPANY_CODE", odm.CompanyCode, "INT", "I");
                        DALOR.spArgumentsCollection(arrList, "p_SUPP_CUST_CODE", odm.SuppCustCode, "VARCHAR", "I");
                        DALOR.spArgumentsCollection(arrList, "p_ATTACH_FILE", null, "BLOB", "I");

                        DALOR.spArgumentsCollection(arrList, "p_CONTENT_TYPE", odm.ContentType, "VARCHAR", "I");
                        DALOR.spArgumentsCollection(arrList, "p_FILE_NAME", odm.FileName, "VARCHAR", "I");
                        DALOR.spArgumentsCollection(arrList, "p_SHIP_TO_PARTY", odm.ShipToParty, "VARCHAR", "I");
                        DALOR.spArgumentsCollection(arrList, "p_SHIP_TO_ADDRESS", odm.ShipToAddress, "VARCHAR", "I");
                        DALOR.spArgumentsCollection(arrList, "p_TRANSPORTER_NAME", odm.TransporterName, "VARCHAR", "I");
                        DALOR.spArgumentsCollection(arrList, "p_BOOKING_STATION", odm.BookingStation, "VARCHAR", "I");
                        DALOR.spArgumentsCollection(arrList, "p_CUSTOMER_PO_NO", odm.CustomerPoNo, "VARCHAR", "I");
                        DALOR.spArgumentsCollection(arrList, "p_SCHOOL_NAME", odm.SchoolName, "VARCHAR", "I");


                        DALOR.spArgumentsCollection(arrList, "p_ITEM_CODE", item.ITEM_CODE, "VARCHAR", "I");
                        DALOR.spArgumentsCollection(arrList, "p_ITEM_QTY", item.ITEM_QTY, "INT", "I");

                        DALOR.spArgumentsCollection(arrList, "p_SerialNum", i.ToString(), "INT", "I");
                        DALOR.spArgumentsCollection(arrList, "p_insertedValue", response.code.ToString(), "INT", "I");

                        DALOR.spArgumentsCollection(arrList, "@ret", "", "VARCHAR", "O");
                        DALOR.spArgumentsCollection(arrList, "@errormsg", "", "VARCHAR", "O");
                        res = DALOR.RunStoredProcedureDsRetError("sp_CreateUpdateDeleteOrder", arrList);
                        // Your logic here
                    }
                    
                }
                

            }
            catch (Exception ex)
            {
                response.code = -1;
                response.data = ex.Message;
            }
            return await Task.FromResult(response);
        }
        public async Task<ResponseModel> UpdateOrder(OrderDetailModel odm)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                //req.Password = await encDcService.Encrypt(req.Password);

                ArrayList arrList = new ArrayList();
                DALOR.spArgumentsCollection(arrList, "p_flag", "U", "VARCHAR", "I");
                //DALOR.spArgumentsCollection(arrList, "p_COMPANY_CODE", "0", "INT", "I");
                DALOR.spArgumentsCollection(arrList, "p_COMPANY_CODE", odm.CompanyCode, "INT", "I");
                DALOR.spArgumentsCollection(arrList, "p_SUPP_CUST_CODE", odm.SuppCustCode, "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_ATTACH_FILE", null, "BLOB", "I");
                
                DALOR.spArgumentsCollection(arrList, "p_CONTENT_TYPE", odm.ContentType ?? "Default", "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_FILE_NAME", odm.FileName ?? "Default", "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_SHIP_TO_PARTY", odm.ShipToParty ?? "Default", "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_SHIP_TO_ADDRESS", odm.ShipToAddress ?? "Default", "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_TRANSPORTER_NAME", odm.TransporterName ?? "Default", "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_BOOKING_STATION", odm.BookingStation ?? "Default", "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_CUSTOMER_PO_NO", odm.CustomerPoNo ?? "Default", "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_SCHOOL_NAME", odm.SchoolName ?? "Default", "VARCHAR", "I");
                
                //// here is the end of ectras

                DALOR.spArgumentsCollection(arrList, "p_ITEM_CODE", odm.Items[0].ITEM_CODE, "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_ITEM_QTY", odm.Items[0].ITEM_QTY, "INT", "I");

                DALOR.spArgumentsCollection(arrList, "p_SerialNum", "11", "INT", "I");
                DALOR.spArgumentsCollection(arrList, "p_insertedValue", odm.AutoKeyOrder.ToString(), "INT", "I");

                DALOR.spArgumentsCollection(arrList, "@ret", "", "VARCHAR", "O");
                DALOR.spArgumentsCollection(arrList, "@errormsg", "", "VARCHAR", "O");
                var res = DALOR.RunStoredProcedureDsRetError("sp_CreateUpdateDeleteOrder", arrList);
                response.code = res.Ret;
                response.msg = res.ErrorMsg;
                

            }
            catch (Exception ex)
            {
                response.code = -1;
                response.data = ex.Message;
            }
            return await Task.FromResult(response);
        }
        public async Task<ResponseModel> DeleteOrder(OrderDetailModel odm)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                //req.Password = await encDcService.Encrypt(req.Password);

                ArrayList arrList = new ArrayList();
                DALOR.spArgumentsCollection(arrList, "p_flag", "D", "VARCHAR", "I");
                //DALOR.spArgumentsCollection(arrList, "p_COMPANY_CODE", "0", "INT", "I");
                DALOR.spArgumentsCollection(arrList, "p_COMPANY_CODE", odm.CompanyCode, "INT", "I");
                DALOR.spArgumentsCollection(arrList, "p_SUPP_CUST_CODE", odm.SuppCustCode, "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_ATTACH_FILE", null, "BLOB", "I");

                DALOR.spArgumentsCollection(arrList, "p_CONTENT_TYPE", odm.ContentType, "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_FILE_NAME", odm.FileName, "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_SHIP_TO_PARTY", odm.ShipToParty, "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_SHIP_TO_ADDRESS", odm.ShipToAddress, "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_TRANSPORTER_NAME", odm.TransporterName, "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_BOOKING_STATION", odm.BookingStation, "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_CUSTOMER_PO_NO", odm.CustomerPoNo, "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_SCHOOL_NAME", odm.SchoolName, "VARCHAR", "I");
                
                
                //// here is the end of ectras


                DALOR.spArgumentsCollection(arrList, "p_ITEM_CODE", odm.Items[0].ITEM_CODE, "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_ITEM_QTY", "0", "INT", "I");

                DALOR.spArgumentsCollection(arrList, "p_SerialNum", "0", "INT", "I");
                DALOR.spArgumentsCollection(arrList, "p_insertedValue", odm.AutoKeyOrder.ToString(), "VARCHAR", "I");

                DALOR.spArgumentsCollection(arrList, "@ret", "", "VARCHAR", "O");
                DALOR.spArgumentsCollection(arrList, "@errormsg", "", "VARCHAR", "O");
                var res = DALOR.RunStoredProcedureRetError("sp_CreateUpdateDeleteOrder", arrList);
                response.code = res.Ret;
                response.msg = res.ErrorMsg;


            }
            catch (Exception ex)
            {
                response.code = -1;
                response.data = ex.Message;
            }
            return await Task.FromResult(response);
        }


        public async Task<ResponseModel> GetDashboardMetrics(DashboardMetricRequest dmReq)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                ArrayList arrList = new ArrayList();

                DALOR.spArgumentsCollection(arrList, "P_CUSTOMER_CODE", dmReq.CustomerCode, "CHAR", "I", 65);
                DALOR.spArgumentsCollection(arrList, "P_COMPANY_CODE", dmReq.CompanyCode, "INT", "I");
                DALOR.spArgumentsCollection(arrList, "P_FLAG", dmReq.DataRange, "VARCHAR", "I");

                DALOR.spArgumentsCollection(arrList, "@ret", "", "VARCHAR", "O");
                DALOR.spArgumentsCollection(arrList, "@errormsg", "", "VARCHAR", "O");

                DALOR.spArgumentsCollection(arrList, "O_METRIC_CARDS", "", "REFCURSOR", "O");
                DALOR.spArgumentsCollection(arrList, "O_DAILY_ORDER_CHART", "", "REFCURSOR", "O");
                DALOR.spArgumentsCollection(arrList, "O_LAST_6_MONTHS_CHART", "", "REFCURSOR", "O");
                DALOR.spArgumentsCollection(arrList, "O_TOP_10_ITEMS_CHART", "", "REFCURSOR", "O");
                DALOR.spArgumentsCollection(arrList, "o_lifetime_ordercount", "", "REFCURSOR", "O");





                DataSet ds = new DataSet();
                var res = DALOR.RunStoredProcedureDsRetError("G_SP_GET_ORDER_METRICS2", arrList, ds);
                response.code = res.Ret;
                response.msg = res.ErrorMsg;

                if (res.Ret > 0)
                {
                    DashboardMetric dashboardMetric = new DashboardMetric();

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        dashboardMetric.Metrics = new List<MetricCardsModel>();
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            dashboardMetric.Metrics.Add(new MetricCardsModel
                            {
                                TotalOrders = Convert.ToInt32(row["TOTAL_ORDERS"]),
                                OrderDate = row["ORDER_DAY"].ToString(),
                                ChartType = row["CHART_TYPE"].ToString()
                            });
                        }

                    }

                    // Table 1: Daily Order Chart
                    if (ds.Tables.Count > 1)
                    {
                        dashboardMetric.DailyOrders = new List<DailyOrderChartModel>();
                        foreach (DataRow row in ds.Tables[1].Rows)
                        {

                            dashboardMetric.DailyOrders.Add(new DailyOrderChartModel
                            {
                                OrderDay = Convert.ToDateTime(row["ORDER_DAY"]),
                                OrderCount = Convert.ToInt32(row["ORDER_COUNT"])
                            });
                        }
                    }

                    // Table 2: Last 6 Months Chart
                    if (ds.Tables.Count > 2)
                    {
                        dashboardMetric.LastSixMonthsItems = new List<LastSixMonthsItemChartModel>();
                        foreach (DataRow row in ds.Tables[2].Rows)
                        {
                            dashboardMetric.LastSixMonthsItems.Add(new LastSixMonthsItemChartModel
                            {
                                Month = row["ORDER_MONTH"].ToString(),
                                ItemStatus = row["ITEMSTATUS"].ToString(),
                                ItemCount = Convert.ToInt32(row["ITEM_COUNT"]),
                                TotalQty = Convert.ToDecimal(row["TOTAL_QTY"])
                            });
                        }
                    }

                    // Table 3: Top 10 Items
                    if (ds.Tables.Count > 3)
                    {
                        dashboardMetric.Top10Items = new List<Top10ItemsChartModel>();
                        foreach (DataRow row in ds.Tables[3].Rows)
                        {
                            dashboardMetric.Top10Items.Add(new Top10ItemsChartModel
                            {
                                ItemCode = row["ITEM_CODE"].ToString(),
                                ItemShortDesc = row["ITEM_SHORT_DESC"].ToString(),
                                TotalItemCount = Convert.ToInt32(row["TOTAL_ITEM_COUNT"]),
                                TotalItemQty = Convert.ToInt32(row["TOTAL_ITEM_QTY"])
                            });
                        }
                    }

                    // Table 4: All time order count and data last updated datetime
                    if (ds.Tables.Count > 4)
                    {
                        var row = ds.Tables[4].Rows[0];
                        dashboardMetric.DataCountAndDateInfo = new ExtendedInfo()
                        {
                            AllOrderCount = Convert.ToInt32(row["AllOrderCount"]),
                            DataLastUpdated = row["LasteUpdatedDate"].ToString()
                        };
                    }

                    response.data = JsonConvert.SerializeObject(dashboardMetric);
                }
            }
            catch (Exception ex)
            {
                response.code = -1;
                response.msg = "Error Getting Dashbaord data";
                response.data = string.Empty;
            }

            return await Task.FromResult(response);


        }
        public async Task<ResponseModel> GetDashboardMetrics2(DashboardMetricRequest2 dmReq)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                ArrayList arrList = new ArrayList();

                DALOR.spArgumentsCollection(arrList, "P_CUSTOMER_CODE", dmReq.CustomerCode, "CHAR", "I", 65);
                DALOR.spArgumentsCollection(arrList, "P_COMPANY_CODE", dmReq.CompanyCode, "INT", "I");
                DALOR.spArgumentsCollection(arrList, "P_FLAG", dmReq.TabId, "VARCHAR", "I");


                DALOR.spArgumentsCollection(arrList, "@ret", "", "VARCHAR", "O");
                DALOR.spArgumentsCollection(arrList, "@errormsg", "", "VARCHAR", "O");

                DALOR.spArgumentsCollection(arrList, "O_DATA_LASTUPDATED", "", "REFCURSOR", "O");
                DALOR.spArgumentsCollection(arrList, "O_METRIC_CARDS", "", "REFCURSOR", "O");
                DALOR.spArgumentsCollection(arrList, "O_DAILY_ORDER_CHART", "", "REFCURSOR", "O");
                DALOR.spArgumentsCollection(arrList, "O_LAST_6_MONTHS_CHART", "", "REFCURSOR", "O");
                DALOR.spArgumentsCollection(arrList, "O_TOP_10_ITEMS_CHART", "", "REFCURSOR", "O");

                DataSet ds = new DataSet();
                var res = DALOR.RunStoredProcedureDsRetError("G_SP_GET_ORDER_METRICS3", arrList, ds);
                response.code = res.Ret;
                response.msg = res.ErrorMsg;

                switch (dmReq.TabId)
                {
                    case "T1":
                        DashboardChartDataT1 dashboardChartDataT1 = new DashboardChartDataT1();

                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            dashboardChartDataT1.DateLastUpdated = ds.Tables[0].Rows[0]["DATA_LAST_UPDATED"].ToString();
                        }

                        if (ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
                        {
                            dashboardChartDataT1.MetricCards = new List<MetricCard>();
                            foreach (DataRow row in ds.Tables[1].Rows)
                            {
                                dashboardChartDataT1.MetricCards.Add(new MetricCard
                                {
                                    TotalOrders = Convert.ToInt32(row["TOTAL_ORDERS"]),
                                    DayOrQuarter = row["DAY_OR_QUARTER"].ToString(),
                                    ChartType = row["CHART_TYPE"].ToString()
                                });
                            }

                        }

                        //Order Trend Chart
                        if (ds.Tables.Count > 1)
                        {
                            dashboardChartDataT1.OrderTrends = new List<OrderTrend>();
                            foreach (DataRow row in ds.Tables[2].Rows)
                            {

                                dashboardChartDataT1.OrderTrends.Add(new OrderTrend
                                {
                                    OrderDayOrQuarter = row["DAY_OR_QUARTER"].ToString(),
                                    OrderCount = Convert.ToInt32(row["ORDER_COUNT"])
                                });
                            }
                        }

                        response.data = JsonConvert.SerializeObject(dashboardChartDataT1);

                        break;

                    case "T2":
                        DashboardChartDataT2 dashboardChartDataT2 = new DashboardChartDataT2();
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            dashboardChartDataT2.DateLastUpdated = ds.Tables[0].Rows[0]["DATA_LAST_UPDATED"].ToString();
                        }

                        if (ds.Tables.Count > 2 && ds.Tables[1].Rows.Count > 0)
                        {
                            dashboardChartDataT2.LastSixMonthsItemsChart = new List<LastSixMonthsItemChart>();
                            foreach (DataRow row in ds.Tables[1].Rows)
                            {
                                dashboardChartDataT2.LastSixMonthsItemsChart.Add(new LastSixMonthsItemChart
                                {
                                    Month = row["ORDER_MONTH"].ToString(),
                                    ItemStatus = row["ITEM_STATUS"].ToString(),
                                    ItemCount = Convert.ToInt32(row["ITEM_COUNT"]),
                                    TotalQty = Convert.ToDecimal(row["TOTAL_QTY"])
                                });
                            }
                        }

                        if (ds.Tables.Count > 2 && ds.Tables[2].Rows.Count > 0)
                        {
                            dashboardChartDataT2.Top10ItemsChart = new List<Top10ItemsChart>();
                            foreach (DataRow row in ds.Tables[2].Rows)
                            {
                                dashboardChartDataT2.Top10ItemsChart.Add(new Top10ItemsChart
                                {
                                    ItemCode = row["ITEM_CODE"].ToString(),
                                    ItemShortDesc = row["ITEM_DESCRIPTION"].ToString(),
                                    TotalItemCount = Convert.ToInt32(row["TOTAL_ITEM_COUNT"]),
                                    TotalItemQty = Convert.ToDecimal(row["TOTAL_ITEM_QTY"])
                                });
                            }
                        }


                        response.data = JsonConvert.SerializeObject(dashboardChartDataT2);

                        break;
                    case "T3":
                        DashboardChartDataT3 dashboardChartDataT3 = new DashboardChartDataT3();

                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            dashboardChartDataT3.DateLastUpdated = ds.Tables[0].Rows[0]["DATA_LAST_UPDATED"].ToString();
                        }


                        if (ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
                        {
                            dashboardChartDataT3.MetricCards = new List<MetricCard>();
                            foreach (DataRow row in ds.Tables[1].Rows)
                            {
                                dashboardChartDataT3.MetricCards.Add(new MetricCard
                                {
                                    TotalOrders = Convert.ToInt32(row["TOTAL_ORDERS"]),
                                    DayOrQuarter = row["DAY_OR_QUARTER"].ToString(),
                                    ChartType = row["CHART_TYPE"].ToString()
                                });
                            }

                        }

                        //Order Trend Chart
                        if (ds.Tables.Count > 1)
                        {
                            dashboardChartDataT3.OrderTrends = new List<OrderTrend>();
                            foreach (DataRow row in ds.Tables[2].Rows)
                            {

                                dashboardChartDataT3.OrderTrends.Add(new OrderTrend
                                {
                                    OrderDayOrQuarter = row["DAY_OR_QUARTER"].ToString(),
                                    OrderCount = Convert.ToInt32(row["ORDER_COUNT"])
                                });
                            }
                        }


                        response.data = JsonConvert.SerializeObject(dashboardChartDataT3);
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                response.code = -1;
                response.msg = "Error Getting Dashbaord data";
                response.data = string.Empty;
            }

            return response;
        }
        public async Task<ResponseModel> GetInvoicedata(string cid1, string cid2, string cid3)
        {
            ResponseModel responseModal = new ResponseModel();

            ArrayList arrList = new ArrayList();

            try
            {
                //DALOR.spArgumentsCollection(arrList, "@p_flag", "M", "Char", "I", 1);
                DALOR.spArgumentsCollection(arrList, "g_Mkey", cid1, "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "g_Cust", cid2, "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "g_Company", cid3, "VARCHAR", "I");



                DALOR.spArgumentsCollection(arrList, "@ret", "", "VARCHAR", "O");

                DALOR.spArgumentsCollection(arrList, "@errormsg", "", "VARCHAR", "O", 4000);
                DALOR.spArgumentsCollection(arrList, "g_ResultSet", "", "REFCURSOR", "O");

                DataSet ds = new DataSet();
                var res = DALOR.RunStoredProcedureDsRetError("sp_GetInvoiceDetail", arrList, ds);

                responseModal.code = res.Ret;
                responseModal.msg = res.ErrorMsg;

                if (res.Ret > 0 && ds != null && ds.Tables.Count > 0)
                {
                    //responseModal.data = JsonSerializer.Serialize(ds.Tables[0]);
                    responseModal.data = JsonConvert.SerializeObject(ds.Tables[0]);
                }
                else
                {
                    responseModal.data = string.Empty;
                }
            }
            catch (Exception ex)
            {
                responseModal.code = -1;
                responseModal.msg = "Error Occurred, Could Not Get Item Details";
                responseModal.data = string.Empty;
            }

            return responseModal;
        }
        public async Task<ResponseModel> GetAllInvoice(string customercode, string companycode, string? ordercode, string? from, string? to)
        {
            ResponseModel responseModal = new ResponseModel();

            ArrayList arrList = new ArrayList();

            try
            {
                DALOR.spArgumentsCollection(arrList, "@p_flag", "M", "Char", "I", 1);
                DALOR.spArgumentsCollection(arrList, "g_CustomerCode", customercode, "CHAR", "I", 5);

                DALOR.spArgumentsCollection(arrList, "g_Company_Code", companycode != null ? companycode : "0", "INT", "I");
                DALOR.spArgumentsCollection(arrList, "g_OrdeNumber", ordercode != null ? ordercode : "0", "INT", "I");
                DALOR.spArgumentsCollection(arrList, "g_From", from, "DATE", "I");
                DALOR.spArgumentsCollection(arrList, "g_To", to, "DATE", "I");


                DALOR.spArgumentsCollection(arrList, "@ret", "", "VARCHAR", "O");

                DALOR.spArgumentsCollection(arrList, "@errormsg", "", "VARCHAR", "O", 4000);
                DALOR.spArgumentsCollection(arrList, "g_ResultSet", "", "REFCURSOR", "O");

                DataSet ds = new DataSet();
                var res = DALOR.RunStoredProcedureDsRetError("sp_GetInvoice", arrList, ds);

                responseModal.code = res.Ret;
                responseModal.msg = res.ErrorMsg;

                if (res.Ret > 0 && ds != null && ds.Tables.Count > 0)
                {
                    //responseModal.data = JsonSerializer.Serialize(ds.Tables[0]);
                    responseModal.data = JsonConvert.SerializeObject(ds.Tables[0]);
                }
                else
                {
                    responseModal.data = string.Empty;
                }
            }
            catch (Exception ex)
            {
                responseModal.code = -1;
                responseModal.msg = "Error Occurred, Could Not Get Item Details";
                responseModal.data = string.Empty;
            }

            return responseModal;
        }
    }
}
