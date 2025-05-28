using System.ComponentModel;
using System.Data;
using System.Net.Mail;
using System.Net;
using DatabaseManager;
using System.Collections;
using ModelsLibrary.Models;

namespace EmailService.Library
{
    public class EmailSerivce: IEmailSerivce
    {
        
        static bool mailSent = false;
        //public async Task QueueEmail(EmailConfiguration emailConfiguration,string toEmailIds, string subject, string body, string token, int isHTML = 1)
        public async Task QueueEmail(EmailConfiguration emailConfiguration, EmailDetails emailDetails)
        {
            ResponseModel responseModel = new ResponseModel();

            emailDetails.body = emailDetails.isHTML == 1 ? WebUtility.HtmlEncode(emailDetails.body) : emailDetails.body;
            try
            {
                ArrayList arrList = new ArrayList();
    

                DALOR.spArgumentsCollection(arrList, "p_flag", "C", "CHAR", "I", 1);
                DALOR.spArgumentsCollection(arrList, "p_token", emailDetails.token ?? string.Empty, "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_toemailids", emailDetails.ToEmailIds ?? string.Empty, "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_subject", emailDetails.subject ?? string.Empty, "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_body", emailDetails.body ?? string.Empty, "CLOB", "I");
                DALOR.spArgumentsCollection(arrList, "p_hasattachment", emailDetails.hasAttachment ? "1":"0", "NUMBER", "I");
                DALOR.spArgumentsCollection(arrList, "p_issent", "0", "NUMBER", "I");
                DALOR.spArgumentsCollection(arrList, "p_status", "A", "CHAR", "I", 1);
                DALOR.spArgumentsCollection(arrList, "p_ishtml", emailDetails.isHTML.ToString(), "NUMBER", "I");

                DALOR.spArgumentsCollection(arrList, "@ret", "", "NUMBER", "O");
                DALOR.spArgumentsCollection(arrList, "@errormsg", "", "VARCHAR", "O", 4000);
                DALOR.spArgumentsCollection(arrList, "p_result", "", "REFCURSOR", "O");

                var res = DALOR.RunStoredProcedureRetError("G_SP_GetSetEmailNotification", arrList);

                responseModel.code = res.Ret;
                responseModel.msg = res.ErrorMsg;

                await ClearEmailQueue(emailConfiguration, emailDetails.token);
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public async Task ClearEmailQueue(EmailConfiguration emailConfiguration, string token)
        {

            ResponseModel responseModel = new ResponseModel();

            try
            {
                ArrayList arrList = new ArrayList();


                DALOR.spArgumentsCollection(arrList, "p_flag", "G", "CHAR", "I", 1);
                DALOR.spArgumentsCollection(arrList, "p_token", token, "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_toemailids", string.Empty, "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_subject", string.Empty, "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_body",  string.Empty, "CLOB", "I");
                DALOR.spArgumentsCollection(arrList, "p_hasattachment", "0", "NUMBER", "I");
                DALOR.spArgumentsCollection(arrList, "p_issent", "0", "NUMBER", "I");
                DALOR.spArgumentsCollection(arrList, "p_status", "A", "CHAR", "I", 1);
                DALOR.spArgumentsCollection(arrList, "p_ishtml", "0", "NUMBER", "I");

                DALOR.spArgumentsCollection(arrList, "@ret", "", "NUMBER", "O");
                DALOR.spArgumentsCollection(arrList, "@errormsg", "", "VARCHAR", "O", 4000);
                DALOR.spArgumentsCollection(arrList, "p_result", "", "REFCURSOR", "O");

                DataSet ds = new DataSet();
                var res = DALOR.RunStoredProcedureDsRetError("G_SP_GetSetEmailNotification", arrList, ds);

                responseModel.code = res.Ret;
                responseModel.msg = res.ErrorMsg;

                if (res.Ret > 0)
                {
                    string? toEmailIds, subject, body;
                    Boolean isHTML = false;

                    if (ds != null && ds.Tables.Count>0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            toEmailIds = row["ToEmailIds"].ToString();
                            subject = row["EmailSubject"].ToString();
                            body = row["EmailBody"].ToString();
                            
                            isHTML = Convert.ToBoolean(row["IsHTML"].ToString() == "1");
                            body = isHTML ? WebUtility.HtmlDecode(body) : body;
                            await SendEmail(emailConfiguration, toEmailIds, subject, body, token);
                        }
                    }
                }
              
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task SendEmail(EmailConfiguration emailConfiguration, string toEmailIds, string subject, string body, string token = "")
        {
            //if (UMSResources.configuration.GetSection("ByPass:sendMail").Value == "Y")
            if (emailConfiguration.ByPass)
            {
                //string smtpServerAddress = emailConfiguration.SmtpServerAddress;
                //int smtpServerPort = emailConfiguration.SmtpServerPort;
                //string smtpServerUserId = emailConfiguration.SmtpServerUserId;
                //string smtpServerPassword = emailConfiguration.SmtpServerPassword;
                //string emailFromAddress = emailConfiguration.EmailFromAddress;
                //string displayName = emailConfiguration.DisplayName;
                try
                {
                    MailMessage mail = new MailMessage
                    {
                        From = new MailAddress(emailConfiguration.EmailFromAddress, emailConfiguration.DisplayName)
                    };
                    if (toEmailIds.Contains(";"))
                    {
                        foreach (string item in toEmailIds.Split(';'))
                        {
                            mail.To.Add(item);
                        }
                    }
                    else
                    {
                        mail.To.Add(toEmailIds);
                    }

                    mail.Subject = $"{subject} for Date:{DateTime.Now.ToShortDateString()}";
                    //mail.Body = $"Hi <br>This is just a Test Email from .net.<br>No further action required.<br>{Environment.NewLine}<br>gNxt Systems, Thanks";
                    mail.Body = body;
                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient(emailConfiguration.SmtpServerAddress, emailConfiguration.SmtpServerPort);
                    smtp.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(emailConfiguration.SmtpServerUserId, emailConfiguration.SmtpServerPassword);
                    string userState = token;
                    smtp.SendAsync(mail, token);
                    //Console.WriteLine($"\tEmail notification process finished at {DateTime.UtcNow}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{ex.Message}");
                }
            }
        }
        private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            // Get the unique identifier for this asynchronous operation.
            string token = (string)e.UserState;
            string status = "";
            int isSent = 0;
            if (e.Cancelled)
            {
                isSent = 1;
                status = "C";
                Console.WriteLine("[{0}] Send canceled.", token);
            }
            if (e.Error != null)
            {
                isSent = 1;
                status = "F";
                Console.WriteLine("[{0}] {1}", token, e.Error.ToString());
            }
            else
            {
                isSent = 1;
                status = "S";
                Console.WriteLine("Message sent.");
            }

            try
            {
                ArrayList arrList = new ArrayList();


                DALOR.spArgumentsCollection(arrList, "p_flag", "U", "CHAR", "I", 1);
                DALOR.spArgumentsCollection(arrList, "p_token", token, "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_toemailids", string.Empty, "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_subject", string.Empty, "VARCHAR", "I");
                DALOR.spArgumentsCollection(arrList, "p_body", string.Empty, "CLOB", "I");
                DALOR.spArgumentsCollection(arrList, "p_hasattachment", "0", "NUMBER", "I");
                DALOR.spArgumentsCollection(arrList, "p_issent", isSent.ToString(), "NUMBER", "I");
                DALOR.spArgumentsCollection(arrList, "p_status", status, "CHAR", "I", 1);
                DALOR.spArgumentsCollection(arrList, "p_ishtml", "0", "NUMBER", "I");

                DALOR.spArgumentsCollection(arrList, "@ret", "", "NUMBER", "O");
                DALOR.spArgumentsCollection(arrList, "@errormsg", "", "VARCHAR", "O", 4000);
                DALOR.spArgumentsCollection(arrList, "p_result", "", "REFCURSOR", "O");

                var res = DALOR.RunStoredProcedureRetError("G_SP_GetSetEmailNotification", arrList);
            }
            catch (Exception ex)
            {
                throw;
            }

            mailSent = true;
        }

        //public async Task<EmailTemplate> GetEmailTemplate(EmailType emailType)
        //{
        //    try
        //    {
        //        ArrayList arrList = new ArrayList();
        //        DAL.spArgumentsCollection(arrList, "@MobileOrEmail", req.MobileOrEmail, "VARCHAR", "I");
        //        DAL.spArgumentsCollection(arrList, "@Ret", "", "INT", "O");
        //        DAL.spArgumentsCollection(arrList, "@ErrorMsg", "", "VARCHAR", "O");

        //        var res = DAL.RunStoredProcedureRetError("sp_SetOTP", arrList);


                
        //    }
        //    catch (Exception ex) 
        //    { 
            

        //    }
            
        //}
    }
}
