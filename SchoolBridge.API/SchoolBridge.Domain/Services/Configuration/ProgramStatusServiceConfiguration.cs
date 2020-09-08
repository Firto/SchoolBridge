using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Helpers.AddtionalClases.ProgramStatusService;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolBridge.Domain.Services.Configuration
{
    public class ProgramStatusServiceConfiguration: IMyService
    {
        public string CurrentStatusPath { get; set; }
        public ProgramStatus DefaultStatus { get; set; } = new ProgramStatus();
    }
}
