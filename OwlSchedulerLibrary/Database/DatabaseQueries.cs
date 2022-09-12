using System;
using System.Globalization;
using MySql.Data.MySqlClient;
using OwlSchedulerLibrary.Classes;

namespace OwlSchedulerLibrary.Database
{
    public static class DatabaseQueries
    {
        public const string GetAllAddress = "SELECT * FROM address";
        public const string GetAllAppointment = "SELECT * FROM appointment";
        public const string GetAllCity = "SELECT * FROM city";
        public const string GetAllCountry = "SELECT * FROM country";
        public const string GetAllCustomer = "SELECT * FROM customer";
        

        public static MySqlCommand GetInsertAddressCommand(MySqlCommand command, Address newAddress)
        {
            //return $"INSERT INTO address (address, address2, cityId, postalCode, phone, createDate, createdBy, lastUpdate, lastUpdateBy) VALUES ('{newAddress.AddressOne}','{newAddress.AddressTwo}','{newAddress.City}','{newAddress.PostalCode}','{newAddress.PhoneNumber}','{DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)}','{SessionManager.Instance.CurrentUser}','{DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)}','{SessionManager.Instance.CurrentUser}') ";
            
            command.CommandText = "INSERT INTO address (address, address2, cityId, postalCode, phone, createDate, createdBy, lastUpdate, lastUpdateBy) " +
                                  "VALUES (@AddressOne, @AddressTwo, @AddressCity, @AddressPostal, @AddressPhone, @AddressCreated, @AddressCreatedUser, @AddressLastUpdate, @AddressLastUpdateUser)";
            command.Parameters.AddWithValue("@AddressOne", newAddress.AddressOne);
            command.Parameters.AddWithValue("@AddressTwo", newAddress.AddressTwo);
            command.Parameters.AddWithValue("@AddressCity", newAddress.City);
            command.Parameters.AddWithValue("@AddressPostal", newAddress.PostalCode);
            command.Parameters.AddWithValue("@AddressPhone", newAddress.PhoneNumber);
            command.Parameters.AddWithValue("@AddressCreated", newAddress.CreateDateTime);
            command.Parameters.AddWithValue("@AddressCreatedUser", newAddress.CreateBy);
            command.Parameters.AddWithValue("@AddressLastUpdate", newAddress.LastUpdateDateTime);
            command.Parameters.AddWithValue("@AddressLastUpdateUser", newAddress.LastUpdateBy);
            return command;
        }

        public static MySqlCommand GetUpdateAddressCommand(MySqlCommand command, Address newAddress)
        { 
            //string query = $"UPDATE address SET address = '{newAddress.AddressOne}', address2 = '{newAddress.AddressTwo}', cityId = '{newAddress.City}', postalCode = '{newAddress.PostalCode}', phone = '{newAddress.PhoneNumber}', lastUpdate = '{DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)}', lastUpdateBy = '{SessionManager.Instance.CurrentUser}' WHERE addressId = '{newAddress.AddressId}'";
            
            command.CommandText = "UPDATE address SET address = @AddressOne, address2 = @AddressTwo, cityId = @AddressCity, postalCode = @AddressPostal, phone = @AddressPhone, lastUpdate = @AddressLastUpdate, lastUpdateBy = @LastUpdateUser WHERE addressId = @AddressId";
            command.Parameters.AddWithValue("@AddressId", newAddress.AddressId);
            command.Parameters.AddWithValue("@AddressOne", newAddress.AddressOne);
            command.Parameters.AddWithValue("@AddressTwo", newAddress.AddressTwo);
            command.Parameters.AddWithValue("@AddressCity", newAddress.City);
            command.Parameters.AddWithValue("@AddressPostal", newAddress.PostalCode);
            command.Parameters.AddWithValue("@AddressPhone", newAddress.PhoneNumber);
            command.Parameters.AddWithValue("@AddressLastUpdate", newAddress.LastUpdateDateTime);
            command.Parameters.AddWithValue("@AddressLastUpdateUser", newAddress.LastUpdateBy);
            return command;
        }

        public static MySqlCommand GetInsertCityCommand(MySqlCommand command, City newCity)
        {
            command.CommandText = "INSERT INTO city (city, countryId, createDate, createdBy, lastUpdate, lastUpdateBy) VALUES (@Name, @Country,@Created,@CreatedBy,@LastUpdate,@LastUpdateUser)";
            command.Parameters.AddWithValue("@Name", newCity.CityName);
            command.Parameters.AddWithValue("@Country", newCity.Country);
            command.Parameters.AddWithValue("@Created", newCity.CreateDateTime);
            command.Parameters.AddWithValue("@CreatedBy", newCity.CreateBy);
            command.Parameters.AddWithValue("@LastUpdate", newCity.LastUpdateDateTime);
            command.Parameters.AddWithValue("@LastUpdateUser", newCity.LastUpdateBy);
            return command;
        }
        
        public static MySqlCommand GetInsertCountryCommand(MySqlCommand command, Country newCountry)
        {
            command.CommandText = "INSERT INTO country (country, createDate, createdBy, lastUpdate, lastUpdateBy) VALUES (@Name,@Created,@CreatedBy,@LastUpdate,@LastUpdateUser)";
            command.Parameters.AddWithValue("@Name", newCountry.CountryName);
            command.Parameters.AddWithValue("@Created", newCountry.CreateDateTime);
            command.Parameters.AddWithValue("@CreatedBy", newCountry.CreateBy);
            command.Parameters.AddWithValue("@LastUpdate", newCountry.LastUpdateDateTime);
            command.Parameters.AddWithValue("@LastUpdateUser", newCountry.LastUpdateBy);
            return command;
        }
    }
}