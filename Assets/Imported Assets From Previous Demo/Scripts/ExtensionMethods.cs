
using UnityEngine;

public static class ExtensionMethods
{
        public static void LookAtIgnoreHeight (this Transform transform, Transform target) 
        {
            var direction =  transform.position - target.transform.position;
            var lookRotation = Quaternion.LookRotation(direction);
            lookRotation.x = 0;
            lookRotation.z = 0;
            transform.rotation = lookRotation;
        }
}
