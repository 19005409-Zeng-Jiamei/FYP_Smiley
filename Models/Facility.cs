using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FYP_Smiley.Models
{
    public partial class Facility
    {
        public Facility()
        {
            AccessPoint = new HashSet<AccessPoint>();
        }

        public int FacilityId { get; set; }

        [Required(ErrorMessage = "AdminId is required")]
        public string AdminId { get; set; }

        [Required(ErrorMessage = "FacilityName is required")]
        public string FacilityName { get; set; }

        [Required(ErrorMessage = "PostalCode is required")]
        public int PostalCode { get; set; }

        [Required(ErrorMessage = "BlockNumber is required")]
        public string BlockNumber { get; set; }

        [Required(ErrorMessage = "StreetName is required")]
        public string StreetName { get; set; }
        public string BannerPic { get; set; }

        public virtual SmileyUser Admin { get; set; }
        public virtual ICollection<AccessPoint> AccessPoint { get; set; }
    }
}
