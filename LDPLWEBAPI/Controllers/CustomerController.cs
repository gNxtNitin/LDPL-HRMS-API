using CustomerManagementLibrary.Models;
using MenuManagementLib.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Implementation;
using Services.Interfaces;

namespace LDPLWEBAPI.Controllers
{
    [Authorize(Roles = "CUSTOMER")]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [AllowAnonymous]
        [HttpGet("GetAllCompany")]
        public async Task<IActionResult> GetAllCompany()
        {
            try
            {

                ResponseModel response = await _customerService.GetCompany();
                return Ok(response);
            }
            catch (Exception ex)
            {
                //await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });

            }
        }
        [HttpGet("GetOrderDetailsMaster")]
        public async Task<IActionResult> GetOrderDetailsMaster(string cid1, string cid2)
        {
            try
            {

                ResponseModel response = await _customerService.GetOrderDetailsMaster(cid1,cid2);
                return Ok(response);
            }
            catch (Exception ex)
            {
                //await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });

            }
        }
        [HttpGet("GetOrderDetailsItem")]
        public async Task<IActionResult> GetOrderDetailsItem(string cid1, string cid2, string cid3)
        {
            try
            {

                ResponseModel response = await _customerService.GetOrderDetailsItem(cid1,cid2,cid3);
                return Ok(response);
            }
            catch (Exception ex)
            {
                //await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });

            }
        }
        [HttpGet("GetAllItems")]
        public async Task<IActionResult> GetAllItems(string cid1)
        {
            try
            {

                ResponseModel response = await _customerService.GetAllItems(cid1);
                return Ok(response);
            }
            catch (Exception ex)
            {
                //await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });

            }
        }
        [HttpGet("GetAllInvoice")]
        public async Task<IActionResult> GetAllInvoice(string cid1, string cid2, string? cid3, string? cid4, string? cid5)
        {
            try
            {

                ResponseModel response = await _customerService.GetAllInvoice(cid1, cid2, cid3, cid4,cid5);
                return Ok(response);
            }
            catch (Exception ex)
            {
                //await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });

            }
        }
        [HttpGet("GetInvoiceData")]
        public async Task<IActionResult> GetInvoiceData(string cid1, string cid2, string cid3)
        {
            try
            {

                ResponseModel response = await _customerService.GetInvoicedata(cid1, cid2, cid3);
                return Ok(response);
            }
            catch (Exception ex)
            {
                //await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });

            }
        }
        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDetailModel odm)
        {
            try
            {
                //List<Menu> menuList = new List<Menu>();
                var res = await _customerService.CreateOrder(odm);
                return Ok(res);
            }
            catch (Exception ex)
            {
                //await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });

            }
        }
        [HttpPost("UpdateOrder")]
        public async Task<IActionResult> UpdateOrder([FromBody] OrderDetailModel odm)
        {
            try
            {
                //List<Menu> menuList = new List<Menu>();
                var res = await _customerService.UpdateOrder(odm);
                return Ok(res);
            }
            catch (Exception ex)
            {
                //await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });

            }
        }
        [HttpPost("DeleteOrder")]
        public async Task<IActionResult> DeleteOrder([FromBody] OrderDetailModel odm)
        {
            try
            {
                //List<Menu> menuList = new List<Menu>();
                var res = await _customerService.DeleteOrder(odm);
                return Ok(res);
            }
            catch (Exception ex)
            {
                //await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });

            }
        }

        [HttpPost("DashboardMetric")]
        public async Task<IActionResult> DashboardMetric([FromBody] DashboardMetricRequest dmRequest)  // W - week, M-Month
        {
            try
            {
                if(string.IsNullOrEmpty(dmRequest.DataRange) || (dmRequest.DataRange.Trim().ToUpper() != "W" && dmRequest.DataRange.Trim().ToUpper() != "M"))
                {
                    dmRequest.DataRange = "W";
                }

                var res = await _customerService.GetDashboardData(dmRequest);
                return Ok(res);
            }
            catch (Exception ex)
            {
                //await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });

            }
        }

        [HttpPost("DashboardMetric2")]
        public async Task<IActionResult> DashboardMetric2([FromBody] DashboardMetricRequest2 dmRequest)  // W - week, M-Month
        {
            try
            {
                if (string.IsNullOrEmpty(dmRequest.TabId) || (dmRequest.TabId.Trim().ToUpper() != "T1" && dmRequest.TabId.Trim().ToUpper() != "T2" && dmRequest.TabId.Trim().ToUpper() != "T3"))
                {
                    dmRequest.TabId = "T1";
                }
                else
                {
                    dmRequest.TabId = dmRequest.TabId.ToUpper();
                }

                var res = await _customerService.GetDashboardData2(dmRequest);
                return Ok(res);
            }
            catch (Exception ex)
            {
                //await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });

            }
        }
    }
}
