using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Configuration; 

namespace CAI_LOGGER
{
    public class Connection 
    {
        //LIVE CONNECTION STRING
        //string ConnectionString = "Data Source=172.16.3.92;" +
        //                            "Database = SDA"+
        //                            "User id=sa;" +
        //                            "Password=adm1n$y$;";

        //test
        string ConnectionString = "Data Source=172.16.3.16;" +
                        "Database = PD;" +
                        "User id=sa;" +
                        "Password=adm1n$y$;";

        SqlConnection con;
        //Opens Connection
        public void OpenConnection()
        {
            con = new SqlConnection(ConnectionString); 
            con.Open();
        }
        //Closes Connection
        public void close()
        {
            con.Close();
        }
        public void ExecuteQuery(string Query)
        {
            SqlCommand cmd = new SqlCommand(Query, con);
            cmd.ExecuteNonQuery();
        }
        public DataTable GrdVw(string Query)
        {
            SqlDataAdapter adapter = new SqlDataAdapter(Query, ConnectionString);
            var commandBuilder = new SqlCommandBuilder(adapter);
            var ds = new DataSet();
            adapter.Fill(ds);

            return ds.Tables[0];
        }
        public string ExecuteScalar(string Query)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand(Query, conn))
            {
                conn.Open();
                string result = Convert.ToString(cmd.ExecuteScalar());
                conn.Close(); 

                return result;
            }
             
        }
        public SqlDataReader DataReader(string Query)
        {
            SqlCommand cmd = new SqlCommand(Query, con);
            SqlDataReader dr = cmd.ExecuteReader();
            return dr;
        }
        public void ExecuteProcedure(string procedure)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(procedure, con))
                {
                    command.Connection = con;
                    command.CommandTimeout = 10000; //in seconds

                    command.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    command.ExecuteNonQuery();
                }
            }

        } 
        //Returns all select statements from stored procedure as dataset
        public DataSet ReturnDS(string Query)
        {

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                DataSet dataset = new DataSet();
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = new SqlCommand(Query, conn);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapter.Fill(dataset);

                return dataset;
            }
        }
        //Make csv writer function for data tables
        public void tableToFile(DataTable table, string dest)
        {
            using (DataTable dt = table)
            {

                //Build the CSV file data as a Comma separated string.
                string csv = string.Empty;
                //string dest = @"Y:\Pog\INV\group\PCS_STOCK\" + destination + ".csv";

                //streamwriter
                StreamWriter fileDest = new StreamWriter(dest, false);

                foreach (DataColumn column in dt.Columns)
                {
                    //last column len
                    int columns = dt.Columns.Count;
                    //last column name
                    var colname = dt.Columns[columns - 1].ColumnName;


                    if (column.ColumnName == colname)
                    {
                        csv += column.ColumnName;
                    }
                    else
                    {
                        //Add the Header row for CSV file.
                        csv += column.ColumnName + ',';
                    }
                }

                //Add new line.
                //csv += "\r\n";
                fileDest.WriteLine(csv);



                foreach (DataRow row in dt.Rows)
                {
                    csv = "";
                    foreach (DataColumn column in dt.Columns)
                    {
                        //last column len
                        int columns = dt.Columns.Count;
                        //last column name
                        var colname = dt.Columns[columns - 1].ColumnName;
                        
                        if (column.ColumnName == colname)
                        {
                            csv += row[column.ColumnName].ToString().Replace(",", ";");
                        }
                        else
                        {
                            //Add the Data rows.
                            csv += row[column.ColumnName].ToString().Replace(",", ";") + ',';
                        }

                    }

                    //Add new line.
                    //csv += "\r\n";
                    fileDest.WriteLine(csv);

                }
                fileDest.Close();
            }
        }
        public List<string> LoadList(string select,string fieldtoselect)
        {
            var listOfPerson = new List<string>();

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string sql = select;
                using (var command = new SqlCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {  
                            listOfPerson.Add(reader[fieldtoselect].ToString());
                        }
                    }
                }
            }

            return listOfPerson;
        }
    }
}