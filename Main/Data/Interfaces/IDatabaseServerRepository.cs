using Main.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Main.Data.Interfaces
{
    interface IDatabaseServerRepository
    {
		IEnumerable<DatabaseServer> DatabaseServers { get; set; }
    }
}
