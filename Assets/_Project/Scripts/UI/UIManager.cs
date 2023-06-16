using System.Collections.Generic;
using gishadev.Shooter.UserCamera;
using TMPro;
using UnityEngine;

namespace gishadev.Shooter.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text cameraModeTMP;
        [SerializeField] private TMP_Text cameraPositionTMP;
        [SerializeField] private TMP_Text fpsTMP;
        [Space] [SerializeField] private GameObject orbitalTutorial;
        [SerializeField] private GameObject freeCamTutorial;

        [Space] [SerializeField] private CameraController cameraController;

        private List<GameObject> _childrenUI = new List<GameObject>();
        private Camera _cam;

        private void Awake()
        {
            cameraController.CameraModeChanged += OnCameraModeChanged;
            _cam = Camera.main;

            for (int i = 0; i < transform.childCount; i++)
                _childrenUI.Add(transform.GetChild(i).gameObject);
        }

        private void OnDisable()
        {
            cameraController.CameraModeChanged -= OnCameraModeChanged;
        }

        private void OnCameraModeChanged(CameraMode cameraMode)
        {
            cameraModeTMP.text = cameraMode.ToString();

            freeCamTutorial.SetActive(false);
            orbitalTutorial.SetActive(false);
            switch (cameraMode)
            {
                case CameraMode.Orbital:
                    orbitalTutorial.SetActive(true);
                    break;
                case CameraMode.FreeCam:
                    freeCamTutorial.SetActive(true);
                    break;
            }
        }

        private void Update()
        {
            HandleGeneralInformation();
            HandleUIActivation();
        }

        private void HandleUIActivation()
        {
            if (Input.GetKeyDown(KeyCode.H))
                foreach (var child in _childrenUI)
                    child.SetActive(!child.activeSelf);
        }

        private void HandleGeneralInformation()
        {
            var pos = _cam.transform.position;
            cameraPositionTMP.text = $"<{pos.x:0.00},{pos.y:0.00},{pos.z:0.00}>";

            var avgFrameRate = Mathf.RoundToInt(Time.frameCount / Time.time);
            fpsTMP.text = avgFrameRate.ToString();
        }
    }
}