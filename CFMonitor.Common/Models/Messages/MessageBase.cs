﻿namespace CFMonitor.Models.Messages
{
    public abstract class MessageBase
    {
        /// <summary>
        /// Unique Id
        /// </summary>
        public string Id { get; set; } = String.Empty;

        /// <summary>
        /// Message type Id
        /// </summary>
        public string TypeId { get; set; } = String.Empty;

        /// <summary>
        /// Response (if any)
        /// </summary>
        public MessageResponse? Response { get; set; }

        /// <summary>
        /// Agent who sent
        /// </summary>
        public string SenderAgentId { get; set; } = String.Empty;

        /// <summary>
        /// Security key
        /// </summary>
        public string SecurityKey { get; set; } = String.Empty;
    }
}
