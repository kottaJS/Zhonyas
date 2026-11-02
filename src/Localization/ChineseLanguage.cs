namespace LoLFirewallManager.Localization
{
    public class ChineseLanguage : ILanguage
    {
        public string Name { get { return "中文 (Chinese)"; } }
        public string Code { get { return "zh"; } }

        public string StatusTitle { get { return "状态:"; } }
        public string StatusReady { get { return "就绪"; } }
        public string StatusWaitingLoL { get { return "等待英雄联盟..."; } }
        public string StatusWaitingClient { get { return "等待客户端..."; } }
        public string StatusStasisActive { get { return "金身生效中"; } }
        public string StatusInGame { get { return "游戏中"; } }
        public string StatusStopped { get { return "已停止"; } }

        public string BtnStart { get { return "开始"; } }
        public string BtnStop { get { return "停止"; } }
        public string BtnReconnect { get { return "重新连接"; } }
        public string ChkSounds { get { return "提示音"; } }
        public string ChkAutoRearm { get { return "自动重置"; } }

        public string MenuOptions { get { return "&选项"; } }
        public string MenuHelp { get { return "&帮助"; } }
        public string MenuLanguage { get { return "&语言"; } }
        public string MenuOptGif { get { return "璐璐动画 (关闭可节省CPU)"; } }
        public string MenuOptSounds { get { return "声音提示"; } }
        public string MenuOptAutoRearm { get { return "下一局自动重置"; } }
        public string MenuExit { get { return "退出"; } }
        public string MenuGuide { get { return "使用指南 (常见问题)"; } }
        public string MenuVanguard { get { return "防作弊与 Vanguard 安全"; } }
        public string MenuGithub { get { return "访问 GitHub 项目..."; } }
        public string MenuAbout { get { return "关于 Zhonyas"; } }

        public string TipToggle { get { return "启动或停止监控英雄联盟。"; } }
        public string TipReconnect { get { return "紧急解冻。立即解除封锁而不停止Zhonyas。"; } }
        public string TipDuration { get { return "选择在重新连接前暂停加载画面的时间 (推荐3.5分钟)。"; } }
        public string TipSounds { get { return "在金身开始、倒计时10秒及重新连接时播放提示音。"; } }
        public string TipAutoRearm { get { return "当前对局结束时自动为下一局比赛做好准备。"; } }
        public string TipLulu { get { return "璐璐动画预览。点击或在选项中禁用以节省CPU。"; } }

        public string Dur1Min { get { return "1 分钟 (极度安全)"; } }
        public string Dur2Min { get { return "2 分钟 (安全)"; } }
        public string Dur3Min { get { return "3 分钟 (安全)"; } }
        public string Dur35Min { get { return "3.5 分钟 (推荐)"; } }
        public string Dur4Min { get { return "4 分钟 (有风险 - 比赛可能开始)"; } }
        public string Dur5Min { get { return "5 分钟 (极度危险 - 重开风险)"; } }
        public string Dur15Sec { get { return "15 秒 (快速测试)"; } }

        public string GuideContent
        {
            get
            {
                return "ZHONYAS 使用说明：\n\n" +
                       "1. 选择所需的延迟时长 (建议选择 3.5 分钟)。\n" +
                       "2. 点击 '开始' (或保持开启 '自动重置')。\n" +
                       "3. 在游戏加载界面，Zhonyas 会临时挂起网络连接。\n" +
                       "4. 倒计时结束时，系统会在小兵刷新前自动重新连接。\n" +
                       "5. 可随时点击 '重新连接' 提前解除锁定。\n\n" +
                       "提示：\n" +
                       "- 在 '选项' 中关闭璐璐动画可使 CPU 占用降至 0%。\n" +
                       "- 鼠标悬停在按钮上可查看对应功能说明。";
            }
        }

        public string VanguardContent
        {
            get
            {
                return "防作弊与 VANGUARD 安全说明：\n\n" +
                       "Zhonyas 纯粹在 Windows 防火墙层面运作。\n\n" +
                       "- 不会向英雄联盟注入任何代码。\n" +
                       "- 不会修改任何游戏文件。\n" +
                       "- 不会读取游戏内存。\n" +
                       "- 仅调用 Windows 官方网络 API。\n\n" +
                       "因此，完全不用担心触发 Vanguard 反作弊封号。";
            }
        }

        public string AboutContent
        {
            get
            {
                return "Zhonyas v1.0.0\n" +
                       "英雄联盟加载界面实用工具\n\n" +
                       "由 kottaJS 开发\n" +
                       "基于 GPL-3.0 协议开源";
            }
        }

        public string StopMsgBoth
        {
            get { return "停止将取消当前的金身状态及下一局的自动重置。\n\n如果只想提前解冻游戏，请点击 '重新连接'。"; }
        }

        public string StopMsgAutoRearm
        {
            get { return "停止将关闭 Zhonyas 并取消下一局的自动重置。\n\n如果只想提前解冻游戏，请点击 '重新连接'。"; }
        }

        public string StopMsgStasis
        {
            get { return "停止将取消当前正在生效的金身状态。\n\n如果只想提前解冻游戏，请点击 '重新连接'。"; }
        }

        public string DontShowAgain { get { return "不再显示此提示"; } }
        public string BtnCancel { get { return "取消"; } }

        public string TrayOpen { get { return "打开 Zhonyas"; } }
        public string TrayReconnected { get { return "已重新连接！游戏已解除冻结。"; } }
        public string TrayArmed { get { return "比赛已结束。已为下一局就绪！"; } }
    }
}

