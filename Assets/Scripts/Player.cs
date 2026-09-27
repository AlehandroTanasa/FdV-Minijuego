using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.Pool;

public class Player : MonoBehaviour{

    /* 
    - rigidBody: componente rigidbody de la nave.
    - thrustDirection: vector de dirección de la nave.
    - thrustForce: fuerza de empuje de la nave.
    - rotationSpeed: velocidad de rotacion de la nave.
    - BulletSpawner: spawner para las balas.
    - BulletPrefab: prefab de las balas que se van a instanciar.
    - gameOverMenu: menu de gameOver que aparece al perder.
    - bulletPool: clase de Unity para guardar GameObjects y hacer pooling.
    - SCORE: variable estática para la puntuacion.
    - xBorderLimit, yBorderLimit: variables para gestionar que la nave no salga de la cámara (espacio infinito).
    */
    private Rigidbody rigidBody;
    private Vector3 thrustDirection;
    public float thrustForce = 200f;
    public float rotationSpeed = 200f;
    public GameObject BulletSpawner, BulletPrefab;
    public GameOverMenu gameOverMenu;
    private ObjectPool<Bullet> bulletPool;
    public static int SCORE = 0;
    public static float xBorderLimit, yBorderLimit;

    /* 
    Start is called once before the first execution of Update after the MonoBehaviour is created
    
    - Inicializa el rigidBody de la nave 
    - crea la pool de Bullets recibiendo como parámetros:
        - Método para la creación de balas cuando no quedan más en el pool
        - Método para sacar de la pull
        - Método para devolver a la pull
        - Método para destruir balas
        - booleano que decide si se comprueba o no duplicados
        - capacidad inicial del pool
        - capacidad máxima del pool 
    - Asigna los valores para el límite de la cámara
    */
    void Start(){
        rigidBody = GetComponent<Rigidbody>();
        bulletPool = new ObjectPool<Bullet>(CreateBullet, OnGetBullet, OnReleaseBullet, DestroyBullet, collectionCheck: true, defaultCapacity: 10, maxSize: 100);
        xBorderLimit = Camera.main.orthographicSize * Camera.main.aspect;
        yBorderLimit = Camera.main.orthographicSize;
    }

    /*
    Instancia la bala, 
    la inicializa pasando la referencia al método ReturnBulletToPool
    desactiva la bala para que no se vea
    */ 
    private Bullet CreateBullet(){
        Bullet bullet = Instantiate(BulletPrefab).GetComponent<Bullet>();
        bullet.Initialize(ReturnBulletToPool);
        bullet.gameObject.SetActive(false);
        return bullet;
    }

    /*
    Método llamado por "Get" de la clase UnityEngine.Pool al sacar balas de la pool
    El método solo muestra la bala activándola en escena, aunque el método
    Get también la saca de la pool para usarla.
    */
    private void OnGetBullet(Bullet bullet){
        bullet.gameObject.SetActive(true);
    }

    /*
    Método llamado por "Release" de la clase UnityEngine.Pool al devolver balas a la pool
    El método solo esconde la bala desactivandola en escena, aunque el método
    Release también la devuelve de la pool para usarla más adelante.
    */
    private void OnReleaseBullet(Bullet bullet){
        bullet.gameObject.SetActive(false);
    }

    /*
    Método usado cuando la pool supera el tamaño máximo de GameObjects
    */
    private void DestroyBullet(Bullet bullet){
        Destroy(bullet.gameObject);
    }

    /*
    Método que se pasa por referencia a bullet como accion. Se encarga
    de devolver la bullet a la pool cuando esta choque con un asteroide
    o se quede sin tiempo de vida, pero se gestiona en la clase bullet.
    */
    private void ReturnBulletToPool(Bullet bullet){
        bulletPool.Release(bullet);
    }

    /*
    Comprueba que tecla está siendo pulsada para asignar un valor (+/-) a la fuerza y a la rotación,
    luego calcula la fuerza de empuje y la de rotación y la aplica sobre la nave.
    */
    void FixedUpdate(){
        float thrustInput = 0f;
        float rotationIntput = 0f;

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

        float thrust = thrustInput * thrustForce * Time.fixedDeltaTime;
        float rotation = rotationIntput * rotationSpeed * Time.fixedDeltaTime;

        thrustDirection = transform.right;
        transform.Rotate(Vector3.forward, rotation);
        rigidBody.AddForce(thrust * thrustDirection);
    }

    /* 
    Update is called once per frame

    comprueba si se ha pulsado el espacio y si el tiempo no está parado (solución a un problema 
    donde la nave podía disparar mientras el juego estaba en el menu de pausa). Luego hace un get de la 
    pool, se guarda la bala y llama al método disparo de la clase bullet pasando la posición del spawner, 
    la orientación de la nave, la velocidad actual de la nave(usada para que las balas mantengan la inercia) y el
    collider de la nave(para que las balas y la nave no choquen si la nave alcanza grandes velocidades).

    El método también implementa el espacio infinito diferenciando eje X y eje Y.
    */
    void Update(){
        if(Keyboard.current.spaceKey.wasPressedThisFrame){
            if(Time.timeScale != 0f){
                Bullet bullet = bulletPool.Get();
                bullet.Disparo(BulletSpawner.transform.position, transform.rotation, rigidBody.linearVelocity, GetComponent<Collider>());
            }
        }
            
        var newPosition = transform.position;

        if(newPosition.x > xBorderLimit){
            newPosition.x = -xBorderLimit + 0.75f;
        } 
        else if(newPosition.x < -xBorderLimit){
            newPosition.x = xBorderLimit - 0.75f;
        }
        if(newPosition.y > yBorderLimit){
            newPosition.y = -yBorderLimit + 0.75f;
        } 
        else if(newPosition.y < -yBorderLimit){
            newPosition.y = yBorderLimit - 0.75f;
        }
        transform.position = newPosition;
    }

    /*
    Método para gestionar que la nave choque con un asteroide o mini asteroide, 
    muestra el menu de gameOver, detiene el script player y detiene la velocidad de la nave.
    */
    private void OnCollisionEnter(Collision collision){
        if(collision.gameObject.CompareTag("Asteroid") || collision.gameObject.CompareTag("MiniAsteroid")){
            SCORE = 0;
            
            gameOverMenu.Show();

            enabled = false;
            rigidBody.linearVelocity = Vector3.zero;
        }
    }

}
