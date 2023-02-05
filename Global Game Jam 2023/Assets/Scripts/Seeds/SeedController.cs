using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedController : MonoBehaviour
{
    public int SeedReward { get; set; } = 1;

    [SerializeField] private float seedDestroyTime = 10f;

    private void OnEnable()
    {
        LevelGenerator.LevelCreated += DestroySeed;
    }

    private void OnDisable()
    {
        LevelGenerator.LevelCreated -= DestroySeed;
    }

    private void Start()
    {
        Destroy(gameObject, seedDestroyTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            SeedWallet.CollectSeeds(SeedReward);
            UI_PlayerDungeon.Instance.ChangeSeedsCount(SeedWallet.Seeds);
            Destroy(gameObject);
        }
    }

    private void DestroySeed(int[,] test, TileCoord test2) => Destroy(gameObject);
}
