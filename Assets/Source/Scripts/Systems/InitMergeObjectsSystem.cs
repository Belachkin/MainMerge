using System;
using System.Collections.Generic;
using Kuhpik;
using Leopotam.EcsLite;
using Source.Scripts.Components;
using Source.Scripts.View;
using UnityEngine;

namespace Source.Scripts.Systems
{
    public class InitMergeObjectsSystem : GameSystem
    {
        [SerializeField] private List<BaseView> views;

        public override void OnInit()
        {
            base.OnInit();

            foreach (var view in views)
            {
                game.Fabric.InitView(view);
            }
            
        }
    }
}