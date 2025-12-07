namespace Core.Services
{
    public interface IWalletService
    {
        int Amount { get; }
        void Add(int amount);
        void Set(int amount);
    }
}