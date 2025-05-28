RoleManagement Library :-


#Packages needs to install into your web project:
=>Microsoft.Data.SqlClient (5.2.2)
=>Microsoft.EntityFrameworkCore.SqlServer(6.0.0)
=>Microsoft.Extensions.DependencyInjection.Abstractions(6.0.0)
=>Newtonsoft.Json(13.0.3)
=>System.Data.SqlClient(4.8.6)


#Add services to the web in Program.cs:
=>builder.Services.AddScoped<IRoleManagerService ,RoleManagerService ();


#Add RoleManagementLibrary's DLL into your web project
=>Create a folder named Reference in web. Add DLL into this folder.


#In controller:
=>private readonly IRoleManagerService _roleManagerService; 


#Need to create a RoleMasterReqModel req model for Create Role into your web project; 
Fields are:
        public string RoleId { get; set; } 
        public string RoleName { get; set; }
        public string Description { get; set; }
        public int CreatedBy { get; set; } = 0;

& AssignRoleReqModel rq model for Assigning Role;
Fields are:
        public string UserId { get; set; }
        public string RoleId { get; set; }


#Now you need create a object of model, then assign the values to the model's fields.
Example:
	   RoleMasterReqModel req = new RoleMasterReqModel();
           req.RoleName = "Admin";
           req.Description = "Manage All Users";

           AssignRoleReqModel rq = new AssignRoleReqModel();
           rq.UserId = "8";
           rq.RoleId = "1";


#Now you have to create a variable to store the result of Creating/Updating Role,Get Role & Delete Role 
And also for Updating Assigned role of User & Getting List of Roles;
Example:
         FOR ROLE :-
	 //Here, it returns code, data and message respectively.
	 var resCreate = await _roleManagerService.CreateRoleMaster(req); //Creating Role with flag 'C'
         var resUpdate = await _roleManagerService.UpdateRoleMaster(req); //Updating Role with flag 'U'
         var resDelete = await _roleManagerService.DeleteRoleMaster(req); //Delete Role with flag 'D'
         var resGet = await _roleManagerService.GetRoleMaster(roleId); //Getting List of All Roles with flag 'G' & for getting Individuall Role with flag 'I'
   
         FOR USER's ROLE :-
         var resUpdate = await _roleManagerService.UpdateUsersRole(rq); //Update the Assigned role to User
         var RoleByUserid = await _roleManagerService.GetRoleByUserId(userId); //Getting List of Roles by UserId






