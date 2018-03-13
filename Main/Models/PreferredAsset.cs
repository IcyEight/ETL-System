using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Main.Models
{
    public class PreferredAsset
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int paID { get; set; }
        public String userID { get; set; }
        public int assetID { get; set; }
        public Boolean isDeleted { get; set; }
    }
}
