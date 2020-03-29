using OfflineMessaging.Domain.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OfflineMessaging.Domain.Entities
{
    [Table("Message", Schema = "dbo")]
    public class Message : BaseEntity
    {
        [ForeignKey("SenderUser")]
        public int SenderUserId { get; set; }
        [ForeignKey("ReceiverUser")]
        public int ReceiverUserId { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime? ReadDate { get; set; }

        public User SenderUser { get; set; }
        public User ReceiverUser { get; set; }
    }
}
