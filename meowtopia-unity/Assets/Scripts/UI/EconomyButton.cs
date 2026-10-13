using UnityEngine;

public class EconomyButton : MonoBehaviour {
    public int coinsToAdd = 50;

    public void EarnCoins() {
        EconomyService.EarnCoins(coinsToAdd);
    }
}
