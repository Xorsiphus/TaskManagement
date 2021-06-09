namespace TaskManagement.Models.Models
{
    public class TreeItemModel
    {
        public string Id { get; set; }
        
        public string Parent { get; set; }

        public string Name { get; set; }

        public bool IsParent { get; set; }
        
        public string Status { get; set; }
    }
}