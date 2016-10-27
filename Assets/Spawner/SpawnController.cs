using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnController : MonoBehaviour {
  AIUtility ai_util;
  List<SpawnStruct> spawns = new List<SpawnStruct>();

  public bool enable_toroid;
  public GameObject prefab_toroid;
  public float toroid_spawn_interval_min = 5.0f;
  public float toroid_spawn_interval_max = 20.0f;
  public int toroid_spawn_amount;

  public bool enable_torkan;
  public GameObject prefab_torkan;
  public float torkan_spawn_interval_min;
  public float torkan_spawn_interval_max;
  public int torkan_spawn_amount;

  public bool enable_zoshi;
  public GameObject prefab_zoshi;
  public float zoshi_spawn_interval_min;
  public float zoshi_spawn_interval_max;
  public int zoshi_spawn_amount;

  public bool enable_grobda;
  public GameObject prefab_grobda;
  public float grobda_spawn_interval_min;
  public float grobda_spawn_interval_max;
  public int grobda_spawn_amount;

  public bool enable_domogram;
  public GameObject prefab_domogram;
  public float domogram_spawn_interval_min;
  public float domogram_spawn_interval_max;
  public int domogram_spawn_amount;

  public bool enable_kapi;
  public GameObject prefab_kapi;
  public float kapi_spawn_interval_min;
  public float kapi_spawn_interval_max;
  public int kapi_spawn_amount;

  public bool enable_zakato;
  public GameObject prefab_zakato;
  public float zakato_spawn_interval_min;
  public float zakato_spawn_interval_max;
  public int zakato_spawn_amount;

  public bool enable_brag_zakato;
  public GameObject prefab_brag_zakato;
  public float brag_zakato_spawn_interval_min;
  public float brag_zakato_spawn_interval_max;
  public int brag_zakato_spawn_amount;

  public bool enable_garu_zakato;
  public GameObject prefab_garu_zakato;
  public float garu_zakato_spawn_interval_min;
  public float garu_zakato_spawn_interval_max;
  public int garu_zakato_spawn_amount;

  public bool enable_jara;
  public GameObject prefab_jara;
  public float jara_spawn_interval_min;
  public float jara_spawn_interval_max;
  public int jara_spawn_amount;

  public bool enable_bacura;
  public GameObject prefab_bacura;
  public float bacura_spawn_interval_min;
  public float bacura_spawn_interval_max;
  public int bacura_spawn_amount;

  public bool enable_andor_genesis;
  public GameObject prefab_andor_genesis;
  public float andor_genesis_spawn_interval_min;
  public float andor_genesis_spawn_interval_max;
  public int andor_genesis_spawn_amount;

  public bool enable_logram;
  public GameObject prefab_logram;
  public float logram_spawn_interval_min;
  public float logram_spawn_interval_max;
  public int logram_spawn_amount;

  public bool enable_derota;
  public GameObject prefab_derota;
  public float derota_spawn_interval_min;
  public float derota_spawn_interval_max;
  public int derota_spawn_amount;

  public bool enable_boza_logram;
  public GameObject prefab_boza_logram;
  public float boza_logram_spawn_interval_min;
  public float boza_logram_spawn_interval_max;
  public int boza_logram_spawn_amount;

  public bool enable_garu_derota;
  public GameObject prefab_garu_derota;
  public float garu_derota_spawn_interval_min;
  public float garu_derota_spawn_interval_max;
  public int garu_derota_spawn_amount;
  // Use this for initialization
  void Start () {
    ai_util = GetComponent<AIUtility>();

    if (enable_toroid) {
      spawns.Add(new SpawnStruct(SpawnStruct.EnemyType.toroid,
        prefab_toroid,
        transform,
        toroid_spawn_interval_min,
        toroid_spawn_interval_max,
        toroid_spawn_amount));
    }

    if (enable_torkan) {
      spawns.Add(new SpawnStruct(SpawnStruct.EnemyType.torkan,
        prefab_torkan,
        transform,
        torkan_spawn_interval_min,
        torkan_spawn_interval_max,
        torkan_spawn_amount));
    }

    if (enable_zoshi) {
      spawns.Add(new SpawnStruct(SpawnStruct.EnemyType.zoshi,
        prefab_zoshi,
        transform,
        zoshi_spawn_interval_min,
        zoshi_spawn_interval_max,
        zoshi_spawn_amount));
    }

    if (enable_grobda) {
      spawns.Add(new SpawnStruct(SpawnStruct.EnemyType.grobda,
        prefab_grobda,
        transform,
        grobda_spawn_interval_min,
        grobda_spawn_interval_max,
        grobda_spawn_amount));
    }

    if (enable_domogram) {
      spawns.Add(new SpawnStruct(SpawnStruct.EnemyType.domogram,
        prefab_domogram,
        transform,
        domogram_spawn_interval_min,
        domogram_spawn_interval_max,
        domogram_spawn_amount));
    }

    if (enable_kapi) {
      spawns.Add(new SpawnStruct(SpawnStruct.EnemyType.kapi,
        prefab_kapi,
        transform,
        kapi_spawn_interval_min,
        kapi_spawn_interval_max,
        kapi_spawn_amount));
    }

    if (enable_zakato) {
      spawns.Add(new SpawnStruct(SpawnStruct.EnemyType.zakato,
        prefab_zakato,
        transform,
        zakato_spawn_interval_min,
        zakato_spawn_interval_max,
        zakato_spawn_amount));
    }

    if (enable_brag_zakato) {
      spawns.Add(new SpawnStruct(SpawnStruct.EnemyType.brag_zakato,
        prefab_brag_zakato,
        transform,
        brag_zakato_spawn_interval_min,
        brag_zakato_spawn_interval_max,
        brag_zakato_spawn_amount));
    }

    if (enable_garu_zakato) {
      spawns.Add(new SpawnStruct(SpawnStruct.EnemyType.garu_zakato,
        prefab_garu_zakato,
        transform,
        garu_zakato_spawn_interval_min,
        garu_zakato_spawn_interval_max,
        garu_zakato_spawn_amount));
    }

    if (enable_jara) {
      spawns.Add(new SpawnStruct(SpawnStruct.EnemyType.jara,
        prefab_jara,
        transform,
        jara_spawn_interval_min,
        jara_spawn_interval_max,
        jara_spawn_amount,
        ai_util.GetCameraWidth(1.0f) / 2));
    }

    if (enable_bacura) {
      spawns.Add(new SpawnStruct(SpawnStruct.EnemyType.bacura,
        prefab_bacura,
        transform,
        bacura_spawn_interval_min,
        bacura_spawn_interval_max,
        bacura_spawn_amount,
        ai_util.GetCameraWidth(1.0f) / 2));
    }

    if (enable_andor_genesis) {
      spawns.Add(new SpawnStruct(SpawnStruct.EnemyType.andor_genesis,
        prefab_andor_genesis,
        transform,
        andor_genesis_spawn_interval_min,
        andor_genesis_spawn_interval_max,
        andor_genesis_spawn_amount));
    }

    if (enable_logram) {
      spawns.Add(new SpawnStruct(SpawnStruct.EnemyType.logram,
        prefab_logram,
        transform,
        logram_spawn_interval_min,
        logram_spawn_interval_max,
        logram_spawn_amount,
        ai_util.GetCameraWidth(1.0f) / 2));
    }

    if (enable_derota) {
      spawns.Add(new SpawnStruct(SpawnStruct.EnemyType.derota,
        prefab_derota,
        transform,
        derota_spawn_interval_min,
        derota_spawn_interval_max,
        derota_spawn_amount,
        ai_util.GetCameraWidth(1.0f) / 2));
    }

    if (enable_boza_logram) {
      spawns.Add(new SpawnStruct(SpawnStruct.EnemyType.boza_logram,
        prefab_boza_logram,
        transform,
        boza_logram_spawn_interval_min,
        boza_logram_spawn_interval_max,
        boza_logram_spawn_amount,
        ai_util.GetCameraWidth(1.0f) / 2));
    }

    if (enable_garu_derota) {
      spawns.Add(new SpawnStruct(SpawnStruct.EnemyType.garu_derota,
        prefab_garu_derota,
        transform,
        garu_derota_spawn_interval_min,
        garu_derota_spawn_interval_max,
        garu_derota_spawn_amount,
        ai_util.GetCameraWidth(1.0f) / 2));
    }
  }
	
	// Update is called once per frame
	void Update () {
    float delta = Time.deltaTime;

    foreach(SpawnStruct spawn in spawns) {
      spawn.Cycle(delta);
    }
	}

  
}
