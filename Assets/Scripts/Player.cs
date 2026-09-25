using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour{
    private Rigidbody _rigidbody;
    private Vector3 thrustDirection;
    public float thrustForce = 200f;
    public float rotationSpeed = 200f;

    public GameObject BulletSpawner, BulletPrefab;

    public static int SCORE = 0;
    public static float xBorderLimit, yBorderLimit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        _rigidbody = GetComponent<Rigidbody>();
        xBorderLimit = Camera.main.orthographicSize * Camera.main.aspect;
        yBorderLimit = Camera.main.orthographicSize;
    }

    // Update is called once per frame
    void FixedUpdate(){
        float thrustInput = 0f;
        float rotationIntput = 0f;

        if(Keyboard.current != null){
            if(Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed){
                thrustInput = 1f;
            } else if(Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed){
                thrustInput = -1f;
            }

            if(Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed){
                rotationIntput = 1f;
            } else if(Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed){
                rotationIntput = -1f;
            }
        }

        float thrust = thrustInput * thrustForce * Time.fixedDeltaTime;
        float rotation = rotationIntput * rotationSpeed * Time.fixedDeltaTime;

        thrustDirection = transform.right;
        transform.Rotate(Vector3.forward, rotation);
        _rigidbody.AddForce(thrust * thrustDirection);
    }

    void Update(){
        if(Keyboard.current != null){
            if(Keyboard.current.spaceKey.wasPressedThisFrame){
                GameObject bullet = Instantiate(BulletPrefab, BulletSpawner.transform.position, transform.rotation);
                Bullet bulletScript = bullet.GetComponent<Bullet>();
                bulletScript.shipVelocity = _rigidbody.linearVelocity;

                Collider bulletCollider = bullet.GetComponent<Collider>();
                Collider playerCollider = GetComponent<Collider>();
                Physics.IgnoreCollision(bulletCollider, playerCollider);
            }
            
            var newPosition = transform.position;

            if(newPosition.x > xBorderLimit){
                newPosition.x = -xBorderLimit + 0.5f;
            } 
            else if(newPosition.x < -xBorderLimit){
                newPosition.x = xBorderLimit - 0.5f;
            }
            if(newPosition.y > yBorderLimit){
                newPosition.y = -yBorderLimit + 0.5f;
            } 
            else if(newPosition.y < -yBorderLimit){
                newPosition.y = yBorderLimit - 0.5f;
            }
            transform.position = newPosition;
        }

    }

    private void OnCollisionEnter(Collision collision){
        if(collision.gameObject.CompareTag("Asteroid")){
            SCORE = 0;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

}
