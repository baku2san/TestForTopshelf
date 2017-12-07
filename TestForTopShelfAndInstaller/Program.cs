using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace TestForTopShelfAndInstaller
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<TestService>(s =>
                {
                    s.ConstructUsing(name => new TestService());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());

                });

                x.EnableServiceRecovery(r =>
                {
                    r.OnCrashOnly();
                    r.RestartService(1); //first
                    r.RestartService(1); //second
                    r.RestartService(1); //subsequents
                });
                //Windowsサービスの設定
                x.RunAsLocalSystem()
                    .DependsOnEventLog()
                    .StartAutomaticallyDelayed();

                x.SetDescription("This is TopShelfSample");
                x.SetDisplayName("TopShelfSample");
                x.SetServiceName("TopShelfSample_Service");
                x.StartAutomaticallyDelayed();
            });
        }
    }
}
