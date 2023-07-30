public interface IParamArgs
{
    public void AddNewContainer(params object[] args);
    public object[] GetContainer();
}