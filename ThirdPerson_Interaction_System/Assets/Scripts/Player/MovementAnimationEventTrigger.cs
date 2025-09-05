using UnityEngine;

public class MovementAnimationEventTrigger : MonoBehaviour
{
    [SerializeField] private AudioClip _landingAudioClip;
    [SerializeField] private AudioClip[] _footstepAudioClips;
    [Range(0, 1)][SerializeField] private float _footstepAudioVolume = 0.5f;

    private CharacterController _controller;

    private void Awake()
    {
        _controller = GetComponentInParent<CharacterController>();
    }

    private void OnFootstep(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            if (_footstepAudioClips.Length > 0)
            {
                var index = Random.Range(0, _footstepAudioClips.Length);
                AudioSource.PlayClipAtPoint(_footstepAudioClips[index], transform.TransformPoint(_controller.center), _footstepAudioVolume);
            }
        }
    }

    private void OnLand(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            AudioSource.PlayClipAtPoint(_landingAudioClip, transform.TransformPoint(_controller.center), _footstepAudioVolume);
        }
    }
}