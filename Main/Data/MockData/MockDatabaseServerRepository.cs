using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Main.Data.Interfaces;
using Main.Models;

namespace Main.Data.MockData
{
	public class MockDatabaseServerRepository : IDatabaseServerRepository
	{
		public IEnumerable<DatabaseServer> DatabaseServers
		{
			get
			{
				return new List<DatabaseServer>
				{
					new DatabaseServer
					{
						serverId = 1,
						name = "Server A",
						connectionString = "Connection String A",
						databases = null
					},
					new DatabaseServer
					{
						serverId = 2,
						name = "Server B",
						connectionString = "Connection String B",
						databases = null
					},
					new DatabaseServer
					{
						serverId = 3,
						name = "Server B",
						connectionString = "Connection String C",
						databases = null
					}
				};
			}
			set => throw new NotImplementedException();
		}
	}
}
