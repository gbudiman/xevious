using UnityEngine;
using System.Collections;

public class ExplosionMaker : MonoBehaviour {
  public GameObject explosion_prefab;
  public LifeController.Owner owner;

  public void MakeBoom(Vector3 location) {
    GameObject explosion = Instantiate(explosion_prefab, location, Quaternion.identity) as GameObject;
    explosion.GetComponent<ExplosionPhysics>().owner = owner;
  }
}
