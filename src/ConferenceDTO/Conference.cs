using System;
using System.ComponentModel.DataAnnotations;

namespace ConferenceDTO
{
    public class Conference
    {
        public int ID { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [Required]
        [StringLength(200)]
        public string Slug { get; set; }

        [DataType(DataType.Date)]
        public virtual DateTimeOffset? StartDate { get; set; }

        [DataType(DataType.Date)]
        public virtual DateTimeOffset? EndDate { get; set; }
    }
}
