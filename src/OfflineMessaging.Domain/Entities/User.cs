using OfflineMessaging.Domain.Entities.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OfflineMessaging.Domain.Entities
{
    [Table("User", Schema = "dbo")]
    public class User : BaseEntity
    {
        [Required]
        [StringLength(150)]
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        [StringLength(150)]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        [InverseProperty("BlockerUser")]
        public List<Block> BlockerBlocks { get; set; }
        [InverseProperty("BlockedUser")]
        public List<Block> BlockedBlocks { get; set; }

        [InverseProperty("SenderUser")]
        public List<Message> SenderMessages { get; set; }
        [InverseProperty("ReceiverUser")]
        public List<Message> ReceiverMessages { get; set; }
    }
}
