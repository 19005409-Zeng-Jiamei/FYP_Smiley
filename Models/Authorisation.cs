using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
namespace FYP_Smiley.Models
{
    public partial class Authorisation
    {

        [Required(ErrorMessage = "Start Date is required")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End Date is required")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Access Point Id is required")]
        public int AccessPointId { get; set; }

        [Required(ErrorMessage = "User Id is required")]
        public string UserId { get; set; }


        public virtual AccessPoint AccessPoint { get; set; }
        public virtual SmileyUser User { get; set; }
    }
}
