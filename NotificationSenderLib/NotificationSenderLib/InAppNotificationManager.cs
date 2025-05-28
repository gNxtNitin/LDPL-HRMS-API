using DatabaseManager;
using Microsoft.AspNetCore.SignalR;
using NotificationSenderLib.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NotificationSenderLib
{
    public class InAppNotificationManager
    {
        private List<InAppNotification> _notifications = new List<InAppNotification>();
        private int _nextId = 1;
               
        public async void SendNotification(string message)
        {
            var notification = new InAppNotification
            {
                Id = _nextId++,
                Message = message,
                CreatedAt = DateTime.Now,
                IsRead = false
            };
            _notifications.Add(notification);
            await LogInAppNotificationsAsync(notification, "Add");
            
        }

        public async Task<List<InAppNotification>> GetUnreadNotifications()
        {
            var notifications = new List<InAppNotification>();

            try
            {
                ArrayList arrList = new ArrayList();
                DAL.spArgumentsCollection(arrList, "@flag", "GetUnread", "NVARCHAR", "I");
                DAL.spArgumentsCollection(arrList, "@errormsg", "", "NVARCHAR", "O");
                DAL.spArgumentsCollection(arrList, "@ret", "0", "INT", "O");

                DataSet ds = DAL.RunStoredProcedure(new DataSet(), "sp_ManageInAppNotifications", arrList);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var inAppTable = ds.Tables[0];

                   
                    foreach (DataRow row in inAppTable.Rows)
                    {
                        notifications.Add(new InAppNotification
                        {
                            Id = Convert.ToInt32(row["Id"]),
                            Message = row["Message"].ToString(),
                            CreatedAt = Convert.ToDateTime(row["CreatedAt"]),
                            IsRead = Convert.ToBoolean(row["IsRead"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving unread notifications: " + ex.Message);
            }

            return notifications;
        }


        public async void MarkAsRead(int id)
        {
            try
            {
                ArrayList arrList = new ArrayList();
                DAL.spArgumentsCollection(arrList, "@flag", "MarkAsRead", "NVARCHAR", "I");
                DAL.spArgumentsCollection(arrList, "@Id", id.ToString(), "INT", "I");
                DAL.spArgumentsCollection(arrList, "@errormsg", "", "NVARCHAR", "O");
                DAL.spArgumentsCollection(arrList, "@ret", "0", "INT", "O");

                var dbReturnList = await Task.Run(() => DAL.RunStoredProcedureRetError("sp_ManageInAppNotifications", arrList));

               
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error marking notification as read: " + ex.Message);
            }
        }

        private async Task<int> LogInAppNotificationsAsync(InAppNotification notification, string flag)
        {
            try
            {
                ArrayList arrList = new ArrayList();
                DAL.spArgumentsCollection(arrList, "@flag", flag, "NVARCHAR", "I");
                DAL.spArgumentsCollection(arrList, "@Id", notification.Id.ToString(), "INT", "I");
                DAL.spArgumentsCollection(arrList, "@Message", notification.Message, "NVARCHAR", "I");
                DAL.spArgumentsCollection(arrList, "@errormsg", "", "NVARCHAR", "O");

                DAL.spArgumentsCollection(arrList, "@ret", "0", "INT", "O");
                var ds = DAL.RunStoredProcedureRetError("sp_ManageInAppNotifications", arrList);
                return ds.Ret;
                
            }
            catch (Exception ex)
            {
                return 0;
                
            }
        }
    }
}

