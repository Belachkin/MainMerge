using Kuhpik;
using Source.Scripts.Helpers;

namespace Source.Scripts.Systems.Loading
{
    public class SaveGameDataLoadingSystem : GameSystem
    {
        public override void OnInit()
        {
            base.OnInit();
            //init save data null values
            // save.UnlockedChests ??= new HashSet<ChestType>() {ChestType.DEFAULT};
            game.CameraSwitcher = FindObjectOfType<CameraSwitcher>();
            game.AnimManager = new AnimManager();
        }
    }
}