using CFMonitor.Constants;
using CFMonitor.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.SystemTask
{
    /// <summary>
    /// Manages password resets. Deletes old instances that have expired.
    /// </summary>
    public class ManagePasswordResetsSystemTask : ISystemTask
    {
        public string Name => SystemTaskTypeNames.ManagePasswordResets;

        public async Task ExecuteAsync(Dictionary<string, object> parameters, IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            var passwordResetService = serviceProvider.GetRequiredService<IPasswordResetService>();

            // Get expired password resets
            var passwordResets = (await passwordResetService.GetAllAsync()).Where(pr => pr.ExpiresDateTime.AddMinutes(60) <= DateTimeOffset.UtcNow).ToList();

            // Delete
            while (passwordResets.Any() && !cancellationToken.IsCancellationRequested)
            {
                var passwordReset = passwordResets.First();
                passwordResets.Remove(passwordReset);

                await passwordResetService.DeleteByIdAsync(passwordReset.Id);
            }
        }
    }
}
}
