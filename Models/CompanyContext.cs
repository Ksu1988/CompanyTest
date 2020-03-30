namespace Company
{
    using Company.Models;
    using System;
    using System.Data.Common;
    using System.Data.Entity;
    using System.Linq;
   

    public class CompanyContext : DbContext
    {
      
        public CompanyContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            Configure();
        }
        public CompanyContext()
            : base("CompanyContext")
        {
            
        }

        public CompanyContext(DbConnection connection, bool contextOwnsConnection)
            : base(connection, contextOwnsConnection)
        {
            Configure();
        }

        private void Configure()
        {
            Configuration.ProxyCreationEnabled = true;
            Configuration.LazyLoadingEnabled = true;
        }

        // Добавьте DbSet для каждого типа сущности, который требуется включить в модель. Дополнительные сведения 
        // о настройке и использовании модели Code First см. в статье http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<Person> People { get; set; }

        public virtual DbSet<SalaryParameters> Salaries { get; set; }

        public virtual DbSet<TypeOfEmployee> TypesOfEmployee { get; set; }
    }

   
}