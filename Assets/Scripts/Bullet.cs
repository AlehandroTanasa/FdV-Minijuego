using UnityEngine;
using UnityEngine.UI;
using System;

public class Bullet : MonoBehaviour{

    /*
    - speed: velocidad de la bala
    - maxLifeTime: tiempo de vida de la bala
    - rigidBody: componente rigidBody de la bala
    - returnToPool: variable que guarda la referencia al metodo enviado por player
    - lifeRemaining: tiempo de vida restante
    */
    public float speed = 6f;
    public float maxLifeTime = 3f;
    private Rigidbody rigidbody;
    private Action<Bullet> returnToPool;
    private float lifeRemaining;

    // Guarda la referencia al método que envía la clase player
    public void Initialize(Action<Bullet> returnToPool){
        this.returnToPool = returnToPool;
    }

    /* 
    Método awake usado para inicializar el rigidBody y arreglar un "nullReferenceException"
    no me servía el método start ya que las balas inicialmente estan inactivas
    */
    private void Awake(){
        rigidbody = GetComponent<Rigidbody>();
    }

    /*
    Método para disparar, le asigna a la bala una dirección, una posición, calcula 
    la velocidad añadiendo la velocidad de la nave (inercia) y la velocidad del disparo
    y se asegura de hacer que la hitbox de bala y nave se ignoren.
    */
    public void Disparo(Vector3 position, Quaternion rotation, Vector3 shipVelocity, Collider playerCollider){
        transform.SetPositionAndRotation(position, rotation);
        rigidbody.linearVelocity = shipVelocity + (rotation * Vector3.right * speed);
        lifeRemaining = maxLifeTime;

        Collider bulletCollider = GetComponent<Collider>();
        Physics.IgnoreCollision(bulletCollider, playerCollider);
    }

    /*
    Comprueba el timepo de vida de la bala y la devuelve a la pool cuando acaba.
    */
    private void Update(){
        lifeRemaining -= Time.deltaTime;
        if(lifeRemaining <= 0f){
            returnToPool(this);
        }
    }

    /*
    Si es un asteroide llama al método HitByBullet con su velocidad,
    incrementa puntuación y se vuelve a la pool de balas.
    Si es un miniAsteroide lo destruye, suma puntos y vuelve a la pool de balas.
    */
    private void OnCollisionEnter(Collision collision){
        if(collision.gameObject.CompareTag("Asteroid")){
            Asteroid asteroid = collision.gameObject.GetComponentInParent<Asteroid>();
            asteroid.HitByBullet(rigidbody.linearVelocity);

            increaseScore();
            returnToPool(this);
        }
        if(collision.gameObject.CompareTag("MiniAsteroid")){
            Destroy(collision.gameObject);
            increaseScore();
            returnToPool(this);
        }
    }

    /*
    Método para incrementar puntos
    */
    private void increaseScore(){
        Player.SCORE += 1;
        UpdateScoreText();
    }


    /*
    Método para actualizar marcador.
    */
    private void UpdateScoreText(){
        GameObject scoreText = GameObject.FindWithTag("UI");
        scoreText.GetComponent<Text>().text = "Puntos: " + Player.SCORE;
    }
}

