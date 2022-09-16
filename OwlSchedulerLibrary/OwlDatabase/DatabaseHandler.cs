using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using MySql.Data.MySqlClient;
using OwlSchedulerLibrary.OwlDatabase.Extensions;
using OwlSchedulerLibrary.OwlLogger;
using OwlSchedulerLibrary.OwlSchedule;
using OwlSchedulerLibrary.OwlSchedule.Classes;

namespace OwlSchedulerLibrary.OwlDatabase
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
        public bool Initialized { get; set; }

        public Dictionary<int, User> Users { get; private set; } = new Dictionary<int, User>();
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
            RefreshUsers();
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
            _mySqlCommand.ExecuteNonQuery();
            _mySqlCommand.Dispose();
            CloseConnectionIfOpen();
        }

        private void RefreshUsers()
        {
            Users.Clear();
            if (!CheckOrOpenConnection()) throw new Exception("Connection Error");
            _mySqlCommand = _mySqlConnection.CreateCommand();
            _mySqlCommand.CommandText = "SELECT userId, userName, active FROM user";
            var mySqlDataReader = _mySqlCommand.ExecuteReader();
            while (mySqlDataReader.Read())
            {
                var user = new User(mySqlDataReader.GetInt32(0), mySqlDataReader.GetString(1), mySqlDataReader.GetInt32(2)); 
                Users.Add(user.UserId,user);
            }
            LogHandler.LogMessage("DatabaseHandler","Total Countries " + Countries.Count);
            mySqlDataReader.Close();
            _mySqlCommand.Dispose();
            CloseConnectionIfOpen();
            DatabaseInformationUpdated?.Invoke(this, new PropertyChangedEventArgs("Users Updated!"));
        }

        private void RefreshCountries()
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

        private void RefreshCities()
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

        private void RefreshAddresses()
        {
            Addresses.Clear();
            if (!CheckOrOpenConnection()) throw new Exception("Connection Error");
            _mySqlCommand = _mySqlConnection.CreateCommand();
            _mySqlCommand.CommandText = "SELECT * FROM address";
            var mySqlDataReader = _mySqlCommand.ExecuteReader();
            while (mySqlDataReader.Read())
            {
                var newAddress = new Address(mySqlDataReader.GetInt32(0), mySqlDataReader.GetString(1), mySqlDataReader.GetString(2), mySqlDataReader.GetInt32(3), 
                    mySqlDataReader.GetString(4), mySqlDataReader.GetString(5),TimeZoneConvert.MySqlDateTimeFromUtc(mySqlDataReader.GetDateTime(6)),mySqlDataReader.GetString(7),
                   TimeZoneConvert.MySqlDateTimeFromUtc(mySqlDataReader.GetDateTime(8)),mySqlDataReader.GetString(9));
                Addresses.Add(newAddress.AddressId, newAddress);
            }
            LogHandler.LogMessage("DatabaseHandler","Total Addresses " + Addresses.Count);
            mySqlDataReader.Close();
            _mySqlCommand.Dispose();
            CloseConnectionIfOpen();
            DatabaseInformationUpdated?.Invoke(this, new PropertyChangedEventArgs("Addresses Updated!"));
        }

        private void RefreshCustomers()
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

        private void RefreshAppointments()
        {
            Appointments.Clear();
            if (!CheckOrOpenConnection()) throw new Exception("Connection Error");
            _mySqlCommand = _mySqlConnection.CreateCommand();
            _mySqlCommand.CommandText = "SELECT * FROM appointment";
            // This version should be implemented for large databases, though given assignment, we are going to collect ALL appointments from DB. Preserved for future reference. 
            //_mySqlCommand.CommandText = "SELECT * FROM appointment WHERE userID = @userId";
            //_mySqlCommand.Parameters.AddWithValue("@userId", CurrentSession.Instance.CurrentUser.UserId);
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
        
        public bool CustomerInAppointments(int id)
        {
            if (!CheckOrOpenConnection()) throw new Exception("Connection Error");
            _mySqlCommand = _mySqlConnection.CreateCommand();
            _mySqlCommand.CommandText = "SELECT * FROM appointment WHERE customerId = @customerId";
            _mySqlCommand.Parameters.AddWithValue("@customerId", id);
            var mySqlDataReader = _mySqlCommand.ExecuteReader();
            var hasRows = mySqlDataReader.HasRows; 
            LogHandler.LogMessage("DatabaseHandler", hasRows ? "Customer " + id + " has associated appointment in database." : "Customer " + id + " has no associations.");
            mySqlDataReader.Close();
            _mySqlCommand.Dispose();
            CloseConnectionIfOpen();
            return hasRows;
        }
        
        public bool AddressInCustomer(int id)
        {
            if (!CheckOrOpenConnection()) throw new Exception("Connection Error");
            _mySqlCommand = _mySqlConnection.CreateCommand();
            _mySqlCommand.CommandText = "SELECT * FROM customer WHERE addressId = @addressId";
            _mySqlCommand.Parameters.AddWithValue("@addressId", id);
            var mySqlDataReader = _mySqlCommand.ExecuteReader();
            var hasRows = mySqlDataReader.HasRows; 
            LogHandler.LogMessage("DatabaseHandler", hasRows ? "Address " + id + " has associated customer in database." : "Address " + id + " has no associations.");
            mySqlDataReader.Close();
            _mySqlCommand.Dispose();
            CloseConnectionIfOpen();
            return hasRows;
        }
        
        public bool CityInAddress(int id)
        {
            if (!CheckOrOpenConnection()) throw new Exception("Connection Error");
            _mySqlCommand = _mySqlConnection.CreateCommand();
            _mySqlCommand.CommandText = "SELECT * FROM address WHERE cityId = @id";
            _mySqlCommand.Parameters.AddWithValue("@id", id);
            var mySqlDataReader = _mySqlCommand.ExecuteReader();
            var hasRows = mySqlDataReader.HasRows; 
            LogHandler.LogMessage("DatabaseHandler", hasRows ? "City " + id + " has associated address in database." : "City " + id + " has no associations.");
            mySqlDataReader.Close();
            _mySqlCommand.Dispose();
            CloseConnectionIfOpen();
            return hasRows;
        }
        
        public bool CountryInCity(int id)
        {
            if (!CheckOrOpenConnection()) throw new Exception("Connection Error");
            _mySqlCommand = _mySqlConnection.CreateCommand();
            _mySqlCommand.CommandText = "SELECT * FROM city WHERE countryId = @id";
            _mySqlCommand.Parameters.AddWithValue("@id", id);
            var mySqlDataReader = _mySqlCommand.ExecuteReader();
            var hasRows = mySqlDataReader.HasRows; 
            LogHandler.LogMessage("DatabaseHandler", hasRows ? "Country " + id + " has associated city in database." : "Country " + id + " has no associations.");
            mySqlDataReader.Close();
            _mySqlCommand.Dispose();
            CloseConnectionIfOpen();
            return hasRows;
        }
        
        #endregion
        
        #region NonQuery

        public int InsertAddress(Address newAddress)
        {
            var row = ExecuteNonQueryReturnRecord(_mySqlCommand = DatabaseQueries.GetInsertAddressCommand(_mySqlConnection.CreateCommand(), newAddress));
            RefreshAddresses();
            return row;
        }

        public int UpdateAddress(Address newAddress)
        {
            var row = ExecuteNonQueryReturnRecord(_mySqlCommand = DatabaseQueries.GetUpdateAddressCommand(_mySqlConnection.CreateCommand(), newAddress));
            RefreshAddresses();
            return row;
        }

        public int DeleteAddress(int id)
        {
            var row = ExecuteNonQueryReturnRow(_mySqlCommand = DatabaseQueries.GetDeleteAddressCommand(_mySqlConnection.CreateCommand(), id));
            RefreshAddresses();
            return row;
        }
        
        public int InsertCity(City newCity)
        {
            var row = ExecuteNonQueryReturnRecord(_mySqlCommand = DatabaseQueries.GetInsertCityCommand(_mySqlConnection.CreateCommand(), newCity));
            RefreshCities();
            return row;
        }
        
        public int UpdateCity(City newCity)
        {
            var row = ExecuteNonQueryReturnRecord(_mySqlCommand = DatabaseQueries.GetUpdateCityCommand(_mySqlConnection.CreateCommand(), newCity));
            RefreshCities();
            return row;
        }
        
        public int DeleteCity(int id)
        {
            var row = ExecuteNonQueryReturnRow(_mySqlCommand = DatabaseQueries.GetDeleteCityCommand(_mySqlConnection.CreateCommand(), id));
            RefreshCities();
            return row;
        }
        
        public int InsertCountry(Country newCountry)
        {
            var row = ExecuteNonQueryReturnRecord(_mySqlCommand = DatabaseQueries.GetInsertCountryCommand(_mySqlConnection.CreateCommand(), newCountry));
            RefreshCountries();
            return row;
        }
        
        public int UpdateCountry(Country newCountry)
        {
            var row = ExecuteNonQueryReturnRecord(_mySqlCommand = DatabaseQueries.GetUpdateCountryCommand(_mySqlConnection.CreateCommand(), newCountry));
            RefreshCountries();
            return row;
        }
        
        public int DeleteCountry(int id)
        {
            var row = ExecuteNonQueryReturnRow(_mySqlCommand = DatabaseQueries.GetDeleteCountryCommand(_mySqlConnection.CreateCommand(), id));
            RefreshCountries();
            return row;
        }
        
        public int InsertCustomer(Customer newCustomer)
        {
            var row = ExecuteNonQueryReturnRecord(_mySqlCommand = DatabaseQueries.GetInsertCustomerCommand(_mySqlConnection.CreateCommand(), newCustomer));
            RefreshCustomers();
            return row;
        }
        
        public int UpdateCustomer(Customer newCustomer)
        {
            var row = ExecuteNonQueryReturnRecord(_mySqlCommand = DatabaseQueries.GetUpdateCustomerCommand(_mySqlConnection.CreateCommand(), newCustomer));
            RefreshCustomers();
            return row;
        }
        
        public int DeleteCustomer(int id)
        {
            var row = ExecuteNonQueryReturnRow(_mySqlCommand = DatabaseQueries.GetDeleteCustomerCommand(_mySqlConnection.CreateCommand(), id));
            RefreshCustomers();
            return row;
        }
        
        public int InsertAppointment(Appointment newAppointment)
        {
            var row = ExecuteNonQueryReturnRecord(_mySqlCommand = DatabaseQueries.GetInsertAppointmentCommand(_mySqlConnection.CreateCommand(), newAppointment));
            RefreshAppointments();
            return row;
        }
        
        public int UpdateAppointment(Appointment newAppointment)
        {
            var row = ExecuteNonQueryReturnRecord(_mySqlCommand = DatabaseQueries.GetUpdateAppointmentCommand(_mySqlConnection.CreateCommand(), newAppointment));
            RefreshAppointments();
            return row;
        }
        
        public int DeleteAppointment(int id)
        {
            var row = ExecuteNonQueryReturnRow(_mySqlCommand = DatabaseQueries.GetDeleteAppointmentCommand(_mySqlConnection.CreateCommand(), id));
            RefreshAppointments();
            return row;
        }

        private int ExecuteNonQueryReturnRecord(MySqlCommand mySqlCommand)
        {            
            if (!CheckOrOpenConnection()) return -1;
            mySqlCommand.ExecuteNonQuery();
            var row = (int)mySqlCommand.LastInsertedId;
            _mySqlCommand.Dispose();
            CloseConnectionIfOpen();
            return row;
        }
        
        private int ExecuteNonQueryReturnRow(MySqlCommand mySqlCommand)
        {            
            if (!CheckOrOpenConnection()) return -1;
            var row = mySqlCommand.ExecuteNonQuery();
            _mySqlCommand.Dispose();
            CloseConnectionIfOpen();
            return row;
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