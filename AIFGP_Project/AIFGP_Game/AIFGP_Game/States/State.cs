namespace AIFGP_Game
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    //This interface is used for states, to
    //start, execute, or end
    interface State
    {
        void Enter(SimpleSensingGameEntity i);

        void Execute(SimpleSensingGameEntity i);

        void Exit(SimpleSensingGameEntity i);
    }
}
