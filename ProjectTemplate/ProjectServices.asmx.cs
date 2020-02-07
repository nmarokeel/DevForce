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
            return "SERVER=107.180.1.16; PORT=3306; DATABASE=" + dbName + "; UID=" + dbID + "; PASSWORD=" + dbPass;
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
                string testQuery = "select * from users";

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
            string sqlSelect = "SELECT userid FROM users WHERE email=@emailvalue AND password=@passvalue";

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
    }
}
