using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tower : MonoBehaviour
{
    #region SFX and VFX
    [SerializeField]
    private GameObject alliedChangeParticle;
    [SerializeField]
    private GameObject oppositeChangeParticle;

    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip sendSoldierClip;
    [SerializeField]
    private AudioClip alliedClip;
    [SerializeField]
    private AudioClip oppositeClip;

    #endregion

    #region Attack Collider
    // Line
    [SerializeField]
    private AttackCollider attackColliderPrefab;
    [SerializeField]
    private AttackCollider currentAttackCollider;

    // Bu kuleye asker gönderen kulelerden gelen AttackCollider'lar
    [SerializeField]
    private List<AttackCollider> senderAttackColliders;

    // Bu kulenin asker göndereceði kulelere giden AttackCollider'lar
    [SerializeField]
    private List<AttackCollider> receiverAttackColliders;

    #endregion

    #region Health And Type
    //Kulenin caný ve cinsi ile ilgili
    public bool isAllied;

    public bool isOpposite;

    public bool canChange;

    public float health = 0;

    // Material deðiþimi
    [SerializeField]
    private Material alliedMaterial;
    [SerializeField]
    private Material oppositeMaterial;
    [SerializeField]
    private Material neutralMaterial;
    [SerializeField]
    private MeshRenderer meshRenderer;

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

    [SerializeField]
    private Soldier currentSoldier;
    [SerializeField]
    private Soldier oppositeSoldierPrefab;
    [SerializeField]
    private Soldier alliedSoldierPrefab;

    [SerializeField]
    private float sendSoldierTime;
    private float _sendSoldierTime;
    #endregion

    #region Opposite AI
    [SerializeField]
    private float oppositeAttackTime = 5;

    #endregion

    public bool isAttack = false;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        canChange = true;
    }

    private void Start()
    {
        if (isAllied)
        {
            TowerManager.towerManager.AddAlliedTower(this);
        }

        else if (isOpposite)
        {
            TowerManager.towerManager.AddOppositeTower(this);
            StartCoroutine(OppositeAttackRoutine());
        }

        else
        {
            TowerManager.towerManager.AddNeutralTower(this);
        }
    }

    private void Update()
    {
        isAttack = receiverTowers.Count > 0;

        if (!isAttack && (isOpposite || isAllied))
        {
            health += Time.deltaTime;
        }

        healthText.text = ((int)health).ToString();
        _sendSoldierTime = sendSoldierTime - health / 100;
        _sendSoldierTime = Mathf.Clamp(_sendSoldierTime, 0.15f, 5);
        // Debug.Log(this.name + " Send Soldier Time : " + _sendSoldierTime);

    }

    #region Opposite Attack
    public IEnumerator OppositeAttackRoutine()
    {
        yield return new WaitForSeconds(oppositeAttackTime);

        while (isOpposite)
        {
            while (receiverTowers.Count < 2)
            {
                if (((int)Random.Range(0, 11)) > 5)
                {
                    Tower _receiverTower = TowerManager.towerManager.GetTower();
                    if (_receiverTower != this)
                    {
                        AddReceiverTower(_receiverTower);
                        _receiverTower.AddSenderTower(this);
                        // Debug.Log("Attack Again: " + _receiverTower);
                    }
                }
                //if (((int)Random.Range(0, 2)) > 0)
                //{
                //    RemoveReceiverTower(receiverTowers[Random.Range(0, receiverTowers.Count)]);
                //    Debug.Log("Remove Receiver : ");
                //}
                //Debug.Log("Attack Routine");
                yield return new WaitForSeconds(oppositeAttackTime);
            }
            yield return new WaitForSeconds(oppositeAttackTime);
        }
    }
    #endregion

    #region Towers And Soldiers
    public IEnumerator SendSoldierRoutine()
    {
        while (isAttack)
        {
            while (receiverTowers.Count > 0 && isAllied)
            {
                for (int i = 0; i < receiverTowers.Count; i++)
                {
                    GenerateAlliedSoldier(receiverTowers[i].transform.position);
                    yield return new WaitForSeconds(_sendSoldierTime);
                }
                yield return new WaitForSeconds(_sendSoldierTime);
            }


            while (receiverTowers.Count > 0 && isOpposite)
            {
                for (int i = 0; i < receiverTowers.Count; i++)
                {
                    GenerateOppositeSoldier(receiverTowers[i].transform.position);
                    yield return new WaitForSeconds(_sendSoldierTime);
                }
                yield return new WaitForSeconds(_sendSoldierTime);
            }
            yield return new WaitForSeconds(_sendSoldierTime);
        }
    }

    public void GenerateAlliedSoldier(Vector3 towerPosition)
    {
        Soldier currentSoldier = Instantiate(alliedSoldierPrefab,
            transform.position, Quaternion.identity);

        currentSoldier.senderTower = this;
        currentSoldier.MovePosition(towerPosition);
        audioSource.PlayOneShot(sendSoldierClip);
    }

    public void GenerateOppositeSoldier(Vector3 towerPosition)
    {
        Soldier currentSoldier = Instantiate(oppositeSoldierPrefab,
            transform.position, Quaternion.identity);

        currentSoldier.senderTower = this;
        currentSoldier.MovePosition(towerPosition);
        audioSource.PlayOneShot(audioSource.clip);
    }

    public void AddReceiverTower(Tower _tower)
    {
        if (!receiverTowers.Contains(_tower))
        {
            CreateAttackCollider(_tower);

            if (isAllied && _tower.isAllied)
            {
                if (senderTowers.Contains(_tower))
                {
                    RemoveSenderTower(_tower);
                }
            }

            else if (isOpposite && _tower.isOpposite)
            {
                if (senderTowers.Contains(_tower))
                {
                    RemoveSenderTower(_tower);
                }
            }
        }

        if (!isAttack)
        {
            isAttack = true;
            StopCoroutine(SendSoldierRoutine());
            StartCoroutine(SendSoldierRoutine());
        }
    }


    public void CreateAttackCollider(Tower _tower)
    {
        currentAttackCollider = Instantiate(attackColliderPrefab, transform);
        currentAttackCollider.transform.position = Vector3.zero;
        currentAttackCollider.SetSenderTower(this);
        AddReceiverAttackCollider(currentAttackCollider);
        currentAttackCollider.SetReceiverTower(_tower);
        _tower.AddSenderAttackCollider(currentAttackCollider);
        receiverTowers.Add(_tower);
    }

    public void RemoveReceiverTower(Tower _tower)
    {
        if (receiverTowers.Contains(_tower))
        {
            receiverTowers.Remove(_tower);
            for (int i = 0; i < receiverAttackColliders.Count; i++)
            {
                if (receiverAttackColliders[i].GetReceiverTower() == _tower)
                {
                    RemoveReceiverAttackCollider(receiverAttackColliders[i]);
                }
            }
        }
    }

    public void AddSenderTower(Tower _tower)
    {
        if (!senderTowers.Contains(_tower))
        {
            senderTowers.Add(_tower);

            if (isAllied && _tower.isAllied)
            {
                if (receiverTowers.Contains(_tower))
                {
                    RemoveReceiverTower(_tower);
                }
            }

            else if (isOpposite && _tower.isOpposite)
            {
                if (receiverTowers.Contains(_tower))
                {
                    RemoveReceiverTower(_tower);
                }
            }
        }
    }

    public void RemoveSenderTower(Tower _tower)
    {
        if (senderTowers.Contains(_tower))
        {
            senderTowers.Remove(_tower);
            for (int i = 0; i < senderAttackColliders.Count; i++)
            {
                if (senderAttackColliders[i].GetSenderTower() == _tower)
                {
                    RemoveSenderAttackCollider(senderAttackColliders[i]);
                }
            }
        }
    }

    // Attack Collider hareketleri
    public void AddReceiverAttackCollider(AttackCollider _attackCollider)
    {
        receiverAttackColliders.Add(_attackCollider);
    }
    public void RemoveReceiverAttackCollider(AttackCollider _attackCollider)
    {
        receiverAttackColliders.Remove(_attackCollider);
        Destroy(_attackCollider.gameObject);
    }
    public void AddSenderAttackCollider(AttackCollider _attackCollider)
    {
        senderAttackColliders.Add(_attackCollider);
    }
    public void RemoveSenderAttackCollider(AttackCollider _attackCollider)
    {
        senderAttackColliders.Remove(_attackCollider);
    }
    #endregion

    // Kulenin caný ve cinsi ile ilgili
    public void IncreaseHealth()
    {
        health++;

        if (isAllied && canChange)
        {
            canChange = false;
            receiverTowers.Clear();

            for (int i = 0; i < receiverAttackColliders.Count; i++)
            {
                Destroy(receiverAttackColliders[i].gameObject);
            }

            receiverAttackColliders.Clear();
        }
        else if (isOpposite && canChange)
        {
            canChange = false;
            receiverTowers.Clear();

            for (int i = 0; i < receiverAttackColliders.Count; i++)
            {
                Destroy(receiverAttackColliders[i].gameObject);
            }

            receiverAttackColliders.Clear();
            StartCoroutine(OppositeAttackRoutine());
        }
    }

    public void DecreaseHealth()
    {
        health--;

        if (health <= 0)
        {
            meshRenderer.material = neutralMaterial;
            isAllied = false;
            isOpposite = false;
            canChange = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Soldier"))
        {
            if (other.GetComponent<Soldier>().isAllied)
            {
                if (other.GetComponent<Soldier>().senderTower != this)
                {
                    Destroy(other.gameObject);
                    if (isOpposite)
                    {
                        DecreaseHealth();
                        if (health < 0)
                        {
                            ChangeTower(true, alliedMaterial, alliedChangeParticle);
                            TowerManager.towerManager.AddAlliedTower(this);
                        }
                    }
                    else if (isAllied)
                    {
                        IncreaseHealth();
                    }
                    else
                    {
                        DecreaseHealth();
                        if (health < 0)
                        {
                            ChangeTower(true, alliedMaterial, alliedChangeParticle);
                            TowerManager.towerManager.AddAlliedTower(this);
                        }
                    }
                }
            }
            else
            {
                if (other.GetComponent<Soldier>().senderTower != this)
                {
                    Destroy(other.gameObject);
                    if (isAllied)
                    {
                        DecreaseHealth();
                        if (health < 0)
                        {
                            ChangeTower(false, oppositeMaterial, oppositeChangeParticle);
                            TowerManager.towerManager.AddOppositeTower(this);
                        }
                    }
                    else if (isOpposite)
                    {
                        IncreaseHealth();
                    }
                    else
                    {
                        DecreaseHealth();
                        if (health < 0)
                        {
                            ChangeTower(false, oppositeMaterial, oppositeChangeParticle);
                            TowerManager.towerManager.AddOppositeTower(this);
                        }
                    }
                }
            }
        }
    }

    private void ChangeTower(bool _allied, Material _material, GameObject _effect)
    {
        if (_allied)
        {
            audioSource.PlayOneShot(alliedClip);
        }
        else if (!_allied)
        {
            audioSource.PlayOneShot(oppositeClip);
        }
        isOpposite = !_allied;
        isAllied = _allied;
        health = 1;
        meshRenderer.material = _material;
        Destroy(Instantiate(_effect, transform.position, Quaternion.identity), 1.5f);
    }
}
