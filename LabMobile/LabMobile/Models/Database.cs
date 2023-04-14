using LabMobile.Models;
using Microsoft.CodeAnalysis;
using System.Drawing;
using System.Numerics;

namespace LabMobile.Models
{
    public class Database
    {
        public List<AirQualityMeasurement>? AirQualityMeasurements { get; set; }
        public List<Analysis>? Analysis { get; set; }
        public List<AnalysisReceptionPoint>? AnalysisReceptionPoints { get; set; }
        public List<LaboratoryAssistant>? LaboratoryAssistants { get; set; }
        public List<Nurse>? Nurses { get; set; }
        public List<Patient>? Patients { get; set; }
        public List<Registrar>? Registrars { get; set; }
    }
}
