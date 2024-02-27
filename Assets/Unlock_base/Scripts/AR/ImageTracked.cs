using UnityEngine;

public class ImageTracked
{
    public GameObject _prefab;
    public KalmanFilterVector3 _positionFilter = new KalmanFilterVector3();
    public KalmanFilterVector3 _upFilter = new KalmanFilterVector3();
    public KalmanFilterVector3 _forwardFilter = new KalmanFilterVector3();


    public ImageTracked(GameObject prefab)
    {
        _prefab = prefab;
    }
}

