using Fusion;
using Ozi.ChartPlayer;
using Ozi.Weapon.Entity;
using UnityEngine;

public class EffectableEntity : BasicEntityBehaviour
{
    [SerializeField] private PlayerController playerController;
    private StatusManager statusManager;
    private BremenChartPlayer bremenChartPlayer;
    private CharacterData characterData;

    private void Start()
    {
        characterData = Resources.Load<CharacterData>(playerController.selectCharacter);
        Status = EntityStatus.FromCharacterData(characterData);
        
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