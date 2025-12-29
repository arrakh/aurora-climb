using UnityEngine;
using FMODUnity;

public class ForceFMODGlobalParam : MonoBehaviour
{
    public StudioGlobalParameterTrigger paramTrigger;

    private void OnTriggerEnter(Collider other)
    {
        paramTrigger.TriggerParameters();
    }
}