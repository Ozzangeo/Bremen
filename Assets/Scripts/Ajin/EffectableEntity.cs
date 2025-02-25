using Fusion;
using Ozi.ChartPlayer;
using Ozi.Weapon.Entity;
using UnityEngine;

public class EffectableEntity : BasicEntityBehaviour
{
    [SerializeField] private PlayerController playerController;
    private StatusManager statusManager;
    private BremenChartPlayer bremenChartPlayer;

    private void Start()
    {
        Status = EntityStatus.FromCharacterData(playerController.selectCharacter);
        
        statusManager = GameObject.Find("StatusManager").GetComponent<StatusManager>();
        bremenChartPlayer = GameObject.FindAnyObjectByType<BremenChartPlayer>();

        OnStatusChanged += o =>
        {
            statusManager.UpdatePlayerStatusUI();
            playerController.hp = o.health;
            playerController.combo = bremenChartPlayer.Combo;
        };
    }
}