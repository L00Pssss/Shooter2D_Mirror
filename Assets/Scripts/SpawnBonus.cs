using Mirror;
using UnityEngine;
public enum SpawnMode
{
    Start,
    Loop
}
public class SpawnBonus : NetworkBehaviour
{
    [SerializeField] private GameObject[] Bonus;
    // Зона где может быть, доступ к классу.
    [SerializeField] private CircleArea m_Area;

    // Какой мод спавнера при старте и все или переодически.
    [SerializeField] private SpawnMode m_SpawnMode;
    // Количество 
    [SerializeField] private int m_NumSpawns;
    // как часто, сброс таймера.
    [SerializeField] private float m_RespwnTime;


    private float m_Timer;

    private void Start()
    {
        if (m_SpawnMode == SpawnMode.Start)
        {
            // запуск при старте 
        }

        m_Timer = m_RespwnTime;
    }

    private void Update()
    {
        if (!isServer)
            return;


        if (m_Timer > 0)
        {
            m_Timer -= Time.deltaTime;
        }

        if (m_SpawnMode == SpawnMode.Loop && m_Timer < 0)
        {
            m_Timer = m_RespwnTime;

            BonusSpawner();
        }
    }

    [Server]
    private void BonusSpawner()
    {
        for (int i = 0; i < m_NumSpawns; i++)
        {
            // переменая из масива рандом.
          //  int index = Random.Range(0, Bonus.Length);
          //  Debug.Log(index);
            // спавним объекты 
            GameObject bonusInstance = Instantiate(Bonus[0].gameObject);

            bonusInstance.transform.position = m_Area.GetRandomInsideZone();

            NetworkServer.Spawn(bonusInstance, connectionToClient);
            RpcSpawnBonusOnClients(bonusInstance);
        }
    }

    [ClientRpc]
    private void RpcSpawnBonusOnClients(GameObject bonusInstance)
    {
        // На клиентах вызываем метод, который отображает бонус
        OnBonusSpawned(bonusInstance);
    }

    private void OnBonusSpawned(GameObject bonusInstance)
    {
        // Ваши дополнительные действия на клиентах при появлении бонуса, если необходимо
    }
}
