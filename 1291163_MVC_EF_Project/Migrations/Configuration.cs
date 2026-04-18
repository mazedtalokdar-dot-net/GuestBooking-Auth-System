namespace _1291163_MVC_EF_Project.Migrations
{
    using _1291163_MVC_EF_Project.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<_1291163_MVC_EF_Project.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(_1291163_MVC_EF_Project.Models.ApplicationDbContext context)
        {
            var roomTypes = new List<RoomType>
            {
                new RoomType { TypeName = "Single" },
                new RoomType { TypeName = "Double" },
                new RoomType { TypeName = "Suite" },
                new RoomType { TypeName = "Penthouse" }
            };

            foreach (var rt in roomTypes)
            {
                if (!context.RoomTypes.Any(r => r.TypeName == rt.TypeName))
                {
                    context.RoomTypes.Add(rt);
                }
            }

            // 2. Seed Room Categories
            var roomCategories = new List<RoomCategory>
            {
                new RoomCategory { CategoryName = "Standard" },
                new RoomCategory { CategoryName = "Deluxe" },
                new RoomCategory { CategoryName = "Executive" },
                new RoomCategory { CategoryName = "VIP" }
            };

            foreach (var rc in roomCategories)
            {
                if (!context.RoomCategories.Any(c => c.CategoryName == rc.CategoryName))
                {
                    context.RoomCategories.Add(rc);
                }
            }

            context.SaveChanges();
        }
    }
}
