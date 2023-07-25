using UnityEngine;

[RequireComponent (typeof(Player))]
public class VehicleInput : MonoBehaviour
{
    private Player m_player;

    [SerializeField] private float sensitivity = 1.0f;

    private void Awake()
    {
        m_player = GetComponent<Player>();
    }

    private void Update()
    {
        if (m_player.isOwned && m_player.isLocalPlayer)
        {
            UpdateControlKeyboard();
            UpdateControlMouse();
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
            m_player.ActiveVechicle.Fire(WeaponType.Simple);

        if (Input.GetKeyDown(KeyCode.RightControl))
            m_player.ActiveVechicle.Fire(WeaponType.Rocket);

        m_player.ActiveVechicle.ThrustControl = thrust;
        m_player.ActiveVechicle.TorqueControl = torque;
    }


    private void UpdateControlMouse()
    {
        if (m_player.ActiveVechicle == null) return;

        // Получаем позицию мыши в экранных координатах
        Vector3 mousePositionScreen = Input.mousePosition;

        // Получаем угол поворота турели на основе позиции мыши
        Vector3 directionToMouse = mousePositionScreen - Camera.main.WorldToScreenPoint(m_player.ActiveVechicle.Turret.TurretVisualMode.position);
        float angle = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;

        // Отправляем запрос на поворот турели на сервер
        m_player.ActiveVechicle.Turret.CmdRotateTurretTowardsMouse(angle);
    }

}
