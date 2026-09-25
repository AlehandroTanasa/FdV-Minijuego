using UnityEngine.InputSystem;
using UnityEngine;

public class Player : MonoBehaviour{
    private Rigidbody _rigidbody;
    private Vector3 thrustDirection;
    public float thrustForce = 100f;
    public float rotationSpeed = 120f;

    public GameObject BulletSpawner, BulletPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        _rigidbody = GetComponent<Rigidbody>();
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
                GameObject bullet = Instantiate(BulletPrefab, BulletSpawner.transform.position, Quaternion.identity);
                Bullet bulletScript = bullet.GetComponent<Bullet>();
                bulletScript.targetVector = transform.right;
            }
        }
    }

}
