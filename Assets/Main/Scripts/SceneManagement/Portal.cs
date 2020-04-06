using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationIdentifier 
        {
            A, B, C, D, E
        }

        [SerializeField] int sceneToLoad = 1;
        [SerializeField] Transform spawnPoint;
        [SerializeField] DestinationIdentifier destination;
        [SerializeField] float fadeOutTime = 3f;
        [SerializeField] float fadeInTime = 2f;
        [SerializeField] float timeWaitingWhileFading = 2f;

        private void OnTriggerEnter(Collider other) 
        {
            
            if (other.tag == "Player")
            {

                StartCoroutine(Transition());
                
            }
        }
        private IEnumerator Transition()
        {
            if (sceneToLoad < 0) 
            {
                Debug.LogError("Scene to load not set");
                yield break;
            }

            Fader fader = FindObjectOfType<Fader>();
            yield return StartCoroutine(fader.FadeOut(fadeOutTime));
            yield return new WaitForSeconds(timeWaitingWhileFading);

            //Save current level
            SavingWraper wraper = FindObjectOfType<SavingWraper>();
            wraper.Save();

            DontDestroyOnLoad(this);
            yield return SceneManager.LoadSceneAsync(sceneToLoad);

            //Load current level
            wraper.Load();

            Portal otherportal = GetOtherPortal();
            UpdatePlayer(otherportal);
            
            wraper.Save();
            otherportal.gameObject.SetActive(false);

            yield return StartCoroutine(fader.FadeIn(fadeInTime));
            otherportal.gameObject.SetActive(true);
            Destroy(gameObject);
            
        }

        private Portal GetOtherPortal()
        {
            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) continue;
                if (portal.destination != destination) continue;
                return portal;
            }
            return null;
        }

        private void UpdatePlayer(Portal otherportal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().enabled=false;
            player.transform.position = otherportal.spawnPoint.position;
            player.transform.rotation = otherportal.spawnPoint.rotation;
            player.GetComponent<NavMeshAgent>().enabled = true;
        }


    }
}
