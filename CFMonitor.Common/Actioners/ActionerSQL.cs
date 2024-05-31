using CFMonitor.Interfaces;
using CFMonitor.Models.ActionItems;
using CFMonitor.Models.MonitorItems;
using CFUtilities.Databases;

namespace CFMonitor.Actioners
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
            //database.ExecuteNonQuery(System.Data.CommandType.Text, actionSQL.SQL, null);
            database.Close();
        }

        public bool CanAction(ActionItem actionItem)
        {
            return actionItem is ActionSQL;
        }
    }
}
