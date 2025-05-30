using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public enum AttackType { Light, Heavy }
    public AttackType attackType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == transform.root.gameObject) return;

        GetHit target = other.GetComponent<GetHit>();

        if (target != null)
        {
            if (attackType == AttackType.Light)
                target.PlayLightHit();
            else if (attackType == AttackType.Heavy)
                target.PlayHeavyHit();
            GetComponent<Collider>().enabled = false;
        }
    }
}
