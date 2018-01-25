using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Main.Models
{
	public class Database_
	{
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int databaseId { get; set; }
		public string name { get; set; }
		public DatabaseServer server { get; set; }

	}
}