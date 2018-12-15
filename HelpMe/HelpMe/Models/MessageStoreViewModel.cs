using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HelpMe.Models
{
    public class MessageStoreViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime DateSend { get; set; }

        public string UserFromId { get; set; }
        public virtual User UserFrom { get; set; }

        public string UserToId { get; set; }
        [ForeignKey("UserToId")]
        public virtual User UserTo { get; set; }

        public class Comparer : IEqualityComparer<MessageStoreViewModel>
        {
            public bool Equals(MessageStoreViewModel x, MessageStoreViewModel y)
            {
                return x.UserToId == y.UserToId;
            }

            public int GetHashCode(MessageStoreViewModel obj)
            {
                unchecked
                {
                    int hash = 17;
                    hash = hash * 23 + (obj.UserToId ?? "").GetHashCode();
                    return hash;
                }
            }
        }

    }

}