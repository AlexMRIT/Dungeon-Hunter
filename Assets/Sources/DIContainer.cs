public sealed class DIContainer : IParamArgs
{
    private object[] _args;

    public void AddNewContainer(params object[] args)
    {
        _args = args;
    }

    public object[] GetContainer()
    {
        return _args;
    }
}