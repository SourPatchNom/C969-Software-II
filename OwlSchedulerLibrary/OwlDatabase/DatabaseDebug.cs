using System;
using MySql.Data.MySqlClient;
using OwlSchedulerLibrary.OwlSchedule.Classes;

namespace OwlSchedulerLibrary.OwlDatabase
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
            DatabaseHandler.Instance.InsertCountry(new Country(-1, "Australia", DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));

            DatabaseHandler.Instance.InsertCity(new City(-1, "Dallas", 1, DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertCity(new City(-1, "Edinburgh", 2, DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertCity(new City(-1, "Paris", 3, DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertCity(new City(-1, "London", 4, DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertCity(new City(-1, "Tokyo", 5, DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertCity(new City(-1, "Austin", 1, DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertCity(new City(-1, "Houston", 1, DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            
            DatabaseHandler.Instance.InsertAddress(new Address(-1, "12", "Main Street", 1, "88888", "18888888888", DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertAddress(new Address(-1, "34", "Peach Highway", 2, "88888", "18888888888", DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertAddress(new Address(-1, "56", "Grape Lane", 3, "88888", "18888888888", DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertAddress(new Address(-1, "78", "Apple Street", 4, "88888", "18888888888", DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertAddress(new Address(-1, "91", "Orange Street", 5, "88888", "18888888888", DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertAddress(new Address(-1, "121", "Austin Street", 6, "88888", "18888888888", DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertAddress(new Address(-1, "232", "Dallas Street", 1, "88888", "18888888888", DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));

            DatabaseHandler.Instance.InsertCustomer(new Customer(-1,"George Washington",1,true,DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertCustomer(new Customer(-1,"Thomas Jefferson",2,true,DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertCustomer(new Customer(-1,"Ben Franklin",3,true,DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertCustomer(new Customer(-1,"Albert Einstein",4,false,DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertCustomer(new Customer(-1,"John Wick",5,true,DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));

            DatabaseHandler.Instance.InsertAppointment(new Appointment(-1,1,1,"Day 1 A","Lorem Ipsum","Facetime","Billy Bob","In Person","http://debug.com/",DateTime.Now.Date.AddHours(0).AddHours(8).AddMinutes(0),DateTime.Now.Date.AddHours(0).AddHours(8).AddMinutes(15),DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertAppointment(new Appointment(-1,1,1,"Day 1 B","Lorem Ipsum","Facetime","Billy Bob","In Person","http://debug.com/",DateTime.Now.Date.AddHours(0).AddHours(9).AddMinutes(0),DateTime.Now.Date.AddHours(0).AddHours(9).AddMinutes(15),DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertAppointment(new Appointment(-1,1,1,"Day 1 C","Lorem Ipsum","Facetime","Billy Bob","Virtual","http://debug.com/",DateTime.Now.Date.AddHours(0).AddHours(10).AddMinutes(0),DateTime.Now.Date.AddHours(0).AddHours(10).AddMinutes(15),DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertAppointment(new Appointment(-1,1,1,"Day 1 D","Lorem Ipsum","Facetime","Billy Bob","Virtual","http://debug.com/",DateTime.Now.Date.AddHours(0).AddHours(11).AddMinutes(0),DateTime.Now.Date.AddHours(0).AddHours(11).AddMinutes(15),DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertAppointment(new Appointment(-1,1,1,"Day 1 E","Lorem Ipsum","Facetime","Billy Bob","Virtual","http://debug.com/",DateTime.Now.Date.AddHours(0).AddHours(12).AddMinutes(0),DateTime.Now.Date.AddHours(0).AddHours(12).AddMinutes(15),DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertAppointment(new Appointment(-1,1,1,"Day 1 F","Lorem Ipsum","Facetime","Billy Bob","Virtual","http://debug.com/",DateTime.Now.Date.AddHours(0).AddHours(14).AddMinutes(0),DateTime.Now.Date.AddHours(0).AddHours(14).AddMinutes(15),DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertAppointment(new Appointment(-1,1,1,"Day 1 G","Lorem Ipsum","Facetime","Billy Bob","Virtual","http://debug.com/",DateTime.Now.Date.AddHours(0).AddHours(13).AddMinutes(0),DateTime.Now.Date.AddHours(0).AddHours(13).AddMinutes(15),DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            
            DatabaseHandler.Instance.InsertAppointment(new Appointment(-1,1,1,"Day 2 A","Lorem Ipsum","Facetime","Billy Bob","Virtual","http://debug.com/",DateTime.Now.Date.AddHours(24).AddHours(8).AddMinutes(0),DateTime.Now.Date.AddHours(24).AddHours(8).AddMinutes(15),DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertAppointment(new Appointment(-1,1,1,"Day 2 B","Lorem Ipsum","Facetime","Billy Bob","Virtual","http://debug.com/",DateTime.Now.Date.AddHours(24).AddHours(9).AddMinutes(0),DateTime.Now.Date.AddHours(24).AddHours(9).AddMinutes(15),DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertAppointment(new Appointment(-1,1,1,"Day 2 C","Lorem Ipsum","Facetime","Billy Bob","Virtual","http://debug.com/",DateTime.Now.Date.AddHours(24).AddHours(10).AddMinutes(0),DateTime.Now.Date.AddHours(24).AddHours(10).AddMinutes(15),DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertAppointment(new Appointment(-1,1,1,"Day 2 D","Lorem Ipsum","Facetime","Billy Bob","Virtual","http://debug.com/",DateTime.Now.Date.AddHours(24).AddHours(11).AddMinutes(0),DateTime.Now.Date.AddHours(24).AddHours(11).AddMinutes(15),DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertAppointment(new Appointment(-1,1,1,"Day 2 E","Lorem Ipsum","Facetime","Billy Bob","Virtual","http://debug.com/",DateTime.Now.Date.AddHours(24).AddHours(12).AddMinutes(0),DateTime.Now.Date.AddHours(24).AddHours(12).AddMinutes(15),DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertAppointment(new Appointment(-1,1,1,"Day 2 F","Lorem Ipsum","Facetime","Billy Bob","Virtual","http://debug.com/",DateTime.Now.Date.AddHours(24).AddHours(13).AddMinutes(0),DateTime.Now.Date.AddHours(24).AddHours(14).AddMinutes(15),DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertAppointment(new Appointment(-1,1,1,"Day 2 G","Lorem Ipsum","Facetime","Billy Bob","Virtual","http://debug.com/",DateTime.Now.Date.AddHours(24).AddHours(14).AddMinutes(0),DateTime.Now.Date.AddHours(24).AddHours(13).AddMinutes(15),DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            
            DatabaseHandler.Instance.InsertAppointment(new Appointment(-1,1,1,"Day 2 A","Lorem Ipsum","Facetime","Billy Bob","Virtual","http://debug.com/",DateTime.Now.Date.AddDays(7).AddHours(24).AddHours(8).AddMinutes(0),DateTime.Now.Date.AddDays(7).AddHours(24).AddHours(8).AddMinutes(15),DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertAppointment(new Appointment(-1,1,1,"Day 2 B","Lorem Ipsum","Facetime","Billy Bob","Virtual","http://debug.com/",DateTime.Now.Date.AddDays(7).AddHours(24).AddHours(9).AddMinutes(0),DateTime.Now.Date.AddDays(7).AddHours(24).AddHours(9).AddMinutes(15),DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertAppointment(new Appointment(-1,1,1,"Day 2 C","Lorem Ipsum","Facetime","Billy Bob","Virtual","http://debug.com/",DateTime.Now.Date.AddDays(7).AddHours(24).AddHours(10).AddMinutes(0),DateTime.Now.Date.AddDays(7).AddHours(24).AddHours(10).AddMinutes(15),DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertAppointment(new Appointment(-1,1,1,"Day 2 D","Lorem Ipsum","Facetime","Billy Bob","Virtual","http://debug.com/",DateTime.Now.Date.AddDays(7).AddHours(24).AddHours(11).AddMinutes(0),DateTime.Now.Date.AddDays(7).AddHours(24).AddHours(11).AddMinutes(15),DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertAppointment(new Appointment(-1,1,1,"Day 2 E","Lorem Ipsum","Facetime","Billy Bob","Virtual","http://debug.com/",DateTime.Now.Date.AddDays(7).AddHours(24).AddHours(12).AddMinutes(0),DateTime.Now.Date.AddDays(7).AddHours(24).AddHours(12).AddMinutes(15),DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertAppointment(new Appointment(-1,1,1,"Day 2 F","Lorem Ipsum","Facetime","Billy Bob","Virtual","http://debug.com/",DateTime.Now.Date.AddDays(7).AddHours(24).AddHours(13).AddMinutes(0),DateTime.Now.Date.AddDays(7).AddHours(24).AddHours(14).AddMinutes(15),DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertAppointment(new Appointment(-1,1,1,"Day 2 G","Lorem Ipsum","Facetime","Billy Bob","Virtual","http://debug.com/",DateTime.Now.Date.AddDays(7).AddHours(24).AddHours(14).AddMinutes(0),DateTime.Now.Date.AddDays(7).AddHours(24).AddHours(13).AddMinutes(15),DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            
            DatabaseHandler.Instance.InsertAppointment(new Appointment(-1,1,1,"Day 2 A","Lorem Ipsum","Facetime","Billy Bob","Virtual","http://debug.com/",DateTime.Now.Date.AddDays(30).AddHours(24).AddHours(8).AddMinutes(0),DateTime.Now.Date.AddDays(30).AddHours(24).AddHours(8).AddMinutes(15),DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertAppointment(new Appointment(-1,1,1,"Day 2 B","Lorem Ipsum","Facetime","Billy Bob","Virtual","http://debug.com/",DateTime.Now.Date.AddDays(30).AddHours(24).AddHours(9).AddMinutes(0),DateTime.Now.Date.AddDays(30).AddHours(24).AddHours(9).AddMinutes(15),DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertAppointment(new Appointment(-1,1,1,"Day 2 C","Lorem Ipsum","Facetime","Billy Bob","Virtual","http://debug.com/",DateTime.Now.Date.AddDays(30).AddHours(24).AddHours(10).AddMinutes(0),DateTime.Now.Date.AddDays(30).AddHours(24).AddHours(10).AddMinutes(15),DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertAppointment(new Appointment(-1,1,1,"Day 2 D","Lorem Ipsum","Facetime","Billy Bob","Virtual","http://debug.com/",DateTime.Now.Date.AddDays(30).AddHours(24).AddHours(11).AddMinutes(0),DateTime.Now.Date.AddDays(30).AddHours(24).AddHours(11).AddMinutes(15),DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertAppointment(new Appointment(-1,1,1,"Day 2 E","Lorem Ipsum","Facetime","Billy Bob","Virtual","http://debug.com/",DateTime.Now.Date.AddDays(30).AddHours(24).AddHours(12).AddMinutes(0),DateTime.Now.Date.AddDays(30).AddHours(24).AddHours(12).AddMinutes(15),DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertAppointment(new Appointment(-1,1,1,"Day 2 F","Lorem Ipsum","Facetime","Billy Bob","Virtual","http://debug.com/",DateTime.Now.Date.AddDays(30).AddHours(24).AddHours(13).AddMinutes(0),DateTime.Now.Date.AddDays(30).AddHours(24).AddHours(14).AddMinutes(15),DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            DatabaseHandler.Instance.InsertAppointment(new Appointment(-1,1,1,"Day 2 G","Lorem Ipsum","Facetime","Billy Bob","Virtual","http://debug.com/",DateTime.Now.Date.AddDays(30).AddHours(24).AddHours(14).AddMinutes(0),DateTime.Now.Date.AddDays(30).AddHours(24).AddHours(13).AddMinutes(15),DateTime.UtcNow, "Admin", DateTime.UtcNow, "Admin"));
            
        }
    }
}