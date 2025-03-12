using System;
using System.Collections.Generic;

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
        public List<InspectionOrderArticle> InspectionOrder{ get; set; } = new List<InspectionOrderArticle>();
        public string QualityMark { get; set; } = null!;
        public string InspectionStandard { get; set; } = null!;
        public string InspectionSampling { get; set; } = null!;
        public int InspectionQuantity { get; set; }
        public int SampleSize { get; set; }
        public string InspectionCartonNo { get; set; } = null!;
        public List<DefectsSummary> DefectsSummary { get; set; } = new List<DefectsSummary>();
        public InspectionResultType InspectionResult { get; set; }
        public string InspectorName { get; set; } = null!;
        public string FactoryRepresentative { get; set; } = null!;
        public List<InspectionDefect> InspectionDefects { get; set; } = new List<InspectionDefect>();
        public List<string> Remarks { get; set; } = null!;
        public ProductionStatus ProductionStatus { get; set; } = null!;
        public ListOfDocuments ListOfDocuments { get; set; } = null!;
        public List<FotoDocumentation> FotoDocumentation { get; set; } = new List<FotoDocumentation>();
    }
}
