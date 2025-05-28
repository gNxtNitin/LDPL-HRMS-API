namespace LDPLWEBAPI.Models
{
    public class Monol
    {
        public int MenuId { get; set; }
        public int? ParentId { get; set; }
        public string? MenuName { get; set; }
        public string? Areas { get; set; }
        public string? ControllerName { get; set; }
        public string? ActionName { get; set; }
        public string? Url { get; set; }
        public int Order { get; set; }
        public bool IsActive { get; set; }
        public string? IconClass { get; set; }
    }
}
