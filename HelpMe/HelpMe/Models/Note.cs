using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpMe.Models
{
    public class Note
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
    }
}