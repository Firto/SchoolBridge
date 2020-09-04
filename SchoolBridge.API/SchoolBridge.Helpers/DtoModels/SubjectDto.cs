using SchoolBridge.Helpers.AddtionalClases.ValidatingService;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolBridge.Helpers.DtoModels
{
    public class SubjectDto
    {
        [PropValid("str-input", "str-sb-name")]
        public string SubjectName { get; set; }
        [PropValid("str-sb-comment")]
        public string Comment { get; set; }
        [PropValid("not-null", "number-day-sb")]
        public string DayNumber { get; set; }
        [PropValid("not-null", "number-lesson-sb")]
        public string LessonNumber { get; set; }
    }
}
