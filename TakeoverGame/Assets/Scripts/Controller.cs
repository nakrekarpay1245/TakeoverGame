using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;

    [SerializeField]
    private Line linePrefab;
    [SerializeField]
    private Line currentLine;

    public Tower senderTower;
    public Tower receiverTower;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Physics.Raycast(ray, out RaycastHit raycastHit))
            {
                if (raycastHit.collider.CompareTag("Tower"))
                {
                    if (raycastHit.collider.gameObject.GetComponent<Tower>().isAllied)
                    {
                        senderTower = raycastHit.collider.gameObject.GetComponent<Tower>();
                        currentLine = Instantiate(linePrefab, senderTower.transform);
                        currentLine.transform.position = Vector3.zero;
                        currentLine.SetSenderTower(senderTower);
                        senderTower.AddReceiverLine(currentLine);
                    }
                }
                else
                {
                    Debug.Log("Kuleye týklamadýk");
                }
            }
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 100))
            {
                if (currentLine)
                {
                    currentLine.SetMovePosition(raycastHit.point);
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 100))
            {
                if (senderTower)
                {
                    if (raycastHit.collider.CompareTag("Tower"))
                    {
                        receiverTower = raycastHit.collider.gameObject.GetComponent<Tower>();
                        currentLine.SetReceiverTower(receiverTower);
                        senderTower.AddReceiverTower(receiverTower);
                        receiverTower.AddSenderTower(senderTower);
                        receiverTower.AddSenderLine(currentLine);
                        senderTower = null;
                        receiverTower = null;
                        currentLine = null;
                    }
                    else
                    {
                        Destroy(currentLine);
                        currentLine = null;
                    }
                }
                else
                {
                    Debug.Log("Fareyi býraktýk ama senderServer yok");
                }
            }
        }
    }
}