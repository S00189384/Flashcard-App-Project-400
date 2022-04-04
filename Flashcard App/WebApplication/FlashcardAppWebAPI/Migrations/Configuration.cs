﻿namespace FlashcardAppWebAPI.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Models;

    internal sealed class Configuration : DbMigrationsConfiguration<FlashcardAppWebAPI.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(FlashcardAppWebAPI.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.

            //var user = new ApplicationUser() { UserName = "shaneb", Email = "shaneb214@gmail.com",Id = "e0197dde - 6f60 - 4e2f - 90a5 - fd0bb2644d34" };

            //IdentityResult result = await UserManager.

            //LANGUAGES

            //context.Languages.AddOrUpdate
            //(
            //    new Language() { ISO = "en", Name = "English" },
            //    new Language() { ISO = "ru", Name = "Russian" },
            //    new Language() { ISO = "it", Name = "Italian" },
            //    new Language() { ISO = "ja", Name = "Japanese" }
            //);
            //context.SaveChanges();

            //LANG PROFILE

            //context.LanguageProfiles.AddOrUpdate(new LanguageProfile()
            //{
            //    ID = Guid.NewGuid().ToString(),
            //    userID = "0ae94ef8-ecff-4d6a-a030-f2b573a797fa",
            //    nativeLanguageISO = "en",
            //    learningLanguageISO = "ru",
            //    IsCurrentProfile = true
            //});

            //context.SaveChanges();

            //SETS.

            //context.LanguageProfiles.Find("9b18fbf4-b158-4fa5-a6c7-860163c87863"). AddOrUpdate(new LanguageProfile()
            //{
            //    ID = Guid.NewGuid().ToString(),
            //    userID = "0ae94ef8-ecff-4d6a-a030-f2b573a797fa",
            //    nativeLanguageISO = "en",
            //    learningLanguageISO = "ru",
            //    IsCurrentProfile = true
            //});

        }
    }
}
