using Cinemachine;
using Player;
using UnityEngine;

namespace Cameraa
{
    public class CameraController : MonoBehaviour
    {
        private CinemachineVirtualCamera _cinemachineCamera;

        private void Awake()
        {
            _cinemachineCamera = GetComponent<CinemachineVirtualCamera>();

            if (_cinemachineCamera == null)
            {
                return;
            }

            var player = GameObject.FindGameObjectWithTag("Player");

            if (player == null)
            {
                return;
            }

            _cinemachineCamera.Follow = player.transform;

            var lookPoint = player.GetComponentInChildren<PlayerLookPoint>();

            if (lookPoint == null)
            {
                return;
            }

            _cinemachineCamera.LookAt = lookPoint.transform;
        }
    }
}