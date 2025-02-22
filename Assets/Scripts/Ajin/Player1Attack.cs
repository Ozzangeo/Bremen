using UnityEngine;

public class Player1Attack : MonoBehaviour, ICharacterAttack
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Attack()
    {
        Debug.Log("");
        _animator.SetTrigger("Attack");
        //
    }
}