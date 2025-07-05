using UnityEngine;
using Zenject;

public class CharacterSystemInstaller : MonoInstaller
{
    [Header("Prefabs")]
    [SerializeField] private EnemyView _enemyPrefab;
    //[SerializeField] private NPCView _npcPrefab;

    [Header("Pool Settings")]
    [SerializeField] private CharacterPoolSettings _poolSettings = new CharacterPoolSettings();

    [Header("References")]
    [SerializeField] private Transform _playerTransform;

    public override void InstallBindings()
    {
        if (_enemyPrefab == null/* || _npcPrefab == null*/)
        {
            Debug.LogError("Character prefabs not assigned in CharacterSystemInstaller!");
            return;
        }

        //Container.BindInstance(_enemyPrefab).WithId("EnemyPrefab");
        //Container.BindInstance(_npcPrefab).WithId("NPCPrefab");

        //Container.Bind<IInteractionValidator>().To<InteractionValidator>().AsTransient();

        // Bind enemy pool
        Container.BindMemoryPool<EnemyView, EnemyViewPool>()
            .WithInitialSize(_poolSettings.EnemyPoolSettings.InitialSize)
            .WithMaxSize(_poolSettings.EnemyPoolSettings.MaxSize)
            //.WithExpandMethod(_poolSettings.EnemyPoolSettings.ExpandMethod)
            .FromComponentInNewPrefab(_enemyPrefab)
            .UnderTransformGroup("EnemyPool");

        //// Bind NPC pool with custom factory
        //Container.BindMemoryPool<NPCView, NPCViewPool>()
        //    .WithInitialSize(_poolSettings.NPCPoolSettings.InitialSize)
        //    .WithMaxSize(_poolSettings.NPCPoolSettings.MaxSize)
        //    //.WithExpandMethod(_poolSettings.NPCPoolSettings.ExpandMethod)
        //    .FromComponentInNewPrefab(_npcPrefab)
        //    .UnderTransformGroup("NPCPool");

        // Bind character factory
        Container.Bind<ICharacterFactory>()
            .To<CharacterFactory>()
            .AsSingle()
            .WithArguments(_playerTransform);
    }
}