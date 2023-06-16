namespace gishadev.Shooter.Camera
{
    public interface ICameraControlModule
    {
        void Tick();
        void OnStart();
        void OnStop();

        void Init();
        bool IsInitialized { get; }
    }
}