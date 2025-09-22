using UnityEngine;
using DG.Tweening;

namespace InteractionSystem
{
    /// <summary>
    /// Represents a Chest interactable object.
    /// Implements IInteractable and IHighlightable for interaction and visual highlighting functionality.
    /// </summary>
    public class Chest : MonoBehaviour
    {
        [SerializeField] private Transform _crateTop;
        [SerializeField] private Vector3 _openAngle;
        [SerializeField] private float _openTweenDuration = 0.5f;

        private const int DEFAULTLAYERINDEX = 0;
        private Interactable _interactable;

        private void Awake()
        {
            _interactable = GetComponent<Interactable>();
        }

        private void OnEnable() {
            _interactable.OnInteract += Open;
        }

        private void OnDisable() {
            _interactable.OnInteract -= Open;
        }

        public void Open()
        {
            _crateTop.DOLocalRotate(_openAngle, _openTweenDuration);
            gameObject.layer = DEFAULTLAYERINDEX;
        }
    }
}