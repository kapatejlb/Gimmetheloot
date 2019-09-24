using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Entities.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public string Text { get; set; }

        public DateTime Date { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
