using ErrorAndExceptionHandling.Library;
using Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ModelsLibrary.Models;
using PasswordManagementLibrary.Models;
using AuthLibrary.Models;

namespace LDPLWEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordManagementController : ControllerBase
    {
        private readonly IErrorLoggingService _errorLoggingService;
        private readonly IPasswordManagementService _passwordManagementService;
        private readonly IConfiguration _configuration;
        public PasswordManagementController(IErrorLoggingService errorLoggingService, IPasswordManagementService passwordManagementService, IConfiguration configuration)
        {
            _errorLoggingService = errorLoggingService;
            _passwordManagementService = passwordManagementService;
            _configuration = configuration;
        }


        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] AuthRequestModel authRequest)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                string webHostUrl = _configuration.GetValue<string>("LDPLWebApiSettings:BaseUrl");
                if(string.IsNullOrEmpty(webHostUrl))
                {
                    return BadRequest(new { Message = "Base URL is not configured.", StatusCode = 400 });
                }

                responseModel = await _passwordManagementService.SendForgotEmail(authRequest, webHostUrl);
                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });

            }

        }


        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] AuthRequestModel auth)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                responseModel = await _passwordManagementService.ResetPassword(auth);
                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });

            }

        }


        [HttpPost("ValidateResetPasswordToken")]
        public async Task<IActionResult> ValidateResetPasswordToken([FromBody] string tokenHash)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                responseModel = await _passwordManagementService.ValidateResetPasswordToken(tokenHash);
                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });

            }
        }


        [HttpGet("GetPasswordValidationRules")]
        public async Task<IActionResult> GetPasswordValidationRules()
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                responseModel = await _passwordManagementService.GetPasswordValidationRules();
                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetPasswordPolicy")]
        public async Task<IActionResult> GetPasswordPolicy()
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                responseModel = await _passwordManagementService.GetPasswordPolicy();
                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("UpdatePasswordPolicy")]
        public async Task<IActionResult> UpdatePasswordPolicy(PasswordPolicyMaster PasswordPolicyMaster)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                responseModel = await _passwordManagementService.UpdatePasswordPolicy(PasswordPolicyMaster);
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
