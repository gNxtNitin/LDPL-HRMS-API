Packages need to install:
=>Newtonsoft.Json(13.0.3)
=>Twilio(7.5.0)
=>Microsoft.AspNetCore.SignalR(1.0.4)

Create a folder Hubs : Inside this folder create a class NotificationHub.
Example:
    public class NotificationHub : Hub
    {
        public async Task SendNotification(string message)
        {
            await Clients.All.SendAsync("ReceiveNotification", message);
        }
    }


Add services to the web.
builder.Services.AddScoped<INotificationSenderService, NotificationSenderService>();
builder.Services.AddScoped<IEmailSerivce, EmailSerivce>();
builder.Services.AddScoped<NotificationHub>();
builder.Services.AddSignalR();
builder.Services.AddScoped<InAppNotificationManager>();
// Configure Twilio client program.cs
var twilioOptions = builder.Configuration.GetSection("Twilio");
TwilioClient.Init(twilioOptions["AccountSid"], twilioOptions["AuthToken"]);


Add NotificationSenderService's DLL into your web project
=>Create a folder named Reference in web. Add DLL into this folder.

Need to map the Hub in program file.
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Account}/{action=Index}/{id?}");

    endpoints.MapHub<NotificationHub>("/notificationHub"); // Map the hub
});



In Controller:
private readonly INotificationSenderService _notificationSender;
private readonly IHubContext<NotificationHub> _hubContext;
private readonly InAppNotificationManager _inAppNotificaitonManger;

Now inside the constructor you have to intialize INotificationSenderService.
public AccountController(INotificationSenderService notificationSender,IHubContext<NotificationHub> hubContext,InAppNotificationManager inAppNotificaitonManger)
 {
     _inAppNotificaitonManger = inAppNotificaitonManger;
     _hubContext = hubContext;
     _notificationSender = notificationSender;
 }

You have to create a model called NotificationSenderModel into your web project.
Fields are:
	  public class NotificationSenderModel
  	    {	
      		public int UserId { get; set; }
      		public string Email { get; set; }
      		public string PhoneNumber { get; set; }
      		public string Subject { get; set; }
     		public int NotificationID { get; set; }
      		public string Message { get; set; }
      		public int IsSent { get; set; } 
      		public DateTime SentDate { get; set; }
      
      		public string NotificationType { get; set; } 
  	    }


You need to create a object of NotificationSenderModel.
Example:
	NotificationSenderModel reqq = new NotificationSenderModel();
	
 	reqq.Subject = "Email's subject";

 	reqq.PhoneNumber = "9123123123";
 	reqq.Message = "Email message";

Then create a variable to store the result of NotificationSenderService.
Example:

	This for Email and SMS.
        =>var res = await _notificationSender.SendNotificationAsync(reqq);


	InAppNotification ob = new InAppNotification();
   	ob.Message = "This is InApp notificaition";

   	Add new notification into InAppNotification table.
   	_inAppNotificaitonManger.SendNotification(ob.Message);

  	Get all the InAppNotification form the table.
  	var unReadMsgs = await _inAppNotificaitonManger.GetUnreadNotifications();

  	Mark as Read.
 	_inAppNotificaitonManger.MarkAsRead(3);






