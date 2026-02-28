using System.Collections.Generic;

namespace Trwn.Inspection.Models
{
    public class PhotoDocumentation
    {
        public int Id { get; set; }
        public PhotoType PhotoType { get; set; }
        public int Code { get; set; }
        public string Description { get; set; } = null!;
        public string PicturePath { get; set; } = null!;
        public int Count { get; set; }
    }
}
