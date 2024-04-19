using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeMangement.Models
{
    public class SQLEmployeeRepository : IEmployeeRepository
    {
        //use this constructor to injuct appdbcontext in this class , by take instance form AppDbContext
        private AppDbContext _context;
        private readonly ILogger<SQLEmployeeRepository> logger;

        public SQLEmployeeRepository(AppDbContext context,
            ILogger<SQLEmployeeRepository> logger)
        {
            _context = context;
            this.logger = logger;
        }
        public Employee Add(Employee employee)
        {
            _context.Eployees.Add(employee);
            _context.SaveChanges();
            return employee;
        }

        public Employee Delete(int id)
        {
           Employee employee= _context.Eployees.Find(id);
            if(employee != null)
            {
                _context.Eployees.Remove(employee);
                _context.SaveChanges();
            }
            return employee;
        }

        public IEnumerable<Employee> GetAllEmployee()
        {
            return _context.Eployees;
        }

        public Employee GetEmployee(int Id)
        {
            logger.LogTrace("Trace log");
            logger.LogInformation("Information log");
            logger.LogDebug("Debug log");
            logger.LogWarning("Warning log");
            logger.LogError("error log");
            logger.LogCritical("Critical log");
            return  _context.Eployees.Find(Id);
           
        }

        public Employee Update(Employee employeeChanges)
        {
          var employee=  _context.Eployees.Attach(employeeChanges);
            employee.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return employeeChanges;
        }
    }
}
