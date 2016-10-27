using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelDescriptor {
  public Dictionary<int, Dictionary<int, List<SpawnStruct.EnemyType>>> data;
  public Dictionary<int, Dictionary<Vector2, List<SpawnStruct.EnemyType>>> static_data;

  public LevelDescriptor() {
    data = new Dictionary<int, Dictionary<int, List<SpawnStruct.EnemyType>>>();
    static_data = new Dictionary<int, Dictionary<Vector2, List<SpawnStruct.EnemyType>>>();
  }

  public void Insert(int area_id, Dictionary<int, List<SpawnStruct.EnemyType>> area_data) {
    data.Add(area_id, area_data);
  }

  public void Insert(int area_id, Dictionary<Vector2, List<SpawnStruct.EnemyType>> area_data) {
    static_data.Add(area_id, area_data);
  }
}
