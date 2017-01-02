using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBot
{
    class Utils
    {
        // 10, 30 => 0-39
        public static tp WeightenedChance<tp>(Dictionary<tp, int> args)
        {
            int i = 0;
            foreach (KeyValuePair<tp, int> pair in args)
            {
                i += pair.Value;
            }
            if (i > 0)
            {
                i = Program.Prog.Rand.Next(i);
                foreach (KeyValuePair<tp, int> pair in args)
                {
                    if (i < pair.Value)
                        return pair.Key;
                    else i -= pair.Value;
                }
            }

            Console.Out.WriteLineAsync("Returning default from mining! >.<");
            return default(tp);
        }

        /*public static string GetVerb(Enum en, int val)
        {
            if(en is Skill)
                switch ((Skill)val)
                {
                    case Skill.Woodcutting: return "Obtained";
                    case Skill.Carpenting: return "Obtained";

                    case Skill.Fishing

                    case Skill.Mining
                    case Skill.Smelting
                    case Skill.GemExtracting
                    case Skill.Digging
                    case Skill.RockPolishing

                    case Skill.Herbalism
                    case Skill.FruitPicking
                }

            return "?????";
        }*/
    }
}
