using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;

namespace ProjectTemplate
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]

    public class ProjectServices : System.Web.Services.WebService
    {
        ////////////////////////////////////////////////////////////////////////
        ///replace the values of these variables with your database credentials
        ////////////////////////////////////////////////////////////////////////
        private string dbID = "devforce";
        private string dbPass = "!!Devforce440";
        private string dbName = "devforce";
        ////////////////////////////////////////////////////////////////////////

        ////////////////////////////////////////////////////////////////////////
        ///call this method anywhere that you need the connection string!
        ////////////////////////////////////////////////////////////////////////
        private string getConString() {
            return "SERVER=107.180.1.16; PORT=3306; DATABASE=" + dbName + "; UID=" + dbID + "; PASSWORD=" + dbPass + "; Convert Zero Datetime=true";
        }
        ////////////////////////////////////////////////////////////////////////



        /////////////////////////////////////////////////////////////////////////
        //don't forget to include this decoration above each method that you want
        //to be exposed as a web service!
        [WebMethod(EnableSession = true)]
        /////////////////////////////////////////////////////////////////////////
        public string TestConnection()
        {
            try
            {
                string testQuery = "select * from person";

                ////////////////////////////////////////////////////////////////////////
                ///here's an example of using the getConString method!
                ////////////////////////////////////////////////////////////////////////
                MySqlConnection con = new MySqlConnection(getConString());
                ////////////////////////////////////////////////////////////////////////

                MySqlCommand cmd = new MySqlCommand(testQuery, con);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable table = new DataTable();
                adapter.Fill(table);
                return "Success!";
            }
            catch (Exception e)
            {
                return "Something went wrong, please check your credentials and db name and try again.  Error: " + e.Message;
            }
        }
        [WebMethod(EnableSession = true)]
        public bool logOn(string uname, string pwd)
        {
            bool success = false;

            //set the sql query to pass through and assign to the string sqlSelect
            string sqlSelect = "SELECT userid FROM person WHERE email=@emailvalue AND password=@passvalue";

            //set the sql connection to be used
            MySqlConnection sqlConnection = new MySqlConnection(getConString());

            //the command to be sent through
            MySqlCommand sqlCommand = new MySqlCommand(sqlSelect, sqlConnection);

            //decode the values sent over from the sql server
            sqlCommand.Parameters.AddWithValue("@emailvalue", HttpUtility.UrlDecode(uname));
            sqlCommand.Parameters.AddWithValue("@passvalue", HttpUtility.UrlDecode(pwd));

            //put the values received into a table and see if they match, if they do then we change our boolean to true, then return the value of the boolean
            MySqlDataAdapter sqlDa = new MySqlDataAdapter(sqlCommand);
            DataTable sqlDt = new DataTable();

            sqlDa.Fill(sqlDt);
            if (sqlDt.Rows.Count > 0)
            {
                Session["userid"] = sqlDt.Rows[0]["userid"];
                success = true;
            }

            return success;
        }

        [WebMethod]
        public void RequestAccount(string email, string pass, string status)
        {
            //string sqlConnectString = System.Configuration.ConfigurationManager.ConnectionStrings["myDB"].ConnectionString;
            //the only thing fancy about this query is SELECT LAST_INSERT_ID() at the end.  All that
            //does is tell mySql server to return the primary key of the last inserted row.
            string sqlSelect = "insert into person (email, password, status) " +
                "values(@emailValue, @passValue, @statusValue); SELECT LAST_INSERT_ID();";

            MySqlConnection sqlConnection = new MySqlConnection(getConString());
            MySqlCommand sqlCommand = new MySqlCommand(sqlSelect, sqlConnection);

            sqlCommand.Parameters.AddWithValue("@emailValue", HttpUtility.UrlDecode(email));
            sqlCommand.Parameters.AddWithValue("@passValue", HttpUtility.UrlDecode(pass));
            sqlCommand.Parameters.AddWithValue("@statusValue", HttpUtility.UrlDecode(status));
            

            //this time, we're not using a data adapter to fill a data table.  We're just
            //opening the connection, telling our command to "executescalar" which says basically
            //execute the query and just hand me back the number the query returns (the ID, remember?).
            //don't forget to close the connection!
            sqlConnection.Open();
            //we're using a try/catch so that if the query errors out we can handle it gracefully
            //by closing the connection and moving on
            try
            {
                int accountID = Convert.ToInt32(sqlCommand.ExecuteScalar());
                //here, you could use this accountID for additional queries regarding
                //the requested account.  Really this is just an example to show you
                //a query where you get the primary key of the inserted row back from
                //the database!
            }
            catch (Exception e)
            {
            }
            sqlConnection.Close();
        }

        //method to get all requests from database that don't have a resolution
        [WebMethod(EnableSession = true)]
        public Request[] GetUnresolvedRequests()
        {
            //string to select all items from requests table that don't have resolution
            string sqlSelect = "select reqid, problem, solution, userid, department, datesubmitted, type, resolution FROM requests WHERE resolution is NULL;";

            //creates a table for us to load an array into
            DataTable sqlDt = new DataTable("requests");

            //standard sql connection variables
            MySqlConnection sqlConnection = new MySqlConnection(getConString());
            MySqlCommand sqlCommand = new MySqlCommand(sqlSelect, sqlConnection);

            MySqlDataAdapter sqlDa = new MySqlDataAdapter(sqlCommand);

            sqlDa.Fill(sqlDt);

            //creating the array of Request objects by looping through the database request table for all entries without a resolution already
            List<Request> requests = new List<Request>();
            for (int i = 0; i < sqlDt.Rows.Count; i++)
            {
                requests.Add(new Request
                {
                    requestID = Convert.ToInt32(sqlDt.Rows[i]["reqid"]),
                    problem = sqlDt.Rows[i]["problem"].ToString(),
                    solution = sqlDt.Rows[i]["solution"].ToString(),
                    userID = Convert.ToInt32(sqlDt.Rows[i]["userid"]),
                    department = sqlDt.Rows[i]["department"].ToString(),
                    date = sqlDt.Rows[i]["datesubmitted"].ToString(),
                    type = sqlDt.Rows[i]["type"].ToString(),
                    resolution = sqlDt.Rows[i]["resolution"].ToString()
                });
            }

            //returns the array of the Request objects
            return requests.ToArray();


        }

        [WebMethod(EnableSession = true)]
        public string AddResolution(string id, string resolution)
        {
            //sql command to update the resoltuion field to the provided resolution value where the request id value
            //matches the provided id value
            string sqlSelect = "update requests set resolution=@resolutionValue where reqid=@idValue;";

            MySqlConnection sqlConnection = new MySqlConnection(getConString());
            MySqlCommand sqlCommand = new MySqlCommand(sqlSelect, sqlConnection);

            //uses the provided values to update the sql command
            sqlCommand.Parameters.AddWithValue("@resolutionValue", HttpUtility.UrlDecode(resolution));
            sqlCommand.Parameters.AddWithValue("@idValue", HttpUtility.UrlDecode(id));

            //open the connection to sql database
            sqlConnection.Open();

            //try - catch block 
            try
            {
                sqlCommand.ExecuteNonQuery();
                return "Success!";
            }
            catch(Exception e)
            {
                return "ERROR!!" + e.Message;
            }

            sqlConnection.Close();
        }

        [WebMethod(EnableSession = true)]
        public string DeleteUser(string id)
        {
            //sql command to delete the user in question identified by id number
            string sqlSelect = "delete from person where userid=@idValue;";

            MySqlConnection sqlConnection = new MySqlConnection(getConString());
            MySqlCommand sqlCommand = new MySqlCommand(sqlSelect, sqlConnection);

            //update the value in the sql command with the provided value to the method
            sqlCommand.Parameters.AddWithValue("@idValue", HttpUtility.UrlDecode(id));

            //open the connection to sql database
            sqlConnection.Open();

            //try-catch block
            try
            {
                sqlCommand.ExecuteNonQuery();
                return "User successfully deleted!";
            }
            catch(Exception e)
            {
                return "ERROR!!" + e.Message;
            }
        }
        
        [WebMethod(EnableSession = true)]
        public Request GetRequestByID(string id)
        {
            string sqlSelect = "select reqid, problem, solution, userid, department, datesubmitted, type, resolution from requests WHERE reqid=@reqidValue;";

            MySqlConnection sqlConnection = new MySqlConnection(getConString());
            MySqlCommand sqlCommand = new MySqlCommand(sqlSelect, sqlConnection);

            DataTable sqlDt = new DataTable("request");

            sqlCommand.Parameters.AddWithValue("@reqidValue", HttpUtility.UrlDecode(id));

            MySqlDataAdapter sqlDa = new MySqlDataAdapter(sqlCommand);

            sqlDa.Fill(sqlDt);

            Request req1 = new Request();

            for (int i = 0; i < sqlDt.Rows.Count; i++)
            {

                req1.requestID = Convert.ToInt32(sqlDt.Rows[i]["reqid"]);
                req1.problem = sqlDt.Rows[i]["problem"].ToString();
                req1.solution = sqlDt.Rows[i]["solution"].ToString();
                req1.userID = Convert.ToInt32(sqlDt.Rows[i]["userid"]);
                req1.department = sqlDt.Rows[i]["department"].ToString();
                req1.date = sqlDt.Rows[i]["datesubmitted"].ToString();
                req1.type = sqlDt.Rows[i]["type"].ToString();
                req1.resolution = sqlDt.Rows[i]["resolution"].ToString();
                
            }

            return req1;
                                                  
        }

    }
}
