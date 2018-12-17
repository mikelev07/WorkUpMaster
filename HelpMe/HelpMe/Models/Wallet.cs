using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpMe.Models
{
    /// <summary>
    /// Класс личного денежного счета
    /// </summary>
    public class Wallet
    {
        public int Id { get; set; }
        public int Summ { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}