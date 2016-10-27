using UnityEngine;
using System.Collections;

public class SpawnInstantiator : MonoBehaviour {

	public static void Spawn(GameObject _prefab, Vector3 _location) {
    Instantiate(_prefab, _location, Quaternion.identity);
  }
}
