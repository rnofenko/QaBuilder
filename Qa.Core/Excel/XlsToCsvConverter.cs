using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using Qa.Core.System;

namespace Qa.Core.Excel
{
    public class XlsToCsvConverter
    {
        public void ConvertAll(string folder)
        {
            var files = Directory.GetFiles(folder, "*.xls");
            foreach (var file in files)
            {
                try
                {
                    Lo.W(Path.GetFileName(file) + "\t\t\t\t - ");
                    Convert(file, file.Replace("xls", "csv"));
                    Lo.Wl("succeed", ConsoleColor.Green);
                }
                catch (Exception exception)
                {
                    Lo.Wl("fail", ConsoleColor.Red);
                    Lo.Wl(exception.Message);
                }
            }
        }

        public void Convert(string excelFilePath, string csvOutputFile, int worksheetNumber = 1)
        {
            if (!File.Exists(excelFilePath)) throw new FileNotFoundException(excelFilePath);
            if (File.Exists(csvOutputFile))
            {
                File.Delete(csvOutputFile);
            }
            
            var connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=\"Excel 8.0;IMEX=1;HDR=NO\"", excelFilePath);
            var connection = new OleDbConnection(connectionString);
            
            var dt = new DataTable();
            try
            {
                connection.Open();
                var schemaTable = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                if (schemaTable ==null || schemaTable.Rows.Count < worksheetNumber) throw new ArgumentException("The worksheet number provided cannot be found in the spreadsheet");
                var worksheet = schemaTable.Rows[worksheetNumber - 1]["table_name"].ToString().Replace("'", "");
                var sql = string.Format("select * from [{0}]", worksheet);
                var da = new OleDbDataAdapter(sql, connection);
                da.Fill(dt);
            }
            finally
            {
                connection.Close();
            }

            using (var writer = new StreamWriter(csvOutputFile))
            {
                foreach (DataRow row in dt.Rows)
                {
                    var firstLine = true;
                    foreach (DataColumn col in dt.Columns)
                    {
                        if (!firstLine) { writer.Write(","); } else { firstLine = false; }
                        var data = row[col.ColumnName].ToString().Replace("\"", "\"\"");
                        writer.Write(string.Format("\"{0}\"", data));
                    }
                    writer.WriteLine();
                }
            }
        }
    }
}
