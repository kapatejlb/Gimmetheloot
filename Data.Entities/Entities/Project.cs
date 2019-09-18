using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Entities.Entities
{
    public class Project
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Created { get; set; }
        public string Subject { get; set; }
        public DateTime ExpirationDate { get; set; }


        public ICollection<Commentary> Comments { get; set; }
        public Content Content { get; set; }
        public ICollection<Post> Posts { get; set; }
    }
}
