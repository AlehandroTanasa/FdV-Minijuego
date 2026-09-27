using UnityEngine;

public class Asteroid : MonoBehaviour{
    
    /*
    este script gestiona principalmente los colliders entre asteroides y el ciclo de vida
    de los mini asteroides. El ciclo de vida de los asteroides grandes es gestionado en 
    enemySpawner.

    - miniMeteorPrefab: prefab del mini meteorito
    - anguloDesviación: angulo respecto del vector que impacte con el meteorito a partir
                        del que se realiza la división en asteroides.
    - miniMeteorSpeed: velocidad del minimeteorito
    - maxLifeTimeMiniMeteos: tiempo de vida de los mini meteoritos
    */
    public GameObject miniMeteorPrefab;
    public float anguloDesviacion = 90f;
    public float miniMeteorSpeed = 10f;
    public float maxLifetimeMiniMeteors = 4f;

    /*
    El método es estático pues se llama en enemySpawner. Recibe un GameObject,
    se queda con su collider y con todos los GameObjects que compartan tags con este
    (sera un array de asteroides grandes). Luego lo recorre, se queda con el collider de
    cada uno de ellos y hace q el asteroide que acaba de spawnear ignore la hitbox de todos los demás
    asteroides grandes. Esto está hecho para que si dos asteroides aparecen juntos de forma aleatoria
    no colisiones entre si. Hacerlo en "OnCollision" me daba problemas porque primero chocaban
    y luego perdian la hitbox asique lo he hecho aparte y lo he llamado al crear el meteorito en el spawner.
    */
    public static void IgnoreAsteroidTypeCollisions(GameObject newAsteroid){

        Collider newCollider = newAsteroid.GetComponent<Collider>();
        GameObject[] existingAsteroids = GameObject.FindGameObjectsWithTag(newAsteroid.tag);

        foreach(GameObject existingAsteroid in existingAsteroids){
            Collider existingCollider = existingAsteroid.GetComponent<Collider>();
            Physics.IgnoreCollision(newCollider, existingCollider);
        }
    }
        
    /*
    El método se encarga de destruit el mini asteroide si este choca con un asteroide grande
    y romper el asteroide después, generando una mecánica de reacción en cadena entre asteroides.
    */
    private void OnCollisionEnter(Collision collision){
        if(collision.gameObject.CompareTag("MiniAsteroid")){
            Rigidbody MiniAsteroidRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            Destroy(collision.gameObject);
            BreakApart(MiniAsteroidRigidbody.linearVelocity);
        }
    }

    /*
    Método hitByBullet, se queda con la velocidad del vector y llama a breakApart.
    La única razón por la que bullet no llama a breakApart directamente con el vector
    de velocidad es que en una primera instancia me daba problemas asique lo separa en dos
    "rutas" una para balas y otra para asteroides para intentar depurar, pero este método
    podría evitarse.
    */
    public void HitByBullet(Vector3 bulletVelocity){
        BreakApart(bulletVelocity);
    }

    /*
    Se carga el asteroide y spawnea los mini meteoritos.
    */
    private void BreakApart(Vector3 impactVelocity){
        SpawnMiniMeteors(impactVelocity.normalized);
        Destroy(gameObject);
    }

    /*
    Normaliza el vector ya que solo nos interesa la dirección que llevaba el objeto
    que ha impactado con el asteroide. Luego calculamos las direcciones en las que se 
    moveran los asteroides que vayan a spawnear. Les damos un pequeño desplazamiento para
    que no colisionen entre sí al spawnear ya que asteroides pequeños si pueden colisionar 
    con otros pequeños  y ambos deberán de destruirse si eso pasa. Calculamos las posiciones de 
    spawn, los instanciamos, les damos una velocidad e iniciamos el tiempo de vida de los mini 
    asteroides.
    */
    private void SpawnMiniMeteors(Vector3 direction){
        direction.Normalize();

        Vector3 dir1 = Quaternion.Euler(0, 0, anguloDesviacion) * direction;
        Vector3 dir2 = Quaternion.Euler(0, 0, -anguloDesviacion) * direction;
    
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
