using System;
using System.Collections;
using AYellowpaper.SerializedCollections;
using Cinemachine;
using DG.Tweening;
using Source.Scripts.Data;
using UnityEngine;

namespace Source.Scripts.Helpers
{
     public class CameraSwitcher : MonoBehaviour
    {
        [SerializeField] private float scope;
        [SerializeField] private SerializedDictionary<CameraPositionType, CinemachineVirtualCamera> cameras;
        private CameraPositionType currentType;
        private CinemachineBasicMultiChannelPerlin shake;
        private float shakeTime;
        private Coroutine shakeCoroutine;
        private bool blocked;

        private void Awake()
        {
            currentType = CameraPositionType.MAIN;
            shake = cameras[currentType].GetComponentInChildren<CinemachineBasicMultiChannelPerlin>();
        }

        public void Switch(CameraPositionType type)
        {
            cameras[currentType].gameObject.SetActive(false);
            cameras[type].gameObject.SetActive(true);
            currentType = type;
        }


        public void ShowTarget(Transform target,Action onComplete,float halfFlyTime=1.5f,float waitTime=1f)
        {
            if (blocked || target == null)
                return;
            
            blocked = true;
           
           
            var cam = cameras[CameraPositionType.MAIN];
            var camFollow = cam.Follow;
            var camLookAt = cam.LookAt;
            //var fieldOfView = cam.m_Lens.FieldOfView;
            var delta = (target.position - camLookAt.position)+(cam.transform.forward)*scope;
            cam.LookAt = null;
            cam.Follow = null;
            DOTween.Sequence()
                .SetUpdate(UpdateType.Normal, true)
                .Append(cam.transform.DOMove(delta, halfFlyTime).SetRelative().SetEase(Ease.InOutSine))
                .AppendInterval(waitTime)
                .Append(cam.transform.DOMove(-delta, halfFlyTime).SetRelative().SetEase(Ease.InOutSine))
                .OnComplete(() =>
                {
                    cam.LookAt = camLookAt;
                    cam.Follow = camFollow;
                    blocked = false;
                    onComplete?.Invoke();
                });
        }

        public void Shake(float value, float time)
        {
            shakeTime += time;
            shake.m_AmplitudeGain += value;
            ;
            if (shakeCoroutine == null)
            {
                shakeCoroutine = StartCoroutine(WaitShake());
            }
        }

        public void SetShake(float value)
        {
            shake.m_AmplitudeGain = value;
        }

        private IEnumerator WaitShake()
        {
            while (shakeTime > 0)
            {
                shakeTime -= Time.deltaTime;
                yield return null;
            }

            shakeTime = 0;
            shake.m_AmplitudeGain = 0;
            shakeCoroutine = null;
        }
    }
}