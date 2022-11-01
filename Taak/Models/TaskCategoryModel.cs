namespace Taak.Models
{
    public class TaskCategoryModel
    {
        public Guid IdTaskCategory { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
