using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // Start is called before the first frame update
    Vector2 m_Position;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_Position.x = Random.Range(-1, 1);
        m_Position.y = Random.Range(-1, 1);
        Vector3 nextPos = m_Position;
        Debug.DrawLine(gameObject.transform.position, transform.TransformDirection(gameObject.transform.position + nextPos), Color.red, 6f);
        Debug.Log(Physics2D.Raycast(gameObject.transform.position, transform.TransformDirection(gameObject.transform.position + nextPos), 5));
    }
}
