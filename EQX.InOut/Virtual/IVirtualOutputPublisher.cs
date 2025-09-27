namespace EQX.InOut.Virtual
{
    internal interface IVirtualOutputPublisher
    {
        void RegisterSubscriber(int outputPin, Action<bool> subscriber);
    }
}
