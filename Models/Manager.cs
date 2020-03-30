using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Models
{
   public class Manager:Person
    {
        private double _salary;
        public override double Salary
        {
            get { return _salary; }
           
        }
                
        public override void SetSalary(DateTime date)
        {
            Subordinates = new List<Person>();// - todo^
            var parameters = this.TypeOfEmployee.SalaryParameters;
            
            var s = base.Salary;
            if (parameters.AllowanceForSubordinates > 0)
                foreach (var subordinate in Subordinates)
                    s += parameters.AllowanceForSubordinates.Value * subordinate.Salary / 100;
          
            _salary = s;
            RaisePropertyChanged("Salary");
        }
    }
}
