using System.Collections.Generic;

namespace BrandyConsole.DTO
{
    /// <summary>
    /// DTO(Data Transfer Object) for Generator details.
    /// </summary>
    public class GeneratorDTO
    {
        public string Name { get; set; }
        public List<DayDTO> Generation { get; set; }
        public double EmissionsRating { get; set; }
        public double TotalHeatInput { get; set; }
        public double ActualNetGeneration { get; set; }
        public double DailyGenerationValue { get; set; }
        public double DailyEmissionsValue { get; set; }
        public double ActualHeatRate { get; set; }

        public string Location { get; set; }
    }
}
