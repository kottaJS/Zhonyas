namespace LoLFirewallManager.Localization
{
    public class JapaneseLanguage : ILanguage
    {
        public string Name { get { return "日本語 (Japanese)"; } }
        public string Code { get { return "ja"; } }

        public string StatusTitle { get { return "状態:"; } }
        public string StatusReady { get { return "準備完了"; } }
        public string StatusWaitingLoL { get { return "LoLを待機中..."; } }
        public string StatusWaitingClient { get { return "クライアント待機中..."; } }
        public string StatusStasisActive { get { return "砂時計発動中"; } }
        public string StatusInGame { get { return "ゲーム中"; } }
        public string StatusStopped { get { return "停止"; } }

        public string BtnStart { get { return "開始"; } }
        public string BtnStop { get { return "停止"; } }
        public string BtnReconnect { get { return "再接続"; } }
        public string ChkSounds { get { return "サウンド"; } }
        public string ChkAutoRearm { get { return "自動再準備"; } }

        public string MenuOptions { get { return "&設定"; } }
        public string MenuHelp { get { return "&ヘルプ"; } }
        public string MenuLanguage { get { return "&言語"; } }
        public string MenuOptGif { get { return "ルル アニメーション (CPU節約)"; } }
        public string MenuOptSounds { get { return "サウンド通知"; } }
        public string MenuOptAutoRearm { get { return "次の試合を自動再準備"; } }
        public string MenuExit { get { return "終了"; } }
        public string MenuGuide { get { return "使い方 (FAQ)"; } }
        public string MenuVanguard { get { return "チート対策とVanguardの安全性"; } }
        public string MenuGithub { get { return "GitHubプロジェクトを開く..."; } }
        public string MenuAbout { get { return "Zhonyasについて"; } }

        public string TipToggle { get { return "LoLの監視を開始または停止します。"; } }
        public string TipReconnect { get { return "緊急ブロック解除。Zhonyasを停止せずに即座に再接続します。"; } }
        public string TipDuration { get { return "再接続前にロード画面を停止する時間を選択します (3.5分推奨)。"; } }
        public string TipSounds { get { return "スタシス開始、10秒前カウントダウン、再接続時に音声を再生します。"; } }
        public string TipAutoRearm { get { return "現在の試合が終了すると次の試合のために自動的に再準備します。"; } }
        public string TipLulu { get { return "ルルのアニメーション。クリックまたはオプションでオフにしてCPUを節約します。"; } }

        public string Dur1Min { get { return "1分 (非常に安全)"; } }
        public string Dur2Min { get { return "2分 (安全)"; } }
        public string Dur3Min { get { return "3分 (安全)"; } }
        public string Dur35Min { get { return "3.5分 (推奨)"; } }
        public string Dur4Min { get { return "4分 (リスクあり - 試合開始の可能性)"; } }
        public string Dur5Min { get { return "5分 (危険 - リメイクのリスク)"; } }
        public string Dur15Sec { get { return "15秒 (クイックテスト)"; } }

        public string GuideContent
        {
            get
            {
                return "ZHONYAS の使い方:\n\n" +
                       "1. 一時停止したい時間を選択します (3.5分が推奨です)。\n" +
                       "2. '開始' をクリックします (または '自動再準備' を有効のままにします)。\n" +
                       "3. ゲームのロード画面中に、Zhonyas が接続を一時停止します。\n" +
                       "4. カウントダウン終了後、ミニオンが出現する前に自動的に再接続されます。\n" +
                       "5. いつでも '再接続' をクリックして即座に解除できます。\n\n" +
                       "ヒント:\n" +
                       "- '設定' でルルのアニメーションをオフにすると、CPU使用率が 0% になります。\n" +
                       "- ボタンにカーソルを合わせると説明が表示されます。";
            }
        }

        public string VanguardContent
        {
            get
            {
                return "チート対策とVANGUARDの安全性:\n\n" +
                       "Zhonyasは完全にWindowsファイアウォールレベルで動作します。\n\n" +
                       "- ゲーム本体へのコード注入は一切行いません。\n" +
                       "- ゲームファイルを変更しません。\n" +
                       "- ゲームメモリを読み取りません。\n" +
                       "- 公式のWindowsネットワークAPIのみを使用します。\n\n" +
                       "そのため、VanguardによるBANのリスクはありません。";
            }
        }

        public string AboutContent
        {
            get
            {
                return "Zhonyas v1.0.0\n" +
                       "League of Legends ロード画面ユーティリティ\n\n" +
                       "開発者: kottaJS\n" +
                       "GPL-3.0 オープンソースライセンス";
            }
        }

        public string StopMsgBoth
        {
            get { return "停止すると、現在のスタシスと次回以降の自動再準備の両方がキャンセルされます。\n\n早期に解除したいだけの場合は '再接続' を使用してください。"; }
        }

        public string StopMsgAutoRearm
        {
            get { return "停止するとZhonyasがオフになり、次回以降の自動再準備がキャンセルされます。\n\n早期に解除したいだけの場合は '再接続' を使用してください。"; }
        }

        public string StopMsgStasis
        {
            get { return "停止すると、現在のスタシスがキャンセルされます。\n\n早期に解除したいだけの場合は '再接続' を使用してください。"; }
        }

        public string DontShowAgain { get { return "今後このメッセージを表示しない"; } }
        public string BtnCancel { get { return "キャンセル"; } }

        public string TrayOpen { get { return "Zhonyasを開く"; } }
        public string TrayReconnected { get { return "再接続完了！ゲームの停止が解除されました。"; } }
        public string TrayArmed { get { return "試合終了。次の試合の準備が整いました！"; } }
    }
}

