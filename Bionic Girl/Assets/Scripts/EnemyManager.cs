using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour{

    public GameObject Enemy;
    public float SpawnTime = 0f;
    public Transform[] SpawnPoints;

    private GameObject _enemyHolder;

    // Use this for initialization
    private void Start(){

        _enemyHolder = GameObject.Find("Enemies").gameObject;;
        //InvokeRepeating("Spawn", SpawnTime, SpawnTime);
        Spawn();
    }

    void Spawn(){
        int spawnPointIndex = Random.Range(0, SpawnPoints.Length);

        var enemyObject = Instantiate(Enemy, SpawnPoints[spawnPointIndex].position, SpawnPoints[spawnPointIndex].rotation) as GameObject;
        enemyObject.transform.parent = _enemyHolder.transform;
    }
}
