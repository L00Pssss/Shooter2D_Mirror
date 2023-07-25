using Mirror;
using UnityEditor;
using UnityEngine;
// ���������� ������� �����. ��� ����� ���������� ������.
public class DeadlyArea : NetworkBehaviour
{
    [SerializeField] private float damagePerSecond = 10; // ���� � �������
    [SerializeField] private float moveSpeed = 2f; // �������� �������� �������
    [SerializeField] private float changeDirectionInterval = 5f; // �������� ����� �����������

    [SerializeField] private CircleArea spawnZone;


    private Vector3 moveDirection;

    private void Start()
    {
        // �������������� ��������� ����������� �������� ��������� �������
        moveDirection = spawnZone.GetRandomInsideZone();

        // ��������� ������������� ������� ��� ����� �����������
        InvokeRepeating(nameof(ChangeDirection), changeDirectionInterval, changeDirectionInterval);
    }

    private void Update()
    {
        // ������� ����������� ������� ������ ���� � ������� �����������
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    private void SpawnPointInsideZone()
    {
        // ���������, ���� �� ��������� CircleArea
        if (spawnZone == null)
        {
            Debug.LogError("No CircleArea assigned to DeadlyArea!");
            return;
        }

        // ������� ����� ������ ���� CircleArea
        Vector2 randomPoint = spawnZone.GetRandomInsideZone();
        // ����� ����� ������� ����� ��� ��������� ������ �������� ������ �������
        Debug.Log("Spawning point at: " + randomPoint);
    }


    [ServerCallback]
    private void OnTriggerStay2D(Collider2D collision)
    {
        Vehicle vehicle = collision.gameObject.GetComponentInParent<Vehicle>();

        if (vehicle != null)
        {
            ApplyBonusToVehicle(vehicle);
        }
    }

    private void ApplyBonusToVehicle(Vehicle vehicle)
    {
        // ����������� ���������� ���� �� ������ ������������� ��������
        vehicle.SvApplyDamage(damagePerSecond * Time.deltaTime);
    }

    private void ChangeDirection()
    {
        // ��� ������ ���� ������� ������ ����������� �������� �� ���������
        moveDirection = spawnZone.GetRandomInsideZone();
    }
}