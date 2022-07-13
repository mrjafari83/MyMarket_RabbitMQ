using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.EndPoint
{
    internal class RabbitBackgroundService : BackgroundService
    {
        public RabbitBackgroundService(IServiceProvider serviceProvider)
        {
            Services = serviceProvider;
        }
        public IServiceProvider Services { get; set; }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            reciveCreatedExcel();
        }

        private void reciveCreatedExcel()
        {
            using (var scope = Services.CreateScope())
            {
                var recive = scope.ServiceProvider.GetRequiredService<IRecive>();
                recive.ReciveCreateExcel();
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }
    }
}
