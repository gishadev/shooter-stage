using UnityEngine;

namespace gishadev.Shooter.Core
{
    public class CameraTransformsSaver
    {
        private readonly Transform _cameraRig;
        private SavedTransforms _savedTransforms;

        public CameraTransformsSaver(Transform cameraRig)
        {
            _cameraRig = cameraRig;
        }

        public SavedTransforms SavedTransforms => _savedTransforms;

        public void SaveTransforms()
        {
            var transforms = _cameraRig.GetComponentsInChildren<Transform>();
            _savedTransforms = new SavedTransforms
            {
                previousSavedPositions = new Vector3[transforms.Length],
                previousSavedRotations = new Quaternion[transforms.Length]
            };

            for (var i = 0; i < transforms.Length; i++)
            {
                SavedTransforms.previousSavedPositions[i] = transforms[i].position;
                SavedTransforms.previousSavedRotations[i] = transforms[i].rotation;
            }
        }

        public void LoadTransforms(SavedTransforms allTransforms)
        {
            if (allTransforms.previousSavedPositions == null || allTransforms.previousSavedRotations == null)
                return;
            var sceneTransforms = _cameraRig.GetComponentsInChildren<Transform>();

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