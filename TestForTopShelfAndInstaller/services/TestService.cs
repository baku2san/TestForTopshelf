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

namespace TestForTopShelfAndInstaller
{
    public class TestService
    {
        private TcpListener tcpListener;
        private readonly System.Timers.Timer timer;

        public void Start() { timer.Start(); }
        public void Stop() { timer.Stop(); }

        public TestService()
        {
            tcpListener = new TcpListener(IPAddress.Any, 8210);
            tcpListener.Start();
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
    }
}

