using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using System.Threading.Tasks;

namespace CFMonitor.Actioners
{
    /// <summary>
    /// Actions running SQL
    /// </summary>
    public class ActionerSQL : IActioner
    {
        private readonly ISystemValueTypeService _systemValueTypeService;

        public ActionerSQL(ISystemValueTypeService systemValueTypeService)
        {
            _systemValueTypeService = systemValueTypeService;
        }

        public string Name => "Execute SQL";

        //public ActionerTypes ActionerType => ActionerTypes.SQL;

        public Task ExecuteAsync(MonitorItem monitorItem, ActionItem actionItem, List<ActionItemParameter> parameters)
        {
            var systemValueTypes = _systemValueTypeService.GetAll();

            var connectionStringParam = actionItem.Parameters.First(p => p.SystemValueTypeId == systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.AIP_SQLConnectionString).Id);
            var sqlParam = actionItem.Parameters.First(p => p.SystemValueTypeId == systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.AIP_SQLSQL).Id);

            ////ActionSQL actionSQL = (ActionSQL)actionItem;
            //OleDbDatabase database = new OleDbDatabase(connectionStringParam.Value);
            //database.Open();            
            //database.ExecuteNonQuery(System.Data.CommandType.Text, sqlParam.Value, null);
            //database.Close();

            return Task.CompletedTask;
        }

        public bool CanExecute(ActionItem actionItem)
        {
            return actionItem.ActionerType == ActionerTypes.SQL;
        }
    }
}
