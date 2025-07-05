using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CharacterSpawnManager : MonoBehaviour
{
    private ICharacterFactory _characterFactory;
    private readonly List<EnemyView> _activeEnemies = new List<EnemyView>();
    private readonly List<NPCView> _activeNPCs = new List<NPCView>();

    [Inject]
    public void Construct(ICharacterFactory characterFactory)
    {
        _characterFactory = characterFactory;
    }

    public void SpawnEnemyWave(int count, float radius, float maxHealth)
    {
        for (int i = 0; i < count; i++)
        {
            var angle = i * (360f / count) * Mathf.Deg2Rad;
            var position = new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);

            var enemy = _characterFactory.SpawnEnemyFromPool(position, maxHealth);
            _activeEnemies.Add(enemy);
        }
    }

    //public void SpawnNPCGroup(Vector3[] positions, string npcIdPrefix, float interactionDistance)
    //{
    //    for (int i = 0; i < positions.Length; i++)
    //    {
    //        var npc = _characterFactory.SpawnNPCFromPool(
    //            positions[i],
    //            $"{npcIdPrefix}_{i}",
    //            interactionDistance
    //        );
    //        _activeNPCs.Add(npc);
    //    }
    //}

    public void DespawnAllEnemies()
    {
        foreach (var enemy in _activeEnemies)
        {
            _characterFactory.DespawnEnemy(enemy);
        }
        _activeEnemies.Clear();
    }

    //public void DespawnAllNPCs()
    //{
    //    foreach (var npc in _activeNPCs)
    //    {
    //        _characterFactory.DespawnNPC(npc);
    //    }
    //    _activeNPCs.Clear();
    //}

    private void OnDestroy()
    {
        DespawnAllEnemies();
       // DespawnAllNPCs();
    }
}