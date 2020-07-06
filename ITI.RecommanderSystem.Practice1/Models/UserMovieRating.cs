using CsvHelper.Configuration;

namespace ITI.RecommanderSystem.Practice1.Models
{
    public class UserMovieRating
    {
        public short UserId { get; set; }
        public short MovieId { get; set; }
        public short Rating { get; set; }

        public sealed class UserMovieRatingMap : ClassMap<UserMovieRating>
        {
            public UserMovieRatingMap()
            {
                Map(r => r.UserId).Index(0);
                Map(r => r.MovieId).Index(1);
                Map(r => r.Rating).Index(2);
            }
        }
    }
}
