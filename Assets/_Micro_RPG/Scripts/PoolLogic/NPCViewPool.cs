//using UnityEngine;
//using Zenject;

//public class NPCViewPool : MonoMemoryPool<NPCModel, Vector3, NPCView>
//{
//    private readonly IInteractionValidator _interactionValidator;
//    private readonly Transform _playerTransform;

//    public NPCViewPool(IInteractionValidator interactionValidator, Transform playerTransform)
//    {
//        _interactionValidator = interactionValidator;
//        _playerTransform = playerTransform;
//    }

//    protected override void Reinitialize(NPCModel model, Vector3 position, NPCView view)
//    {
//        view.transform.position = position;
//        view.transform.rotation = Quaternion.identity;
//        view.Initialize(model);
//        view.SetDependencies(_interactionValidator, _playerTransform);
//        view.OnSpawned();
//    }

//    protected override void OnDespawned(NPCView view)
//    {
//        view.OnDespawned();
//        base.OnDespawned(view);
//    }

//    protected override void OnCreated(NPCView view)
//    {
//        view.SetPool(this);
//        base.OnCreated(view);
//    }
//}