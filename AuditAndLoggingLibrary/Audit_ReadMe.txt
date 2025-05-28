AuditAndLogging Library :-


#Packages needs to install into your web project:
=>Microsoft.Data.SqlClient (5.2.2)
=>Microsoft.EntityFrameworkCore.SqlServer(6.0.0)
=>Microsoft.Extensions.DependencyInjection.Abstractions(6.0.0)
=>Newtonsoft.Json(13.0.3)
=>System.Data.SqlClient(4.8.6)


#Add services to the web in Program.cs :
=>builder.Services.AddScoped<IAuditAndLoggingService ,AuditAndLoggingService ();


#Add AuditAndLoggingLibrary's DLL into your web project
=>Create a folder named Reference in web. Add DLL into this folder.


#In controller:
=>private readonly IAuditAndLoggingService _auditAndLoggingService; 


#Add AuditLog and AuditLogActionFilters class(For Handle Log) in web Project;
=>AuditLog .cs :-
        public int Id { get; set; }
        public string UserName { get; set; } // Optionally capture the username or user ID
        public string Controller { get; set; }
        public string Action { get; set; }
        public DateTime Timestamp { get; set; }
        public string Status { get; set; } // Success or Failure
        public string RequestData { get; set; } // Request body or parameters
        public string ResponseData { get; set; } // Response body
        public string ErrorMessage { get; set; } // If action failed, capture the error message

=>AuditLogActionFilters.cs :-
            AuditMasterReqModel reqModel = new AuditMasterReqModel();
            //reqModel.UserID = ;
            reqModel.IPAddress = context.HttpContext.Connection.RemoteIpAddress.ToString();
            reqModel.Module = auditLog.Controller;
            reqModel.Action = auditLog.Action;
            reqModel.ActionStatus = auditLog.Status;
            reqModel.SessionID = context.HttpContext.Session.Id;
            await _auditAndLoggingService.AuditLogAction(reqModel);


#Need to create a AuditMasterReqModel req model for Maintaining the AuditLog into your web project;
Fields are:

            public string AuditLogID { get; set; }
            public string UserID { get; set; }
            public string IPAddress { get; set; }
            public string Module { get; set; }
            public string Action { get; set; }
            public string ActionStatus { get; set; }
            public string SessionID { get; set; }


#Now you need create a object of model, then assign the values to the model's fields.
Example:

               AuditAndLoggingLibrary.Models.AuditMasterReqModel reqModel = new AuditAndLoggingLibrary.Models.AuditMasterReqModel();

               //Mantain AuditLog Of a User
               reqModel.UserID = "2";
               reqModel.IPAddress = HttpContext.Connection.RemoteIpAddress.ToString();
               reqModel.Module = ControllerContext.ActionDescriptor.ControllerName + " Module";
               reqModel.Action = ControllerContext.ActionDescriptor.ActionName;
               //reqModel.ActionStatus = ; 
               reqModel.SessionID = HttpContext.Session.Id;


#Now you have to create a variable to store the result of Maintaining the AuditLog
Example:

	 //Here, it returns code, data and message respectively.
	 var result = await _auditAndLoggingService.AuditLogAction(reqModel);









