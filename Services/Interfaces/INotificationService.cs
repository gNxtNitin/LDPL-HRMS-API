using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotificationPreferenceLib.Models;

namespace Services.Interfaces
{
    public interface INotificationService
    {
        public  Task<NotificationPreference> GetNotificationPreferenceById(int npid);
        public  Task<List<NotificationPreference>> GetAllNotificationPreference();
        public  Task<bool> SaveOrUpdateNotificationPreference(NotificationPreference preference, bool isCreate);
        public  Task<bool> DeleteNotificationPreference(int npid);
        public  Task<List<UserNotificationPreference>> GetUserNotificationPreferenceById(int id);
        public  Task<List<UserNotificationPreference>> GetAllUserNotificationPreferences();
        public  Task<bool> SaveOrUpdateUserNotificationPreference(UserNotificationPreference userPreference, bool isCreate);
        public  Task<bool> DeleteUserNotificationPreference(int unpid);
    }
}
