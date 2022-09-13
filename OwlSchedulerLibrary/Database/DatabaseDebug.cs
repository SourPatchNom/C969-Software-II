using System;
using MySql.Data.MySqlClient;
using OwlSchedulerLibrary.Classes;
using OwlSchedulerLibrary.Database.Classes;

namespace OwlSchedulerLibrary.Database
{
    public static class DatabaseDebugTools
    {
        public static MySqlCommand GetDeleteAllCommand(MySqlCommand command)
        {
            command.CommandText = "DELETE FROM appointment; ALTER TABLE appointment AUTO_INCREMENT=1;" +
                                  "DELETE FROM customer; ALTER TABLE customer AUTO_INCREMENT=1; " +
                                  "DELETE FROM address; ALTER TABLE address AUTO_INCREMENT=1; " +
                                  "DELETE FROM city; ALTER TABLE city AUTO_INCREMENT=1; " +
                                  "DELETE FROM country; ALTER TABLE country AUTO_INCREMENT=1; ";
            return command;
        }
        
        public static void PopulateWithData()
        {
            DatabaseHandler.Instance.InsertCountry(new Country(-1, "USA", DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertCountry(new Country(-1, "Scotland", DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertCountry(new Country(-1, "France", DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertCountry(new Country(-1, "England", DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertCountry(new Country(-1, "Japan", DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));

            DatabaseHandler.Instance.InsertCity(new City(-1, "Dallas", 1, DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertCity(new City(-1, "Edinburgh", 2, DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertCity(new City(-1, "Paris", 3, DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertCity(new City(-1, "London", 4, DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertCity(new City(-1, "Tokyo", 5, DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            
            DatabaseHandler.Instance.InsertAddress(new Address(-1, "12", "Main Street", 1, "88888", "18888888888", DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertAddress(new Address(-1, "34", "Peach Highway", 2, "88888", "18888888888", DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertAddress(new Address(-1, "56", "Grape Lane", 3, "88888", "18888888888", DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertAddress(new Address(-1, "78", "Apple Street", 4, "88888", "18888888888", DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertAddress(new Address(-1, "91", "Orange Street", 5, "88888", "18888888888", DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));

            DatabaseHandler.Instance.InsertCustomer(new Customer(-1,"John Doe",1,true,DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertCustomer(new Customer(-1,"John Doe",2,true,DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertCustomer(new Customer(-1,"John Doe",3,true,DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertCustomer(new Customer(-1,"John Doe",4,false,DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertCustomer(new Customer(-1,"John Doe",5,true,DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));

            DatabaseHandler.Instance.InsertAppointment(new Appointment(-1,1,1,"Meeting A","Lorem Ipsum","Facetime","Billy Bob","Virtual","http://debug.com/",DateTime.UtcNow.Add(new TimeSpan(0,2,2)), DateTime.UtcNow.Add(new TimeSpan(0,5,2)), DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertAppointment(new Appointment(-1,2,1,"Meeting B","Lorem Ipsum","Facetime","Billy Bob","Virtual","http://debug.com/",DateTime.UtcNow.Add(new TimeSpan(0,0,30)), DateTime.UtcNow.Add(new TimeSpan(0,0,45)), DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertAppointment(new Appointment(-1,3,1,"Meeting C","Lorem Ipsum","Facetime","Billy Bob","Virtual","http://debug.com/",DateTime.UtcNow.Add(new TimeSpan(3,2,2)), DateTime.UtcNow.Add(new TimeSpan(3,22,2)), DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertAppointment(new Appointment(-1,4,1,"Meeting D","Lorem Ipsum","Facetime","Billy Bob","Virtual","http://debug.com/",DateTime.UtcNow.Add(new TimeSpan(2,2,2)), DateTime.UtcNow.Add(new TimeSpan(2,22,2)), DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertAppointment(new Appointment(-1,5,1,"Meeting E","Lorem Ipsum","Facetime","Billy Bob","Virtual","http://debug.com/",DateTime.UtcNow.Add(new TimeSpan(6,2,2)), DateTime.UtcNow.Add(new TimeSpan(6,22,2)), DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            
            DatabaseHandler.Instance.InsertAppointment(new Appointment(-1,1,1,"Meeting A1","Lorem Ipsum","Facetime","Billy Bob","Virtual","http://debug.com/",DateTime.UtcNow.Add(new TimeSpan(163,2,2)), DateTime.UtcNow.Add(new TimeSpan(163,22,2)), DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertAppointment(new Appointment(-1,2,1,"Meeting B1","Lorem Ipsum","Facetime","Billy Bob","Virtual","http://debug.com/",DateTime.UtcNow.Add(new TimeSpan(162,2,2)), DateTime.UtcNow.Add(new TimeSpan(162,22,2)), DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertAppointment(new Appointment(-1,3,1,"Meeting C1","Lorem Ipsum","Facetime","Billy Bob","Virtual","http://debug.com/",DateTime.UtcNow.Add(new TimeSpan(165,2,2)), DateTime.UtcNow.Add(new TimeSpan(165,22,2)), DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertAppointment(new Appointment(-1,4,1,"Meeting D1","Lorem Ipsum","Facetime","Billy Bob","Virtual","http://debug.com/",DateTime.UtcNow.Add(new TimeSpan(164,2,2)), DateTime.UtcNow.Add(new TimeSpan(164,22,2)), DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertAppointment(new Appointment(-1,5,1,"Meeting E1","Lorem Ipsum","Facetime","Billy Bob","Virtual","http://debug.com/",DateTime.UtcNow.Add(new TimeSpan(166,2,2)), DateTime.UtcNow.Add(new TimeSpan(166,22,2)), DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            
            DatabaseHandler.Instance.InsertAppointment(new Appointment(-1,1,1,"Meeting AA","Lorem Ipsum","Facetime","Billy Bob","Virtual","http://debug.com/",DateTime.UtcNow.Add(new TimeSpan(733,2,2)), DateTime.UtcNow.Add(new TimeSpan(733,22,2)), DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertAppointment(new Appointment(-1,2,1,"Meeting BB","Lorem Ipsum","Facetime","Billy Bob","Virtual","http://debug.com/",DateTime.UtcNow.Add(new TimeSpan(732,2,2)), DateTime.UtcNow.Add(new TimeSpan(732,22,2)), DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertAppointment(new Appointment(-1,3,1,"Meeting CC","Lorem Ipsum","Facetime","Billy Bob","Virtual","http://debug.com/",DateTime.UtcNow.Add(new TimeSpan(735,2,2)), DateTime.UtcNow.Add(new TimeSpan(735,22,2)), DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertAppointment(new Appointment(-1,4,1,"Meeting DD","Lorem Ipsum","Facetime","Billy Bob","Virtual","http://debug.com/",DateTime.UtcNow.Add(new TimeSpan(734,2,2)), DateTime.UtcNow.Add(new TimeSpan(734,22,2)), DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertAppointment(new Appointment(-1,5,1,"Meeting EE","Lorem Ipsum","Facetime","Billy Bob","Virtual","http://debug.com/",DateTime.UtcNow.Add(new TimeSpan(736,2,2)), DateTime.UtcNow.Add(new TimeSpan(736,22,2)), DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            
            DatabaseHandler.Instance.InsertAppointment(new Appointment(-1,1,1,"Meeting AAA","Lorem Ipsum","Facetime","Billy Bob","Virtual","http://debug.com/",DateTime.UtcNow.Add(new TimeSpan(1433,2,2)), DateTime.UtcNow.Add(new TimeSpan(1433,22,2)), DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertAppointment(new Appointment(-1,2,1,"Meeting BBB","Lorem Ipsum","Facetime","Billy Bob","Virtual","http://debug.com/",DateTime.UtcNow.Add(new TimeSpan(1442,2,2)), DateTime.UtcNow.Add(new TimeSpan(1442,22,2)), DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertAppointment(new Appointment(-1,3,1,"Meeting CCC","Lorem Ipsum","Facetime","Billy Bob","Virtual","http://debug.com/",DateTime.UtcNow.Add(new TimeSpan(1435,2,2)), DateTime.UtcNow.Add(new TimeSpan(1435,22,2)), DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertAppointment(new Appointment(-1,4,1,"Meeting DDD","Lorem Ipsum","Facetime","Billy Bob","Virtual","http://debug.com/",DateTime.UtcNow.Add(new TimeSpan(1434,2,2)), DateTime.UtcNow.Add(new TimeSpan(1434,22,2)), DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertAppointment(new Appointment(-1,5,1,"Meeting EEE","Lorem Ipsum","Facetime","Billy Bob","Virtual","http://debug.com/",DateTime.UtcNow.Add(new TimeSpan(1436,2,2)), DateTime.UtcNow.Add(new TimeSpan(1436,22,2)), DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
        }
    }
}