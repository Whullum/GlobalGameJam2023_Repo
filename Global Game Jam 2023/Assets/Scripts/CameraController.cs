using Cinemachine;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour
{
    private CinemachineVirtualCamera cmCamera;
    private CinemachineConfiner2D cinemachineConfiner;
    private GameObject levelBoundary;

    private void Awake()
    {
        cmCamera = GetComponent<CinemachineVirtualCamera>();
        cinemachineConfiner = GetComponent<CinemachineConfiner2D>();
    }

    private void OnEnable() => LevelGenerator.LevelColliderCreated += SetCameraConfiner;
    private void OnDisable() => LevelGenerator.LevelColliderCreated -= SetCameraConfiner;

    /// <summary>
    /// Creates a new confiner with the tilemaps bounds and set the confiner inside cinemachine.
    /// </summary>
    /// <param name="tilemap"></param>
    private void SetCameraConfiner(Tilemap tilemap)
    {
        if(levelBoundary == null)
        {
            levelBoundary = new GameObject("LevelBoundary");
            levelBoundary.layer = LayerMask.NameToLayer("LevelBoundary");
            levelBoundary.AddComponent<PolygonCollider2D>();
        }
        PolygonCollider2D boundary;

        tilemap.CompressBounds();
        boundary = levelBoundary.GetComponent<PolygonCollider2D>();

        Vector2[] path = new Vector2[4];

        path[0] = new Vector2(tilemap.cellBounds.xMin, tilemap.cellBounds.yMax);
        path[1] = new Vector2(tilemap.cellBounds.xMin, tilemap.cellBounds.yMin);
        path[2] = new Vector2(tilemap.cellBounds.xMax, tilemap.cellBounds.yMin);
        path[3] = new Vector2(tilemap.cellBounds.xMax, tilemap.cellBounds.yMax);
        boundary.pathCount = 1;
        boundary.SetPath(0, path);
        boundary.isTrigger = true;

        cinemachineConfiner.m_BoundingShape2D = boundary;
    }
}
