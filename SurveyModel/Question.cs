using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SurveyModel
{
    public class Question
    {
        public int Id { get; set; }
        public String Label { get; set; }
        public String Description { get; set; }
        public int Order { get; set; }
        public int MaxSize { get; set; }
        public int ChoiceCount { get; set; }
        public bool IsMandatory { get; set; }
        public String Number { get; set; }
        public int PageNumber { get; set; }
        public int BlockNumber { get; set; }
        public String ControlType { get; set; }
        public String Prefix { get; set; }
        public String ControlId
        {
            get
            {
                return Prefix + Id;
            }
        }
        public String Category { get; set; }
        public String Column { get; set; }
        public List<Choice> Choices { get; set; }
        public string Answer { get; set; }
    }
}
    