Add service to the web
=>builder.Services.AddScoped<INotificationPreferenceService, NotifcationPreferenceService>();

Add NotificationPreferenceLib's DLL into your web project
=>Create a folder named Reference in web. Add DLL into that folder.

In controller:
=> private readonly INotificationPreferenceService _notificationPreferenceService;

Now inside the constructor you have to intialize INotificationPreferenceService
public AccountController(INotificationPreferenceService notificationPreferenceService)
 {
     _notificationPreferenceService = notificationPreferenceService;
 }

You have to create two models, NotificationPreference and UserNotificationPreference into your web project.
Fields are:
	 public class NotificationPreference
 	    {
     		public int NPID { get; set; }
     		public string Preference { get; set; } 
     		public DateTime CreatedDate { get; set; }
     		public string? CreatedBy { get; set; }  
     		public string? ModifiedBy { get; set; }  
     		public DateTime? ModifiedDate { get; set; }  
 	    }

	 public class UserNotificationPreference
	    {
    		public int UNPID { get; set; }  
    		public int NPID { get; set; }  
    		public int UserId { get; set; }  
    		public int? CreatedBy { get; set; }  
    		public DateTime CreatedDate { get; set; }
    		public int? ModifiedBy { get; set; }  
    		public DateTime? ModifiedDate { get; set; }  

    		public NotificationPreference Preference { get; set; } 
	    }

Now you need to create a object of NotificationPreference model, then assign the values to the model's fields.
Example:
	NotificationPreference n = new NotificationPreference();
   	n.Preference = "SMS";
   	n.CreatedDate = DateTime.Now;
   	n.CreatedBy = "3";

Pass the object of NotificationPreference model and a boolean value to the method as arguments to create a Notification preference.

=>await _notificationPreferenceService.SaveOrUpdateNotificationPreferenceAsync(n, true);

Create a object of UserNotificationPreference model.
Example:
	var userPreference = new UserNotificationPreference
	{
    	    NPID = 1,
    	    UserId = 3,
    	    CreatedBy = 1, 
    	    CreatedDate = DateTime.Now
	};


You have to pass the object of UserNotificationPreference model and a boolean value as arguments to the method to create a User Notification preference.
 =>await _notificationPreferenceService.SaveOrUpdateUserNotificationPreferenceAsync(userPreference, true);


To delete the UserNotificationPreference and NotificationPreference, pass the UNPID and NPID respectively.
Example:
	 =>await _notificationPreferenceService.DeleteNotificationPreferenceAsync(NPID);
 	 =>await _notificationPreferenceService.DeleteUserNotificationPreferenceAsync(UNPID);


Similarly you can use get methods also to get the UserNotificationPreference and NotificationPreference.
 =>await _notificatioPreferenceService.GetAllNotificationPreferencesAsync();
 =>await _notificatioPreferenceService.GetNotificationPreferenceByIdAsync(NPID);

 =>await _notificatioPreferenceService.GetAllUserNotificationPreferencesAsync();
 =>await _notificatioPreferenceService.GetUserNotificationPreferenceByIdAsync(UNPID);


