using Serilog;

namespace HelpLine.BuildingBlocks.Bus.SQS;

internal class ConsumePlanner
{
    private bool _running = false;
    private ILogger _logger;
    private readonly TimeSpan _interval;
    private readonly List<Func<Task>> _handlers = new();

    public ConsumePlanner(TimeSpan interval, ILogger logger)
    {
        _interval = interval;
        _logger = logger;
    }


    public async Task Start(CancellationToken cancellationToken = default)
    {
        if (_running)
            return;
        
        _logger.Debug("Consume planner started");
        _running = true;

        while (true)
        {
            if (_running)
            {
                _logger.Debug("Next SQS consume batch operation");
                await Do();
                await Task.Delay(_interval, cancellationToken);
            }
        }
    }

    public void Stop()
    {
        _running = false;
        
    }

    public void Add(Func<Task> handler)
    {
        if (!_handlers.Contains(handler))
        {
            _handlers.Add(handler);
        }
    }
    
    public void Delete(Func<Task> handler)
    {
        _handlers.Remove(handler);
        if (_handlers.Count == 0)
            Stop();
    }

    private async Task Do()
    {
        var forCheck = _handlers.ToArray();
        foreach (var handler in forCheck)
        {
            try
            {
                await handler();
            }
            catch (Exception e)
            {
                _logger.Error(e,"Can't handle consume");
            }
        }
           
    }
}
