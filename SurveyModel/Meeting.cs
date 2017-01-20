using System;
using System.Collections.Generic;

namespace SurveyModel
{
    public class Meeting : EventActivity
    {
        public string company_name { get; set; }
        public int id_company { get; set; }
        public Person owner;
        public List<Person> guests { get; set; }
        public DateTime date_start { get; set; }
        public DateTime date_end { get; set; }
        public string location_name { get; set; }
        public int id_meeting { get; set; }
    }
}
