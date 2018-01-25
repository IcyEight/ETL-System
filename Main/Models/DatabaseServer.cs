using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Main.Models
{
	public class DatabaseServer
	{
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int serverId { get; set; }
		public string name { get; set; }
		public string connectionString { get; set; }
		public List<Database_> databases { get; set; }
	}
}