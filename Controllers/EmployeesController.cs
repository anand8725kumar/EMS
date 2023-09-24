using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update.Internal;
using NormalForm.Data;
using NormalForm.Models;
using NormalForm.Models.Doman;

namespace NormalForm.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly DemoDbContext demoDbContext;

        public EmployeesController(DemoDbContext demoDbContext)
        {
            this.demoDbContext = demoDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var employees = await demoDbContext.Employees.ToListAsync();
            return View(employees);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeeViewModle addEmployeeRequest)
        {
            var employee = new Employee()
            {
                Id = Guid.NewGuid(),
                Name = addEmployeeRequest.Name,
                Email = addEmployeeRequest.Email,
                Salary = addEmployeeRequest.Salary,
                Department = addEmployeeRequest.Department,
                DateOfBirth = addEmployeeRequest.DateOfBirth
            };

            await demoDbContext.Employees.AddAsync(employee);
            await demoDbContext.SaveChangesAsync();
            return View();
        }



        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
            var employee = await demoDbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);

            if (employee == null)
            {
                var viewModel = new UpdateEmployeeViewModle()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Email = employee.Email,
                    Salary = employee.Salary,
                    Department = employee.Department,
                    DateOfBirth = employee.DateOfBirth
                };
                return await Task.Run(() => View("view",viewModel));
            }


            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> View(UpdateEmployeeViewModle model)
        {
            var employee = await demoDbContext.Employees.FindAsync(model.Id);

            if (employee == null)
            {
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Salary = model.Salary;
                employee.DateOfBirth = model.DateOfBirth;
                employee.Department = model.Department;

                await demoDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult>Delete(UpdateEmployeeViewModle model)
        {
            var employee = await demoDbContext.Employees.FindAsync(model.Id);
            if (employee != null)
            {
                demoDbContext.Employees.Remove(employee);
                await demoDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

    }
}
