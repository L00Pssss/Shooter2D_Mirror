using Mirror;
using UnityEngine;

public class VehicleColor : NetworkBehaviour
{
    [SerializeField] 
    private SpriteRenderer m_SpriteRenderer;
    [SerializeField]
    private Vehicle m_Vehicle;

    private void Start()
    {
        m_SpriteRenderer.color = m_Vehicle.Owner.GetComponent<Player>().PlayerColor;
    }
}
