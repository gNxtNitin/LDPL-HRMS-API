using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotificationPreferenceLib.Interface;
using NotificationPreferenceLib.Models;
using Services.Interfaces;

namespace Services.Implementation
{
    public class NotificationService: INotificationService
    {
        private readonly INotificationPreferenceService _notificationService;
        public NotificationService(INotificationPreferenceService notificationService)
        {
            _notificationService = notificationService;
        }
        public async Task<NotificationPreference> GetNotificationPreferenceById(int npid)
        {
            NotificationPreference np = new NotificationPreference();
            np = await _notificationService.GetNotificationPreferenceByIdAsync(npid);
            return np;
        }
        public async Task<List<NotificationPreference>> GetAllNotificationPreference()
        {
            List<NotificationPreference> np = new List<NotificationPreference>();
            np = await _notificationService.GetAllNotificationPreferencesAsync();
            return np;
        }
        public async Task<bool> SaveOrUpdateNotificationPreference(NotificationPreference preference, bool isCreate)
        {
            bool b = false;
            b = await _notificationService.SaveOrUpdateNotificationPreferenceAsync(preference, isCreate);
            return b;
        }
        public async Task<bool> DeleteNotificationPreference(int npid)
        {
            bool b = false;
            b = await _notificationService.DeleteNotificationPreferenceAsync(npid);
            return b;
        }
        public async Task<List<UserNotificationPreference>> GetUserNotificationPreferenceById(int id)
        {
            List<UserNotificationPreference> unp = new List<UserNotificationPreference>();
            unp = await _notificationService.GetUserNotificationPreferenceByIdAsync(id);
            return unp;
        }
        public async Task<List<UserNotificationPreference>> GetAllUserNotificationPreferences()
        {
            List<UserNotificationPreference> unp = new List<UserNotificationPreference>();
            unp = await _notificationService.GetAllUserNotificationPreferencesAsync();
            return unp;
        }
        public async Task<bool> SaveOrUpdateUserNotificationPreference(UserNotificationPreference userPreference, bool isCreate)
        {
            bool b = false;
            b = await _notificationService.SaveOrUpdateUserNotificationPreferenceAsync(userPreference, isCreate);
            return b;
        }
        public async Task<bool> DeleteUserNotificationPreference(int unpid)
        {
            bool b = false;
            b = await _notificationService.DeleteUserNotificationPreferenceAsync(unpid);
            return b;
        }
    }
}
