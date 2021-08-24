using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour {
  [SerializeField] GameObject selectTile_prefab;
  [SerializeField] GameObject controlObject;
  [SerializeField] int controlZ;
  public static Color selectedColor;
  Dictionary<Color, Pattern> patterns = new Dictionary<Color, Pattern>();
  List<GameObject> selectTileObjects = new List<GameObject>();

  void spawnSelectTiles() {
    if (patterns.ContainsKey(selectedColor)) {
      showPattern(patterns[selectedColor]);
    } else {
      clearSelectTiles();
      patterns[selectedColor] = new Pattern();
    }
  }

  void makePattern() {
    for (int y = 0; y < Pattern.SIZE; y++) {
      for (int x = 0; x < Pattern.SIZE; x++) {
        spawnSelectTile(x, y);
      }
    }
  }

  void showPattern(Pattern pattern) {
    foreach ((int, int) coords in pattern.selectedTiles.Keys) {
      spawnSelectTile(coords.Item1, coords.Item2, pattern.selectedTiles[coords]);
    }
  }

  void savePattern() {
    patterns[selectedColor] = new Pattern();
    foreach (GameObject tile in selectTileObjects) {
      SelectTile selectTile = tile.GetComponent<SelectTile>();
      patterns[selectedColor].selectedTiles[(selectTile.x, selectTile.y)] = selectTile.selected;
    }
  }

  void spawnSelectTile(int x, int y, bool selected = false) {
    Vector3 spawnPoint = new Vector3(x * Tile.SIZE, y * Tile.SIZE, controlZ);
    spawnPoint += transform.position;
    GameObject tileObject = Instantiate(selectTile_prefab, spawnPoint, Quaternion.identity);
    tileObject.transform.SetParent(transform);
    SelectTile sTile = tileObject.GetComponent<SelectTile>();
    sTile.x = x;
    sTile.y = y;
    if (selected) sTile.toggle();
  }

  void clearSelectTiles() {
    foreach (GameObject tile in selectTileObjects) Destroy(tile);
    selectTileObjects.RemoveRange(0, selectTileObjects.Count);
  }

  public void show() {
    controlObject.SetActive(true);
  }
}
