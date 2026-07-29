namespace PrintProxy.Hub.Services
{
    public class EmergencyManager
    {

        private readonly ILogger<EmergencyManager> _logger;
        private readonly IServiceProvider _ServiceProvider;

        public static bool SoftStop = true;

        private bool State = false;

        public EmergencyManager(ILogger<EmergencyManager> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _ServiceProvider = serviceProvider;
        }

        public delegate void EmergencyListener(bool state);
        public event EmergencyListener OnAlarm = null!;
        
        public async Task TripAlarmAsync()
        {

            if (State)
            {
                return;
            }

            using var scope = _ServiceProvider.CreateScope();
            IPrinterFactory printerFactory = scope.ServiceProvider.GetRequiredService<IPrinterFactory>();

            State = true;
            if (OnAlarm != null)
            {
                OnAlarm.Invoke(true);
            }

            _logger.LogCritical("Emergency stop tripped. Sending messages to printers.");

            foreach (var printer in printerFactory.GetPrinters())
            {
                if (SoftStop)
                {
                    await printer.PauseAsync();
                }
                else
                {
                    await printer.StopAsync();
                }
            }
        }

        public async Task StopAlarmAsync()
        {

            if (!State)
            {
                return;
            }

            using var scope = _ServiceProvider.CreateScope();
            IPrinterFactory printerFactory = scope.ServiceProvider.GetRequiredService<IPrinterFactory>();

            State = false;
            if (OnAlarm != null)
            {
                OnAlarm.Invoke(false);
            }

            _logger.LogCritical("Emergency stop Disabled.");

            foreach (var printer in printerFactory.GetPrinters())
            {
                if (SoftStop)
                {
                    await printer.ContinueAsync();
                }
            }
        }

        public bool GetState() => State;
    }
}
