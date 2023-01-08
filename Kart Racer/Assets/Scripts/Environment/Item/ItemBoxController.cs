using Actor.Racer;
using Data.Audio;
using Manager;
using ScriptableObject.Audio;
using UnityEngine;
using Util.Helpers;

namespace Environment.Item
{
    [RequireComponent(typeof(Collider))]
    public class ItemBoxController : MonoBehaviour
    {
        [SerializeField] private AudioDataScriptableObject _itemBoxBreakAudioDataScriptableObject;

        private AudioSource _itemBoxBreakAudioSource;
        
        private Collider _collider;

        void Awake()
        {
            _collider = GetComponent<Collider>();

            _itemBoxBreakAudioSource = _itemBoxBreakAudioDataScriptableObject.AudioData.CreateNewAudioSource(gameObject);
        }
        
        void OnTriggerEnter(Collider collider)
        {
            var racer = collider.gameObject.GetComponent<RacerController>();
            if (racer != null)
            {
                racer.PickupItem();

                // _itemBoxBreakAudioSource.Play();
                AudioSource.PlayClipAtPoint(_itemBoxBreakAudioDataScriptableObject.AudioData.AudioClip, transform.position);
                gameObject.Disable();

                Invoke("Respawn", GameManager.Instance.Config.ItemConfig.TimeToRespawn);
            }
        }

        public void Respawn()
        {
            gameObject.Enable();
        }
    }
}
