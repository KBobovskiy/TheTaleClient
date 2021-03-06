using System;
using System.Collections.Generic;
using System.Text;

namespace Resources.Enums
{
    public enum HeroStates : int
    {
        /// <summary>
        /// Unknown state, nothing to do.
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// The hero is idle.
        /// </summary>
        Idle = 10,

        /// <summary>
        /// The hero is dead.
        /// </summary>
        Dead = 20,

        /// <summary>
        /// The hero is fighting with mob.
        /// </summary>
        FightWithMob = 30,

        /// <summary>
        /// The hero have low health, need to heal him.
        /// </summary>
        LowHealth = 40,

        /// <summary>
        /// The hero have low health and he is fighting with a mob, need to kill the mob.
        /// </summary>
        LowHealthInFightWithMob = 50,
    }
}