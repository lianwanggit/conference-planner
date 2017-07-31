using System;
using System.Collections.Generic;
using System.Text;

namespace ConferenceDTO.Response
{
    public class TagResponse : Tag
    {
        public ICollection<Session> Sessions { get; set; } = new List<Session>();
    }
}
