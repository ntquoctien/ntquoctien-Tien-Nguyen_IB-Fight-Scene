using UnityEngine;

public class GetHit : MonoBehaviour
{
    private Animator animator;
    private HealthSystem healthSystem;

    [SerializeField] private float lightHitDamage = 10f;
    [SerializeField] private float heavyHitDamage = 25f;

    void Start()
    {
        animator = GetComponent<Animator>();
        healthSystem = GetComponent<HealthSystem>();
    }

    public void PlayLightHit()
    {
            animator.SetTrigger("GetLightHit");
            healthSystem.TakeDmg(lightHitDamage);
    }

    public void PlayHeavyHit()
    {
            animator.SetTrigger("GetHeavyHit");
            healthSystem.TakeDmg(heavyHitDamage);
    }

    public void PlayKnockOut()
    {
        animator.SetTrigger("KnockOut");
    }
}
