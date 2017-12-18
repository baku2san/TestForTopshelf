
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;

namespace TestForTopShelfAndInstaller.Models
{
    public class MemoryContext : DbContext
    {

        public MemoryContext()
        {
            Database.SetInitializer<MemoryContext>(null);
        }
        public DbSet<TableA> TableAs { get; set; }
        //Logger 設定 データは行うため他のプロジェクトから自動Migration
    }
}
