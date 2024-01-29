namespace EqualMoneySplit.Models
{
    /// <summary>
    /// Type of event triggering an action
    /// </summary>
    public enum EventContext
    {
        None,
        InventoryChanged,
        EndOfDay
    }
}
