using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private int playerDamage = 25;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.activeSelf != false && collision.name == "Pawn")
        {
            Debug.Log("Hit");
            collision.gameObject.transform.GetComponent<EnemyStats>().DealDamage(playerDamage);
            Debug.Log(collision.gameObject.GetComponent<EnemyStats>().GetHealth());
        } 
    }

    /// <summary>
    /// Adds more damage to the player.
    /// </summary>
    /// <param name="upgradeAmount">Aditional damage.</param>
    public void UpgradeAttack(int upgradeAmount)
    {
        playerDamage += upgradeAmount;
    }
}
