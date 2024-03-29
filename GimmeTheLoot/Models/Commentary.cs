﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GimmeTheLoot.Models
{
    public class Commentary
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; }

        public string AspNetUserId { get; set; }
        public AspNetUser AspNetUser { get; set; }

        public string UserName { get; set; }

        //public string UserId { get; set; }
        //public AspNetUsers aspNetUser { get; set; }
        //public string UserName { get; set; }
    }
}
