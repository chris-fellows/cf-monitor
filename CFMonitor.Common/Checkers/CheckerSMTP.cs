﻿using CFMonitor.Enums;
using CFMonitor.Interfaces;
using CFMonitor.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Net.Security;
using System.Threading.Tasks;

namespace CFMonitor.Checkers
{
    /// <summary>
    /// Checks SMTP server
    /// </summary>
    public class CheckerSMTP : CheckerBase, IChecker
    {        
        public CheckerSMTP(IEventItemService eventItemService,
                        ISystemValueTypeService systemValueTypeService) : base(eventItemService, systemValueTypeService)
        {
            
        }

        public string Name => "SMTP";

        //public CheckerTypes CheckerType => CheckerTypes.SMTP;

        public Task CheckAsync(MonitorItem monitorItem, List<IActioner> actionerList, bool testMode)
        {
            return Task.Factory.StartNew(async () =>
            {
                // Get event items
                var eventItems = _eventItemService.GetByMonitorItemId(monitorItem.Id).Where(ei => ei.ActionItems.Any()).ToList();
                if (!eventItems.Any())
                {
                    return;
                }

                var systemValueTypes = _systemValueTypeService.GetAll();

                var svtServer = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_SMTPServer);
                var serverParam = monitorItem.Parameters.First(p => p.SystemValueTypeId == svtServer.Id);

                var svtPort = systemValueTypes.First(svt => svt.ValueType == SystemValueTypes.MIP_SMTPPort);
                var portParam = monitorItem.Parameters.First(p => p.SystemValueTypeId == svtPort.Id);

                Exception exception = null;
                ActionParameters actionParameters = new ActionParameters();

                try
                {
                    using (var client = new TcpClient())
                    {

                        //var server = "smtp.gmail.com";
                        //var port = 465;
                        client.Connect(serverParam.Value, Convert.ToInt32(portParam.Value));
                        // As GMail requires SSL we should use SslStream
                        // If your SMTP server doesn't support SSL you can
                        // work directly with the underlying stream
                        using (var stream = client.GetStream())
                        using (var sslStream = new SslStream(stream))
                        {
                            sslStream.AuthenticateAsClient(serverParam.Value);
                            using (var writer = new StreamWriter(sslStream))
                            using (var reader = new StreamReader(sslStream))
                            {
                                writer.WriteLine("EHLO " + serverParam.Value);
                                writer.Flush();
                                Console.WriteLine(reader.ReadLine());
                                // GMail responds with: 220 mx.google.com ESMTP
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                try
                {
                    foreach (var eventItem in eventItems)
                    {
                        await CheckEventAync(eventItem, actionerList, monitorItem, actionParameters, exception);
                    }
                }
                catch (Exception ex)
                {

                }
            });
        }

        public bool CanCheck(MonitorItem monitorItem)
        {
            return monitorItem.MonitorItemType == MonitorItemTypes.SMTP;
        }

        private async Task CheckEventAync(EventItem eventItem, List<IActioner> actionerList, MonitorItem monitorSMTP, ActionParameters actionParameters, Exception exception)
        {            
                bool meetsCondition = false;

                switch (eventItem.EventCondition.SourceValueType)
                {
                    case SystemValueTypes.ECS_Exception:
                        meetsCondition = eventItem.EventCondition.IsValid(exception != null);
                        break;                    
                }

                if (meetsCondition)
                {
                    foreach (ActionItem actionItem in eventItem.ActionItems)
                    {
                        await ExecuteActionAsync(actionerList, monitorSMTP, actionItem, actionParameters);
                    }
                }         
        }

        //private void DoAction(List<IActioner> actionerList, MonitorItem monitorItem, ActionItem actionItem, ActionParameters actionParameters)
        //{
        //    foreach (IActioner actioner in actionerList)
        //    {
        //        if (actioner.CanExecute(actionItem))
        //        {
        //            actioner.ExecuteAsync(monitorItem, actionItem, actionParameters);
        //            break;
        //        }
        //    }
        //}
    }
}
