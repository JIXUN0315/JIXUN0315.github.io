using Hatsukoi.Hubs;
using Hatsukoi.Repository.DataContext;
using Hatsukoi.Repository.DBRepository;
using Hatsukoi.Repository.EntityModel;
using Hatsukoi.Repository.Interface;
using Microsoft.AspNetCore.SignalR;
using System.Net.WebSockets;

namespace Hatsukoi.Service
{
    public class TimedHostedService /*: IHostedService, IDisposable*/
    {
        //private readonly IHubContext<ChatHub> _hubContext;
        //private int executionCount = 0;
        //private readonly ILogger<TimedHostedService> _logger;
        //private Timer? _timer = null;
        //private readonly IServiceProvider _services;

        //public TimedHostedService(
        //    ILogger<TimedHostedService> logger,
        //    IHubContext<ChatHub> hubContext,
        //    IServiceProvider services)
        //{
        //    _logger = logger;
        //    _hubContext = hubContext;
        //    _services = services;
        //}
        //public Task StartAsync(CancellationToken stoppingToken)
        //{
        //    _logger.LogInformation("Timed Hosted Service running.");

        //    _timer = new Timer(DoWork, null, TimeSpan.FromSeconds(1 * 60),
        //        TimeSpan.FromSeconds(30));

        //    return Task.CompletedTask;
        //}

        //private async void DoWork(object? state)
        //{
   
        //    using (var scope = _services.CreateScope())
        //    {
        //        var count = Interlocked.Increment(ref executionCount);
        //        List<CenterNotify> lst = new List<CenterNotify>();
        //        var dbCtx = scope.ServiceProvider.GetRequiredService<HatsukoiContext>();
        //        lst = dbCtx.CenterNotifies.Where(x => x.IsSend == false && x.SendTime < DateTime.UtcNow).ToList();
    

        //        if (lst.Count > 0)
        //        {
        //            foreach (var item in lst)
        //            {
        //                item.IsSend = true;
        //                dbCtx.CenterNotifies.Update(item);
        //                dbCtx.SaveChanges();
        //                await _hubContext.Clients.All.SendAsync("CenterSend", "send");
        //            }
        //        }
        //    }
        //}

        //public Task StopAsync(CancellationToken stoppingToken)
        //{
        //    _logger.LogInformation("Timed Hosted Service is stopping.");

        //    _timer?.Change(Timeout.Infinite, 0);

        //    return Task.CompletedTask;
        //}

        //public void Dispose()
        //{
        //    _timer?.Dispose();
        //}
    }
}
