using System;
using System.Collections.Generic;
using System.Text;

namespace Illie.Chat
{
    class Message
    {
        public int Id { get; set; }
        public string MessageText { get; set; }
        public int RecipientId { get; set; }
        public int SenderId { get; set; }
    }
}
