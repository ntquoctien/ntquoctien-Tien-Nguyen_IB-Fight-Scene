using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private float Health = 100f;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void TakeDmg(float damage)
    {
        Health -= damage;
        Health = Mathf.Max(Health, 0f);
        if (Health <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        GetComponent<GetHit>()?.PlayKnockOut();
        Destroy(gameObject, 3f);
    }
}
