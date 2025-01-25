namespace Redbox.HAL.Component.Model
{
    public interface IMotionControlLimitResponse
    {
        bool IsLimitBlocked(MotionControlLimits limit);

        bool ReadOk { get; }

        IMotionControlLimit[] Limits { get; }
    }
}
