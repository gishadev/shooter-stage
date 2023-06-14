using System;
using UnityEngine;

namespace gishadev.Shooter.Core
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private GameDataSO gameDataSO;
        
        private CameraMode _currentCameraMode;
        private ICameraControlModule _currentControlModule;

        public event Action<CameraMode> CameraModeChanged;

        private void Start()
        {
            ChangeCameraMode(CameraMode.Orbital);
        }

        private void Update()
        {
            _currentControlModule.Tick();
        }

        private void OnDisable()
        {
            _currentControlModule.OnStop();
        }

        public void ChangeCameraMode(CameraMode cameraMode)
        {
            _currentCameraMode = cameraMode;
            if (_currentControlModule != null)
                _currentControlModule.OnStop();

            switch (_currentCameraMode)
            {
                case CameraMode.Orbital:
                    _currentControlModule = new OrbitalControlModule(transform, gameDataSO);
                    break;
                case CameraMode.FreeCam:
                    _currentControlModule = new OrbitalControlModule(transform, gameDataSO);
                    break;
            }

            if (_currentControlModule == null)
            {
                Debug.LogError("Current control module is null!");
                return;
            }

            _currentControlModule.OnStart();
            CameraModeChanged?.Invoke(cameraMode);
        }
    }
}