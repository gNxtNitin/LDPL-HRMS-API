using AuthLibrary.Interface;
using AuthLibrary.Models;
using Azure;
using DatabaseManager;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ModelsLibrary.Models;
using Newtonsoft.Json;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthLibrary
{
    public class AuthService : IAuthService
    {

        EncDcService encDcService = new EncDcService();

        private readonly IConfiguration _configuration;


        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;

        }


        public async Task<string> GenerateJwtToken(string name, string role, string userId)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(role))
            {
                return "Invalid parameters"; // Return a clear error message for invalid input
            }

            try
            {
                var claims = new[]
                {
                    //new Claim(ClaimTypes.NameIdentifier, userId),
                    new Claim(ClaimTypes.Name, name),
                    new Claim(ClaimTypes.Role, role)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddMinutes(30),
                    //Expires = DateTime.UtcNow.AddDays(10),
                    Issuer = _configuration["Jwt:Issuer"],
                    Audience = _configuration["Jwt:Audience"],
                    SigningCredentials = credentials
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                return await Task.FromResult(tokenString);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        //public async Task<ResponseModel> AuthenticateUser2(AuthRequestModel loginReq)
        //{
        //    ResponseModel responseModel = new ResponseModel();

        //    try
        //    {
        //        if (string.IsNullOrEmpty(loginReq.MobileOrEmail))
        //        {
        //            responseModel.msg = "Mobile or email is required.";
        //        }
        //        else
        //        {
        //            //if (loginReq.IsLoginWithOtp == true)
        //            //{
        //            //    return await SetOTP(loginReq);
        //            //}
        //            //else 
        //            if (!string.IsNullOrEmpty(loginReq.Password))
        //            {
        //                //loginReq.Password = await encDcService.Encrypt(loginReq.Password);
        //                DataSet ds = new DataSet();
        //                ArrayList arrList = new ArrayList();
        //                DALOR.spArgumentsCollection(arrList, "g_MobileOrEmailId", loginReq.MobileOrEmail, "CHAR", "I", 5);
        //                DALOR.spArgumentsCollection(arrList, "g_Password", loginReq.Password, "VARCHAR", "I");
        //                DALOR.spArgumentsCollection(arrList, "g_Company_Code", loginReq.CompanyCode != null ? loginReq.CompanyCode.ToString() : "0", "INT", "I");


        //                DALOR.spArgumentsCollection(arrList, "@ret", "", "VARCHAR", "O");

        //                DALOR.spArgumentsCollection(arrList, "@errormsg", "", "VARCHAR", "O", 4000);
        //                DALOR.spArgumentsCollection(arrList, "g_ResultSet", "", "REFCURSOR", "O");
        //                var res = DALOR.RunStoredProcedureDsRetError("sp_GetAuthenticatedUser1", arrList, ds);

        //                if (res.Ret > 0)
        //                {
        //                    if (ds.Tables.Count > 0)
        //                    {
        //                        //if (loginReq.IsLoginWithOtp == true)
        //                        //{
        //                        //    return await SetOTP(loginReq);
        //                        //}
        //                        if (loginReq.IsJwtToken == true)
        //                        {
        //                            string userRole = loginReq.MobileOrEmail;
        //                            string uId = ds.Tables[0].Rows[0]["USERID"].ToString();
        //                            string fname = ds.Tables[0].Rows[0]["FIRSTNAME"].ToString();
        //                            string cname = ds.Tables[0].Rows[0]["COMPANY_NAME"].ToString();
        //                            string userrole = ds.Tables[0].Rows[0]["ROLEID"].ToString();
        //                            string jwtToken = await GenerateJwtToken(loginReq.MobileOrEmail, userrole, loginReq.MobileOrEmail);
        //                            responseModel.data = new { token = jwtToken, userId = uId, name = fname, companyname = cname, role= userrole };


        //                            responseModel.code = 200;
        //                            responseModel.msg = "Success";

        //                        }



        //                    }
        //                    else
        //                    {
        //                        responseModel.code = -1;

        //                        responseModel.msg = "Username Or Password Incorrect.";
        //                    }
        //                }

        //                else
        //                {
        //                    responseModel.code = -2;
        //                    responseModel.msg = "Invalid User";
        //                }
        //            }
        //            else
        //            {
        //                responseModel.code = -1;
        //                responseModel.msg = "Enter password.";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        responseModel.code = -1;
        //        responseModel.msg = $"An error occurred while processing the request: {ex.Message}";
        //    }
        //    return responseModel;
        //}
        //public async Task<ResponseModel> AuthenticateUser(AuthRequestModel loginReq)
        //{
        //    ResponseModel responseModel = new ResponseModel();

        //    try
        //    {
        //        if (string.IsNullOrEmpty(loginReq.MobileOrEmail))
        //        {
        //            responseModel.msg = "Mobile or email is required.";
        //        }
        //        else
        //        {
        //            //if (loginReq.IsLoginWithOtp == true)
        //            //{
        //            //    return await SetOTP(loginReq);
        //            //}
        //            //else 
        //            if (!string.IsNullOrEmpty(loginReq.Password))
        //            {
        //                //loginReq.Password = await encDcService.Encrypt(loginReq.Password);
        //                DataSet ds = new DataSet();
        //                ArrayList arrList = new ArrayList();
        //                DALOR.spArgumentsCollection(arrList, "p_flag", "A", "VARCHAR", "I");
        //                DALOR.spArgumentsCollection(arrList, "g_MobileOrEmailId", loginReq.MobileOrEmail, "CHAR", "I", 50);
        //                DALOR.spArgumentsCollection(arrList, "g_Password", loginReq.Password, "VARCHAR", "I");
        //                //DALOR.spArgumentsCollection(arrList, "g_Company_Code", loginReq.CompanyCode != null ? loginReq.CompanyCode.ToString() : "0", "INT", "I");


        //                DALOR.spArgumentsCollection(arrList, "@ret", "", "VARCHAR", "O");

        //                DALOR.spArgumentsCollection(arrList, "@errormsg", "", "VARCHAR", "O", 4000);
        //                DALOR.spArgumentsCollection(arrList, "g_ResultSet", "", "REFCURSOR", "O");
        //                var res = DALOR.RunStoredProcedureDsRetError("G_SP_GetAuthenticatedUser", arrList, ds);
                        

        //                if (res.Ret > 0)
        //                {
        //                    if (ds.Tables.Count > 0)
        //                    {
        //                        //if (loginReq.IsLoginWithOtp == true)
        //                        //{
        //                        //    return await SetOTP(loginReq);
        //                        //}
        //                        if (loginReq.IsJwtToken == true)
        //                        {
        //                            string userRole = loginReq.MobileOrEmail;
        //                            string uId = ds.Tables[0].Rows[0]["USERID"].ToString();
        //                            string fname = ds.Tables[0].Rows[0]["ENAME"].ToString();
        //                            string roleE = ds.Tables[0].Rows[0]["ROLEID"].ToString();
        //                            string empId = ds.Tables[0].Rows[0]["EMPID"].ToString();
        //                            string jwtToken = await GenerateJwtToken(loginReq.MobileOrEmail, roleE, uId);
        //                            responseModel.data = new { token = jwtToken, userId = uId, name = fname, role = roleE, empId = empId };


        //                            responseModel.code = 200;
        //                            responseModel.msg = "Success";

        //                        }



        //                    }
        //                    else
        //                    {
        //                        responseModel.code = -1;

        //                        responseModel.msg = res.ErrorMsg;
        //                    }
        //                }

        //                else
        //                {
        //                    responseModel.code = -2;
        //                    responseModel.msg = res.ErrorMsg;
        //                }
        //            }
        //            else
        //            {
        //                responseModel.code = -1;
        //                responseModel.msg = "Enter password.";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        responseModel.code = -1;
        //        responseModel.msg = $"An error occurred while processing the request: {ex.Message}";
        //    }
        //    return responseModel;
        //}

        public async Task<ResponseModel> AuthenticateUser(AuthRequestModel loginReq)
        {
            ResponseModel responseModel = new ResponseModel();

            try
            {
                if (string.IsNullOrEmpty(loginReq.MobileOrEmail))
                {
                    responseModel.msg = "Mobile or email is required.";
                }
                else
                {
                    //if (loginReq.IsLoginWithOtp == true)
                    //{
                    //    return await SetOTP(loginReq);
                    //}
                    //else 
                    if (!string.IsNullOrEmpty(loginReq.Password))
                    {
                        loginReq.Password = await encDcService.Encrypt(loginReq.Password);
                        DataSet ds = new DataSet();
                        ArrayList arrList = new ArrayList();
                        DALOR.spArgumentsCollection(arrList, "p_userid", loginReq.MobileOrEmail, "CHAR", "I", 50);
                        DALOR.spArgumentsCollection(arrList, "p_password", loginReq.Password, "VARCHAR", "I");
                        
                        DALOR.spArgumentsCollection(arrList, "@ret", "", "VARCHAR", "O");
                        DALOR.spArgumentsCollection(arrList, "@errormsg", "", "VARCHAR", "O", 4000);
                        DALOR.spArgumentsCollection(arrList, "p_result", "", "REFCURSOR", "O");
                        var res = DALOR.RunStoredProcedureDsRetError("G_SP_GetAuthenticatedUser", arrList, ds);
                       

                        if (res.Ret>0)
                        {
                            if (ds.Tables.Count > 0)
                            {
                               
                                if (loginReq.IsJwtToken == true)
                                {
                                    string userRole = ds.Tables[0].Rows[0]["RoleName"].ToString();
                                    string uId = ds.Tables[0].Rows[0]["UserId"].ToString();
                                    string employeeId = ds.Tables[0].Rows[0]["UserName"].ToString();
                                    string fName = ds.Tables[0].Rows[0]["FirstName"].ToString();
                                    string jwtToken = await GenerateJwtToken(loginReq.MobileOrEmail, userRole, uId);
                                    responseModel.data = new { token = jwtToken, userId = uId, empId = employeeId, uName = fName, role=userRole};
                                    responseModel.code = res.Ret;

                                    if (res.Ret == 201)
                                    {
                                        responseModel.msg = "Reset Your Password!";
                                    }
                                    else
                                    {
                                        responseModel.msg = "Success";
                                    }
                                }
                            }
                            else
                            {
                                responseModel.code = -1;

                                responseModel.msg = res.ErrorMsg;
                            }
                        }
                        else
                        {
                            responseModel.code = -1;
                            responseModel.msg = res.ErrorMsg;
                        }
                    }
                    else
                    {
                        responseModel.code = -1;
                        responseModel.msg = "Invalid password!.";
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


        public async Task<ResponseModel> SetOTP(AuthRequestModel req)
        {
            Random r = new Random();
            int randNum = r.Next(10000);
            // string verificationCode = randNum.ToString("D4");
            string verificationCode = "1111";

            ResponseModel response = new ResponseModel();
            try
            {
                ArrayList arrList = new ArrayList();
                DAL.spArgumentsCollection(arrList, "@MobileOrEmail", req.MobileOrEmail, "VARCHAR", "I");

                DAL.spArgumentsCollection(arrList, "@VerificationCode", verificationCode, "VARCHAR", "I");
                DAL.spArgumentsCollection(arrList, "@IsResendCode", req.IsResendCode.ToString(), "TINYINT", "I");
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



        public async Task<ResponseModel> ValidateOTP(AuthRequestModel req)
        {
            var response = new ResponseModel();

            try
            {

                if (string.IsNullOrEmpty(req.MobileOrEmail))
                {
                    response.msg = "Mobile or email is required.";
                    return response;
                }

                if (string.IsNullOrEmpty(req.VerificationCode))
                {
                    response.code = -1;
                    response.msg = "OTP is required.";
                    return response;
                }

                ArrayList arrList = new ArrayList();
                DAL.spArgumentsCollection(arrList, "@MobileOrEmail", req.MobileOrEmail, "VARCHAR", "I");
                DAL.spArgumentsCollection(arrList, "@OTP", req.VerificationCode, "VARCHAR", "I");
                DAL.spArgumentsCollection(arrList, "@Ret", "", "INT", "O");
                DAL.spArgumentsCollection(arrList, "@UserRole", "", "VARCHAR", "O");
                DAL.spArgumentsCollection(arrList, "@ErrorMsg", "", "VARCHAR", "O");

                var res = DAL.RunStoredProcedureRetError("sp_ValidateOTP", arrList);

                response.code = res.Ret;
                response.msg = res.ErrorMsg;
                req.UserId = res.Ret.ToString();
                var usersRole = await GetUserRoleById(req.UserId);

                if (response.code > 0)
                {
                    string jwtToken = await GenerateJwtToken(req.MobileOrEmail, "UserRole", res.Ret.ToString());
                    response.data = jwtToken;
                    response.msg = "OTP validated successfully.";
                }
            }
            catch (Exception ex)
            {
                response.code = -1;
                response.msg = $"An error occurred while validating OTP: {ex.Message}";
            }

            return response;
        }


        public async Task<ResponseModel> GetUserRoleById(string UserId)
        {
            ResponseModel response = new ResponseModel();

            try
            {
                // Validate the UserId input
                if (string.IsNullOrEmpty(UserId))
                {
                    response.code = -1;
                    response.msg = "Valid user ID is required.";
                    return response;
                }

                // Prepare parameters for the stored procedure
                ArrayList arrList = new ArrayList();
                DAL.spArgumentsCollection(arrList, "@flag", "I", "CHAR", "I");
                DAL.spArgumentsCollection(arrList, "@UserId", UserId, "INT", "I");
                //DAL.spArgumentsCollection(arrList, "@roleId", req.RoleId, "TINYINT", "I");
                DAL.spArgumentsCollection(arrList, "@ret", "", "INT", "O");
                DAL.spArgumentsCollection(arrList, "@errorMsg", "", "VARCHAR", "O");

                // Execute the stored procedure and get the result
                DataSet ds = new DataSet();
                ds = DAL.RunStoredProcedure(ds, "sp_GetSetDeleteRole", arrList);

                // Check if the DataSet contains data
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var rolesTable = ds.Tables[0];

                    // Extract UserId and RoleId into a list of objects
                    var userRoles = rolesTable.AsEnumerable().Select(row => new
                    {
                        RoleName = row["RoleName"].ToString(),
                        RoleId = row["RoleId"].ToString()
                    }).ToList();

                    // Populate the response
                    response.code = 1;
                    response.data = userRoles; // Assign the list of user roles (not in JSON)
                    response.msg = "User roles retrieved successfully.";
                }
                else
                {
                    response.code = -2;
                    response.msg = "No roles found for the user.";
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during execution
                response.code = -1;
                response.msg = $"An error occurred while retrieving user roles: {ex.Message}";
            }

            return await Task.FromResult(response);
        }


        /// <summary>
        /// Asynchronously manages the login history for a specified user by invoking a stored procedure.
        /// </summary>
        /// <param name="userId">The ID of the user for whom the login history is being managed.</param>
        /// <returns>Returns a ResponseModel containing the status code and any error message.</returns>
        private static async void ManageLoginHistroy(int userId)
        {
            //string connStr = UMSResources.GetConnectionString();
            string connStr = "Server=DESKTOP-P9QLVJS;Database=UserManagement;Trusted_Connection=True;MultipleActiveResultSets=True";
            ResponseModel response = new ResponseModel();
            try
            {
                using (SqlConnection connection = new SqlConnection(connStr))
                {
                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "sp_InsertLoginHistory";
                        command.CommandType = CommandType.StoredProcedure;
                        SqlParameter parameter = new SqlParameter();

                        parameter = new SqlParameter();
                        parameter = command.Parameters.Add("@userId", SqlDbType.Int);
                        parameter.Value = userId;
                        parameter.Direction = ParameterDirection.Input;

                        parameter = new SqlParameter();
                        parameter = command.Parameters.Add("@ret", SqlDbType.TinyInt);
                        parameter.Direction = ParameterDirection.Output;

                        parameter = new SqlParameter();
                        parameter = command.Parameters.Add("@errorCode", SqlDbType.VarChar, 200);
                        parameter.Direction = ParameterDirection.Output;

                        int res = await command.ExecuteNonQueryAsync();
                        response.code = Convert.ToInt32(command.Parameters["@ret"].Value);
                        response.msg = command.Parameters["@errorCode"].Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                response.code = -1;
                response.data = ex.Message;
                // _logger.LogError("SetVerificationCode", ex);
            }
        }


        public async Task<ResponseModel> UserDetail(string userId)
        {
            ResponseModel response = new ResponseModel();

            try
            {
                // Validate the UserId input
                if (string.IsNullOrEmpty(userId))
                {
                    response.code = -1;
                    response.msg = "Valid user ID is required.";
                    return response;
                }

                DataSet ds = new DataSet();
                ArrayList arrList = new ArrayList();
                DALOR.spArgumentsCollection(arrList, "p_flag", "G", "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "g_MobileOrEmailId", userId, "CHAR", "I", 50);
                DALOR.spArgumentsCollection(arrList, "g_Password", "", "VARCHAR", "I");

                DALOR.spArgumentsCollection(arrList, "@ret", "", "VARCHAR", "O");

                DALOR.spArgumentsCollection(arrList, "@errormsg", "", "VARCHAR", "O", 4000);
                DALOR.spArgumentsCollection(arrList, "g_ResultSet", "", "REFCURSOR", "O");
                var res = DALOR.RunStoredProcedureDsRetError("sp_GetAuthenticatedUser3", arrList, ds);
                response.code = res.Ret;
                response.msg = res.ErrorMsg;

                if (res.Ret > 0)
                {
                    if (ds.Tables.Count > 0)
                    {
                        var dt = ds.Tables[0];
                        if (dt.Rows.Count == 0)
                        {
                            response.data = string.Empty;
                        }
                        else
                        {
                            var row =dt.Rows[0];
                            var result = new Dictionary<string, object>();
                            foreach (DataColumn col in dt.Columns)
                            {
                                result[col.ColumnName] = row[col].ToString(); // or row[col.ColumnName]
                            }

                            response.data = result;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                response.code = -1;
                response.msg = $"An error occurred while retrieving user details!";
            }

            return await Task.FromResult(response);

        }

    }
}
