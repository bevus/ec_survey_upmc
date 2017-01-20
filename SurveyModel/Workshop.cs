using System;
using System.Collections.Generic;

namespace SurveyModel
{
    public class Workshop : EventActivity
    {
        public int etat { get; set; }
        public DateTime attended_date { get; set; }
        public string theme { get; set; }
        public string description { get; set; }
        public int id_atelier { get; set; }
        public List<Person> attenders { get; set; }
    }
}
