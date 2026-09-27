using UnityEngine;

public class EnemySpawner : MonoBehaviour{
    
    // Prefab del meteorito que se va a instanciar
    public GameObject asteroidPrefab;

    /*
    - spawnRatePerMinute: cantidad de meteoritos que se van a generar por minuto.
    - spawnRateIncrement: incremento de la cantidad de meteoritos que se generan por minuto.
    - maxSpawnRate: cantidad máxima de meteoritos que se pueden generar por minuto (limita la dificultad máxima).
    - spawnNext: tiempo en el que se va a generar el siguiente meteorito.
    - initialSpawnDelay: tiempo de espera antes de generar el primer meteorito (hecho para no morir instantaneamente).
    - spawnY: posición relativa de Y donde se van a generar los meteoritos (1.1f es un poco más arriba de la vista del jugador).
    - maxLifetime: tiempo de vida de los asteroides.
    */
    public float spawnRatePerMinute = 30f;
    public float spawnRateIncrement = 1f;
    public float maxSpawnRate = 60f;
    private float spawnNext = 0f;
    public float initialSpawnDelay = 2f;
    public float maxLifetime = 6f;
    private float spawnY = 1.1f;

    /*
    Start is called before the first frame update
    
    Start inicializa el tiempo de spawn al momento acual + delat.
    */
    void Start(){
        spawnNext = Time.time + initialSpawnDelay;
    }
    
    /* 
    Update is called once per frame

    - Se calcula la vista del jugador en la cámara con valores relativos 
      de X e Y. La coordenada Z está calculada a mano para que quede alineada
      con el resto de elementos (La cámara está en -2 y los elementos en 5).
      Luego esto se transforma a coordenadas reales dentro del juego y se utiliza 
      como posición de spawn para los meteoriots.
    - Hay un límite de dificultad impuesto por maxSpawnRate
    - Como los asteroides spawnean de forma aleatoria en un rango de valores relativos a
      la cámara, puede que aparezcan demasiado cerca unos de otros, por lo que se llama
      al método IgnoreAsteroidTypeCollision de la clase Asteroio para que este meteorito
      ignore la hitbox de todo otro meteorito existente en la escena.
    */
    void Update(){
        if(Time.time > spawnNext){
            spawnNext = Time.time + 60f / spawnRatePerMinute;
            spawnRatePerMinute = spawnRatePerMinute >= maxSpawnRate ? maxSpawnRate : spawnRatePerMinute += spawnRateIncrement;

            float randomX = Random.Range(0f, 1f);

            Vector3 playersView = new Vector3(randomX, spawnY, 7f);
            Vector3 spawnPosition = Camera.main.ViewportToWorldPoint(playersView);
            GameObject meteor = Instantiate(asteroidPrefab, spawnPosition, Quaternion.identity);

            Asteroid.IgnoreAsteroidTypeCollisions(meteor);

            Destroy(meteor, maxLifetime);
        }
    }

}
