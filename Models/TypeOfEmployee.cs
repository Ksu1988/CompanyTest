using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Models
{
   public class TypeOfEmployee
    {
        [Key]
        public int TypeOfEmployeeId { get; set; }

        public string NameOfEmployee { get; set; }

        public int SalaryParametersId;

        public SalaryParameters SalaryParameters { get; set;}
    }
}
