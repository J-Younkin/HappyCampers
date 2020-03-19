using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BeltExam.Models
  {
  public class Activity
  {
    public int ActivityId { get; set; }
    [Required]
    public string ActivityName { get; set; }
    [Required]
    public DateTime ActivityTime { get; set; }
    [Required]
    public DateTime ActivityDate { get; set; }
    public int ActivityLength { get; set; }
    public string ActivityDescription { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int UserId { get; set; }
    public User Coordinator { get; set; }
    public List<Participant> Participants { get; set; }
    public Activity()
    {
      Participants = new List<Participant>();
    }
  }
}