using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomerManagementLibrary.Models;

namespace Services.Interfaces
{
    public interface ICustomerService
    {
        Task<ResponseModel> GetCompany();
        Task<ResponseModel> GetOrderDetailsMaster(string customercode, string companycode);
        Task<ResponseModel> GetOrderDetailsItem(string customercode, string companycode, string ordercode);
        Task<ResponseModel> GetAllItems(string companycode);
        Task<ResponseModel> CreateOrder(OrderDetailModel odm);
        Task<ResponseModel> UpdateOrder(OrderDetailModel odm);
        Task<ResponseModel> DeleteOrder(OrderDetailModel odm);
        Task<ResponseModel> GetAllInvoice(string customercode, string companycode, string? ordercode, string? from, string? to);
        Task<ResponseModel> GetDashboardData(DashboardMetricRequest dashboardMetricRequest);
        Task<ResponseModel> GetDashboardData2(DashboardMetricRequest2 dashboardMetricRequest);
        Task<ResponseModel> GetInvoicedata(string cid1, string cid2, string cid3);
    }
}
