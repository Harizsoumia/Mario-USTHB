using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    public Transform target;
    public float speed = 6f;

    void Start()
    {
        if (target == null)
        {
            GameObject p = GameObject.FindWithTag("Player");
            if (p != null) target = p.transform;
        }
    }

    void Update()
    {
        if (target == null) return;

        Vector3 dir = target.position - transform.position;
        dir.y = 0f;

        transform.position += dir.normalized * speed * Time.deltaTime;
        transform.forward = dir.normalized;
    }
}

