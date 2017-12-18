using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Serilog;
using System.Data.SqlClient;
using System.Transactions;
using System.Data;
using System.Configuration;
using TestForTopShelfAndInstaller.Services;
using static TestForTopShelfAndInstaller.Services.MemoryInfo;

namespace TestForTopShelfAndInstaller
{
    public class TestService
    {
        private TcpListener tcpListener;
        private readonly System.Timers.Timer timer;
        private string ConnectionString;

        public void Start() { timer.Start(); }
        public void Stop() { timer.Stop(); }

        public TestService()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["MemoryContext"].ConnectionString;
            tcpListener = new TcpListener(IPAddress.Any, 8210);
            tcpListener.Start();
            Console.WriteLine("start ..." );
            Log.Information("start!");

            timer = new System.Timers.Timer(1000)
            {
                AutoReset = true
            };
            timer.Elapsed += (sender, eventArgs) => DoSomething();
        }

        /// <summary>
        /// リクエストチェック
        /// </summary>
        private void DoSomething()
        {
            var path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            Console.WriteLine("doing at ..." + path);
            testInsert();
            if (tcpListener.Pending() == true)
            {

                Thread threadReceive = new Thread(new ThreadStart(threadExecute));
                threadReceive.Start();
            }
        }

        /// <summary>
        /// スレッド処理
        /// </summary>
        private void threadExecute()
        {
            Console.WriteLine("Execute!");
        }

        private void testInsert()
        {
            var infos = new List<MemoryInfo>()
            {
                new MemoryInfo("ColumnA", 0, AccessSizes.BIT),
                new MemoryInfo("ColumnB", 0, AccessSizes.BIT),
                new MemoryInfo("ColumnC", 0, AccessSizes.BIT),
            };
            var tableName = "TableAs";

            var sqlCommand = GetSqlCommandAsInsert(tableName, "Line1", infos);
            sqlCommand = GetSqlCommandAsUpdate(tableName, "Line1", infos);
            sqlCommand = GetSqlCommandAsUpdateOrInsert(tableName, "Line10", infos);
            var valueList = new MemoryValueList("testList", sqlCommand);
            valueList.MemoryValuesGroups.Add(infos.Select(s => new MemoryValue(s, true)).ToList());
            InsertData(valueList);
        }
        static public SqlCommand GetSqlCommandAsInsert(string tableName, string lineName, IEnumerable<MemoryInfo> infos)
        {
            var commandText = new StringBuilder("INSERT INTO ");
            commandText.Append(tableName);
            infos.Aggregate(commandText.Append("("), (a, b) => a.Append(b.Name).Append(","), (a) => a.Append("LineName, Created)"));
            //settings.ReadingTargets.Aggregate(commandText.Append("("), (a, b) => a.Append(b.Name).Append(","), (a) => a.Replace(",", ")", a.Length - 1, 1));
            infos.Aggregate(commandText.Append(" Values ("), (a, b) => a.Append("@").Append(b.Name).Append(","), (a) => a.Append("@LineName,@Created)"));

            void AddParameters(MemoryInfo info, SqlCommand command)
            {
                switch (info.AccessSize)
                {
                    case AccessSizes.BIT:
                        command.Parameters.Add(info.Name, SqlDbType.Bit);
                        break;
                    case AccessSizes.BYTE:
                        command.Parameters.Add(info.Name, SqlDbType.Binary, 1);
                        break;
                    case AccessSizes.WORD:
                        command.Parameters.Add(info.Name, SqlDbType.Binary, 2);
                        break;
                    case AccessSizes.DWORD:
                    default:
                        command.Parameters.Add(info.Name, SqlDbType.Binary, 4);
                        break;
                }
            }
            var sqlCommand = new SqlCommand(commandText.ToString());
            infos.ToList().ForEach(f => AddParameters(f, sqlCommand));
            sqlCommand.Parameters.Add("LineName", SqlDbType.NVarChar);
            sqlCommand.Parameters["LineName"].Value = lineName;
            sqlCommand.Parameters.Add("Created", SqlDbType.DateTime2);

            return sqlCommand;
        }
        static public SqlCommand GetSqlCommandAsUpdateOrInsert(string tableName, string lineName, IEnumerable<MemoryInfo> infos)
        {
            // 現状 source は不要だけど、Merge には USING 必須っぽいので、付けてるだけ as も要らんけど
            var commandText = new StringBuilder("MERGE TOP(1) ").Append(tableName).Append(" as target USING ").Append(tableName).Append(" as source");
            commandText.Append(" ON (target.LineName='").Append(lineName).Append("')");
            commandText.Append(" WHEN MATCHED THEN");

            infos.Aggregate(commandText.Append(" UPDATE SET "), (a, b) => a.Append(b.Name).Append("=@").Append(b.Name).Append(","), (a) => a.Append("Created=@Created "));

            commandText.Append(" WHEN NOT MATCHED BY target THEN");

            infos.Aggregate(commandText.Append(" INSERT ("), (a, b) => a.Append(b.Name).Append(","), (a) => a.Append("LineName, Created)"));
            infos.Aggregate(commandText.Append(" Values ("), (a, b) => a.Append("@").Append(b.Name).Append(","), (a) => a.Append("@LineName,@Created);"));

            void AddParameters(MemoryInfo info, SqlCommand command)
            {
                switch (info.AccessSize)
                {
                    case AccessSizes.BIT:
                        command.Parameters.Add(info.Name, SqlDbType.Bit);
                        break;
                    case AccessSizes.BYTE:
                        command.Parameters.Add(info.Name, SqlDbType.Binary, 1);
                        break;
                    case AccessSizes.WORD:
                        command.Parameters.Add(info.Name, SqlDbType.Binary, 2);
                        break;
                    default:
                        command.Parameters.Add(info.Name, SqlDbType.Binary, 4);
                        break;
                }
            }
            var sqlCommand = new SqlCommand(commandText.ToString());
            infos.ToList().ForEach(f => AddParameters(f, sqlCommand));
            sqlCommand.Parameters.Add("LineName", SqlDbType.NVarChar);
            sqlCommand.Parameters["LineName"].Value = lineName;
            sqlCommand.Parameters.Add("Created", SqlDbType.DateTime2);

            return sqlCommand;
        }
        static public SqlCommand GetSqlCommandAsUpdate(string tableName, string lineName, IEnumerable<MemoryInfo> infos)
        {
            var commandText = new StringBuilder("UPDATE TOP(1) ");
            commandText.Append(tableName);
            infos.Aggregate(commandText.Append(" SET "), (a, b) => a.Append(b.Name).Append("=@").Append(b.Name).Append(","), (a) => a.Append("Created=@Created "));
            commandText.Append("WHERE LineName='").Append(lineName).Append("'");

            void AddParameters(MemoryInfo info, SqlCommand command)
            {
                switch (info.AccessSize)
                {
                    case AccessSizes.BIT:
                        command.Parameters.Add(info.Name, SqlDbType.Bit);
                        break;
                    case AccessSizes.BYTE:
                        command.Parameters.Add(info.Name, SqlDbType.Binary, 1);
                        break;
                    case AccessSizes.WORD:
                        command.Parameters.Add(info.Name, SqlDbType.Binary, 2);
                        break;
                    default:
                        command.Parameters.Add(info.Name, SqlDbType.Binary, 4);
                        break;
                }
            }
            var sqlCommand = new SqlCommand(commandText.ToString());
            infos.ToList().ForEach(f => AddParameters(f, sqlCommand));
            sqlCommand.Parameters.Add("Created", SqlDbType.DateTime2);

            return sqlCommand;
        }
        private int InsertData(MemoryValueList valueList)
        {
            // Initialize the return value to zero and create a StringWriter to display results.
            int affectedCountOfRows = 0;
            var writer = new System.IO.StringWriter();

            var sqlCommands = valueList.MemoryValuesGroups.Select(s => new { Command = valueList.SqlCommand.Clone(), Values = s })
                .Select(f => {
                    f.Values.ForEach(fo => f.Command.Parameters[fo.MemoryInfo.Name].Value = fo.Value);
                    f.Command.Parameters["Created"].Value = valueList.Created;
                    return f.Command;
                });
            try
            {
                // Create the TransactionScope to execute the commands, guaranteeing
                // that both commands can commit or roll back as a single unit of work.
                using (var scope = new TransactionScope())
                {
                    using (var sqlConnection = new SqlConnection(ConnectionString))
                    {
                        // Opening the connection automatically enlists it in the 
                        // TransactionScope as a lightweight transaction.
                        sqlConnection.Open();

                        // Create the SqlCommand object and execute the first command.
                        // sqlCommands.ToList().ForEach(f=> f.Connection = sqlConnection);
                        affectedCountOfRows = sqlCommands.ToList().Aggregate(0, (x, y) => { y.Connection = sqlConnection; return x + y.ExecuteNonQuery(); });
                        //writer.WriteLine("Rows to be affected by command1: {0}", affectedCountOfRows);
                        //sqlConnection.Close();    // dispose と同じなんでやらない
                    }
                    // The Complete method commits the transaction. If an exception has been thrown,
                    // Complete is not  called and the transaction is rolled back.
                    scope.Complete();
                }
                writer.Write("L");  // logger data inserted
            }
            catch (TransactionAbortedException ex)
            {
                writer.WriteLine("TransactionAbortedException Message: {0}", ex.Message);
            }
            catch (ApplicationException ex)
            {
                writer.WriteLine("ApplicationException Message: {0}", ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            // Display messages.
            Console.Write(writer.ToString());
            return affectedCountOfRows;
        }
    }
}

