using Main.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Main.Data.Interfaces
{
    interface IDatabase_Repository
    {
		IEnumerable<Database_> Databases { get; set; }
    }
}
