using UnityEngine;

// Interface to define save and load game functionality
public interface ISaveGame
{
    // Called to load game data
    void LoadGameData();

    // Called to clear game state before loading data
    void ClearGameState();

    // Called to save game data
    void SaveGameData();
}
