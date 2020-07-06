using CsvHelper.Configuration;

namespace ITI.RecommanderSystem.Practice1.Models
{
    public class User
    {
        public short UserId { get; set; }
        public char Gender { get; set; }
        public short Age { get; set; }
        public string Occupation { get; set; }
        public string ZipCode { get; set; }

        public sealed class UserMap : ClassMap<User>
        {
            public UserMap()
            {
                Map(u => u.UserId).Index(0);
                Map(u => u.Gender).Index(1);
                Map(u => u.Age).Index(2);
                Map(u => u.Occupation).Index(3);
                Map(u => u.ZipCode).Index(4);
            }
        }
    }
}
