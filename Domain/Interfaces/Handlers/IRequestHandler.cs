namespace Domain.Interfaces.Handlers
{
    public interface IRequestHandler<T, R>
    {
        R Handler(T command);
    }

    public interface IRequestHandler<T>
    {
        void Handler(T command);        
    }
}
