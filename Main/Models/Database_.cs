using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Main.Models
{
	public class Database_
	{
		public int databaseId { get; set; }
		public string name { get; set; }
		public DatabaseServer server { get; set; }

	}
}