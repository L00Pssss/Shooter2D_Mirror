using UnityEngine;

[RequireComponent (typeof(Player))]
public class VehicleInput : MonoBehaviour
{
    private Player m_player;

    private void Awake()
    {
        m_player = GetComponent<Player>();
    }

    private void Update()
    {
        if (m_player.isOwned && m_player.isLocalPlayer)
        {
            UpdateControlKeyboard();
        }
    }

    private void UpdateControlKeyboard()
    {

        if (m_player.ActiveVechicle == null) return;


        float thrust = 0;
        float torque = 0;

        if (Input.GetKey(KeyCode.UpArrow))
            thrust = 1.0f;

        if (Input.GetKey(KeyCode.DownArrow))
            thrust = -1.0f;

        if (Input.GetKey(KeyCode.LeftArrow))
            torque = 1.0f;

        if (Input.GetKey(KeyCode.RightArrow))
            torque = -1.0f;

        if (Input.GetKeyDown(KeyCode.Space))
            m_player.ActiveVechicle.Fire();

        m_player.ActiveVechicle.ThrustControl = thrust;
        m_player.ActiveVechicle.TorqueControl = torque;
    }

}
