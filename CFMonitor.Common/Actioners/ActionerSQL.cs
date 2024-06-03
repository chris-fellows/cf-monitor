using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models.ActionItems;
using CFMonitor.Models.MonitorItems;
using CFUtilities.Databases;
using System.Threading.Tasks;

namespace CFMonitor.Actioners
{
    /// <summary>
    /// Actions running SQL
    /// </summary>
    public class ActionerSQL : IActioner
    {
        public string Name => "Execute SQL";

        public ActionerTypes ActionerType => ActionerTypes.SQL;

        public Task ExecuteAsync(MonitorItem monitorItem, ActionItem actionItem, ActionParameters actionParameters)
        {
            ActionSQL actionSQL = (ActionSQL)actionItem;
            OleDbDatabase database = new OleDbDatabase(actionSQL.ConnectionString);
            database.Open();            
            database.ExecuteNonQuery(System.Data.CommandType.Text, actionSQL.SQL, null);
            database.Close();

            return Task.CompletedTask;
        }

        public bool CanExecute(ActionItem actionItem)
        {
            return actionItem is ActionSQL;
        }
    }
}
