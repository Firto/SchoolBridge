using SchoolBridge.DataAccess.Entities.Authorization;
using SchoolBridge.Domain.Services.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolBridge.Domain.Hostings
{
    public class TokenRemoveHosting : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly IServiceProvider _serviceProvider;
        private readonly TokenServiceConfiguration _tokenServiceConfiguration;
        public TokenRemoveHosting(IServiceProvider serviceProvider,
                                  TokenServiceConfiguration tokenServiceConfiguration)
        {
            _serviceProvider = serviceProvider;
            _tokenServiceConfiguration = tokenServiceConfiguration;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                _tokenServiceConfiguration.RefreshTokenRemove);

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                DbContext _db = scope.ServiceProvider.GetRequiredService<DbContext>();
                _db.Set<ActiveRefreshToken>().RemoveRange(_db.Set<ActiveRefreshToken>().Where((x) => x.Expire < DateTime.Now));
                _db.SaveChanges();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
            => _timer?.Dispose();
    }
}