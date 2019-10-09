using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LearnAspCore.Models
{
    public class SQLStudentRepository:IStudentRepository
    {
        private OurDbContext context;
        private ILogger logger1;

        public SQLStudentRepository(OurDbContext context,ILogger<SQLStudentRepository> logger)
        {
            this.context = context;
            logger1 = logger;
        }
        public Student GetStudents(int StudentID)
        {
            return context.Students.Find(StudentID);
        }

        public IEnumerable<Student> GetAllStudent()
        {
            logger1.LogTrace("Trace");
            return context.Students;
        }

        public Student AddStudent(Student student)
        {
            context.Students.Add(student);
            context.SaveChanges();
            return student;
        }

        public Student DeleteStudent(int studentID)
        {
            var stud = context.Students.Find(studentID);
            if (stud != null)
            {
                context.Students.Remove(stud);
                context.SaveChanges();
            }
            return stud;
        }

        public Student UpdateStudent(Student student)
        {
            var stud = context.Students.Attach(student);
            stud.State = EntityState.Modified;
            context.SaveChanges();
            return student;
        }
    }
}
