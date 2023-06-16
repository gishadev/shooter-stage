using UnityEngine;

namespace gishadev.Shooter.Game.Stage
{
    [RequireComponent(typeof(LineRenderer))]
    public class ArrowAnimation : MonoBehaviour
    {
        [SerializeField] private float animationSpeed = 1f;

        private LineRenderer _lr;
        private float _offsetValue;

        private void Awake()
        {
            _lr = GetComponent<LineRenderer>();
        }

        private void Update()
        {
            ChangeOffset();
        }

        private void ChangeOffset()
        {
            _offsetValue += Time.deltaTime * animationSpeed;
            var offset = new Vector2(_offsetValue, 0f);
            _lr.material.mainTextureOffset = offset;
        }
    }
}