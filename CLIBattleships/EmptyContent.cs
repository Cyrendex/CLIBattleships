using System;
using System.Collections.Generic;
using System.Text;

namespace CLIBattleships
{
    public class EmptyContent : GridContent
    {
        public override int score { get; set; } = 0;
        public override string ReturnHitMessage(bool isSalvoVariation = false)
        {
            return " missed!";
        }
    }
}
