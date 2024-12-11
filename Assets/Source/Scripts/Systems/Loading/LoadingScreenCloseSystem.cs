using System.Collections;
using Kuhpik;
using Source.Scripts.SDK;
using Source.Scripts.UI;
using UnityEngine;

namespace Source.Scripts.Systems.Loading
{
    public class LoadingScreenCloseSystem : GameSystem
    {
        private LoadingUIScreen screen;

        public override void OnInit()
        {
            base.OnInit();
            screen = FindObjectOfType<LoadingUIScreen>();
            StartCoroutine(WaitFrame());
        }

        private IEnumerator WaitFrame()
        {
            yield return new WaitForEndOfFrame();
            //Debug.Log("1st frame time - "+ (Time.deltaTime));
            YandexManager.Instance.GameReadyAPIReady();
            screen.gameObject.SetActive(false);
        }
    }
}