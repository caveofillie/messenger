using System;
using System.Collections.Generic;
using System.Text;

namespace Illie.Chat
{
    class Message
    {
        public string messageText { get; set; }
        public int recipientId { get; set; }
        public int senderId { get; set; }
    }
}
