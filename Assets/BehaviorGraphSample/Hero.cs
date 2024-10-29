using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace BehaviorGraphSample
{
public class Hero : MonoBehaviour
{
   [SerializeField] private Button button;
   [SerializeField] private TMP_Text text;
   [SerializeField] private float pillDurationInSeconds = 5f;
   [SerializeField] private float minPillRespawnDurationInSeconds = 1f;
   [SerializeField] private float maxPillRespawnDurationInSeconds = 4f;
   [SerializeField] private float minEnemyRespawnInSeconds = 3f;
   [SerializeField] private float maxEnemyRespawnInSeconds = 5f;

   public bool HasEatenPill { get; private set; }
   private int _score;
   
   private SpriteRenderer _sprite;
   private float _pillDuration;
   
   private void Awake()
   {
      Time.timeScale = 1;
      if(!TryGetComponent(out _sprite))
         Debug.LogError("Could not find Sprite Component!");
      
      _sprite.color = Color.blue;
      _pillDuration = 0;
      button.interactable = false;
   }

   private void Start()
   {
      transform.position = new Vector3(Random.Range(-10f, 10f), Random.Range(-5f, 5f));
   }

   private void Update()
   {
      if (!HasEatenPill) return;
      
      if (_pillDuration > 0)
      {
         _pillDuration -= Time.deltaTime;
      }
      else
      {
         HasEatenPill = false;
         _sprite.color = Color.blue;
      }
   }

   private void OnCollisionEnter2D(Collision2D other)
   {
      if(other.gameObject.CompareTag("Enemy"))
      {
         if (HasEatenPill)
         {
            RandomSpawn(other.gameObject, minEnemyRespawnInSeconds, maxEnemyRespawnInSeconds, destroyCancellationToken);
            IncreaseScore();
         }
         else
         {
            EndGame();
         }
      }

      if (other.gameObject.CompareTag("Pill"))
      {
         EatPill();
         RandomSpawn(other.gameObject, minPillRespawnDurationInSeconds, maxPillRespawnDurationInSeconds, destroyCancellationToken);
      }
   }

   public void LoadScene()
   {
      var scene = SceneManager.GetActiveScene();
      SceneManager.LoadScene(scene.name);
      Time.timeScale = 1;
   }

   private async void RandomSpawn(GameObject go, float minTime, float maxTime, CancellationToken token)
   {
      go.SetActive(false);
      await Awaitable.WaitForSecondsAsync(Random.Range(minTime, maxTime), token);
      if (!token.IsCancellationRequested)
      {
         go.transform.position = new Vector3(Random.Range(-10f, 10f), Random.Range(-5f, 5f));
         go.SetActive(true);
      }
   }
   
   private void EatPill()
   {
      HasEatenPill = true;
      _pillDuration += pillDurationInSeconds;
      _sprite.color = Color.black;
   }

   private void EndGame()
   {
      Time.timeScale = 0;
      button.interactable = true;
   }

   private void IncreaseScore()
   {
      _score++;
      text.text = _score.ToString();
   }
}
}
