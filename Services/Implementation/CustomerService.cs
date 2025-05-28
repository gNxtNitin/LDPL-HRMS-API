using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomerManagementLibrary;
using CustomerManagementLibrary.Models;
using Services.Interfaces;

namespace Services.Implementation
{
    public class CustomerService: ICustomerService
    {
        private readonly ICustomerManagementService _customerManagementService;
        public CustomerService(ICustomerManagementService customerManagementService)
        {
            _customerManagementService = customerManagementService;
        }
        public async Task<ResponseModel> GetCompany()
        {

            ResponseModel response = await _customerManagementService.GetCompany();
            return response;
        }
        public async Task<ResponseModel> GetOrderDetailsMaster(string customercode, string companycode)
        {

            ResponseModel response = await _customerManagementService.GetOrderDetailsMaster(customercode,companycode);
            return response;
        }
        public async Task<ResponseModel> GetOrderDetailsItem(string customercode, string companycode, string ordercode)
        {

            ResponseModel response = await _customerManagementService.GetOrderDetailsItem(customercode,companycode,ordercode);
            return response;
        }
        public async Task<ResponseModel> GetAllItems(string companycode)
        {

            ResponseModel response = await _customerManagementService.GetAllItems(companycode);
            return response;
        }
        public async Task<ResponseModel> CreateOrder(OrderDetailModel odm)
        {

            ResponseModel response = await _customerManagementService.CreateOrder(odm);
            return response;
        }
        public async Task<ResponseModel> UpdateOrder(OrderDetailModel odm)
        {

            ResponseModel response = await _customerManagementService.UpdateOrder(odm);
            return response;
        }
        public async Task<ResponseModel> DeleteOrder(OrderDetailModel odm)
        {

            ResponseModel response = await _customerManagementService.DeleteOrder(odm);
            return response;
        }

        public async Task<ResponseModel> GetDashboardData(DashboardMetricRequest dashboardMetricRequest)
        {
            ResponseModel response = await _customerManagementService.GetDashboardMetrics(dashboardMetricRequest);
            return response;
        }

        public async Task<ResponseModel> GetDashboardData2(DashboardMetricRequest2 dashboardMetricRequest)
        {
            ResponseModel response = await _customerManagementService.GetDashboardMetrics2(dashboardMetricRequest);
            return response;
        }
        public async Task<ResponseModel> GetAllInvoice(string customercode, string companycode, string? ordercode, string? from, string? to)
        {

            ResponseModel response = await _customerManagementService.GetAllInvoice(customercode, companycode, ordercode, from, to);
            return response;
        }
        public async Task<ResponseModel> GetInvoicedata(string cid1, string cid2, string cid3)
        {

            ResponseModel response = await _customerManagementService.GetInvoicedata(cid1, cid2, cid3);
            return response;
        }
    }
}
