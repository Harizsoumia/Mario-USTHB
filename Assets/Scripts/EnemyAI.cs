using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float speed = 4f;

    void Update()
    {
       transform.Translate(Vector3.forward * speed * Time.deltaTime);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Enemy hit the player");
        }
    }
}
