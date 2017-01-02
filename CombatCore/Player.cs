using System;
using Discord;
using Discord.WebSocket;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameBot
{
    [Serializable]
    class Player
    {
        public Int32 Money;
        public Int32 Exp;
        public Int32 MaxHealth;
        public Int32 Health;
        public Task Action;
        public Dictionary<Process, Task> ProcessCurrent;
        public Dictionary<Process, List<Delegate>> ProcessQueue;
        public Dictionary<Process, int> Equipment;
        public Dictionary<Material, int> Materials;
        public Dictionary<Skill, int> SkillPoints;

        public bool Interrupted = false;
        public int MinutesLeft = -1;
        //If I need to add more tasks to the "queue" further, do I
        //public delegate Task ProcessOverTimeDelegate(IOMode mode, Object msg, int minutes, Process process, Material input, Material output);
        //public delegate Task IncomeOverTimeDelegate(IOMode mode, Object msg, int minutes, Skill skill, Dictionary<Material, int> drops);

        public static void Init(IOMode mode, Object msg)
        {
            IO IO = Program.Prog.IO;

            if (Program.Prog.Players.ContainsKey(IO.GetInputAuthor(mode, msg)))
            {
                IO.Output(mode, msg, "You're a player already.");
                return;
            }

            IO.Output(mode, msg, "Welcome " + IO.GetInputAuthor(mode, msg) + " to the game!");

            Player p = new Player();

            Program.Prog.AddPlayer(IO.GetInputAuthor(mode, msg), p);
        }

        public Player()
        {
            Money = 1000;
            Exp = 0;
            MaxHealth = 100;
            Health = MaxHealth;
            Materials = new Dictionary<Material, int>() { { Material.TinOre, 200 } };
            SkillPoints = new Dictionary<Skill, int>();
            ProcessQueue = new Dictionary<Process, List<Delegate>>();
            Equipment = new Dictionary<Process, int>();
            foreach (Skill s in Enum.GetValues(typeof(Skill)))
            {
                SkillPoints.Add(s, 0);
            }
            foreach (Process p in Enum.GetValues(typeof(Process)))
            {
                ProcessQueue.Add(p, new List<Delegate>());
                Equipment.Add(p, 1);
            }

            //REMOVE

            Materials[Material.TinOre] = 1000;

            //REM/OVE
        }

        public async Task IncomeOverTime(IOMode mode, Object msg, int minutes, Skill skill, Dictionary<Material, int> drops) //mining, collecting... Stuff
        {
            var gain = new Dictionary<Material, int>();

            int i = 0;

            while (i < minutes)
            {
                if (this.Interrupted == true) { Interrupted = false; goto end; }
                await Task.Delay(60000 / 1000); // will remove "/1000" later
                MinutesLeft = minutes - i;
                i += 1;
            }
        end: { }

            Material m;
            while (i > 0)
            {
                i -= 1;
                m = Utils.WeightenedChance<Material>(drops);
                if (Materials.ContainsKey(m)) Materials[m] += 1;
                else Materials.Add(m, 1);

                if (gain.ContainsKey(m)) gain[m] += 1;
                else gain.Add(m, 1);
            }

            string str = "";
            foreach (KeyValuePair<Material, int> pair in gain)
            {
                str = str + "Obtained " + pair.Value.ToString() + " " + pair.Key.ToString() + "!\n";
            }
            Program.Prog.IO.Output(mode, msg, str);

            SkillPoints[Skill.Mining] += minutes;
            this.Action = null;
            MinutesLeft = 0;
            return;
        }

        public async Task ProcessOverTime(IOMode mode, Object msg, int minutes, Process process, Material input, Material output) //mining, collecting... Stuff
        {
            int i = 0;

            while (i < minutes)
            {
                if (this.Interrupted == true && Materials[input] <= 0) { Interrupted = false; goto end; }
                await Task.Delay(60000 / 1000); // will remove "/1000" later
                Materials[input] -= 1;
                if (Materials.ContainsKey(output)) Materials[output] += 1;
                else Materials.Add(output, 1);
                MinutesLeft = minutes - i;
                i += 1;
            }
        end: { }

            Program.Prog.IO.Output(mode, msg, "Processed " + i.ToString() + " " + input.ToString() + " to " + output.ToString() + "!");
            if (ProcessQueue.Count > 0)
            {
                ProcessCurrent[process] = ProcessQueue[process][0].DynamicInvoke() as Task;
                ProcessQueue[process].RemoveAt(0);
            }
            else ProcessCurrent[process] = null;


            MinutesLeft = 0;
            return;
        }

        public void AddProcessOverTime(IOMode mode, Object msg, int minutes, Process process, Material input, Material output)
        {
            if (ProcessCurrent[process] == null)
            {
                ProcessCurrent[process] = ProcessOverTime(mode, msg, minutes, process, input, output);
            }
            else ProcessQueue[process].Add(() => ProcessOverTime(mode, msg, minutes, process, input, output));
        }
    }
}