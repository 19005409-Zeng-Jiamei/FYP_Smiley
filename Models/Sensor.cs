using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FYP_Smiley.Models
{
    public partial class Sensor
    {
        public Sensor()
        {
            SensorData = new HashSet<SensorData>();
        }

        public int SensorId { get; set; }

        [Required(ErrorMessage = "Access Point Id is required")]
        public int AccessPointId { get; set; }

        [Required(ErrorMessage = "Sensor Name is required")]
        public string SensorName { get; set; }

        public virtual AccessPoint AccessPoint { get; set; }
        public virtual ICollection<SensorData> SensorData { get; set; }
    }
}
