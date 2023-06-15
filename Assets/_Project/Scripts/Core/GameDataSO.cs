using UnityEngine;

namespace gishadev.Shooter.Core
{
    [CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData", order = 0)]
    public class GameDataSO : ScriptableObject
    {
        [Header("Orbital Camera Settings")]
        [Header("Movement/Rotation")]
        [SerializeField] private float orbitalRotationMouseSens = 0.25f;

        [SerializeField] private float orbitalRotationSmoothness = 250f;
        [SerializeField] private float orbitalMovementMouseSens = 1f;
        [SerializeField] private float orbitalMovementSmoothness = 125f;

        [Header("Zoom")] 
        [SerializeField] private float zoomSmoothness = 255f;
        [SerializeField] private float zoomSteps = 10f;
        [SerializeField] private float minZoomSize = 3.5f;
        [Header("Free Camera Settings")]
        [SerializeField] private float freeCamRotationMouseSens = 0.25f;
        [SerializeField] private float freeCamMoveSpeed = 25f;
        
        public float OrbitalRotationMouseSens => orbitalRotationMouseSens;
        public float OrbitalMovementMouseSens => orbitalMovementMouseSens;
        public float OrbitalMovementSmoothness => orbitalMovementSmoothness;
        public float ZoomSmoothness => zoomSmoothness;
        public float ZoomSteps => zoomSteps;
        public float MinZoomSize => minZoomSize;
        public float OrbitalRotationSmoothness => orbitalRotationSmoothness;
        public float FreeCamRotationMouseSens => freeCamRotationMouseSens;

        public float FreeCamMoveSpeed => freeCamMoveSpeed;
    }
}