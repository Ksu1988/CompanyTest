using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Company.Models
{
    public class SalaryParameters
    {
        [Key,Required]
        public int SalaryParametersId;
              
        /// <summary>
        /// Базовая ставка
        /// </summary>
        public double BaseRate { get; set; }

        /// <summary>
        /// Максимальное значение надбавки в процентах
        /// </summary>
        public double Maxallowance { get; set; }

        //% за каждый год работы в компании, но не больше 35% суммарной надбавки за стаж работы.Плюс 0,3% зарплаты всех подчинённых всех уровней.

        /// <summary>
        /// Процент надбавки за стаж
        /// </summary>
        public double SeniorityAllowance { get; set; }

        // <summary>
        /// Процент от зарплаты подчиненных всех уровней
        /// </summary>
        public double? AllowanceForSubordinatesAll { get; set; }

        // <summary>
        /// Процент от зарплаты подчиненных первого уровня
        /// </summary>
        public double? AllowanceForSubordinates { get; set; }
    }
}
