using UnityEngine;

namespace GDD
{
    public interface IActorEnterExitHandler
    {
        void ActorEnter(GameObject actor);
        void ActorExit(GameObject actor);
    }
}