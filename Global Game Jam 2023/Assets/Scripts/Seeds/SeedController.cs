using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedController : MonoBehaviour
{
    public int SeedReward { get; set; } = 1;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            SeedWallet.CollectSeeds(SeedReward);
            UI_PlayerDungeon.Instance.ChangeSeedsCount(SeedWallet.Seeds);
            Destroy(gameObject);
        }
    }
}
