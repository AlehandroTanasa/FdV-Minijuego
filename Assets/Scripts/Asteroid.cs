using UnityEngine;

public class Asteroid : MonoBehaviour{
    
    public GameObject miniMeteorPrefab;
    public float anguloDesviacion = 90f;
    public float miniMeteorSpeed = 10f;
    public float maxLifetimeMiniMeteors = 4f;

    private void OnCollisionEnter(Collision collision){
        if(collision.gameObject.CompareTag("Bullet") || collision.gameObject.CompareTag("Asteroid")){
            Destroy(collision.gameObject);
            SpawnMiniMeteors(collision);
            Destroy(gameObject);
        }
    }

    private void SpawnMiniMeteors(Collision collision){
        Rigidbody bulletRigidbody = collision.gameObject.GetComponent<Rigidbody>();
        Vector3 bulletDirection = bulletRigidbody.linearVelocity.normalized;

        Vector3 dir1 = Quaternion.Euler(0, 0, anguloDesviacion) * bulletDirection;
        Vector3 dir2 = Quaternion.Euler(0, 0, -anguloDesviacion) * bulletDirection;
    
        float desplazamiento = 1f; 

        Vector3 spawnPosition1 = transform.position + dir1 * desplazamiento;
        Vector3 spawnPosition2 = transform.position + dir2 * desplazamiento;

        GameObject miniMeteor1 = Instantiate(miniMeteorPrefab, spawnPosition1, Quaternion.identity);
        GameObject miniMeteor2 = Instantiate(miniMeteorPrefab, spawnPosition2, Quaternion.identity);

        Destroy(miniMeteor1, maxLifetimeMiniMeteors);
        Destroy(miniMeteor2, maxLifetimeMiniMeteors);

        Rigidbody miniMeteorRigidbody1 = miniMeteor1.GetComponent<Rigidbody>();
        Rigidbody miniMeteorRigidbody2 = miniMeteor2.GetComponent<Rigidbody>();

        miniMeteorRigidbody1.linearVelocity = dir1 * miniMeteorSpeed;
        miniMeteorRigidbody2.linearVelocity = dir2 * miniMeteorSpeed;
    }

}
