using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebEdoc2017.DataAccess
{
   public class OracleDataAccess
    {
        public enum ExecutionType
        {
            ExecuteNonQuery,
            ExecuteScalar,
            ExecuteReader,
            ExecuteDataSet
        }

        public class OracleCommandData
        {
            // OracleConnection con = new OracleConnection("DATA SOURCE=192.168.1.22:1521/ORCL;USER ID=KE;password=KE2016;");
            //DATA SOURCE=192.168.1.22:1521/ORCL;USER ID=KE

            DataTable dt;
            OracleDataAdapter adpt;

            OracleCommand cmd = new OracleCommand();
            OracleConnection conn = new OracleConnection();
            OracleTransaction trans;
            OracleDataAdapter da;

            DataSet ds;
            OracleDataReader dr;
            string ConnectionString;

            public OracleCommandData()
            {
                this.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["OraDBConn"].ConnectionString;

                conn.ConnectionString = ConnectionString;
                cmd.Connection = conn;
                //cmd.Transaction = trans;
                cmd.CommandTimeout = 1440;
                //cmd.CommandType = CommandType.StoredProcedure;
            }

            public OracleCommandData(string connString)
            {
                this.ConnectionString = connString;
                conn.ConnectionString = ConnectionString;
                cmd.Connection = conn;
                //cmd.Transaction = trans;
                cmd.CommandType = CommandType.StoredProcedure;
            }

            public string CommandText
            {
                set
                {
                    cmd.CommandText = value;
                }
            }

            public void AddParameter(string ParameterName, object ParameterValue)
            {
                SqlParameter param = new SqlParameter();
                param.ParameterName = ParameterName;
                param.Value = ParameterValue;
                param.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(param);

            }

            public void AddParameter(string ParameterName, object ParameterValue, ParameterDirection direction)
            {
                SqlParameter param = new SqlParameter();
                param.ParameterName = ParameterName;
                param.Value = ParameterValue;
                param.Direction = direction;
                cmd.Parameters.Add(param);

            }

            public object Execute(ExecutionType e)
            {
                object o = -1;

                switch (e)
                {
                    case ExecutionType.ExecuteNonQuery:
                        o = ExecuteNonQuery();
                        break;

                    case ExecutionType.ExecuteScalar:
                        o = ExecuteScalar();
                        break;

                    case ExecutionType.ExecuteDataSet:
                        o = ExecuteDataSet();
                        break;

                    //case ExecutionType.ExecuteReader:
                    //    o = ExecuteReader();
                    //    break;

                }//end of switch

                cmd.Parameters.Clear();

                return o;
            }//end of method

            private int ExecuteNonQuery()
            {
                //			try
                //			{
                conn.Open();

                int nRows = cmd.ExecuteNonQuery();
                conn.Close();
                return nRows;
                //			}
                //			catch(Exception exc)
                //			{
                //				Console.WriteLine(exc.Message);
                //				return -1;
                //			}
            }

            private object ExecuteScalar()
            {
                //conn.Open();
                object o = cmd.ExecuteScalar();
                //conn.Close();
                return o;
            }

            private DataSet ExecuteDataSet()
            {
                da = new OracleDataAdapter();
                ds = new DataSet();
                da.SelectCommand = cmd;
                cmd.Connection = conn;
                da.Fill(ds, cmd.CommandText);
                return ds;
            }

            //private OracleDataAdapter ExecuteReader()
            //{
            //    //conn.Open();
            //  // dr =new  OracleDataReader();
            //    dr = cmd.ExecuteReader();
            //    //conn.Close();
            //    return dr;
            //}

            public OracleParameterCollection Parameters
            {
                get
                {
                    return cmd.Parameters;
                }
            }

            //public void Open()
            //{
            //    if (conn.State == ConnectionState.Closed)
            //    {
            //        conn.Open();
            //        trans = BeginTrans();
            //        cmd.Transaction = trans;
            //    }
            //}

            public void OpenWithOutTrans()
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
            }


            public void Close()
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }

            public CommandType _CommandType
            {
                get { return cmd.CommandType; }
                set { cmd.CommandType = value; }
            }

            //public SqlTransaction BeginTrans()
            //{
            //    return conn.BeginTransaction();
            //}

            public void Commit()
            {
                trans.Commit();
            }

            public void RollBack()
            {
                trans.Rollback();
            }



            //public class CommandWrapper
            //{
            //    public static string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["OracleDataBaseConnection"].ConnectionString;

            //    public static OracleCommandData CommandObj;

            //    public static void Initialize()
            //    {
            //        if (CommandObj == null)
            //        {
            //            CommandObj = new OracleCommandData();
            //        }
            //    }


            //}
        }
    }
}
