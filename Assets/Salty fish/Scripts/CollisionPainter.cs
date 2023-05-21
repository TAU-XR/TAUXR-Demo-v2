using UnityEngine;

public class CollisionPainter : MonoBehaviour{
    public Color paintColor;
    
    public float radius = 1;
    public float strength = 1;
    public float hardness = 1;

    private void OnCollisionStay(Collision other) {
        Debug.Log("s");
        Paintable p = other.collider.GetComponent<Paintable>();
        if(p != null){
            Vector3 pos = other.contacts[0].point;
            
            Debug.Log(p);
            PaintManager.instance.paint(p, pos, radius, hardness, strength, paintColor);
        }
    }
}
