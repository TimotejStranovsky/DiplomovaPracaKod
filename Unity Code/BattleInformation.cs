using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleInformation : MonoBehaviour
{
    public float reduceEnemyHPAmount;
    public float increaseCaputreTimeAmount;
    public int increaseBattleRewardAmount;

    public bool reduceEnemyHP;
    public bool increaseCaputreTime;
    public bool increaseBattleReward;
    public bool stopRotationChange;
    public bool stopPhasingOut;

    public void SetHPReduceAmount(float amount)
    {
        if (reduceEnemyHP)
        {
            reduceEnemyHPAmount += amount;
        }
        else
        {
            reduceEnemyHP = true;
            reduceEnemyHPAmount = amount;
        }
    }

    public void SetTimeIncreaseAmount(float amount)
    {
        if (increaseCaputreTime)
        {
            increaseCaputreTimeAmount += amount;
        }
        else
        {
            increaseCaputreTime = true;
            increaseCaputreTimeAmount = amount;
        }
    }

    public void SetRewardIncreaseAmount(int amount)
    {
        if (increaseBattleReward)
        {
            increaseBattleRewardAmount += amount;
        }
        else
        {
            increaseBattleReward = true;
            increaseBattleRewardAmount = amount;
        }
    }

    public float ApplyHPChange()
    {
        reduceEnemyHP = false;
        return reduceEnemyHPAmount;
    }

    public float ApplyTimeChange()
    {
        increaseCaputreTime = false;
        return increaseCaputreTimeAmount;
    }

    public int ApplyRewardChange()
    {
        increaseBattleReward = false;
        return increaseBattleRewardAmount;
    }

    public bool ApplyStopRotatation()
    {
        var pom = stopRotationChange;
        stopRotationChange = false;
        return !pom;
    }

    public bool ApplyStopPhasing()
    {
        var pom = stopPhasingOut;
        stopPhasingOut = false;
        return !pom;
    }
}
