using System;
using System.Collections.Generic;

namespace TechnicDemoUi
{
    public class BirthdayCelebration
    {
        private string _goodThoughts = string.Empty;
        public string MyDate { get; set; }
      public string Birthday { get; set; }
        public string BirthdayOfWeek { get; set; }
        public string AgeOfPerson { get; set; }

        public string AgeGroup { get; set; }

        public int CountOfGoodThought { get; set; }
        public string GoodThought { get { return _goodThoughts; }  set { _goodThoughts = value; } }
        public string ExceptionMessage { get; set; }
    
    }
}
