using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
    public float speed = 8f; 
    
    void Update()
    {
          if (PlayerMovement.isGameOver) return;
       
        transform.Translate(Vector3.back * speed * Time.deltaTime);
        
       
        if (transform.position.z < -5f)
        {
          
            Destroy(gameObject);
        }
    }
}