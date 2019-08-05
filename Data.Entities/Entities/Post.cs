using System;

namespace Data.Entities.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public string Text { get; set; }

        public DateTime Date { get; set; }


        public string AspNetUserId { get; set; }
        public AspNetUsers AspNetUsers { get; set; }




        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
