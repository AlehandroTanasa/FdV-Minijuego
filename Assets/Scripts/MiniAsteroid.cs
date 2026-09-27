using UnityEngine;

public class MiniAsteroid : MonoBehaviour{

    /*
    Si dos miniAsteroides chocan, se rompen los dos
    */
    public void OnCollisionEnter(Collision collision){
        if(collision.gameObject.CompareTag("MiniAsteroid")){
            Rigidbody asteroidRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
