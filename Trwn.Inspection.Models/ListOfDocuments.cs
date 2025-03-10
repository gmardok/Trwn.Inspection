namespace Trwn.Inspection.Models
{
    public class ListOfDocuments
    {
        public string Contract { get; set; } = null!;
        public bool Spectification { get; set; }
        public bool ProductEvaluationForm { get; set; }
        public bool TestReport { get; set; }
        public bool TrimCard { get; set; }
        public string Other { get; set; } = null!;
    }
}
