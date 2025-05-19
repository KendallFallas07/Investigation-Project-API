namespace Model
{
    public class Game
    {

        public int Id { get; set; }
        public string? Title { get; set; }
        public string? ImageUrl { get; set; }
        public string? Platform { get; set; }
        public int HoursPlayed { get; set; }
        public bool IsCompleted { get; set; }
        public string? Genre { get; set; }

    }   
}
