public interface IPoolableView<TModel> : IPoolable where TModel : class
{
    void Initialize(TModel model);
    void Cleanup();
}