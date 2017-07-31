using ConferenceDTO;

namespace BackEnd.Data
{
    public class SessionSpeaker
    {
        public int SessionID { get; set; }

        public Session Session { get; set; }

        public int SpeakerID { get; set; }

        public Speaker Speaker { get; set; }
    }
}
