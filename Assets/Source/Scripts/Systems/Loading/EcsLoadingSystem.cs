using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.EcsUtil;

namespace Source.Scripts.Systems.Loading
{
    public class EcsLoadingSystem : GameSystem
    {
        public override void OnInit()
        {
            base.OnInit();
            game.World = new EcsWorld();
            game.EventWorld = new EcsWorld();
            game.Pools = new Pools(game.World,game.EventWorld);
            game.Fabric = new Fabric(game.World,save,game,config,game.Pools);
            
            //init services
            //game.PositionService = new PositionService(game.World, player, game, config, game.Pools);
        }
    }
}