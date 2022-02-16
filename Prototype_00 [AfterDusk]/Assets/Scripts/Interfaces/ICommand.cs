namespace AfterDusk
{
    public interface ICommand
    {
        void Execute();
        void Undo();
    }
}
