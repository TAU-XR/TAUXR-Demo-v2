using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnTriggerDo : MonoBehaviour
{
    [SerializeField] UnityEvent onTriggerDo;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Toucher")
            onTriggerDo.Invoke();
    }


}
