using System;

namespace Trwn.Inspection.Models
{
    public class FotoDocumentation
    {
        public Guid Id { get; set; }
        public InspectionDefectType DefectType { get; set; }
        public string Code { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string PicturePath { get; set; } = null!;
    }
}
