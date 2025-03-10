using System;

namespace Trwn.Inspection.Models
{
    public class InspectionReport
    {
        public Guid Id { get; set; }
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
        public InspectionOrderArticle[] InspectionOrder{ get; set; } = null!;
        public string QualityMark { get; set; } = null!;
        public string InspectionStandard { get; set; } = null!;
        public string InspectionSampling { get; set; } = null!;
        public int InspectionQuantity { get; set; }
        public int SampleSize { get; set; }
        public string InspectionCartonNo { get; set; } = null!;
        public DefectsSummary[] DefectsSummary { get; set; } = null!;
        public InspectionResultType InspectionResult { get; set; }
        public string InspectorName { get; set; } = null!;
        public string FactoryRepresentative { get; set; } = null!;
        public InspectionDefect[] InspectionDefects { get; set; } = null!;
        public string[] Remarks { get; set; } = null!;
        public ProductionStatus ProductionStatus { get; set; } = null!;
        public ListOfDocuments ListOfDocuments { get; set; } = null!;
        public FotoDocumentation[] FotoDocumentation { get; set; } = null!;
    }
}
