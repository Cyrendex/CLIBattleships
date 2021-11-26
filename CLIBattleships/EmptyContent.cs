using System;
using System.Collections.Generic;
using System.Text;

namespace CLIBattleships
{
    public class EmptyContent : GridContent
    {
        public override char Symbol { get; } = Symbols.EMPTY_SYMBOL;
        public override int Score { get; set; } = 0;
        public override string ReturnHitMessage(bool isSalvoVariation = false)
        {
            return " missed!";
        }
    }
}
