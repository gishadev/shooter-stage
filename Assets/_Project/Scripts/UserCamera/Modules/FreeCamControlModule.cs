using gishadev.Shooter.Core;
using UnityEngine;

namespace gishadev.Shooter.UserCamera.Modules
{
    public class FreeCamControlModule : ICameraControlModule
    {
        public bool IsInitialized { get; private set; }

        private readonly Transform _transform;
        private readonly Transform _offsetRig;
        private readonly GameDataSO _gameDataSo;

        private float _xRot;

        private readonly Camera _cam;

        public FreeCamControlModule(Transform transform, Transform offsetRig, GameDataSO gameDataSO)
        {
            _transform = transform;
            _offsetRig = offsetRig;
            _gameDataSo = gameDataSO;

            _cam = Camera.main;
        }

        public void Tick()
        {
            HandleMovement();
            HandleCameraRotation();
        }

        public void OnStart()
        {
            Cursor.lockState = CursorLockMode.Locked;
            _xRot = _cam.transform.localRotation.eulerAngles.x;
        }

        public void OnStop()
        {
        }

        public void Init()
        {
            _transform.localPosition = _cam.transform.localPosition;
            _offsetRig.localPosition = Vector3.zero;
            _cam.transform.localPosition = Vector3.zero;

            IsInitialized = true;
        }

        private void HandleMovement()
        {
            var hInput = Input.GetAxis("Horizontal");
            var vInput = Input.GetAxis("Vertical");

            var moveInput = _cam.transform.forward * vInput + _cam.transform.right * hInput;
            _transform.Translate(moveInput * (_gameDataSo.FreeCamMoveSpeed * Time.deltaTime), Space.World);
        }

        private void HandleCameraRotation()
        {
            float mouseX = Input.GetAxis("Mouse X") * _gameDataSo.FreeCamRotationMouseSens * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * _gameDataSo.FreeCamRotationMouseSens * Time.deltaTime;

            _xRot -= mouseY;
            _xRot = Mathf.Clamp(_xRot, -90f, 90f);

            _cam.transform.localRotation = Quaternion.Euler(_xRot, 0f, 0f);

            var yRot = mouseX;
            _transform.transform.Rotate(Vector3.up * yRot);
        }
    }
}