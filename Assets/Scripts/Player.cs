using Mirror;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SerializeField] 
    private Vehicle m_SpaceVehiclePrefab;

    public Vehicle ActiveVechicle {get;set;}
    public override void OnStartClient()
    {
        base.OnStartClient();

        if (isOwned)
        {
            CmdSpawnVehicle();
        }
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

        GameObject playerVehicle = Instantiate(m_SpaceVehiclePrefab.gameObject, transform.position, Quaternion.identity);

        NetworkServer.Spawn(playerVehicle, netIdentity.connectionToClient);

        ActiveVechicle = playerVehicle.GetComponent<Vehicle>();

        RpcSetVehicle(ActiveVechicle.netIdentity); // передача клиенту. 
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
