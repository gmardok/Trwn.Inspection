using System;

namespace Trwn.Inspection.Models
{
    public class InspectionReport
    {
        public InspectionType InspectionType { get; set; }
        public string Name { get; set; } = null!;
        public string Inspector { get; set; } = null!;
        public string ReportNo { get; set; } = null!;
        public string Client { get; set; } = null!;
        public string ContractNo { get; set; } = null!;
        public string ArticleName { get; set; } = null!;
        public string Supplier { get; set; } = null!;
        public string Factory { get; set; } = null!;
        public string InspectionPlace { get; set; } = null!;
        public DateTime InspectionDate { get; set; }
    }
}
