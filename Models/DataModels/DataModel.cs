using System;

namespace DojoWall.Models
{
 
    public abstract class DataModel
    {
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}