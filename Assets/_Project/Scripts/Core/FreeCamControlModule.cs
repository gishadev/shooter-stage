using UnityEngine;

namespace gishadev.Shooter.Core
{
    public class FreeCamControlModule : ICameraControlModule
    {
        private readonly Transform _transform;
        private readonly Transform _offsetRig;
        private readonly GameDataSO _gameDataSo;

        private float _xRot, _yRot;
        private float _hInput, _vInput;

        public FreeCamControlModule(Transform transform, Transform offsetRig, GameDataSO gameDataSO)
        {
            _transform = transform;
            _offsetRig = offsetRig;
            _gameDataSo = gameDataSO;
        }

        public void Tick()
        {
            HandleMovement();
            HandleCameraRotation();
        }

        public void OnStart()
        {
            Cursor.lockState = CursorLockMode.Locked;

            _offsetRig.localPosition = Vector3.zero;
            Camera.main.transform.localPosition = Vector3.zero;
        }

        public void OnStop()
        {
        }

        private void HandleMovement()
        {
            _hInput = Input.GetAxis("Horizontal");
            _vInput = Input.GetAxis("Vertical");

            var moveInput = Camera.main.transform.forward * _vInput + Camera.main.transform.right * _hInput;
            _transform.Translate(moveInput * (_gameDataSo.FreeCamMoveSpeed * Time.deltaTime), Space.World);
        }

        private void HandleCameraRotation()
        {
            float mouseX = Input.GetAxis("Mouse X") * _gameDataSo.FreeCamRotationMouseSens * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * _gameDataSo.FreeCamRotationMouseSens * Time.deltaTime;

            _xRot -= mouseY;
            _xRot = Mathf.Clamp(_xRot, -90f, 90f);

            Camera.main.transform.localRotation = Quaternion.Euler(_xRot, 0f, 0f);
            _yRot = mouseX;
            _transform.transform.Rotate(Vector3.up * _yRot);
        }
    }
}