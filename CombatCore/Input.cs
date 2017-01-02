using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameBot
{
    class IO
    {
        public async Task ProcessInput(IOMode mode, Object msg)
        {
            if (IsCommand(mode, msg))
            {
                string str = GetInputContent(mode, msg);
                if (str.StartsWith(")")) str = str.Substring(1);
                string[] input = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (input.Length < 10)
                {
                    var i = input;
                    input = new string[] { "", "", "", "", "", "", "", "", "", "" };
                    i.CopyTo(input, 0);
                }

                Player Pl;
                if (Program.Prog.Players.ContainsKey(GetInputAuthor(mode, msg)))
                    Pl = Program.Prog.Players[GetInputAuthor(mode, msg)];
                else Pl = null;

                int val1, val2, val3;
                switch (input[0])
                {
                    case "start": Player.Init(mode, msg); break;

                    case "interrupt":
                    if (Pl != null)
                        {
                            if (Pl.Action != null)
                            {
                                Pl.Interrupted = true;
                            }
                            else Output(mode, msg, "You're not busy!");
                        }
                            else Output(mode, msg, "You're not a player yet!");
                            break;

                    case "mine":
                        if (Pl != null && int.TryParse(input[1], out val1))
                        {
                            if (Pl.Action == null)
                            {
                                Pl.Action = Pl.IncomeOverTime(mode, msg, val1, Skill.Mining, new Dictionary<Material, int>
                                { { Material.TinOre, 40 }, { Material.CopperOre, 100 } });
                            }
                            else Output(mode, msg, "You're busy! Wait " + Pl.MinutesLeft + " minutes or use )interrupt .");
                        }
                        else Output(mode, msg, "Either wrong argument or you're not a player yet!");
                        break;

                    case "fish":
                        if (Pl != null && int.TryParse(input[1], out val1))
                        {
                            if (Pl.Action == null)
                            {
                                Pl.Action = Pl.IncomeOverTime(mode, msg, val1, Skill.Fishing, new Dictionary<Material, int>
                                { { Material.Cod, 30 }, { Material.Bass, 100 } });
                            }
                            else Output(mode, msg, "You're busy! Wait " + Pl.MinutesLeft + " minutes or use )interrupt .");
                        }
                        else Output(mode, msg, "Either wrong argument or you're not a player yet!");
                        break;

                    case "woodcut":
                        if (Pl != null && int.TryParse(input[1], out val1))
                        {
                            if (Pl.Action == null)
                            {
                                Pl.Action = Pl.IncomeOverTime(mode, msg, val1, Skill.Woodcutting, new Dictionary<Material, int>
                                { { Material.BirchLog, 100 }, { Material.OakLog, 70 } });
                            }
                            else Output(mode, msg, "You're busy! Wait " + Pl.MinutesLeft + " minutes or use )interrupt .");
                        }
                        else Output(mode, msg, "Either wrong argument or you're not a player yet!");
                        break;

                    case "collect":
                        if (Pl != null && int.TryParse(input[1], out val1))
                        {
                            if (Pl.Action == null)
                            {
                                Pl.Action = Pl.IncomeOverTime(mode, msg, val1, Skill.Herbalism, new Dictionary<Material, int>
                                { { Material.Thyme, 100 }, { Material.Basil, 60 } });
                            }
                            else Output(mode, msg, "You're busy! Wait " + Pl.MinutesLeft + " minutes or use )interrupt .");
                        }
                        else Output(mode, msg, "Either wrong argument or you're not a player yet!");
                        break;

                    case "saw":
                        if (Pl != null && int.TryParse(input[1], out val1) && int.TryParse(input[2], out val2))
                        {
                            if (Enum.IsDefined(typeof(Material), val2) && val2 >= 0 && val2 < 64)
                            {
                                if (Pl.Materials.ContainsKey((Material)val2) && Pl.Materials[(Material)val2] >= val1)
                                {
                                    Process p = Process.Saw;
                                    /*if (Pl.Processes[p].Count == 0)
                                        Pl.Processes[p].Add(Pl.ProcessOverTime(mode, msg, val1, p, (Material)val2,
                                            (Material)(val2 + 64)));
                                    else Pl.Processes[p].Add(
                                        Pl.Processes[p][Pl.Processes[p].Count - 1].ContinueWith(delegate {
                                            Pl.ProcessOverTime(mode, msg, val1, p, (Material)val2,
                                                (Material)(val2 + 64));
                                        }));*/
                                }
                                else Output(mode, msg, "You don't have enough of this item!");
                            }
                            else Output(mode, msg, "Invalid material! Inapplicable or nonexistent.");
                        }
                        else Output(mode, msg, "Either wrong argument or you're not a player yet!");
                        break;

                    case "smelt":
                        if (Pl != null && int.TryParse(input[1], out val1) && int.TryParse(input[2], out val2))
                        {
                            if (Enum.IsDefined(typeof(Material), val2) && val2 >= 1024 && val2 < 1080)
                            {
                                if (Pl.Materials.ContainsKey((Material)val2) && Pl.Materials[(Material)val2] >= val1)
                                {
                                    Process p = Process.Smelt;
                                    /*if (Pl.Processes[p].Count == 0)
                                        Pl.Processes[p].Add(Pl.ProcessOverTime(mode, msg, val1, p, (Material)val2,
                                            (Material)(val2 + 64)));
                                    else Pl.Processes[p].Add(
                                        Pl.Processes[p][Pl.Processes[p].Count - 1].ContinueWith(delegate
                                        {
                                            Pl.ProcessOverTime(mode, msg, val1, p, (Material)val2,
                                                (Material)(val2 + 64));
                                        }));*/
                                }
                                else Output(mode, msg, "You don't have enough of this item!");
                            }
                            else Output(mode, msg, "Invalid material! Inapplicable or nonexistent.");
                        }
                        else Output(mode, msg, "Either wrong argument or you're not a player yet!");
                        break;

                    case "admin": AdminCommands.Process(mode, msg, input); break;
                    default: Output(mode, msg, "Hmm?"); break;
                }
            }

            /*if (ArrayContains(arg.GetType().GetInterfaces(), typeof (ITextChannel)))
            {
                //ITextChannel msg = arg as ITextChannel;
                //if(msg.)
            }*/
        }

        public bool ArrayContains(Array a, Object o)
        {
            if (a == null || o == null) return false;
            //for (var i = 0; i < a.GetLength(); i += 1)
            foreach (Object obj in a)
            {
                if (o.Equals(obj)) return true;
            }
            return false;
        }

        public bool IsCommand(IOMode mode, Object msg)
        {
            if (mode == IOMode.Discord)
            {
                string c = (msg as SocketMessage).Content;
                if (c.StartsWith(")")) return true;
                else return false;
            }
            if (mode == IOMode.Console)
            {
                return true;
            }

            return false;
        }

        public string GetInputContent(IOMode mode, Object msg)
        {
            if (mode == IOMode.Discord)
            {
                return (msg as SocketMessage).Content;
            }
            if (mode == IOMode.Console)
            {
                return (msg as ACIData).Content;
            }

            return "";
        }

        public ulong GetInputAuthor(IOMode mode, Object msg)
        {
            if (mode == IOMode.Discord)
            {
                return (msg as SocketMessage).Author.Id;
            }
            if (mode == IOMode.Console)
            {
                return (msg as ACIData).Player;
            }

            return 0;
        }

        public void Output(IOMode mode, Object msg, string output)
        {
            if (mode == IOMode.Discord)
            {
                (msg as SocketMessage).Channel.SendMessageAsync(output);
            }
            if (mode == IOMode.Console)
            {
                Console.Out.WriteLineAsync(output);
            }
        }

        public async Task DiscordInput(SocketMessage msg)
        {
            await ProcessInput(IOMode.Discord, msg);
        }
    }

    class ACIData
    {
        public ulong Player;
        public string Content;

        public ACIData(string msg)
        {
            Player = 173007751822311424ul;
            Content = msg;
        }
    }
}