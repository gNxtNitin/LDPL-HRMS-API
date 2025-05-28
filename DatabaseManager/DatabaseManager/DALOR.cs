using System;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections;
using System.Configuration;
using System.Data.OleDb;
using System.Web;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Data.OracleClient;
using Oracle.ManagedDataAccess.Types;
using System.Runtime.Intrinsics.Arm;

namespace DatabaseManager
{
    public class DALOR
    {
        //static readonly string dbConnStr = "User Id=LDPLERP;Password=LDPLERP;Data Source=103.178.248.36:1521/LDPLCLOUD;";
        //static readonly string dbConnStr = "User Id=C##USER_NAME;Password=user_password;Data Source=localhost:1521/ORCL;";
        //static readonly string dbConnStr = "User Id=SYSTEM;Password=A;Data Source=localhost:1521/ORCL;";
        //static readonly string dbConnStr = "User Id=DEV;Password=gNxt_123;Data Source=103.178.248.36:1521/LDPLCLOUD;";
        //static readonly string dbConnStr = "User Id=DEV;Password=gNxt_123;Data Source=10.10.248.36:1521/LDPLCLOUD;";
        public static string dbConnStr { get; set;}
        public static string SelectAllFromTable()
        {
            try
            {
                var connection = new OracleConnection(dbConnStr);
                connection.Open();
                Console.WriteLine("Oracle DB Connection Successful!");
                return "connection";
            }
            catch (OracleException ex)
            {
                Console.WriteLine($"Oracle DB Connection Error: {ex.Message}");
                throw;
            }
        }

        public async static Task<string> GetAuthenticatedUserAsync(string emailOrMobile, string password)
        {
            string errorMsg = string.Empty;
            string retValue = "-1;";
            string eeemail = string.Empty;
            string designation = string.Empty;

            using (var conn = new OracleConnection(dbConnStr))
            using (var cmd = new OracleCommand("sp_GetAuthenticatedUser", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("p_MobileOrEmailId", OracleDbType.Varchar2).Value = emailOrMobile;
                cmd.Parameters.Add("p_Password", OracleDbType.Varchar2).Value = password;
                cmd.Parameters.Add("p_ErrorMsg", OracleDbType.Varchar2, 4000).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("p_Ret", OracleDbType.Int64).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("p_ResultSet", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                errorMsg = cmd.Parameters["p_ErrorMsg"].Value.ToString();
                retValue = cmd.Parameters["p_Ret"].Value.ToString();

                if (int.Parse(retValue) > 0)
                {
                    var reader = ((OracleRefCursor)cmd.Parameters["p_ResultSet"].Value).GetDataReader();
                    while (reader.Read())
                    {
                        eeemail = reader["Email"].ToString();
                        designation = reader["DESIGNATION"].ToString();
                    }
                }
            }
            return eeemail + designation;
        }

        public DataTable ExecuteStoredProcedure(string procedureName, OracleParameter[] parameters = null)
        {
            using (OracleConnection connection = new OracleConnection(dbConnStr))
            using (OracleCommand command = new OracleCommand(procedureName, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }
                OracleDataAdapter adapter = new OracleDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }

        public void ExecuteNonQueryStoredProcedure(string procedureName, OracleParameter[] parameters)
        {
            using (OracleConnection connection = new OracleConnection(dbConnStr))
            using (OracleCommand command = new OracleCommand(procedureName, connection))
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

        public object GetOutputParameterValue(OracleCommand command, string parameterName)
        {
            return command.Parameters[parameterName]?.Value;
        }

        public class DbReturnList
        {
            public int Ret { get; set; }
            public string ErrorMsg { get; set; }
        }

        internal class SPArgBuild
        {
            internal string parameterName = "";
            internal string parameterValue = "";
            internal string pramValueType = "";
            internal string parameterDirection = "";
            internal int? size = null;

            internal SPArgBuild(string pramName, string pramValue, string pramValueType, string parameterDirection, int? size = null)
            {
                this.parameterName = pramName;
                this.parameterValue = pramValue;
                this.pramValueType = pramValueType;
                this.parameterDirection = parameterDirection;
                this.size = size;
            }
        }

        public static ArrayList spArgumentsCollection(ArrayList arrLst, string spParmName, string spParmValue, string spPramValueType, string parameterDirection, int? size = null)
        {
            SPArgBuild spArgBuiltObj = new SPArgBuild(spParmName, spParmValue, spPramValueType, parameterDirection, size);
            arrLst.Add(spArgBuiltObj);
            return arrLst;
        }

        public static OracleCommand ArrangeParameter(ArrayList spPramArrList)
        {
            OracleCommand cmd = new OracleCommand();
            foreach (SPArgBuild arg in spPramArrList)
            {
                OracleParameter pram = new OracleParameter(arg.parameterName, GetOracleDbType(arg.pramValueType));

                if (arg.parameterDirection.ToUpper() == "I" || arg.parameterDirection.ToUpper() == "IO")
                {
                    pram.Value = arg.parameterValue;
                }

                if (arg.pramValueType == "INT")
                {
                    pram.Value = arg.parameterValue != null ? int.Parse(arg.parameterValue) : 0;
                }
                if (arg.pramValueType == "BLOB")
                {
                    pram.Value = arg.parameterValue != null ? Convert.FromBase64String(arg.parameterValue) : Array.Empty<byte>();
                }
                if (arg.size.HasValue)
                {
                    pram.Size = arg.size.Value;
                }
                else if ((arg.pramValueType.ToUpper() == "VARCHAR" || arg.pramValueType.ToUpper() == "NVARCHAR" || arg.pramValueType.ToUpper() == "CHAR")
                         && arg.parameterDirection.ToUpper() != "I")
                {
                    pram.Size = 4000;
                }

                switch (arg.parameterDirection.ToUpper())
                {
                    case "I": pram.Direction = ParameterDirection.Input; break;
                    case "O": pram.Direction = ParameterDirection.Output; break;
                    case "IO": pram.Direction = ParameterDirection.InputOutput; break;
                    case "R": pram.Direction = ParameterDirection.ReturnValue; break;
                }

                cmd.Parameters.Add(pram);
            }
            return cmd;
        }

        private static OracleDbType GetOracleDbType(string dbType)
        {
            switch (dbType.ToUpper())
            {
                case "VARCHAR": return OracleDbType.Varchar2;
                case "NVARCHAR": return OracleDbType.NVarchar2;
                case "CHAR": return OracleDbType.Char;
                case "INT": return OracleDbType.Int32;
                case "NUMBER": return OracleDbType.Decimal;
                case "DATE": return OracleDbType.Date;
                case "CLOB": return OracleDbType.Clob;
                case "BLOB": return OracleDbType.Blob;
                case "FLOAT": return OracleDbType.Single;
                case "LONG": return OracleDbType.Long;
                case "TIMESTAMP": return OracleDbType.TimeStamp;
                case "REFCURSOR": return OracleDbType.RefCursor;
                default: return OracleDbType.Varchar2;
            }
        }

        public static DataSet RunStoredProcedure(DataSet ds, string spName, ArrayList spPramArrList)
        {
            OracleConnection conn = new OracleConnection(dbConnStr);
            conn.Open();
            OracleCommand cmd = ArrangeParameter(spPramArrList);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = conn;
            cmd.CommandText = spName;
            if (ds != null && ds is DataSet)
            {
                OracleDataAdapter adap = new OracleDataAdapter(cmd);
                adap.Fill(ds);
            }
            else
            {
                cmd.ExecuteNonQuery();
            }
            conn.Close();
            return ds;
        }

        public static void RunStoredProcedure(string spName, ArrayList spPramArrList)
        {
            OracleConnection conn = new OracleConnection(dbConnStr);
            conn.Open();
            OracleCommand cmd = ArrangeParameter(spPramArrList);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = conn;
            cmd.CommandText = spName;
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public static int RunStoredProcedureRet(string spName, ArrayList spPramArrList)
        {
            OracleConnection conn = new OracleConnection(dbConnStr);
            conn.Open();
            OracleCommand cmd = ArrangeParameter(spPramArrList);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = conn;
            cmd.CommandText = spName;
            cmd.ExecuteNonQuery();
            int ret = int.Parse(cmd.Parameters["@ret"].Value.ToString());
            conn.Close();
            return ret;
        }

        public static DbReturnList RunStoredProcedureRetError(string spName, ArrayList spPramArrList)
        {
            DbReturnList dbReturnList = new DbReturnList();
            OracleConnection conn = new OracleConnection(dbConnStr);
            conn.Open();
            OracleCommand cmd = ArrangeParameter(spPramArrList);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = conn;
            cmd.CommandText = spName;
            cmd.ExecuteNonQuery();
            //var kk = cmd.Parameters["@ret"].Value;
            dbReturnList.Ret = int.Parse(cmd.Parameters["@ret"].Value.ToString());
            dbReturnList.ErrorMsg = cmd.Parameters["@errormsg"].Value.ToString();
            conn.Close();
            return dbReturnList;
        }

        public static DbReturnList RunStoredProcedureDsRetError(string spName, ArrayList spPramArrList, DataSet ds = null)
        {
            DbReturnList dbReturnList = new DbReturnList();
            using (OracleConnection conn = new OracleConnection(dbConnStr))
            {
                conn.Open();
                OracleCommand cmd = ArrangeParameter(spPramArrList);


                //debug

                //OracleCommand cmd = new OracleCommand();
                //cmd.Parameters.Add("P_CUSTOMER_CODE", OracleDbType.Char, 65).Value = "18166";
                //cmd.Parameters.Add("P_COMPANY_CODE", OracleDbType.Int32).Value = "1";
                //cmd.Parameters.Add("P_FLAG", OracleDbType.Varchar2).Value = "W";

                //cmd.Parameters.Add("RET", OracleDbType.Varchar2, 32767).Direction = ParameterDirection.Output;
                //cmd.Parameters.Add("ERRORMSG", OracleDbType.Varchar2, 32767).Direction = ParameterDirection.Output;

                //cmd.Parameters.Add("O_METRIC_CARDS", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                //cmd.Parameters.Add("O_DAILY_ORDER_CHART", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                //cmd.Parameters.Add("O_LAST_6_MONTHS_CHART", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                //cmd.Parameters.Add("O_TOP_10_ITEMS_CHART", OracleDbType.RefCursor).Direction = ParameterDirection.Output;


                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;
                cmd.CommandText = spName;

                if (ds != null && ds is DataSet)
                {
                    OracleDataAdapter adap = new OracleDataAdapter(cmd);
                    adap.Fill(ds);
                }
                else
                {
                    cmd.ExecuteNonQuery();
                }
                //var kk = cmd.Parameters["@ret"].Value.ToString();
                //var kk2 = cmd.Parameters["@errormsg"].Value.ToString();
                dbReturnList.Ret = int.Parse(cmd.Parameters["@ret"].Value.ToString());
                dbReturnList.ErrorMsg = cmd.Parameters["@errormsg"].Value.ToString();
                return dbReturnList;
            }
        }
    }
}
