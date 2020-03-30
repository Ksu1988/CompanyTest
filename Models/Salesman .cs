using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Models
{
    public class Salesman : Person
    {
        private double _salary;
        public override double Salary
        {
            get { return _salary; }
           
        }

        public override void SetSalary(DateTime date)
        {
            var parameters = this.TypeOfEmployee.SalaryParameters;
            var allDay = (date - DateOfEmployment).TotalDays / 365;
            var seniority = (allDay * parameters.SeniorityAllowance < parameters.Maxallowance) ? (allDay * parameters.SeniorityAllowance) : parameters.Maxallowance;
            var s = parameters.BaseRate * (1 + seniority / 100);
            _salary = s;
            RaisePropertyChanged("Salary");
        }
    }
}
