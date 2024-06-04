using CFMonitor.Checkers;
using CFMonitor.Interfaces;
using System.Collections.Generic;

namespace CFMonitor.Services
{
    public class CheckersService : ICheckersService
    {
        public List<IChecker> GetAll()
        {
            List<IChecker> checkerList = new List<IChecker>();
            checkerList.Add(new CheckerDHCP());
            checkerList.Add(new CheckerDiskSpace());
            checkerList.Add(new CheckerDNS());
            checkerList.Add(new CheckerJSON());
            checkerList.Add(new CheckerLDAP());
            checkerList.Add(new CheckerMemory());
            checkerList.Add(new CheckerLocalFile());
            checkerList.Add(new CheckerMemory());
            checkerList.Add(new CheckerPing());
            checkerList.Add(new CheckerProcess());
            checkerList.Add(new CheckerRegistry());
            checkerList.Add(new CheckerREST());
            checkerList.Add(new CheckerService());
            checkerList.Add(new CheckerSMTP());
            checkerList.Add(new CheckerSOAP());
            checkerList.Add(new CheckerSocket());
            checkerList.Add(new CheckerSQL());
            checkerList.Add(new CheckerURL());
            return checkerList;
        }
    }
}
