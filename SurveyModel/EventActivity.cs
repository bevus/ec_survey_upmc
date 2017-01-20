using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyModel
{
    public abstract class EventActivity
    {
        public List<Question> Questions { get; set; }
    }
}
