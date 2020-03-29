using OfflineMessaging.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace OfflineMessaging.Domain.Entities
{
    [Table("Block", Schema = "dbo")]
    public class Block : BaseEntity
    {
        [ForeignKey("BlockerUser")]
        public int BlockerUserId { get; set; }
        [ForeignKey("BlockedUser")]
        public int BlockedUserId { get; set; }

        public User BlockerUser { get; set; }
        public User BlockedUser { get; set; }
    }
}
