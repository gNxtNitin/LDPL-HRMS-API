namespace UMS.Web.Models.ViewModel
{
    public class MenuPermissionModel
    {
        public string MenuPermissionId { get; set; }
        public string MenuId { get; set; }
        public string ParentId { get; set; }
        public string MenuName { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string PermissionId { get; set; }
    }

    public class MenuResponse
    {
        public List<MenuPermissionModel> Menus { get; set; }
    }
}
