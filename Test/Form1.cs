using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BLMDPSSim
{
	public delegate void SimStartHandler(int seconds);
	public delegate void SimProgressHandler();
	public delegate void SimStopHandler();
	public delegate void SimCompleteHandler();
 
	public partial class Form1 : Form
	{
		public static event SimStartHandler SimStartEvent;
		public static event SimProgressHandler SimProgressEvent;
		public static event SimStopHandler SimStopEvent;
		public static event SimCompleteHandler SimCompleteEvent;
		private static double _simLength = 10000;

		public enum SpellType
		{
			ThunderDoT,
			Thunder2,
			Thunder3,
			Fire,
			Fire3,
			Blizzard3,
			None
		}
		public enum BuffType
		{
			AF,
			AF3,
			UI3

		private int SpellPotency(SpellType spell, BuffType mode, bool ThunderCloud)
		{
			switch (spell)
			{
				case SpellType.ThunderDoT:
					return 35;
				case SpellType.Thunder2:
					return 50;
				case SpellType.Thunder3:
					if (ThunderCloud) return 60 + 35 * 8;
					return 60;
				case SpellType.Fire:
					if (mode == BuffType.AF3) return (int)(150 * 1.8);
					else if (mode == BuffType.AF) return (int)(150*1.2);
					return (int)(150 * 0.5);
				case SpellType.Fire3:
					if (mode == BuffType.AF3) return (int)(220 * 1.8);
					else if (mode == BuffType.AF) return (int)(220 * 1.2);
					return (int)(220 * 0.5);
				case SpellType.Blizzard3:
					if (mode != BuffType.UI3) return (int)(220 * .05);
					return 220;
				default:
					return 0;
			}
		}

		public Form1()
		{
			InitializeComponent();
		}

		private double DamageValue(int wdmg, int intel, int dtr, int crit)
		{
			return (wdmg * .2714745 + intel * .1006032 + (dtr - 202) * .0241327 + wdmg * intel *.0036167 + wdmg *(dtr - 202) * .0010800 - 1) * 1.3 * (1 + .5 * (0.0697 * crit - 18.437) / 100);
		}

		private int SpellDamage(int wdmg, int intel, int dtr, int crit, SpellType Spell, BuffType mode, bool ThunderCloud)
		{
			return (int)(DamageValue(wdmg, intel, dtr, crit) * SpellPotency(Spell, mode, ThunderCloud));
		}

		private void CalculateWeights()
		{
			int wdmg = (int)numericUpDown5.Value;
			int intel = (int)numericUpDown1.Value;
			int dtr = (int)numericUpDown2.Value;
			int crit = (int)numericUpDown3.Value;

			double baseDmg = DamageValue(wdmg, intel, dtr, crit);
			double wdmgPlus = DamageValue(wdmg + 1, intel, dtr, crit);
			double intPlus = DamageValue(wdmg, intel + 1, dtr, crit);
			double dtrPlus = DamageValue(wdmg, intel, dtr + 1, crit);
			double critPlus = DamageValue(wdmg, intel, dtr, crit + 1);

			double wdmgDelta = wdmgPlus - baseDmg;
			double intDelta = intPlus - baseDmg;
			double dtrDelta = dtrPlus - baseDmg;
			double critDelta = critPlus - baseDmg;

			double wdmgWeight = wdmgDelta / intDelta;
			double intWeight = intDelta / intDelta;
			double dtrWeight = dtrDelta / intDelta;
			double critWeight = critDelta / intDelta;

			textBox1.Text = intWeight.ToString();
			textBox2.Text = dtrWeight.ToString();
			textBox3.Text = critWeight.ToString();
			textBox6.Text = wdmgWeight.ToString();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			CalculateWeights();
		}

		private double GCDSpeed(int speed)
		{
			return 2.5 - (speed - 341) / 100;
		}

		private double CastSpeed(SpellType spell, BuffType mode) {
			if (spell == SpellType.Fire3) {
				if (mode == BuffType.UI3) {
					return 3.0;
				}
				return 3.0;
			}
			return 0.0;
		}

		private double RunSim(int wdmg, int intel, int dtr, int crit, int spd)
		{
			double totalDmg = 0;
			double currentTime = 0;
			bool ThunderCloud = false;
			BuffType mode = BuffType.AF3;
			bool FireStarter = false;
			bool nextSpell = false;
			int currentMP = 0;
			int MPMax = 5;
			int MPtics = 0;
			int MPticsMax = 2;
			double ThunderDuration = 0;
			double castTime = 0;
			double castComplete = 0;
			double GCD = 0;
			SpellType castingSpell = SpellType.None;
			bool regenThunder = false;

			do
			{
				// Decide what action we're taking
				if (currentTime <= castComplete && GCD <= 0) // If we're casting or on GCD, nothing else we can do.
				{
					if (currentTime == castComplete && castingSpell != SpellType.None) // If we just finished casting a spell, process the damage.
					{
						totalDmg += SpellDamage(wdmg, intel, dtr, crit, castingSpell, mode, false);
						if (castingSpell == SpellType.Fire3)
						{
							mode = BuffType.AF3;
						}
						if (castingSpell == SpellType.Blizzard3) 
						{
							mode = BuffType.UI3;
							regenThunder = false;
						}
						if (castingSpell == SpellType.Thunder2) {ThunderDuration = 21;}
					}
					if (ThunderCloud) // If we have a Thundercloud, use it.
					{
						totalDmg += SpellDamage(wdmg, intel, dtr, crit, SpellType.Thunder3, mode, ThunderCloud);
						ThunderCloud = false;
						ThunderDuration = 24;
						GCD = GCDSpeed(spd);
						castingSpell = SpellType.None;
						if (mode == BuffType.UI3)
						{
							regenThunder = true; // if we were recovering MP, note that we got our thunder in.
						}
					}
					if (FireStarter && nextSpell && (mode == BuffType.AF3 || currentMP == MPMax)) // Use Firestarters if we're in fire mode, or we finished the regen phase.
					{
						if (mode == BuffType.UI3) mode = BuffType.AF; // simulate Transpose -> AF1 before tossing firestarter
						totalDmg += SpellDamage(wdmg, intel, dtr, crit, SpellType.Fire3, mode, ThunderCloud);
						nextSpell = false;
						FireStarter = false;
						castingSpell = SpellType.None;
						GCD = GCDSpeed(spd);
					}
					if (currentMP == 0 && mode == BuffType.AF3) // If we're out of MP and in astral fire, cast blizzard 3
					{

					}
					if (mode == BuffType.UI3 && !regenThunder) // If we're recovering MP and haven't cast thunder yet, do so.
					{
					}
					if (mode == BuffType.UI3 && regenThunder && MPtics >= 1 && (currentTime + CastSpeed(SpellType.Fire3, mode)) >= GetNextServerTick(currentTime)) // If we got our thunder in, and will finish casting Fire 3 after we get our next MP tick, begin casting.
					{
					}
				}
			} while (currentTime < _simLength);
			return totalDmg;
		}
	}
}
