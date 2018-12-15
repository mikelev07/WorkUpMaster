using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpMe.Models
{
    public class TaskCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<CustomViewModel> Categories { get; set; }

        public TaskCategory()
        {
            Categories = new List<CustomViewModel>();

        }
    }
}