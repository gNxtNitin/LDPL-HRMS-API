using AuthLibrary.Models;
using ErrorAndExceptionHandling.Library;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelsLibrary.Models;
using Services.Implementation;
using Services.Interfaces;
using DatabaseManager;
using System.Data.OleDb;
using System.Collections;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using NotificationSenderLib.Models;
using LDPLWEBAPI.Models;
using LDPLWEBAPI.Utility;
using PasswordManagementLibrary.Models;
//using Oracle.ManagedDataAccess.Client;
//using Dapper;
//using Dapper.Oracle;
//using System.Data;


namespace LDPLWEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserAuthService _userAuthService;
        private readonly IErrorLoggingService _errorLoggingService;
       
        public AuthController(IUserAuthService userAuthService, IErrorLoggingService errorLoggingService)
        {
            _userAuthService = userAuthService;
            _errorLoggingService = errorLoggingService;
          
        }

        //private readonly string dbConnStr = "User Id=LDPLERP;Password=LDPLERP;Data Source=103.178.248.36:1521/LDPLCLOUD;"; // Your Oracle DB connection string
        //[HttpGet("gicolo")]
        //public async Task<IActionResult> GetMyModels()
        //{
        //    using (var connection = new OracleConnection(dbConnStr))
        //    {
        //        connection.Open();



        //        var p = new OracleDynamicParameters();

        //        // Input Parameters
        //        p.Add("@p_flag", "I", OracleMappingType.Char, ParameterDirection.Input, size: 1);
        //        p.Add("@p_MenuId", 82543, OracleMappingType.Int32, ParameterDirection.Input);
        //        p.Add("@p_UserId", 0, OracleMappingType.Int32, ParameterDirection.Input);

        //        // Output Parameters
        //        p.Add("@ret", dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Output, size: 50);
        //        p.Add("@errormsg", dbType: OracleMappingType.Varchar2, direction: ParameterDirection.Output, size: 4000);
        //        p.Add("g_ResultSet", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

        //        // Query and map to model
        //        var results = await connection.QueryAsync<Monol>(
        //            "sp_GetSetMenu2",
        //            param: p,
        //            commandType: CommandType.StoredProcedure
        //        );

        //        // Query with Dapper, mapping to MyModel







        //    }
        //    return Ok("pp");
        //}


        //[HttpGet("GetAllMenu")]
        //public async Task<IActionResult> GetAllMenu()
        //{
        //    try
        //    {
        //        DataSet ds = new DataSet();
        //        ArrayList arrList = new ArrayList();
        //        DALOR.spArgumentsCollection(arrList, "g_MobileOrEmailId", "18166", "CHAR", "I", 5);
        //        DALOR.spArgumentsCollection(arrList, "g_Password", "18166", "VARCHAR", "I");


                
        //        DALOR.spArgumentsCollection(arrList, "@ret", "1", "VARCHAR", "O");

        //        DALOR.spArgumentsCollection(arrList, "@errormsg", "", "VARCHAR", "O", 4000);
        //        DALOR.spArgumentsCollection(arrList, "g_ResultSet", "", "REFCURSOR", "O");
        //        var res = DALOR.RunStoredProcedureDsRetError("sp_GetAuthenticatedUser", arrList, ds);
        //        //string pp = await DALOR.GetAuthenticatedUserAsync("lalit@LDPL.com", "81dc9bdb52d04dc20036dbd8313ed055");
        //        //DataSet ds = new DataSet();
        //        //ArrayList arrList = new ArrayList();
        //        //DALOR.spArgumentsCollection(arrList, "p_MobileOrEmailId", "lalit@LDPL.com", "VARCHAR", "I");
        //        //DALOR.spArgumentsCollection(arrList, "p_Password", "81dc9bdb52d04dc20036dbd8313ed055", "VARCHAR", "I");
        //        //DALOR.spArgumentsCollection(arrList, "p_ErrorMsg", "", "VARCHAR", "O");
        //        //DALOR.spArgumentsCollection(arrList, "p_Ret", "", "INT", "O");

        //        //DALOR.spArgumentsCollection(arrList, "p_ResultSet", "", "REFCURSOR", "O");
        //        //var p2 = DALOR.RunStoredProcedureDsRetError("sp_GetAuthenticatedUser", arrList, ds);
        //        //responseModel = await _userAuthService.AuthenticateUser(lrm);

        //        return Ok("pp");
        //    }
        //    catch (Exception ex)
        //    {
        //        await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
        //        return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });

        //    }
        //}

        [HttpPost("AuthenticateUser")]
        public async Task<IActionResult> AuthenticateUser([FromBody] AuthRequestModel lrm)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {

                responseModel = await _userAuthService.AuthenticateUser(lrm);
                
                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });

            }
        }
        

        [HttpPost("ValidateOTP")]
        public async Task<IActionResult> ValidateOTP([FromBody] AuthRequestModel lrm)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                responseModel = await _userAuthService.ValidateOTP(lrm);
                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                await _errorLoggingService.LogError(ex, ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName);
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500 });

            }
        }

        
        [HttpGet("Me")]
        public async Task<IActionResult> Me(string uId)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                if(string.IsNullOrEmpty(uId))
                {
                    return BadRequest("User ID cannot be null or empty.");
                }

                responseModel = await _userAuthService.UserInfo(uId);
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
