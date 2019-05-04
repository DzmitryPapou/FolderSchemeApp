namespace TestTaskForSVAPSSystems.Interfaces
{
    public interface IReader<T>
    {
        T Read(string filename);
    }
}
