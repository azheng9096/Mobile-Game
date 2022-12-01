using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerSavedData {
    public static float maxHP = 50f;
    public static float HP = 50f;
    public static List<Module> playerDeck = new List<Module>();

    public static void ResetSavedData() {
        maxHP = 50f;
        HP = 50f;
        playerDeck.Clear();
    }
}
