using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapScroller : MonoBehaviour {

	public float minDelay = 8.0f;
	public float maxDelay= 8.1f;
	AudioSource my_audio_source;
  public AudioClip in_game_music;
  public AudioClip intro_clip;

  public enum PinpointProperty { clear, randomize_x, from_below };
  public List<Sprite> map_sprites;
  AIUtility ai_util;

  public TextAsset level_descriptor_dynamic_spawner_text;
  public TextAsset level_descriptor_static_top_spawner_text;
  public TextAsset level_descriptor_static_bottom_spawner_text;
  public TextAsset level_descriptor_pinpoint_text;

  public float scroll_speed;
  public int current_area;
  int loop_count;

  LevelLoader level_loader;
  LevelDescriptor level_descriptor_dynamic_spawner;
  LevelDescriptor level_descriptor_static_top_spawner;
  LevelDescriptor level_descriptor_static_bottom_spawner;
  LevelDescriptor level_descriptor_pinpoint_spawner;

  List<SpawnStruct.EnemyType> allowed_enemy_types_in_dynamic_spawner;
  List<SpawnStruct.EnemyType> allowed_enemy_types_in_static_top_spawner;
  List<SpawnStruct.EnemyType> allowed_enemy_types_in_static_bottom_spawner;

  int latched_segment;

  Dictionary<int, List<SpawnStruct.EnemyType>> area_data_dynamic_spawner;
  Dictionary<int, List<SpawnStruct.EnemyType>> area_data_static_top_spawner;
  Dictionary<int, List<SpawnStruct.EnemyType>> area_data_static_bottom_spawner;
  Dictionary<Vector2, List<SpawnStruct.EnemyType>> area_data_pinpoint_spawner;

  SpriteRenderer[] sprite_renderers;
  float sprite_scaling_factor = 5f;
  bool sprite_has_been_setup;
  public bool one_shot_setup;
  bool map_deja_vu;

  Dictionary<int, int> pinpoint_x_position;
  Dictionary<int, SpawnStruct.EnemyType> pinpoint_enemy_type;
  Dictionary<int, PinpointProperty> pinpoint_enemy_property;

  DynamicSpawner pinpoint_spawner;
  // Use this for initialization
  void Start () { 

    ai_util = GetComponent<AIUtility>();

    area_data_dynamic_spawner = new Dictionary<int, List<SpawnStruct.EnemyType>>();
    area_data_static_top_spawner = new Dictionary<int, List<SpawnStruct.EnemyType>>();
    area_data_static_bottom_spawner = new Dictionary<int, List<SpawnStruct.EnemyType>>();
    area_data_pinpoint_spawner = new Dictionary<Vector2, List<SpawnStruct.EnemyType>>();

    level_loader = GetComponent<LevelLoader>();
    level_loader.load(level_descriptor_dynamic_spawner_text);
    level_descriptor_dynamic_spawner = level_loader.ld;

    level_loader.load(level_descriptor_static_top_spawner_text);
    level_descriptor_static_top_spawner = level_loader.ld;

    level_loader.load(level_descriptor_static_bottom_spawner_text);
    level_descriptor_static_bottom_spawner = level_loader.ld;

    level_loader.load(level_descriptor_pinpoint_text, true);
    level_descriptor_pinpoint_spawner = level_loader.ld;

    area_data_dynamic_spawner = level_descriptor_dynamic_spawner.data[current_area];
    area_data_static_top_spawner = level_descriptor_static_top_spawner.data[current_area];
    area_data_static_bottom_spawner = level_descriptor_static_bottom_spawner.data[current_area];
    area_data_pinpoint_spawner = level_descriptor_pinpoint_spawner.static_data[current_area];

    pinpoint_x_position = new Dictionary<int, int>();
    pinpoint_enemy_type = new Dictionary<int, SpawnStruct.EnemyType>();
    pinpoint_enemy_property = new Dictionary<int, PinpointProperty>();

    if (area_data_pinpoint_spawner != null) {
      PostProcessPinpointSpawnerData();
    }

    latched_segment = 0;

    sprite_renderers = GetComponentsInChildren<SpriteRenderer>();
    foreach(SpriteRenderer sprite_renderer in sprite_renderers) {
      sprite_renderer.enabled = false;
    }
    sprite_has_been_setup = false;
    one_shot_setup = false;
	  map_deja_vu = false;

    foreach(DynamicSpawner ds in GameObject.FindObjectsOfType<DynamicSpawner>()) {
      if (ds.spawner_type == DynamicSpawner.SpawnerType.pinpoint) {
        pinpoint_spawner = ds;
        break;
      }
    }

    float delayTime = Random.Range(minDelay, maxDelay);
    my_audio_source = GetComponent<AudioSource>();
    my_audio_source.PlayOneShot(intro_clip);
    my_audio_source.PlayDelayed(6.8f);
    //if(GetComponent<AudioSource>() != null)
    //{
    //	
    //	GetComponent<AudioSource>().PlayDelayed(delayTime);
    //}
  }
	
	// Update is called once per frame
	void Update () {
    if (ai_util.is_paused()) { return; }
    ScrollMap();
	}

  void ScrollMap() {
    transform.Translate(0, -1 * scroll_speed, 0);
    LoadEnemyBank();
    LoadStaticEnemyBank();
    UpdateArea();
  }

  void UpdateArea() {
    int normalized_position = (int)Mathf.Abs(transform.position.y);
    int diff_next = normalized_position % 102;

    if ((diff_next <= 1 || diff_next >= 81) && !one_shot_setup) {
      SetupMap();
      sprite_has_been_setup = true;
      one_shot_setup = true;

	  	if (map_deja_vu) {
				loop_count++;
				map_deja_vu = false;
	  	}
    } else if (diff_next == 80 && sprite_has_been_setup) {
      current_area++;
      LoadStaticEnemyBank();
      if (current_area == 17) {
        current_area = 7;
				map_deja_vu = true;
      }

      sprite_has_been_setup = false;
      //SetupMap();
      print("next area: " + current_area);
    }

    if (diff_next == 40) {
      //current_area++;

      one_shot_setup = false;
    }
  }

  void SetupMap() {
    float y_pad = 42f + (current_area > 1 ? 20.5f : 0f);
    sprite_renderers[current_area % 2].enabled = true;
    sprite_renderers[current_area % 2].sprite = map_sprites[current_area];
    sprite_renderers[current_area % 2].transform.localScale = new Vector3(sprite_scaling_factor, sprite_scaling_factor, 1);
    sprite_renderers[current_area % 2].transform.position = new Vector3(0, y_pad, 10);

    level_descriptor_dynamic_spawner.data.TryGetValue(current_area, out area_data_dynamic_spawner);
    level_descriptor_static_top_spawner.data.TryGetValue(current_area, out area_data_static_top_spawner);
    level_descriptor_static_bottom_spawner.data.TryGetValue(current_area, out area_data_static_bottom_spawner);
    level_descriptor_pinpoint_spawner.static_data.TryGetValue(current_area, out area_data_pinpoint_spawner);

    pinpoint_x_position = new Dictionary<int, int>();
    pinpoint_enemy_type = new Dictionary<int, SpawnStruct.EnemyType>();
    pinpoint_enemy_property = new Dictionary<int, PinpointProperty>();

    if (area_data_pinpoint_spawner != null) {
      PostProcessPinpointSpawnerData();
    } 
			
    //area_data_dynamic_spawner = level_descriptor_dynamic_spawner.data[current_area]
    //area_data_static_top_spawner = level_descriptor_static_top_spawner.data[current_area];
    //area_data_static_bottom_spawner = level_descriptor_static_bottom_spawner.data[current_area];
  }

  void PostProcessPinpointSpawnerData() {
    foreach (KeyValuePair<Vector2, List<SpawnStruct.EnemyType>> pinpoint in area_data_pinpoint_spawner) {
      Vector2 vector_position = pinpoint.Key;
      int pos_x = Mathf.RoundToInt(vector_position.x * 100);
      int pos_y = Mathf.RoundToInt(vector_position.y * 100);
      SpawnStruct.EnemyType enemy_type = pinpoint.Value[0];

      if (pinpoint.Value.Count > 1) {
        PinpointProperty pinpoint_property = PinpointProperty.clear;
        SpawnStruct.EnemyType pinpoint_str = pinpoint.Value[1];

        switch (pinpoint_str) {
          case SpawnStruct.EnemyType.randomize_x: pinpoint_property = PinpointProperty.randomize_x; break;
          case SpawnStruct.EnemyType.from_below: pinpoint_property = PinpointProperty.from_below; break;
        }

        pinpoint_enemy_property.Add(pos_y, pinpoint_property);
      }

      pinpoint_x_position.Add(pos_y, pos_x);
      pinpoint_enemy_type.Add(pos_y, enemy_type);
    }
  }

  void LoadStaticEnemyBank() {
    int b = Mathf.RoundToInt(GetNormalizedMapPosition(2));

    //print("area = " + current_area + " / " + b);
    switch (current_area) {
      case 1: break;
      case 2: b += 8200; break;
			default:
				b += 8200;
				b += ((current_area - 2) * 10200);
        break;
    }
    //print("b = " + b);

    if (pinpoint_x_position.ContainsKey(b)) {
      SpawnStruct.EnemyType enemy_type;
      PinpointProperty pinpoint_property = PinpointProperty.clear;
      int x_pos;

      pinpoint_enemy_type.TryGetValue(b, out enemy_type);
      pinpoint_x_position.TryGetValue(b, out x_pos);
      pinpoint_enemy_property.TryGetValue(b, out pinpoint_property);

      pinpoint_spawner.PlaceEnemy(enemy_type, (float)x_pos / 100, pinpoint_property);
    }
  }

  void LoadEnemyBank() {
    int segment = GetNormalizedMapPosition();

    if (segment != latched_segment) {
      allowed_enemy_types_in_dynamic_spawner = new List<SpawnStruct.EnemyType>();
      allowed_enemy_types_in_static_top_spawner = new List<SpawnStruct.EnemyType>();
      List<SpawnStruct.EnemyType> es;
      print(current_area + ": " + segment);

      if (area_data_dynamic_spawner != null && area_data_dynamic_spawner.TryGetValue(segment, out es)) {
        allowed_enemy_types_in_dynamic_spawner = es;
        SetDynamicSpawner(DynamicSpawner.SpawnerType.dynamic);
      }

      if (area_data_static_top_spawner != null && area_data_static_top_spawner.TryGetValue(segment, out es)) {
        allowed_enemy_types_in_static_top_spawner = es;
        SetDynamicSpawner(DynamicSpawner.SpawnerType.static_top);
      }

      if (area_data_static_bottom_spawner != null && area_data_static_bottom_spawner.TryGetValue(segment, out es)) {
        allowed_enemy_types_in_static_bottom_spawner = es;
        SetDynamicSpawner(DynamicSpawner.SpawnerType.static_bottom);
      }

      latched_segment = segment;
    }
  }

  void SetDynamicSpawner(DynamicSpawner.SpawnerType spawner_type) {
    DynamicSpawner[] dss = GameObject.FindObjectsOfType<DynamicSpawner>();
    foreach(DynamicSpawner ds in dss) {
      if (ds.spawner_type == spawner_type) {
        switch (spawner_type) {
          case DynamicSpawner.SpawnerType.dynamic:
            ds.allowed_to_spawn = allowed_enemy_types_in_dynamic_spawner;
            break;
          case DynamicSpawner.SpawnerType.static_top:
            ds.allowed_to_spawn = allowed_enemy_types_in_static_top_spawner;
            break;
          case DynamicSpawner.SpawnerType.static_bottom:
            ds.allowed_to_spawn = allowed_enemy_types_in_static_bottom_spawner;
            break;
        }
        
      }
    }
  }

  int GetNormalizedMapPosition() {
    if (current_area == 1) {
      return (int)(transform.position.y * -1.0f);
    } else {
      return (int) (-transform.position.y - 82) % 102;
    }
  }

  float GetNormalizedMapPosition(int acc) {
    if (current_area == 1) {
      return transform.position.y * Mathf.Pow(10, acc) * -1;
    } else {
	  	return ((transform.position.y - (10 * 102 * loop_count) + 82) % 102) * Mathf.Pow(10, acc) * -1 ;
    }
  }
}
