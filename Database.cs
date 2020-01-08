using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using ClosedXML.Excel;

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

        private static string fileName = "data.xlsx";

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

        public bool RefreshData()
        {
            try
            {
                using (var client = new System.Net.WebClient())
                {
                    // Download the updated Excel file
                    client.DownloadFile("https://docs.google.com/spreadsheets/d/1D-8P4mfhVw-TNQnyzkJT2aPVxSQSIOnu_AR-AXWAGxk/export?format=xlsx", fileName);
                    
                    // Clear the data table
                    using (var clearCommand = new SqlCommand("ClearRawData", connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    })
                    {
                        clearCommand.ExecuteNonQuery();

                        // Add the updated data
                        using (var addCommand = new SqlCommand("AddRawData", connection)
                        {
                            CommandType = CommandType.StoredProcedure
                        }) {
                            addCommand.Parameters.Add(new SqlParameter("@row", SqlDbType.NVarChar));
                            addCommand.Prepare();

                            var rows = ReadFile();

                            foreach (var row in rows)
                            {
                                string rowString = string.Join(",", row);
                                addCommand.Parameters[0].Value = rowString;
                                addCommand.ExecuteNonQuery();
                            }
                        }

                        return true;
                    }
                }
            } catch (Exception e)
            {
                Console.WriteLine("Exception caught: {0}", e);
                return false;
            } 
        }

        internal static List<List<string>> ReadFile()
        {
            XLDataType[] stringTypes =
                { XLDataType.Text, XLDataType.DateTime };

            List<string> cols = new List<string>();
            List<List<string>> rows = new List<List<string>>();

            using (XLWorkbook excelFile =
                XLWorkbook.OpenFromTemplate(fileName))
            {
                IXLWorksheet firstSheet = excelFile.Worksheets.FirstOrDefault();

                if (firstSheet != null)
                {
                    // Skip the first row and read data
                    foreach (IXLRow currentRow in firstSheet.Rows().Skip(1))
                    {
                        List<string> values = new List<string>();

                        foreach (IXLCell currentCell in currentRow.Cells())
                        {
                            // Make it SQL Proof from reading
                            if (currentCell.Value.ToString().Trim().Equals(""))
                                values.Add("NULL");
                            else
                                values.Add(stringTypes.Contains(currentCell.DataType)
                                    ? $"N'{currentCell.Value}'"
                                    : currentCell.Value.ToString());
                        }

                        rows.Add(values);
                    }
                }
            }

            return rows;
        }
    }
}
