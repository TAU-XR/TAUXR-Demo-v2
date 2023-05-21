using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesController : MonoBehaviour
{
    public bool bRockNRoll;

    [SerializeField] float popFreq;
    [SerializeField] float popFreqRange;

    TileBehavior[] tiles;

    // Start is called before the first frame update
    void Start()
    {
        tiles = GetComponentsInChildren<TileBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        if (bRockNRoll)
        {
            bRockNRoll = false;
            PopEmUp();
        }
    }

    public void PopEmUp()
    {
        float popDelay = 0;
        foreach (TileBehavior tile in tiles)
        {
            tile.Pop(popDelay);
            popDelay += (popFreq + Random.Range(-popFreqRange, popFreqRange));
        }
    }
}
