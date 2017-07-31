using System;
using System.Collections.Generic;
using System.Text;

namespace ConferenceDTO.Response
{
    public class TrackResponse : Track
    {
        public Conference Conference { get; set; }

        public ICollection<Session> Sessions { get; set; } = new List<Session>();
    }
}
