using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using MySql.Data.MySqlClient;
using OwlSchedulerLibrary.Classes;
using OwlSchedulerLibrary.Database.Classes;
using OwlSchedulerLibrary.OwlLogger;
using OwlSchedulerLibrary.Database.Extensions;

namespace OwlSchedulerLibrary.Database
{
    public sealed class DatabaseHandler
    {
        private static readonly Lazy<DatabaseHandler> LazySingleton = new Lazy<DatabaseHandler>(() => new DatabaseHandler());

        public static DatabaseHandler Instance => LazySingleton.Value;
        public event PropertyChangedEventHandler DatabaseInformationUpdated;
        
        private DatabaseHandler()
        {
            
            
        }

        private readonly MySqlConnection _mySqlConnection = new MySqlConnection();
        private MySqlCommand _mySqlCommand = new MySqlCommand();
        private string _connectionString = "";
        public bool Initialized { get; private set; }

        public Dictionary<int, Address> Addresses { get; private set; } = new Dictionary<int, Address>();
        public Dictionary<int, Appointment> Appointments { get; private set; } = new Dictionary<int, Appointment>();
        public Dictionary<int, City> Cities { get; private set; } = new Dictionary<int, City>();
        public Dictionary<int, Country> Countries { get; private set; } = new Dictionary<int, Country>();
        public Dictionary<int, Customer> Customers { get; private set; } = new Dictionary<int, Customer>();

        /// <summary>
        /// Manages opening a connection to the mysql server.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception">Throws if a connection is not open and/or a connection cannot be established</exception>
        private bool CheckOrOpenConnection()
        {
            try
            {
                //Is the connection open already?
                if (_mySqlConnection.State == ConnectionState.Open)
                {
                    //ping connection to verify health
                    if (_mySqlConnection.Ping())
                    {
                        return true;
                    }

                    //We know it is open but fails to rx so close if ping fails, we will attempt to re-open next.
                    _mySqlConnection.Close();
                }

                //Open a connection
                _mySqlConnection.Open();

                //Make sure it worked!
                if (_mySqlConnection.State == ConnectionState.Open)
                {
                    return true;
                }

                //It didn't work somethings wrong.
                throw new Exception("Mysql Open Failed!");
            }
            catch (MySqlException e)
            {
                throw new Exception("Mysql Open Failed!",e);
            }
        }

        private void CloseConnectionIfOpen()
        {
            try
            {
                if (_mySqlConnection.State == ConnectionState.Open)
                {
                    _mySqlConnection.Close();
                }
            }
            catch (MySqlException e)
            {
                throw new Exception("Mysql Close Failed!",e);
            }
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
                CheckOrOpenConnection();
                if (_mySqlConnection.Ping())
                {
                    result += "Connection Test Successful.";    
                    LogHandler.LogMessage("DatabaseHandler","Initialize connection successful!");
                    CloseConnectionIfOpen();
                    Initialized = true;
                    return true;
                }
                result += "Connection Test Failed!";
                LogHandler.LogMessage("DatabaseHandler","Initialize connection failed!");
                CloseConnectionIfOpen();
                Initialized = false;
                return false;
            }
            catch (Exception e)
            {
                result += "Critical Failure!\n"+e.Message;
                Console.WriteLine(e);
                Initialized = false;
                return false;
            }
            
        }

        #region UserManagement
        
        public bool LoginUser(string username, string password, out User currentUser)
        {
            if (Initialized)
            {
                try
                {
                    _mySqlCommand = _mySqlConnection.CreateCommand();
                    _mySqlCommand.CommandText = "SELECT * FROM user WHERE username = @user AND password = @pass";
                    _mySqlCommand.Parameters.AddWithValue("@user", username);
                    _mySqlCommand.Parameters.AddWithValue("@pass", password);
                    if (CheckOrOpenConnection())
                    {
                        var result = _mySqlCommand.ExecuteReader();
                        if (result.HasRows)
                        {
                            while (result.Read())
                            {
                                if (result.GetInt32(3) == 1)
                                {
                                    currentUser = new User(result.GetInt32(0), result.GetString(1),  result.GetInt32(3));
                                    CloseConnectionIfOpen();
                                    return true;
                                }
                                   
                            }
                        }
                    }
                    CloseConnectionIfOpen();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            currentUser = null;
            return false;
        }
        
        
        #endregion
        #region GetAndSyncFromDatabase

        public void SyncFromDatabase()
        {
            if (!Initialized) return;
            
            //TODO remove for release!
#if DEBUG
            LogHandler.LogMessage("DatabaseHandler","DEBUG! Attempting to clean and repopulate database!");
            DebugClearDatabase();
            DatabaseDebugTools.PopulateWithData();
#endif
            
            LogHandler.LogMessage("DatabaseHandler","Attempting to sync database!");
            RefreshCountries();
            RefreshCities();
            RefreshAddresses();
            RefreshCustomers();
            RefreshAppointments();
        }
        
        private void DebugClearDatabase()
        {
            if (!CheckOrOpenConnection()) return;
            _mySqlCommand = DatabaseDebugTools.GetDeleteAllCommand(_mySqlConnection.CreateCommand());
            var result = _mySqlCommand.ExecuteNonQuery();
            _mySqlCommand.Dispose();
            CloseConnectionIfOpen();
        }

        public void RefreshCountries()
        {
            Countries.Clear();
            if (!CheckOrOpenConnection()) throw new Exception("Connection Error");
            _mySqlCommand = _mySqlConnection.CreateCommand();
            _mySqlCommand.CommandText = "SELECT * FROM country";
            var mySqlDataReader = _mySqlCommand.ExecuteReader();
            while (mySqlDataReader.Read())
            {
                var newCountry = new Country(mySqlDataReader.GetInt32(0), mySqlDataReader.GetString(1),TimeZoneConvert.MySqlDateTimeFromUtc(mySqlDataReader.GetDateTime(2)), mySqlDataReader.GetString(3),TimeZoneConvert.MySqlDateTimeFromUtc(mySqlDataReader.GetDateTime(4)), mySqlDataReader.GetString(5));
                Countries.Add(newCountry.CountryId, newCountry);
            }
            LogHandler.LogMessage("DatabaseHandler","Total Countries " + Countries.Count);
            mySqlDataReader.Close();
            _mySqlCommand.Dispose();
            CloseConnectionIfOpen();
            DatabaseInformationUpdated?.Invoke(this, new PropertyChangedEventArgs("Countries Updated!"));
        }

        public void RefreshCities()
        {
            Cities.Clear();
            if (!CheckOrOpenConnection()) throw new Exception("Connection Error");
            _mySqlCommand = _mySqlConnection.CreateCommand();
            _mySqlCommand.CommandText = "SELECT * FROM city";
            var mySqlDataReader = _mySqlCommand.ExecuteReader();
            while (mySqlDataReader.Read())
            {
                var newCity = new City(
                    mySqlDataReader.GetInt32(0), mySqlDataReader.GetString(1), mySqlDataReader.GetInt32(2),TimeZoneConvert.MySqlDateTimeFromUtc(mySqlDataReader.GetDateTime(3)), mySqlDataReader.GetString(4), 
                   TimeZoneConvert.MySqlDateTimeFromUtc(mySqlDataReader.GetDateTime(5)),mySqlDataReader.GetString(6));
                Cities.Add(newCity.CityId, newCity);
            }
            LogHandler.LogMessage("DatabaseHandler","Total Cities " + Cities.Count);
            mySqlDataReader.Close();
            _mySqlCommand.Dispose();
            CloseConnectionIfOpen();
            DatabaseInformationUpdated?.Invoke(this, new PropertyChangedEventArgs("Cities Updated!"));
        }

        public void RefreshAddresses()
        {
            Addresses.Clear();
            if (!CheckOrOpenConnection()) throw new Exception("Connection Error");
            _mySqlCommand = _mySqlConnection.CreateCommand();
            _mySqlCommand.CommandText = "SELECT * FROM address";
            var mySqlDataReader = _mySqlCommand.ExecuteReader();
            while (mySqlDataReader.Read())
            {
                var newAddress = new Address(mySqlDataReader.GetInt32(0), mySqlDataReader.GetString(1), mySqlDataReader.GetString(2), mySqlDataReader.GetInt32(3), 
                    mySqlDataReader.GetString(4), mySqlDataReader.GetString(5),mySqlDataReader.GetDateTime(6),mySqlDataReader.GetString(7),
                   TimeZoneConvert.MySqlDateTimeFromUtc(mySqlDataReader.GetDateTime(8)),mySqlDataReader.GetString(9));
                Addresses.Add(newAddress.AddressId, newAddress);
            }
            LogHandler.LogMessage("DatabaseHandler","Total Addresses " + Addresses.Count);
            mySqlDataReader.Close();
            _mySqlCommand.Dispose();
            CloseConnectionIfOpen();
            DatabaseInformationUpdated?.Invoke(this, new PropertyChangedEventArgs("Addresses Updated!"));
        }

        public void RefreshCustomers()
        {
            Customers.Clear();
            if (!CheckOrOpenConnection()) throw new Exception("Connection Error");
            _mySqlCommand = _mySqlConnection.CreateCommand();
            _mySqlCommand.CommandText = "SELECT * FROM customer";
            var mySqlDataReader = _mySqlCommand.ExecuteReader();
            while (mySqlDataReader.Read())
            {
                var newCustomer = new Customer(mySqlDataReader.GetInt32(0), mySqlDataReader.GetString(1), mySqlDataReader.GetInt32(2), mySqlDataReader.GetBoolean(3), 
                   TimeZoneConvert.MySqlDateTimeFromUtc(mySqlDataReader.GetDateTime(4)), mySqlDataReader.GetString(5),
                   TimeZoneConvert.MySqlDateTimeFromUtc(mySqlDataReader.GetDateTime(6)),mySqlDataReader.GetString(7));
                Customers.Add(newCustomer.CustomerId, newCustomer);
            }
            LogHandler.LogMessage("DatabaseHandler","Total Customers " + Customers.Count);
            mySqlDataReader.Close();
            _mySqlCommand.Dispose();
            CloseConnectionIfOpen();
            DatabaseInformationUpdated?.Invoke(this, new PropertyChangedEventArgs("Customers Updated!"));
        }

        public void RefreshAppointments()
        {
            Appointments.Clear();
            if (!CheckOrOpenConnection()) throw new Exception("Connection Error");
            _mySqlCommand = _mySqlConnection.CreateCommand();
            _mySqlCommand.CommandText = "SELECT * FROM appointment";
            var mySqlDataReader = _mySqlCommand.ExecuteReader();
            while (mySqlDataReader.Read())
            {
                var newAppointment = new Appointment(mySqlDataReader.GetInt32(0), mySqlDataReader.GetInt32(1), mySqlDataReader.GetInt32(2), mySqlDataReader.GetString(3), 
                    mySqlDataReader.GetString(4), mySqlDataReader.GetString(5), mySqlDataReader.GetString(6), mySqlDataReader.GetString(7), mySqlDataReader.GetString(8), 
                    TimeZoneConvert.MySqlDateTimeFromUtc(mySqlDataReader.GetDateTime(9)),TimeZoneConvert.MySqlDateTimeFromUtc(mySqlDataReader.GetDateTime(10)),
                   TimeZoneConvert.MySqlDateTimeFromUtc(mySqlDataReader.GetDateTime(11)), mySqlDataReader.GetString(12),
                   TimeZoneConvert.MySqlDateTimeFromUtc(mySqlDataReader.GetDateTime(13)), mySqlDataReader.GetString(14));
                Appointments.Add(newAppointment.AppointmentId, newAppointment);
            }
            LogHandler.LogMessage("DatabaseHandler","Total Appointments " + Appointments.Count);
            mySqlDataReader.Close();
            _mySqlCommand.Dispose();
            CloseConnectionIfOpen();
            DatabaseInformationUpdated?.Invoke(this, new PropertyChangedEventArgs("Appointments Updated!"));
        }
        
        #endregion
        
        #region InsertNewRecords

        public bool InsertAddress(Address newAddress)
        {
            if (!CheckOrOpenConnection()) return false;
            _mySqlCommand = DatabaseQueries.GetInsertAddressCommand(_mySqlConnection.CreateCommand(), newAddress);
            var result = _mySqlCommand.ExecuteNonQuery();
            _mySqlCommand.Dispose();
            CloseConnectionIfOpen();
            RefreshAddresses();
            return result > 0;
        }

        public bool InsertCity(City newCity)
        {
            if (!CheckOrOpenConnection()) return false;
            _mySqlCommand = DatabaseQueries.GetInsertCityCommand(_mySqlConnection.CreateCommand(), newCity);
            var result = _mySqlCommand.ExecuteNonQuery();
            CloseConnectionIfOpen();
            RefreshCities();
            return result > 0;
        }
        
        public bool InsertCountry(Country newCountry)
        {
            if (!CheckOrOpenConnection()) return false;
            _mySqlCommand = DatabaseQueries.GetInsertCountryCommand(_mySqlConnection.CreateCommand(), newCountry);
            var result = _mySqlCommand.ExecuteNonQuery();
            CloseConnectionIfOpen();
            return result > 0;
        }
        
        public bool InsertCustomer(Customer newCustomer)
        {
            if (!CheckOrOpenConnection()) return false;
            _mySqlCommand = DatabaseQueries.GetInsertCustomerCommand(_mySqlConnection.CreateCommand(), newCustomer);
            var result = _mySqlCommand.ExecuteNonQuery();
            CloseConnectionIfOpen();
            RefreshCustomers();
            return result > 0;
        }
        
        public bool InsertAppointment(Appointment newAppointment)
        {
            if (!CheckOrOpenConnection()) return false;
            _mySqlCommand = DatabaseQueries.GetInsertAppointmentCommand(_mySqlConnection.CreateCommand(), newAppointment);
            var result = _mySqlCommand.ExecuteNonQuery();
            CloseConnectionIfOpen();
            RefreshAppointments();
            return result > 0;
        }
        
        #endregion

        public void ClearLocalData()
        {
            Appointments.Clear();
            Customers.Clear();
            Addresses.Clear();
            Cities.Clear();
            Countries.Clear();
        }
    }
}