//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class EyePaintTest : MonoBehaviour
//{
//    public Color paintColor;

//    public float radius = 1;
//    public float strength = 1;
//    public float hardness = 1;
//    float magnify = 0;
//    int layerToInteractWith;


//    // Start is called before the first frame update
//    void Start()
//    {
//        layerToInteractWith = LayerMask.NameToLayer("Paintable");
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        RaycastHit hit;
//        if (Physics.Raycast(transform.position, transform.forward, out hit))
//        {
//            Debug.DrawRay(transform.position, hit.point - transform.position, Color.red);
//            Paintable p = hit.collider.GetComponent<Paintable>();
//            if (p != null)
//            {
//                //float magnify = Vector3.Distance(TAUXRPlayer.Instance.PlayerHead.position, TAUXRPlayer.Instance.RightHand.position);
//                PaintManager.instance.paint(p, hit.point, radius*(1+magnify), hardness, strength, paintColor);
//            }
//        }
//    }
//}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyePaintTest : MonoBehaviour
{
    public Color paintColor;

    public float radius = 1;
    public float strength = 1;
    public float hardness = 1;
    float magnify = 0;
    int layerToInteractWith;

    // Start is called before the first frame update
    void Start()
    {
        layerToInteractWith = LayerMask.NameToLayer("Paintable");
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            Debug.DrawRay(transform.position, hit.point - transform.position, Color.red);
            Paintable p = hit.collider.GetComponent<Paintable>();
            if (p != null)
            {
                // Hit a paintable object, paint on it
                PaintManager.instance.paint(p, hit.point, radius * (1 + magnify), hardness, strength, paintColor);
            }
            else
            {
                // Hit a non-paintable object, cast another ray from that object in the same direction
                RaycastHit nestedHit;
                if (Physics.Raycast(hit.point, transform.forward, out nestedHit))
                {
                    Debug.DrawRay(hit.point, nestedHit.point - hit.point, Color.blue);
                    Paintable nestedP = nestedHit.collider.GetComponent<Paintable>();
                    if (nestedP != null)
                    {
                        // Hit a paintable object with the nested raycast, paint on it
                        PaintManager.instance.paint(nestedP, nestedHit.point, radius * (1 + magnify), hardness, strength, paintColor);
                    }
                }
            }
        }
    }
}
