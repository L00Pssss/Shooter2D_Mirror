using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : NetworkBehaviour
{
    [SerializeField] private Transform turretVisualMode;

    [Server]
    public void SVRotateTurretTowardsMouse(float sensitivity)
    {
        if (turretVisualMode != null)
        {
            Vector3 mousePositionScreen = Input.mousePosition;
            Vector3 mousePositionWorld = Camera.main.ScreenToWorldPoint(mousePositionScreen);
            Vector3 directionToMouse = mousePositionWorld - turretVisualMode.transform.position;
            float angle = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;
            turretVisualMode.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);

            CLRotateTurretTowardsMouse(angle);
        }
    }

    [ClientRpc]
    public void CLRotateTurretTowardsMouse(float angel)
    {
        if (turretVisualMode != null)
        {
            turretVisualMode.transform.rotation = Quaternion.Euler(0, 0, angel - 90f);
        }
    }
}
