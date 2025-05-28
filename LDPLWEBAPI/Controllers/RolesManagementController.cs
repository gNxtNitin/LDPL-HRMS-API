using ErrorAndExceptionHandling.Library;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelsLibrary.Models;
using RoleManagementLibrary.Models;
using Services.Implementation;
using Services.Interfaces;

namespace LDPLWEBAPI.Controllers
{
    [Authorize(Roles = "ADMIN")]
    [Route("api/[controller]")]
    [ApiController]
    public class RolesManagementController : ControllerBase
    {
        private readonly IRolesManagementService _rolesManagementService;
        private readonly IErrorLoggingService _errorLoggingService;
        public RolesManagementController(IRolesManagementService rolesManagementService, IErrorLoggingService errorLoggingService)
        {
            _rolesManagementService = rolesManagementService;
            _errorLoggingService = errorLoggingService;
        }
        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRole([FromBody] RoleMasterReqModel rm)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                responseModel = await _rolesManagementService.CreateRoleMaster(rm);
                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });

            }
        }
        [HttpPost("UpdateRole")]
        public async Task<IActionResult> UpdateRole([FromBody] RoleMasterReqModel rm)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                responseModel = await _rolesManagementService.UpdateRoleMaster(rm);
                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });

            }
        }
        [HttpPost("DeleteRole")]
        public async Task<IActionResult> DeleteRole([FromBody] RoleMasterReqModel rm)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                responseModel = await _rolesManagementService.DeleteRoleMaster(rm);
                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });

            }
        }
        [HttpPost("UpdateUserRole")]
        public async Task<IActionResult> UpdateUserRole([FromBody] AssignRoleReqModel ar)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                responseModel = await _rolesManagementService.UpdateUserRole(ar);
                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });

            }
        }
        [HttpPost("GetRoleByUserId")]
        public async Task<IActionResult> GetRoleByUserId([FromBody] AssignRoleReqModel ar)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                responseModel = await _rolesManagementService.GetRoleByUserId(ar);
                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });

            }
        }
        [HttpGet("GetRoleMaster")]
        public async Task<IActionResult> GetRoleMaster(string? Id)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                responseModel = await _rolesManagementService.GetRoleMaster(Id);
                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });

            }
        }

        [HttpGet("GetRolesWithUsers")]
        public async Task<IActionResult> GetRolesWithUsers()
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                responseModel = await _rolesManagementService.GetRoleWithUsers();
                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });

            }
        }
    }
}
