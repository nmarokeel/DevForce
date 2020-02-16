using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectTemplate
{
    
    public class Person
    {
        private string fristName, lastName, email, password;
        private int userID;

        public Person(string fristName, string lastName, string email, string password, int userID)
        {
            FristName = fristName;
            LastName = lastName;
            Email = email;
            Password = password;
            UserID = userID;
        }

        public Person() { }

        public string FristName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int UserID { get; set; }

        static protected string[] statusus = new string[] { "admin", "manager", "user" };
    }

    public class Admin : Person
    {
        protected string Status { get; set; }
        public Admin(string fristName, string lastName, string email, string password, int userID)
        {
            Status = Person.statusus[0];
        }
    }
    public class Manager : Person
    {
        protected string Status { get; set; }
        public Manager(string fristName, string lastName, string email, string password, int userID)
        {
            Status = Person.statusus[1];
        }
    }
    public class User : Person
    {
        protected string Status { get; set; }
        public User(string fristName, string lastName, string email, string password, int userID)
        {
            Status = Person.statusus[2];
        }
    }
}