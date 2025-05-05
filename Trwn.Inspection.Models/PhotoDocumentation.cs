using System.Collections.Generic;

namespace Trwn.Inspection.Models
{
    public class PhotoDocumentation
    {
        public PhotoType PhotoType { get; set; }
        public int Code { get; set; }
        public string Description { get; set; } = null!;
        public string PicturePath { get; set; } = null!;
    }
}
