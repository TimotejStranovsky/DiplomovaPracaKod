using DigitalRuby.LightningBolt;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] ARSessionOrigin arOrigin;
    // Start is called before the first frame update
    void Start()
    {
        var player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        var ghostName = player.enemyName;
        var loadedGameObject = Resources.Load<GameObject>("ghosts/"+ghostName);
        var prefab = GameObject.Instantiate(loadedGameObject);
        prefab.transform.position = this.transform.position;
        prefab.GetComponent<RotateAroundTarget>().SetTarget(target);
        ApplyBonuses(player, prefab);
    }

    private void ApplyBonuses(PlayerController player, GameObject prefab)
    {
        var movementStats = prefab.GetComponent<RotateAroundTarget>();
        var enemyStats = prefab.GetComponent<Enemy>();

        if (player.battleInformation.increaseCaputreTime)
            enemyStats.ChangeMaxTime(player.battleInformation.ApplyTimeChange());
        if (player.battleInformation.increaseBattleReward)
            enemyStats.ChangeReward(player.battleInformation.ApplyRewardChange());
        if (player.battleInformation.stopRotationChange)
            movementStats.switchesRotation = player.battleInformation.ApplyStopRotatation();
        if (player.battleInformation.stopPhasingOut)
            movementStats.phasesOut = player.battleInformation.ApplyStopPhasing();
        if (player.battleInformation.reduceEnemyHP)
            enemyStats.GetHit(player.battleInformation.ApplyHPChange());
    }
}
