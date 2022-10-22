using Actor.Racer;
using Manager;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class RacerPlane : MonoBehaviour
{
    private GUID _parentRacerId;

    void Awake()
    {
        _parentRacerId = GetComponentInParent<RacerController>().RacerId;
    }

    public UnityEvent RacerExitsPlaneEvent { get; set; }
    public void OnTriggerExit(Collider collider)
    {
        // var racer = collider?.GetComponent<RacerController>();
        // if (racer != null && racer.RacerId != _parentRacerId)
        if (collider?.GetComponent<RacerPlane>() != null)
        {
            // RacerExitsPlaneEvent.Invoke();
            GameManager.Instance.RaceManager.CalculatePositions();
        }
    }
}
