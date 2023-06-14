namespace gishadev.Shooter.Core
{
    public interface ICameraControlModule
    {
        void Tick();
        void OnStart();
        void OnStop();
    }
}