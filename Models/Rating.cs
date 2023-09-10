﻿namespace Rater.Models
{
    public class Rating
    {
        public int UserId { get; set; }
        public int ReviewId { get; set; }
        public byte Value { get; set; }

        public User User { get; set; }
        public Review Review { get; set; }
    }
}