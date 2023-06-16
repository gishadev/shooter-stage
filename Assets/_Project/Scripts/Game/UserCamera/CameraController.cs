using System;
using DG.Tweening;
using gishadev.Shooter.Game.Core;
using gishadev.Shooter.Game.UserCamera.Modules;
using UnityEngine;

namespace gishadev.Shooter.Game.UserCamera
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private GameDataSO gameDataSO;
        [SerializeField] private Transform offsetRig;

        private ICameraControlModule _currentControlModule;
        private ICameraControlModule _orbitalControlModule, _freeCamControlModule;

        private CameraMode _currentCameraMode;
        private CameraTransformsSaver _cameraTransformsSaver;
        public event Action<CameraMode> CameraModeChanged;

        private void Awake()
        {
            _orbitalControlModule = new OrbitalControlModule(transform, offsetRig, gameDataSO);
            _freeCamControlModule = new FreeCamControlModule(transform, offsetRig, gameDataSO);
        }

        private void Start()
        {
            _cameraTransformsSaver = new CameraTransformsSaver(transform);
            ChangeCameraMode(CameraMode.Orbital);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.G))
                ChangeNext();

            _currentControlModule.Tick();
        }

        private void OnDisable()
        {
            _currentControlModule.OnStop();
        }

        private void ChangeCameraMode(CameraMode cameraMode)
        {
            _currentCameraMode = cameraMode;

            var currentTransformsToLoad = _cameraTransformsSaver.SavedTransforms;
            _currentControlModule?.OnStop();
            _cameraTransformsSaver.SaveTransforms();

            switch (_currentCameraMode)
            {
                case CameraMode.Orbital:
                    _currentControlModule = _orbitalControlModule;
                    break;
                case CameraMode.FreeCam:
                    _currentControlModule = _freeCamControlModule;
                    break;
                default:
                    Debug.LogError("Current control module was not setup!");
                    return;
            }

            // Using DoTween sequence for creating smooth transitional animations. 
            var seq = _cameraTransformsSaver.LoadTransforms(currentTransformsToLoad);
            seq.OnComplete(() =>
            {
                _currentControlModule.OnStart();
                if (!_currentControlModule.IsInitialized)
                    _currentControlModule.Init();

                CameraModeChanged?.Invoke(cameraMode);
            });
        }

        [ContextMenu("Change Camera Mode to Orbital")]
        public void ChangeToOrbital() => ChangeCameraMode(CameraMode.Orbital);

        [ContextMenu("Change Camera Mode to FreeCam")]
        public void ChangeToFree() => ChangeCameraMode(CameraMode.FreeCam);

        private void ChangeNext()
        {
            var index = (int) _currentCameraMode;
            index++;
            if (index > 1)
                index = 0;

            ChangeCameraMode((CameraMode) index);
        }
    }
}