namespace Trwn.Inspection.Models
{
    public class DefectsSummary
    {
        public PhotoType DefectType { get; set; }
        public int Quantity { get; set; }
        public int AllowedQuantity { get; set; }
        public string Notes { get; set; } = null!;
    }
}
