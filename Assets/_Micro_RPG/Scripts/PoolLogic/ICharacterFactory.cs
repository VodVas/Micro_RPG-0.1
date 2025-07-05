using UnityEngine;

public interface ICharacterFactory
{
   // EnemyView CreateEnemy(Vector3 position, float maxHealth);
    //NPCView CreateNPC(Vector3 position, string npcId, float interactionDistance);
    EnemyView SpawnEnemyFromPool(Vector3 position, float maxHealth);
    //NPCView SpawnNPCFromPool(Vector3 position, string npcId, float interactionDistance);
    void DespawnEnemy(EnemyView enemy);
    //void DespawnNPC(NPCView npc);
}