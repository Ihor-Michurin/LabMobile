namespace LabMobile.Models
{
    public class LaboratoryAssistant
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string ContactInformation { get; set; }
    }
}
