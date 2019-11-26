namespace NoteApp.API.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using NoteApp.API.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<NoteApp.API.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(NoteApp.API.Models.ApplicationDbContext context)
        {
            if (!context.Users.Any())
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);

                var user = new ApplicationUser
                {
                    UserName = "tahacan.atak@gmail.com",
                    Email = "tahacan.atak@gmail.com",
                    EmailConfirmed = true
                };

                userManager.Create(user, "Ankara1.");

                context.Notes.Add(new Note
                {
                    AuthorId = user.Id,
                    Title = "Sample Note 1",
                    Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam hendrerit rhoncus ipsum. Duis ac velit arcu. Maecenas velit eros, aliquam id turpis sit amet, posuere ...",
                    CreatedTime = DateTime.Now
                });

                context.Notes.Add(new Note
                {
                    AuthorId = user.Id,
                    Title = "Sample Note 2",
                    Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam hendrerit rhoncus ipsum. Duis ac velit arcu. Maecenas velit eros, aliquam id turpis sit amet, posuere ...",
                    CreatedTime = DateTime.Now.AddMinutes(10)
                });

            }

        }
    }
}
