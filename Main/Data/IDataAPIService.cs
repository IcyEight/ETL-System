using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Main.Data
{
    interface IDataAPIService
    {
        void StartDataMonitoringThread();
        void LoadConfigurationXml();
    }
}
