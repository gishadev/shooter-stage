namespace gishadev.Shooter.UserCamera.Modules
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