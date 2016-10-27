using UnityEngine;
using System.Collections;

public class SpawnStruct {
  public enum EnemyType { toroid, torkan, zoshi, grobda, domogram, kapi, zakato, brag_zakato, garu_zakato, jara, bacura, andor_genesis, logram, derota, boza_logram, garu_derota, barra, garu_barra, sol, zolbak, secret_life_up, clear, randomize_x, from_below };
  public EnemyType enemy_type;
  public float spawn_interval_min, spawn_interval_max;
  public int amount;
  public GameObject prefab;
  public Transform transform;

  float jitter;

  float elapsed_time;
  float threshold;

  public float enemy_count_threshold;

  public SpawnStruct(EnemyType _enemy_type, 
    GameObject _prefab, 
    Transform _transform, 
    float min, float max, int _amount,
    float _jitter = 1.5f) {

    enemy_count_threshold = 64;
    enemy_type = _enemy_type;
    prefab = _prefab;
    spawn_interval_min = min;
    spawn_interval_max = max;
    amount = _amount;
    transform = _transform;
    jitter = Mathf.Abs(_jitter);

    RollDice();
  }

  /// <summary>
  /// Spawns enemy
  /// </summary>
  /// <param name="delta_time">Increment time to threshold on when to spawn enemy</param>
	public void Cycle(float delta_time) {
    elapsed_time += delta_time;
    int on_screen_enemies = GameObject.FindObjectsOfType<AIUtility>().Length;

    if (elapsed_time > threshold && on_screen_enemies < enemy_count_threshold) {
      if (enemy_type == EnemyType.grobda || enemy_type == EnemyType.bacura || enemy_type == EnemyType.logram || enemy_type == EnemyType.derota) {
        SpawnEnemy(true);
      } else {
        SpawnEnemy();
      }
      RollDice();
    }
  }

  /// <summary>
  /// Spawn enemy at designated spawn point
  /// </summary>
  /// <param name="jitter_x_only">Set to true when spawning enemy at the top or bottom so </param>
  void SpawnEnemy(bool jitter_x_only = false) {
    for (int i = 0; i < amount; i++) {
      float jitter_x = Random.Range(-jitter, jitter);
      float jitter_y = jitter_x_only ? 0 : Random.Range(-jitter, jitter);

      Vector3 jittered_transform = new Vector3(transform.position.x + jitter_x, transform.position.y + jitter_y, transform.position.z);
      SpawnInstantiator.Spawn(prefab, jittered_transform);
    }
  }

  void RollDice() {
    elapsed_time = 0;
    threshold = Random.Range(spawn_interval_min, spawn_interval_max);
  }
}
