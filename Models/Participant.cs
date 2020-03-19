using System;
using System.Collections.Generic;

namespace BeltExam.Models
{
    public class Participant
    {
        public int ParticipantId { get; set; }
        public int ActivityId { get; set; }
        public Activity Activity { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}