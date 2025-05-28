using NotificationSenderLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationSenderLib
{
    public interface INotificationSenderService
    {
        /// <summary>
        /// Sends an email notification asynchronously.
        /// </summary>
        /// <param name="req">The notification request containing user details, message, and email information.</param>
        /// <returns>Returns true if the email was successfully sent, otherwise false.</returns>
        public Task<bool> SendEmailAsync(NotificationSenderModel req);

        /// <summary>
        /// Sends an SMS notification asynchronously.
        /// </summary>
        /// <param name="req">The notification request containing user details, message, and SMS information.</param>
        /// <returns>Returns true if the SMS was successfully sent, otherwise false.</returns>
        public Task<bool> SendSmsAsync(NotificationSenderModel req);

        /// <summary>
        /// Sends a notification based on the user's preferred notification method asynchronously.
        /// </summary>
        /// <param name="req">The notification request containing user details and message to be sent through the preferred notification method.</param>
        /// <returns>Returns true if the notification was successfully sent, otherwise false.</returns>
        public Task<bool> SendNotificationAsync(NotificationSenderModel req);
    }
}
