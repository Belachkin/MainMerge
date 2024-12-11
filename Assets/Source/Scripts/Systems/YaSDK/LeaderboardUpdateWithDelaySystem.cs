using System.Collections;
using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Components.Events;
using Source.Scripts.SDK;
using UnityEngine;

namespace Source.Scripts.Systems.YaSDK
{
    public class LeaderboardUpdateWithDelaySystem : GameSystem
    {
        [SerializeField] private float lbUpdateTick = 5;
        private EcsFilter filter;

        private bool canUpdate = true;
        private bool needUpdate;
        private Coroutine timer;

        public override void OnInit()
        {
            base.OnInit();
            filter = eventWorld.Filter<SaveEvent>().End();
            timer = StartCoroutine(LBUpdateTimer());
        }

        private IEnumerator LBUpdateTimer()
        {
            yield return new WaitForSeconds(lbUpdateTick);
            canUpdate = true;
            TryUpdateLB();
        }

        private void TryUpdateLB()
        {
            if (needUpdate && canUpdate)
            {
                //YandexManager.Instance.UpdateLB(save.Money);
                needUpdate = false;
                canUpdate = false;
                StopCoroutine(timer);
                timer = StartCoroutine(LBUpdateTimer());
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            foreach (var e in filter)
            {
                needUpdate = true;
                TryUpdateLB();
            }
        }
    }
}