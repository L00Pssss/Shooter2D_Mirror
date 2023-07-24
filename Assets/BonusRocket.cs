using UnityEngine;
using Mirror;

public class BonusRocket : NetworkBehaviour
{
    [SerializeField]
    private int bonusAmmo = 1;

    [ServerCallback]
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Vehicle vehicle = collision.gameObject.GetComponentInParent<Vehicle>();

        if (vehicle != null)
        {
            ApplyBonusToVehicle(vehicle);
        }
    }

    [Server]
    private void ApplyBonusToVehicle(Vehicle vehicle)
    {
        // ����������� ���������� ���� �� ������ ������������� ��������
        vehicle.Turret.Ammo += bonusAmmo;

        // ���������� ����� ����� ����������
        NetworkServer.Destroy(gameObject);
    }
}