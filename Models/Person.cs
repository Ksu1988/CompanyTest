using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Prism.Mvvm;

namespace Company.Models
{
    public class Person : BindableBase
    {
        public int Id { get; set; }

        public TypeOfEmployee TypeOfEmployee { get; set; }

        private string _firstName { get; set; }
        public string FirstName
        {
            get { return _firstName; }
            set
            {

                _firstName = value;
                RaisePropertyChanged("FirstName");
            }
        }

        private string _currentPosition { get; set; }
        public string CurrentPosition
        {
            get { return _currentPosition; }
            set
            {
                _currentPosition = value;
                RaisePropertyChanged("CurrentPosition");
            }
        }

        private string _lastName { get; set; }
        [MaxLength(50)]
        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                RaisePropertyChanged("LastName");
            }
        }

        private DateTime _dateOfEmployment { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateOfEmployment
        {
            get { return _dateOfEmployment; }
            set
            {
                _dateOfEmployment = value;
                RaisePropertyChanged("DateOfEmployment");
            }
        }

        private double _salary { get; set; }
        public virtual double Salary
        {
            get { return _salary; }
        }

        public virtual void SetSalary(DateTime date)
        {
            var parameters = this.TypeOfEmployee.SalaryParameters;
            _salary = parameters.BaseRate;
            RaisePropertyChanged("Salary");
        }

        public Person Boss { get; set; }

        public List<Person> Subordinates { get; set; }

    }
}
