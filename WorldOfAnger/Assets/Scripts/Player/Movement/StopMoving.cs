using General.State;

namespace Player.Movement
{
    public class StopMoving : StateForMovement
    {
        protected override void Initialization_State()
        {
            base.Initialization_State();
            Priority = 1000;
        }
    }
}
