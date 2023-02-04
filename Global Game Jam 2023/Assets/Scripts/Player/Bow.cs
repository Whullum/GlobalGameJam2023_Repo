using UnityEngine;

public class Bow : MonoBehaviour
{
    private float shootTimer;

    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float shootForce = 10f;
    [SerializeField] private float shootRate = 2f;

    private void Update()
    {
        Aim();

        if (Input.GetMouseButtonDown(0))
            ShootArrow();

        shootTimer -= Time.deltaTime * shootRate;
    }

    private void Aim()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 rotation = mousePos - transform.position;
        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotZ);
    }

    private void ShootArrow()
    {
        if (shootTimer < 0)
        {
            Rigidbody2D arrowRb = Instantiate(arrowPrefab, shootPoint.position, transform.rotation).GetComponent<Rigidbody2D>();
            arrowRb.AddForce(transform.forward * shootForce, ForceMode2D.Impulse);

            shootTimer = 1;
        }
    }
}
    