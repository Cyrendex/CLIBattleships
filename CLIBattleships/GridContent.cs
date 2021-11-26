using System;
using System.Collections.Generic;
using System.Text;

namespace CLIBattleships
{
    public abstract class GridContent
    {
        public abstract char Symbol { get; }
        public abstract int Score { get; set; }
        public abstract string ReturnHitMessage(bool isSalvoVariation = false);

    }
}
