using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyModel
{
    public class SubQuestion : Question, ICloneable
    {
        public Question ParentQuestion { get; set; }
        public EventActivity Activity { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
