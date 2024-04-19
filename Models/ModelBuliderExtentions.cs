using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeMangement.Models
{
    public static class ModelBuliderExtentions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
          new Employee
          {
              Id = 2,
              Name = "khaled",
              Department = Dept.HR,
              Email = "khaled@gmail.com"
          },
          new Employee
          {
              Id = 3,
              Name = "salama",
              Department = Dept.HR,
              Email = "salama@gmail.com"
          }
               );
        
    }
    }
}
