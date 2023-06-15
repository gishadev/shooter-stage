using System;
using UnityEngine;

namespace gishadev.Shooter.Core
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private GameDataSO gameDataSO;
        [SerializeField] private Transform offsetRig;

        private CameraMode _currentCameraMode;
        private ICameraControlModule _currentControlModule;

        private SavedTransforms _savedTransforms;
        public event Action<CameraMode> CameraModeChanged;

        private void Start()
        {
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

        public void ChangeCameraMode(CameraMode cameraMode)
        {
            _currentCameraMode = cameraMode;
            SavedTransforms transformsToLoad = _savedTransforms;

            if (_currentControlModule != null)
            {
                SaveTransforms();
                _currentControlModule.OnStop();
            }

            switch (_currentCameraMode)
            {
                case CameraMode.Orbital:
                    _currentControlModule = new OrbitalControlModule(transform, offsetRig, gameDataSO);
                    break;
                case CameraMode.FreeCam:
                    _currentControlModule = new FreeCamControlModule(transform, offsetRig, gameDataSO);
                    break;
            }

            if (_currentControlModule == null)
            {
                Debug.LogError("Current control module is null!");
                return;
            }

            _currentControlModule.OnStart();
            LoadTransforms(transformsToLoad);
            CameraModeChanged?.Invoke(cameraMode);
        }

        [ContextMenu("Change Camera Mode to Orbital")]
        public void ChangeToOrbital() => ChangeCameraMode(CameraMode.Orbital);

        [ContextMenu("Change Camera Mode to FreeCam")]
        public void ChangeToFree() => ChangeCameraMode(CameraMode.FreeCam);

        public void ChangeNext()
        {
            var index = (int) _currentCameraMode;
            index++;
            if (index > 1)
                index = 0;

            ChangeCameraMode((CameraMode) index);
        }

        private void SaveTransforms()
        {
            var transforms = transform.GetComponentsInChildren<Transform>();
            _savedTransforms = new SavedTransforms
            {
                previousSavedPositions = new Vector3[transforms.Length],
                previousSavedRotations = new Quaternion[transforms.Length]
            };

            for (var i = 0; i < transforms.Length; i++)
            {
                _savedTransforms.previousSavedPositions[i] = transforms[i].position;
                _savedTransforms.previousSavedRotations[i] = transforms[i].rotation;
            }
        }

        private void LoadTransforms(SavedTransforms allTransforms)
        {
            if (allTransforms.previousSavedPositions == null || allTransforms.previousSavedRotations == null)
                return;
            var sceneTransforms = transform.GetComponentsInChildren<Transform>();

            for (var i = 0; i < sceneTransforms.Length; i++)
            {
                sceneTransforms[i].position = allTransforms.previousSavedPositions[i];
                sceneTransforms[i].rotation = allTransforms.previousSavedRotations[i];
            }
        }
    }

    public struct SavedTransforms
    {
        public Vector3[] previousSavedPositions;
        public Quaternion[] previousSavedRotations;
    }
}