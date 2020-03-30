using System;
using System.Collections.ObjectModel;
using System.Windows;
using Prism.Commands;
using Prism.Mvvm;
using Company.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.Data;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Company
{
    public class ViewModel : BindableBase
    {
        //private CompanyContext db;
        private DelegateCommand addCommand;

        private DelegateCommand getSubordinatesCommand;
        public DelegateCommand OkCommand { get; }

        private DelegateCommand editCommand;
        private DelegateCommand deleteCommand;

        private ObservableCollection<Person> _people = new ObservableCollection<Person>();

        public ReadOnlyObservableCollection<Person> People { get; set; }

        public ReadOnlyObservableCollection<Person> Subordinates { get; set; }

        public ObservableCollection<TypeOfEmployee> Positions { get; set; }

        public ObservableCollection<SalaryParameters> SalaryParameters { get; set; }

        private string dbFileName = @"db\companyDb.db";
        private SQLiteConnection m_dbConn;
        private SQLiteCommand m_sqlCmd;

        public ViewModel()
        {
            try
            {
                //заполнить Salary
                CreateSalary();
                //заполнить Position
                CreatePosition();
                //Загрузить данные базы в DataGrid
                m_dbConn = new SQLiteConnection();
                m_sqlCmd = new SQLiteCommand();
                People = new ReadOnlyObservableCollection<Person>(_people);
                DateOfEmpl = new DateTime(2000, 01, 01);
                CreatePeople();
                _culcDate = DateTime.Now.Date;
                 CompanyContext db = new CompanyContext();
                var t = db.People.Local;
                // var per = new Employee
                // {
                //     Id = 3,
                //     FirstName = "Петров",
                //     LastName = "Вася",
                //     //TypeOfEmployeeId = 1,
                //     DateOfEmployment = new DateTime(2002, 01, 01)

                // };
                // _people.Add(per);

                /*
                var per2 = new Salesman
                {
                    Id = 4,
                    FirstName = "ИвановSal",
                    LastName = "Вася",
                    //TypeOfEmployeeId = 3,
                    DateOfEmployment = new DateTime(2002, 04, 01)

                };
                _people.Add(per2);
                var pi1 = new Manager
                {
                    Id = 2,
                    FirstName = "МашаMan",
                    LastName = "Иванова",
                    DateOfEmployment = new DateTime(2000, 01, 01),
                    // TypeOfEmployeeId = 2,
                    Boss = per2
                };
                _people.Add(pi1);
                var pi = new Employee
                {
                    Id = 1,
                    FirstName = "Муся",
                    LastName = "Муся",
                    DateOfEmployment = new DateTime(2002, 01, 01),
                    //  TypeOfEmployeeId = 1,
                    Boss = pi1
                };
                _people.Add(pi);


                // db = new CompanyContext();
                var per = new Employee
                {
                    Id = 3,
                    FirstName = "Петров",
                    LastName = "Вася",
                    //TypeOfEmployeeId = 1,
                    DateOfEmployment = new DateTime(2002, 01, 01)

                };
                _people.Add(per);
                    */

                OkCommand = new DelegateCommand(() =>
                {
                    Person p = null;

                });

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void CreatePeople()
        {
            DataTable dTable = new DataTable();
            String sqlQuery;
            SQLiteFactory factory = (SQLiteFactory)System.Data.Common.DbProviderFactories.GetFactory("System.Data.SQLite");
            using (SQLiteConnection connection = (SQLiteConnection)factory.CreateConnection())
            {
                connection.ConnectionString = "Data Source = " + dbFileName;
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        MessageBox.Show("Open connection with database");
                        return;
                    }

                    try
                    {
                        sqlQuery = "SELECT * FROM People";
                        SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlQuery, connection);
                        adapter.Fill(dTable);
                        if (dTable.Rows.Count > 0)
                        {
                            for (int i = 0; i < dTable.Rows.Count; i++)
                            {
                                var row = dTable.Rows[i].ItemArray;
                                Person p = null;
                                var typeofEmployeeId = Convert.ToInt32(row[1]);
                                switch (typeofEmployeeId)
                                {
                                    case (1):

                                        p = new Employee
                                        {
                                            Id = Convert.ToInt32(row[0]),
                                            TypeOfEmployee = Positions.Where(t => t.TypeOfEmployeeId == Convert.ToInt32(row[1])).FirstOrDefault(),
                                            FirstName = Convert.ToString(row[2]),
                                            LastName = Convert.ToString(row[3]),
                                            DateOfEmployment = Convert.ToDateTime(row[4])
                                        };
                                        break;
                                    case (2):
                                        p = new Manager
                                        {
                                            Id = Convert.ToInt32(row[0]),
                                            TypeOfEmployee = Positions.Where(t => t.TypeOfEmployeeId == Convert.ToInt32(row[1])).FirstOrDefault(),
                                            FirstName = Convert.ToString(row[2]),
                                            LastName = Convert.ToString(row[3]),
                                            DateOfEmployment = Convert.ToDateTime(row[4])
                                        };
                                        break;
                                    case (3):
                                        p = new Salesman
                                        {
                                            Id = Convert.ToInt32(row[0]),
                                            TypeOfEmployee = Positions.Where(t => t.TypeOfEmployeeId == Convert.ToInt32(row[1])).FirstOrDefault(),
                                            FirstName = Convert.ToString(row[2]),
                                            LastName = Convert.ToString(row[3]),
                                            DateOfEmployment = Convert.ToDateTime(row[4])
                                        };
                                        break;
                                }

                                _people.Add(p);
                            }
                        }
                        else
                            MessageBox.Show("Database is empty");
                    }
                    catch (SQLiteException ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }

            }
        }

        private void CreateSalary()
        {
            SalaryParameters = new ObservableCollection<SalaryParameters>();

            //Заполняем коллекцию из базы
            SQLiteFactory factory = (SQLiteFactory)System.Data.Common.DbProviderFactories.GetFactory("System.Data.SQLite");
            using (SQLiteConnection connection = (SQLiteConnection)factory.CreateConnection())
            {
                connection.ConnectionString = "Data Source = " + dbFileName;
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    //command.CommandText = @"CREATE TABLE [workers] (
                    //[id] integer PRIMARY KEY AUTOINCREMENT NOT NULL,
                    //[name] char(100) NOT NULL,
                    //[family] char(100) NOT NULL,
                    //[age] int NOT NULL,
                    //[profession] char(100) NOT NULL
                    //);";
                    //command.CommandType = CommandType.Text;
                    //command.ExecuteNonQuery();
                    DataTable dTable = new DataTable();
                    String sqlQuery;

                    if (connection.State != ConnectionState.Open)
                    {
                        MessageBox.Show("Open connection with database");
                        return;
                    }

                    try
                    {
                        sqlQuery = "SELECT * FROM SalaryParameters";
                        SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlQuery, connection);
                        adapter.Fill(dTable);
                        if (dTable.Rows.Count > 0)
                        {
                            //dgvViewer.Rows.Clear();

                            for (int i = 0; i < dTable.Rows.Count; i++)
                            {
                                var row = dTable.Rows[i].ItemArray;
                                var sp = new SalaryParameters
                                {
                                    SalaryParametersId = Convert.ToInt32(row[0]),
                                    BaseRate = Convert.ToDouble(row[1]),
                                    Maxallowance = Convert.ToDouble(row[2]),
                                    SeniorityAllowance = Convert.ToDouble(row[3]),
                                    AllowanceForSubordinates = (row[4] != Convert.DBNull) ? Convert.ToDouble(row[4]) : 0,
                                    AllowanceForSubordinatesAll = (row[5] != Convert.DBNull) ? Convert.ToDouble(row[5]) : 0
                                };
                                SalaryParameters.Add(sp);
                            }
                        }
                        else
                            MessageBox.Show("Database is empty");
                    }
                    catch (SQLiteException ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }

                }
            }

            //SalaryParameters.Add(new SalaryParameters
            //{
            //    SalaryParametersId = 1,

            //    BaseRate = 20000,
            //    Maxallowance = 30,
            //    AllowanceForSubordinates = 0,
            //    AllowanceForSubordinatesAll = 0,
            //    SeniorityAllowance = 3
            //});
            //SalaryParameters.Add(new SalaryParameters
            //{
            //    SalaryParametersId = 2,

            //    BaseRate = 20000,
            //    Maxallowance = 40,
            //    AllowanceForSubordinates = 0.5,
            //    AllowanceForSubordinatesAll = 0,
            //    SeniorityAllowance = 5
            //});
            //SalaryParameters.Add(new SalaryParameters
            //{
            //    SalaryParametersId = 3,

            //    BaseRate = 20000,
            //    Maxallowance = 34,
            //    AllowanceForSubordinates = 0,
            //    AllowanceForSubordinatesAll = 0.3,
            //    SeniorityAllowance = 1
            //});
        }

        private void CreatePosition()
        {
            Positions = new ObservableCollection<TypeOfEmployee>();
            DataTable dTable = new DataTable();
            String sqlQuery;
            SQLiteFactory factory = (SQLiteFactory)System.Data.Common.DbProviderFactories.GetFactory("System.Data.SQLite");
            using (SQLiteConnection connection = (SQLiteConnection)factory.CreateConnection())
            {
                connection.ConnectionString = "Data Source = " + dbFileName;
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        MessageBox.Show("Open connection with database");
                        return;
                    }

                    try
                    {
                        sqlQuery = "SELECT * FROM TypeOfEmployee";
                        SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlQuery, connection);
                        adapter.Fill(dTable);
                        if (dTable.Rows.Count > 0)
                        {
                            for (int i = 0; i < dTable.Rows.Count; i++)
                            {
                                var row = dTable.Rows[i].ItemArray;
                                var te = new TypeOfEmployee
                                {
                                    TypeOfEmployeeId = Convert.ToInt32(row[0]),
                                    NameOfEmployee = Convert.ToString(row[1]),
                                    SalaryParameters = SalaryParameters.Where(sp => sp.SalaryParametersId == Convert.ToInt32(row[2])).FirstOrDefault()
                                };
                                Positions.Add(te);
                            }
                        }
                        else
                            MessageBox.Show("Database is empty");
                    }
                    catch (SQLiteException ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
            //Positions.Add(new TypeOfEmployee { TypeOfEmployeeId = 2, NameOfEmployee = "Менеджер" });
            //Positions.Add(new TypeOfEmployee { TypeOfEmployeeId = 1, NameOfEmployee = "Работник" });
            //Positions.Add(new TypeOfEmployee { TypeOfEmployeeId = 3, NameOfEmployee = "Продавец" });
            /* Выборка данных
 string sql = "SELECT Id, Name FROM Table1";
             command = new SQLiteCommand(sql, conn);
             SQLiteDataReader reader = command.ExecuteReader();
             while (reader.Read())
                 MessageBox.Show("Id: " + reader["Id"] + " Name: " + reader["Name"]);
                 */
        }

        private string _currentPosition;

        public string CurrentPosition
        {
            get { return _currentPosition; }
            set
            {
                _currentPosition = value;
            }
        }

        private string _personName;
        public string PersonName
        {
            get { return _personName; }
            set
            {
                _personName = value;
            }
        }

        private DateTime _dateOfEmpl;
        /// <summary>
        /// Дата поступления на работу.
        /// </summary>
        public DateTime DateOfEmpl
        {
            get { return _dateOfEmpl; }
            set
            {
                _dateOfEmpl = value;
            }
        }


        private DateTime _culcDate;

        /// <summary>
        /// Дата расчета.
        /// </summary>
        public DateTime CulcDate
        {
            get { return _culcDate; }
            set
            {
                _culcDate = value;
            }
        }

        public Person SelectedPerson {get;set;}

        /// команда добавления
        public DelegateCommand AddCommand
        {
            get
            {
                return addCommand ??
                  (addCommand = new DelegateCommand(() =>
                  {
                      AddWindow phoneWindow = new AddWindow(new Person() { DateOfEmployment = new DateTime(2010,1,1)});
                      if (phoneWindow.ShowDialog() == true)
                      {
                          Person person = phoneWindow.Person;
                          Person p = null;
                          person.CurrentPosition = "Employee";
                          switch (person.CurrentPosition)
                          {
                              case ("Employee"):

                                  p = new Employee
                                  {
                                      Id = 1,
                                      FirstName = person.FirstName,
                                      LastName = person.LastName,
                                      DateOfEmployment = person.DateOfEmployment,
                                      CurrentPosition = person.CurrentPosition,
                                      TypeOfEmployee = Positions.Where(pos => pos.NameOfEmployee == person.CurrentPosition).FirstOrDefault()
                                  };
                                  break;
                              case ("Manager"):

                                  p = new Manager
                                  {
                                      Id = 2,
                                      FirstName = person.FirstName,
                                      LastName = person.LastName,
                                      DateOfEmployment = person.DateOfEmployment,
                                      CurrentPosition = person.CurrentPosition,
                                      TypeOfEmployee = Positions.Where(pos => pos.NameOfEmployee == person.CurrentPosition).FirstOrDefault()
                                  };
                                  break;
                              case ("Salesman"):

                                  p = new Salesman
                                  {
                                      Id = 3,
                                      FirstName = person.FirstName,
                                      LastName = person.LastName,
                                      DateOfEmployment = person.DateOfEmployment,
                                      CurrentPosition = person.CurrentPosition,
                                      TypeOfEmployee = Positions.Where(pos => pos.NameOfEmployee == person.CurrentPosition).FirstOrDefault()
                                  };
                                  break;

                          }
                          _people.Add(p);
                          RaisePropertyChanged("People");
                          //Добавить в базу
                          SQLiteFactory factory = (SQLiteFactory)System.Data.Common.DbProviderFactories.GetFactory("System.Data.SQLite");
                          using (SQLiteConnection connection = (SQLiteConnection)factory.CreateConnection())
                          {
                              connection.ConnectionString = "Data Source = " + dbFileName;
                              connection.Open();

                              using (SQLiteCommand command = new SQLiteCommand(connection))
                              {
                                  if (connection.State != ConnectionState.Open)
                                  {
                                      MessageBox.Show("Open connection with database");
                                      return;
                                  }
                                  try
                                  {
                                      command.CommandText = "INSERT INTO People ('TypeOfEmployeeId', 'FirstName','LastName','DateOfEmployment') values ('" +
                                          p.TypeOfEmployee.TypeOfEmployeeId + "' , '" +
                                          p.FirstName + "' , '" +
                                          p.LastName + "' , '" +
                                          p.DateOfEmployment + "')";

                                      command.ExecuteNonQuery();
                                  }
                                  catch (SQLiteException ex)
                                  {
                                      MessageBox.Show("Error: " + ex.Message);
                                  }

                              }
                          }
                      }
                  }));
            }
        }


        public DelegateCommand GetSubordinatesCommand
        {
            get
            {
                return getSubordinatesCommand ??
                  (getSubordinatesCommand = new DelegateCommand(() =>
                  {
                      AddWindow phoneWindow = new AddWindow(new Person());
                      if (phoneWindow.ShowDialog() == true)
                      {
                          Person person = phoneWindow.Person;
                          Person p = null;
                          person.CurrentPosition = "Employee";
                          switch (person.CurrentPosition)
                          {
                              case ("Employee"):

                                  p = new Employee
                                  {
                                      Id = 1,
                                      FirstName = person.FirstName,
                                      LastName = person.LastName,
                                      DateOfEmployment = person.DateOfEmployment,
                                      CurrentPosition = person.CurrentPosition,
                                      TypeOfEmployee = Positions.Where(pos => pos.NameOfEmployee == person.CurrentPosition).FirstOrDefault()
                                  };
                                  break;
                              case ("Manager"):

                                  p = new Manager
                                  {
                                      Id = 2,
                                      FirstName = person.FirstName,
                                      LastName = person.LastName,
                                      DateOfEmployment = person.DateOfEmployment,
                                      CurrentPosition = person.CurrentPosition,
                                      TypeOfEmployee = Positions.Where(pos => pos.NameOfEmployee == person.CurrentPosition).FirstOrDefault()
                                  };
                                  break;
                              case ("Salesman"):

                                  p = new Salesman
                                  {
                                      Id = 3,
                                      FirstName = person.FirstName,
                                      LastName = person.LastName,
                                      DateOfEmployment = person.DateOfEmployment,
                                      CurrentPosition = person.CurrentPosition,
                                      TypeOfEmployee = Positions.Where(pos => pos.NameOfEmployee == person.CurrentPosition).FirstOrDefault()
                                  };
                                  break;

                          }
                          _people.Add(p);
                          RaisePropertyChanged("People");
                          //Добавить в базу
                          SQLiteFactory factory = (SQLiteFactory)System.Data.Common.DbProviderFactories.GetFactory("System.Data.SQLite");
                          using (SQLiteConnection connection = (SQLiteConnection)factory.CreateConnection())
                          {
                              connection.ConnectionString = "Data Source = " + dbFileName;
                              connection.Open();

                              using (SQLiteCommand command = new SQLiteCommand(connection))
                              {
                                  if (connection.State != ConnectionState.Open)
                                  {
                                      MessageBox.Show("Open connection with database");
                                      return;
                                  }
                                  try
                                  {
                                      command.CommandText = "INSERT INTO People ('TypeOfEmployeeId', 'FirstName','LastName','DateOfEmployment') values ('" +
                                          p.TypeOfEmployee.TypeOfEmployeeId + "' , '" +
                                          p.FirstName + "' , '" +
                                          p.LastName + "' , '" +
                                          p.DateOfEmployment + "')";

                                      command.ExecuteNonQuery();
                                  }
                                  catch (SQLiteException ex)
                                  {
                                      MessageBox.Show("Error: " + ex.Message);
                                  }

                              }
                          }
                      }
                  }));
            }
        }

        private void GetSubordinates()
        {

        }

        private DelegateCommand _calculateSalary;
        public DelegateCommand CalculateSalary
        {
            get
            {
                return _calculateSalary ??
                  (_calculateSalary = new DelegateCommand(() =>
                  {

                      foreach (var p in _people)
                          p.SetSalary( CulcDate);
                      RaisePropertyChanged("People");

                  }));
            }
        }
        //// команда редактирования
        //public DelegateCommand EditCommand
        //{
        //    get
        //    {
        //        return editCommand ??
        //          (editCommand = new DelegateCommand((selectedItem) =>
        //          {
        //              if (selectedItem == null) return;
        //              // получаем выделенный объект
        //              Phone phone = selectedItem as Phone;

        //              Phone vm = new Phone()
        //              {
        //                  Id = phone.Id,
        //                  Company = phone.Company,
        //                  Price = phone.Price,
        //                  Title = phone.Title
        //              };
        //              PhoneWindow phoneWindow = new PhoneWindow(vm);


        //              if (phoneWindow.ShowDialog() == true)
        //              {
        //                  // получаем измененный объект
        //                  phone = db.Phones.Find(phoneWindow.Phone.Id);
        //                  if (phone != null)
        //                  {
        //                      phone.Company = phoneWindow.Phone.Company;
        //                      phone.Title = phoneWindow.Phone.Title;
        //                      phone.Price = phoneWindow.Phone.Price;
        //                      db.Entry(phone).State = EntityState.Modified;
        //                      db.SaveChanges();
        //                  }
        //              }
        //          }));
        //    }
        //}
        //// команда удаления
        //public DelegateCommand DeleteCommand
        //{
        //    get
        //    {
        //        return deleteCommand ??
        //          (deleteCommand = new DelegateCommand((selectedItem) =>
        //          {
        //              if (selectedItem == null) return;
        //              // получаем выделенный объект
        //              Phone phone = selectedItem as Phone;
        //              db.Phones.Remove(phone);
        //              db.SaveChanges();
        //          }));
        //    }
        //}


    }

}

