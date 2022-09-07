using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    //[SerializeField]
    //private Tower senderTower;
    //[SerializeField]
    //private Tower receiverTower;

    //private LineRenderer lineRenderer;

    //#region AttackCollider
    //[SerializeField]
    //private AttackCollider attackColliderPrefab;
    //[SerializeField]
    //private AttackCollider currentAttackCollider;
    //#endregion

    //private void Awake()
    //{
    //    lineRenderer = GetComponent<LineRenderer>();
    //}

    //public void SetSenderTower(Tower sender)
    //{
    //    senderTower = sender;
    //    Vector3 position0 = new Vector3(senderTower.transform.position.x, 0.5f, senderTower.transform.position.z);
    //    lineRenderer.SetPosition(0, position0);

    //    if (senderTower.isAllied)
    //    {
    //        lineRenderer.startColor = Color.green;
    //        lineRenderer.endColor = Color.green;
    //    }
    //    else if (senderTower.isOpposite)
    //    {
    //        lineRenderer.startColor = Color.red;
    //        lineRenderer.endColor = Color.red;
    //    }
    //}

    //public void SetMovePosition(Vector3 position)
    //{
    //    lineRenderer.SetPosition(1, position);
    //}

    //public void SetReceiverTower(Tower receiver)
    //{
    //    receiverTower = receiver;
    //    Vector3 position1 = new Vector3(receiverTower.transform.position.x, 0.5f, receiverTower.transform.position.z);
    //    lineRenderer.SetPosition(1, position1);
    //    CreateAttackCollider();
    //}

    //public Tower GetSenderTower()
    //{
    //    return senderTower;
    //}

    //public Tower GetReceiverTower()
    //{
    //    return receiverTower;
    //}

    //// Attack Collider
    //public void CreateAttackCollider()
    //{
    //    Vector3 position0 = lineRenderer.GetPosition(0);
    //    Vector3 position1 = lineRenderer.GetPosition(1);

    //    if (senderTower.isAllied)
    //    {
    //        currentAttackCollider = Instantiate(attackColliderPrefab, transform);

    //        currentAttackCollider.SetTransform(position0, position1, this);
    //    }
    //}

    //public void CutAttack()
    //{
    //    senderTower.RemoveReceiverTower(receiverTower);

    //    receiverTower.RemoveSenderTower(senderTower);

    //    senderTower.RemoveReceiverLine(this);

    //    receiverTower.RemoveSenderLine(this);
    //}
}
