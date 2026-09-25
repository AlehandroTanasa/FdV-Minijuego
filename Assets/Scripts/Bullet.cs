using UnityEngine;
using UnityEngine.UI;

public class Bullet : MonoBehaviour{

    public float speed = 6f;
    public float maxLifeTime = 3f;
    public Vector3 targetVector;
    public Vector3 shipVelocity = Vector3.zero;
    private Rigidbody _rigidbody;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.linearVelocity = shipVelocity + (transform.right * speed);
        Destroy(gameObject, maxLifeTime);
    }

    private void OnCollisionEnter(Collision collision){
        if(collision.gameObject.CompareTag("Asteroid")){
            increaseScore();
            Destroy(collision.gameObject);
            Destroy(gameObject);
            
        }
    }

    private void increaseScore(){
        Player.SCORE += 1;
        UpdateScoreText();
    }

    private void UpdateScoreText(){
        GameObject scoreText = GameObject.FindWithTag("UI");
        scoreText.GetComponent<Text>().text = "Puntos: " + Player.SCORE;
    }
}

