using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;
using System.Text.RegularExpressions;

public class LevelLoader : MonoBehaviour {
  //public TextAsset level_descriptor;
  public LevelDescriptor ld;
  public void load(TextAsset level_descriptor, bool is_pinpoint = false) {
    ld = new LevelDescriptor();

    string area_pattern = @"area_(\d+)";
    string descriptor_pattern = @"(\d+)\: (\w+)";
    Regex r = new Regex(area_pattern);
    Regex s = new Regex(descriptor_pattern);
    string[] lines = level_descriptor.text.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

    int latched_area_id = 1;
    Dictionary<int, List<SpawnStruct.EnemyType>> area_data = new Dictionary<int, List<SpawnStruct.EnemyType>>();
    Dictionary<Vector2, List<SpawnStruct.EnemyType>> pinpoint_data = new Dictionary<Vector2, List<SpawnStruct.EnemyType>>();
    foreach (string line in lines) {
      int new_area_id = 1;

      if (line[0] == '#') { continue;  }
      Match m = r.Match(line);
      Match d = s.Match(line);

      if (m.Success) {
        new_area_id = HolyFuckRegexOnCSharpIsSoComplicatedIJustWantToMatchAreaName(m);

        if (new_area_id != latched_area_id) {
          if (is_pinpoint) {
            ld.Insert(latched_area_id, pinpoint_data);
          } else {
            ld.Insert(latched_area_id, area_data);
          }
          
          area_data = new Dictionary<int, List<SpawnStruct.EnemyType>>();
          pinpoint_data = new Dictionary<Vector2, List<SpawnStruct.EnemyType>>();
          latched_area_id = new_area_id;
        }
      } else if (d.Success || is_pinpoint) {
        string[] enemies;
        int segment_indicator = -1;
        Vector2 position_indicator = new Vector2(0, 0);

        if (is_pinpoint) {
          string[] split = line.Split(':');
          string[] vector_positions = split[0].Split(',');
          float pos_x = float.Parse(vector_positions[0]);
          float pos_y = float.Parse(vector_positions[1]);

          position_indicator = new Vector2(pos_x, pos_y);
          enemies = split[1].Split(',');
        } else {
          string[] split = line.Split(':');
          segment_indicator = Int32.Parse(split[0]);
          enemies = split[1].Split(',');
        }

        List<SpawnStruct.EnemyType> enemy_types = new List<SpawnStruct.EnemyType>();

        foreach (string _enemy in enemies) {
          string enemy = _enemy.Trim();
          switch(enemy) {
            case "toroid": enemy_types.Add(SpawnStruct.EnemyType.toroid); break;
            case "torkan": enemy_types.Add(SpawnStruct.EnemyType.torkan); break;
            case "barra": enemy_types.Add(SpawnStruct.EnemyType.barra); break;
            case "derota": enemy_types.Add(SpawnStruct.EnemyType.derota); break;
            case "domogram": enemy_types.Add(SpawnStruct.EnemyType.domogram); break;
            case "logram": enemy_types.Add(SpawnStruct.EnemyType.logram); break;
            case "boza_logram": enemy_types.Add(SpawnStruct.EnemyType.boza_logram); break;
            case "bacura": enemy_types.Add(SpawnStruct.EnemyType.bacura); break;
            case "brag_zakato": enemy_types.Add(SpawnStruct.EnemyType.brag_zakato); break;
            case "garu_zakato": enemy_types.Add(SpawnStruct.EnemyType.garu_zakato); break;
            case "jara": enemy_types.Add(SpawnStruct.EnemyType.jara); break;
            case "kapi": enemy_types.Add(SpawnStruct.EnemyType.kapi); break;
            case "zakato": enemy_types.Add(SpawnStruct.EnemyType.zakato); break;
            case "zoshi": enemy_types.Add(SpawnStruct.EnemyType.zoshi); break;
            case "andor_genesis": enemy_types.Add(SpawnStruct.EnemyType.andor_genesis); break;
            case "garu_barra": enemy_types.Add(SpawnStruct.EnemyType.garu_barra); break;
            case "garu_derota": enemy_types.Add(SpawnStruct.EnemyType.garu_derota); break;
            case "grobda": enemy_types.Add(SpawnStruct.EnemyType.grobda); break;
            case "sol": enemy_types.Add(SpawnStruct.EnemyType.sol); break;
            case "zolbak": enemy_types.Add(SpawnStruct.EnemyType.zolbak); break;
            case "secret_life_up": enemy_types.Add(SpawnStruct.EnemyType.secret_life_up); break;

            case "clear": enemy_types.Add(SpawnStruct.EnemyType.clear); break;
            case "randomize_x": enemy_types.Add(SpawnStruct.EnemyType.randomize_x); break;
            case "from_below": enemy_types.Add(SpawnStruct.EnemyType.from_below); break;
            default:
              print("WARNING!!! Unknown enemy type " + enemy);
              break;
          }
        }

        if (is_pinpoint) {
          pinpoint_data.Add(position_indicator, enemy_types);
        } else {
          area_data.Add(segment_indicator, enemy_types);
        }
      }
      
    }

    if (is_pinpoint) {
      ld.Insert(latched_area_id, pinpoint_data);
    } else {
      ld.Insert(latched_area_id, area_data);
    }
  }

  int HolyFuckRegexOnCSharpIsSoComplicatedIJustWantToMatchAreaName(Match m) {
    List<string> result = new List<string>();

    while (m.Success) {
      int group_count = m.Groups.Count;

      // Start from index 1, because index 0 is the actual matched string (OMFGLOLWTFBBQWUT???)
      for (int i = 1; i < group_count; i++) {
        CaptureCollection cc = m.Groups[i].Captures;

        for (int j = 0; j < cc.Count; j++) {
          Capture c = cc[j];
          result.Add(c.ToString());
        }
      }
      m = m.NextMatch();
    }

    return Int32.Parse(result[0]);
  }
}
