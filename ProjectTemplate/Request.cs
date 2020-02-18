using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectTemplate
{
    public class Request
    {
        private int requestID;
        private string problem;
        private string solution;
        private int userID;
        private string department;
        private string date;
        private string type;
        private string resolution;

        public int RequestID { get => requestID; set => requestID = value; }
        public string Problem { get => problem; set => problem = value; }
        public string Solution { get => solution; set => solution = value; }
        public int UserID { get => userID; set => userID = value; }
        public string Department { get => department; set => department = value; }
        public string Date { get => date; set => date = value; }
        public string Type { get => type; set => type = value; }
        public string Resolution { get => resolution; set => resolution = value; }
    }
}