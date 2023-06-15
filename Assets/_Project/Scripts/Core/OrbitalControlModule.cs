﻿using UnityEngine;

namespace gishadev.Shooter.Core
{
    public class OrbitalControlModule : ICameraControlModule
    {
        private readonly Transform _rig;
        private readonly GameDataSO _gameDataSo;
        private readonly Transform _offsetRig;

        private float _yDeltaRotation;
        private float _zoomValue;
        private Vector3 _newPos;
        private Quaternion _newRotation;

        private Vector3 _dragStartPos, _dragCurrentPos;
        private Vector3 _rotateStartPos, _rotateCurrentPos;
        private float _maxZoomSize, _zoomStep;
        private Vector3 _topBorder, _bottomBorder;

        public OrbitalControlModule(Transform rig, Transform offsetRig, GameDataSO gameDataSO)
        {
            _rig = rig;
            _gameDataSo = gameDataSO;
            _offsetRig = offsetRig;
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
            _yDeltaRotation = _rig.rotation.eulerAngles.y;

            _newPos = _rig.position;
            _newRotation = _rig.rotation;

            _maxZoomSize = _gameDataSo.MaxZoomSize;
            _zoomValue = Camera.main.transform.position.y;
            _zoomStep = (_maxZoomSize - _gameDataSo.MinZoomSize) / _gameDataSo.ZoomSteps;

            _topBorder = new Vector3(50f, 0f, 50f);
            _bottomBorder = _topBorder * -1f;
        }

        public void OnStop()
        {
        }

        private void HandleMovement()
        {
            if (Input.GetMouseButtonDown(1))
            {
                Plane plane = new Plane(Vector3.up, Vector3.zero);

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                float entry;

                if (plane.Raycast(ray, out entry))
                    _dragStartPos = ray.GetPoint(entry);
            }

            if (Input.GetMouseButton(1))
            {
                Plane plane = new Plane(Vector3.up, Vector3.zero);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (plane.Raycast(ray, out var entry))
                {
                    _dragCurrentPos = ray.GetPoint(entry);

                    _newPos += (_dragStartPos - _dragCurrentPos) * _gameDataSo.OrbitalMovementMouseSens;
                    _newPos.x = Mathf.Clamp(_newPos.x, _bottomBorder.x, _topBorder.x);
                    _newPos.z = Mathf.Clamp(_newPos.z, _bottomBorder.z, _topBorder.z);

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

                _yDeltaRotation = diff * _gameDataSo.OrbitalRotationMouseSens;
                _newRotation *= Quaternion.Euler(-Vector3.up * _yDeltaRotation);
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

            _zoomValue = Mathf.Clamp(zoomDelta + _zoomValue, _gameDataSo.MinZoomSize, _maxZoomSize);
            var zoomVector = new Vector3(0f, _zoomValue, -_zoomValue);
            Camera.main.transform.localPosition = Vector3.Lerp(Camera.main.transform.localPosition, zoomVector,
                Time.deltaTime * _gameDataSo.ZoomSmoothness);
        }
    }
}