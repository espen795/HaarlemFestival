namespace HaarlemFestival.Models
{
    public class Activity
    {
        public int ActivityId { get; set; }
        public virtual Event Event { get; set; }
    }
}