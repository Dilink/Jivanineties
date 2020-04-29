public interface IAbsorbable
{
    bool OnAbsorption();

    bool OnRestore();

    bool CanRestore();
}
