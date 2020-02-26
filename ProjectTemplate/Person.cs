using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectTemplate
{
    
    public class Person
    {
        private string firstName, lastName, email, password;
        private int userID;
        private bool canApproveAccRq, isWhiteList;

        public Person(string fristName, string lastName, string email, string password, int userID)
        {
            FirstName = fristName;
            LastName = lastName;
            Email = email;
            Password = password;
            UserID = userID;
            Approval();
        }

        public Person() { }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int UserID { get; set; }
        public bool CanApproveAccRq { get => canApproveAccRq; set => canApproveAccRq = false; }
        public bool IsWhiteList { get => isWhiteList; set => isWhiteList = false; }

        protected virtual void Approval()
        {
            canApproveAccRq = false;
        }

        static protected string[] statusus = new string[] { "admin", "manager", "user" };
    }

    public class Admin : Person
    {
        protected string Status { get; set; }
        public Admin(string fristName, string lastName, string email, string password, int userID)
        {
            Status = Person.statusus[0];

        }

        protected override void Approval()
        {
            CanApproveAccRq = true;
            IsWhiteList = true;
        }

        public 
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