using Mirror;
using UnityEditor;
using UnityEngine;
// Необходимо создать точки. Для более корректной работы.
public class DeadlyArea : NetworkBehaviour
{
    [SerializeField] private float damagePerSecond = 10; // Урон в секунду
    [SerializeField] private float moveSpeed = 2f; // Скорость движения области
    [SerializeField] private float changeDirectionInterval = 5f; // Интервал смены направления

    [SerializeField] private CircleArea spawnZone;


    private Vector3 moveDirection;

    private void Start()
    {
        // Инициализируем начальное направление движения случайным образом
        moveDirection = spawnZone.GetRandomInsideZone();

        // Запускаем периодическую функцию для смены направления
        InvokeRepeating(nameof(ChangeDirection), changeDirectionInterval, changeDirectionInterval);
    }

    private void Update()
    {
        // Двигаем смертельную область каждый кадр в текущем направлении
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    private void SpawnPointInsideZone()
    {
        // Проверяем, есть ли компонент CircleArea
        if (spawnZone == null)
        {
            Debug.LogError("No CircleArea assigned to DeadlyArea!");
            return;
        }

        // Спавним точку внутри зоны CircleArea
        Vector2 randomPoint = spawnZone.GetRandomInsideZone();
        // Здесь можно создать точку или выполнить другие действия внутри области
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
        // Увеличиваем количество аммо на турели транспортного средства
        vehicle.SvApplyDamage(damagePerSecond * Time.deltaTime);
    }

    private void ChangeDirection()
    {
        // При вызове этой функции меняем направление движения на случайное
        moveDirection = spawnZone.GetRandomInsideZone();
    }
}