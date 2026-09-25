using UnityEngine;

public class Bullet : MonoBehaviour{

    public float speed = 6f;
    public float maxLifeTime = 2f;
    public Vector3 targetVector;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        Destroy(gameObject, maxLifeTime);
    }

    // Update is called once per frame
    void Update(){
        transform.Translate(targetVector * speed * Time.deltaTime);
    }
}
