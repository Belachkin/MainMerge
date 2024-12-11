using System.Collections.Generic;
using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Components.Life;
using Source.Scripts.View;
using Source.Scripts.View.DataInitViewComponents;
using UnityEngine;

namespace Source.Scripts.EcsUtil
{
    public class Fabric
    {
        private EcsWorld world;
        private readonly SaveData saveData;
        private readonly GameData gameData;
        private readonly GameConfig config;
        private readonly Pools pool;

        public Fabric(EcsWorld world, SaveData saveData, GameData gameData, GameConfig config, Pools pool)
        {
            this.world = world;
            this.saveData = saveData;
            this.gameData = gameData;
            this.config = config;
            this.pool = pool;
        }
        

        public int InitView(BaseView baseView)
        {
            if (baseView == null)
                return -1;

            var position = baseView.transform.position;
            /*var respawnableView = baseView.GetComponent<RespawnableView>();
            if (respawnableView != null)
            {
               
            }
            else
            {
                /*
                if (saveData.DestoyedNonRespPositions.Contains(position))
                {
                    GameObject.DestroyImmediate(baseView.gameObject);
                    return -1;
                }
            }
            */

            //already inited
            if (baseView.Entity != 0)
                return baseView.Entity;


            var entity = world.NewEntity();
            baseView.Entity = entity;
            pool.View.Add(entity).Value = baseView;
            pool.Dir.Add(entity).Value = baseView.transform.forward;

            var rigidbody = baseView.GetComponent<Rigidbody>();
            if (rigidbody != null)
                pool.Rb.Add(entity).Value = rigidbody;
            
            var animationView = baseView.GetComponent<AnimationView>();
            if (animationView != null)
            {
                pool.Anim.Add(entity).Value = animationView;
            }

            var mergeMono = baseView.GetComponent<MergeMono>();
            if (mergeMono != null)
            {
                pool.Outline.Add(entity).Outline = mergeMono.Outline;
                pool.Hoverable.Add(entity);
                pool.MergeType.Add(entity).MergeType = mergeMono.Type;
                Debug.Log("Merge Mono");
            }
            
            //ui
           /* var hpBarView = baseView.GetComponent<HpBarView>();
            if (hpBarView != null)
            {
                pool.HpView.Add(entity).Value = hpBarView.HpBarUIView;
            }

            var counterView = baseView.GetComponent<CounterView>();
            if (counterView != null)
            {
                pool.CounterView.Add(entity).Value = counterView;
            }
            */
           
            //clear mono components
            var destroyOnInitViews = baseView.gameObject.GetComponents<DestroyComponentOnInitView>();
            for (int i = 0; i < destroyOnInitViews.Length; i++)
            {
                destroyOnInitViews[i].Destroy();
            }

            return entity;
        }
    }
}