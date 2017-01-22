using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyModel
{
   public  class Atelier
    {
        public int id_atelier { get; set; }
        public int id_event { get; set; }
        public string theme { get; set; }
        public string description { get; set; }

        public List<Person> participants { get; set; }

    }
}
