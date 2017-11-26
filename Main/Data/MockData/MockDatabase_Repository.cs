using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Main.Data.Interfaces;
using Main.Models;

namespace Main.Data.MockData
{
	public class MockDatabase_Repository : IDatabase_Repository
	{
		public IEnumerable<Database_> Databases
		{
			get
			{
				return new List<Database_>
				{
					new Database_
					{
						databaseId = 1,
						name = "Database A",
						server = null
					},
					new Database_
					{
						databaseId = 2,
						name = "Database B",
						server = null
					},
					new Database_
					{
						databaseId = 3,
						name = "Database C",
						server = null
					},
				};
			}
			set => throw new NotImplementedException();
		}
	}
}
