using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    [SerializeField]
    private Line line;
    public void SetTransform(Vector3 position0, Vector3 position1, Line _line)
    {
        line = _line;
        transform.position = Vector3.zero;

        transform.localScale = new Vector3(0.25f, 0.25f,
            Mathf.Sqrt(Mathf.Pow(Mathf.Abs(position0.z - position1.z), 2) +
            Mathf.Pow(Mathf.Abs(position0.x - position1.x), 2)));

        transform.position =
            new Vector3((position0.x + position1.x) / 2, 0.5f,
            (position0.z + position1.z) / 2);

        transform.LookAt(position0 - position1);
    }

    public void CutAttack()
    {
        line.CutAttack();
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("AttackCutter"))
        {
            CutAttack();
        }
    }
}
