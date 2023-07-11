using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Data.Context;
using TMS.Models.Entities;
using TMS.Models.Enums;

namespace TMS.Data.DatabaseSeeder
{
    public static class DatabaseDataSeeder
    {
        public static void SeedData(this IApplicationBuilder app)
        {
            ApplicationDbContext context = app.ApplicationServices.CreateScope().ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

            context.Database.EnsureCreated();
            var roleExist = context.Roles.Any();

            if (!roleExist)
            {
                context.Roles.AddRange(SeededRoles());
                context.SaveChanges();
            }
        }

        private static IEnumerable<ApplicationRole> SeededRoles()
        {
            return new List<ApplicationRole>()
            {
                new ApplicationRole()
                {
                    Name = UserTypeExtension.GetStringValue(UserType.User),
                    Type = UserType.User,
                    NormalizedName = UserTypeExtension.GetStringValue(UserType.User)?.ToUpper()
                                                      .Normalize()
                },
                new ApplicationRole()
                {
                    Name = UserTypeExtension.GetStringValue(UserType.Admin),
                    Type = UserType.Admin,
                    NormalizedName = UserTypeExtension.GetStringValue(UserType.Admin)?.ToUpper()
                                                      .Normalize()
                },
                new ApplicationRole
                {
                    Name = UserTypeExtension.GetStringValue(UserType.SuperAdmin),
                    Type = UserType.SuperAdmin,
                    NormalizedName = UserTypeExtension.GetStringValue(UserType.SuperAdmin)?.ToUpper()
                                                      .Normalize()
                }
            };
        }

    }
}
