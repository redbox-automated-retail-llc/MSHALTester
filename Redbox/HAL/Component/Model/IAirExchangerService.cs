namespace Redbox.HAL.Component.Model
{
    public interface IAirExchangerService
    {
        AirExchangerStatus CheckStatus();

        void TurnOnFan();

        void TurnOffFan();

        AirExchangerStatus Reset();

        int PersistentFailureCount();

        bool ShouldRetry();

        void ResetFailureCount();

        void IncrementResetFailureCount();

        ExchangerFanStatus FanStatus { get; }

        bool Configured { get; }
    }
}
