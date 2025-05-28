GroupManagement Library :-


#Packages needs to install into your web project:
=>Microsoft.Data.SqlClient (5.2.2)
=>Microsoft.EntityFrameworkCore.SqlServer(6.0.0)
=>Microsoft.Extensions.DependencyInjection.Abstractions(6.0.0)
=>Newtonsoft.Json(13.0.3)
=>System.Data.SqlClient(4.8.6)


#Add services to the web in Program.cs :
=>builder.Services.AddScoped<IGroupManagerService, GroupManagerService >();


#Add GroupManagementLibrary's DLL into your web project
=>Create a folder named Reference in web. Add DLL into this folder.


#In controller:
=>private readonly IGroupManagerService _groupManagerService; 


#Need to create a GroupMasterReqModel req model for Create Group into your web project;
Fields are:     
        public string GroupId { get; set; } 
        public string GroupName { get; set; }
        public string Description { get; set; }
        public int CreatedBy { get; set; } = 0;

& AssignGroupReqModel rq model for Assigning Group;
Fields are:
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public string GroupId { get; set; }


#Now you need create a object of model, then assign the values to the model's fields.
Example:
	    GroupMasterReqModel req = new GroupMasterReqModel ();
            rq.GroupName = "Group5";
            rq.Description = "Only for Review.";

           AssignGroupReqModel rq = new AssignGroupReqModel();
           rq.UserId = "8";
           rq.GroupId = "1";


#Now you have to create a variable to store the result of Creating/Updating Group,Get Group & Delete Group
And also for Assigning/Updating group of User/Roles & Getting List of Groups by Userid/Roleid;
Example:
         FOR GROUP :
	 //Here, it returns code, data and message respectively.
	 var resCreate = await _groupManagerService.CreateGroupMaster(req); //For Creating Group with flag 'C'
         var resUpdate = await _groupManagerService.UpdateGroupMaster(req); //For Updating Group with flag 'U'
         var resDelete = await _groupManagerService.DeleteGroupMaster(req); //For Delete Group with flag 'D'
         var resGet = await _groupManagerService.GetGroupMaster(groupId); //Getting List of All Roles with flag 'G' & for getting Individuall Role with flag 'I'

         FOR USER's/ROLE's GROUP :
         var resUpdate = await _groupManagerService.AssignOrUpdateUsersGroup(rq); //For Assigning/Updating Group to Users
         var groupByUser = await _groupManagerService.GetGroupByUserId(userId); //For Getting List of Groups with UserId
         var resUpdate1 = await _groupManagerService.AssignOrUpdateRolesGroup(rq); //For Assigning/Updating Group to Roles
         var groupByRole = await _groupManagerService.GetGroupByRoleId(roleId); //For Getting List of Groups with RoleId
     






