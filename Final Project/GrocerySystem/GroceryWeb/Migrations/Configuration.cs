namespace GroceryWeb.Migrations
{
    using GroceryWeb.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Security.Claims;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<GroceryWeb.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "GroceryWeb.Models.ApplicationDbContext";
        }

        protected override void Seed(GroceryWeb.Models.ApplicationDbContext context)
        {

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            // Add any existing roles
            context.Roles.AddOrUpdate(r => r.Name, new IdentityRole() { Name = "Manager" });
            context.Roles.AddOrUpdate(r => r.Name, new IdentityRole() { Name = "Supervisor" });
            context.Roles.AddOrUpdate(r => r.Name, new IdentityRole() { Name = "Carryout" });
            context.Roles.AddOrUpdate(r => r.Name, new IdentityRole() { Name = "Checkout" });
            context.Roles.AddOrUpdate(r => r.Name, new IdentityRole() { Name = "Trainer" });
            context.Roles.AddOrUpdate(r => r.Name, new IdentityRole() { Name = "Administrator" });
            context.Roles.AddOrUpdate(r => r.Name, new IdentityRole() { Name = "Customer" });

            // Create the admin user
            string admin = "admin@jwsfoods.com";
            string defaultPassword = "P@ssw0rd";

            if (!context.Users.Any(u => u.UserName.Equals(admin)))
            {
                var user = new ApplicationUser()
                {
                    UserName = admin,
                    Email = admin
                };

                IdentityResult result = userManager.Create(user, defaultPassword);
                context.SaveChanges();
                if (result.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Administrator");
                    context.SaveChanges();
                }
            }

            // Create the customer user
            string customer = "eric@gmail.com";

            if (!context.Users.Any(u => u.UserName.Equals(customer)))
            {
                var user = new ApplicationUser()
                {
                    UserName = customer,
                    Email = customer
                };

                IdentityResult result = userManager.Create(user, defaultPassword);
                context.SaveChanges();
                if (result.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Customer");
                    context.SaveChanges();
                }
            }

            // Create the employee user
            string employee = "jacobslaubaugh@jwsfoods.com";

            if (!context.Users.Any(u => u.UserName.Equals(employee)))
            {
                var user = new ApplicationUser()
                {
                    UserName = employee,
                    Email = employee
                };

                IdentityResult result = userManager.Create(user, defaultPassword);
                context.SaveChanges();
                if (result.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Manager");
                    context.SaveChanges();
                }
            }

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
