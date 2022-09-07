using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public List<Tower> oppositeTowers;
    public List<Tower> alliedTowers;
    public List<Tower> neutralTowers;
    public List<Tower> allTowers;

    public static TowerManager towerManager;
    private void Awake()
    {
        if (!towerManager)
        {
            towerManager = this;
        }
        StartCoroutine(LevelControl());
    }

    private IEnumerator LevelControl()
    {
        yield return new WaitForSeconds(5);
        while (true)
        {
            if (GetAlliedTowerCount() == GetTowerCount())
            {
                //Debug.Log("Allied : " + GetAlliedTowerCount() +
                //    " / Opposite : " + GetOppositeTowerCount() +
                //    " / All : " + GetTowerCount());
                Manager.manager.FinishLevel(true);
            }
            else if (GetOppositeTowerCount() == GetTowerCount())
            {
                //Debug.Log("Allied : " + GetAlliedTowerCount() +
                //    " / Opposite : " + GetOppositeTowerCount() +
                //    " / All : " + GetTowerCount());
                Manager.manager.FinishLevel(false);
            }
            yield return new WaitForSeconds(1);
        }
    }

    public int GetTowerCount()
    {
        return allTowers.Count;
    }
    public int GetAlliedTowerCount()
    {
        return alliedTowers.Count;
    }
    public int GetOppositeTowerCount()
    {
        return oppositeTowers.Count;
    }

    public Tower GetTower()
    {
        return allTowers[Random.Range(0, allTowers.Count)];
    }

    public void AddAlliedTower(Tower _tower)
    {
        if (!alliedTowers.Contains(_tower))
        {
            RemoveNeutralTower(_tower);
            RemoveOppositeTower(_tower);
            alliedTowers.Add(_tower);
            AddTower(_tower);
        }
    }

    public void RemoveAlliedTower(Tower _tower)
    {
        if (alliedTowers.Contains(_tower))
        {
            alliedTowers.Remove(_tower);
        }
    }

    public void AddOppositeTower(Tower _tower)
    {
        if (!oppositeTowers.Contains(_tower))
        {
            RemoveNeutralTower(_tower);
            RemoveAlliedTower(_tower);
            oppositeTowers.Add(_tower);
            AddTower(_tower);
        }
    }

    public void RemoveOppositeTower(Tower _tower)
    {
        if (oppositeTowers.Contains(_tower))
        {
            oppositeTowers.Remove(_tower);
        }
    }

    public void AddNeutralTower(Tower _tower)
    {
        if (!neutralTowers.Contains(_tower))
        {
            RemoveAlliedTower(_tower);
            RemoveOppositeTower(_tower);
            neutralTowers.Add(_tower);
            AddTower(_tower);
        }
    }

    public void RemoveNeutralTower(Tower _tower)
    {
        if (neutralTowers.Contains(_tower))
        {
            neutralTowers.Remove(_tower);
        }
    }

    public void AddTower(Tower _tower)
    {
        if (!allTowers.Contains(_tower))
        {
            allTowers.Add(_tower);
        }
    }
}
