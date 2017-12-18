using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestForTopShelfAndInstaller.Models
{
    public class TableA
    {
        public int TableAId { get; set; }
        public bool ColumnA { get; set; }
        public bool ColumnB { get; set; }
        public bool ColumnC { get; set; }
        public string LineName { get; set; }
        public DateTime Created { get; set; }
    }
}
