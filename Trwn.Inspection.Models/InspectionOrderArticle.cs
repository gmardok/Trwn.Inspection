namespace Trwn.Inspection.Models
{
    public class InspectionOrderArticle
    {
        public int LotNo { get; set; }
        public string ArticleNumber { get; set; } = null!;
        public int OrderQuantity { get; set; }
        public int ShipmentQuantityPcs { get; set; }
        public int ShipmentQuantityCartons { get; set; }
        public int UnitsPacked { get; set; }
        public int UnitsFinishedNotPacked { get; set; }
        public int UnitsNotFinished { get; set; }
    }
}
