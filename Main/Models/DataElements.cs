using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Main.Models
{
    public class DataElements
    {
        public List<Dictionary<String, String>> rowEntries;

        public void AddRow(Dictionary<String, String> rowIn)
        {
            rowEntries.Add(rowIn);
        }
    }
}
