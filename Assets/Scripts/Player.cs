using Mirror;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SerializeField] 
    private Vehicle[] m_SpaceVehiclePrefab;

    private Transform[] spawnPoints; // Массив точек спавна

    public Vehicle ActiveVechicle {get;set;}

    [SyncVar]
    private Color playerColor;
    public Color PlayerColor => playerColor;

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (isOwned)
        {
            CmdSpawnVehicle();
        }
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        SpawnPoints spawnPoints = FindObjectOfType<SpawnPoints>();

        this.spawnPoints = spawnPoints.SpawnPoint;

        playerColor = PlayerColorPallete.Instance.TakeRandomColor();
    }

    public override void OnStopServer()
    {
        base.OnStopServer();

        PlayerColorPallete.Instance.PutColor(playerColor);

    }

    [Command]
    private void CmdSpawnVehicle()
    {
        SvSpwanClintVehicle();
    }

    [Server]
    public void SvSpwanClintVehicle()
    {
        if (ActiveVechicle != null) return;

        GameObject vehicle = m_SpaceVehiclePrefab[Random.Range(0, m_SpaceVehiclePrefab.Length)].gameObject;

        // Check if all spawn points are occupied
        bool allSpawnPointsOccupied = true;
        foreach (Transform spawnPoint in spawnPoints)
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll(spawnPoint.position, transform.localScale, LayerMask.GetMask("Ignore Raycast"));
            if (colliders.Length == 0)
            {
                allSpawnPointsOccupied = false;
                break;
            }
        }

        // If all spawn points are occupied, return without spawning the vehicle
        if (allSpawnPointsOccupied)
        {
            Debug.Log("All spawn points are occupied. Cannot spawn vehicle.");
            return;
        }

        // Find a free spawn point and spawn the vehicle
        while (true)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Collider2D[] colliders = Physics2D.OverlapBoxAll(spawnPoint.position, transform.localScale, LayerMask.GetMask("Ignore Raycast"));

            if (colliders.Length == 0)
            {
                GameObject playerVehicle = Instantiate(vehicle, spawnPoint.position, Quaternion.identity);
                NetworkServer.Spawn(playerVehicle, netIdentity.connectionToClient);

                ActiveVechicle = playerVehicle.GetComponentInParent<Vehicle>();
                ActiveVechicle.Owner = netIdentity;

                RpcSetVehicle(ActiveVechicle.netIdentity); // передача клиенту. 
                break;
            }
        }
    }

    [ClientRpc]
    private void RpcSetVehicle(NetworkIdentity vehicle)
    {
        ActiveVechicle = vehicle.GetComponent<Vehicle>();

        if (ActiveVechicle != null && ActiveVechicle.isOwned && VehicleCamera.Instance != null)
        {
            VehicleCamera.Instance.SetTarget(ActiveVechicle.transform); // передаем камеру. 
        }
    }



}
