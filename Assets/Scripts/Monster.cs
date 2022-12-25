using System;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    [SerializeField] float _attackRange = 1f;
    [SerializeField] int _health = 2;

    [SerializeField]int _currentHealth;
    
    NavMeshAgent _navMeshAgent;
    Animator _animator;

    PlayerMovement player;

    bool isAttacking = false;
    bool isAlive => _currentHealth > 0;

    private void Awake()
    {
        _currentHealth = _health;
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }
    void Update()
    {
        if (!isAlive) 
            return;

        player = FindObjectOfType<PlayerMovement>();
        if (_navMeshAgent.enabled)
            _navMeshAgent.SetDestination(player.transform.position);
       
        if (Vector3.Distance(transform.position, player.transform.position) < _attackRange && !isAttacking)
        {
            isAttacking = true;
            Attack();           
        }                    
    }

    private void Attack()
    {
        _animator.SetTrigger("Attack");
        _navMeshAgent.enabled = false;
    }
    private void Die()
    {
        
        GetComponent<Collider>().enabled = false;
        _navMeshAgent.enabled = false;
        _animator.SetTrigger("Died");
        Destroy(gameObject, 5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        var blasterShot = collision.collider.GetComponent<BlasterShot>();
        if (blasterShot!= null)
        {
            _currentHealth--;
            if (_currentHealth <=0 )
                Die();
            else
                TakeHit();
        }
    }

    #region Animator Callbacks

    void AttackHit()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < _attackRange)
            Debug.Log("Killed Player");
    }
    void AttackComplete()
    {
        if(isAlive){
            _navMeshAgent.enabled = true;        
            isAttacking = false;
        }
        
    }

    private void TakeHit()
    {
        _navMeshAgent.enabled = false;
        _animator.SetTrigger("Hit");
    }

    void HitComplete() {
        if (isAlive)
            _navMeshAgent.enabled = true;
    }
    #endregion

    public void StartWalking()
    {
        _navMeshAgent.enabled = true;
        
    }
}
