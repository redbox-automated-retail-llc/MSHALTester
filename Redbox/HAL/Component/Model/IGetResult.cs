using System;


namespace Redbox.HAL.Component.Model
{
    public interface IGetResult
    {
        void Update(ErrorCodes newError);

        bool Success { get; }

        bool IsSlotEmpty { get; }

        bool ItemStuck { get; }

        string Previous { get; }

        ILocation Location { get; }

        DateTime? ReturnTime { get; }

        MerchFlags Flags { get; }

        ErrorCodes HardwareError { get; }
    }
}
