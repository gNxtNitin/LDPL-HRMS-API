using DatabaseManager;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ModelsLibrary.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UMS.Services.Services;
using UserManagementLibrary.Interfaces;
using UserManagementLibrary.Models;

namespace UserManagementLibrary
{
    public class UserManagerService : IUserManagerService
    {
        EncDcService encDcService = new EncDcService();
        MiscDataSetting _miscDataSetting = new MiscDataSetting();

        /// <summary>
        /// Create user's information in the system by calling a stored procedure.
        /// 
        /// This method:
        /// - Takes user details from the `UserMasterReqModel` object.
        /// - Encrypts the user's password.
        /// - Populates a list of parameters to pass to the stored procedure `sp_GetSetDeleteUsers`.
        /// - Executes the stored procedure with the flag set to 'C' (for creation).
        /// - Captures and returns the stored procedure's result (status code and error message).
        /// - Returns a ResponseModel object containing the result of the operation or an error message in case of an exception.
        /// </summary>
        public async Task<ResponseModel> CreateUserMaster(UserMasterReqModel req)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                req.Password = await encDcService.Encrypt(req.Password);

                ArrayList arrList = new ArrayList();
                DAL.spArgumentsCollection(arrList, "@FirstName", req.FirstName ?? "", "VARCHAR", "I");
                DAL.spArgumentsCollection(arrList, "@LastName", req.LastName ?? "", "VARCHAR", "I");
                DAL.spArgumentsCollection(arrList, "@DOB", req.DOB ?? "", "SMALLDATETIME", "I");
                //DAL.spArgumentsCollection(arrList, "@Address", req.Address ?? "", "VARCHAR", "I");
                DAL.spArgumentsCollection(arrList, "@Email", req.Email ?? "", "NVARCHAR", "I");
                DAL.spArgumentsCollection(arrList, "@Mobile", req.Mobile ?? "", "NVARCHAR", "I");
                DAL.spArgumentsCollection(arrList, "@Password", req.Password ?? "", "NVARCHAR", "I");
                DAL.spArgumentsCollection(arrList, "@FilePath", req.FilePath ?? "", "NVARCHAR", "I");
                DAL.spArgumentsCollection(arrList, "@flag", "C", "CHAR", "I");
                DAL.spArgumentsCollection(arrList, "@Ret", "", "INT", "O");
                DAL.spArgumentsCollection(arrList, "@ErrorMsg", "","VARCHAR", "O");        
                var res = DAL.RunStoredProcedureRetError("sp_GetSetDeleteUsers", arrList);
                response.code = res.Ret;
                response.msg = res.ErrorMsg;
                response.data = JsonConvert.SerializeObject(res);
            }
            catch (Exception ex)
            {
                response.code = -1;
                response.data = ex.Message;
            }
            return await Task.FromResult(response);
        }

        /// <summary>
        /// Updates a user's information in the system by calling a stored procedure.
        /// </summary>
        public async Task<ResponseModel> UpdateUserMaster(UserMasterReqModel req)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                if (string.IsNullOrEmpty(req.UserId))
                {
                    throw new ArgumentException("UserId cannot be null or empty.");
                }
                //req.Password = await encDcService.Encrypt(req.Password);

                ArrayList arrList = new ArrayList();
                DAL.spArgumentsCollection(arrList, "@UserId", req.UserId , "INT", "I");
                DAL.spArgumentsCollection(arrList, "@FirstName", req.FirstName ?? "", "VARCHAR", "I");
                DAL.spArgumentsCollection(arrList, "@LastName", req.LastName ?? "", "VARCHAR", "I");
                DAL.spArgumentsCollection(arrList, "@DOB", req.DOB ?? "", "SMALLDATETIME", "I");
                DAL.spArgumentsCollection(arrList, "@Email", req.Email ?? "", "NVARCHAR", "I");
                DAL.spArgumentsCollection(arrList, "@Mobile", req.Mobile ?? "", "NVARCHAR", "I");
                DAL.spArgumentsCollection(arrList, "@Password", req.Password ?? "", "NVARCHAR", "I");
                DAL.spArgumentsCollection(arrList, "@FilePath", req.FilePath ?? "", "NVARCHAR", "I");
                DAL.spArgumentsCollection(arrList, "@flag", "U", "CHAR", "I");
                DAL.spArgumentsCollection(arrList, "@Ret", "", "INT", "O");
                DAL.spArgumentsCollection(arrList, "@ErrorMsg", "", "VARCHAR", "O");
                var res = DAL.RunStoredProcedureRetError("sp_GetSetDeleteUsers", arrList);
                response.code = res.Ret;
                response.msg = res.ErrorMsg;
                response.data = JsonConvert.SerializeObject(res);
            }
            catch (Exception ex)
            {
                response.code = -1;
                response.data = ex.Message;
            }
            return await Task.FromResult(response);
        }

        /// <summary>
        /// Generates and sets a One-Time Password (OTP) for user authentication by calling a stored procedure.
        /// 
        /// This method:
        /// - Generates a random OTP 
        /// - Populates a list of parameters including the user's mobile or email, the OTP, and whether it's a resend request.
        /// - Executes the stored procedure `sp_SetOTP` to store the OTP in the database.
        /// - Returns a ResponseModel with the stored procedure's result and the generated OTP.
        /// </summary>
        public async Task<ResponseModel> SetOTP(VerificationReqModel rq)
        {
            Random r = new Random();
            int randNum = r.Next(10000);
            string verificationCode = "1111";

            ResponseModel response = new ResponseModel();
            try
            {
                ArrayList arrList = new ArrayList();
                DAL.spArgumentsCollection(arrList, "@MobileOrEmail", rq.MobileOrEmail, "VARCHAR", "I");
                DAL.spArgumentsCollection(arrList, "@VerificationCode", verificationCode, "VARCHAR", "I");
                DAL.spArgumentsCollection(arrList, "@IsResendCode", rq.IsResendCode.ToString(), "TINYINT", "I");
                DAL.spArgumentsCollection(arrList, "@Ret", "", "INT", "O");
                DAL.spArgumentsCollection(arrList, "@ErrorMsg", "", "VARCHAR", "O");
                var res = DAL.RunStoredProcedureRetError("sp_SetOTP", arrList);
                response.code = res.Ret;
                response.msg = res.ErrorMsg;
                response.data = verificationCode;
            }
            catch (Exception ex)
            {
                response.code = -1;
                response.data = ex.Message;
            }
            return await Task.FromResult(response);
        }

        /// <summary>
        /// Retrieves user information from the database by calling a stored procedure.
        /// 
        /// This method:
        /// - Determines the flag based on the `userId`. If `userId` is null or empty, the flag is set to 'G' (get all users); otherwise, it is set to 'I' (get specific user).
        /// - Populates a list of parameters including the flag and user ID, and passes them to the stored procedure `sp_GetSetDeleteUsers`.
        /// - Executes the stored procedure and retrieves the user data into a DataSet.
        /// - If data is returned, it converts the DataSet to JSON format and includes it in the response.
        /// - Returns a `ResponseModel` containing the result of the operation with a success message, data, and status code.
        /// </summary>
        public async Task<ResponseModel> GetUsers(string userId)
        {
            ResponseModel response = new ResponseModel();
            string flag = userId == null || userId == "" ? "G" : "I";
            userId = userId == null || userId == "" ? "0" : userId.ToString();
            try
            {
                DataSet ds = new DataSet();
                ArrayList arrList = new ArrayList();
                DAL.spArgumentsCollection(arrList, "@Flag", flag, "CHAR", "I");
                DAL.spArgumentsCollection(arrList, "@UserId", userId , "INT", "I");
                DAL.spArgumentsCollection(arrList, "@Ret", "", "INT", "O");
                DAL.spArgumentsCollection(arrList, "@ErrorMsg", "", "VARCHAR", "O");
                ds = DAL.RunStoredProcedure(ds, "sp_GetSetDeleteUsers", arrList);

                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Users";
                }
                string data = _miscDataSetting.ConvertToJSON(ds);
                response.msg = "Success";
                response.data = data;
                response.code = 200;
            }
            catch (Exception ex)
            {
                response.code = -3;
                response.msg = ex.Message;
            }
            return await Task.FromResult(response);
        }

        /// <summary>
        /// Deletes a user from the database by calling a stored procedure.
        /// 
        /// This method:
        /// - Determines the flag based on the `userId`. If `userId` is not null or empty, it is set to 'D' (delete user).
        /// - Populates a list of parameters including the flag and user ID, and passes them to the stored procedure `sp_GetSetDeleteUsers`.
        /// - Executes the stored procedure to delete the user, capturing the return status and error message.
        /// - Returns a `ResponseModel` with the result of the operation, including the status code, error message, and serialized result data.
        /// </summary>
        public async Task<ResponseModel> DeleteUserMaster(UserMasterReqModel req)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                if (string.IsNullOrEmpty(req.UserId))
                {
                    throw new ArgumentException("UserId cannot be null or empty.");
                }

                ArrayList arrList = new ArrayList();
                DAL.spArgumentsCollection(arrList, "@Flag", "D", "CHAR", "I");
                DAL.spArgumentsCollection(arrList, "@UserId", req.UserId, "INT", "I");  
                DAL.spArgumentsCollection(arrList, "@Ret", "", "INT", "O");
                DAL.spArgumentsCollection(arrList, "@ErrorMsg", "", "VARCHAR", "O");
                var res = DAL.RunStoredProcedureRetError("sp_GetSetDeleteUsers", arrList);
                response.code = res.Ret;
                response.msg = res.ErrorMsg;
                response.data = JsonConvert.SerializeObject(res);
            }
            catch (Exception ex)
            {
                response.code = -1;
                response.data = ex.Message;
            }
            return await Task.FromResult(response);
        }


    }
}
