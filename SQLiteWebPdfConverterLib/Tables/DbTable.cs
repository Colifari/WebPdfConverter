using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteWebPdfConverterLib.Tables
{
    /// <summary>
    /// Basic table object
    /// </summary>
    public class DbTable
    {
        protected internal string connectionString;
        protected readonly internal DataContext Context;

        internal DbTable(string cs, DataContext context)
        {
            connectionString = cs;
            Context = context;
        }

    }
}
