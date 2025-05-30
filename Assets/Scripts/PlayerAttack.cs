using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Animator animator;
    public PlayerMovement playerMovement;
    public Button lattackButton;
    public Button hattackButton;

    private bool hasAttacked = false;


    [SerializeField] private Collider rightweapon;
    [SerializeField] private Collider leftweapon;
    void Start()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();

        lattackButton = GameObject.Find("LightPunch").GetComponent<Button>();
        hattackButton = GameObject.Find("HeavyPunch").GetComponent<Button>();

    }

    void Update()
    {
        if (animator.GetBool("isAttacking")) return;

        if (hattackButton.isPressed && !hasAttacked)
        {
            DoHeavyAttack();
            hasAttacked = true;
        }
        else if (lattackButton.isPressed && !hasAttacked)
        {
            DoLightAttack();
            hasAttacked = true;
        }

        if (!hattackButton.isPressed && !lattackButton.isPressed)
        {
            hasAttacked = false; // Reset khi thả nút
        }
    }

    void DoLightAttack()
    {
        playerMovement.SetAttacking(true);
        animator.SetBool("isAttacking", true);
        animator.SetTrigger("LightPunch");
    }

    void DoHeavyAttack()
    {
        playerMovement.SetAttacking(true);
        animator.SetBool("isAttacking", true);
        animator.SetTrigger("HeavyPunch");
    }

    public void EndAttack()
    {
        playerMovement.SetAttacking(false);
        animator.SetBool("isAttacking", false);
    }
    public void EnableLeftWeapon() => leftweapon.enabled = true;
    public void EnableRightWeapon() => rightweapon.enabled = true;
    public void DisableAllWeapons()
    {
        leftweapon.enabled = false;
        rightweapon.enabled = false;
    }

}
