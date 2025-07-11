using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Most_Wanted.Scripts
{
    public class Music : MonoBehaviour
    {
        public AudioMixer mixer;

        public Slider slider;

        private void OnEnable()
        {
            slider?.onValueChanged.AddListener(delegate
            {
                float dB;
                // Evitar log(0) que sería -infinito
                if (slider.value > 0)
                    dB = Mathf.Log10(slider.value) * 20f;
                else
                    dB = -80f; // volumen mínimo seguro en dB

                mixer.SetFloat("Volume", dB);
                PlayerPrefs.SetFloat("MusicVolume", slider.value);
                PlayerPrefs.Save();
            });
            
            try
            {
               slider.value = PlayerPrefs.GetFloat("MusicVolume");
               float dB;
               // Evitar log(0) que sería -infinito
               if (slider.value > 0)
                   dB = Mathf.Log10(slider.value) * 20f;
               else
                   dB = -80f; // volumen mínimo seguro en dB

               mixer.SetFloat("Volume", dB);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private void OnDisable()
        {
            slider.onValueChanged.RemoveAllListeners();
        }
    }
}
