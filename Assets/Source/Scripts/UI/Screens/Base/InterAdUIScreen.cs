using System.Collections;
using Kuhpik;
using TMPro;
using UnityEngine;

namespace Source.Scripts.UI
{
    public class InterAdUIScreen : UIScreen
    {
        [SerializeField] private TextMeshProUGUI numberDots;
        
        public void AnimateCountDots(float time)
        {
            StartCoroutine(DotsAnimation(time));
        }

        private IEnumerator DotsAnimation(float time)
        {
            float timeCount = time;
            int dotsNumber = 0;

            SetNumberDots(Mathf.CeilToInt(timeCount), dotsNumber);
            while (timeCount>0)
            {
                yield return new WaitForSeconds(0.25f);
                dotsNumber++;
                if (dotsNumber>3) 
                    dotsNumber = 0;
                SetNumberDots(Mathf.CeilToInt(timeCount), dotsNumber);
                timeCount -= 0.25f;
            }
        }

        private void SetNumberDots(int number, int dots)
        {
            string s = "";
            for (int i = 0; i < dots; i++) 
                s += ".";
            numberDots.text = number + s;
        }
    }
}