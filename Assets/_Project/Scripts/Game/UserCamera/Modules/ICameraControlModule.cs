﻿namespace gishadev.Shooter.Game.UserCamera.Modules
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