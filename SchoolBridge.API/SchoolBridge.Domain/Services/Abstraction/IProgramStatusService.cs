using SchoolBridge.Helpers.AddtionalClases.ProgramStatusService;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolBridge.Domain.Services.Abstraction
{
    public interface IProgramStatusService: IMyService
    {
        ProgramStatus Status { get; set; }
    }
}
