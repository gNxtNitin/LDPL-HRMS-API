Add services to the web.
=>builder.Services.AddScoped<IMenuManagementService,MenuManagementService>();

Add MenuManagementLib's DLL into your web project's Reference folder.

In controller:
=> private readonly IMenuManagementService _menuManagementService;

Now inside the constructor you have to intialize IMenuManagementService. 
 public AccountController(IMenuManagementService menuManagementService) 
 {
     _menuManagementService = menuManagementService;
 }

Now you have to create a model called MenuModel.
Fields are:

 public class MenuModel
 {
     public int MenuId { get; set; }
     public string MenuName { get; set; }
     public int? ParentId { get; set; }
     public List<MenuModel> Children { get; set; } = new List<MenuModel>();
 }

=>You can use this model(MenuModel) with BuildMenuHierarchy and BuildChildren methods.

There are following methods:

=>Menu : This method allow us to diplay all the menu present in the database.

=>Create : This method is used to create a new menu.

=>Edit : This method is used to Edit the existing menu.

=>Delete : By using this method we can update the IsActive from 1 to 0.










