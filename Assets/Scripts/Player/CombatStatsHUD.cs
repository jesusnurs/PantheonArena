using UnityEngine;
using UnityEngine.Serialization;

public class CombatStatsHUD : MonoBehaviour
{
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        transform.GetComponent<Canvas>().worldCamera = mainCamera;
    }

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position) ;
    }
}
