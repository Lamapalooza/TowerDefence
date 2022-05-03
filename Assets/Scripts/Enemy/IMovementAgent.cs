using Fields;

namespace Enemy
{
    public interface IMovementAgent
    {
        void TickMovement();
        Node GetCurrentNode();
    }
}