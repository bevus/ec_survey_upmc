using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyModel
{
    public class Session : EventActivity
    {
    public int etat { get; set; }
    public DateTime attended_date { get; set; }
    public DateTime creation_date { get; set; }
    public string theme { get; set; }
    public string description { get; set; }
    public int id_atelier { get; set; }
    public List<Person> attenders { get; set; }
    }
}
