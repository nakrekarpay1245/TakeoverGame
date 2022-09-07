using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    [SerializeField]
    private Tower senderTower;
    [SerializeField]
    private Tower receiverTower;

    [SerializeField]
    private ParticleSystem senderParticle;

    private Vector3 position0;
    private Vector3 position1;
    public void SetSenderTower(Tower sender)
    {
        senderTower = sender;
        position0 = new Vector3(senderTower.transform.position.x,
            0.5f, senderTower.transform.position.z);


        if (senderTower.isAllied)
        {
            // Change image color
            senderParticle.startColor = Color.green;
        }
        else if (senderTower.isOpposite)
        {
            // Change image color
            senderParticle.startColor = Color.red;
        }
    }
    public void SetReceiverTower(Tower receiver)
    {
        receiverTower = receiver;
        position1 = new Vector3(receiverTower.transform.position.x,
            0.5f, receiverTower.transform.position.z);

        if (senderTower)
        {
            SetTransform(position0, position1);
        }
    }

    public void SetTransform(Vector3 position0, Vector3 position1)
    {
        transform.localScale = new Vector3(0.5f, 1,
            Mathf.Sqrt(Mathf.Pow(Mathf.Abs(position0.z - position1.z), 2) +
            Mathf.Pow(Mathf.Abs(position0.x - position1.x), 2)));

        transform.position = new Vector3((position0.x + position1.x) / 2, 0.5f,
            (position0.z + position1.z) / 2);

        Quaternion rotationTarget = Quaternion.LookRotation(position0 - position1);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationTarget, 1000);
    }

    public Tower GetSenderTower()
    {
        return senderTower;
    }

    public Tower GetReceiverTower()
    {
        return receiverTower;
    }


    public void CutAttack()
    {
        Destroy(gameObject);

        senderTower.RemoveReceiverTower(receiverTower);

        receiverTower.RemoveSenderTower(senderTower);

        senderTower.RemoveReceiverAttackCollider(this);

        receiverTower.RemoveSenderAttackCollider(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("AttackCutter"))
        {
            if (senderTower.isAllied)
            {
                CutAttack();
            }
        }
    }
}
