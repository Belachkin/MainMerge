﻿using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

namespace Kuhpik
{
    public class UIScreen : MonoBehaviour, IUIScreen
    {
        [SerializeField] [BoxGroup("Settings")] bool shouldOpenWithState;
        [SerializeField] [BoxGroup("Settings")] [ShowIf("shouldOpenWithState")] GameStateID[] statesToOpenWith;

        //You will get the idea once you use it
        [SerializeField] [BoxGroup("Elements")] bool hideElementsOnOpen;
        [SerializeField] [BoxGroup("Elements")] bool showElementsOnHide;

        [SerializeField] [BoxGroup("Elements")] [ShowIf("hideElementsOnOpen")] GameObject[] elementsToHideOnOpen;
        [SerializeField] [BoxGroup("Elements")] [ShowIf("showElementsOnHide")] GameObject[] elementsToShowOnHide;

        HashSet<GameStateID> statesMap;
        protected UIConfig config;

        void Awake()
        {
            statesMap = new HashSet<GameStateID>(statesToOpenWith);
        }

        public void Init(UIConfig config)
        {
            this.config = config;
        }

        public virtual void Open()
        {
            foreach (var element in elementsToHideOnOpen)
            {
                element.SetActive(false);
            }

            gameObject.SetActive(true);
        }

        public virtual void Close()
        {
            foreach (var element in elementsToShowOnHide)
            {
                element.SetActive(true);
            }

            gameObject.SetActive(false);
        }

        /// <summary>
        /// Subscribe is called on Awake()
        /// </summary>
        public virtual void Subscribe()
        {
        }

        internal void TryOpenWithState(GameStateID id)
        {
            if (shouldOpenWithState && statesMap.Contains(id))
            {
                Open();
            }
        }
    }
}