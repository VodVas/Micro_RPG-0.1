using UnityEngine;
using Zenject;

public class EnemyViewPool : MonoMemoryPool<EnemyModel, Vector3, EnemyView>
{
    protected override void Reinitialize(EnemyModel model, Vector3 position, EnemyView view)
    {
        view.transform.position = position;
        view.transform.rotation = Quaternion.identity;
        view.Initialize(model);
        view.OnSpawned();
    }

    protected override void OnDespawned(EnemyView view)
    {
        view.OnDespawned();
        base.OnDespawned(view);
    }

    protected override void OnCreated(EnemyView view)
    {
        view.SetPool(this);
        base.OnCreated(view);
    }
}