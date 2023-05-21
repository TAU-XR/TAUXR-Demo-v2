using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectSaltyFish : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Animateonlook anim))
        {
            if (anim == null) return;
            anim.IsLookedAt = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Animateonlook anim))
        {
            if (anim == null) return;
            anim.IsLookedAt = false;
        }
    }
}
