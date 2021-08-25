using System;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {
  [SerializeField] public int width;
  [SerializeField] public int height;
  [SerializeField] int boardZ;
  [SerializeField] float frequency;
  [SerializeField] GameObject tile_prefab;
  [SerializeField] GameObject plant_prefab;
  [SerializeField] Controls controls;
  [SerializeField] Interfaze interfaze;
  List<(Vector2, int)> plantInitPositions;
  public List<Tile> tiles = new List<Tile>();
  Dictionary<Vector2, Plant> plants = new Dictionary<Vector2, Plant>();
  float nextTick;
  bool started = false;

  void Start() {
    plantInitPositions = new List<(Vector2, int)>() {
      (new Vector2(6,7), 1),
      (new Vector2(1,1), 2),
      (new Vector2(34,22), 3),
      (new Vector2(9,6), 4),
      (new Vector2(29,49), 0),
    };
    nextTick = Time.time + 1 / frequency;
    Tile.SIZE = 1;
    Pattern.SIZE = 3;
    spawnTiles();
    spawnPlants();
  }

  public void startGame() {
    started = true;
  }

  void spawnTiles() {
    for (int y = 0; y < height; y++) {
      for (int x = 0; x < width; x++) {
        spawnTile(x, y);
      }
    }
  }

  void spawnPlants() {
    foreach ((Vector2, int) tuplant in plantInitPositions) {
      spawnPlant(tuplant.Item1, Pattern.pallete[tuplant.Item2]);
    }
  }

  public void spawnPlant(Vector2 plantPos, Color color) {
    if (outOfBounds(plantPos)) return;
    Vector3 spawnPoint = new Vector3(plantPos.x * Plant.SIZE, plantPos.y * Plant.SIZE, 0);
    GameObject plantObject = Instantiate(plant_prefab, spawnPoint, Quaternion.identity);
    Plant plant = plantObject.GetComponent<Plant>();
    plants[plantPos] = plant;
    plant.board = this;
    plant.pos = plantPos;
    plant.setColor(color);
    interfaze.addColorCount(color);
  }

  bool outOfBounds(Vector2 pos) {
    return pos.x < 0 || pos.x >= width || pos.y < 0 || pos.y >= height;
  }

  void spawnTile(int x, int y) {
    Vector3 spawnPoint = new Vector3(x * Tile.SIZE, y * Tile.SIZE, boardZ);
    GameObject tileObject = Instantiate(tile_prefab, spawnPoint, Quaternion.identity);
    tileObject.transform.SetParent(transform);
    Tile tile = tileObject.GetComponent<Tile>();
    tile.board = this;
    tile.pos = new Vector2(x, y);
    tiles.Add(tile);
  }

  void FixedUpdate() {
    if (Time.time >= nextTick && started) {
      tick();
      nextTick = Time.time + 1 / frequency;
    }
  }

  void tick() {
    List<Vector2> localPositions = new List<Vector2>();
    foreach (Vector2 plantPos in plants.Keys) {
      localPositions.Add(plantPos);
    }

    foreach (Vector2 plantPos in localPositions) {
      checkGrowth(plantPos, plants[plantPos]);
    }
  }

  void checkGrowth(Vector2 pos, Plant plant) {
    foreach (Vector2 relPos in Controls.patterns[plant.color]) {
      grow(pos + relPos, plant);
    }
    // if (!controls.patterns.ContainsKey(plant.color)) return;
    // Dictionary<(int, int), bool> pattern = controls.patterns[plant.color];
    // foreach (KeyValuePair<(int, int), bool> select in pattern) {
    //   Vector2 offset = new Vector2(select.Key.Item1 - (Pattern.SIZE - 1) / 2, select.Key.Item2 - (Pattern.SIZE - 1) / 2);
    //   if (select.Value) grow(pos + offset, plant);
    // }
  }

  void grow(Vector2 pos, Plant plant) {
    if (plants.ContainsKey(pos)) return;
    spawnPlant(pos, plant.color);
  }
}
