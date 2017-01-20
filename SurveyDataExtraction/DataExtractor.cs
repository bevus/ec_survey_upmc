using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;

namespace SurveyDataExtraction
{
    public class DataExtractor
    {
        public DataExtractor()
        {
            var manager = new Manager();
            manager.getPoll(1);
        }
    }
}
