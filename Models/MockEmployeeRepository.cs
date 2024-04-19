using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeMangement.Models
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        private List<Employee> _employeelist;
        public MockEmployeeRepository()
        {
            _employeelist = new List<Employee>()
            {
                new Employee() {Id=1,Name="mahmoud",Email="mahmoudkhaled2022x",Department=Dept.IT},
                new Employee() {Id=2,Name="khaled",Email="khaledkhaled2022x",Department=Dept.HR},
                new Employee() {Id=3,Name="salama",Email="salamakhaled2022x",Department=Dept.HR},
            };
        }

        public Employee Add(Employee employee)
        {
            employee.Id = _employeelist.Max(e => e.Id) + 1;
            _employeelist.Add(employee);
            return  employee;
        }

        public Employee Delete(int id)
        {
         Employee employee=   _employeelist.FirstOrDefault(e => e.Id == id);
            if(employee != null)
            {
                _employeelist.Remove(employee);
            }
            return employee;
        }

        public IEnumerable<Employee> GetAllEmployee()
        {
            return _employeelist;
        }

        public Employee GetEmployee(int Id)
        {
            return _employeelist.FirstOrDefault(e => e.Id == Id);
        }

        public Employee Update(Employee employeeChanges)
        {
            Employee employee = _employeelist.FirstOrDefault(e => e.Id == employeeChanges.Id);
            if (employee != null)
            {
                employee.Name = employeeChanges.Name;
                employee.Email = employeeChanges.Email;
                employee.Department = employeeChanges.Department;
            }
            return employee;
        }
    }
}
