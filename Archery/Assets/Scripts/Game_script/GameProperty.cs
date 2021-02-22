using Photon.Realtime;
using Random = UnityEngine.Random;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public static class GameProperty
{
    private const string ScoreKey = "Score"; // スコアのキーの文字列

    private static Hashtable hashtable = new Hashtable();

    // （Hashtableに）プレイヤーのスコアがあれば取得する
    public static bool TryGetScore(this Hashtable hashtable, out int score) {
        if (hashtable[ScoreKey] is int value) {
            score = value;
            return true;
        }
        score = 0;
        return false;
    }

    // プレイヤーのスコアを取得する
    public static int GetScore(this Player player) {
        player.CustomProperties.TryGetScore(out int score);
        return score;
    }

    // （相手に弾を当てた）プレイヤーのカスタムプロパティを更新する
    public static void OnDealDamage(this Player player) {
        hashtable[ScoreKey] = player.GetScore() + 10; // スコアを増やす

        player.SetCustomProperties(hashtable);
        hashtable.Clear();
    }
}