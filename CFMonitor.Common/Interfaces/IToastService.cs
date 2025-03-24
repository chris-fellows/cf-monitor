﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFMonitor.Interfaces
{
    /// <summary>
    /// Toast service.
    /// </summary>
    public interface IToastService
    { 
        /// <summary>
        /// Registers for notification of Information calls. Ensure that UnregisterInformation is called when dependent
        /// component is disposed.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        string RegisterInformation(Action<string> action);

        /// <summary>
        /// Unregistered notifcation
        /// </summary>
        /// <param name="id"></param>
        void UnregisterInformation(string id);

        /// <summary>
        /// Displays information toast
        /// </summary>
        /// <param name="text"></param>
        /// <param name="time"></param>
        void Information(string text);
    }
}
