﻿using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using Users.Infrastructure.DataBase.EntityFramework.Context;

namespace Users.Api.Extensions.Migration
{
    [ExcludeFromCodeCoverage]
    public static class MigrationExtensions
    {
        public static void ExecuteMigrations(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                db.Database.Migrate();
            }
        }
    }
}
