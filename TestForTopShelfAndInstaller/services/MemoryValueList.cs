using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace JMLoggerApp.Producers
{
    public class MemoryValueList 
    {
        public string Name { get; set; }
        public SqlCommand SqlCommand { get; set; }
        public DateTime Created { get; set; }

        public List<List<MemoryValue>> MemoryValuesGroups { get; set; }

        public MemoryValueList(string name, SqlCommand sqlCommand)
        {
            Name = name;
            SqlCommand = sqlCommand;
            Created = DateTime.Now;

            MemoryValuesGroups = new List<List<MemoryValue>>();
        }
    }
}
