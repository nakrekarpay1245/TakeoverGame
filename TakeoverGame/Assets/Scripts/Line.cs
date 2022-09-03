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
    }

    public Tower GetSenderTower()
    {
        return senderTower;
    }

    public Tower GetReceiverTower()
    {
        return receiverTower;
    }
}
