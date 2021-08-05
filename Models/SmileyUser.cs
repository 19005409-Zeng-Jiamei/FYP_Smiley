using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FYP_Smiley.Models
{
    public partial class SmileyUser
    {
        public SmileyUser()
        {
            AccessLog = new HashSet<AccessLog>();
            Authorisation = new HashSet<Authorisation>();
            Facility = new HashSet<Facility>();
        }

        [Required(ErrorMessage = "User Id is required")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Full Name is required")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid date format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public byte[] Password { get; set; }

        [Required(ErrorMessage = "Voice File is required")]
        public string Voicefile { get; set; }

        [Required(ErrorMessage = "Pic File is required")]
        public string Picfile { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; }

        public string PhoneNum { get; set; }
        public DateTime? LastLogin { get; set; }

        public virtual ICollection<AccessLog> AccessLog { get; set; }
        public virtual ICollection<Authorisation> Authorisation { get; set; }
        public virtual ICollection<Facility> Facility { get; set; }
    }
}
