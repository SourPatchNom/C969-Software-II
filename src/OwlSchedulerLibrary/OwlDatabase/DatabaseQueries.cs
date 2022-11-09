using MySql.Data.MySqlClient;
using OwlSchedulerLibrary.OwlSchedule.Classes;

namespace OwlSchedulerLibrary.OwlDatabase
{
    public static class DatabaseQueries
    {

        #region InsertCommands
        
        /// <summary>
        /// Formats the MySqlCommand object for an insert of the Address object.
        /// </summary>
        /// <param name="command">Command associated with mysql connection.</param>
        /// <param name="targetAddress">Address object that needs to be inserted into database.</param>
        /// <returns>Formatted MySqlCommand object.</returns>
        public static MySqlCommand GetInsertAddressCommand(MySqlCommand command, Address targetAddress)
        {
            command.CommandText = "INSERT INTO address (address, address2, cityId, postalCode, phone, createDate, createdBy, lastUpdate, lastUpdateBy) " +
                                  "VALUES (@AddressOne, @AddressTwo, @AddressCity, @AddressPostal, @AddressPhone, @AddressCreated, @AddressCreatedUser, @AddressLastUpdate, @AddressLastUpdateUser)";
            command.Parameters.AddWithValue("@AddressOne", targetAddress.AddressOne);
            command.Parameters.AddWithValue("@AddressTwo", targetAddress.AddressTwo);
            command.Parameters.AddWithValue("@AddressCity", targetAddress.CityId);
            command.Parameters.AddWithValue("@AddressPostal", targetAddress.PostalCode);
            command.Parameters.AddWithValue("@AddressPhone", targetAddress.PhoneNumber);
            command.Parameters.AddWithValue("@AddressCreated", targetAddress.CreateDateTime.ToUniversalTime());
            command.Parameters.AddWithValue("@AddressCreatedUser", targetAddress.CreateBy);
            command.Parameters.AddWithValue("@AddressLastUpdate", targetAddress.LastUpdateDateTime.ToUniversalTime());
            command.Parameters.AddWithValue("@AddressLastUpdateUser", targetAddress.LastUpdateBy);
            return command;
        }

        /// <summary>
        /// Formats the MySqlCommand object for an insert of the Appointment object.
        /// </summary>
        /// <param name="command">Command associated with mysql connection.</param>
        /// <param name="targetAppointment">Appointment object that needs to be inserted into database.</param>
        /// <returns>Formatted MySqlCommand object.</returns>
        public static MySqlCommand GetInsertAppointmentCommand(MySqlCommand command, Appointment targetAppointment)
        {
            command.CommandText = "INSERT INTO appointment (customerId, userId, title, description, location, contact, type, url, start, end, createDate, createdBy, lastUpdate, lastUpdateBy) " +
                                  "VALUES (@CustomerId,@UserId,@Title,@Description,@Location,@Contact,@Type,@Url,@Start,@End,@CreateDate,@CreatedBy,@LastUpdate,@LastUpdateBy)";
            command.Parameters.AddWithValue("@CustomerId", targetAppointment.CustomerId);
            command.Parameters.AddWithValue("@UserId", targetAppointment.UserId);
            command.Parameters.AddWithValue("@Title", targetAppointment.Title);
            command.Parameters.AddWithValue("@Description", targetAppointment.Description);
            command.Parameters.AddWithValue("@Location", targetAppointment.Location);
            command.Parameters.AddWithValue("@Contact", targetAppointment.Contact);
            command.Parameters.AddWithValue("@Type", targetAppointment.Type);
            command.Parameters.AddWithValue("@Url", targetAppointment.Url);
            command.Parameters.AddWithValue("@Start", targetAppointment.StartDateTime.ToUniversalTime());
            command.Parameters.AddWithValue("@End", targetAppointment.EndDateTime.ToUniversalTime());
            command.Parameters.AddWithValue("@CreateDate", targetAppointment.CreateDateTime.ToUniversalTime());
            command.Parameters.AddWithValue("@CreatedBy", targetAppointment.CreateBy);
            command.Parameters.AddWithValue("@LastUpdate", targetAppointment.LastUpdateDateTime.ToUniversalTime());
            command.Parameters.AddWithValue("@LastUpdateBy", targetAppointment.LastUpdateBy);
            return command;
        }
        
        /// <summary>
        /// Formats the MySqlCommand object for an insert of the City object.
        /// </summary>
        /// <param name="command">Command associated with mysql connection.</param>
        /// <param name="targetCity">City object that needs to be inserted into database.</param>
        /// <returns>Formatted MySqlCommand object.</returns>
        public static MySqlCommand GetInsertCityCommand(MySqlCommand command, City targetCity)
        {
            command.CommandText = "INSERT INTO city (city, countryId, createDate, createdBy, lastUpdate, lastUpdateBy) VALUES (@Name, @Country,@CreateDate,@CreatedBy,@LastUpdate,@LastUpdateUser)";
            command.Parameters.AddWithValue("@Name", targetCity.CityName);
            command.Parameters.AddWithValue("@Country", targetCity.Country);
            command.Parameters.AddWithValue("@CreateDate", targetCity.CreateDateTime.ToUniversalTime());
            command.Parameters.AddWithValue("@CreatedBy", targetCity.CreateBy);
            command.Parameters.AddWithValue("@LastUpdate", targetCity.LastUpdateDateTime.ToUniversalTime());
            command.Parameters.AddWithValue("@LastUpdateUser", targetCity.LastUpdateBy);
            return command;
        }
        
        /// <summary>
        /// Formats the MySqlCommand object for an insert of the Country object.
        /// </summary>
        /// <param name="command">Command associated with mysql connection.</param>
        /// <param name="targetCountry">Country object that needs to be inserted into database.</param>
        /// <returns>Formatted MySqlCommand object.</returns>
        public static MySqlCommand GetInsertCountryCommand(MySqlCommand command, Country targetCountry)
        {
            command.CommandText = "INSERT INTO country (country, createDate, createdBy, lastUpdate, lastUpdateBy) VALUES (@Name,@Created,@CreatedBy,@LastUpdate,@LastUpdateBy)";
            command.Parameters.AddWithValue("@Name", targetCountry.CountryName);
            command.Parameters.AddWithValue("@Created", targetCountry.CreateDateTime.ToUniversalTime());
            command.Parameters.AddWithValue("@CreatedBy", targetCountry.CreateBy);
            command.Parameters.AddWithValue("@LastUpdate", targetCountry.LastUpdateDateTime.ToUniversalTime());
            command.Parameters.AddWithValue("@LastUpdateBy", targetCountry.LastUpdateBy);
            return command;
        }

        /// <summary>
        /// Formats the MySqlCommand object for an insert of the Customer object.
        /// </summary>
        /// <param name="command">Command associated with mysql connection.</param>
        /// <param name="targetCustomer">Customer object that needs to be inserted into database.</param>
        /// <returns>Formatted MySqlCommand object.</returns>
        public static MySqlCommand GetInsertCustomerCommand(MySqlCommand command, Customer targetCustomer)
        {
            command.CommandText = "INSERT INTO customer (customerName, addressId, active, createDate, createdBy, lastUpdate, lastUpdateBy) VALUES (@Name,@Address,@Active,@Created,@CreatedBy,@LastUpdate,@LastUpdateBy)";
            command.Parameters.AddWithValue("@Name", targetCustomer.CustomerName);
            command.Parameters.AddWithValue("@Address", targetCustomer.CustomerAddress);
            command.Parameters.AddWithValue("@Active", targetCustomer.Active ? 1 : 0);
            command.Parameters.AddWithValue("@Created", targetCustomer.CreateDateTime.ToUniversalTime());
            command.Parameters.AddWithValue("@CreatedBy", targetCustomer.CreateBy);
            command.Parameters.AddWithValue("@LastUpdate", targetCustomer.LastUpdateDateTime.ToUniversalTime());
            command.Parameters.AddWithValue("@LastUpdateBy", targetCustomer.LastUpdateBy);
            return command;
        }

        #endregion
        
        #region UpdateCommands
        
        /// <summary>
        /// Formats the MySqlCommand object for an UPDATE of the Address object.
        /// </summary>
        /// <param name="command">Command associated with mysql connection.</param>
        /// <param name="targetAddress">Address object that needs to be updated in the database.</param>
        /// <returns>Formatted MySqlCommand object.</returns>
        public static MySqlCommand GetUpdateAddressCommand(MySqlCommand command, Address targetAddress)
        {
            command.CommandText = "UPDATE address SET address = @AddressOne, address2 = @AddressTwo, cityId = @AddressCity, postalCode = @AddressPostal, phone = @AddressPhone, lastUpdate = @AddressLastUpdate, lastUpdateBy = @AddressLastUpdateUser WHERE addressId = @AddressId";
            command.Parameters.AddWithValue("@AddressId", targetAddress.AddressId);
            command.Parameters.AddWithValue("@AddressOne", targetAddress.AddressOne);
            command.Parameters.AddWithValue("@AddressTwo", targetAddress.AddressTwo);
            command.Parameters.AddWithValue("@AddressCity", targetAddress.CityId);
            command.Parameters.AddWithValue("@AddressPostal", targetAddress.PostalCode);
            command.Parameters.AddWithValue("@AddressPhone", targetAddress.PhoneNumber);
            command.Parameters.AddWithValue("@AddressLastUpdate", targetAddress.LastUpdateDateTime.ToUniversalTime());
            command.Parameters.AddWithValue("@AddressLastUpdateUser", targetAddress.LastUpdateBy);
            return command;
        }

        /// <summary>
        /// Formats the MySqlCommand object for an UPDATE of the Appointment object.
        /// </summary>
        /// <param name="command">Command associated with mysql connection.</param>
        /// <param name="targetAppointment">Appointment object that needs to be updated in the database.</param>
        /// <returns>Formatted MySqlCommand object.</returns>
        public static MySqlCommand GetUpdateAppointmentCommand(MySqlCommand command, Appointment targetAppointment)
        {
            command.CommandText = "UPDATE appointment SET customerId=@CustomerId,userId=@UserId,title=@Title,description=@Description,location=@Location,contact=@Contact," +
                                  "type=@Type,url=@Url,start=@Start,end=@end,lastUpdate=@LastUpdate,lastUpdateBy=@LastUpdateUser WHERE appointmentId=@AppointmentId";
            command.Parameters.AddWithValue("@AppointmentId", targetAppointment.AppointmentId);
            command.Parameters.AddWithValue("@CustomerId", targetAppointment.CustomerId);
            command.Parameters.AddWithValue("@UserId", targetAppointment.UserId);
            command.Parameters.AddWithValue("@Title", targetAppointment.Title);
            command.Parameters.AddWithValue("@Description", targetAppointment.Description);
            command.Parameters.AddWithValue("@Location", targetAppointment.Location);
            command.Parameters.AddWithValue("@Contact", targetAppointment.Contact);
            command.Parameters.AddWithValue("@Type", targetAppointment.Type);
            command.Parameters.AddWithValue("@Url", targetAppointment.Url);
            command.Parameters.AddWithValue("@Start", targetAppointment.StartDateTime.ToUniversalTime());
            command.Parameters.AddWithValue("@End", targetAppointment.EndDateTime.ToUniversalTime());
            command.Parameters.AddWithValue("@LastUpdate", targetAppointment.LastUpdateDateTime.ToUniversalTime());
            command.Parameters.AddWithValue("@LastUpdateUser", targetAppointment.LastUpdateBy);
            return command;
        }
        
        /// <summary>
        /// Formats the MySqlCommand object for an UPDATE of the City object.
        /// </summary>
        /// <param name="command">Command associated with mysql connection.</param>
        /// <param name="targetCity">City object that needs to be updated in the database.</param>
        /// <returns>Formatted MySqlCommand object.</returns>
        public static MySqlCommand GetUpdateCityCommand(MySqlCommand command, City targetCity)
        {
            command.CommandText = "UPDATE city SET city=@Name, countryId=@Country, lastUpdate=@LastUpdate, lastUpdateBy=@LastUpdateUser WHERE cityId=@Id";
            command.Parameters.AddWithValue("@Id", targetCity.CityId);
            command.Parameters.AddWithValue("@Name", targetCity.CityName);
            command.Parameters.AddWithValue("@Country", targetCity.Country);
            command.Parameters.AddWithValue("@LastUpdate", targetCity.LastUpdateDateTime.ToUniversalTime());
            command.Parameters.AddWithValue("@LastUpdateUser", targetCity.LastUpdateBy);
            return command;
        }
        
        /// <summary>
        /// Formats the MySqlCommand object for an UPDATE of the Country object.
        /// </summary>
        /// <param name="command">Command associated with mysql connection.</param>
        /// <param name="targetCountry">Country object that needs to be updated in the database.</param>
        /// <returns>Formatted MySqlCommand object.</returns>
        public static MySqlCommand GetUpdateCountryCommand(MySqlCommand command, Country targetCountry)
        {
            command.CommandText = "UPDATE country SET country=@Name, lastUpdate=@LastUpdate, lastUpdateBy=@LastUpdateUser WHERE countryId=@CountryId";
            command.Parameters.AddWithValue("@CountryId", targetCountry.CountryId);
            command.Parameters.AddWithValue("@Name", targetCountry.CountryName);
            command.Parameters.AddWithValue("@LastUpdate", targetCountry.LastUpdateDateTime.ToUniversalTime());
            command.Parameters.AddWithValue("@LastUpdateUser", targetCountry.LastUpdateBy);
            return command;
        }
        
        /// <summary>
        /// Formats the MySqlCommand object for an UPDATE of the Customer object.
        /// </summary>
        /// <param name="command">Command associated with mysql connection.</param>
        /// <param name="targetCustomer">Customer object that needs to be updated in the database.</param>
        /// <returns>Formatted MySqlCommand object.</returns>
        public static MySqlCommand GetUpdateCustomerCommand(MySqlCommand command, Customer targetCustomer)
        {
            command.CommandText = "UPDATE customer SET customerName=@Name, addressId=@Address,active=@Active, lastUpdate=@LastUpdate, lastUpdateBy=@LastUpdateUser WHERE customerId=@CustomerId";
            command.Parameters.AddWithValue("@CustomerId", targetCustomer.CustomerId);
            command.Parameters.AddWithValue("@Name", targetCustomer.CustomerName);
            command.Parameters.AddWithValue("@Address", targetCustomer.CustomerAddress);
            command.Parameters.AddWithValue("@Active", targetCustomer.Active ? 1 : 0);
            command.Parameters.AddWithValue("@LastUpdate", targetCustomer.LastUpdateDateTime.ToUniversalTime());
            command.Parameters.AddWithValue("@LastUpdateUser", targetCustomer.LastUpdateBy);
            return command;
        }
        
        #endregion
        
        #region DeleteCommands
        
        /// <summary>
        /// Formats the MySqlCommand object for a DELETE of the Address object.
        /// </summary>
        /// <param name="command">Command associated with mysql connection.</param>
        /// <param name="targetAddress">Address object that needs to be deleted from the database.</param>
        /// <returns>Formatted MySqlCommand object.</returns>
        public static MySqlCommand GetDeleteAddressCommand(MySqlCommand command, int targetAddress)
        {
            command.CommandText = "DELETE FROM address WHERE addressId = @AddressId";
            command.Parameters.AddWithValue("@AddressId", targetAddress);
            return command;
        }

        /// <summary>
        /// Formats the MySqlCommand object for a DELETE of the Appointment object.
        /// </summary>
        /// <param name="command">Command associated with mysql connection.</param>
        /// <param name="targetAppointment">Appointment object that needs to be deleted from the database.</param>
        /// <returns>Formatted MySqlCommand object.</returns>
        public static MySqlCommand GetDeleteAppointmentCommand(MySqlCommand command, int targetAppointment)
        {
            command.CommandText = "DELETE FROM appointment WHERE appointmentId=@AppointmentId";
            command.Parameters.AddWithValue("@AppointmentId", targetAppointment);
            return command;
        }
        
        /// <summary>
        /// Formats the MySqlCommand object for a DELETE of the City object.
        /// </summary>
        /// <param name="command">Command associated with mysql connection.</param>
        /// <param name="targetCity">City object that needs to be deleted from the database.</param>
        /// <returns>Formatted MySqlCommand object.</returns>
        public static MySqlCommand GetDeleteCityCommand(MySqlCommand command, int targetCity)
        {
            command.CommandText = "DELETE FROM city WHERE cityId=@Id";
            command.Parameters.AddWithValue("@Id", targetCity);
            return command;
        }
        
        /// <summary>
        /// Formats the MySqlCommand object for an DELETE of the Country object.
        /// </summary>
        /// <param name="command">Command associated with mysql connection.</param>
        /// <param name="targetCountry">Country object that needs to be deleted from the database.</param>
        /// <returns>Formatted MySqlCommand object.</returns>
        public static MySqlCommand GetDeleteCountryCommand(MySqlCommand command, int targetCountry)
        {
            command.CommandText = "DELETE FROM country WHERE countryId=@CountryId";
            command.Parameters.AddWithValue("@CountryId", targetCountry);
            return command;
        }
        
        /// <summary>
        /// Formats the MySqlCommand object for a DELETE of the Customer object.
        /// </summary>
        /// <param name="command">Command associated with mysql connection.</param>
        /// <param name="targetCustomer">Customer object that needs to be deleted from the database.</param>
        /// <returns>Formatted MySqlCommand object.</returns>
        public static MySqlCommand GetDeleteCustomerCommand(MySqlCommand command, int targetCustomer)
        {
            command.CommandText = "DELETE FROM customer WHERE customerId=@CustomerId";
            command.Parameters.AddWithValue("@CustomerId", targetCustomer);
            return command;
        }
        
        #endregion
    }
}