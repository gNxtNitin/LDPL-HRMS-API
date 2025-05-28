using NotificationPreferenceLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationPreferenceLib.Interface
{
    public interface INotificationPreferenceService
    {
        /// <summary>
        /// Retrieves a specific notification preference by its unique identifier (NPID).
        /// </summary>
        /// <param name="npid">The unique identifier for the notification preference.</param>
        /// <returns>
        /// A task representing the asynchronous operation. The task result contains 
        /// the <see cref="NotificationPreference"/> object if found, or <c>null</c> if not found.
        /// </returns>
        /// <remarks>
        /// This method asynchronously fetches the notification preference from the data source based 
        /// on the provided NPID. It is designed to be non-blocking and is intended to be used with 
        /// <c>await</c> in asynchronous code.
        /// </remarks>
        public Task<NotificationPreference> GetNotificationPreferenceByIdAsync(int npid);

        /// <summary>
        /// Retrieves all available notification preferences from the data source asynchronously.
        /// </summary>
        /// <param name="npid">The unique identifier for the notification preference.</param>
        /// <returns>
        /// A task representing the asynchronous operation. The task result contains a list of 
        /// <see cref="NotificationPreference"/> objects, or an empty list if no preferences are found.
        /// </returns>
        /// <remarks>
        /// This method fetches all notification preferences without blocking the calling thread. 
        /// It is designed for use in asynchronous programming with the <c>await</c> keyword.
        /// </remarks>
        public Task<List<NotificationPreference>> GetAllNotificationPreferencesAsync();

        /// <summary>
        /// Asynchronously saves a new notification preference or updates an existing one in the data source.
        /// </summary>
        /// <param name="preference">The <see cref="NotificationPreference"/> object to be saved or updated.</param>
        /// <param name="isCreate">If <c>true</c>, a new record is created. If <c>false</c>, an existing record is updated.</param>
        /// <returns>A task representing the asynchronous save or update operation.</returns>
        /// <remarks>
        /// This method performs the save or update operation asynchronously to avoid blocking the calling thread. 
        /// It should be used with the <c>await</c> keyword in asynchronous methods.
        /// </remarks>
        public Task<bool> SaveOrUpdateNotificationPreferenceAsync(NotificationPreference preference, bool isCreate);


        /// <summary>
        /// Asynchronously deletes a notification preference by its unique identifier (NPID).
        /// </summary>
        /// <param name="npid">The unique identifier of the notification preference to be deleted.</param>
        /// <returns>A task representing the asynchronous delete operation.</returns>
        /// <remarks>
        /// This method performs the delete operation asynchronously to avoid blocking the calling thread. 
        /// It should be used with the <c>await</c> keyword in asynchronous methods.
        /// </remarks>
        public Task<bool> DeleteNotificationPreferenceAsync(int npid);

        /// <summary>
        /// Asynchronously retrieves a user notification preference by its unique identifier (UNPID).
        /// </summary>
        /// <param name="unpid">The unique identifier of the user notification preference to retrieve.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing the <see cref="UserNotificationPreference"/> if found.
        /// If no preference is found, the task result will be <c>null</c>.
        /// </returns>
        /// <remarks>
        /// This method performs the retrieval operation asynchronously to avoid blocking the calling thread. 
        /// It should be used with the <c>await</c> keyword in asynchronous methods.
        /// </remarks>
        public Task<List<UserNotificationPreference>> GetUserNotificationPreferenceByIdAsync(int unpid);


        /// <summary>
        /// Asynchronously retrieves all user notification preferences.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation, containing a list of <see cref="UserNotificationPreference"/> objects.
        /// If no preferences are found, an empty list will be returned.
        /// </returns>
        /// <remarks>
        /// This method retrieves all user notification preferences from the data source asynchronously, 
        /// allowing for non-blocking operations. It should be awaited in asynchronous methods.
        /// </remarks>
        public Task<List<UserNotificationPreference>> GetAllUserNotificationPreferencesAsync();


        /// <summary>
        /// Asynchronously saves or updates a user notification preference based on the specified operation type.
        /// </summary>
        /// <param name="userPreference">
        /// The <see cref="UserNotificationPreference"/> object containing the notification preference data to save or update.
        /// </param>
        /// <param name="isCreate">
        /// A boolean value indicating whether to create a new preference (<c>true</c>) or update an existing one (<c>false</c>).
        /// </param>
        /// <returns>
        /// A task representing the asynchronous save or update operation.
        /// </returns>
        /// <remarks>
        /// This method allows for asynchronous operations, either creating a new user notification preference 
        /// or updating an existing one based on the value of <paramref name="isCreate"/>.
        /// </remarks>
        public Task<bool> SaveOrUpdateUserNotificationPreferenceAsync(UserNotificationPreference userPreference, bool isCreate);


        /// <summary>
        /// Asynchronously deletes a user notification preference by its unique identifier.
        /// </summary>
        /// <param name="unpid">
        /// The unique identifier of the user notification preference to be deleted.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous delete operation.
        /// </returns>
        /// <remarks>
        /// This method allows for the deletion of a user notification preference from the database 
        /// based on the provided unique identifier <paramref name="unpid"/>.
        /// </remarks>
        public Task<bool> DeleteUserNotificationPreferenceAsync(int unpid);


    }
}
