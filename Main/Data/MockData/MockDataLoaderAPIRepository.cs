using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Main.Data.Interfaces;
using Main.Models;

namespace Main.Data.MockData
{
	public class MockDataLoaderAPIRepository : IDataLoaderAPIRepository
	{
		public IEnumerable<DataLoaderAPI> DataLoaderAPIs
		{
			get
			{
				return new List<DataLoaderAPI>
				{
					new DataLoaderAPI
					{
						id = 1,
						name = "DataLoaderAPI A"
					},
					new DataLoaderAPI
					{
						id = 2,
						name = "DataLoaderAPI B"
					},
				};
			}
			set => throw new NotImplementedException();
		}
	}
}
