namespace Trwn.Inspection.Models
{
    public class InspectionDefect
    {
        public PhotoType DefectType { get; set; }
        public string Code { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string PictureNo { get; set; } = null!;
        public int Quantity { get; set; }
    }
}
