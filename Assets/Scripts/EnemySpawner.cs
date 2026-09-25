using UnityEngine;

public class EnemySpawner : MonoBehaviour{
    
    public GameObject asteroidPrefab;
    public float spawnRatePerMinute = 30f;
    public float spawnRateIncrement = 1f;
    public float maxLifetime = 6f;
    private float spawnNext = 0f;
    public float initialSpawnDelay = 2f;

    /*
    - randomX (inicializado dentro del método Update) genera un valor
      entre 0 y 1 para determinar la posición relativa del spawn en 
      el eje X respecto a la cámara.
    - spawnY es la posición relativa del spawn en el eye Y
      respecto de la camara. Se pone en 1.1 para que el spawn se genere 
      fuera de la vista de la cámara y no se vea el spawn del meteorito.
    */
    private float spawnY = 1.1f;
    void Start(){
        // Se inicializa el tiempo de spawn del primer meteorito a 1segundo
        // para que el jugador no muera instantaneamente al iniciar el juego.
        spawnNext = Time.time + initialSpawnDelay;
    }
    
    // Update is called once per frame
    void Update(){
        /*
        Se calcula la vista del jugador en la cámara con valores relativos 
        de X y Y, y se le suma respecto al plano más cercano rederizable a la
        cámara un valor z calculado a mano para que el meteorito quede entre 
        el espacio y la nave del jugador. Luego esto se transforma a coordenadas
        reales dentro del juego y se utiliza como posición de spawn para los meteoriots.
        */
        if(Time.time > spawnNext){
            spawnNext = Time.time + 60f / spawnRatePerMinute;
            spawnRatePerMinute += spawnRateIncrement;

            float randomX = Random.Range(0f, 1f);

            Vector3 playersView = new Vector3(randomX, spawnY, Camera.main.nearClipPlane + 7f);
            Vector3 spawnPosition = Camera.main.ViewportToWorldPoint(playersView);
            GameObject meteor = Instantiate(asteroidPrefab, spawnPosition, Quaternion.identity);

            Destroy(meteor, maxLifetime);
        }
    }

}
