using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpMe.Models
{
    public class ApplicationRoleManager: RoleManager<RoleViewModel>
    {
        public ApplicationRoleManager(RoleStore<RoleViewModel> store)
                : base(store)
        { }
        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options,
                                                IOwinContext context)
        {
            return new ApplicationRoleManager(new
                    RoleStore<RoleViewModel>(context.Get<ApplicationDbContext>()));
        }

    }
}