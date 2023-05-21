using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using Cysharp.Threading.Tasks;

public class MazeAppearance : MonoBehaviour
{
    public bool bAppear;
    public bool bFold;
    [SerializeField] float foldDuration;

    [SerializeField] MeshController[] walls;
    [SerializeField] Vector3 wallsStartScale = new Vector3(1, 0, 1);
    [SerializeField] float wallsAppearenceDuration;
    [SerializeField] float wallsAppearenceDelay;

    [SerializeField] MeshController[] floors;
    [SerializeField] Vector3 moveFloorsOnStart = new Vector3(0, -2, 0);
    [SerializeField] float floorAppearenceDuration;
    [SerializeField] float floorAppearenceDelay;


    [SerializeField] MeshController[] spatialCues;
    [SerializeField] Vector3 moveSpatialCuesOnStart = new Vector3(0, -4, 0);
    [SerializeField] float spatialCueAppearenceDuration;
    [SerializeField] float spatialCueAppearenceDelay;

    [SerializeField] MeshController innerCue;
    [SerializeField] Vector3 moveInnerCueOnStart = new Vector3(0, -2, 0);
    [SerializeField] float innerCueAppearenceDuration;
    [SerializeField] float innerCueAppearenceDelay;

    [SerializeField] AnimationCurve appearenceCurve;
    [SerializeField] AnimationCurve foldCurve;

    [SerializeField] DemoHoldButton endButton1, endButton2;

    // Start is called before the first frame update
    void Start()
    {
        ///vive-specific
        //Calibration.BasestationCalibrator.OnCalibrateOccured += HideMaze;
    }

    public void HideMaze()
    {
        // update origin transform after scene was moved in calibration.
        foreach(MeshController mc in GetComponentsInChildren<MeshController>())   mc.updateOriginTransform();   

        // set walls y scale to 0
        foreach (MeshController wall in walls)
            wall.SetClampedScale(wallsStartScale);

        // set floor position down
        foreach (MeshController floor in floors)
            floor.Move(moveSpatialCuesOnStart);

        // set spatial cues position down
        foreach (MeshController spatialCue in spatialCues)
            spatialCue.Move(moveSpatialCuesOnStart);

        // set inner cue position down    
        innerCue.Move(moveInnerCueOnStart);
    }

    public async void SummonMaze() => await mazeAppear();
    public void FoldMaze() => _ = mazeFold();


    private async UniTask mazeAppear()
    {
        // raise floor        
        await UniTask.Delay((int)(floorAppearenceDelay * 1000));
        foreach (MeshController floor in floors)
            floor.BackToOriginalPos(floorAppearenceDuration, appearenceCurve);

        // raise walls
        await UniTask.Delay((int)(wallsAppearenceDelay * 1000));
        foreach (MeshController wall in walls)
            wall.BackToOriginalScale(wallsAppearenceDuration, appearenceCurve);

        // raise spatial cues
        await UniTask.Delay((int)(spatialCueAppearenceDelay * 1000));
        foreach (MeshController spatialCue in spatialCues)
            spatialCue.BackToOriginalPos(spatialCueAppearenceDuration, appearenceCurve);

        // raise inner cue
        await UniTask.Delay((int)(innerCueAppearenceDelay * 1000));
        innerCue.BackToOriginalPos(innerCueAppearenceDuration, appearenceCurve);
        // make buttons appear
    }

    private async UniTask mazeFold()
    {
        // set walls y scale to 0
        foreach (MeshController wall in walls)
            wall.SetClampedScale(wallsStartScale, foldDuration, foldCurve);

        // set floor position down
        foreach (MeshController floor in floors)
            floor.Move(moveFloorsOnStart, foldDuration, foldCurve);

        // set spatial cues position down
        foreach (MeshController spatialCue in spatialCues)
            spatialCue.Move(moveSpatialCuesOnStart, foldDuration, foldCurve);

        // set inner cue position down    
        innerCue.Move(moveInnerCueOnStart, foldDuration, foldCurve);

        await UniTask.Delay((int)(foldDuration * 1000));

        // trigger event fold finish -> summon back to menu button
    }
}
