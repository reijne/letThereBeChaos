using System;
using System.Collections.Generic;
using UnityEngine;

public class Plant : Tile {
  public static int count;
  public bool isAlive = true;
  int age = 0;
  private void Start() {
    count++;
  }

  public void tick() {
    age++;
    if (age == board.lifeTime) {
      kill();
    } else if (age >= board.deathTimeMult * board.lifeTime) {
      remove();
    }
  }

  public void revive(Color color) {
    isAlive = true;
    age = 0;
    setColor(color);
    gameObject.SetActive(true);
  }

  private void kill() {
    // gameObject.SetActive(false);
    setColor(Pallette.invis);
    isAlive = false;
  }

  private void remove() {
    // board.removePlant(this);
    Destroy(this.gameObject);
    Destroy(this);
  }

  private void OnMouseDown() {
    remove();
    // if (Pattern.selected != null && pos != null) board.spawnPlant(pos, Pattern.selected.color);
  }
}
