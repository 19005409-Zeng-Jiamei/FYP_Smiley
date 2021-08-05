using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FYP_Smiley.Models
{
    public partial class AccessPoint
    {
        public AccessPoint()
        {
            Authorisation = new HashSet<Authorisation>();
            Sensor = new HashSet<Sensor>();
        }

        public int AccessPointId { get; set; }

        [Required(ErrorMessage = "FacilityId is required")]
        public int FacilityId { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        public virtual Facility Facility { get; set; }
        public virtual ICollection<Authorisation> Authorisation { get; set; }
        public virtual ICollection<Sensor> Sensor { get; set; }
    }
}
