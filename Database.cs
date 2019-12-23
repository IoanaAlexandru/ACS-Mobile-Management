using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;

namespace ACS_Form
{
    public class UserInput
    {
        public string name { get; }
        public string info { get; }
        public string input { get; }

        public UserInput(string name, string info, string input)
        {
            this.name = name;
            this.info = info;
            this.input = input;
        }
    }

    public class Database
{
        private static string user = "user";
        private static string password = "P@ssw0rd";
        private static string connectionString = $"Server=tcp:acs-app.database.windows.net,1433;Initial Catalog=Forms;Persist Security Info=False;User ID={user};Password={password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        private SqlConnection connection;

        public Database()
        {
            connection = new SqlConnection(connectionString);
            connection.Open();
        }

        public List<UserInput> GetUserInput()
        {
            var inputs = new List<UserInput>();
            using (var command = new SqlCommand("GetUserInput", connection)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read()) {
                    string name = reader["Name"].ToString();
                    string info = reader["Info"].ToString();
                    string input = reader["Input"].ToString();
                    inputs.Add(new UserInput(name, info, input));
                }
            }
            return inputs;
        }
}
}
