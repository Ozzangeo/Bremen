using Fusion;
using Ozi.Weapon.Entity;
using UnityEngine;

public class EffectableEntity : BasicEntityBehaviour
{
    [SerializeField] private PlayerController playerController;
    private StatusManager statusManager;

    private void Start()
    {
        //statusManager = GameObject.Find("StatusManager").GetComponent<StatusManager>();
        //OnStatusChanged += o =>
        //{
            
        //};
    }
}