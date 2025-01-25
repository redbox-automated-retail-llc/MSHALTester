namespace Redbox.HAL.MSHALTester
{
    internal sealed class TransferLocation
    {
        internal readonly int Deck;
        internal readonly int Slot;

        internal bool IsValid => -1 != this.Deck && -1 != this.Slot;

        internal TransferLocation(int deck, int slot)
        {
            this.Deck = deck;
            this.Slot = slot;
        }
    }
}
