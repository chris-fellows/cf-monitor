using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CFUtilities.Databases;

namespace CFMonitor
{
    /// <summary>
    /// Actions running SQL
    /// </summary>
    public class ActionerSQL : IActioner
    {
        public void DoAction(MonitorItem monitorItem, ActionItem actionItem, ActionParameters actionParameters)
        {
            ActionSQL actionSQL = (ActionSQL)actionItem;
            OleDbDatabase database = new OleDbDatabase(actionSQL.ConnectionString);
            database.Open();
            database.ExecuteNonQuery(System.Data.CommandType.Text, actionSQL.SQL, null);
            database.Close();
        }

        public bool CanAction(ActionItem actionItem)
        {
            return actionItem is ActionSQL;
        }
    }
}
