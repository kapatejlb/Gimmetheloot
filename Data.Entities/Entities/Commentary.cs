using System;

namespace Data.Entities
{
    public class Commentary
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public string UserId { get; set; }
        public AspNetUsers aspNetUser {get; set;}
        public string UserName { get; set; }
    }
}
