using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    [SerializeField]
    private Tower senderTower;
    [SerializeField]
    private Tower receiverTower;

    private LineRenderer lineRenderer;

    #region AttackCollider
    [SerializeField]
    private AttackCollider attackColliderPrefab;
    [SerializeField]
    private AttackCollider currentAttackCollider;
    #endregion

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void SetSenderTower(Tower sender)
    {
        senderTower = sender;
        lineRenderer.SetPosition(0, senderTower.transform.position);
    }

    public void SetMovePosition(Vector3 position)
    {
        lineRenderer.SetPosition(1, position);
    }
    public void SetReceiverTower(Tower receiver)
    {
        receiverTower = receiver;
        lineRenderer.SetPosition(1, receiverTower.transform.position);
        CreateAttackCollider();
    }

    public Tower GetSenderTower()
    {
        return senderTower;
    }

    public Tower GetReceiverTower()
    {
        return receiverTower;
    }

    // Attack Collider
    public void CreateAttackCollider()
    {
        Vector3 position0 = lineRenderer.GetPosition(0);
        Vector3 position1 = lineRenderer.GetPosition(1);

        currentAttackCollider = Instantiate(attackColliderPrefab,
           position0, Quaternion.identity);

        currentAttackCollider.SetTransform(position0, position1, this);
    }

    public void CutAttack()
    {
        senderTower.RemoveReceiverTower(receiverTower);

        receiverTower.RemoveSenderTower(senderTower);

        senderTower.RemoveReceiverLine(this);

        receiverTower.RemoveSenderLine(this);
    }
}
