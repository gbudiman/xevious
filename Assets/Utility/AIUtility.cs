using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIUtility : MonoBehaviour {
  GameObject[] players;
  Vector3 bounds_top_right;
  Vector3 bounds_bottom_left;
  public LifeController.Owner owner;
  MenuController menu_controller;

  bool bounds_have_been_calculated = false;
  public GameObject projectile_prefab;
  float movement_boundary = 0.5f;

  void Start() {
    menu_controller = GameObject.FindObjectOfType<MenuController>();
    players = GameObject.FindGameObjectsWithTag("Player");
    PrecalculateBounds();
  }

  public bool is_paused() {
    if (menu_controller == null) { return false; }
    return menu_controller.game_is_paused;
  }

  public Vector3 GetLeftCorner() {
    PrecalculateBounds();
    return bounds_bottom_left;
  }

  public Vector3 GetRightCorner() {
    PrecalculateBounds();
    return new Vector3(bounds_top_right.x, bounds_bottom_left.y, 0);
  }

  public GameObject PickRandomPlayer() {
    if (players.Length == 0) { return null; }
    int random = Random.value > 0.5 ? 1 : 0;
    if (players.Length == 1 && random > 0) {
      random = 0;
    }


    return players[random];
  }

  /// <summary>
  /// Get player position
  /// </summary>
  /// <param name="_jitter">Randomization which is calculated independently for each X and Y position</param>
  /// <returns>Vector3 or player's actual position</returns>
	public Vector3 GetPlayerPosition(float _jitter = 0.0f) {
    float jitter_x = Random.Range(-_jitter, _jitter);
    float jitter_y = Random.Range(-_jitter, _jitter);

    GameObject player = PickRandomPlayer();
    if (player == null) { return new Vector3(0, 0, 0);  }
    return new Vector3(player.transform.position.x + jitter_x, player.transform.position.y + jitter_y, player.transform.position.z);
  }

  /// <summary>
  /// Shortcut to get vector to aim at player
  /// </summary>
  /// <param name="origin">The position of the hooster</param>
  /// <returns>Vector3 towards player</returns>
  public Vector3 GetVectorToPlayer(Vector3 origin) {
    Vector3 player_position = GetPlayerPosition();
    //return GetPlayerPosition() - origin;
    return new Vector3(player_position.x, player_position.y, 0) - new Vector3(origin.x, origin.y, 0);
  }

  public Vector3 GetToShootingRangeInFrontOfPlayer(float y_distance) {
    GameObject player = PickRandomPlayer();
    if (player == null) { return new Vector3(0, 0, 0); }
    return new Vector3(player.transform.position.x, player.transform.position.y + y_distance, player.transform.position.z);
  }

  /// <summary>
  /// Check whether a given Vector3 is within viewport
  /// </summary>
  /// <param name="pos">The Vector3 input to check</param>
  /// <param name="padding">Give negative value to shrink boundary inside, positive to dilate boundary outside</param>
  /// <returns>Boolean whether a given Vector3 is within viewport</returns>
  public bool IsOutsideViewport(Vector3 pos, float padding = 0.0f) {
    float x = pos.x;
    float y = pos.y;

    return !((bounds_bottom_left.x - padding) < x && x < (bounds_top_right.x + padding) &&
      (bounds_bottom_left.y - padding) < y && y < (bounds_top_right.y + padding));
  }

  public bool HitLeftBoundary(Vector3 position) {
    return position.x < bounds_bottom_left.x + movement_boundary;
  }

  public bool HitRightBoundary(Vector3 position) {
    return position.x > bounds_top_right.x - movement_boundary;
  }

  public bool HitTopBoundary(Vector3 position) {
    return position.y > bounds_top_right.y - movement_boundary;
  }

  public bool HitBottomBoundary(Vector3 position) {
    return position.y < bounds_bottom_left.y + movement_boundary;
  }

  void PrecalculateBounds() {
    if (bounds_have_been_calculated) { return; }
    Camera camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    bounds_bottom_left = camera.ViewportToWorldPoint(new Vector3(0, 0, camera.nearClipPlane));
    bounds_top_right = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));
    bounds_have_been_calculated = true;
  }

  /// <summary>
  /// Syntactic sugar for distance between A and B within d 
  /// </summary>
  /// <param name="a"></param>
  /// <param name="b"></param>
  /// <param name="distance"></param>
  /// <returns>True if A and B is within d distance apart, else otherwise</returns>
  public bool WithinDistance(Vector3 a, Vector3 b, float distance) {
    return Vector3.Distance(a, b) < distance;
  }

  /// <summary>
  /// Pick a random point inside viewport but outside specified position beyond safe_zone
  /// </summary>
  /// <param name="position">The position of safe_zone</param>
  /// <param name="safe_zone">The radius of safe_zone</param>
  /// <returns></returns>
  public Vector3 PickRandomPointOutside(Vector3 position, float safe_zone) {
    Vector3 picked_point = new Vector3(0,0,0);
    float distance = 0;
    while (distance < safe_zone) {
      picked_point = PickRandomPoint();
      distance = Vector3.Distance(position, picked_point);
    }

    return picked_point;
  }

  /// <summary>
  /// Pick a random point inside viewport
  /// </summary>
  /// <returns>A Vector3 of randomly selected point</returns>
  public Vector3 PickRandomPoint() {
    float x = Random.Range(bounds_bottom_left.x, bounds_top_right.x);
    float y = Random.Range(bounds_bottom_left.y, bounds_top_right.y);

    return new Vector3(x, y, 0);
  }

  /// <summary>
  /// This is player's Gatling gun. Call to instantiate GatlingBullet projectile
  /// </summary>
  /// <param name="launcher_position"></param>
  /// <param name="speed"></param>
  public void LaunchPlayerProjectile(Vector3 launcher_position, float speed) {
    GameObject projectile = Instantiate(projectile_prefab, launcher_position, Quaternion.identity) as GameObject;
    MassDriver mass_driver = projectile.GetComponent<MassDriver>();

    mass_driver.owner = owner;
    mass_driver.SetProjectileVector(new Vector3(0, speed, 0));
  }

	public void LaunchPlayerBomb(Vector3 launcher_position, float speed, float lifetime) {
		GameObject projectile = Instantiate (projectile_prefab, launcher_position, Quaternion.identity) as GameObject;
		MassDriver mass_driver = projectile.GetComponent<MassDriver> ();
    mass_driver.owner = owner;
		mass_driver.SetProjectileVector(new Vector3(0, speed, 0));
		mass_driver.SetExplosionLocation(launcher_position + new Vector3(0,5,0));

	}

  /// <summary>
  /// Generic method to launch projectile without overriding their speed
  /// </summary>
  /// <param name="launcher_position">Starting point of the projectile</param>
  /// <param name="shoot_straight">Toggle to shoot straight instead of at players</param>
  public void LaunchProjectile(Vector3 launcher_position, bool shoot_straight = false) {
    if (PlayerIsStillInPlay()) {
      GameObject projectile = Instantiate(projectile_prefab, fixate_z_vector(launcher_position), Quaternion.identity) as GameObject;
      projectile.GetComponent<SparioAI>().shoot_straight = shoot_straight;
    }
  }

  /// <summary>
  /// Check whether player is still in play
  /// </summary>
  /// <returns>You don't know what this method returns???</returns>
  public bool PlayerIsStillInPlay() {
    return GameObject.FindObjectsOfType<PlayerController>().Length > 0;
  }

  public void LaunchProjectileArc(Vector3 launcher_position) {
    //Vector3 player_position = GetPlayerPosition();
    //Vector3 projectile_vector = player_position - launcher_position;
    //float magnitude_scaler = speed / Vector3.Magnitude(projectile_vector);
    //projectile_vector *= magnitude_scaler;
    Vector3 projectile_vector = GetVectorToPlayer(launcher_position).normalized;

    for (int angle = 30; angle >= -30; angle -= 15) {
      Quaternion rotation = Quaternion.Euler(0, 0, angle);
      GameObject projectile = Instantiate(projectile_prefab, fixate_z_vector(launcher_position), Quaternion.identity) as GameObject;
      projectile.GetComponent<SparioAI>().SetMovementVector(rotation * projectile_vector);
      //projectile.GetComponent<MassDriver>().SetProjectileVector(rotation * projectile_vector);
    }
  }

  public Vector3 fixate_z_vector(Vector3 input) {
    return new Vector3(input.x, input.y, -9f);
  }

  public void LaunchProjectileFullCircle(Vector3 launcher_position) {
    float sq = 1 / Mathf.Sqrt(2);

    List<Vector3> vectors = new List<Vector3>();
    vectors.Add(new Vector3(0, 1, 0));
    vectors.Add(new Vector3(sq, sq, 0));
    vectors.Add(new Vector3(1, 0, 0));
    vectors.Add(new Vector3(sq, -sq, 0));
    vectors.Add(new Vector3(0, -1, 0));
    vectors.Add(new Vector3(-sq, -sq, 0));
    vectors.Add(new Vector3(-1, 0, 0));
    vectors.Add(new Vector3(-sq, sq, 0));

    foreach(Vector3 _v in vectors) {
      GameObject projectile = Instantiate(projectile_prefab, fixate_z_vector(launcher_position), Quaternion.identity) as GameObject;
      //projectile.GetComponent<MassDriver>().SetProjectileVector(v);
      projectile.GetComponent<SparioAI>().SetMovementVector(_v);

    }
  }

  /// <summary>
  /// This generates on-land reticule for bomb to trigger
  /// </summary>
  /// <param name="prefab">Prefab for on-land reticule</param>
  /// <param name="position">Vector3 of reticule position</param>
	public void CreateOnLandReticule(GameObject prefab, Vector3 position, float lifetime) {
		GameObject reticule = Instantiate(prefab, position, Quaternion.identity) as GameObject;
		Destroy (reticule, lifetime);
  }

  /// <summary>
  /// Check whether OnLandReticule is within distance. Used in avoidance AI.
  /// </summary>
  /// <param name="position">Caller's position</param>
  /// <param name="distance">Distance between caller's and OnLandReticule</param>
  /// <returns>Boolean whether caller's is within explosive distance</returns>
  public bool IsBeingTargeted(Vector3 position, float distance) {
    GameObject[] on_land_reticules = GameObject.FindGameObjectsWithTag("on_land_reticule");

    foreach(GameObject on_land_reticule in on_land_reticules) {
      Vector3 ret_pos = on_land_reticule.transform.position;
      float dist = Vector3.Distance(position, ret_pos);

      if (dist < distance) { return true;  }
    }

    return false;
  }

  /// <summary>
  /// Check when object A and B are in firing line within given tolerance
  /// </summary>
  /// <param name="a"></param>
  /// <param name="b"></param>
  /// <param name="tolerance"></param>
  /// <returns>True when firing line exists</returns>
  public bool OnFiringLine(Vector3 a, Vector3 b, float tolerance) {
    return Mathf.Abs(a.x - b.x) < tolerance;
  }

  /// <summary>
  /// Get viewport width
  /// </summary>
  /// <param name="padding">Extra padding to subtract from width</param>
  /// <returns></returns>
  public float GetCameraWidth(float padding = 0.0f) {
    return bounds_top_right.x - bounds_bottom_left.x - padding;
  }
}
