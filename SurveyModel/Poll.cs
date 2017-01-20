using System.Collections.Generic;

namespace SurveyModel
{
    public class Poll
    { 
        public int Id { get; set; }
        public int SurveyId { get; set; }
        public int EventId { get; set; }
        public string EventName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ExternalId { get; set; }
        public string TableName { get; set; }
        public string TableMeetingName { get; set; }
        public string TableWsName { get; set; }
        public string TableSessionName { get; set; }
        public List<Question> Questions { get; set; }
        public Dictionary<int, string> Blocks { get; set; }
        public List<Meeting> Meetings { get; set; }
        public List<Session> Sessions { get; set; }
        public List<Workshop> Workshops { get; set; }
        public int PersonId { get; set; }
    }
}
