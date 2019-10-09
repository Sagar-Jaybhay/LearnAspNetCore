using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LearnAspCore.Models
{
    public static class ModelBuilderSeeding
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().HasData(
                new Student
                {
                    StudentId = 1,
                    Division = Divi.A_10,
                    Address = "abcd efgh",
                    FullName = "abc"
                },
                new Student
                {
                    StudentId = 2,
                    Division = Divi.A_10,
                    Address = "werr",
                    FullName = "xyz"
                }
            );
        }
    }
}
