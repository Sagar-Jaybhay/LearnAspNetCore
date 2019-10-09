using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnAspCore.Models
{
   public interface IStudentRepository
    {
        Student GetStudents(int StudentID);
        IEnumerable<Student> GetAllStudent();
        Student AddStudent(Student student);
        Student DeleteStudent(int studentID);
        Student UpdateStudent(Student student);
    }
}
