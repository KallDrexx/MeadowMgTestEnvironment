using Meadow;
using Meadow.Hardware;

namespace MeadowMgTestEnvironment;

public class KeyboardPort : IDigitalInputPort
{
    private DigitalState? _state;
    
    public InterruptMode InterruptMode => InterruptMode.None;
    public TimeSpan DebounceDuration { get; set; } = TimeSpan.Zero;
    public TimeSpan GlitchDuration { get; set; } = TimeSpan.Zero;
    public ResistorMode Resistor { get; set; } = ResistorMode.Disabled;

    public bool State
    {
        get => _state?.State ?? false;
        set
        {
            var isChanged = _state?.State != value;
            if (isChanged)
            {
                var oldState = _state;
                var newState = new DigitalState(value, DateTime.UtcNow);
                _state = newState;

                Changed?.Invoke(this, new DigitalPortResult(newState, oldState));
            }
        }
    }
    
    public event EventHandler<DigitalPortResult>? Changed;

    public IDigitalChannelInfo Channel => new DigitalChannelInfo("test");

    public IPin Pin => throw new NotImplementedException();

    public IDisposable Subscribe(IObserver<IChangeResult<DigitalState>> observer)
    {
        throw new NotImplementedException();
    }
    
    public void Dispose()
    {
        
    }
}