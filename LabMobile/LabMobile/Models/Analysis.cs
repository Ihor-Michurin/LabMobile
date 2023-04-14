namespace LabMobile.Models
{
    public class Analysis
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public Guid AnalysisReceptionPointId { get; set; }
        public Guid LaboratoryAssistantId { get; set; }
        public Guid NurseId { get; set; }
        public Guid RegistrarId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public decimal Price { get; set; }
        public string Method { get; set; }
        public string AnalysisValues { get; set; } // This property stores the JSON data
    }

    public enum AnalysisType
    {
        Blood,
        Iron,
    }
}
