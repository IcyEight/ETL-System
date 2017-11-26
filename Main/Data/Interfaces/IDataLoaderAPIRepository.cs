using Main.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Main.Data.Interfaces
{
    interface IDataLoaderAPIRepository
    {
		IEnumerable<DataLoaderAPI> DataLoaderAPIs { get; set; }
    }
}
