using CFMonitor.Actioners;
using CFMonitor.Interfaces;
using CFMonitor.Models.MonitorItems;
using System;
using System.Collections.Generic;

namespace CFMonitor
{
    /// <summary>
    /// Factory for returning object instances
    /// </summary>
    public class Factory
    {
        public static List<IChecker> GetCheckerList()
        {
            List<IChecker> checkerList = new List<IChecker>();
            checkerList.Add(new CheckerDHCP());
            checkerList.Add(new CheckerDiskSpace());
            checkerList.Add(new CheckerDNS());
            checkerList.Add(new CheckerJSON());
            checkerList.Add(new CheckerLDAP());
            checkerList.Add(new CheckerMemory());
            checkerList.Add(new CheckerFile());
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

        public static List<IActioner> GetActionerList()
        {
            List<IActioner> actionerList = new List<IActioner>();
            actionerList.Add(new ActionerEmail());
            actionerList.Add(new ActionerLog());
            actionerList.Add(new ActionerProcess());
            actionerList.Add(new ActionerSQL());
            actionerList.Add(new ActionerURL());
            return actionerList;
        }

        public static List<Type> GetMonitorItemTypeList()
        {
            List<Type> typeList = new List<Type>();
            typeList.Add(typeof(MonitorDHCP));
            typeList.Add(typeof(MonitorDiskSpace));
            typeList.Add(typeof(MonitorDNS));
            typeList.Add(typeof(MonitorFile));
            typeList.Add(typeof(MonitorJSON));
            typeList.Add(typeof(MonitorLDAP));
            typeList.Add(typeof(MonitorMemory));
            typeList.Add(typeof(MonitorPing));
            typeList.Add(typeof(MonitorProcess));
            typeList.Add(typeof(MonitorRegistry));
            typeList.Add(typeof(MonitorREST));
            typeList.Add(typeof(MonitorService));
            typeList.Add(typeof(MonitorSMTP));
            typeList.Add(typeof(MonitorSOAP));
            typeList.Add(typeof(MonitorSocket));
            typeList.Add(typeof(MonitorSQL));
            typeList.Add(typeof(MonitorURL));
            //typeList.Add(typeof(MonitorItem));
            return typeList;
        }

        //public static IMonitorItemService GetDefaultMonitorItemRepository()
        //{
        //    return new MonitorItemService(Globals.MonitorItemFolder);
        //}
    }
}
