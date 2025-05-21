using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerSpawn : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform spawnPoint;

    private GameObject spawnedPlayer;
    private bool playerSpawned = false;

    // Update is called once per frame
    void Update()
    {
        if(!playerSpawned && Input.GetKeyDown(KeyCode.Return))
        {
            SpawnPlayer();
        }
    }

    void SpawnPlayer()
    {
        spawnedPlayer = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        playerSpawned = true;
        Debug.Log("Jugador spawneado en el punto de aparicion");
    }
}
