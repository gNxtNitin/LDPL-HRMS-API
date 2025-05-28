using System.Data;
using System;
using System.Data.SqlClient;
using System.Collections;
using System.Text;

namespace DatabaseManager
{
    public class DAL
    {
        private readonly string? _connectionString;
        //static string? dbConnStr;

        public DAL(string? connectionString)
        {
            _connectionString = connectionString;
            //dbConnStr = connectionString;
        }
        private SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
        
        static readonly string dbConnStr = "Server=DESKTOP-P9QLVJS;Database=UserManagement;Trusted_Connection=True;MultipleActiveResultSets=True";

        // Method to execute a stored procedure that returns a result set
        public DataTable ExecuteStoredProcedure(string procedureName, SqlParameter[] parameters = null)
        {
            using (SqlConnection connection = GetConnection())
            {
                using (SqlCommand command = new SqlCommand(procedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    connection.Open();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }
        }

        // Method to execute a stored procedure that does not return a result set (e.g., insert, update, delete)
        public void ExecuteNonQueryStoredProcedure(string procedureName, SqlParameter[] parameters)
        {
            using (SqlConnection connection = GetConnection())
            {
                using (SqlCommand command = new SqlCommand(procedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }


        public void ExecuteStoredProcedureNonQueryWithOutput(string procedureName, SqlParameter[] parameters)
        {
            using (SqlConnection connection = GetConnection())
            {
                using (SqlCommand command = new SqlCommand(procedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    connection.Open();
                    command.ExecuteNonQuery();

                    // Check for output parameters and read their values after execution
                    //foreach (SqlParameter parameter in command.Parameters)
                    //{
                    //    if (parameter.Direction == ParameterDirection.Output || parameter.Direction == ParameterDirection.InputOutput)
                    //    {
                    //        Console.WriteLine($"Output value for {parameter.ParameterName}: {parameter.Value}");
                    //    }
                    //}
                }
            }
        }

        public DataTable ExecuteStoredProcedureWithOutput(string procedureName, SqlParameter[] parameters)
        {
            using (SqlConnection connection = GetConnection())
            {
                using (SqlCommand command = new SqlCommand(procedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    connection.Open();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }
        }

        // ExecuteNonQuery method for stored procedures with input/output parameters
        // public DbReturnList ExecuteNonQuery(string storedProcedureName, SqlParameter[] parameters, out object retValue, out object errormsgValue, string outputParamName, string outputParamName2)
        public DbReturnList ExecuteNonQuery(string storedProcedureName, SqlParameter[] parameters, string outputParamName, string outputParamName2)
        {
            try
            {
                DbReturnList dbReturnList = new DbReturnList();
                using (SqlConnection connection = new SqlConnection(dbConnStr))
                {
                    using (SqlCommand cmd = new SqlCommand(storedProcedureName, connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Add parameters to the command
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }
                        connection.Open();
                        // ExecuteNonQuery and return number of affected rows
                        int rowsAffected = cmd.ExecuteNonQuery();
                        // Retrieve the value of the output parameter
                        //retValue = cmd.Parameters[outputParamName].Value;
                        //errormsgValue = cmd.Parameters[outputParamName2].Value;
                        dbReturnList.Ret = Convert.ToInt32(cmd.Parameters["@ret"].Value);
                        dbReturnList.ErrorMsg = cmd.Parameters["@errormsg"].Value.ToString();
                        //return rowsAffected;
                        return dbReturnList;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception (log or rethrow)
                throw new Exception("An error occurred while executing the stored procedure.", ex);
            }
        }
        internal class SPArgBuild
        {
            internal string parameterName = "";
            internal string parameterValue = "";
            /// <summary>
            /// Write full data type, such as SqlDBType.VarChar.
            /// </summary>
            internal string pramValueType = "";
            internal string parameterDirection = "";
            /// <summary>
            /// Use to create SP Argument Build conestruction.
            /// </summary>
            /// <param name="pramName">SP Argument Parameter Name.</param>
            /// <param name="pramValue">SP Argument Parameter Value.</param>
            internal SPArgBuild(string pramName, string pramValue, string pramValueType, string parameterDirection)
            {
                this.parameterName = pramName;
                this.parameterValue = pramValue;
                this.pramValueType = pramValueType;
                this.parameterDirection = parameterDirection;
            }
        }

        /// <summary>
        /// This function built an Array List, which is collection of some SP parameter's Name, Value and Data type.
        /// </summary>
        /// <param name="arrLst">Array List which will store all argument.</param>
        /// <param name="spParmName">SP Argument Parameter Name.</param>
        /// <param name="spParmValue">SP Argument Parameter Value.</param>
        /// <param name="spPramValueType">Parameter value type EXACTLY same as SqlDBType. E.g. 'SqlDbType.BigInt' will 'BigInt'. </param>
        /// <returns></returns>
        public static ArrayList spArgumentsCollection(ArrayList arrLst, string spParmName, string spParmValue, string spPramValueType, string parameterDirection)
        {
            SPArgBuild spArgBuiltObj = new SPArgBuild(spParmName, spParmValue, spPramValueType, parameterDirection);
            arrLst.Add(spArgBuiltObj);
            return arrLst;
        }

        /// <summary>
        /// Run a stored procedure of Select SQL type.
        /// </summary>
        /// <param name="ds">DataSet which will return after filling Data</param>
        /// <param name="spName">Stored Procedure Name</param>
        /// <param name="spPramArrList">Parameters in ArrayList</param>
        /// <returns>Return DataSet after filing data by SQL.</returns>
        public static DataSet RunStoredProcedure(DataSet ds, string spName, ArrayList spPramArrList)
        {
            SqlConnection conn = new SqlConnection(dbConnStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand();

            cmd = ArrangeParamter(spPramArrList);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = conn;
            cmd.CommandText = spName;
            SqlDataAdapter adap = new SqlDataAdapter(cmd);
            adap.Fill(ds);
            conn.Close();
            return ds;

        }

        /// <summary>
        /// Run a stored procedure which will execure some nonquery SQL.
        /// </summary>
        /// <param name="spName">Stored Procedure Name</param>
        /// <param name="spPramArrList">Parameters in a ArrayList</param>
        public static void RunStoredProcedure(string spName, ArrayList spPramArrList)
        {
            SqlConnection conn = new SqlConnection(dbConnStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = ArrangeParamter(spPramArrList);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = conn;
            cmd.CommandText = spName;
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public static int RunStoredProcedureRet(string spName, ArrayList spPramArrList)
        {
            SqlConnection conn = new SqlConnection(dbConnStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = ArrangeParamter(spPramArrList);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = conn;
            cmd.CommandText = spName;
            cmd.ExecuteNonQuery();
            int ret = Convert.ToInt32(cmd.Parameters["@ret"].Value);
            conn.Close();
            return ret;
        }
        public static DbReturnList RunStoredProcedureDsRetError(string spName, ArrayList spPramArrList, DataSet ds = null)
        {
            DbReturnList dbReturnList = new DbReturnList();
            SqlConnection conn = new SqlConnection(dbConnStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = ArrangeParamter(spPramArrList);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = conn;
            cmd.CommandText = spName;

            if (ds!=null && ds is DataSet)
            {
                SqlDataAdapter adap = new SqlDataAdapter(cmd);
                adap.Fill(ds);
            }
            else
            {
                cmd.ExecuteNonQuery();
            }

            dbReturnList.Ret = Convert.ToInt32(cmd.Parameters["@ret"].Value);
            dbReturnList.ErrorMsg = cmd.Parameters["@errormsg"].Value.ToString();
            conn.Close();
            return dbReturnList;
        }


        public static DbReturnList RunStoredProcedureRetError(string spName, ArrayList spPramArrList)
        {
            DbReturnList dbReturnList = new DbReturnList();
            SqlConnection conn = new SqlConnection(dbConnStr);
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = ArrangeParamter(spPramArrList);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = conn;
            cmd.CommandText = spName;
            cmd.ExecuteNonQuery();
            dbReturnList.Ret = Convert.ToInt32(cmd.Parameters["@ret"].Value);
            dbReturnList.ErrorMsg = cmd.Parameters["@errormsg"].Value.ToString();
            conn.Close();
            return dbReturnList;
        }

        internal static SqlCommand ArrangeParamter(ArrayList spPramArrList)
        {
            SqlCommand cmd = new SqlCommand();
            string spPramName = "";
            string spPramValue = "";
            string spPramDataType = "";
            string spParameterDirection = "";
            for (int i = 0; i < spPramArrList.Count; i++)
            {
                spPramName = ((SPArgBuild)spPramArrList[i]).parameterName;
                spPramValue = ((SPArgBuild)spPramArrList[i]).parameterValue;
                spPramDataType = ((SPArgBuild)spPramArrList[i]).pramValueType;
                spParameterDirection = ((SPArgBuild)spPramArrList[i]).parameterDirection;
                SqlParameter pram = null;
                #region SQL DB TYPE AND VALUE ASSIGNMENT
                switch (spPramDataType.ToUpper())
                {
                    case "BIGINT":
                        pram = cmd.Parameters.Add(spPramName, SqlDbType.BigInt);
                        pram.Value = spPramValue;
                        break;

                    case "BINARY":
                        pram = cmd.Parameters.Add(spPramName, SqlDbType.Binary);
                        pram.Value = spPramValue;
                        break;

                    case "BIT":
                        pram = cmd.Parameters.Add(spPramName, SqlDbType.Bit);
                        pram.Value = spPramValue;
                        break;

                    case "CHAR":
                        pram = cmd.Parameters.Add(spPramName, SqlDbType.Char);
                        pram.Value = spPramValue;
                        break;

                    case "DATETIME":
                        pram = cmd.Parameters.Add(spPramName, SqlDbType.DateTime);
                        pram.Value = spPramValue;
                        break;

                    case "DECIMAL":
                        pram = cmd.Parameters.Add(spPramName, SqlDbType.Decimal);
                        pram.Value = spPramValue;
                        break;

                    case "FLOAT":
                        pram = cmd.Parameters.Add(spPramName, SqlDbType.Float);
                        pram.Value = spPramValue;
                        break;

                    case "IMAGE":
                        pram = cmd.Parameters.Add(spPramName, SqlDbType.Image);
                        pram.Value = spPramValue;
                        break;

                    case "INT":
                        pram = cmd.Parameters.Add(spPramName, SqlDbType.Int);
                        pram.Value = spPramValue;
                        break;

                    case "MONEY":
                        pram = cmd.Parameters.Add(spPramName, SqlDbType.Money);
                        pram.Value = spPramValue;
                        break;

                    case "NCHAR":
                        pram = cmd.Parameters.Add(spPramName, SqlDbType.NChar);
                        pram.Value = spPramValue;
                        break;

                    case "NTEXT":
                        pram = cmd.Parameters.Add(spPramName, SqlDbType.NText);
                        pram.Value = spPramValue;
                        break;

                    case "NVARCHAR":
                        pram = cmd.Parameters.Add(spPramName, SqlDbType.NVarChar);
                        pram.Value = spPramValue;
                        break;

                    case "REAL":
                        pram = cmd.Parameters.Add(spPramName, SqlDbType.Real);
                        pram.Value = spPramValue;
                        break;

                    case "SMALLDATETIME":
                        pram = cmd.Parameters.Add(spPramName, SqlDbType.SmallDateTime);
                        pram.Value = spPramValue;
                        break;

                    case "SMALLINT":
                        pram = cmd.Parameters.Add(spPramName, SqlDbType.SmallInt);
                        pram.Value = spPramValue;
                        break;

                    case "SMALLMONEY":
                        pram = cmd.Parameters.Add(spPramName, SqlDbType.SmallMoney);
                        pram.Value = spPramValue;
                        break;

                    case "TEXT":
                        pram = cmd.Parameters.Add(spPramName, SqlDbType.Text);
                        pram.Value = spPramValue;
                        break;

                    case "TIMESTAMP":
                        pram = cmd.Parameters.Add(spPramName, SqlDbType.Timestamp);
                        pram.Value = spPramValue;
                        break;

                    case "TINYINT":
                        pram = cmd.Parameters.Add(spPramName, SqlDbType.TinyInt);
                        pram.Value = spPramValue;
                        break;

                    case "UDT":
                        pram = cmd.Parameters.Add(spPramName, SqlDbType.Udt);
                        pram.Value = spPramValue;
                        break;

                    case "UNIQUEIDENTIFIER":
                        pram = cmd.Parameters.Add(spPramName, SqlDbType.UniqueIdentifier);
                        pram.Value = spPramValue;
                        break;

                    case "VARBINARY":
                        pram = cmd.Parameters.Add(spPramName, SqlDbType.VarBinary);
                        pram.Value = spPramValue;
                        break;

                    case "VARCHAR":
                        pram = cmd.Parameters.Add(spPramName, SqlDbType.VarChar);
                        pram.Value = spPramValue;
                        break;

                    case "VARIANT":
                        pram = cmd.Parameters.Add(spPramName, SqlDbType.Variant);
                        pram.Value = spPramValue;
                        break;

                    case "XML":
                        pram = cmd.Parameters.Add(spPramName, SqlDbType.Xml);
                        pram.Value = spPramValue;
                        break;
                }
                switch (spParameterDirection.ToUpper())
                {
                    case "I":
                        pram.Direction = ParameterDirection.Input;
                        break;

                    case "O":
                        pram.Direction = ParameterDirection.Output;
                        break;

                    case "R":
                        pram.Direction = ParameterDirection.ReturnValue;
                        break;

                    case "IO":
                        pram.Direction = ParameterDirection.InputOutput;
                        break;
                }
                #endregion
            }
            return cmd;
        }

        public static async Task<DbReturnList> DataImportInTable(DataTable dt, string tableName)
        {
            DbReturnList dbReturnList = new DbReturnList();
            try
            {
                using (SqlConnection connection = new SqlConnection(dbConnStr))
                {
                    await connection.OpenAsync();
                    // Check if the table exists
                    if (await TableExistsAsync(connection, tableName))
                    {
                        //// Verify if columns match before inserting
                        if (await ColumnsMatchAsync(connection, dt, tableName))
                        {
                            dbReturnList.Ret = -1;
                            dbReturnList.ErrorMsg = "Column names or types do not match the existing table.";
                            return dbReturnList;
                        }
                        // Perform bulk copy into the existing table
                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                        {
                            bulkCopy.DestinationTableName = tableName;
                            foreach (DataColumn column in dt.Columns)
                            {
                                bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                            }
                            await bulkCopy.WriteToServerAsync(dt);
                            dbReturnList.Ret = 201;
                            dbReturnList.ErrorMsg = "Data Imported Successfully Into " + tableName + " Table.";
                            return dbReturnList;
                        }
                    }
                    else
                    {
                        // Create a new table if it doesn't exist
                        await CreateTableAsync(connection, dt, tableName);

                        // Perform bulk copy into the new table
                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                        {
                            bulkCopy.DestinationTableName = tableName;
                            foreach (DataColumn column in dt.Columns)
                            {
                                bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                            }
                            await bulkCopy.WriteToServerAsync(dt);
                        }
                        dbReturnList.Ret = 201;
                        dbReturnList.ErrorMsg = "Data Imported Successfully Into New Table i.e. " + tableName;
                        return dbReturnList;
                    }
                }
            }
            catch (Exception ex)
            {
                dbReturnList.Ret = -1;
                dbReturnList.ErrorMsg = ex.Message;
                return dbReturnList;
            }
        }

        //Check Table is exists or not
        private static async Task<bool> TableExistsAsync(SqlConnection connection, string tableName)
        {
            var query = $@"SELECT CASE WHEN OBJECT_ID(N'{tableName}', 'U') IS NOT NULL THEN 1 ELSE 0 END";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                return (int)await command.ExecuteScalarAsync() == 1;
            }
        }
        //Create New Table into Database.
        private static async Task CreateTableAsync(SqlConnection connection, DataTable dt, string tableName)
        {
            StringBuilder createTableCommand = new StringBuilder($"CREATE TABLE {tableName} (");
            foreach (DataColumn column in dt.Columns)
            {
                string columnType = GetSqlType(column.DataType);
                createTableCommand.Append($"[{column.ColumnName}] {columnType}, ");
            }
            createTableCommand.Length -= 2; // Remove last comma and space
            createTableCommand.Append(");");

            using (SqlCommand command = new SqlCommand(createTableCommand.ToString(), connection))
            {
                await command.ExecuteNonQueryAsync();
            }
        }
        // Match the columns into existing tables
        private static async Task<bool> ColumnsMatchAsync(SqlConnection connection, DataTable dt, string tableName)
        {
            var query = $@"SELECT COLUMN_NAME, DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{tableName}'";

            using (SqlCommand command = new SqlCommand(query, connection))
            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                var existingColumns = new Dictionary<string, string>();

                while (await reader.ReadAsync())
                {
                    existingColumns[reader["COLUMN_NAME"].ToString()] = reader["DATA_TYPE"].ToString();
                }

                if (existingColumns.Count != dt.Columns.Count)
                    return false;

                foreach (DataColumn column in dt.Columns)
                {
                    if (!existingColumns.TryGetValue(column.ColumnName, out var existingType) ||
                        GetSqlType(column.DataType) != existingType.ToUpper())
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        //Get the SQl data type of column data
        private static string GetSqlType(Type type)
        {
            // Map .NET data types to SQL Server types
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Int32:
                    return "INT";
                case TypeCode.String:
                    return "NVARCHAR(MAX)";
                case TypeCode.DateTime:
                    return "DATETIME";
                case TypeCode.Boolean:
                    return "BIT";
                case TypeCode.Decimal:
                    return "DECIMAL(18,2)";
                case TypeCode.Double:
                    return "FLOAT";
                case TypeCode.Int64:
                    return "BIGINT";
                default:
                    return "NVARCHAR(MAX)"; // Default to string if the type is unknown
            }
        }
    }
}
