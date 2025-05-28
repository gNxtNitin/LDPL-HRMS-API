using DatabaseManager;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Data;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Threading.Tasks;
//using PasswordManagementLibrary.Models;
//using PasswordManagementLibrary.Models;
using System.ComponentModel;
using EmailService.Library;
using System.Web;
using AuthLibrary.Models;
using Newtonsoft;
using Newtonsoft.Json;
using ModelsLibrary.Models;
using PasswordManagementLibrary.Models;
using LoginReqModel = PasswordManagementLibrary.Models.LoginReqModel;
using System.Drawing;

namespace PasswordManagementLibrary
{
    public class PasswordRepository : IPasswordRepository
    {
        //private readonly ILogger<PasswordRepository> _logger;
        private readonly EncDcService encDcService = new EncDcService();
        private readonly MiscDataSetting _miscDataSetting = new MiscDataSetting();
        private readonly IEmailSerivce _emailService;

        public PasswordRepository(ILogger<PasswordRepository> logger, IEmailSerivce emailSerivce)
        {
            _emailService = emailSerivce;
        }
        /// <summary>
        /// Sends a password reset email to the user identified by the provided mobile number or email.
        /// </summary>
        /// <param name="req"></param>
        /// <param name="userName"></param>
        /// <param name="applicationName"></param>
        /// <returns>A <see cref="ResponseModel"/> indicating the success or failure of the email sending operation.</returns>
        /// 
        public async Task<ResponseModel> SendForgotPasswordEmail(AuthRequestModel req, string userName, string applicationName)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                DataSet ds = new DataSet();
                ArrayList arrList = new ArrayList();

                // Prepare stored procedure parameters
                DAL.spArgumentsCollection(arrList, "@Flag", "F", "CHAR", "I");
                DAL.spArgumentsCollection(arrList, "@MobileOrEmailId", req.MobileOrEmail, "VARCHAR", "I");
                DAL.spArgumentsCollection(arrList, "@Ret", "", "INT", "O");
                DAL.spArgumentsCollection(arrList, "@ErrorMsg", "", "VARCHAR", "O");

                // Execute stored procedure
                ds = DAL.RunStoredProcedure(ds, "sp_GetAuthenticatedUser", arrList);
                int result = Convert.ToInt32(ds.Tables[0].Rows[0]["UserCount"]);

                if (result == 0)
                {

                    response.code = -1;
                    response.msg = "Email not found.";
                    return response;
                }

               // response = await SetOTP(req);
                if (response.code > 0)
                {

                    // Read the email template
                    //string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    //string projectRoot = Directory.GetParent(baseDirectory).Parent.Parent.Parent.Parent.FullName;
                   
                    //string filePath = Path.Combine(projectRoot, "PasswordManagementLibrary", "ResetPassword.txt");

                    string emailBody
                                        = string.Format(@"<!DOCTYPE html>
                    <html>
                    <head>
                        <meta charset=""utf-8"" />
                        <title>Password Reset</title>
                    </head>
                    <body>
                        <h1>Password Reset Request</h1>
                        <p>Dear User,</p>
                        <p>You have requested to reset your password. Please click the link below to proceed:</p>
                        <a href=""$hostingURL?code=$code"">Reset Password</a>
                        <p>If you did not request this, please ignore this email.</p>
                        <p>Thank you!</p>
                    </body>
                    </html>");
                    //using (StreamReader file = new StreamReader(filePath))
                    //{
                    //    emailBody = await file.ReadToEndAsync();
                    //}
                    // Encrypt the email for the reset link
                    string encStr = req.MobileOrEmail + "&" + response.data;
                    string emailToken = await encDcService.Encrypt(encStr);
                    emailToken = HttpUtility.UrlEncode(emailToken);
                    string hostingURL = "https://localhost:7227/Auth/ResetPassword";

                    // Replace placeholders in the email body
                    emailBody = emailBody.Replace("{userName}", userName)
                                         .Replace("$code", emailToken)
                                         .Replace("$hostingURL", hostingURL)
                                         .Replace("{applicationName}", applicationName);

                    EmailDetails details = new EmailDetails();
                    details.ToEmailIds = req.MobileOrEmail;
                    details.subject = "Forgot Password";
                    details.body = emailBody;
                    EmailConfiguration emailConfiguration = new EmailConfiguration();

                    // Queue the email
                    await _emailService.QueueEmail(emailConfiguration, details);
                    //await QueueEmail(req.MobileOrEmail, "Reset Password", emailBody, Guid.NewGuid().ToString());

                    // Send the email
                    // await SendEmail(req.MobileOrEmail, $"Reset Password for {applicationName}", emailBody);

                    response.code = 1;
                    response.msg = "Password reset link sent.";
                }
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Error in sending forgot password email");
                response.code = -1;
                response.msg = "Forgot password link not sent.";
                response.data = ex.Message;
            }

            return response;
        }

        public async Task<ResponseModel> SetOTP(LoginReqModel req)
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



        public async Task<ResponseModel> ResetPassword(AuthRequestModel req)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                if (string.IsNullOrEmpty(req.UserId) || string.IsNullOrEmpty(req.Password))
                {
                    throw new ArgumentException("UserId cannot be null or empty.");
                }
                req.Password = await encDcService.Encrypt(req.Password);

                ArrayList arrList = new ArrayList();
                DALOR.spArgumentsCollection(arrList, "p_userid", req.MobileOrEmail, "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_password", req.Password, "VARCHAR", "I");

                DALOR.spArgumentsCollection(arrList, "@ret", "", "VARCHAR", "O");
                DALOR.spArgumentsCollection(arrList, "@errormsg", "", "VARCHAR", "O", 4000);
                
                var res = DALOR.RunStoredProcedureRetError("G_SP_ResetPassword", arrList);

                response.code = res.Ret;
                response.msg = res.ErrorMsg;
                response.data = string.Empty;
                if (response.code > 0)
                {
                    response.msg = "Password reset successfully.";
                }
                else
                {
                    response.msg = "Failed to reset password.";
                }
            }
            catch (Exception ex)
            {
                response.code = -1;
                response.msg = ex.Message;
                response.data = string.Empty;
            }
            return await Task.FromResult(response);
        }





        /// <summary>
        /// Queues an email for sending by inserting it into the EmailNotification table.
        /// </summary>
        /// <param name="toEmailIds">A comma-separated string of email addresses to which the email will be sent.</param>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="body">The body content of the email, which can be plain text or HTML.</param>
        /// <param name="token">A unique token associated with the email, used for tracking or verification.</param>
        /// <param name="isHTML">An optional parameter indicating whether the email body is in HTML format (default is 1 for HTML).</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method inserts the email details into the EmailNotification table and subsequently
        /// clears the email queue to maintain optimal performance.
        /// </remarks>
        /// 
        //public async Task QueueEmail(string toEmailIds, string subject, string body, string token, int isHTML = 1)
        //{
        //    body = isHTML == 1 ? WebUtility.HtmlEncode(body) : body;
        //    string query = $"INSERT INTO EmailNotification(Token, ToEmailIds, EmailSubject, EmailBody, HasAttachment, IsSent, CreatedDate, Status, IsHTML) " +
        //                   $"VALUES('{token}', '{toEmailIds}', '{subject}', '{body}', 0, 0, GETUTCDATE(), 'A', {isHTML})";
        //    await _miscDataSetting.ExecuteNonQuery(query);
        //    await ClearEmailQueue();
        //}

        /// <summary>
        /// Clears the email queue by sending unsent emails that have a valid recipient address.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method retrieves emails from the EmailNotification table where IsSent is 0,
        /// processes each email, and sends it using the <see cref="SendEmail"/> method.
        /// Emails are decoded if they are in HTML format before sending.
        /// </remarks>

        //public async Task ClearEmailQueue()
        //{
        //    string query = "SELECT ToEmailIds, EmailSubject, EmailBody, Token, IsHTML FROM EmailNotification WHERE IsSent = 0 AND Status='A' AND LEN(ISNULL(ToEmailIds, '')) > 0";
        //    DataSet ds = await _miscDataSetting.GetDataSet(query);

        //    if (ds != null)
        //    {
        //        foreach (DataRow row in ds.Tables[0].Rows)
        //        {
        //            string toEmailIds = row["ToEmailIds"].ToString();
        //            string subject = row["EmailSubject"].ToString();
        //            string body = row["EmailBody"].ToString();
        //            string token = row["Token"].ToString();
        //            bool isHTML = Convert.ToBoolean(row["IsHTML"]);

        //            body = isHTML ? WebUtility.HtmlDecode(body) : body;
        //            await SendEmail(toEmailIds, subject, body);
        //        }
        //    }
        //}

        /// <summary>
        /// Sends an email to the specified recipients using SMTP configuration.
        /// </summary>
        /// <param name="toEmailIds">A semicolon-separated string of email addresses to which the email will be sent.</param>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="body">The body content of the email, which is expected to be in HTML format.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method configures the SMTP client with the specified server settings and sends an email message.
        /// It logs the success or failure of the email sending operation.
        /// Make sure to use an app-specific password if using services like Gmail.
        /// </remarks>
        //public async Task SendEmail(string toEmailIds, string subject, string body)
        //{
        //    var emailConfiguration = new EmailConfiguration
        //    {
        //        SmtpServerAddress = "smtp.gmail.com",
        //        SmtpServerPort = 587,
        //        EmailFromAddress = "pratapvansh584@gmail.com",
        //        SmtpServerUserId = "pratapvansh584@gmail.com",
        //        SmtpServerPassword = "qmmi bjia mcov cded", // Use your app password
        //        DisplayName = "Application",
        //        ByPass = true
        //    };

        //    if (emailConfiguration.ByPass)
        //    {
        //        try
        //        {
        //            using (MailMessage mail = new MailMessage())
        //            {
        //                mail.From = new MailAddress(emailConfiguration.EmailFromAddress, emailConfiguration.DisplayName);
        //                foreach (string recipient in toEmailIds.Split(';'))
        //                {
        //                    mail.To.Add(recipient.Trim());
        //                }

        //                mail.Subject = subject;
        //                mail.Body = body;
        //                mail.IsBodyHtml = true;

        //                using (SmtpClient smtp = new SmtpClient(emailConfiguration.SmtpServerAddress, emailConfiguration.SmtpServerPort))
        //                {
        //                    smtp.EnableSsl = true;
        //                    smtp.UseDefaultCredentials = false;
        //                    smtp.Credentials = new NetworkCredential(emailConfiguration.SmtpServerUserId, emailConfiguration.SmtpServerPassword);
        //                    //smtp.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
        //                    await smtp.SendMailAsync(mail);
        //                }
        //            }
        //            //_logger.LogInformation("Email sent successfully to {toEmailIds}", toEmailIds);
        //        }
        //        catch (Exception ex)
        //        {
        //            //_logger.LogError(ex, "Error sending email to {toEmailIds}", toEmailIds);
        //        }
        //    }
        //}

        /// <summary>
        /// Callback method invoked when the asynchronous email sending operation is completed.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="AsyncCompletedEventArgs"/> that contains the event data.</param>
        /// <param name="token">A unique token associated with the email message, used for tracking.</param>
        /// <remarks>
        /// This method checks if the email sending operation was canceled or if an error occurred.
        /// It updates the status of the email in the EmailNotification table accordingly.
        /// The status can be 'C' for canceled, 'F' for failure, or 'S' for success.
        /// Ensure to uncomment the line that executes the update query to update the database.
        /// </remarks>

        //private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e, string token)
        //{
        //    int isSent = 0;
        //    string status = e.Cancelled ? "C" : e.Error != null ? "F" : "S";

        //    if (e.Cancelled)
        //    {
        //        Console.WriteLine("[{0}] Send canceled.", token);
        //    }
        //    else if (e.Error != null)
        //    {
        //        Console.WriteLine("[{0}] {1}", token, e.Error.ToString());
        //    }
        //    else
        //    {
        //        Console.WriteLine("Message sent.");
        //    }

        //    isSent = 1;
        //    string updateQuery = $"UPDATE EmailNotification SET IsSent = {isSent}, SentDate = GETUTCDATE(), Status = '{status}' WHERE Token = '{token}'";
        //    // Uncomment this line to execute the update
        //    // _miscDataSetting.ExecuteNonQueryNonAsync(updateQuery);
        //}


        /// <summary>
        /// generates a token (unique code) for the user to reset the password
        /// token is sent to the user's email in reset password link
        /// <param name="req"></param>
        ///</summary>
        
        public async Task<ResponseModel> GeneratePasswordResetToken(AuthRequestModel req)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                if (string.IsNullOrEmpty(req.MobileOrEmail))
                {
                    throw new ArgumentException("UserId cannot be null or empty.");
                }

                string token = Guid.NewGuid().ToString();
                token = await encDcService.Encrypt(token);



                ArrayList arrList = new ArrayList();
                DALOR.spArgumentsCollection(arrList, "p_flag", "C", "CHAR", "I", 1);
                DALOR.spArgumentsCollection(arrList, "p_userid", req.MobileOrEmail, "CHAR", "I", 50);
                DALOR.spArgumentsCollection(arrList, "p_tokenhash", token, "VARCHAR", "I");

                DALOR.spArgumentsCollection(arrList, "@ret", "", "VARCHAR", "O");
                DALOR.spArgumentsCollection(arrList, "@errormsg", "", "VARCHAR", "O", 4000);

                var res = DALOR.RunStoredProcedureRetError("G_SP_SetValidatePasswordResetToken", arrList);

                response.code = res.Ret;
                response.msg = res.ErrorMsg;
                string encTokenForUrl = await encDcService.Encrypt(string.Concat(token, "&", res.Ret.ToString()));

                response.data = response.code > 0 ? HttpUtility.UrlEncode(encTokenForUrl) : string.Empty;

                if (response.code == -404)
                {
                    response.msg = "User Email not found.";
                }
                else
                {
                    response.msg = res.ErrorMsg;
                }
            }

            catch (Exception ex)
            {
                response.code = -1;
                response.data = ex.Message;
            }
            return await Task.FromResult(response);
        }


        public async Task<ResponseModel> ValidateResetPasswordToken(string tokenHash)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                ArrayList arrList = new ArrayList();

                DALOR.spArgumentsCollection(arrList, "p_flag", "V", "CHAR", "I", 1);
                DALOR.spArgumentsCollection(arrList, "p_userid", string.Empty, "CHAR", "I", 50);
                DALOR.spArgumentsCollection(arrList, "p_tokenhash", tokenHash, "VARCHAR", "I");

                DALOR.spArgumentsCollection(arrList, "@ret", "", "VARCHAR", "O");
                DALOR.spArgumentsCollection(arrList, "@errormsg", "", "VARCHAR", "O");

                var res = DALOR.RunStoredProcedureRetError("G_SP_SetValidatePasswordResetToken", arrList);

                response.code = res.Ret;
                response.msg = res.Ret > 0 ? "Valid token" : "Invalid token";
            }
            catch (Exception ex)
            {
                response.code = -1;
                response.data = ex.Message;
            }
            return await Task.FromResult(response);
        }


        public async Task<ResponseModel> GetPasswordPolicyValidationRules()
        {
            ResponseModel response = new ResponseModel();
            try
            {
                ArrayList arrList = new ArrayList();

                DAL.spArgumentsCollection(arrList, "@Flag", "G", "CHAR", "I");
                DAL.spArgumentsCollection(arrList, "@Ret", "", "INT", "O");
                DAL.spArgumentsCollection(arrList, "@ErrorMsg", "", "VARCHAR", "O");
                DataSet dataSet = new DataSet();
                DAL.RunStoredProcedure(dataSet, "sp_GetSetPasswordPolicy", arrList);
                
                if (dataSet != null && dataSet.Tables.Count > 0)
                {
                    response.code = 200;
                    response.msg = "OK";
                    response.data = JsonConvert.SerializeObject(dataSet);
                }
                else
                {
                    response.code = -1;
                    response.msg = "Failed to retrieve data";
                    response.data = string.Empty;
                }

              
            }
            catch (Exception ex)
            {
                response.code = -1;
                response.data = ex.Message;
            }

            return response;
        }

        public async Task<ResponseModel> GetPasswordPolicyMaster()
        {
            ResponseModel response = new ResponseModel();
            try
            {
                ArrayList arrList = new ArrayList();

                DAL.spArgumentsCollection(arrList, "@Flag", "M", "CHAR", "I");
                DAL.spArgumentsCollection(arrList, "@Ret", "", "INT", "O");
                DAL.spArgumentsCollection(arrList, "@ErrorMsg", "", "VARCHAR", "O");
                DataSet dataSet = new DataSet();
                DAL.RunStoredProcedure(dataSet, "sp_GetSetPasswordPolicy", arrList);

                if (dataSet != null && dataSet.Tables.Count > 0)
                {
                    response.code = 200;
                    response.msg = "OK";
                    response.data = JsonConvert.SerializeObject(dataSet);
                }
                else
                {
                    response.code = -1;
                    response.msg = "Failed to retrieve data";
                    response.data = string.Empty;
                }


            }
            catch (Exception ex)
            {
                response.code = -1;
                response.data = ex.Message;
            }

            return response;
        }
        


        public async Task<ResponseModel> UpdatePasswordPolicy(PasswordPolicyMaster PasswordPolicyMaster)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                ArrayList arrList = new ArrayList();

                DAL.spArgumentsCollection(arrList, "@Flag", "U", "CHAR", "U");
                DAL.spArgumentsCollection(arrList, "@MinLength", PasswordPolicyMaster.MinLength.ToString(), "INT", "I");
                DAL.spArgumentsCollection(arrList, "@MaxLength", PasswordPolicyMaster.MaxLength.ToString(), "INT", "I");
                DAL.spArgumentsCollection(arrList, "@MinUppercase", PasswordPolicyMaster.MinUppercase.ToString(), "INT", "I");
                DAL.spArgumentsCollection(arrList, "@MaxUppercase", PasswordPolicyMaster.MaxUppercase.ToString(), "INT", "I");
                DAL.spArgumentsCollection(arrList, "@MinLowercase", PasswordPolicyMaster.MinLowercase.ToString(), "INT", "I");
                DAL.spArgumentsCollection(arrList, "@MaxLowercase", PasswordPolicyMaster.MaxLowercase.ToString(), "INT", "I");
                DAL.spArgumentsCollection(arrList, "@MinNumeric", PasswordPolicyMaster.MinNumeric.ToString(), "INT", "I");
                DAL.spArgumentsCollection(arrList, "@MaxNumeric", PasswordPolicyMaster.MaxNumeric.ToString(), "INT", "I");
                DAL.spArgumentsCollection(arrList, "@MinPunctuation", PasswordPolicyMaster.MinPunctuation.ToString(), "INT", "I");
                DAL.spArgumentsCollection(arrList, "@MaxPunctuation", PasswordPolicyMaster.MaxPunctuation.ToString(), "INT", "I");
                DAL.spArgumentsCollection(arrList, "@DisallowRepeatCharacters", PasswordPolicyMaster.DisallowRepeatCharacters, "VARCHAR", "I");
                DAL.spArgumentsCollection(arrList, "@DisallowDuplicateCharacters", PasswordPolicyMaster.DisallowDuplicateCharacters, "VARCHAR", "I");
                DAL.spArgumentsCollection(arrList, "@DisallowSequentialCharacters", PasswordPolicyMaster.DisallowSequentialCharacters, "VARCHAR", "I");
                DAL.spArgumentsCollection(arrList, "@BeginWithUppercase", PasswordPolicyMaster.BeginWithUppercase.ToString(), "BIT", "I");
                DAL.spArgumentsCollection(arrList, "@EndWithUppercase", PasswordPolicyMaster.EndWithUppercase.ToString(), "BIT", "I");
                DAL.spArgumentsCollection(arrList, "@BeginWithAlpha", PasswordPolicyMaster.BeginWithAlpha.ToString(), "BIT", "I");
                DAL.spArgumentsCollection(arrList, "@EndWithAlpha", PasswordPolicyMaster.EndWithAlpha.ToString(), "BIT", "I");
                DAL.spArgumentsCollection(arrList, "@BeginWithNumber", PasswordPolicyMaster.BeginWithNumber.ToString(), "BIT", "I");
                DAL.spArgumentsCollection(arrList, "@EndWithNumber", PasswordPolicyMaster.EndWithNumber.ToString(), "BIT", "I");
                DAL.spArgumentsCollection(arrList, "@BeginWithSymbol", PasswordPolicyMaster.BeginWithSymbol.ToString(), "BIT", "I");
                DAL.spArgumentsCollection(arrList, "@EndWithSymbol", PasswordPolicyMaster.EndWithSymbol.ToString(), "BIT", "I");
                DAL.spArgumentsCollection(arrList, "@ProhibitedCharacters", PasswordPolicyMaster.ProhibitedCharacters ?? "", "VARCHAR", "I");
                DAL.spArgumentsCollection(arrList, "@ExpiryDays", PasswordPolicyMaster.ExpiryDays.ToString(), "INT", "I");
                DAL.spArgumentsCollection(arrList, "@ExpiryNotifyBeforeDays", PasswordPolicyMaster.ExpiryNotifyBeforeDays.ToString(), "INT", "I");
                DAL.spArgumentsCollection(arrList, "@MinAgeDays", PasswordPolicyMaster.MinAgeDays.ToString(), "INT", "I");
                DAL.spArgumentsCollection(arrList, "@EnforcePasswordHistory", PasswordPolicyMaster.EnforcePasswordHistory.ToString(), "INT", "I");
                DAL.spArgumentsCollection(arrList, "@LockoutThreshold", PasswordPolicyMaster.LockoutThreshold.ToString(), "INT", "I");
                DAL.spArgumentsCollection(arrList, "@LockoutDurationMinutes", PasswordPolicyMaster.LockoutDurationMinutes.ToString(), "INT", "I");
                DAL.spArgumentsCollection(arrList, "@ResetTokenExpiryMinutes", PasswordPolicyMaster.ResetTokenExpiryMinutes.ToString(), "INT", "I");
                DAL.spArgumentsCollection(arrList, "@ForceChangeOnFirstLogin", PasswordPolicyMaster.ForceChangeOnFirstLogin.ToString(), "BIT", "I");
                DAL.spArgumentsCollection(arrList, "@ModifiedBy", PasswordPolicyMaster.ModifiedBy.ToString(), "INT", "I");
                

                DAL.spArgumentsCollection(arrList, "@Ret", "", "INT", "O");
                DAL.spArgumentsCollection(arrList, "@ErrorMsg", "", "VARCHAR", "O");
                var res = DAL.RunStoredProcedureRetError("sp_GetSetPasswordPolicy", arrList);
                response.code = res.Ret;
                response.msg = res.ErrorMsg;
                response.data = string.Empty;
            }
            catch (Exception ex)
            {
                response.code = -1;
                response.msg = ex.Message;
                response.data = string.Empty;
            }

            return response;
        }
    }
}
