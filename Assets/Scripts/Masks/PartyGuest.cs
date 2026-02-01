#nullable enable

using Unity.Cinemachine;
using UnityEngine;

namespace Masks
{
    public class PartyGuest : MonoBehaviour
    {
        [SerializeField] private Character _character;

        [SerializeField] private CinemachineCamera _camera;
        
        private CinemachineBrain _brain;

        public Character Character => _character;

        [SerializeField] public Transform bubbleAnchor;

        private void Awake()
        {
            _brain = FindFirstObjectByType<CinemachineBrain>();
        }

        public void SetData(PlayerData data)
        {
            _character.Load(data);
        }

        public void ZoomIn()
        {
            _camera.Priority = new PrioritySettings
            {
                Value = 10,
            };
        }

        public bool CameraIsBlending() => _brain.IsBlending;

        public void ZoomOut()
        {
            _camera.Priority = new PrioritySettings
            {
                Value = 0,
            };
        }

        private Animator? _animator;
        
        public void PlayState(string state, float normalizedTiem)
        {
            _animator ??= GetComponentInChildren<Animator>();
            _animator.Play(state, 0, normalizedTiem);
        }
    }
}