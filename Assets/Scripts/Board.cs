using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour {
  [SerializeField] public int width;
  [SerializeField] public int height;
  [SerializeField] public int lifeTime;
  [SerializeField] public int deathTimeMult;
  [SerializeField] public int randomPerTick;
  [SerializeField] float eraserFraction;
  [SerializeField] float frequency;
  [SerializeField] public AudioSource audioSource;
  [SerializeField] EyeComputer eyeComputer;
  [SerializeField] Interfaze interfaze;
  [SerializeField] public List<Level> levels;
  [NonSerialized] public int lid;
  public float startingTime;
  public float endTime;
  public bool eraserMode;
  public int eraserSize;
  private float nextTick;
  private bool ticking = false;
  private bool started = false;

  void Awake() {
    nextTick = Time.time + 1 / frequency;
    loadLevel(0);
    // Tile.SIZE = 1;
    // Pattern.SIZE = 3;
    // spawnTiles();
    // spawnPlants();
    // StartCoroutine("ticker");

  }

  private void Update() {
    if (Input.GetKeyDown(KeyCode.Space)) toggleGame();
  }

  public void toggleGame() {
    if (!started) {
      startGame();
      return;
    }
    ticking = !ticking;

  }

  public void startGame() {
    ticking = true;
    started = true;
    Cursor.lockState = CursorLockMode.Confined;
    eyeComputer.initialisation();
    eyeComputer.preparation();

    startingTime = Time.time;
    if (audioSource.clip != null) endTime = startingTime + audioSource.clip.length;
    else endTime = startingTime + levels[lid].time;
    audioSource.Play();

    interfaze.startInterface();
    interfaze.hideControl();
    StartCoroutine("levelStopper");
  }

  public void stopGame() {
    ticking = false;
    started = false;
    Cursor.lockState = CursorLockMode.None;
    audioSource.Stop();
    interfaze.updateTimer();
    checkResult();
  }

  private void checkResult() {
    int desiredPercentage = levels[lid].desiredPercentage;
    // First counting then we can cleanup
    (Dictionary<Color, int>, int) colourCounts = eyeComputer.countColours();
    eyeComputer.cleanup();

    int reachedPercentage = (int)((float)colourCounts.Item1[Pattern.selected] / colourCounts.Item2 * 100);
    // Debug.Log(String.Format("col {0}, total {1}", colourCounts.Item1[Pattern.selected], colourCounts.Item2));
    // Debug.Log(String.Format("Reached {0}, desired {1}", reachedPercentage, levels[lid].desiredPercentage));

    if (reachedPercentage >= levels[lid].desiredPercentage)
      interfaze.showEnd(colourCounts.Item1[Pattern.selected], reachedPercentage, true);
    else
      interfaze.showEnd(colourCounts.Item1[Pattern.selected], reachedPercentage, false);

  }

  void FixedUpdate() {
    if (Time.time >= nextTick && ticking) {
      eyeComputer.doCompute();
      eyeComputer.doAdditions();
      interfaze.updateTimer();
      nextTick = Time.time + 1 / frequency;
    }
  }

  public void loadLevel(int levelID) {
    lid = levelID;
    Level l = levels[lid];
    audioSource.clip = l.theme;
    Debug.Log(l.theme.length);
    frequency = l.frequency;
    eraserMode = l.erasorMode;
    randomPerTick = l.randomPerTick;
    eraserSize = (int)(Mathf.Min(width, height) / (1 / l.eraserFraction));
    Pattern.tileCount = l.patternTileCount;
    Pattern.createPattern();
    Pattern.selected = l.desiredColor;
    Pattern.cleanPallete();
    Pattern.addColor(l.desiredColor);
    foreach (Color c in l.otherColours) Pattern.addColor(c);
    interfaze.initInterface();
    interfaze.setMidText(l.startingMessage);
  }

  IEnumerator levelStopper() {
    // float waittime = levels[lid].time;
    // if (audioSource.isPlaying) waittime = audioSource.clip.length;
    while (Time.time < endTime) yield return new WaitForSeconds(0.5f);
    stopGame();
  }


  // Timer coroutine
  // Colour counter
  // 
}
