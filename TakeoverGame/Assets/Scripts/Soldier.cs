using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    public bool isAllied;
    public bool isOpposite;

    public float moveSpeed = 5;

    public Vector3 towerPosition = Vector3.zero;

    public Tower senderTower;

    private Collider colliderComponent;
    private void Awake()
    {
        colliderComponent = GetComponent<Collider>();
    }
    private void Update()
    {
        if (towerPosition != Vector3.zero)
        {
            // Debug.Log("move");
            transform.position = Vector3.MoveTowards(transform.position,
                towerPosition, Time.deltaTime * moveSpeed);

            transform.LookAt(towerPosition);
        }
    }

    public void MovePosition(Vector3 position)
    {
        towerPosition = position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Soldier"))
        {
            if (other.GetComponent<Soldier>().isAllied && isOpposite)
            {
                Dead();
            }
            else if (other.GetComponent<Soldier>().isOpposite && isAllied)
            {
                Dead();
            }
        }
    }

    private void Dead()
    {
        moveSpeed = 0;
        colliderComponent.enabled = false;
        Destroy(gameObject, 1);
    }
}
