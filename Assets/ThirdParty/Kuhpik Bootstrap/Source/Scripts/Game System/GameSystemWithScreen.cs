namespace Kuhpik
{
    public abstract class GameSystemWithScreen<T> : GameSystem where T : UIScreen
    {
        protected T screen;
        protected UIConfig uiConfig;
        public override void OnInit()
        {
            base.OnInit();
            screen.Init(uiConfig);
        }
    }
}