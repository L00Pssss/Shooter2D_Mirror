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
    // ���� ��� ����� ����, ������ � ������.
    [SerializeField] private CircleArea m_Area;

    // ����� ��� �������� ��� ������ � ��� ��� ������������.
    [SerializeField] private SpawnMode m_SpawnMode;
    // ���������� 
    [SerializeField] private int m_NumSpawns;
    // ��� �����, ����� �������.
    [SerializeField] private float m_RespwnTime;


    private float m_Timer;

    private void Start()
    {
        if (m_SpawnMode == SpawnMode.Start)
        {
            // ������ ��� ������ 
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
            // ��������� �� ������ ������.
          //  int index = Random.Range(0, Bonus.Length);
          //  Debug.Log(index);
            // ������� ������� 
            GameObject bonusInstance = Instantiate(Bonus[0].gameObject);

            bonusInstance.transform.position = m_Area.GetRandomInsideZone();

            NetworkServer.Spawn(bonusInstance, connectionToClient);
            RpcSpawnBonusOnClients(bonusInstance);
        }
    }

    [ClientRpc]
    private void RpcSpawnBonusOnClients(GameObject bonusInstance)
    {
        // �� �������� �������� �����, ������� ���������� �����
        OnBonusSpawned(bonusInstance);
    }

    private void OnBonusSpawned(GameObject bonusInstance)
    {
        // ���� �������������� �������� �� �������� ��� ��������� ������, ���� ����������
    }
}
