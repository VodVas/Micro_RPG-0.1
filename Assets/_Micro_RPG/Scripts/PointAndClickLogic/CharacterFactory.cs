//using UnityEngine;
//using Zenject;

//public class CharacterFactory : ICharacterFactory //TODO: бет с мыши даже когда враг мертв
//{
//    private readonly DiContainer _container;
//    private readonly EnemyView _enemyPrefab;
//    private readonly NPCView _npcPrefab;
//    private readonly IInteractionValidator _interactionValidator;
//    private readonly Transform _playerTransform;

//    public CharacterFactory(
//        DiContainer container,
//        EnemyView enemyPrefab,
//        NPCView npcPrefab,
//        IInteractionValidator interactionValidator,
//        Transform playerTransform)
//    {
//        _container = container;
//        _enemyPrefab = enemyPrefab;
//        _npcPrefab = npcPrefab;
//        _interactionValidator = interactionValidator;
//        _playerTransform = playerTransform;
//    }

//    public EnemyView CreateEnemy(Vector3 position, float maxHealth)
//    {
//        var model = new EnemyModel(maxHealth);
//        var view = _container.InstantiatePrefabForComponent<EnemyView>(_enemyPrefab, position, Quaternion.identity, null);
//        view.Initialize(model);
//        return view;
//    }

//    public NPCView CreateNPC(Vector3 position, string npcId, float interactionDistance)
//    {
//        var model = new NPCModel(npcId, interactionDistance);
//        var view = _container.InstantiatePrefabForComponent<NPCView>(_npcPrefab, position, Quaternion.identity, null);
//        view.Initialize(model, _interactionValidator, _playerTransform);
//        return view;
//    }
//}


using UnityEngine;
using Zenject;

public class CharacterFactory : ICharacterFactory
{
    private readonly DiContainer _container;
    //private readonly EnemyView _enemyPrefab;
    private readonly NPCView _npcPrefab;
    private readonly IInteractionValidator _interactionValidator;
    private readonly Transform _playerTransform;
    private readonly EnemyViewPool _enemyPool;
    //private readonly NPCViewPool _npcPool;

    public CharacterFactory(
        DiContainer container,
        //EnemyView enemyPrefab,
        //[Inject(Id = "EnemyPrefab")] EnemyView enemyPrefab,
        //NPCView npcPrefab,
        IInteractionValidator interactionValidator,
        Transform playerTransform,
        EnemyViewPool enemyPool)
        //NPCViewPool npcPool)
    {
        _container = container;
        //_enemyPrefab = enemyPrefab;
        //_npcPrefab = npcPrefab;
        _interactionValidator = interactionValidator;
        _playerTransform = playerTransform;
        _enemyPool = enemyPool;
        //_npcPool = npcPool;
    }

    // Original methods for backward compatibility
    //public EnemyView CreateEnemy(Vector3 position, float maxHealth)
    //{
    //    var model = new EnemyModel(maxHealth);
    //    var view = _container.InstantiatePrefabForComponent<EnemyView>(_enemyPrefab, position, Quaternion.identity, null);
    //    view.Initialize(model);
    //    return view;
    //}

    public NPCView CreateNPC(Vector3 position, string npcId, float interactionDistance)
    {
        var model = new NPCModel(npcId, interactionDistance);
        var view = _container.InstantiatePrefabForComponent<NPCView>(_npcPrefab, position, Quaternion.identity, null);
        view.Initialize(model);
        view.SetDependencies(_interactionValidator, _playerTransform);
        return view;
    }

    // New pooled spawn methods
    public EnemyView SpawnEnemyFromPool(Vector3 position, float maxHealth)
    {
        var model = new EnemyModel(maxHealth);
        return _enemyPool.Spawn(model, position);
    }

    //public NPCView SpawnNPCFromPool(Vector3 position, string npcId, float interactionDistance)
    //{
    //    var model = new NPCModel(npcId, interactionDistance);
    //    return _npcPool.Spawn(model, position);
    //}

    public void DespawnEnemy(EnemyView enemy)
    {
        if (enemy != null)
        {
            _enemyPool.Despawn(enemy);
        }
    }

    //public void DespawnNPC(NPCView npc)
    //{
    //    if (npc != null)
    //    {
    //        _npcPool.Despawn(npc);
    //    }
    //}
}