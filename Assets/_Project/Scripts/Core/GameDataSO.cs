using UnityEngine;

namespace gishadev.Shooter.Core
{
    [CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData", order = 0)]
    public class GameDataSO : ScriptableObject
    {
        [Header("Orbital Camera Settings")]
        [Header("Movement/Rotation")]
        [SerializeField] private float rotationMouseSens = 0.25f;

        [SerializeField] private float rotationSmoothness = 250f;
        [SerializeField] private float movementMouseSens = 1f;
        [SerializeField] private float movementSmoothness = 125f;

        [Header("Zoom")] 
        [SerializeField] private float zoomSmoothness = 255f;
        [SerializeField] private float zoomSteps = 10f;
        [SerializeField] private float minZoomSize = 3.5f;

        public float RotationMouseSens => rotationMouseSens;
        public float MovementMouseSens => movementMouseSens;
        public float MovementSmoothness => movementSmoothness;
        public float ZoomSmoothness => zoomSmoothness;
        public float ZoomSteps => zoomSteps;
        public float MinZoomSize => minZoomSize;
        public float RotationSmoothness => rotationSmoothness;

    }
}