using Mirror;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SerializeField] 
    private Vehicle m_SpaceVehiclePrefab;

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

        GameObject playerVehicle = Instantiate(m_SpaceVehiclePrefab.gameObject, transform.position, Quaternion.identity);

        NetworkServer.Spawn(playerVehicle, netIdentity.connectionToClient);

        ActiveVechicle = playerVehicle.GetComponent<Vehicle>();
        ActiveVechicle.Owner = netIdentity;

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
