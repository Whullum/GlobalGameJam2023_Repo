using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance { get; private set; }

    [Header("Dungeon Music Events")]
    public AK.Wwise.Event playDungeonMusic;
    public AK.Wwise.Event stopDungeonMusic;

    [Header("Dungeon Music States")]
    public AK.Wwise.State normalState;
    public AK.Wwise.State dangerState;
    public AK.Wwise.State deathState;
    public AK.Wwise.State victoryState;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;
    }

    private void Start()
    {
        normalState.SetValue();
        playDungeonMusic.Post(gameObject);
    }

    private void OnDestroy()
    {
        stopDungeonMusic.Post(gameObject);
    }
}
