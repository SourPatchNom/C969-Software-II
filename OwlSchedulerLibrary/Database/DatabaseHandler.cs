using System;
using System.Collections.Generic;
using System.Configuration;
using MySql.Data.MySqlClient;
using OwlSchedulerLibrary.Classes;

namespace OwlSchedulerLibrary.Database
{
    public sealed class DatabaseHandler
    {
        private static readonly Lazy<DatabaseHandler> LazySingleton = new Lazy<DatabaseHandler>(() => new DatabaseHandler());

        public static DatabaseHandler Instance => LazySingleton.Value;

        private DatabaseHandler()
        {
            
            
        }

        private MySqlConnection _mySqlConnection = new MySqlConnection();
        private MySqlCommand _mySqlCommand = new MySqlCommand();
        private string _connectionString = "";
        private bool initialized = false;

        public Dictionary<int, Address> Addresses { get; private set; }
        public Dictionary<int, Appointment> Appointments { get; private set; }
        public Dictionary<int, City> Cities { get; private set; }
        public Dictionary<int, Country> Countries { get; private set; }
        public Dictionary<int, Customer> Customers { get; private set; }

        /// <summary>
        /// Made prior to configuration integration.
        /// </summary>
        /// <param name="connectionUrl"></param>
        /// <param name="connectionPort"></param>
        /// <param name="connectionUsername"></param>
        /// <param name="connectionPassword"></param>
        /// <param name="connectionDatabase"></param>
        /// <param name="connectionHost"></param>
        /// <returns></returns>
        private string buildConnectionString(string connectionUrl, string connectionPort, string connectionUsername, string connectionPassword, string connectionDatabase, string connectionHost)
        {
            return "server=" + connectionUrl + ";user=" + connectionUsername + ";database=" + connectionHost + ";port=" + connectionPort + ";password=" + connectionPassword;
        }

        /// <summary>
        /// Initializes the database handler, attempts to establish a database connection and fails if the connection fails. 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool Initialize(string connectionString, out string result)
        {
            _connectionString = connectionString;
            result = "DatabaseHandler Initialize ";
            try
            {
                _mySqlConnection.ConnectionString = _connectionString;
                _mySqlConnection.Open();
                if (_mySqlConnection.Ping())
                {
                    result += "Connection Test Successful.";    
                    LogHandler.Instance.LogMessage("DatabaseHandler","Initialize connection successful!");
                    _mySqlConnection.Close();
                    initialized = true;
                    return true;
                }
                result += "Connection Test Failed!";    
                LogHandler.Instance.LogMessage("DatabaseHandler","Initialize connection failed!");
                _mySqlConnection.Close();
                initialized = false;
                return false;
            }
            catch (Exception e)
            {
                result += "Critical Failure!\n"+e.Message;
                Console.WriteLine(e);
                initialized = false;
                return false;
            }
            
        }

        #region UserManagement

        

        #endregion
        
        public bool LoginUser(string username, string password)
        {
            if (initialized)
            {
                try
                {
                    _mySqlCommand = _mySqlConnection.CreateCommand();
                    _mySqlCommand.CommandText = "SELECT active FROM user WHERE username = @user AND password = @pass";
                    _mySqlCommand.Parameters.AddWithValue("@user", username);
                    _mySqlCommand.Parameters.AddWithValue("@pass", password);
                    _mySqlConnection.Open();
                    var result = _mySqlCommand.ExecuteScalar();
                    if (result != null)
                    {
                        int active = Convert.ToInt32(result);
                        if (active == 1)
                        {
                            _mySqlConnection.Close();

                            InsertAddress(new Address(-1, "1111", "Main Street", 8, "88888", "18888888888", DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
                            InsertAddress(new Address(-1, "1111", "Peach Highway", 9, "88888", "18888888888", DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
                            InsertAddress(new Address(-1, "1111", "Grape Lane", 10, "88888", "18888888888", DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
                            InsertAddress(new Address(-1, "1111", "Apple Street", 11, "88888", "18888888888", DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
                            InsertAddress(new Address(-1, "1111", "Orange Street", 12, "88888", "18888888888", DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
                            
                            return true;    
                        }
                    }
                    _mySqlConnection.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            return false;
        }
        
        #region InsertNewRecords

        public bool InsertAddress(Address newAddress)
        {
            _mySqlConnection.Open();
            //Address add = new Address(1, "test", "test", 1, "76244", "1-888-888-8888", DateTime.UtcNow, "TEST", DateTime.UtcNow, "TEST");
            _mySqlCommand = DatabaseQueries.GetInsertAddressCommand(_mySqlConnection.CreateCommand(), newAddress);
            var result = _mySqlCommand.ExecuteNonQuery();
            _mySqlConnection.Close();
            return result > 0;
        }

        public bool InsertCity(City newCity)
        {
            _mySqlConnection.Open();
            //Address add = new Address(1, "test", "test", 1, "76244", "1-888-888-8888", DateTime.UtcNow, "TEST", DateTime.UtcNow, "TEST");
            _mySqlCommand = DatabaseQueries.GetInsertCityCommand(_mySqlConnection.CreateCommand(), newCity);
            var result = _mySqlCommand.ExecuteNonQuery();
            _mySqlConnection.Close();
            return result > 0;
        }
        
        public bool InsertCountry(Country newCountry)
        {
            _mySqlConnection.Open();
            //Address add = new Address(1, "test", "test", 1, "76244", "1-888-888-8888", DateTime.UtcNow, "TEST", DateTime.UtcNow, "TEST");
            _mySqlCommand = DatabaseQueries.GetInsertCountryCommand(_mySqlConnection.CreateCommand(), newCountry);
            var result = _mySqlCommand.ExecuteNonQuery();
            _mySqlConnection.Close();
            return result > 0;
        }
        
        #endregion
    }
}