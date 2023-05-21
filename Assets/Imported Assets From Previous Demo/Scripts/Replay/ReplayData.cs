using UnityEngine;

public class ReplayData
{
    public Vector3 Position;
    public Quaternion Rotation;

    public ReplayData(Vector3 position, Quaternion rotation)
    {
        this.Position = position;
        this.Rotation = rotation;
    }
}
