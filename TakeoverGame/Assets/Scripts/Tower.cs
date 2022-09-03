using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tower : MonoBehaviour
{
    #region Line
    // Line
    [SerializeField]
    private Line linePrefab;
    [SerializeField]
    private Line currentLine;
    #endregion
    #region Health And Type
    //Kulenin caný ve cinsi ile ilgili
    public bool isAllied;

    public bool isOpposite;

    public float health = 0;
    public float maxHealth = 5;

    // Material deðiþimi
    [SerializeField]
    private Material alliedMaterial;
    [SerializeField]
    private Material oppositeMaterial;
    [SerializeField]
    private Material neutralMaterial;
    [SerializeField]
    private MeshRenderer meshRenderer;

    // Deðiþim Particle'larý

    // Arayüz
    [SerializeField]
    private TextMeshProUGUI healthText;
    #endregion


    #region Towers And Soldiers
    // Kulelerin birbirine baðlanmasý ve asker gönderme ile ilgili
    // Bu kuleye asker gönderen kuleler
    [SerializeField]
    private List<Tower> senderTowers;

    // Bu kulenin asker göndereceði kuleler
    [SerializeField]
    private List<Tower> receiverTowers;

    // Bu kuleye asker gönderen kulelerden gelen Line'lar
    [SerializeField]
    private List<Line> senderLines;

    // Bu kulenin asker göndereceði kulelere giden Line'lan
    [SerializeField]
    private List<Line> receiverLines;

    [SerializeField]
    private Soldier currentSoldier;
    [SerializeField]
    private Soldier oppositeSoldierPrefab;
    [SerializeField]
    private Soldier alliedSoldierPrefab;

    [SerializeField]
    private float sendSoldierTime;
    #endregion
    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    #region Towers And Soldiers
    public IEnumerator SendSoldierRoutine()
    {
        if (isAllied)
        {
            while (receiverTowers.Count > 0)
            {
                for (int i = 0; i < receiverTowers.Count; i++)
                {
                    GenerateAlliedSoldier(receiverTowers[i].transform.position);
                    yield return new WaitForSeconds(sendSoldierTime);
                }
                yield return new WaitForSeconds(sendSoldierTime);
            }
        }
        else if (isOpposite)
        {
            while (receiverTowers.Count > 0)
            {
                for (int i = 0; i < receiverTowers.Count; i++)
                {
                    GenerateOppositeSoldier(receiverTowers[i].transform.position);
                    yield return new WaitForSeconds(sendSoldierTime);
                }
                yield return new WaitForSeconds(sendSoldierTime);
            }
        }
        else
        {
            yield return new WaitForSeconds(sendSoldierTime);
        }
    }

    public void GenerateAlliedSoldier(Vector3 towerPosition)
    {
        Soldier currentSoldier = Instantiate(alliedSoldierPrefab, transform.position, Quaternion.identity);
        currentSoldier.senderTower = this;
        currentSoldier.MovePosition(towerPosition);
    }

    public void GenerateOppositeSoldier(Vector3 towerPosition)
    {
        Soldier currentSoldier = Instantiate(oppositeSoldierPrefab, transform.position, Quaternion.identity);
        currentSoldier.senderTower = this;
        currentSoldier.MovePosition(towerPosition);
    }

    public void AddReceiverTower(Tower _tower)
    {
        if (!receiverTowers.Contains(_tower))
        {
            CreateLine(_tower);

            if (senderTowers.Contains(_tower))
            {
                RemoveSenderTower(_tower);
            }
        }


        StopAllCoroutines();
        StartCoroutine(SendSoldierRoutine());
    }

    public void CreateLine(Tower _tower)
    {
        // Line
        currentLine = Instantiate(linePrefab, transform);
        currentLine.transform.position = Vector3.zero;
        currentLine.SetSenderTower(this);
        AddReceiverLine(currentLine);
        currentLine.SetReceiverTower(_tower);
        _tower.AddSenderLine(currentLine);
        receiverTowers.Add(_tower);
    }

    public void RemoveReceiverTower(Tower _tower)
    {
        if (receiverTowers.Contains(_tower))
        {
            receiverTowers.Remove(_tower);
            for (int i = 0; i < receiverLines.Count; i++)
            {
                if (receiverLines[i].GetReceiverTower() == _tower)
                {
                    RemoveReceiverLine(receiverLines[i]);
                }
            }
        }
    }

    public void AddSenderTower(Tower _tower)
    {
        if (!senderTowers.Contains(_tower))
        {
            senderTowers.Add(_tower);
            if (receiverTowers.Contains(_tower))
            {
                RemoveReceiverTower(_tower);
            }
        }
    }
    public void RemoveSenderTower(Tower _tower)
    {
        if (senderTowers.Contains(_tower))
        {
            senderTowers.Remove(_tower);
            for (int i = 0; i < senderLines.Count; i++)
            {
                if (senderLines[i].GetSenderTower() == _tower)
                {
                    RemoveSenderLine(senderLines[i]);
                }
            }
        }
    }

    // Line hareketleri
    public void AddReceiverLine(Line _line)
    {
        receiverLines.Add(_line);
    }
    public void RemoveReceiverLine(Line _line)
    {
        receiverLines.Remove(_line);
        Destroy(_line.gameObject);
    }
    public void AddSenderLine(Line _line)
    {
        senderLines.Add(_line);
    }
    public void RemoveSenderLine(Line _line)
    {
        senderLines.Remove(_line);
    }
    #endregion

    // Kulenin caný ve cinsi ile ilgili
    public void IncreaseHealth()
    {
        health++;
        healthText.text = health.ToString();

        if (isAllied)
        {
            meshRenderer.material = alliedMaterial;
        }
        else if (isOpposite)
        {
            meshRenderer.material = oppositeMaterial;
        }
    }
    public void DecreaseHealth()
    {
        health--;
        healthText.text = health.ToString();

        if (health <= 0)
        {
            meshRenderer.material = neutralMaterial;
            isAllied = false;
            isOpposite = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Allied"))
        {
            if (other.GetComponent<Soldier>().senderTower != this)
            {
                Destroy(other.gameObject);
                if (isOpposite)
                {
                    DecreaseHealth();
                }
                else
                {
                    if (health <= 0)
                    {
                        isAllied = true;
                    }
                    IncreaseHealth();
                }
            }
        }

        if (other.gameObject.CompareTag("Opposite"))
        {
            if (other.GetComponent<Soldier>().senderTower != this)
            {
                Destroy(other.gameObject);
                if (isAllied)
                {
                    DecreaseHealth();
                }
                else
                {
                    if (health <= 0)
                    {
                        isOpposite = true;
                    }
                    IncreaseHealth();
                }
            }
        }
    }
}
