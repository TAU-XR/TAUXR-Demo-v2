using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassthroughManager : SingletonMonoBehaviour<PassthroughManager>
{
    [SerializeField] private OVRPassthroughLayer passthroughLayer;
    /// <summary>
    /// Put here the room model, together with any other objects you wish to toggle off when activating the passthrough
    /// </summary>
    [SerializeField]
    private List<GameObject> ObjectsToToggle;
    // Start is called before the first frame update

    public void TogglePassthrough(bool turnOn)
    {
        passthroughLayer.hidden = !turnOn;
        foreach (GameObject obj in ObjectsToToggle)
        {
            obj.SetActive(!turnOn);
        }
    }
}
