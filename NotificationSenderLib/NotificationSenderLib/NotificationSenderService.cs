using DatabaseManager;
using EmailService.Library;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NotificationPreferenceLib.Interface;
using NotificationSenderLib.Models;
using System.Collections;
using System.Data;
using System.Net.Mail;
using System.Net;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.SignalR;
using UserManagementLibrary.Models;
using ModelsLibrary.Models;

namespace NotificationSenderLib
{
    public class NotificationSenderService : INotificationSenderService
    {
        private readonly IEmailSerivce _emailService;
        private readonly IConfiguration _configuration;
        private readonly INotificationPreferenceService _notificationPreferenceService;
        MiscDataSetting _miscDataSetting = new MiscDataSetting();


        public NotificationSenderService(INotificationPreferenceService notificationPreferenceService, IConfiguration configuration, IEmailSerivce emailService)
        {
            _configuration = configuration;
            _notificationPreferenceService = notificationPreferenceService;
            _emailService = emailService;
        }

        private async Task<ResponseModel> GetUsers(string? userId)
        {
            ResponseModel response = new ResponseModel();
            string flag = userId == null || userId == "" ? "G" : "I";
            try
            {
                DataSet ds = new DataSet();
                ArrayList arrList = new ArrayList();
                DAL.spArgumentsCollection(arrList, "@Flag", flag, "CHAR", "I");
                DAL.spArgumentsCollection(arrList, "@userId", userId == "" ? "0" : userId, "INT", "I");
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

        public async Task<bool> SendNotificationAsync(NotificationSenderModel req)
        {
            bool result = false;
            var userPreference1 = await _notificationPreferenceService.GetUserNotificationPreferenceByIdAsync(req.UserId);
            NotificationPreferenceLib.Models.NotificationPreference notification = new NotificationPreferenceLib.Models.NotificationPreference();
            for (int i = 0; i < userPreference1.Count(); i++)
            {
                notification = userPreference1[i].Preference;


                if (userPreference1[i] == null || userPreference1[i].Preference == null)
                {
                    Console.WriteLine($"No preferences found for UserId {req.UserId}");
                    return false;
                }


                req.NotificationType = notification.Preference;
                var res = await GetUsers(req.UserId.ToString());

                UserResponse userResponse = JsonConvert.DeserializeObject<UserResponse>(res.data.ToString());

                var user = userResponse.Users.FirstOrDefault();
                if (user != null)
                {
                    req.Email = user.Email;
                    req.PhoneNumber = user.Mobile;
                }


                


                switch (req.NotificationType)
                {
                    case "Email":

                        result = await SendEmailAsync(req);
                        break;
                    case "SMS":

                        result = await SendSmsAsync(req);
                        break;
                    default:
                        Console.WriteLine("Invalid notification type.");
                        break;
                }
            }
            

            return result;
        }
                
        public async Task<bool> SendEmailAsync(NotificationSenderModel req)
        {
            try
            {
                int ncid = await LogNotificationAsync(req, "C");
                EmailDetails emailDetails = new EmailDetails
                {
                    ToEmailIds = req.Email,
                    subject = req.Subject,
                    body = req.Message,
                    token = Guid.NewGuid().ToString(),  
                    isHTML = req.isHTML
                };
                EmailConfiguration emailConfig = new EmailConfiguration();

                await _emailService.QueueEmail(emailConfig, emailDetails);

                // update Email          
                bool isUpdate = await UpdateSmsAsync(ncid);

                return isUpdate;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> SendSmsAsync(NotificationSenderModel req)
         {
            try
            {
                int ncid = await LogNotificationAsync(req,"C");
                //send sms
                const string accountSid = "";
                const string authToken = "";

                TwilioClient.Init(accountSid, authToken);
                try
                {
                   var message = await MessageResource.CreateAsync(
                       to: new PhoneNumber(req.PhoneNumber.Trim()),
                       from: new PhoneNumber(""),
                       body: req.Message
                   );
                }
                catch(Exception ex)
                {                                        
                    return false;
                }
                // update sms          
                bool isUpdate = await UpdateSmsAsync(ncid);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending SMS: " + ex.Message);
                return false;
            }
        }


        //Update
        private async Task<bool> UpdateSmsAsync(int ncid)
        {
            try
            {
                NotificationSenderModel forUpdate = new NotificationSenderModel();                
                forUpdate.NCID = ncid;
                await LogNotificationAsync(forUpdate, "U");                              
                return true; 
            }
            catch (Exception ex)
            {                                
                return false;
            }
        }
         
        private async Task<int> LogNotificationAsync(NotificationSenderModel notification, string flag)
        {
            try
            {
                ArrayList arrList = new ArrayList();
                DAL.spArgumentsCollection(arrList, "@Flag", flag, "CHAR", "I");
                DAL.spArgumentsCollection(arrList, "@UserId", notification.UserId.ToString(), "INT", "I");

                int ncidValue = notification.NCID ?? 0; 
                DAL.spArgumentsCollection(arrList, "@NCID", ncidValue.ToString(), "INT", "I");

                DAL.spArgumentsCollection(arrList, "@Message", notification.Message, "NVARCHAR", "I");
                DAL.spArgumentsCollection(arrList, "@CreatedBy", notification.UserId.ToString(), "INT", "I");

                DAL.spArgumentsCollection(arrList, "@errorMsg", "", "VARCHAR", "O");
                DAL.spArgumentsCollection(arrList, "@ret", "", "INT", "O");

             
                var ds = DAL.RunStoredProcedureRetError("sp_GetSetDeleteNotificationCenter", arrList);
                return ds.Ret;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }


    }
}
