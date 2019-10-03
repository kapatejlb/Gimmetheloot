using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GimmeTheLoot.Models
{
    public class Content
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
