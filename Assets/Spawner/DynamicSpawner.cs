using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DynamicSpawner : MonoBehaviour {
  AIUtility ai_util;
  public enum SpawnerType { dynamic, static_top, static_bottom, pinpoint };
  public SpawnerType spawner_type;

  public List<GameObject> enemy_prefab_banks;
  public List<SpawnStruct.EnemyType> allowed_to_spawn;
  public float jitter;
  public float interval_between_spawn;
  AggressionController agc;
  float time_since_last_spawn;

  Dictionary<SpawnStruct.EnemyType, List<int>> agt;

	// Use this for initialization
	void Start () {
    InitializeAggressionTable();

    agc = GameObject.FindObjectOfType<AggressionController>();
    ReRollInterval();
    ai_util = GetComponent<AIUtility>();
  }
	
	// Update is called once per frame
	void Update () {
    if (ai_util.is_paused()) { return; }
    if (spawner_type != SpawnerType.pinpoint) {
      UpdateTimer();
    }
	}

  void InitializeAggressionTable() {
    agt = new Dictionary<SpawnStruct.EnemyType, List<int>>() {
      { SpawnStruct.EnemyType.toroid,       new List<int> { 3, 4, 5, 6, 7, 7, 7 } },
      { SpawnStruct.EnemyType.torkan,       new List<int> { 1, 1, 2, 2, 3, 3, 3 } },
      { SpawnStruct.EnemyType.kapi,         new List<int> { 1, 1, 2, 2, 3, 3, 3 } },
      { SpawnStruct.EnemyType.jara,         new List<int> { 1, 1, 2, 2, 3, 4, 5 } },
      { SpawnStruct.EnemyType.zakato,       new List<int> { 1, 1, 1, 2, 2, 2, 3 } },
      { SpawnStruct.EnemyType.brag_zakato,  new List<int> { 1, 1, 1, 1, 2, 2, 2 } },
      { SpawnStruct.EnemyType.zoshi,        new List<int> { 1, 1, 2, 2, 3, 3, 5 } },
      { SpawnStruct.EnemyType.bacura,       new List<int> { 1, 2, 2, 3, 3, 3, 3 } }
    };
  }

  public void PlaceEnemy(SpawnStruct.EnemyType enemy_type, float _pos_x, MapScroller.PinpointProperty pinpoint_property) {
    foreach (GameObject enemy_prefab in enemy_prefab_banks) {
      ObjectIdentifier object_identifier = enemy_prefab.GetComponent<ObjectIdentifier>();

      if (object_identifier == null) { continue; }
      if (object_identifier.enemy_type == enemy_type) {
        float pos_x = _pos_x;
        float pos_y = 0;
        switch(pinpoint_property) {
          case MapScroller.PinpointProperty.randomize_x:
            pos_x = Random.Range(-4f, 4f);
            break;
          case MapScroller.PinpointProperty.from_below:
            pos_y = -24f;
            break;
        }
        Instantiate(enemy_prefab, transform.position + new Vector3(pos_x, pos_y, 0), Quaternion.identity);
        break;
      }
    }
  }

  void UpdateTimer() {
    time_since_last_spawn -= Time.deltaTime;

    if (time_since_last_spawn < 0) {
      SelectEnemyToSpawn();
      ReRollInterval();
    }
  }

  void ReRollInterval() {
    time_since_last_spawn = interval_between_spawn + Random.Range(-jitter, +jitter);
  }

  void SelectEnemyToSpawn() {
    if (allowed_to_spawn.Count == 0) { return; }
    if (allowed_to_spawn.Count == 1 && allowed_to_spawn[0] == SpawnStruct.EnemyType.clear) { return; }

    int max_index = allowed_to_spawn.Count;
    // NOTE: Integer limit is high-exclusive
    int pick = Random.Range(0, max_index);

    SpawnStruct.EnemyType e_selected = allowed_to_spawn[pick];
    List<int> agl;
    agt.TryGetValue(e_selected, out agl);
    int scaled_spawn_count = agl[agc.Aggression];

    foreach (GameObject enemy_prefab in enemy_prefab_banks) {
      ObjectIdentifier object_identifier = enemy_prefab.GetComponent<ObjectIdentifier>();

      if (object_identifier == null) { continue; }
      if (enemy_prefab.GetComponent<ObjectIdentifier>().enemy_type == e_selected) {

        if (spawner_type == SpawnerType.static_top) {
          for (int i = 0; i < scaled_spawn_count; i++) {
            Vector3 jittered = transform.position + new Vector3(Random.Range(-5.0f, +5.0f), 0, 0);
            Instantiate(enemy_prefab, jittered, Quaternion.identity);
          }
        } else {
          for (int i = 0; i < scaled_spawn_count; i++) {
            Vector3 jittered = transform.position + new Vector3(Random.Range(-1.0f, +1.0f), Random.Range(-1.0f, +1.0f), 0);
            Instantiate(enemy_prefab, jittered, Quaternion.identity);
          }
        }
        break;
      }
    }
  }
}
