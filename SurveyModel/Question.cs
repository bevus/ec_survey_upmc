using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SurveyModel
{
    public class Question
    {
        private static int controlIdCount = 0;
        private string controlId;
        public Question()
        {
            controlId = "control_" + controlIdCount++;
        }

        public int Id { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public int MaxSize { get; set; }
        public int ChoiceCount { get; set; }
        public bool IsMandatory { get; set; }
        public string Number { get; set; }
        public int PageNumber { get; set; }
        public int BlockNumber { get; set; }
        public string ControlType { get; set; }
        public string Prefix { get; set; }
        public string ControlId
        {
            get
            {
                return controlId;
            }
            set
            {
                controlId = value;
            }
        }
        public string Category { get; set; }
        public string Column { get; set; }
        public List<Choice> Choices { get; set; }
        public string Answer { get; set; }
        public List<SubQuestion> SubQuestions { get; set; }
    }
}
    