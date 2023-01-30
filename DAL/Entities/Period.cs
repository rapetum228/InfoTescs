namespace DAL.Entities
{
    public class Period
    {
        public long Id { get; set; }
        public int Days { get; set; }
        public byte Hours { get; set; }
        public byte Minutes { get; set; }
        public byte Seconds { get; set; }
    }
}
