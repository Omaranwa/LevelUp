using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fish
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float _smoothTime;
        private Vector3 _cameraVelocity;
        private Lure _currentLure;
        private EventService _eventService;
        private Vector3 _targetPosition;
        private Vector3 _defaultPosition;
        
        void Start()
        {
            _defaultPosition = transform.position;
            
            _eventService = Services.Instance.EventService;
            _eventService.Add<SlingReadyWithProjectile>(ResetCamera);
            _eventService.Add<LureThrownEvent>(TargetLavaling);
        }

        void Update()
        {
            if (_currentLure != null) {
                _targetPosition = _currentLure.transform.position;
                _targetPosition.z = _defaultPosition.z;
            }

            var dampPosition = Vector3.SmoothDamp(transform.position, _targetPosition, ref _cameraVelocity, _smoothTime);
            transform.position = dampPosition;
        }
        
        void TargetLavaling(LureThrownEvent eventType) {
            _currentLure = eventType.Lure;
        }
        
        void ResetCamera() {
            _currentLure = null;
            _targetPosition = _defaultPosition;
        }
    }
}

