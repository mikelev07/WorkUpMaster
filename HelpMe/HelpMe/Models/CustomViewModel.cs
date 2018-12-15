using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Web;

namespace HelpMe.Models
{
    /// <summary>
    /// класс заказов
    /// </summary>
    public class CustomViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public CustomStatus Status { get; set; }
        public string FilePath { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndingDate { get; set; }
        public int? TypeTaskId { get; set; }
        public TypeCustomViewModel TypeTask { get; set; }
        public int? CategoryTaskId { get; set; }
        public TaskCategory CategoryTask { get; set; }
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }
        public string AttachFilePath { get; set; }

        [NotMapped]
        public HttpPostedFileBase AttachFile { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }

        public string ExecutorId { get; set; }
        [ForeignKey("ExecutorId")]
        public virtual User Executor { get; set; }

        public ICollection<CommentViewModel> Comments { get; set; }

        [NotMapped]
        public TimeSpan TotalHrs
        {
             get
            { 
                return EndingDate - DateTime.Now;
            }
        }

        public CustomViewModel()
        {
            Comments = new List<CommentViewModel>();
        } 
    }

    public class PageInfo
    {
        public int PageNumber { get; set; } // номер текущей страницы
        public int PageSize { get; set; } // кол-во объектов на странице
        public int TotalItems { get; set; } // всего объектов
        public int TotalPages  // всего страниц
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / PageSize); }
        }

        public bool HasPreviousPage { get { return PageNumber > 1 && TotalPages > 1; } }
        public int PreviousPageIndex { get { return PageNumber - 1; } }
        public bool HasNextPage { get { return PageNumber < TotalPages && TotalPages > 1; } }
        public int NextPageIndex { get { return PageNumber + 1; } }
    }
    public class CustomIndexViewModel
    {
        public IEnumerable<CustomViewModel> Customs { get; set; }
        public PageInfo PageInfo { get; set; }
    }

    public enum CustomStatus
    {
        [Display(Name = "Открытая заявка")]
        Open,
        [Display(Name = "Закрытая заявка")]
        Close,
        [Display(Name = "В разработке")]
        Progress,
        [Display(Name = "After Accepting Quote")]
        Check
    }

  

}