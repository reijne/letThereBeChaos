﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour {
  [SerializeField] GameObject selectTile_prefab;
  [SerializeField] GameObject controlObject;
  [SerializeField] int controlZ;
  public static Color selectedColor;
  public static Vector2Int offset = Vector2Int.zero;
  public static bool show = false;
  Dictionary<Color, Pattern> patterns = new Dictionary<Color, Pattern>();
  List<GameObject> selectTileObjects = new List<GameObject>();

  private void Update() {
    if (show) {
      showControls();
      show = false;
    }
  }

  public void showControls() {
    controlObject.SetActive(true);
    spawnSelectTiles();
  }

  public void hideControls() {
    clearSelectTiles();
    controlObject.SetActive(false);
  }

  void spawnSelectTiles() {
    if (patterns.ContainsKey(selectedColor)) {
      showPattern(patterns[selectedColor]);
    } else {
      clearSelectTiles();
      patterns[selectedColor] = new Pattern();
      makePattern();
    }
  }

  void makePattern() {
    for (int y = 0; y < Pattern.SIZE; y++) {
      for (int x = 0; x < Pattern.SIZE; x++) {
        spawnSelectTile(x + offset.x, y + offset.y);
      }
    }
  }

  void showPattern(Pattern pattern) {
    foreach (KeyValuePair<(int, int), bool> tileInfo in pattern.selectedTiles) {
      spawnSelectTile(tileInfo.Key.Item1, tileInfo.Key.Item2, pattern.selectedTiles[tileInfo.Key]);
    }
  }

  public void savePattern() {
    patterns[selectedColor] = new Pattern();
    foreach (GameObject tile in selectTileObjects) {
      Debug.Log("Adding a tile");
      SelectTile selectTile = tile.GetComponent<SelectTile>();
      patterns[selectedColor].selectedTiles[(selectTile.x, selectTile.y)] = selectTile.selected;
    }
    Debug.Log(patterns[selectedColor].stringer());
    hideControls();
  }

  void spawnSelectTile(int x, int y, bool selected = false) {
    Vector3 spawnPoint = new Vector3(x * Tile.SIZE, y * Tile.SIZE, controlZ);
    spawnPoint += transform.position;
    GameObject tileObject = Instantiate(selectTile_prefab, spawnPoint, Quaternion.identity);
    tileObject.transform.SetParent(transform);
    SelectTile sTile = tileObject.GetComponent<SelectTile>();
    sTile.choseColor(new Color(selectedColor.r, selectedColor.g, selectedColor.b, 0.5f));
    sTile.x = x;
    sTile.y = y;
    if (selected) sTile.toggle();
    selectTileObjects.Add(tileObject);
  }

  void clearSelectTiles() {
    foreach (GameObject tile in selectTileObjects) Destroy(tile);
    selectTileObjects.RemoveRange(0, selectTileObjects.Count);
  }
}