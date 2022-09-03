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
    private void Update()
    {
        if (towerPosition != Vector3.zero)
        {
            // Debug.Log("move");
            transform.position = Vector3.MoveTowards(transform.position,
                towerPosition, Time.deltaTime * moveSpeed);
        }
    }

    public void MovePosition(Vector3 position)
    {
        towerPosition = position;
    }
}
