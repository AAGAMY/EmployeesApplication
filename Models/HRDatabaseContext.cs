using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EmployeesApp.Models;

namespace EmployeesApp.Models
{
    public class HRDatabaseContext:DbContext
    {
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Provider=SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=Electronics;Data Source=DEVELOPMENT\SQLEXPRESS
            optionsBuilder.UseSqlServer(@"Data Source=DEVELOPMENT\SQLEXPRESS;Initial Catalog=EmployeesDB;Integrated Security=SSPI");
        }
    }
}
