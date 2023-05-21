using System.Collections.Generic;
using UnityEngine;

public class ReplayTrackedObject : MonoBehaviour
{
    [SerializeField] private GameObject clonePrefab; // what prefab should be instantiated upon playing replay
    private List<ReplayData> _replayDataList = new List<ReplayData>();

    private bool _isInPlayMode = false; // true if is in play mode.

    #region Clones Variables

    private ReplayClone _mainClone;
    private List<ReplayClone> _subClones = new List<ReplayClone>();

    [SerializeField] private int subCloneSummonRate = 3; // how many seconds between summon.
    [SerializeField] private float subCloneAlpha = .3f;
    [SerializeField] private float subCloneAppearDuration = 3f;
    [SerializeField] private float trailSpeed = 3f;
    [SerializeField] private int trailRate = 100;
    [SerializeField] private float trailAlpha = .02f;
    private int lastSubCloneIndex;
    [SerializeField] private float _cloneListDestroyTime = 3f;
    [SerializeField] private Material _mainCloneMaterial;
    [SerializeField] private Material _subCloneMaterial;

/* Movement Clones Variables
    private List<ReplayClone> _movementClones = new List<ReplayClone>();
    private bool _bDisplayMovementClones;
    private int _movementClonesCount = 5;
    private int movementClonesFrameSeparation = 10; // how many frames between each subClone
    private float movementCloneAlpha = .18f;*/

    #endregion

    private int _currentReplayIndex = 0;
    private float _nextReplayIndex = 0;
    [Range(0, 2)] [SerializeField] private float replaySpeedMultiplier = 1f; //currently not in use

    private float _replayTime = 0;
    private float _fixedCloneTimer;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown((KeyCode.R)))
        {
            _isInPlayMode = !_isInPlayMode;
            ResetReplayParams(_isInPlayMode);
        }

        if (_isInPlayMode)
        {
            // add sub clones
            _replayTime += Time.deltaTime;
            if (_replayTime >= _fixedCloneTimer)
            {
                _subClones.Add(SummonClone(subCloneAlpha, subCloneAppearDuration, 0, _mainCloneMaterial));
                ApplyReplayData(_subClones[_subClones.Count - 1].transform, _replayDataList[(int) _nextReplayIndex]);

                if (_subClones.Count > 1)
                    SummonSubCloneTrail(lastSubCloneIndex, (int) _nextReplayIndex, trailSpeed, trailRate);

                lastSubCloneIndex = (int) _nextReplayIndex;

                _fixedCloneTimer += subCloneSummonRate;
            }
        }
    }

    private void FixedUpdate()
    {
        RecordReplayData();

        if (_isInPlayMode)
        {
            // _nextReplayIndex = (_currentReplayIndex + 1) * replaySpeedMultiplier;
            _nextReplayIndex = (_currentReplayIndex + 1);

            // play main clone replay
            ApplyReplayData(_mainClone.transform, _replayDataList[(int) _nextReplayIndex]);

            _currentReplayIndex++;
        }
    }

    void SummonSubClone(int replayDataIndex, float summonDelay, float cloneAlpha)
    {
        _subClones.Add(SummonClone(cloneAlpha, subCloneAppearDuration, summonDelay, _subCloneMaterial));
        ApplyReplayData(_subClones[_subClones.Count - 1].transform, _replayDataList[replayDataIndex]);
    }

    void SummonSubCloneTrail(int startIndex, int endIndex, float summonSpeed, int summonRate)
    {
        int trailLength = endIndex - startIndex + 1;

        for (int i = 0; i < trailLength; i++)
        {
            if (i % summonRate != 0) continue;
            float appearanceDelay = (float)i / trailLength * summonSpeed;
            Debug.Log("Wallak: "+appearanceDelay);
            SummonSubClone(i + startIndex, appearanceDelay, trailAlpha);
        }
    }

    private void ResetReplayParams(bool didEnterPlayMode)
    {
        if (didEnterPlayMode)
        {
            // summon main clone
            _mainClone = SummonClone(.9f, 0, _mainCloneMaterial);
            
            // summon sub clone
            _subClones.Add(SummonClone(subCloneAlpha, subCloneAppearDuration, 0, _mainCloneMaterial));
            ApplyReplayData(_subClones[_subClones.Count - 1].transform, _replayDataList[(int) _nextReplayIndex]);

            _fixedCloneTimer = subCloneSummonRate;
        }
        else // exit playMode
        {
            // destroy main clone
            _mainClone.Destroy(_cloneListDestroyTime);
            _mainClone = null;

            // destroy sub clones
            for (int i = 0; i < _subClones.Count; i++)
            {
                float subCloneDestroyTime = _cloneListDestroyTime * Mathf.Pow(.8f, i + 1);
                _subClones[i].Destroy(subCloneDestroyTime);
            }


            // reset list 
            _currentReplayIndex = 0;
            _replayDataList.Clear();

            _replayTime = 0;
        }
    }

    ReplayClone SummonClone(float cloneAlpha, float cloneAppearTime, Material cloneMaterial)
    {
        ReplayClone clone;

        clone = Instantiate(clonePrefab).GetComponent<ReplayClone>();
        clone.Init(cloneAlpha, cloneAppearTime, cloneMaterial);

        return clone;
    }

    ReplayClone SummonClone(float cloneAlpha, float cloneAppearTime, float appearenceDelay, Material cloneMaterial)
    {
        ReplayClone clone;

        clone = Instantiate(clonePrefab).GetComponent<ReplayClone>();
        clone.Init(cloneAlpha, cloneAppearTime, appearenceDelay, cloneMaterial);

        return clone;
    }

    private void RecordReplayData()
    {
        _replayDataList.Add(new ReplayData(transform.position, transform.rotation));
    }

    private void ApplyReplayData(Transform obj, ReplayData replayData)
    {
        obj.position = replayData.Position;
        obj.rotation = replayData.Rotation;
    }

    public void PlayReplay(int fromIndex)
    {
    }

    /* public void DisplayMovementClones(int nextReplayIndex)
    {
        // this display form is right for movement clones, but should be implemented in another script.
        for (int i = 0; i < _movementClones.Count; i++)
        {
            int subCloneindex = (_replayDataList.Count - 1) - (i + 1) * movementClonesFrameSeparation;
            if (subCloneindex < 0)
                subCloneindex = 0;

            ApplyReplayData(_movementClones[i].transform, _replayDataList[subCloneindex]);
            _movementClones[i].SetAlpha(movementCloneAlpha * Mathf.Pow(.7f, i + 1), 0);
        }
        
         //This displayment form was used to display movement clones in replay behind the main clone. It was overwhelming so I think fixed clones are way better.
        for (int i = 0; i < _movementClones.Count; i++)
        {
             int subCloneindex = nextReplayIndex - (i + 1) * movementClonesFrameSeparation;
             if (subCloneindex < 0)
                 subCloneindex = 0;
 
             ApplyReplayData(_movementClones[i].transform, _replayDataList[subCloneindex]);
             _movementClones[i].SetAlpha(subCloneAlpha * Mathf.Pow(.7f, i + 1), 0);
        }
    }*/
}