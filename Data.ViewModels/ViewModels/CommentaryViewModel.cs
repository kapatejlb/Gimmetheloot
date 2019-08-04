using System;
using System.Collections.Generic;
using System.Text;

namespace Data.ViewModels.ViewModels
{
    public class CommentaryViewModel
    {
        public int Id { get; set; }
        public string Text { get; set; }

        public int ProjectId { get; set; }
    }
}
