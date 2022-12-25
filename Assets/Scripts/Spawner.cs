using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]float _spawnDelay = 12f; 
    [SerializeField] Monster _monsterPrefab;
    float _nextSpawnTime;
    

    void Update()
    {
        if (canSpawn()) {
            StartCoroutine(Spawn());
        }
    }

    IEnumerator Spawn()
    {
      _nextSpawnTime = Time.time + _spawnDelay; 
      var monster = Instantiate(_monsterPrefab, transform.position, transform.rotation);
      yield return new WaitForSeconds(1f);
      monster.StartWalking();      
    }

    bool canSpawn() => Time.time >= _nextSpawnTime;
    
}
