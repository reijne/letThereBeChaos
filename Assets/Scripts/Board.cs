using System;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {
  [SerializeField] int width;
  [SerializeField] int height;
  [SerializeField] int boardZ;
  [SerializeField] float frequency;
  [SerializeField] GameObject tile_prefab;
  [SerializeField] GameObject plant_prefab;
  [SerializeField] Controls controls;
  List<(Vector2, Color)> plantInitPositions;
  public List<Tile> tiles = new List<Tile>();
  Dictionary<Vector2, Plant> plants = new Dictionary<Vector2, Plant>();
  float nextTick;

  void Start() {
    plantInitPositions = new List<(Vector2, Color)>() {
      (new Vector2(6,7), Color.blue),
      (new Vector2(1,1), Color.cyan),
      (new Vector2(9,6), Color.magenta),
    };
    nextTick = Time.time + 1 / frequency;
    Tile.SIZE = 0.5;
    Pattern.SIZE = 3;
    spawnTiles();
    spawnPlants();
  }

  void spawnTiles() {
    for (int y = 0; y < height; y++) {
      for (int x = 0; x < width; x++) {
        spawnTile(x, y);
      }
    }
  }

  void spawnPlants() {
    foreach ((Vector2, Color) tuplant in plantInitPositions) {
      spawnPlant(tuplant.Item1, tuplant.Item2);
    }
  }

  void spawnPlant(Vector2 plantPos, Color color) {
    if (outOfBounds(plantPos)) return;
    Vector3 spawnPoint = new Vector3(plantPos.x * Plant.SIZE, plantPos.y * Plant.SIZE, 0);
    GameObject plantObject = Instantiate(plant_prefab, spawnPoint, Quaternion.identity);
    Plant plant = plantObject.GetComponent<Plant>();
    plants[plantPos] = plant;
    plant.x = (int)plantPos.x;
    plant.y = (int)plantPos.y;
    plant.setColor(color);
  }

  bool outOfBounds(Vector2 pos) {
    return pos.x < 0 || pos.x >= width || pos.y < 0 || pos.y >= height;
  }

  void spawnTile(int x, int y) {
    Vector3 spawnPoint = new Vector3(x * Tile.SIZE, y * Tile.SIZE, boardZ);
    GameObject tileObject = Instantiate(tile_prefab, spawnPoint, Quaternion.identity);
    tileObject.transform.SetParent(transform);
    Tile tile = tileObject.GetComponent<Tile>();
    tiles.Add(tile);
  }

  void FixedUpdate() {
    if (Time.time >= nextTick) {
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
    if (!controls.patterns.ContainsKey(plant.color)) return;
    Pattern pattern = controls.patterns[plant.color];
    foreach (KeyValuePair<(int, int), bool> select in pattern.selectedTiles) {
      Vector2 offset = new Vector2(select.Key.Item1 - (Pattern.SIZE - 1) / 2, select.Key.Item2 - (Pattern.SIZE - 1) / 2);
      if (select.Value) grow(pos + offset, plant);
    }
  }

  void grow(Vector2 pos, Plant plant) {
    if (plants.ContainsKey(pos)) return;
    spawnPlant(pos, plant.color);
  }
}
