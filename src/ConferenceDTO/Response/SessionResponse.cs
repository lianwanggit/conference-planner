using System;
using System.Collections.Generic;
using System.Text;

namespace ConferenceDTO.Response
{
    public class SessionResponse : Session
    {
        public Conference Conference { get; set; }

        public Track Track { get; set; }

        public ICollection<Speaker> Speakers { get; set; } = new List<Speaker>();

        public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    }
}
