using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class VehicleColor : NetworkBehaviour
{
    [SerializeField] 
    private List<SpriteRenderer> m_SpriteRenderer;
    [SerializeField]
    private Vehicle m_Vehicle;

    private void Start()
    {
        SetVehicleColor();
    }

    private void SetVehicleColor()
    {

        if (m_SpriteRenderer == null || m_SpriteRenderer.Count == 0 || m_Vehicle == null)
            return;

        Color playerColor = m_Vehicle.Owner.GetComponent<Player>().PlayerColor;


        foreach (SpriteRenderer renderer in m_SpriteRenderer)
        {
            renderer.color = playerColor;
        }
    }
}
