using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SchoolBridge.Domain.Services.Abstraction
{
    public interface IBanUsserService: IOnInitService
    {
        void BanUser(string userId, string reason);
        Task BanAsync(string userId, string reason);


        void UnbanUser(string userId);
        Task UnbanAsync(string userId);
    }
}
