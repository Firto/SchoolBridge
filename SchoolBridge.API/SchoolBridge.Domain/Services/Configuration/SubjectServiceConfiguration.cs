using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolBridge.Domain.Services.Configuration
{
    public class SubjectServiceConfiguration
    {
        public short MaxCountCharsName { get; set; } = 25;
        public short MinCountCharsName { get; set; } = 1;
        public short MaxCountCharsComment { get; set; } = 80;
        public byte MinDayNumber { get; set; } = 0;
        public byte MaxDayNumber { get; set; } = 8;
        public byte MinLessonNumber { get; set; } = 0;
        public byte MaxLessonNumber { get; set; } = 9;
    }
}
