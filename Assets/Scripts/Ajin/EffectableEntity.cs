using Fusion;
using UnityEngine;

public class EffectableEntity : MonoBehaviour
{
    [Networked] public int HP { get; set; }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void UpdateHP_RPC()
    {
        HP = PlayerData.Instance.hp;
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void UpdateCombo(int combo)
    {

    }
}