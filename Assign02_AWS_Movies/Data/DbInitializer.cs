using Assign02_AWS_Movies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assign02_AWS_Movies.Data
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            //Ckecking whether exist records in the table
            if (context.Category.Any())
            {
                return;
            }

            //*
            // If there is no record the table is initialized
            // 
            var Categories = new Category[]
            {
                new Category{ CategoryName ="Action"},
                new Category{ CategoryName ="Adult"},
                new Category{ CategoryName ="Adventure"},
                new Category{ CategoryName ="Animation"},
                new Category{ CategoryName ="Anime"},
                new Category{ CategoryName ="Biography"},
                new Category{ CategoryName ="Cartoon"},
                new Category{ CategoryName ="Comedy"},
                new Category{ CategoryName ="Comedy Drama"},
                new Category{ CategoryName ="Comedy-Romance"},
                new Category{ CategoryName ="Crime"},
                new Category{ CategoryName ="Documentary"},
                new Category{ CategoryName ="Drama"},
                new Category{ CategoryName ="Fantasy"},
                new Category{ CategoryName ="Horror"},
                new Category{ CategoryName ="Martial-Arts"},
                new Category{ CategoryName ="Musical"},
                new Category{ CategoryName ="Mystery"},
                new Category{ CategoryName ="Reality-TV"},
                new Category{ CategoryName ="Romance"},
                new Category{ CategoryName ="Scifi"},
                new Category{ CategoryName ="Sport"},
                new Category{ CategoryName ="Superhero"},
                new Category{ CategoryName ="Talk Show"},
                new Category{ CategoryName ="Thriller"},
                new Category{ CategoryName ="TV Series"},
                new Category{ CategoryName ="TV Shows"},
                new Category{ CategoryName ="War"},
                new Category{ CategoryName ="Western"}
            };

            foreach(Category c in Categories)
            {
                context.Category.Add(c);

            }

            context.SaveChanges();
        }
    }
}
