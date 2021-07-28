using System;
using System.Collections.Generic;

namespace FYP_Smiley.Models
{
    public partial class AccessLog
    {
        public int Sno { get; set; }
        public string UserId { get; set; }
        public string Direction { get; set; }
        public DateTime TimeStamp { get; set; }

        public virtual SmileyUser User { get; set; }
    }
}
