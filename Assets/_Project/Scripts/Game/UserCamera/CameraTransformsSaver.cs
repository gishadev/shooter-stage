using DG.Tweening;
using UnityEngine;

namespace gishadev.Shooter.Game.UserCamera
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
            var sceneTransforms = _cameraRig.GetComponentsInChildren<Transform>();
            _savedTransforms = new SavedTransforms
            {
                previousSavedPositions = new Vector3[sceneTransforms.Length],
                previousSavedRotations = new Quaternion[sceneTransforms.Length]
            };

            for (var i = 0; i < sceneTransforms.Length; i++)
            {
                SavedTransforms.previousSavedPositions[i] = sceneTransforms[i].position;
                SavedTransforms.previousSavedRotations[i] = sceneTransforms[i].rotation;
            }
        }

        public Sequence LoadTransforms(SavedTransforms allTransforms)
        {
            if (allTransforms.previousSavedPositions == null || allTransforms.previousSavedRotations == null)
                return DOTween.Sequence();
            
            var seq = DOTween.Sequence();
            var sceneTransforms = _cameraRig.GetComponentsInChildren<Transform>();
            for (var i = 0; i < sceneTransforms.Length; i++)
            {
                seq.Join(sceneTransforms[i].DOMove(allTransforms.previousSavedPositions[i], 0.2f));
                seq.Join(sceneTransforms[i].DORotateQuaternion(allTransforms.previousSavedRotations[i], 0.2f));
            }

            return seq;
        }
    }

    public struct SavedTransforms
    {
        public Vector3[] previousSavedPositions;
        public Quaternion[] previousSavedRotations;
    }
}