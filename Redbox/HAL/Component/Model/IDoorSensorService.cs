namespace Redbox.HAL.Component.Model
{
    public interface IDoorSensorService : IMoveVeto
    {
        DoorSensorResult Query();

        DoorSensorResult QueryStateForDisplay();

        bool SensorsEnabled { get; }

        bool SoftwareOverride { get; set; }
    }
}
