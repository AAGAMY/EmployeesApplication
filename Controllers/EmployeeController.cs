using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeesApp.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace EmployeesApp.Controllers
{
    public class EmployeeController : Controller
    {
        HRDatabaseContext dbContext = new HRDatabaseContext();
        public IActionResult Index(string SortField, string CurrentsortField, string SortDirection, string searchByName)
        {
            var employees = GetEmployees();
            if (!string.IsNullOrEmpty(searchByName))
                employees = employees.Where(a => a.EmployeeName.ToLower().Contains(searchByName.ToLower())).ToList();
                return View(this.SortEmployees(employees,SortField,CurrentsortField,SortDirection));
        }

        private List<Employee> GetEmployees()
        {
            var employees = (from employee in dbContext.Employees
                             join department in dbContext.Departments on
                             employee.Departmentid equals
                             department.DepartmentId
                             select new Employee
                             {

                                 EmployeeNumber = employee.EmployeeNumber,
                                 EmployeeName = employee.EmployeeName,
                                 DOB = employee.DOB,
                                 HiringDate = employee.HiringDate,
                                 GroosSalary = employee.GroosSalary,
                                 NetsSalary = employee.NetsSalary,
                                 Departmentid = employee.Departmentid,
                                 EmployeeId = employee.EmployeeId,
                                 DepartmentName = department.DepartmentName
                             }).ToList();

            return employees;
        }

        public IActionResult Create()
        {
            ViewBag.Departments = dbContext.Departments.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Employee emp)
        {
            //not need validation on this properies in emplpyee model
            ModelState.Remove("EmployeeId");
            ModelState.Remove("DepartmentName");
            ModelState.Remove("department");

            if (ModelState.IsValid)
            {
                dbContext.Employees.Add(emp);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Departments = this.dbContext.Departments.ToList();
            return View();
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            //Other way
            #region Other way
            //var r = (from i in dbContext.Employees where i.EmployeeId == id select i).ToList();
            //return View("Create", r);

            #endregion
            Employee data = this.dbContext.Employees.Where(a => a.EmployeeId == id).FirstOrDefault();
            ViewBag.Departments = this.dbContext.Departments.ToList();
            return View("Create", data);
        }

        [HttpPost]
        public IActionResult Edit(Employee model)
        {
            ModelState.Remove("EmployeeId");
            ModelState.Remove("DepartmentName");
            ModelState.Remove("department");


            if (ModelState.IsValid)
            {
                dbContext.Employees.Update(model);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Departments = this.dbContext.Departments.ToList();
            return View("Create", model);
        }

        public IActionResult Delete(int id)
        {
            Employee data = this.dbContext.Employees.Where(a => a.EmployeeId == id).FirstOrDefault();
            if(data != null)
            {
            dbContext.Employees.Remove(data);
            dbContext.SaveChanges();
               
            }
            return RedirectToAction("Index");


        }

        //Sorting Field
        #region Sorting Method
        private List<Employee> SortEmployees(List<Employee> employees, string sortField, string currentsortField, string sortDirection)
        {
            if (string.IsNullOrEmpty(sortField))
            {
                ViewBag.SortField = "EmployeeNumber";
                ViewBag.SortDirection = "Asc";
            }
            else
            {
                if (currentsortField == sortField)
                    ViewBag.SortDirection = sortDirection == "Asc" ? "Desc" : "Asc";
                else
                    ViewBag.SortDirection = "Asc";
                ViewBag.SortField = sortField;
            }

            var propertyInfo = typeof(Employee).GetProperty(ViewBag.SortField);
            if (ViewBag.SortDirection == "Asc")
                employees = employees.OrderBy(e => propertyInfo.GetValue(e, null)).ToList();
            else
                employees = employees.OrderByDescending(e => propertyInfo.GetValue(e, null)).ToList();
            return employees;

        }

        #endregion    
    }
 }
