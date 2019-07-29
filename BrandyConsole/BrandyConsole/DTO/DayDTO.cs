using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrandyConsole.DTO
{
    /// <summary>
    /// DTO(Data Transfer Object) for Generation information
    /// </summary>
    public class DayDTO
    {
        public DateTime Date { get; set; }
        public double Energy { get; set; }
        public double Price { get; set; }

        public double DailyEmissionsValue { get; set; }

    }
}
