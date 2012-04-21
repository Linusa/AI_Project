namespace AIFGP_Game
{
    //This interface is used for states, to
    //start, execute, or end
    public interface State
    {
        void Enter(SmartFarmer i);

        void Execute(SmartFarmer i);

        void Exit(SmartFarmer i);
    }
}
