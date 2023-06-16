using gishadev.Shooter.Core;
using UnityEngine;

namespace gishadev.Shooter.UserCamera.Modules
{
    public class OrbitalControlModule : ICameraControlModule
    {
        public bool IsInitialized { get; private set; }

        private readonly Transform _rig;
        private readonly GameDataSO _gameDataSo;
        private readonly Transform _offsetRig;

        private Vector3 _newPos;
        private Quaternion _newRotation;

        private Vector3 _dragStartPos, _dragCurrentPos;
        private Vector3 _rotateStartPos, _rotateCurrentPos;
        private readonly float _zoomStep;

        private readonly Camera _cam;

        public OrbitalControlModule(Transform rig, Transform offsetRig, GameDataSO gameDataSO)
        {
            _rig = rig;
            _gameDataSo = gameDataSO;
            _offsetRig = offsetRig;

            _cam = Camera.main;

            _zoomStep = (_gameDataSo.MaxZoomSize - _gameDataSo.MinZoomSize) / _gameDataSo.ZoomSteps;
        }

        public void Tick()
        {
            HandleMovement();
            HandleOrbitalRotation();
            HandleZoomChange();
        }

        public void OnStart()
        {
            Cursor.lockState = CursorLockMode.None;

            _newPos = _rig.position;
            _newRotation = _rig.rotation;
        }

        public void OnStop()
        {
        }

        public void Init()
        {
            IsInitialized = true;
        }

        private void HandleMovement()
        {
            var plane = new Plane(Vector3.up, Vector3.zero);
            var ray = _cam.ScreenPointToRay(Input.mousePosition);

            if (Input.GetMouseButtonDown(1))
                if (plane.Raycast(ray, out var entry))
                    _dragStartPos = ray.GetPoint(entry);

            if (Input.GetMouseButton(1))
            {
                if (plane.Raycast(ray, out var entry))
                {
                    _dragCurrentPos = ray.GetPoint(entry);

                    _newPos += (_dragStartPos - _dragCurrentPos) * _gameDataSo.OrbitalMovementMouseSens;

                    _rig.position = Vector3.Lerp(_rig.position, _newPos,
                        Time.deltaTime * _gameDataSo.OrbitalMovementSmoothness);
                }
            }
        }

        private void HandleOrbitalRotation()
        {
            if (Input.GetMouseButtonDown(0))
                _rotateStartPos = Input.mousePosition;
            if (Input.GetMouseButton(0))
            {
                _rotateCurrentPos = Input.mousePosition;

                float diff = _rotateStartPos.x - _rotateCurrentPos.x;
                _rotateStartPos = _rotateCurrentPos;

                var yDeltaRotation = diff * _gameDataSo.OrbitalRotationMouseSens;
                _newRotation *= Quaternion.Euler(-Vector3.up * yDeltaRotation);
            }

            _rig.rotation = Quaternion.Slerp(_rig.rotation, _newRotation,
                Time.deltaTime * _gameDataSo.OrbitalRotationSmoothness);
        }

        private void HandleZoomChange()
        {
            if (Input.mouseScrollDelta.magnitude < 0.1f)
                return;

            float zoomDelta = 0f;
            if (Input.mouseScrollDelta.y > 0f)
                zoomDelta = -_zoomStep;
            else if (Input.mouseScrollDelta.y < 0f)
                zoomDelta = _zoomStep;

            var zoomValue = Mathf.Clamp(zoomDelta + _cam.transform.position.y, _gameDataSo.MinZoomSize,
                _gameDataSo.MaxZoomSize);
            var zoomVector = new Vector3(0f, zoomValue, -zoomValue);
            _cam.transform.localPosition = Vector3.Lerp(_cam.transform.localPosition, zoomVector,
                Time.deltaTime * _gameDataSo.ZoomSmoothness);
        }
    }
}