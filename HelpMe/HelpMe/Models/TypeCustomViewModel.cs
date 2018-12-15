using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HelpMe.Models
{
    /// <summary>
    /// Тип задачи
    /// </summary>
    public class TypeCustomViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public string ImagePath { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }

        public ICollection<CustomViewModel> Customs { get; set; }

        public TypeCustomViewModel()
        {
            Customs = new List<CustomViewModel>();
        }
    }
}