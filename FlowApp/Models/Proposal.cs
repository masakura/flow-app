﻿namespace FlowApp.Models
{
    public class Proposal
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}