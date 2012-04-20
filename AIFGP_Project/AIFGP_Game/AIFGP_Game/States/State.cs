namespace AIFGP_Game
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    //This interface is used for states, to
    //start, execute, or end
    public interface State
    {
        void Enter(SmartFarmer i);

        void Execute(SmartFarmer i);

        void Exit(SmartFarmer i);
    }
}
