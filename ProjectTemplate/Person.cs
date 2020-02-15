using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectTemplate
{
    public class Person
    {
        private string fristName, lastName, email, password, status;
        private int userID;

        public Person(string fristName, string lastName, string email, string password, string status, int userID)
        {
            this.fristName = fristName;
            this.lastName = lastName;
            this.email = email;
            this.password = password;
            this.status = status;
            this.userID = userID;
        }

        public string FristName { get => fristName; set => fristName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public string Email { get => email; set => email = value; }
        public string Password { get => password; set => password = value; }
        public string Status { get => status; set => status = value; }
        public int UserID { get => userID; set => userID = value; }
    }
}