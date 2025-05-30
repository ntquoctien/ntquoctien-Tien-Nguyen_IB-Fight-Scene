using UnityEngine;

public class AutoTargeter : MonoBehaviour
{
    [Header("Targeting Settings")]
    public string targetTag = "Enemy"; // Tag mục tiêu muốn nhắm tới
    public float searchRadius = 5f;    // Bán kính tìm kiếm mục tiêu
    public Transform target;
    [HideInInspector] public GameObject currentTarget;

    void Update()
    {
        FindNearestTarget();
        if (currentTarget != null)
        {
            FaceTarget(currentTarget);
        }
    }
    void FindNearestTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);
        float minDistance = searchRadius;
        GameObject nearest = null;

        foreach (GameObject target in targets)
        {
            Transform hitPoint = target.transform.Find("HitPoint");
            if (hitPoint == null)
            {
                Debug.LogWarning("HitPoint not found on " + target.name);
                continue;
            }

            float distance = Vector3.Distance(transform.position, hitPoint.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = target;
            }
        }

        currentTarget = nearest;
    }


    public void FaceTarget(GameObject target)
    {
        Transform hitPoint = target.transform.Find("HitPoint");
        if (hitPoint == null)
        {
            Debug.LogWarning("HitPoint not found on target");
            return;
        }

        Vector3 direction = (hitPoint.position - transform.position).normalized;
        direction.y = 0f;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
        }
    }

    public GameObject GetCurrentTarget()
    {
        return currentTarget;
    }
}
