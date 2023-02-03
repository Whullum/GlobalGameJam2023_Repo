using UnityEngine;

public class SeedDropper : MonoBehaviour
{
    [SerializeField] private GameObject seedPrefab;
    [SerializeField] private int minDropReward = 1;
    [SerializeField] private int maxDropReward = 5;
    [SerializeField] private int minSeedDrop = 5;
    [SerializeField] private int maxSeedDrop = 20;
    [SerializeField] private float seedDropForce = 5f;

    private void Start()
    {
        Invoke("DropSeeds", 2f);
    }

    public void DropSeeds()
    {
        int seedsToDrop  = Random.Range(minSeedDrop, maxSeedDrop + 1);
        


        for (int i = 0; i < seedsToDrop; i++)
        {
            int seedReward = Random.Range(minDropReward, maxDropReward + 1);
            Quaternion rndRotation = Random.rotation;
            rndRotation.x = 0;
            rndRotation.y = 0;

            Rigidbody2D seedRB = Instantiate(seedPrefab, transform.position, rndRotation).GetComponent<Rigidbody2D>();
            SeedController seed = seedRB.gameObject.GetComponent<SeedController>();
            seed.SeedReward = seedReward;
            seedRB.AddForce(seedRB.transform.forward * seedDropForce, ForceMode2D.Impulse);
        }
    }
}
