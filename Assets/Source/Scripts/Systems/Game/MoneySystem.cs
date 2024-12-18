using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Components.Events;
using Source.Scripts.UI.Screens;
using Unity.Mathematics;
using Random = UnityEngine.Random;

namespace Source.Scripts.Systems.Game
{
    public class MoneySystem : GameSystemWithScreen<MoneyUIScreen>
    {
        private EcsFilter filter;
        private EcsFilter filter2;
        public override void OnInit()
        {
            base.OnInit();
            screen.MoneyText.text = save.Money.ToString();

            filter = eventWorld.Filter<LevelCompleteEvent>().End();
            filter2 = eventWorld.Filter<UpdateMoneyEvent>().End();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            foreach (var e in filter)
            {
                int getMoney = 50 + Random.Range(0, 25);
                
                if (save.CurrentLevel == 0)
                {
                    getMoney = 100;
                }
                
                save.Money += getMoney;
                
                screen.MoneyText.text = save.Money.ToString();
                pool.AddMoneyEvent.Add(eventWorld.NewEntity()).Value = getMoney;
            }

            foreach (var e in filter2)
            {
                screen.MoneyText.text = save.Money.ToString();
            }
        }
    }
}