using System.Collections.Generic;

namespace TASBoardConsole.Definitions
{
    public sealed class TimelineLaneDefinition
    {
        public TimelineLaneDefinition(IReadOnlyList<InputAction> inputActions)
        {
            InputActions = inputActions;
        }

        public IReadOnlyList<InputAction> InputActions { get; }

        public float ProportionalSize { get; set; } = 1;
    }
}
