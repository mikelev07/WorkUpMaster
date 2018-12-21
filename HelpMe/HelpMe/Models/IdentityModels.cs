using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace HelpMe.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }

    //public class UserProfile
    //{
    //    [Key]
    //    [ForeignKey("ApplicationUser")]
    //    public string Id { get; set; }
    //    public string Description { get; set; }
    //    public ApplicationUser User { get; set; }
    //}

    public class User : IdentityUser
    {
        public int Age { get; set; } // добавляем свойство Age
        public string Description { get; set; }
        public string ConnectionId { get; set; } // для SignalR
        //public UserProfile Profile { get; set; }
        
        public ICollection<CustomViewModel> Customs { get; set; }
        public ICollection<CommentViewModel> Comments { get; set; }
        public ICollection<MessageStoreViewModel> Messages { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Note> Notes { get; set; }
        public ICollection<Wallet> Wallets { get; set; } // создаем универсальную коллекцию
        public ICollection<AttachModel> Attachments { get; set; }

        public User()
        {
            Comments = new List<CommentViewModel>();
            Customs = new List<CustomViewModel>();
            Messages = new List<MessageStoreViewModel>();
            Reviews = new List<Review>();
            Notes = new List<Note>();
            Attachments = new List<AttachModel>();
            Wallets = new List<Wallet>(); // добавляем список кошельков
        }
      
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            // Обратите внимание, что authenticationType должен совпадать с типом, определенным в CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Здесь добавьте утверждения пользователя
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext()
            : base("DbConnection")
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasMany(m => m.Customs).WithRequired(m => m.Executor);
            modelBuilder.Entity<User>().HasMany(m => m.Messages).WithRequired(m => m.UserTo);
            modelBuilder.Entity<User>().HasMany(m => m.Reviews).WithRequired(m => m.Owner);
        }

        public DbSet<CustomViewModel> Customs { get; set; }
        public DbSet<AttachModel> Attachments { get; set; }
        public DbSet<CommentViewModel> Comments { get; set; }
        public DbSet<MessageStoreViewModel> Messages { get; set; }
        public DbSet<TypeCustomViewModel> CustomTypes { get; set; }
        public DbSet<TaskCategory> TaskCategories { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Wallet> Wallets { get; set; } // коллекция сущностей типа Wallet
        // public DbSet<UserProfile> UserProfiles { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<HelpMe.Models.RoleViewModel> IdentityRoles { get; set; }

        
       
    }
}