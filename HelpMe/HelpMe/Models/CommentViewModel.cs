using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpMe.Models
{
    public class CommentViewModel
    { 
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Days { get; set; }
        public int OfferPrice { get; set; } 
        public int? CustomViewModelId { get; set; }
        public CustomViewModel CustomViewModel { get; set; }

        public string UserId { get; set; }

        public virtual User User { get; set; }
    }
}