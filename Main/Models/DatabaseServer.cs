using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Main.Models
{
	public class DatabaseServer
	{
		public int serverId { get; set; }
		public string name { get; set; }
		public string connectionString { get; set; }
		public List<Database_> databases { get; set; }
	}
}