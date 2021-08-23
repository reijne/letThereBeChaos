using System;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
  [SerializeField] int width;
  [SerializeField] int height;
  [SerializeField] int boardZ;
  [SerializeField] float frequency;
  [SerializeField] GameObject tile_prefab;
  [SerializeField] GameObject plant_prefab;
  [SerializeField] int globalCycleTime;
  public List<Vector2> plantInitPositions;
  public List<Tile> tiles = new List<Tile>();
  Dictionary<Vector2, Plant> plants = new Dictionary<Vector2, Plant>();
  float nextTick;
  
  void Start() {
    plantInitPositions = new List<Vector2>() {
      new Vector2(3,3),
      new Vector2(1,1)
    };
    nextTick = Time.time + 1/frequency;
    Tile.SIZE = 1;
    spawnPlants();
    spawnTiles();
  }

  void spawnTiles() {
    for (int y = 0; y < height; y++) {
      for (int x = 0; x < width; x++) {
        spawnTile(x, y);
      }
    }
  }

  void spawnPlants() {
    foreach (Vector2 plantPos in plantInitPositions) {
      Debug.Log(String.Format("inity {0}",plantPos));
      spawnPlant(plantPos);
    }
  }

  void spawnPlant(Vector2 plantPos) {
    if (outOfBounds(plantPos)) return;
    Debug.Log(String.Format("Spawning at {0}",plantPos));
    Vector3 spawnPoint = new Vector3(plantPos.x*Plant.SIZE, plantPos.y*Plant.SIZE, 0);
    GameObject plantObject = Instantiate(plant_prefab, spawnPoint, Quaternion.identity);
    Plant plant = plantObject.GetComponent<Plant>();
    plant.age = 0;
    plant.cycleTime = globalCycleTime;
    plants[plantPos] = plant;
  }

  bool outOfBounds(Vector2 pos) {
    return pos.x < 0 || pos.x >= width || pos.y < 0 || pos.y >= height;
  }

  void spawnTile(int x, int y) {
    Vector3 spawnPoint = new Vector3(x*Tile.SIZE, y*Tile.SIZE, boardZ);
    GameObject tileObject = Instantiate(tile_prefab, spawnPoint, Quaternion.identity);
    tileObject.transform.SetParent(transform);
    Tile tile = tileObject.GetComponent<Tile>();
    tiles.Add(tile);
  }

  void FixedUpdate() {
    if (Time.time >= nextTick) {
      tickBoard();
      nextTick = Time.time + 1 / frequency;
    }
  }

  void tickBoard() {
    List<Vector2> localPositions = new List<Vector2>();
    foreach (Vector2 plantPos in plants.Keys) {
      localPositions.Add(plantPos);
    }
    
    foreach(Vector2 plantPos in localPositions) {
      checkGrowth(plantPos, plants[plantPos]);
      if (plants[plantPos] != null) plants[plantPos].tick();
    }

    // for (int y = 0; y <= height; y++) {
    //   for (int x = 0; x <= width; x++) {
    //     Vector2 pos = new Vector2(x, y);
    //     if (plants.ContainsKey(pos)) {
    //       plants[pos].tick();
    //       checkGrowth(pos, plants[pos]);
    //     }
    //   }
    // }
  }

  void checkGrowth(Vector2 pos, Plant plant) {
    for (int rely = -plant.gSize; rely <= plant.gSize; rely++) {
      for (int relx = -plant.gSize; relx <= plant.gSize; relx++) {
        if (plant.growthPattern[(relx, rely)]) {
          growMerge(pos + new Vector2(relx, rely), plant);
        }
      }
    }
  }

  void growMerge(Vector2 pos, Plant plant) {
    if (plants.ContainsKey(pos)) {
      // Existing plant must merge
      Plant existing = plants[pos];
      if (existing == null) return;
      // existing.age = plant.age;
      // existing.age = 0;
      // existing.age = Mathf.Max(existing.age, plant.age);
    } else {
      spawnPlant(pos);
    }
  }
}
