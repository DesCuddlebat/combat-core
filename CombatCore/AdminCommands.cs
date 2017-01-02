
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord;

namespace GameBot
{
    class AdminCommands
    {
        public static void Process(IOMode mode, Object msg, string[] args)
        {
            IO IO = Program.Prog.IO;

            if (IO.GetInputAuthor(mode, msg) != 173007751822311424ul)
            {
                IO.Output(mode, msg, "U wot m8");
                return;
            }
            else
            {
                int val1, val2, val3;
                ulong u1, u2, u3;
                switch (args[1])
                {
                    case "give":
                        if (ulong.TryParse(args[2], out u1) && int.TryParse(args[3], out val1) && int.TryParse(args[4], out val2))
                        {
                            if (Program.Prog.Players.ContainsKey(u1) && Enum.IsDefined(typeof(Material), val1))
                            {
                                if (Enum.IsDefined(typeof(Material), val1))
                                {
                                    if (Program.Prog.Players[u1].Materials.ContainsKey((Material)val1))
                                        Program.Prog.Players[u1].Materials[(Material)val1] += val2;
                                    else Program.Prog.Players[u1].Materials.Add((Material)val1, val2);

                                    IO.Output(mode, msg, "Gave user ID " + IO.GetInputAuthor(mode, msg).ToString() + " " + val2 + " " + ((Material)val1).ToString() + "!");
                                    break;
                                }
                            }
                        }
                        IO.Output(mode, msg, "Wrong arguments ya mustard");
                        break;
                }
            }
        }
    }
}
