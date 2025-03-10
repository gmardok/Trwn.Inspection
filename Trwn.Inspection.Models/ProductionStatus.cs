namespace Trwn.Inspection.Models
{
    public class ProductionStatus
    {
        public int NumberOfPcsCutting { get; set; }
        public int NumberOfPcsSewing { get; set; }
        public int NumberOfPcsPacked { get; set; }
        public bool MeasuringEquipmentChecking { get; set; }
        public bool FalseProducts { get; set; }
        public bool VerificationOfQuantity { get; set; }
        public bool ProductsIdentification { get; set; }
    }
}
