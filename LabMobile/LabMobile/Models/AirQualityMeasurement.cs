namespace LabMobile.Models
{
    public class AirQualityMeasurement
    {
        public Guid Id { get; set; }
        public Guid ReceptionPointId { get; set; }
        public DateTime Date { get; set; }
        public decimal Temperature { get; set; }
        public decimal Pressure { get; set; }
        public decimal Humidity { get; set; }
        public decimal Gas { get; set; }
        public decimal Iaq { get; set; }
        public bool IsNormal { get; set; }
    }
}
