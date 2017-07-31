using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConferenceDTO
{
    public class Attendee
    {
        public int ID { get; set; }

        public int? AvatarId { get; set; }

        [ForeignKey("AvatarId")]
        public virtual File Avatar { get; set; }

        [Required]
        [StringLength(200)]
        public virtual string FirstName { get; set; }

        [Required]
        [StringLength(200)]
        public virtual string LastName { get; set; }

        [Required]
        [StringLength(200)]
        public string UserName { get; set; }

        [StringLength(256)]
        public virtual string EmailAddress { get; set; }
    }
}
