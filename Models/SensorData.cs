using System;
using System.Collections.Generic;

namespace FYP_Smiley.Models
{
    public partial class SensorData
    {
        public int Sno { get; set; }
        public int SensorId { get; set; }
        public string FeedbackGesture { get; set; }
        public DateTime TimeStamp { get; set; }

        public virtual Sensor Sensor { get; set; }
    }
}
