using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    public float speed = 6f;
    public Transform player;

    void Start()
    {
        if (player == null)
        {
            var p = GameObject.Find("Player");
            if (p != null) player = p.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        // direction vers le joueur
        Vector3 dir = (player.position - transform.position);
        dir.y = 0f; // garder au sol

        // regarder vers le joueur
        if (dir.sqrMagnitude > 0.01f)
            transform.rotation = Quaternion.LookRotation(dir);

        // avancer vers le joueur
        transform.position += dir.normalized * speed * Time.deltaTime;
    }
}
