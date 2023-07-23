using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SpawnPoints : NetworkBehaviour
{
    [SerializeField]
    private Transform[] spawnPoints;

    public Transform[] SpawnPoint => spawnPoints;
}
