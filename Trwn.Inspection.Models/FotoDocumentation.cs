namespace Trwn.Inspection.Models
{
    public class FotoDocumentation
    {
        public InspectionDefectType DefectType { get; set; }
        public string Code { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string PicturePath { get; set; } = null!;
    }
}
