using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyModel
{
    public class Choice
    {
        public int Id { get; set; }
        public String Label { get; set; }
        public int Order { get; set; }
        public String Value { get; set; }
    }
}
