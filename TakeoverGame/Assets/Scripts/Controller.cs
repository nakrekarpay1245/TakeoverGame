using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;

    [SerializeField]
    private GameObject attackCutter;

    [SerializeField]
    private LineRenderer attackLineRenderer;

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
                        attackLineRenderer.gameObject.SetActive(true);
                        senderTower = raycastHit.collider.gameObject.GetComponent<Tower>();
                        attackLineRenderer.SetPosition(0, senderTower.transform.position);
                    }
                }
                else
                {
                    attackCutter.transform.position = raycastHit.point;
                    attackCutter.SetActive(true);
                    // Debug.Log("Kuleye týklamadýk");
                }
            }
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 100))
            {
                if (senderTower)
                {
                    attackLineRenderer.SetPosition(1, raycastHit.point);
                }
                else
                {
                    attackCutter.transform.position = raycastHit.point;
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
                        senderTower.AddReceiverTower(receiverTower);
                        receiverTower.AddSenderTower(senderTower);
                        attackLineRenderer.SetPosition(1, receiverTower.transform.position);
                    }
                }
                else
                {
                    attackCutter.SetActive(false);
                    // Debug.Log("Fareyi býraktýk ama senderServer yok");
                }
            }
            senderTower = null;
            receiverTower = null;
            attackLineRenderer.gameObject.SetActive(false);
        }
    }
}