using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Test
{
	class Program
	{
		private static int _simMinutes = 13;
		private static int simIterations = 25;
		private static Random _rng;
		private static double critmod = 0.0;
		private static int _missedThunderClouds = 0;
		private static int _missedFireStarters = 0;
		private static double _deadTime = 0.0;
		private static bool _experimental = false;
		private static bool _fireWeave = false;
		private static int _thunderTicsClipped = 0;
		private static bool _standardBLM = true;
		private static int _totalSims = 0;
		private static int _bloodLetterProcs = 0;
		private static int _baseBloodLetterProcs = 0;
		private static int _critBloodLetterProcs = 0;
		private const string _logFile = @"C:\users\tporth\Documents\Side Projects\Test.log";

		static void Main(string[] args)
		{
			double baseDmg = 0.0;
			double baseDmgNoIgnore = 0.0;
			double wDmg = 0.0;
			double wDmgNoIgnore = 0.0;
			double statDmg = 0.0;
			double statDmgNoIgnore = 0.0;
			double critDmg = 0.0;
			double critDmgNoIgnore = 0.0;
			double dtrDmg = 0.0;
			double dtrDmgNoIgnore = 0.0;
			double spdDiffDmg = 0.0;
            double spdDiffDmgNoIgnore = 0.0;
            double spdNormDmg = 0.0;
            double spdNormStatDmg = 0.0;
            double spdNormDmgNoIgnore = 0.0;
            double spdNormStatDmgNoIgnore = 0.0;
            double spdMinDmg = 0.0;
            double spdMinStatDmg = 0.0;
            double spdMinDmgNoIgnore = 0.0;
            double spdMinStatDmgNoIgnore = 0.0;
            double spdMinNormDmg = 0.0;
            double spdMinNormStatDmg = 0.0;
            double spdMinNormDmgNoIgnore = 0.0;
            double spdMinNormStatDmgNoIgnore = 0.0;
			int spdDiffGCDs = 0;
			int spdDiffGCDsNoIgnore = 0;
			int spdNormGCDs = 0;
            int spdNormGCDsNoIgnore = 0;
            int spdMinGCDs = 0;
            int spdMinGCDsNoIgnore = 0;
            int spdMinNormGCDs = 0;
            int spdMinNormGCDsNoIgnore = 0;
			int baseGCDs = 0;
			int baseGCDsNoIgnore = 0;
			int spdDiff = 0;
			int spdDiffNoIgnore = 0;
            int spdNorm = 0;
            int spdMin = 0;
            int spdMinNoIgnore = 0;
            int spdMinNorm = 0;
            int spdMinNormNoIgnore = 0;
			int spdNormNoIgnore = 0;
			string job = "";

			int wdmg, stat, crit, dtr, spd;

			do
			{
				Console.Write("Job: ");
				job = Console.ReadLine();

				spdDiff = 0;
				spdNorm = 0;
                spdMin = 0;
				spdDiffNoIgnore = 0;
				spdNormNoIgnore = 0;
                spdMinNoIgnore = 0;
				spdDiffDmgNoIgnore = 0.0;
				baseDmgNoIgnore = 0;

				switch (job.ToUpper())
				{
					case "BARD":
					case "BRD":
						job = "BRD";

						wdmg = 48;
						stat = 574; //574, 571
						crit = 568; //568, 591
						dtr = 321; //321, 309
						spd = 402;

						_bloodLetterProcs = 0;
						_totalSims = 0;

						baseDmg = BRDSim(wdmg, stat, crit, dtr, spd, out baseGCDs, true);
						_baseBloodLetterProcs = _bloodLetterProcs;
						wDmg = BRDSim(wdmg + 1, stat, crit, dtr, spd, true);
						statDmg = BRDSim(wdmg, stat + 1, crit, dtr, spd, true);
						_bloodLetterProcs = 0;
						critDmg = BRDSim(wdmg, stat, crit + 1, dtr, spd, true);
						_critBloodLetterProcs = _bloodLetterProcs;
						dtrDmg = BRDSim(wdmg, stat, crit, dtr + 1, spd, true);
						do
						{
							spdNorm++;
							spdNormDmg = BRDSim(wdmg, stat, crit, dtr, spd - spdNorm, out spdNormGCDs, true);
						} while (spdNormGCDs >= baseGCDs || spdNormDmg >= baseDmg);
						spdNormStatDmg = BRDSim(wdmg, stat + 1, crit, dtr, spd - spdNorm, true);
						do
						{
							spdDiff++;
							spdDiffDmg = BRDSim(wdmg, stat, crit, dtr, spd + spdDiff, out spdDiffGCDs, true);
                        } while (spdDiffGCDs <= baseGCDs || spdDiffDmg <= baseDmg);

						baseDmgNoIgnore = BRDSim(wdmg, stat, crit, dtr, spd, out baseGCDsNoIgnore, false);
						wDmgNoIgnore = BRDSim(wdmg + 1, stat, crit, dtr, spd, false);
						statDmgNoIgnore = BRDSim(wdmg, stat + 1, crit, dtr, spd, false);
						critDmgNoIgnore = BRDSim(wdmg, stat, crit + 1, dtr, spd, false);
						dtrDmgNoIgnore = BRDSim(wdmg, stat, crit, dtr + 1, spd, false);
						do
						{
							spdNormNoIgnore++;
							spdNormDmgNoIgnore = BRDSim(wdmg, stat, crit, dtr, spd - spdNormNoIgnore, out spdNormGCDsNoIgnore, false);
						} while ((spdNormGCDsNoIgnore >= baseGCDsNoIgnore || spdNormDmgNoIgnore >= baseDmgNoIgnore) && spdNormNoIgnore < 10);
						spdNormStatDmgNoIgnore = BRDSim(wdmg, stat + 1, crit, dtr, spd - spdNormNoIgnore, false);
						do
						{
							spdDiffNoIgnore++;
							spdDiffDmgNoIgnore = BRDSim(wdmg, stat, crit, dtr, spd + spdDiffNoIgnore, out spdDiffGCDsNoIgnore, false);
						} while ((spdDiffGCDsNoIgnore <= baseGCDsNoIgnore || spdDiffDmgNoIgnore <= baseDmgNoIgnore) && spdDiffNoIgnore < 10);
						break;
					case "DRAGOON":
					case "DRG":
						wdmg = 53;
						stat = 580;
						crit = 546;
						dtr = 328;
						spd = 429;

						baseDmg = DRGSim(wdmg, stat, crit, dtr, spd, out baseGCDs, true);
						wDmg = DRGSim(wdmg + 1, stat, crit, dtr, spd, true);
						statDmg = DRGSim(wdmg, stat + 1, crit, dtr, spd, true);
						critDmg = DRGSim(wdmg, stat, crit + 1, dtr, spd, true);
						dtrDmg = DRGSim(wdmg, stat, crit, dtr + 1, spd, true);
						do
						{
							spdNorm++;
							spdNormDmg = DRGSim(wdmg, stat, crit, dtr, spd + spdNorm, out spdNormGCDs, true);
						} while (spdNormGCDs <= baseGCDs);
						do
						{
							spdDiff++;
							spdDiffDmg = DRGSim(wdmg, stat, crit, dtr, spd + spdNorm + spdDiff, out spdDiffGCDs, true);
						} while (spdDiffGCDs <= spdNormGCDs);
                        spdNorm += spdDiff;
                        spdNormGCDs = spdDiffGCDs;
                        spdDiff = 0;
						spdNormStatDmg = DRGSim(wdmg, stat + 1, crit, dtr, spd + spdNorm, true);
						do
						{
							spdDiff++;
							spdDiffDmg = DRGSim(wdmg, stat, crit, dtr, spd + spdNorm + spdDiff, out spdDiffGCDs, true);
						} while (spdDiffGCDs <= spdNormGCDs);

						baseDmgNoIgnore = DRGSim(wdmg, stat, crit, dtr, spd, out baseGCDsNoIgnore, false);
						statDmgNoIgnore = DRGSim(wdmg, stat + 1, crit, dtr, spd);
						wDmgNoIgnore = DRGSim(wdmg + 1, stat, crit, dtr, spd, false);
						critDmgNoIgnore = DRGSim(wdmg, stat, crit + 1, dtr, spd, false);
						dtrDmgNoIgnore = DRGSim(wdmg, stat, crit, dtr + 1, spd, false);
						do
						{
							spdNormNoIgnore++;
							spdNormDmgNoIgnore = DRGSim(wdmg, stat, crit, dtr, spd + spdNormNoIgnore, out spdNormGCDsNoIgnore, false);
						} while (spdNormGCDsNoIgnore <= baseGCDsNoIgnore && spdNormNoIgnore < 10);
						do
						{
							spdDiffNoIgnore++;
							spdDiffDmgNoIgnore = DRGSim(wdmg, stat, crit, dtr, spd + spdNormNoIgnore + spdDiffNoIgnore, out spdDiffGCDsNoIgnore, false);
						} while (spdDiffGCDsNoIgnore <= spdNormGCDsNoIgnore && spdDiffNoIgnore < 10);
                        spdNormNoIgnore += spdDiffNoIgnore;
                        spdNormGCDsNoIgnore = spdDiffGCDsNoIgnore;
                        spdDiffNoIgnore = 0;
						spdNormStatDmgNoIgnore = DRGSim(wdmg, stat + 1, crit, dtr, spd + spdNormNoIgnore, false);
						do
						{
							spdDiffNoIgnore++;
							spdDiffDmgNoIgnore = DRGSim(wdmg, stat, crit, dtr, spd + spdNormNoIgnore + spdDiffNoIgnore, out spdDiffGCDsNoIgnore, false);
						} while (spdDiffGCDsNoIgnore <= spdNormGCDsNoIgnore && spdDiffNoIgnore < 10);
						break;
					case "SUMMONER":
					case "SMN":
						wdmg = 77;
						stat = 578;
						crit = 578;
						dtr = 319;
						spd = 459;

						baseDmg = SMNSim(wdmg, stat, crit, dtr, spd, out baseGCDs, true);
						wDmg = SMNSim(wdmg + 1, stat, crit, dtr, spd, true);
						statDmg = SMNSim(wdmg, stat + 1, crit, dtr, spd, true);
						critDmg = SMNSim(wdmg, stat, crit + 1, dtr, spd, true);
						dtrDmg = SMNSim(wdmg, stat, crit, dtr + 1, spd, true);
						do
						{
							spdNorm++;
							spdNormDmg = SMNSim(wdmg, stat, crit, dtr, spd - spdNorm, out spdNormGCDs, true);
						} while (spdNormGCDs >= baseGCDs || spdNormDmg >= baseDmg);
						spdNormStatDmg = SMNSim(wdmg, stat + 1, crit, dtr, spd - spdNorm, true);
						do
						{
							spdDiff++;
							spdDiffDmg = SMNSim(wdmg, stat, crit, dtr, spd + spdDiff, out spdDiffGCDs, true);
                        } while (spdDiffGCDs <= baseGCDs || spdDiffDmg <= baseDmg);

						baseDmgNoIgnore = SMNSim(wdmg, stat, crit, dtr, spd, out baseGCDsNoIgnore, false);
						statDmgNoIgnore = SMNSim(wdmg, stat + 1, crit, dtr, spd);
						wDmgNoIgnore = SMNSim(wdmg + 1, stat, crit, dtr, spd, false);
						critDmgNoIgnore = SMNSim(wdmg, stat, crit + 1, dtr, spd, false);
						dtrDmgNoIgnore = SMNSim(wdmg, stat, crit, dtr + 1, spd, false);
						do
						{
							spdNormNoIgnore++;
							spdNormDmgNoIgnore = SMNSim(wdmg, stat, crit, dtr, spd - spdNormNoIgnore, out spdNormGCDsNoIgnore, false);
						} while ((spdNormGCDsNoIgnore >= baseGCDsNoIgnore || spdNormDmgNoIgnore >= baseDmgNoIgnore) && spdNormNoIgnore < 10);
						spdNormStatDmgNoIgnore = SMNSim(wdmg, stat + 1, crit, dtr, spd - spdNormNoIgnore, false);
                        do
                        {
                            spdDiffNoIgnore++;
                            spdDiffDmgNoIgnore = SMNSim(wdmg, stat, crit, dtr, spd + spdDiffNoIgnore, out spdDiffGCDsNoIgnore, false);
                        } while ((spdDiffGCDsNoIgnore <= baseGCDsNoIgnore || spdDiffDmgNoIgnore <= baseDmgNoIgnore) && spdDiffNoIgnore < 10);
						break;
					case "BLACK MAGE":
					case "BLM":
						job = "BLM";

						wdmg = 77;
						stat = 578;
						crit = 461;
						dtr = 340;
						spd = 510;

						_missedFireStarters = 0;
						_missedThunderClouds = 0;
						_thunderTicsClipped = 0;
						_deadTime = 0.0;

						_experimental = false;
						_fireWeave = false;
						_standardBLM = true;
						_totalSims = 0;

						baseDmg = BLMSim(wdmg, stat, crit, dtr, spd, out baseGCDs);
						wDmg = BLMSim(wdmg + 1, stat, crit, dtr, spd);
						statDmg = BLMSim(wdmg, stat + 1, crit, dtr, spd);
						critDmg = BLMSim(wdmg, stat, crit + 1, dtr, spd);
						dtrDmg = BLMSim(wdmg, stat, crit, dtr + 1, spd);
						do
						{
							spdMinNorm++;
                            spdMinNormDmg = BLMSim(wdmg, stat, crit, dtr, spd - spdMinNorm, out spdMinNormGCDs);
						} while (spdMinNormGCDs >= baseGCDs || spdMinNormDmg >= baseDmg);
						do
						{
							spdMin++;
							spdMinDmg = BLMSim(wdmg, stat, crit, dtr, spd - spdMinNorm - spdMin, out spdMinGCDs);
                        } while (spdMinGCDs >= spdMinNormGCDs || spdMinDmg >= spdMinNormDmg);
                        spdMinStatDmg = BLMSim(wdmg, stat + 1, crit, dtr, spd - spdMinNorm - spdMin);
						do
						{
							spdNorm++;
							spdNormDmg = BLMSim(wdmg, stat, crit, dtr, spd + spdNorm, out spdNormGCDs);
                        } while (spdNormGCDs <= baseGCDs || spdNormDmg <= baseDmg);
						do
						{
							spdDiff++;
							spdDiffDmg = BLMSim(wdmg, stat, crit, dtr, spd + spdNorm + spdDiff, out spdDiffGCDs);
                        } while (spdDiffGCDs <= spdNormGCDs || spdDiffDmg <= spdNormDmg);
						break;
					default:
						job = "";
						break;
				}
				if (!string.IsNullOrWhiteSpace(job))
				{
					double wdmgDelta = wDmg - baseDmg;
					double statDelta = statDmg - baseDmg;
					double critDelta = critDmg - baseDmg;
					double dtrDelta = dtrDmg - baseDmg;
                    double spdDelta = spdDiffDmg - baseDmg;

					double wdmgWeight = wdmgDelta / statDelta;
					double critweight = critDelta / statDelta;
					double dtrweight = dtrDelta / statDelta;
                    double spdWeight = spdDelta / statDelta /* (spdDiffGCDs - baseGCDs)*/ / (spdNorm + spdDiff);
                    double spdWeightDiffMin = (spdDiffDmg - spdMinDmg) / (spdMinStatDmg - spdMinDmg) / (spdNorm + spdDiff + spdMin + spdMinNorm);
                    double spdWeightMin = (baseDmg - spdMinDmg) / (spdMinStatDmg - spdMinDmg) / (spdMin + spdMinNorm);

					if (baseDmgNoIgnore != 0)
					{
						double statDeltaNoIgnore = statDmgNoIgnore - baseDmgNoIgnore;
                        double spdDeltaNoIgnore = spdDiffDmgNoIgnore - spdNormDmgNoIgnore;
						double critDeltaNoIgnore = critDmgNoIgnore - baseDmgNoIgnore;
						double dtrDeltaNoIgnore = dtrDmgNoIgnore - baseDmgNoIgnore;
						double wdmgDeltaNoIgnore = wDmgNoIgnore - baseDmgNoIgnore;
						wdmgWeight = (wdmgWeight + (wdmgDeltaNoIgnore / statDeltaNoIgnore)) / 2.0;
						critweight = (critweight + (critDeltaNoIgnore / statDeltaNoIgnore)) / 2.0;
						dtrweight = (dtrweight + (dtrDeltaNoIgnore / statDeltaNoIgnore)) / 2.0;
                        spdWeight = (spdWeight + ((spdDeltaNoIgnore / (spdNormStatDmgNoIgnore - spdNormDmgNoIgnore)) / (spdNormNoIgnore + spdDiffNoIgnore))) / 2.0;
					}

					double dps = baseDmg / (_simMinutes * 60 * simIterations);
					Console.WriteLine("Job:                  {0}", job.ToUpper());
					Console.WriteLine("Sim Length (mins):    {0}", _simMinutes);
					Console.WriteLine("Sim Iterations:       {0}", simIterations);
                    Console.WriteLine("Base Attacks:         {0}", baseGCDs);
                    Console.WriteLine("Speed Attacks:        {0}", spdDiffGCDs);
                    Console.WriteLine("Speed Difference:     {0}", spdDiff + spdNorm);
                    Console.WriteLine("Speed Min Attacks:    {0}", spdMinGCDs);
                    Console.WriteLine("Speed Min Difference: {0}", spdMin + spdMinNorm);
					if (job == "BLM")
					{
						Console.WriteLine("Avg Missed Firestarters:  {0}", (double)_missedFireStarters / simIterations / _totalSims);
						Console.WriteLine("Avg Missed Thunderclouds: {0}", (double)_missedThunderClouds / simIterations / _totalSims);
						Console.WriteLine("Avg Thunder Tics Clipped: {0}", (double)_thunderTicsClipped / simIterations / _totalSims);
						Console.WriteLine("Avg Dead Time: {0}", _deadTime / simIterations / _totalSims);
					}
					else if (job == "BRD")
					{
						Console.WriteLine("Avg Bloodletter Procs (Base): {0}", (double)_baseBloodLetterProcs / simIterations);
						Console.WriteLine("Avg Bloodletter Procs (Crit): {0}", (double)_critBloodLetterProcs / simIterations);
					}
					Console.WriteLine("Estimated DPS:   {0}", dps);
					Console.WriteLine("  Weapon Damage: {0}", wDmg / (_simMinutes * 60 * simIterations));
					Console.WriteLine("  Main Stat:     {0}", statDmg / (_simMinutes * 60 * simIterations));
					Console.WriteLine("  Determination: {0}", dtrDmg / (_simMinutes * 60 * simIterations));
					Console.WriteLine("  Crit Rate:     {0}", critDmg / (_simMinutes * 60 * simIterations));
					Console.WriteLine("  Speed:         {0}", spdDiffDmg / (_simMinutes * 60 * simIterations));
					Console.WriteLine("Weights:");
					Console.WriteLine("  Weapon Damage:    {0}", wdmgWeight);
					Console.WriteLine("  Determination:    {0}", dtrweight);
					Console.WriteLine("  Crit Rate:        {0}", critweight);
                    Console.WriteLine("  Speed (DiffBase): {0}", Math.Max(spdWeight, 0));
                    Console.WriteLine("  Speed (BaseMin):  {0}", Math.Max(spdWeightMin, 0));
                    Console.WriteLine("  Speed (DiffMin):  {0}", Math.Max(spdWeightDiffMin, 0));
                    Console.WriteLine("  Speed (Avg):      {0}", Math.Max((spdWeight + spdWeightMin + spdWeightDiffMin) / 3.0, 0));
				}
				Console.WriteLine("");
			} while (!string.IsNullOrWhiteSpace(job));
		}

		private static double BLMSim(int wdmg, int intel, int crit, int dtr, int spd, out int avgCasts)
		{
			double totalDmg = 0.0;
			avgCasts = 0;
			_totalSims++;

			//File.Delete(_logFile);
			//FileStream fs = new FileStream(_logFile, FileMode.OpenOrCreate);
			//StreamWriter logWriter = new StreamWriter(fs);

			for (int i = 0; i < simIterations; i++)
			{
				_rng = new Random(i);
				
				BuffType mode = BuffType.None;
				bool nextSpell = false;
				int maxMP = (int)Math.Round(((int)(245 * 1.029) * 8.23 + 1662), 0); // Dunesfolk with SCH in party for sake of argument
				int MP = 3736;
				double ThunderDuration = 0.0;
				double ThunderCloudDuration = 0.0;
				double FireStarterDuration = 0.0;
				double thunderDmgMod = 1.0;
				double swiftRecast = 0.0;
				double swiftDuration = 0.0;
				double ragingRecast = 0.0;
				double ragingDuration = 0.0;
				double modeDuration = 0.0;
				double convertRecast = 0.0;
				double transposeRecast = 0.0;
				double sureCastRecast = 0.0, quellingRecast = 0.0, lethargyRecast = 0.0, virusRecast = 0.0;
				double manaWallRecast = 0.0, manawardRecast = 0.0, apocRecast = 0.0, eye4eyeRecast = 0.0;
				bool regenThunder = false;
				bool regenBlizzard = false;
				double spelldamage = 0.0;

				double currentTime = 0.0;
				double GCD = 0.0;
				double animationDelay = 0.0;
				double animationDelayMax = 0.5;
				double castComplete = 0.0;
				double nextTick = (double)_rng.Next(3001) / 1000.0;
				SpellType castingSpell = SpellType.None;
				SpellType lastSpell = SpellType.None;
				SpellType prevSpell = SpellType.None;
				SpellType forcedThunder = SpellType.None;

				#region Janky priority style
				int priority = 0;
				#endregion
				do
				{
					if (animationDelay <= 0)
					{
						if (currentTime >= castComplete && castingSpell != SpellType.None) // If we just finished casting a spell, process the damage.
						{
							spelldamage = SpellDamage(wdmg, intel, dtr, crit, castingSpell, mode, false);
							//logWriter.WriteLine(currentTime + " - " + castingSpell.ToString() + ": " + spelldamage);
							totalDmg += spelldamage;
							if (castingSpell == SpellType.Flare)
							{
								MP = 0;
								mode = BuffType.AF3;
								modeDuration = 10.0;
							}
							else
							{
								MP -= MPCost(castingSpell, mode);
								//logWriter.WriteLine("\tMP: " + MP);
							}
							if (FireStarterDuration > 0)
							{
								nextSpell = true;
							}
							if (castingSpell == SpellType.Fire3)
							{
								mode = BuffType.AF3;
								modeDuration = 10.0;
								if (priority == 0)
								{
									priority = 1;
								}
								else if (ragingRecast < GCDSpeed(spd) && convertRecast < GCDSpeed(spd) * 6 && swiftRecast < GCDSpeed(spd) * 7)
								{
									priority = 2;
								}
								else if (swiftRecast < GCDSpeed(spd) * 5)
								{
									priority = 3;
								}
								else
								{
									priority = 4;
								}
							}
							else if (castingSpell == SpellType.Blizzard3)
							{
								mode = BuffType.UI3;
								regenThunder = false;
								regenBlizzard = false;
								modeDuration = 10.0;
							}
							else if (castingSpell == SpellType.Blizzard)
							{
								if (mode == BuffType.UI3 || mode == BuffType.UI2 || mode == BuffType.UI)
								{
									regenBlizzard = true;
									modeDuration = 10.0;
									if (mode == BuffType.UI2)
									{
										mode = BuffType.UI3;
									}
									else if (mode == BuffType.UI)
									{
										mode = BuffType.UI2;
									}
								}
								else if (mode == BuffType.None)
								{
									mode = BuffType.UI;
									modeDuration = 10.0;
								}
								else
								{
									mode = BuffType.None;
								}
							}
							else if (castingSpell == SpellType.Thunder || castingSpell == SpellType.Thunder2 || castingSpell == SpellType.Thunder3)
							{
								if (ThunderDuration > 0)
								{
									double tempDuration = ThunderDuration - (nextTick - currentTime);
									if (tempDuration >= 0)
									{
										int tics = 1 + (int)(tempDuration / 3);
										_thunderTicsClipped += tics;
									}
								}
								ThunderDuration = 18 + (castingSpell == SpellType.Thunder2 ? 3 : (castingSpell == SpellType.Thunder3 ? 6 : 0));
								if (mode == BuffType.UI3 || mode == BuffType.UI2 || mode == BuffType.UI)
								{
									regenThunder = true;
								}
							}
							else if (castingSpell == SpellType.Fire)
							{
								if (_rng.Next(10) < 4)
								{
									if (FireStarterDuration > 0)
									{
										_missedFireStarters++;
									}
									//logWriter.WriteLine("\t(Firestarter Proc)");
									FireStarterDuration = 12.0;
								}
								if (mode == BuffType.AF || mode == BuffType.AF2 || mode == BuffType.AF3)
								{
									modeDuration = 10.0;
									if (mode == BuffType.AF2)
									{
										mode = BuffType.AF3;
									}
									else if (mode == BuffType.AF)
									{
										mode = BuffType.AF2;
									}
								}
								else if (mode == BuffType.None)
								{
									mode = BuffType.AF;
									modeDuration = 10.0;
								}
								else
								{
									mode = BuffType.None;
								}
							}
							avgCasts++;
							prevSpell = lastSpell;
							lastSpell = castingSpell;
							castingSpell = SpellType.None;
						}

						if (currentTime >= castComplete && GCD <= 0) // If we're done casting and the GCD is over, pick an action
						{
							if (_standardBLM)
							{
								if (ThunderCloudDuration > 0 && (ThunderCloudDuration < GCDSpeed(spd) || ThunderDuration < GCDSpeed(spd) || (ragingDuration > 0 && ragingDuration < GCDSpeed(spd))))// ||
								//(prevSpell == SpellType.Fire3 && lastSpell == SpellType.Fire))) // the prev/last spell bit is to try fishing for firestarters to see how that compares with just waiting out the timer
								{ // Use thunderclouds if thundercloud is about to fall off, thunder Dot is about to fall off, or raging strikes is about to fall off
									spelldamage = SpellDamage(wdmg, intel, dtr, crit, SpellType.Thunder3, mode, ThunderCloudDuration > 0) * (ragingDuration > 0.0 ? 1.2 : 1);
									//logWriter.WriteLine(currentTime + " - " + "Thunder 3 (Instant): " + spelldamage);
									totalDmg += spelldamage;
									ThunderCloudDuration = 0.0;
									thunderDmgMod = (ragingDuration > 0.0 ? 1.2 : 1);
									if (ThunderDuration > 0)
									{
										double tempDuration = ThunderDuration - (nextTick - currentTime);
										if (tempDuration >= 0)
										{
											int tics = 1 + (int)(tempDuration / 3);
											_thunderTicsClipped += tics;
										}
									}
									ThunderDuration = 24;
									GCD = GCDSpeed(spd);
									castingSpell = SpellType.None;
									prevSpell = lastSpell;
									lastSpell = SpellType.Thunder3;
									if (mode == BuffType.UI3)
									{
										regenThunder = true; // if we were recovering MP, note that we got our thunder in.
									}
									if (FireStarterDuration > 0)
									{
										nextSpell = true;
									}
									avgCasts++;
									animationDelay = animationDelayMax;
								}
								else if (mode == BuffType.None) // Opener
								{
									if (ThunderDuration <= 0)
									{
										castingSpell = SpellType.Thunder2;
									}
									else
									{
										castingSpell = SpellType.Fire3;
									}
									castComplete = currentTime + CastSpeed(spd, castingSpell, mode);
									GCD = GCDSpeed(spd);
								}
								else if (mode == BuffType.UI)
								{
									if (!regenThunder && MP >= MPCost(SpellType.Thunder2, BuffType.UI))
									{
										castingSpell = SpellType.Thunder2;
										castComplete = currentTime + CastSpeed(spd, castingSpell, mode);
										GCD = GCDSpeed(spd);
									}
									if (MP >= MPCost(SpellType.Fire3, BuffType.UI) && currentTime + CastSpeed(spd, SpellType.Fire3, mode) > nextTick)
									{
										castingSpell = SpellType.Fire3;
										castComplete = currentTime + CastSpeed(spd, castingSpell, mode);
										GCD = GCDSpeed(spd);
									}
								}
								else if (mode == BuffType.UI3)
								{
									// *** EXPERIMENTAL - Use extra swiftcasts not needed for SC Flare -> Convert to do B3 -> B1 -> SC T3 as regen phase
									if (_experimental && MP > MPCost(SpellType.Blizzard, BuffType.UI3) && MP < (int)(maxMP * 0.62) && swiftRecast < CastSpeed(spd, SpellType.Blizzard, BuffType.UI3) &&
										convertRecast > 60 + CastSpeed(spd, SpellType.Blizzard, BuffType.UI3))
									{ // Blizzard
										castingSpell = SpellType.Blizzard;
										castComplete = currentTime + CastSpeed(spd, castingSpell, mode);
										GCD = GCDSpeed(spd);
									}
									else if (_experimental && regenBlizzard && ThunderCloudDuration < 0 && swiftRecast <= 0.0 & convertRecast > 60.0)
									{ // Swiftcast
										swiftRecast = 60.0;
										swiftDuration = 10.0;
										animationDelay = animationDelayMax;
									}
									else if (_experimental && swiftDuration > 0.0 && ThunderCloudDuration < 0)
									{ // Thunder 3
										spelldamage = SpellDamage(wdmg, intel, dtr, crit, SpellType.Thunder3, mode, ThunderCloudDuration > 0) * (ragingDuration > 0.0 ? 1.2 : 1);
										totalDmg += spelldamage;
										MP -= MPCost(SpellType.Thunder3, mode);
										thunderDmgMod = (ragingDuration > 0.0 ? 1.2 : 1);
										if (ThunderDuration > 0)
										{
											double tempDuration = ThunderDuration - (nextTick - currentTime);
											if (tempDuration >= 0)
											{
												int tics = 1 + (int)(tempDuration / 3);
												_thunderTicsClipped += tics;
											}
										}
										ThunderDuration = 24;
										swiftDuration = 0.0;
										GCD = GCDSpeed(spd);
										castingSpell = SpellType.None;
										prevSpell = lastSpell;
										lastSpell = SpellType.Thunder3;
										regenThunder = true; // if we were recovering MP, note that we got our thunder in.
										if (FireStarterDuration > 0)
										{
											nextSpell = true;
										}
										avgCasts++;
										animationDelay = animationDelayMax;
									}
									else if (_experimental && swiftDuration > 0.0 && ThunderCloudDuration > 0)
									{ // If a thundercloud proced just before we would have swiftcast... Insta-Blizzard
										spelldamage = SpellDamage(wdmg, intel, dtr, crit, SpellType.Blizzard3, mode, false);
										totalDmg += spelldamage;
										MP -= MPCost(SpellType.Blizzard, mode);
										swiftDuration = 0.0;
										GCD = GCDSpeed(spd);
										castingSpell = SpellType.None;
										prevSpell = lastSpell;
										lastSpell = SpellType.Blizzard;
										regenBlizzard = true;
										if (FireStarterDuration > 0)
										{
											nextSpell = true;
										}
										avgCasts++;
										animationDelay = animationDelayMax;
									}
									// ***

									/* Potential conditions for using a thunder:
									 * 1. Just do it if not done yet
									 * 2. Expected value of thunder (initial DMG + expected Dot DMG - overwritten DoT Damage) > Blizzard value
									 * 3. Applicable to all, don't use if thundercloud
									 */
									else if (ThunderCloudDuration <= 0 &&
										//!regenThunder) // 1. Just do it if not done yet
										//2. Expected value of thunder (either forced or whichever we have the MP for) - lost DoT damage due to clipping > Blizzard
										(forcedThunder != SpellType.None ?
											SpellDamage(wdmg, intel, dtr, crit, forcedThunder, mode, false) * (ragingDuration > CastSpeed(spd, forcedThunder, mode) ? 1.2 : 1) +
												(forcedThunder == SpellType.Thunder ? 6 : 7) * SpellDamage(wdmg, intel, dtr, crit, SpellType.ThunderDoT, mode, false) * (ragingDuration >= 0 ? 1.2 : 1) :
											(MP > MPCost(SpellType.Thunder2, mode) ?
												SpellDamage(wdmg, intel, dtr, crit, SpellType.Thunder2, mode, false) * (ragingDuration >= CastSpeed(spd, SpellType.Thunder2, mode) ? 1.2 : 1) +
													SpellDamage(wdmg, intel, dtr, crit, SpellType.ThunderDoT, mode, false) * 7 * (ragingDuration >= CastSpeed(spd, SpellType.Thunder2, mode) ? 1.2 : 1) :
												SpellDamage(wdmg, intel, dtr, crit, SpellType.Thunder, mode, false) * (ragingDuration >= CastSpeed(spd, SpellType.Thunder, mode) ? 1.2 : 1) +
													SpellDamage(wdmg, intel, dtr, crit, SpellType.ThunderDoT, mode, false) * 6 * (ragingDuration >= CastSpeed(spd, SpellType.Thunder, mode) ? 1.2 : 1))) -
										ThunderDuration / 3 * SpellDamage(wdmg, intel, dtr, crit, SpellType.ThunderDoT, mode, false) * thunderDmgMod >
										SpellDamage(wdmg, intel, dtr, crit, SpellType.Blizzard, mode, false))
									{ // Thunder 1 or 2
										if (forcedThunder != SpellType.Thunder && MP >= MPCost(SpellType.Thunder2, mode) && !(MP > (int)(maxMP * 0.62) && currentTime + CastSpeed(spd, SpellType.Thunder, mode) < nextTick))
										{
											castingSpell = SpellType.Thunder2;
											castComplete = currentTime + CastSpeed(spd, castingSpell, mode);
										}
										else if (forcedThunder == SpellType.Thunder || MP >= MPCost(SpellType.Thunder, mode))
										{
											castingSpell = SpellType.Thunder;
											castComplete = currentTime + CastSpeed(spd, castingSpell, mode);
										}
										GCD = GCDSpeed(spd);
									}
									else if ((maxMP - (MP - MPCost(SpellType.Scathe, BuffType.UI3))) < (int)(maxMP * 0.62) && FireStarterDuration > 0 && currentTime + GCDSpeed(spd) > nextTick + animationDelayMax)
									{ // Scathe
										spelldamage = SpellDamage(wdmg, intel, dtr, crit, SpellType.Scathe, mode, ThunderCloudDuration > 0) * (ragingDuration > 0.0 ? 1.2 : 1);
										//logWriter.WriteLine(currentTime + " - " + "Scathe: " + spelldamage);
										totalDmg += spelldamage;
										MP -= MPCost(SpellType.Scathe, mode);
										//logWriter.WriteLine("\tMP: " + MP);
										GCD = GCDSpeed(spd);
										castingSpell = SpellType.None;
										prevSpell = lastSpell;
										lastSpell = SpellType.Scathe;
										nextSpell = true;
										animationDelay = animationDelayMax;
									}
									else if (FireStarterDuration < 0 && ((maxMP - MP) < (int)(maxMP * 0.62) && currentTime + CastSpeed(spd, SpellType.Fire3, BuffType.UI3) > nextTick))
									{ // Fire 3
										castingSpell = SpellType.Fire3;
										castComplete = currentTime + CastSpeed(spd, castingSpell, mode);
										GCD = GCDSpeed(spd);
									}
									else if (MP < (int)(maxMP * 0.62) || ((maxMP - (MP - MPCost(SpellType.Blizzard, BuffType.UI3))) < (int)(maxMP * 0.62) && currentTime + CastSpeed(spd, SpellType.Blizzard, BuffType.UI3) < nextTick
										//&& ((SpellDamage(wdmg,intel,dtr,crit,SpellType.Blizzard,BuffType.UI3,false)+SpellDamage(wdmg,intel,dtr,crit,SpellType.Fire3,BuffType.UI3,false))/(CastSpeed(spd,SpellType.Blizzard,BuffType.UI3) + CastSpeed(spd,SpellType.Fire3,BuffType.UI3))) > 
										//    (SpellDamage(wdmg,intel,dtr,crit,SpellType.Fire3,BuffType.UI3,false)/(CastSpeed(spd,SpellType.Fire3,BuffType.UI3) + Math.Max(0,nextTick - currentTime - CastSpeed(spd,SpellType.Fire3,BuffType.UI3)))
										//+ ((SpellDamage(wdmg,intel,dtr,crit,SpellType.Fire,BuffType.AF3,false) * (CastSpeed(spd, SpellType.Blizzard, BuffType.UI3) - Math.Max(0,nextTick - currentTime - CastSpeed(spd,SpellType.Fire3,BuffType.UI3))))/Math.Pow(CastSpeed(spd,SpellType.Fire,BuffType.AF3),2))
										//)
										))
									{ // Blizzard
										castingSpell = SpellType.Blizzard;
										castComplete = currentTime + CastSpeed(spd, castingSpell, mode);
										GCD = GCDSpeed(spd);
									}
								}
								else // AF1 or AF3
								{
									if (mode == BuffType.AF3 && MP > MPCost(SpellType.Fire, BuffType.AF3) * 4 + MPCost(SpellType.Flare, BuffType.AF3) && ragingRecast <= 0 && convertRecast < GCDSpeed(spd) * 4)
									{ // Raging Strikes
										ragingRecast = 180.0;
										ragingDuration = 20.0;
										animationDelay = animationDelayMax;
										if (FireStarterDuration > 0)
										{
											nextSpell = true;
										}
									}
									else if (MP < MPCost(SpellType.Fire, BuffType.AF3) && MP >= MPCost(SpellType.Flare, BuffType.AF3) && swiftRecast <= 0 &&
										(ragingRecast > 0 && convertRecast < GCDSpeed(spd) - animationDelayMax /*|| convertRecast > 60*/))
									{ // Swiftcast (for Flare)
										swiftRecast = 60.0;
										swiftDuration = 10.0;
										animationDelay = animationDelayMax;
									}
									else if (MP > MPCost(SpellType.Flare, BuffType.AF3) && swiftDuration > 0 && convertRecast < GCDSpeed(spd) - animationDelayMax)
									{ // Flare
										spelldamage = SpellDamage(wdmg, intel, dtr, crit, SpellType.Flare, mode, ThunderCloudDuration > 0) * (ragingDuration > 0.0 ? 1.2 : 1);
										//logWriter.WriteLine(currentTime + " - " + "Flare (Instant): " + spelldamage);
										totalDmg += spelldamage;
										MP = 0;
										animationDelay = animationDelayMax;
										GCD = GCDSpeed(spd);
										swiftDuration = 0.0;
										if (FireStarterDuration > 0)
										{
											nextSpell = true;
										}
										castingSpell = SpellType.None;
										prevSpell = lastSpell;
										lastSpell = SpellType.Flare;
										avgCasts++;
										modeDuration = 10.0;
										mode = BuffType.AF3;
									}
									else if (FireStarterDuration > 0 && nextSpell)
									{ // Fire 3 (Instant)
										GCD = GCDSpeed(spd);
										spelldamage = SpellDamage(wdmg, intel, dtr, crit, SpellType.Fire3, mode, ThunderCloudDuration > 0) * (ragingDuration > 0.0 ? 1.2 : 1);
										//logWriter.WriteLine(currentTime + " - " + "Fire3 (Instant): " + spelldamage);
										totalDmg += spelldamage;
										nextSpell = false;
										FireStarterDuration = 0;
										castingSpell = SpellType.None;
										prevSpell = lastSpell;
										lastSpell = SpellType.Fire3;
										avgCasts++;
										GCD = GCDSpeed(spd);
										animationDelay = animationDelayMax;
										modeDuration = 10.0;
										mode = BuffType.AF3;
									}
									else if (MP > MPCost(SpellType.Fire, BuffType.AF3) + MPCost(SpellType.Blizzard3, BuffType.AF3))
									{ // Fire
										if (swiftDuration < 0)
										{
											castingSpell = SpellType.Fire;
											castComplete = currentTime + CastSpeed(spd, castingSpell, mode);
										}
										else
										{
											spelldamage = SpellDamage(wdmg, intel, dtr, crit, SpellType.Fire, mode, ThunderCloudDuration > 0) * (ragingDuration > 0.0 ? 1.2 : 1);
											//logWriter.WriteLine(currentTime + " - " + "Fire (Instant): " + spelldamage);
											totalDmg += spelldamage;
											MP -= MPCost(SpellType.Fire, mode);
											//logWriter.WriteLine("\tMP: " + MP);
											castingSpell = SpellType.None;
											prevSpell = lastSpell;
											lastSpell = SpellType.Fire;
											swiftDuration = 0;
											if (FireStarterDuration > 0)
											{
												nextSpell = true;
											}
											if (_rng.Next(10) < 4)
											{
												if (FireStarterDuration > 0)
												{
													_missedFireStarters++;
												}
												//logWriter.WriteLine("\t(Firestarter Proc)");
												FireStarterDuration = 12.0;
											}
											castingSpell = SpellType.None;
											prevSpell = lastSpell;
											lastSpell = SpellType.Fire;
											avgCasts++;
											animationDelay = animationDelayMax;
											modeDuration = 10.0;
										}
										GCD = GCDSpeed(spd);
									}
									else if (_fireWeave && lastSpell == SpellType.Fire && (FireStarterDuration < 0 || (FireStarterDuration > 0 && !nextSpell)) && MP > MPCost(SpellType.Blizzard3, BuffType.AF3) &&
										(sureCastRecast <= 0 || quellingRecast <= 0 || lethargyRecast <= 0 || virusRecast <= 0 ||
										manaWallRecast <= 0 || manawardRecast <= 0 || apocRecast <= 0 || eye4eyeRecast <= 0))
									{
										if (sureCastRecast <= 0.0)
										{
											sureCastRecast = 30;
										}
										else if (lethargyRecast <= 0.0)
										{
											lethargyRecast = 30;
										}
										else if (virusRecast <= 0.0)
										{
											virusRecast = 90;
										}
										else if (quellingRecast <= 0.0)
										{
											quellingRecast = 120;
										}
										else if (manawardRecast <= 0.0)
										{
											manawardRecast = 120;
										}
										else if (manaWallRecast <= 0.0)
										{
											manaWallRecast = 120;
										}
										else if (apocRecast <= 0.0)
										{
											apocRecast = 180;
										}
										else if (eye4eyeRecast <= 0.0)
										{
											eye4eyeRecast = 180;
										}

										if (FireStarterDuration > 0)
										{
											nextSpell = true;
										}
										animationDelay = animationDelayMax;
									}
									else if (MP > MPCost(SpellType.Blizzard3, BuffType.AF3))
									{ // Blizzard 3
										castingSpell = SpellType.Blizzard3;
										castComplete = currentTime + CastSpeed(spd, castingSpell, mode);
										GCD = GCDSpeed(spd);
									}
								}
							}
							else // Death Lotus' janked up rotation
							{
								if (ThunderCloudDuration > 0 && (ThunderCloudDuration < GCDSpeed(spd) || ThunderDuration < GCDSpeed(spd) || (ragingDuration > 0 && ragingDuration < GCDSpeed(spd))))// ||
								//(prevSpell == SpellType.Fire3 && lastSpell == SpellType.Fire))) // the prev/last spell bit is to try fishing for firestarters to see how that compares with just waiting out the timer
								{ // Use thunderclouds if thundercloud is about to fall off, thunder Dot is about to fall off, or raging strikes is about to fall off
									spelldamage = SpellDamage(wdmg, intel, dtr, crit, SpellType.Thunder3, mode, ThunderCloudDuration > 0) * (ragingDuration > 0.0 ? 1.2 : 1);
									totalDmg += spelldamage;
									ThunderCloudDuration = 0.0;
									thunderDmgMod = (ragingDuration > 0.0 ? 1.2 : 1);
									if (ThunderDuration > 0)
									{
										double tempDuration = ThunderDuration - (nextTick - currentTime);
										if (tempDuration >= 0)
										{
											int tics = 1 + (int)(tempDuration / 3);
											_thunderTicsClipped += tics;
										}
									}
									ThunderDuration = 24;
									GCD = GCDSpeed(spd);
									castingSpell = SpellType.None;
									prevSpell = lastSpell;
									lastSpell = SpellType.Thunder3;
									if (mode == BuffType.UI3 || mode == BuffType.UI2 || mode == BuffType.UI)
									{
										regenThunder = true; // if we were recovering MP, note that we got our thunder in.
									}
									if (FireStarterDuration > 0)
									{
										nextSpell = true;
									}
									avgCasts++;
									animationDelay = animationDelayMax;
								}
								/*
								 * Opening Rotation – Priority 1
								 * Fire 3 -> Fire 1 -> Thunder ½ (depends on mana) -> (Be sure to use procs) -> Fire 1 -> Raging Strikes -> (Be sure to use procs) -> Fire 1 -> Fire 1 -> Hardcast flare -> (Be sure to use procs) -> Convert -> Fire 1 -> Swiftcast flare -> (Be sure to use procs) -> Transpose -> Blizzard 1 -> Thunder 2 ->
								 * 
								 * Rotation w/ Swiftcast + Raging Strikes + Convert up – Priority 2
								 * Fire 3 -> Fire 1 -> Raging Strikes -> (Be sure to use procs) -> Fire 1 -> Fire 1 -> Fire 1 -> Fire 1 -> Hardcast flare -> (Be sure to use procs) -> Convert -> Fire 1 -> Swiftcast flare -> (Be sure to use procs) -> Transpose -> Blizzard 1 -> Thunder 2 ->
								 * 
								 * Rotation w/ only Swiftcast up – Priority 3
								 * Fire 3 -> Fire 1 x5 -> Swiftcast Flare -> Transpose -> Blizzard 1 -> Thunder 2 ->
								 * 
								 * Rotation w/ a Firestarter proc on the second to last fire (i.e. the fourth one) – Priority 4
								 * Fire 3 -> Fire 1 x3 -> Fire 1 number 4 is cast -> Start casting Fire 1 number 5 -> (Oh boy I have a proc!) -> Finish casting Fire 1 number 5 -> Hardcast flare -> Use proc -> Transpose -> Blizzard 1 -> Thunder 2 ->
								 * 
								 * Rotation when you have nothing up – Priority 5
								 * Fire 3 -> Fire 1 x5 -> Blizzard 3 -> Thunder 2 -> (you can do blizzard 1 / scathe here if that’s your thing although it's slightly suboptimal) -> 
								 */
								else if ((mode == BuffType.AF || mode == BuffType.AF3) && FireStarterDuration > 0 && nextSpell && (MP >= MPCost(SpellType.Fire, BuffType.AF3) || MP == 0))
								{ // Fire 3 (Instant)
									spelldamage = SpellDamage(wdmg, intel, dtr, crit, SpellType.Fire3, mode, ThunderCloudDuration > 0) * (ragingDuration > 0.0 ? 1.2 : 1);
									totalDmg += spelldamage;
									nextSpell = false;
									FireStarterDuration = 0;
									castingSpell = SpellType.None;
									prevSpell = lastSpell;
									lastSpell = SpellType.Fire3;
									avgCasts++;
									GCD = GCDSpeed(spd);
									animationDelay = animationDelayMax;
									modeDuration = 10.0;
									mode = BuffType.AF3;
									if (MP == maxMP)
									{
										if (ragingRecast < GCDSpeed(spd) && convertRecast < GCDSpeed(spd) * 6 && swiftRecast < GCDSpeed(spd) * 7)
										{
											priority = 2;
										}
										else if (swiftRecast < GCDSpeed(spd) * 5)
										{
											priority = 3;
										}
										else
										{
											priority = 4;
										}
									}
								}
								else if ((priority == 1 && lastSpell == SpellType.Fire && prevSpell == SpellType.Fire3 && ThunderDuration <= 0) ||
									((mode == BuffType.UI2 || mode == BuffType.UI3) && !regenThunder))
								{
									castingSpell = SpellType.Thunder2;
									castComplete = currentTime + CastSpeed(spd, castingSpell, mode);
									GCD = GCDSpeed(spd);
								}
								else if (FireStarterDuration <= 0 && (MP == maxMP || (Math.Min(MP + MPTicAmt(maxMP, mode), maxMP) == maxMP && nextTick < currentTime + CastSpeed(spd, SpellType.Fire3, mode))) && !(mode == BuffType.AF || mode == BuffType.AF2 || mode == BuffType.AF3))
								{
									castingSpell = SpellType.Fire3;
									castComplete = currentTime + CastSpeed(spd, castingSpell, mode);
									GCD = GCDSpeed(spd);
								}
								else if (priority <= 2 && lastSpell == SpellType.Fire && ragingRecast <= 0.0)
								{
									ragingRecast = 180.0;
									ragingDuration = 20.0;
									animationDelay = animationDelayMax;
									if (FireStarterDuration > 0)
									{
										nextSpell = true;
									}
								}
								else if (mode == BuffType.AF3 && MP > MPCost(SpellType.Fire, mode))
								{
									castingSpell = SpellType.Fire;
									castComplete = currentTime + CastSpeed(spd, castingSpell, mode);
									GCD = GCDSpeed(spd);
								}
								else if (mode == BuffType.AF3 && MP >= MPCost(SpellType.Flare, BuffType.AF3) && swiftRecast <= 0 && convertRecast > 0)
								{ // Swiftcast (for Flare)
									swiftRecast = 60.0;
									swiftDuration = 10.0;
									animationDelay = animationDelayMax;
								}
								else if (mode == BuffType.AF3 && MP >= MPCost(SpellType.Flare, BuffType.AF3) && swiftDuration > 0)
								{ // Flare (Swiftcasted)
									spelldamage = SpellDamage(wdmg, intel, dtr, crit, SpellType.Flare, mode, ThunderCloudDuration > 0) * (ragingDuration > 0.0 ? 1.2 : 1);
									totalDmg += spelldamage;
									MP = 0;
									animationDelay = animationDelayMax;
									GCD = GCDSpeed(spd);
									swiftDuration = 0.0;
									if (FireStarterDuration > 0)
									{
										nextSpell = true;
									}
									castingSpell = SpellType.None;
									prevSpell = lastSpell;
									lastSpell = SpellType.Flare;
									avgCasts++;
									modeDuration = 10.0;
									mode = BuffType.AF3;
								}
								else if (mode == BuffType.AF3 && MP >= MPCost(SpellType.Flare, mode) && ((priority <= 2 && convertRecast <= CastSpeed(spd, SpellType.Flare, mode)) || (FireStarterDuration > CastSpeed(spd, SpellType.Flare, mode) && nextSpell)))
								{
									castingSpell = SpellType.Flare;
									castComplete = currentTime + CastSpeed(spd, castingSpell, mode);
									GCD = GCDSpeed(spd);
								}
								else if (mode == BuffType.AF3 && MP > MPCost(SpellType.Blizzard3, mode))
								{
									castingSpell = SpellType.Blizzard3;
									castComplete = currentTime + CastSpeed(spd, castingSpell, mode);
									GCD = GCDSpeed(spd);
								}
								else if (mode == BuffType.UI && !regenBlizzard)
								{
									castingSpell = SpellType.Blizzard;
									castComplete = currentTime + CastSpeed(spd, castingSpell, mode);
									GCD = GCDSpeed(spd);
								}
							}
							if (GCD <= 0 && animationDelay <= 0)
							{
								_deadTime += 0.001;
							}
						}
						// Off-GCD actions while not actively casting a spell
						if (castingSpell == SpellType.None)
						{
							if (MP == maxMP && FireStarterDuration > 0 && (mode == BuffType.UI3 || mode == BuffType.UI2 || mode == BuffType.UI))
							{ // Transpose out of UI3 before tossing a firestarter
								transposeRecast = 12.0;
								mode = BuffType.AF;
								modeDuration = 10.0;
								animationDelay = animationDelayMax;
							}
							else if (_standardBLM && mode == BuffType.AF3 && MP > MPCost(SpellType.Fire, BuffType.AF3) && lastSpell == SpellType.Fire3 && GCD > animationDelayMax && swiftRecast < 0 && convertRecast > 60 && FireStarterDuration < 0)
							{
								swiftDuration = 10.0;
								swiftRecast = 60.0;
								animationDelay = animationDelayMax;
							}
							else if (mode == BuffType.AF3 && MP < MPCost(SpellType.Blizzard3, BuffType.AF3))
							{
								// if we're out of MP, convert if we have it, otherwise transpose
								if (convertRecast <= 0 && (_standardBLM || ragingRecast > 0))
								{
									convertRecast = 180;
									MP = (int)(maxMP * 0.3);
									animationDelay = animationDelayMax;
								}
								else if (transposeRecast <= 0.0 && (_standardBLM || FireStarterDuration <= 0.0))
								{
									transposeRecast = 12.0;
									mode = BuffType.UI;
									modeDuration = 10.0;
									animationDelay = animationDelayMax;
									regenThunder = false;
									regenBlizzard = false;
								}
							}
						}
					}
					if (currentTime >= nextTick)
					{
						if (ThunderDuration >= 0)
						{
							spelldamage = SpellDamage(wdmg, intel, dtr, crit, SpellType.ThunderDoT, mode, ThunderCloudDuration > 0) * thunderDmgMod;
							totalDmg += spelldamage;
							if (_rng.Next(100) < 5)
							{
								if (ThunderCloudDuration > 0)
								{
									_missedThunderClouds++;
								}
								//logWriter.WriteLine(currentTime + " - Thundercloud Proc");
								ThunderCloudDuration = 12.0;// = true;
							}
						}
						/* MP recovery */
						MP = (int)Math.Min(MP + MPTicAmt(maxMP, mode), maxMP);
						nextTick = GetNextServerTick(nextTick);
					}

					ThunderDuration -= 0.001;
					swiftRecast -= 0.001;
					swiftDuration -= 0.001;
					convertRecast -= 0.001;
					ragingRecast -= 0.001;
					ragingDuration -= 0.001;
					transposeRecast -= 0.001;
					modeDuration -= 0.001;
					if (modeDuration <= 0.0 && mode != BuffType.None)
					{
						mode = BuffType.None;
					}

					sureCastRecast -= 0.001;
					lethargyRecast -= 0.001;
					virusRecast -= 0.001;
					manaWallRecast -= 0.001;
					manawardRecast -= 0.001;
					quellingRecast -= 0.001;
					apocRecast -= 0.001;
					eye4eyeRecast -= 0.001;

					FireStarterDuration -= 0.001;
					ThunderCloudDuration -= 0.001;

					GCD -= 0.001;
					animationDelay -= 0.001;
					currentTime += 0.001;
				} while (currentTime <= _simMinutes * 60);
			}
			avgCasts /= simIterations;
			//logWriter.Close();
			//fs.Close();
			return totalDmg;
		}

		private static int MPTicAmt(int maxMP)
		{
			return MPTicAmt(maxMP, BuffType.None);
		}
		private static int MPTicAmt(int maxMP, BuffType mode)
		{
			if (mode == BuffType.AF || mode == BuffType.AF2 || mode == BuffType.AF3)
			{
				return 0;
			}
			double recoveryPct = 0.02;
			if (mode == BuffType.UI)
			{
				recoveryPct += 0.2;
			}
			else if (mode == BuffType.UI2)
			{
				recoveryPct += 0.4;
			}
			else if (mode == BuffType.UI3)
			{
				recoveryPct += 0.6;
			}

			return (int)(maxMP * recoveryPct);
		}

		private static double GCDSpeed(int speed)
		{
			return (double)Math.Round(2.5 - (speed > 344 ? (.001952 * 3 + (speed - (341 + 3)) * .000952) : (speed - 341) * .001952), 3);
		}

		private static double BardSkillDamage(string skillName, int wdmg, int dex, int dtr, int crit, bool straighterShot)
		{
			return BardSkillDamage(skillName, wdmg, dex, dtr, crit, straighterShot, critmod);
		}
		private static double BardSkillDamage(string skillName, int wdmg, int dex, int dtr, int crit, bool straighterShot, double critModifier)
		{
			double potency;
			switch (skillName)
			{
				case "Windbite":
					potency = 60;
					break;
				case "WindDoT":
					potency = 45;
					break;
				case "Venomous Bite":
					potency = 100;
					break;
				case "VenomDoT":
					potency = 35;
					break;
				case "Straight Shot":
					potency = 140;
					break;
				case "Heavy Shot":
				case "Bloodletter":
					potency = 150;
					break;
				case "Blunt Arrow":
					potency = 50;
					break;
				case "Repelling Shot":
					potency = 80;
					break;
				case "Flaming Arrow":
					potency = 35;
					break;
				default:
					potency = 0.0;
					break;
			}
			double baseDamage = (wdmg * .2714745 + dex * .1006032 + (dtr - 202) * .0241327 + wdmg * dex * .0036167 + wdmg * (dtr - 202) * .0010800 - 1.0) * 1.2;
			if (skillName == "Straight Shot" && straighterShot)
			{
				baseDamage *= 1.5;
			}
			else
			{
				baseDamage *= (1.0 + 0.5 * (0.0697 * crit - 18.437 + critModifier) / 100.0);
			}
			double skillDamage = baseDamage * potency / 100.0;
			return skillDamage;
		}

		private static double DRGSkillDamage(string skillName, int wdmg, int str, int dtr, int crit, bool heavyThrust, bool disembowel)
		{
			return DRGSkillDamage(skillName, wdmg, str, dtr, crit, heavyThrust, disembowel, critmod, false);
		}
		private static double DRGSkillDamage(string skillName, int wdmg, int str, int dtr, int crit, bool heavyThrust, bool disembowel, bool lifeSurge)
		{
			return DRGSkillDamage(skillName, wdmg, str, dtr, crit, heavyThrust, disembowel, critmod, lifeSurge);
		}
		private static double DRGSkillDamage(string skillName, int wdmg, int str, int dtr, int crit, bool heavyThrust, bool disembowel, double CriticalModifier)
		{
			return DRGSkillDamage(skillName, wdmg, str, dtr, crit, heavyThrust, disembowel, CriticalModifier, false);
		}
		private static double DRGSkillDamage(string skillName, int wdmg, int str, int dtr, int crit, bool heavyThrust, bool disembowel, double CriticalModifier, bool lifeSurge)
		{
			double potency;
			switch (skillName)
			{
				case "Heavy Thrust":
					potency = 170;
					break;
				case "True Thrust":
					potency = 150;
					break;
				case "Vorpal Thrust":
					potency = 200;
					break;
				case "Full Thrust":
					potency = 330;
					break;
				case "Impulse Drive":
					potency = 180;
					break;
				case "Disembowel":
					potency = 220;
					break;
				case "Phlebotomize":
					potency = 170;
					break;
				case "PhlebDoT":
					potency = 25;
					break;
				case "Chaos Thrust":
					potency = 200;
					break;
				case "ChaosDoT":
					potency = 30;
					break;
				case "Jump":
					potency = 200;
					break;
				case "Spineshatter Dive":
					potency = 170;
					break;
				case "Dragonfire Dive":
					potency = 250;
					break;
				case "Leg Sweep":
					potency = 130;
					break;
				default:
					potency = 0.0;
					break;
			}
			double baseDamage = (wdmg * .2714745 + str * .1006032 + (dtr - 202) * .0241327 + wdmg * str * .0036167 + wdmg * (dtr - 202) * .0010800 - 1);
			if (!skillName.Contains("DoT"))
			{
				baseDamage *= (lifeSurge ? 1.5 : (1 + .5 * (0.0697 * crit - 18.437 + critmod) / 100));
				if (disembowel)
				{
					baseDamage = baseDamage / 0.9;
				}
				if (heavyThrust)
				{
					baseDamage *= 1.15;
				}
			}
			else
			{
				baseDamage *= (1 + .5 * (0.0697 * crit - 18.437 + critmod) / 100);
			}
			return baseDamage * potency / 100.0;
		}

		private static double SMNSkillDamage(string spellName, int wdmg, int intel, int dtr, int crit)
		{
			double potency;
			switch (spellName)
			{
				case "Miasma2DoT":
					potency = 10;
					break;
				case "Aerial Slash":
					potency = 90;
					break;
				case "Wind Blade":
					potency = 100;
					break;
				case "Miasma":
				case "Miasma II":
					potency = 20;
					break;
				case "MiasmaDoT":
				case "Bio2Dot":
					potency = 35;
					break;
				case "BioDoT":
					potency = 40;
					break;
				case "FlareDoT":
					potency = 25;
					break;
				case "Ruin":
				case "Ruin II":
					potency = 80;
					break;
				case "Fester":
					potency = 300;
					break;
				case "Energy Drain":
					potency = 150;
					break;
				default:
					potency = 0.0;
					break;
			}
			double baseDamage = (wdmg * .2714745 + intel * .1006032 + (dtr - 202) * .0241327 + wdmg * intel * .0036167 + wdmg * (dtr - 202) * .0010800 - 1) * (1 + .5 * (0.0697 * crit - 18.437) / 100) * 1.3;
			return baseDamage * potency / 100.0;
		}

		private static double GetNextServerTick(double time)
		{
			return time + 3.0;
		}

		private static double SMNSim(int wdmg, int intel, int crit, int dtr, int spd, bool ignoreResources = false)
		{
			int avgCasts = 0;
			return SMNSim(wdmg, intel, crit, dtr, spd, out avgCasts, ignoreResources);
		}
		private static double SMNSim(int wdmg, int intel, int crit, int dtr, int spd, out int avgCasts, bool ignoreResources = false)
		{
			double totalDmg = 0.0;
			avgCasts = 0;
			for (int i = 0; i < simIterations; i++)
			{
				_rng = new Random(i);

				int aetherflowStacks = 3;
				int aetherflowMax = 3;
				int currentMP = 3020;
				int mpMax = 3020;
				double Bio2Duration = 0.0;
				double MiasmaDuration = 0.0;
				double Miasma2Duration = 0.0;
				double BioDuration = 0.0;
				double shadowFlareDuration = 0.0;
				double festerRecast = 0.0;
				double energyDrainRecast = 0.0;
				double aetherflowRecast = 0.0;
				double contagionRecast = 0.0;
				double aerialSlashRecast = 0.0;
				double speedBoostDuration = 0.0;

				double RouseDuration = 0.0;
				double RouseRecast = 0.0;
				double SpurDuration = 0.0;
				double SpurRecast = 0.0;

				double Bio2Mod = 0.0;
				double MiasmaMod = 0.0;
				double Miasma2Mod = 0.0;
				double BioMod = 0.0;
				double FlareMod = 0.0;
				double ragingDuration = 0.0;
				double ragingRecast = 0.0;
				double swiftRecast = 0.0;

				double currentTime = 0.0;
				double GCD = 0.0;
				double petGCD = 0.0;
				double animationDelay = 0.0;
				double animationDelayMax = 0.5;
				double petAnimationDelay = 0.0;
				double castComplete = 0.0;
				double petCastComplete = 0.0;
				bool petActive = false;
				double nextTick = (double)_rng.Next(3001) / 1000.0;
				string castingSpell = "";
				string petSpell = "";

				do
				{
					if (animationDelay <= 0)
					{
						if (ragingRecast <= 0.0 && string.IsNullOrWhiteSpace(castingSpell))
						{
							ragingRecast = 180.0;
							ragingDuration = 20.0;
							animationDelay = animationDelayMax;
						}

						if (animationDelay <= 0.0 && currentTime >= castComplete && GCD <= 0)
						{
							if (!string.IsNullOrWhiteSpace(castingSpell))
							{ // If we were actively casting something, process it's impact
								switch (castingSpell)
								{
									case "Bio II":
										Bio2Mod = (ragingDuration > 0.0 ? 1.2 : 1.0);
										Bio2Duration = 30.0;
										currentMP -= 186;
										break;
									case "Miasma":
										MiasmaMod = (ragingDuration > 0.0 ? 1.2 : 1.0);
										totalDmg += SMNSkillDamage(castingSpell, wdmg, intel, dtr, crit) * (ragingDuration > 0.0 ? 1.2 : 1.0);
										MiasmaDuration = 24.0;
										currentMP -= 133;
										break;
									case "Shadow Flare":
										FlareMod = (ragingDuration > 0.0 ? 1.2 : 1.0);
										shadowFlareDuration = 30.0;
										currentMP -= 212;
										break;
									case "Ruin":
										totalDmg += SMNSkillDamage("Ruin", wdmg, intel, dtr, crit) * (ragingDuration > 0.0 ? 1.2 : 1.0);
										currentMP -= 79;
										break;
								}
								avgCasts++;
								castingSpell = "";
								petActive = true;
							}
							if ((Bio2Duration - GCDSpeed(spd) < 3.0 || (ragingDuration > 0.0 && Bio2Mod < 1.2)) && (ignoreResources || currentMP >= 186)) // Ok to overlap DoT times to keep active as long as we don't lose more than one tick
							{
								castingSpell = "Bio II";
								castComplete = currentTime + GCDSpeed(spd);
								animationDelay = animationDelayMax;
								GCD = GCDSpeed(spd) * (speedBoostDuration > 0.0 ? 0.8 : 1);
							}
							else if ((MiasmaDuration - GCDSpeed(spd) < 3.0 || (ragingDuration > 0.0 && MiasmaMod < 1.2)) && (ignoreResources || currentMP >= 133))
							{
								castingSpell = "Miasma";
								castComplete = currentTime + GCDSpeed(spd);
								animationDelay = animationDelayMax;
								GCD = GCDSpeed(spd) * (speedBoostDuration > 0.0 ? 0.8 : 1);
							}
							else if ((BioDuration < 3.0 || (ragingDuration > 0.0 && BioMod < 1.2)) && (ignoreResources || currentMP >= 106))
							{
								avgCasts++;
								BioMod = (ragingDuration > 0.0 ? 1.2 : 1.0);
								animationDelay = animationDelayMax;
								currentMP -= 106;
								GCD = GCDSpeed(spd) * (speedBoostDuration > 0.0 ? 0.8 : 1);
								BioDuration = 18.0;
								castingSpell = "";
								petActive = true;
							}
							else if ((contagionRecast <= 0.0 && ragingDuration > 0.0) && (ignoreResources || currentMP >= 186))
							{
								avgCasts++;
								totalDmg += SMNSkillDamage("Miasma II", wdmg, intel, dtr, crit) * (ragingDuration > 0.0 ? 1.2 : 1.0);
								currentMP -= 186;
								Miasma2Mod = (ragingDuration > 0.0 ? 1.2 : 1.0);
								animationDelay = animationDelayMax;
								GCD = GCDSpeed(spd) * (speedBoostDuration > 0.0 ? 0.8 : 1);
								Miasma2Duration = 15.0;
								castingSpell = "";
								petActive = true;
							}
							else if ((shadowFlareDuration - CastSpeed(spd, 3.0) < 3.0 || (ragingDuration > 0.0 && FlareMod < 1.2)) && (ignoreResources || currentMP >= 212))
							{
								animationDelay = animationDelayMax + (swiftRecast <= 0.0 ? animationDelayMax : 0); //add an additional 1/2 second of "animation delay" for placing the shadow flare, and another for possible swiftcase usage
								GCD = GCDSpeed(spd) * (speedBoostDuration > 0.0 ? 0.8 : 1) + animationDelayMax + (swiftRecast <= 0.0 ? animationDelayMax : 0);
								if (swiftRecast <= 0.0)
								{
									swiftRecast = 60.0;
									FlareMod = (ragingDuration > 0.0 ? 1.2 : 1.0);
									shadowFlareDuration = 30.0 + animationDelay; // fudge duration for swiftcast usage, we'll de-fudge in DoT tick calculation
									currentMP -= 212;
									avgCasts++;
									castingSpell = "";
									petActive = true;
								}
								else
								{
									castingSpell = "Shadow Flare";
									castComplete = currentTime + CastSpeed(spd, 3.0) * (speedBoostDuration > 0.0 ? 0.8 : 1) + animationDelay;
								}
							}
							else if ((Bio2Duration > 0.0 && MiasmaDuration > 0.0 && BioDuration > 0.0 && shadowFlareDuration > 0.0) && ((aetherflowStacks == 0 && aetherflowRecast <= animationDelayMax) ||
								(festerRecast <= animationDelayMax && aetherflowStacks > 0) || ragingRecast <= animationDelayMax || SpurRecast <= animationDelayMax || RouseRecast <= animationDelayMax) && (ignoreResources || currentMP >= 133)) // use Ruin II if one of the off-GCDs should be used prior to the next GCD
							{
								avgCasts++;
								totalDmg += SMNSkillDamage("Ruin II", wdmg, intel, dtr, crit) * (ragingDuration > 0.0 ? 1.2 : 1.0);
								currentMP -= 133;
								GCD = GCDSpeed(spd) * (speedBoostDuration > 0.0 ? 0.8 : 1);
								castingSpell = "";
								animationDelay = animationDelayMax;
								petActive = true;
							}
							else if ((Bio2Duration > 0.0 && MiasmaDuration > 0.0 && BioDuration > 0.0 && shadowFlareDuration > 0.0) && (ignoreResources || currentMP >= 79)) // if nothing is coming off recast soon, use Ruin to conserve MP
							{
								castingSpell = "Ruin";
								castComplete = currentTime + GCDSpeed(spd);
								animationDelay = animationDelayMax;
								GCD = GCDSpeed(spd) * (speedBoostDuration > 0.0 ? 0.8 : 1);
								petActive = true;
							}
						}

						if (animationDelay <= 0.0 && GCD > animationDelayMax && string.IsNullOrWhiteSpace(castingSpell)) // If we're not actively casting anything, but we have time, try for Aetherflow/Fester/Rouse/Spur
						{
							if (aetherflowStacks > 0 && festerRecast <= 0 && Bio2Duration > 0 && MiasmaDuration > 0 && BioDuration > 0 && (ignoreResources || currentMP >= mpMax / 4 || ragingDuration > 0.0))
							{ // only fester if we have stacks, it's off recast and all DoTs are up
								totalDmg += SMNSkillDamage("Fester", wdmg, intel, dtr, crit) * (ragingDuration > 0.0 ? 1.2 : 1.0);
								festerRecast = 10.0;
								aetherflowStacks--;
								animationDelay = animationDelayMax;
								petActive = true;
							}
							else if (energyDrainRecast <= 0.0 && (aetherflowStacks > 1 || (aetherflowStacks > 0 && aetherflowRecast < ragingRecast)) && currentMP < mpMax / 4 && !ignoreResources)
							{ // Energy Drain if we're low, but hold onto a stack for Fester unless aetherflow will be back before raging strikes
								totalDmg += SMNSkillDamage("Energy Drain", wdmg, intel, dtr, crit) * (ragingDuration > 0.0 ? 1.2 : 1.0);
								currentMP = Math.Min(mpMax, currentMP + 266);
								energyDrainRecast = 3.0;
								aetherflowStacks--;
								animationDelay = animationDelayMax;
								petActive = true;
							}
							else if (aetherflowStacks == 0 && aetherflowRecast <= 0)
							{
								// MP recovery
								currentMP = (int)Math.Min(mpMax, currentMP + mpMax * .2);
								aetherflowStacks = aetherflowMax;
								aetherflowRecast = 60.0;
								animationDelay = animationDelayMax;
							}
							else if (RouseRecast <= 0)
							{
								RouseDuration = 20.0;
								RouseRecast = 90.0;
								animationDelay = animationDelayMax;
							}
							else if (SpurRecast <= 0)
							{
								SpurDuration = 20.0;
								SpurRecast = 120.0;
								animationDelay = animationDelayMax;
							}
						}
					}

					// pet actions
					if (currentTime >= petCastComplete && !string.IsNullOrWhiteSpace(petSpell))
					{
						totalDmg += SMNSkillDamage(petSpell, wdmg, intel, dtr, crit) * (1 + (RouseDuration > 0 ? 0.4 : 0) + (SpurDuration > 0 ? 0.4 : 0));
						if ((double)_rng.Next(100000) / 1000.0 <= (int)(0.0697 * crit - 18.437))
						{
							if (_rng.Next(10) < 2)
							{
								speedBoostDuration = 8.0;
							}
						}
						petSpell = "";
					}
					else if (petGCD <= 0.0 && petActive && string.IsNullOrWhiteSpace(petSpell) && petAnimationDelay <= 0.0)
					{
						petSpell = "Wind Blade";
						petCastComplete = currentTime + 1.0;
						petGCD = 3.0;
					}
					if (petAnimationDelay <= 0.0 && string.IsNullOrWhiteSpace(petSpell))
					{
						// contagion if all DoTs are up, hold if raging strikes will be back before contagion's recast is up
						// also make sure if raging strikes is up that we had the chance to get all DoTs up, including Miasma2
						if (contagionRecast <= 0.0 && (Bio2Duration > 0.0 && MiasmaDuration > 0.0 && BioDuration > 0.0 && Miasma2Duration > 0.0) && ragingRecast > 60.0 &&
							(ragingDuration > 0.0 ? (Bio2Mod > 1.0 && MiasmaMod > 1.0 && BioMod > 1.0 && Miasma2Mod > 1.0) : true))
						{
							contagionRecast = 60.0;
							Bio2Duration += 15.0;
							MiasmaDuration += 15.0;
							Miasma2Duration += 15.0;
							BioDuration += 15.0;
							petGCD = animationDelayMax;
						}
						//Aerial Slash, not on pet GCD, instant 30s recast, don't mess with petGCD though
						/*else if (aerialSlashRecast <= 0.0 && petGCD > animationDelayMax)
						{
							totalDmg += SMNSkillDamage("Aerial Slash", wdmg, intel, dtr, crit) * (1 + (RouseDuration > 0 ? 0.4 : 0) + (SpurDuration > 0 ? 0.4 : 0));
							aerialSlashRecast = 30.0;
							petAnimationDelay = animationDelayMax;
						}*/
					}
					

					if (currentTime >= nextTick)
					{
						if (Bio2Duration >= 0)
						{ //35 pot
							totalDmg += SMNSkillDamage("Bio2DoT", wdmg, intel, dtr, crit) * Bio2Mod;
						}
						if (MiasmaDuration >= 0)
						{ //35 pot
							totalDmg += SMNSkillDamage("MiasmaDoT", wdmg, intel, dtr, crit) * MiasmaMod;
						}
						if (Miasma2Duration >= 0)
						{ //10 pot
							totalDmg += SMNSkillDamage("Miasma2DoT", wdmg, intel, dtr, crit) * Miasma2Mod;
						}
						if (BioDuration >= 0)
						{ // 40 pot
							totalDmg += SMNSkillDamage("BioDoT", wdmg, intel, dtr, crit) * BioMod;
						}
						if (shadowFlareDuration >= 0 && shadowFlareDuration <= 30.0)
						{ // 25 pot
							totalDmg += SMNSkillDamage("FlareDoT", wdmg, intel, dtr, crit) * FlareMod;
						}
						currentMP = Math.Min(currentMP + (int)(mpMax * 0.02), mpMax);
						nextTick = GetNextServerTick(nextTick);
					}
					swiftRecast -= 0.001;
					RouseDuration -= 0.001;
					RouseRecast -= 0.001;
					SpurDuration -= 0.001;
					SpurRecast -= 0.001;
					ragingDuration -= 0.001;
					ragingRecast -= 0.001;
					Bio2Duration -= 0.001;
					MiasmaDuration -= 0.001;
					Miasma2Duration -= 0.001;
					BioDuration -= 0.001;
					shadowFlareDuration -= 0.001;
					festerRecast -= 0.001;
					energyDrainRecast -= 0.001;
					aetherflowRecast -= 0.001;
					GCD -= 0.001;
					petGCD -= 0.001;
					contagionRecast -= 0.001;
					aerialSlashRecast -= 0.001;
					speedBoostDuration -= 0.001;
					animationDelay -= 0.001;
					petAnimationDelay -= 0.001;
					currentTime += 0.001;
				} while (currentTime <= _simMinutes * 60);
			}
			avgCasts /= simIterations;
			return totalDmg;
		}

		private static double DRGSim(int wdmg, int str, int crit, int dtr, int spd, bool ignoreResources = false)
		{
			int avgGCDs = 0;
			return DRGSim(wdmg, str, crit, dtr, spd, out avgGCDs, ignoreResources);
		}
		private static double DRGSim(int wdmg, int str, int crit, int dtr, int spd, out int avgGCDs, bool ignoreResources = false)
		{
			double totalDmg = 0.0;
			avgGCDs = 0;

			for (int i = 0; i < simIterations; i++)
			{
				_rng = new Random(i);

				double heavyThrustDuration = 0.0;
				double disemBowelDuration = 0.0;
				double chaosThrustDuration = 0.0;
				double chaosDmgMod = 0.0;
				double chaosCritMod = 0.0;
				double phlebotomizeDuration = 0.0;
				double phlebDmgMod = 0.0;
				double phlebCritMod = 0.0;

				double InvigorateRecast = 0.0;
				double InternalReleaseDuration = 0.0;
				double InternalReleaseRecast = 0.0;
				double bloodforbloodDuration = 0.0;
				double bloodforbloodRecast = 0.0;
				double powerSurgeDuration = 0.0;//10
				double powerSurgeRecast = 0.0;//60
				double lifeSurgeDuration = 0.0;//10
				double lifeSurgeRecast = 0.0;//60
				double jumpRecast = 0.0;//40
				double spineshatterRecast = 0.0;//90
				double dragonFireRecast = 0.0; //180
				double legSweepRecast = 0.0;//20

				double autoattackDelay = 3.0;
				//3.12 Mode, 2.97 avg
				double autoattackDelayMax = 2.97;

				int TP = 1000;
				double GCD = 0.0;

				double currentTime = 0.0;
				double nextTick = (double)_rng.Next(3001) / 1000.0;

				double animationDelay = 0.0;
				double animationDelayMax = 1.0;

				string comboSkill = "";

				do
				{
					if (animationDelay <= 0)
					{
						critmod = (InternalReleaseDuration > 0.0 ? 10.0 : 0.0);
						if (GCD <= 0)
						{
							if (((heavyThrustDuration <= GCDSpeed(spd) * 2 && string.IsNullOrWhiteSpace(comboSkill)) || heavyThrustDuration <= GCDSpeed(spd)) && (ignoreResources || TP >= 70))
							{
								totalDmg += DRGSkillDamage("Heavy Thrust", wdmg, str, dtr, crit, heavyThrustDuration >= 0, disemBowelDuration >= 0, critmod) * (bloodforbloodDuration > 0.0 ? 1.3 : 1);
								TP -= 70;
								heavyThrustDuration = 20;
								animationDelay = animationDelayMax;
								GCD = GCDSpeed(spd);
								comboSkill = "";
								avgGCDs++;
								//autoattackDelay = Math.Max(autoattackDelay, 0.1);
							}
							else if (disemBowelDuration > 0 && phlebotomizeDuration <= GCDSpeed(spd) * 2 && string.IsNullOrWhiteSpace(comboSkill) && (ignoreResources || TP >= 90))
							{
								totalDmg += DRGSkillDamage("Phlebotomize", wdmg, str, dtr, crit, heavyThrustDuration >= 0, disemBowelDuration >= 0, critmod) * (bloodforbloodDuration > 0.0 ? 1.3 : 1);
								TP -= 90;
								phlebotomizeDuration = 18;
								animationDelay = animationDelayMax;
								phlebCritMod = critmod;
								phlebDmgMod = 1 * (heavyThrustDuration > 0.0 ? 1.15 : 1) * (bloodforbloodDuration > 0.0 ? 1.3 : 1);
								GCD = GCDSpeed(spd);
								comboSkill = "";
								avgGCDs++;
								//autoattackDelay = Math.Max(autoattackDelay, 0.1);
							}
							else if (disemBowelDuration <= GCDSpeed(spd) * 5 && string.IsNullOrWhiteSpace(comboSkill) && (ignoreResources || TP >= 70))
							{ // start rear combo - Impulse Drive
								totalDmg += DRGSkillDamage("Impulse Drive", wdmg, str, dtr, crit, heavyThrustDuration >= 0, disemBowelDuration >= 0, critmod) * (bloodforbloodDuration > 0.0 ? 1.3 : 1);
								TP -= 70;
								animationDelay = animationDelayMax;
								GCD = GCDSpeed(spd);
								comboSkill = "Disembowel";
								avgGCDs++;
								//autoattackDelay = Math.Max(autoattackDelay, 0.1);
							}
							else if (string.IsNullOrWhiteSpace(comboSkill) && (ignoreResources || TP >= 70))
							{ // start main combo - True Thrust
								totalDmg += DRGSkillDamage("True Thrust", wdmg, str, dtr, crit, heavyThrustDuration >= 0, disemBowelDuration >= 0, critmod) * (bloodforbloodDuration > 0.0 ? 1.3 : 1);
								TP -= 70;
								animationDelay = animationDelayMax;
								GCD = GCDSpeed(spd);
								comboSkill = "Vorpal Thrust";
								avgGCDs++;
								//autoattackDelay = Math.Max(autoattackDelay, 0.1);
							}
							else if (!string.IsNullOrWhiteSpace(comboSkill) && (ignoreResources || TP >= 60))
							{ // do the next skill in the combo
								totalDmg += DRGSkillDamage(comboSkill, wdmg, str, dtr, crit, heavyThrustDuration > 0.0, disemBowelDuration > 0.0, critmod, lifeSurgeDuration > 0.0) * (bloodforbloodDuration > 0.0 ? 1.3 : 1);
								TP -= 60;
								switch (comboSkill)
								{
									case "Vorpal Thrust":
										comboSkill = "Full Thrust";
										break;
									case "Full Thrust":
										comboSkill = "";
										break;
									case "Disembowel":
										disemBowelDuration = 30;
										comboSkill = "Chaos Thrust";
										break;
									case "Chaos Thrust":
										chaosThrustDuration = 30;
										chaosCritMod = critmod;
										chaosDmgMod = 1 * (heavyThrustDuration > 0.0 ? 1.15 : 1) * (bloodforbloodDuration > 0.0 ? 1.3 : 1);
										comboSkill = "";
										break;
									default:
										break;
								}
								animationDelay = animationDelayMax;
								GCD = GCDSpeed(spd);
								avgGCDs++;
								//autoattackDelay = Math.Max(autoattackDelay, 0.1);
							}
						}
						//lifesurge
						if (animationDelay <= 0 && lifeSurgeRecast <= 0 && GCD >= animationDelayMax && comboSkill == "Full Thrust")
						{
							lifeSurgeDuration = 10.0;
							lifeSurgeRecast = 60.0;
							animationDelay = animationDelayMax;
							//autoattackDelay = Math.Max(autoattackDelay, 0.1);
						}
						//internal release
						if (animationDelay <= 0 && InternalReleaseRecast <= 0 && GCD >= animationDelayMax)
						{
							InternalReleaseDuration = 15.0;
							InternalReleaseRecast = 60.0;
							animationDelay = animationDelayMax;
							//autoattackDelay = Math.Max(autoattackDelay, 0.1);
						}
						//blood for blood
						if (animationDelay <= 0 && bloodforbloodRecast <= 0 && GCD >= animationDelayMax)
						{
							bloodforbloodDuration = 20.0;
							bloodforbloodRecast = 80.0;
							animationDelay = animationDelayMax;
							//autoattackDelay = Math.Max(autoattackDelay, 0.1);
						}
						//power surge
						if (animationDelay <= 0 && powerSurgeRecast <= 0 && GCD >= animationDelayMax && jumpRecast <= GCD + animationDelay)
						{ // make sure jump is going to be ready before using power surge
							powerSurgeDuration = 10.0;
							powerSurgeRecast = 60.0;
							animationDelay = animationDelayMax;
							//autoattackDelay = Math.Max(autoattackDelay, 0.1);
						}
						//jump
						if (animationDelay <= 0 && jumpRecast <= 0 && GCD >= animationDelayMax)
						{
							totalDmg += DRGSkillDamage("Jump", wdmg, str, dtr, crit, heavyThrustDuration > 0.0, disemBowelDuration > 0.0, critmod) * (powerSurgeDuration > 0.0 ? 1.5 : 1) * (bloodforbloodDuration > 0.0 ? 1.3 : 1);
							powerSurgeDuration = 0.0;
							jumpRecast = 45.0;
							animationDelay = animationDelayMax * 1.5;
							//autoattackDelay = Math.Max(autoattackDelay, 0.1);
						}
						//leg sweep
						if (animationDelay <= 0 && legSweepRecast <= 0 && GCD >= animationDelayMax)
						{
							totalDmg += DRGSkillDamage("Leg Sweep", wdmg, str, dtr, crit, heavyThrustDuration > 0.0, disemBowelDuration > 0.0, critmod) * (bloodforbloodDuration > 0.0 ? 1.3 : 1);
							legSweepRecast = 20.0;
							animationDelay = animationDelayMax;
							//autoattackDelay = Math.Max(autoattackDelay, 0.1);
						}
						//dragonfire
						if (animationDelay <= 0 && dragonFireRecast <= 0 && GCD >= animationDelayMax)
						{
							totalDmg += DRGSkillDamage("Dragonfire Dive", wdmg, str, dtr, crit, heavyThrustDuration > 0.0, disemBowelDuration > 0.0, critmod) * (bloodforbloodDuration > 0.0 ? 1.3 : 1);
							dragonFireRecast = 180.0;
							animationDelay = animationDelayMax;
							//autoattackDelay = Math.Max(autoattackDelay, 0.1);
						}
						//spineshatter
						if (animationDelay <= 0 && spineshatterRecast <= 0 && GCD >= animationDelayMax)
						{
							totalDmg += DRGSkillDamage("Spineshatter Dive", wdmg, str, dtr, crit, heavyThrustDuration > 0.0, disemBowelDuration > 0.0, critmod) * (powerSurgeDuration > 0.0 ? 1.5 : 1) * (bloodforbloodDuration > 0.0 ? 1.3 : 1);
							powerSurgeDuration = 0.0;
							spineshatterRecast = 90.0;
							animationDelay = animationDelayMax;
							//autoattackDelay = Math.Max(autoattackDelay, 0.1);
						}
						if (animationDelay <= 0 && TP <= (1000 - 500 - 60) && InvigorateRecast <= 0 && GCD >= animationDelayMax)
						{
							TP = Math.Min(TP + 500, 1000);
							InvigorateRecast = 120.0;
							//autoattackDelay = Math.Max(autoattackDelay, 0.1);
						}
					}
					if (currentTime >= nextTick)
					{
						if (chaosThrustDuration >= 0)
						{
							totalDmg += DRGSkillDamage("ChaosDoT", wdmg, str, dtr, crit, heavyThrustDuration >= 0, disemBowelDuration >= 0, chaosCritMod) * chaosDmgMod;
						}
						if (phlebotomizeDuration >= 0)
						{
							totalDmg += DRGSkillDamage("PhlebDoT", wdmg, str, dtr, crit, heavyThrustDuration >= 0, disemBowelDuration >= 0, phlebCritMod) * phlebDmgMod;
						}
						TP = Math.Min(TP + 60, 1000);
						nextTick = GetNextServerTick(nextTick);
					}
					if (autoattackDelay <= 0.0)
					{
						totalDmg += (wdmg * .2714745 + str * .1006032 + (dtr - 202) * .0241327 + wdmg * str * .0036167 + wdmg * (dtr - 202) * .0022597 - 1) * (autoattackDelayMax / 3.0) * (1 + .5 * (0.0697 * crit - 18.437 + critmod) / 100);
						autoattackDelay = autoattackDelayMax;
					}
					autoattackDelay -= 0.001;
					heavyThrustDuration -= 0.001;
					disemBowelDuration -= 0.001;
					chaosThrustDuration -= 0.001;
					phlebotomizeDuration -= 0.001;
					animationDelay -= 0.001;
					InternalReleaseDuration -= 0.001;
					InternalReleaseRecast -= 0.001;
					bloodforbloodDuration -= 0.001;
					bloodforbloodRecast -= 0.001;
					powerSurgeDuration -= 0.001;
					powerSurgeRecast -= 0.001;
					lifeSurgeDuration -= 0.001;
					lifeSurgeRecast -= 0.001;
					jumpRecast -= 0.001;
					spineshatterRecast -= 0.001;
					dragonFireRecast -= 0.001;
					legSweepRecast -= 0.001;
					GCD -= 0.001;
					currentTime += 0.001;
				} while (currentTime <= _simMinutes * 60);
			}
			avgGCDs /= simIterations;
			return totalDmg;
		}

		private static double BRDSim(int wdmg, int dex, int crit, int dtr, int spd, bool ignoreResources = false)
		{
			int avgGCDs = 0;
			return BRDSim(wdmg, dex, crit, dtr, spd, out avgGCDs, ignoreResources);
		}
		private static double BRDSim(int wdmg, int dex, int crit, int dtr, int spd, out int avgGCDs, bool ignoreResources = false)
		{
			double totalDmg = 0.0;
			avgGCDs = 0;
			_totalSims++;
			Random riverRNG;
			for (int i = 0; i < simIterations; i++)
			{
				_rng = new Random(i);
				riverRNG = new Random(i);

				double straightShotDuration = 0.0;
				double bloodLetterRecast = 0.0;
				double InvigorateRecast = 0.0;
				double windBiteDuration = 0.0;
				double windBitecritMod = 0.0;
				double windBitedmgMod = 0.0;
				bool windbiteHawkEye = false;
				double venomBiteDuration = 0.0;
				double venomBitecritMod = 0.0;
				double venomBitedmgMod = 0.0;
				bool venombiteHawkEye = false;
				bool flamingArrowHawkEye = false;
				double flamingArrowDuration = 0.0;
				double flamingArrowRecast = 0.0;
				double flamingArrowCritMod = 0.0;
				double flamingArrowDmgMod = 0.0;

				double InnerReleaseDuration = 0.0;
				double InnerReleaseRecast = 0.0;

				double bloodforbloodRecast = 0.0;
				double bloodforbloodDuration = 0.0;
				double barrageDuration = 0.0;
				double barrageRecast = 0.0;
				double ragingStrikesRecast = 0.0;
				double ragingStrikesDuration = 0.0;
				double hawkeyeRecast = 0.0;
				double hawkeyeDuration = 0.0;
				double bluntArrowRecast = 0.0;
				double repellingShotRecast = 0.0;

				bool straighterShot = false;

				double GCD = 0.0;

				double currentTime = 0.0;
				double nextTick = (double)_rng.Next(3001) / 1000.0;

				int TP = 1000;

				double animationDelay = 0.0;
				double animationDelayMax = 0.5;
				double autoattackDelay = 0.0;
				//3.28 mode, 3.12 avg
				double autoattackDelayMax = 3.13;

				do
				{
					critmod = (InnerReleaseDuration > 0.0 ? 10.0 : 0.0) + (straightShotDuration > 0.0 ? 10.0 : 0.0);
					if (animationDelay <= 0.0) // It's been long enough since the last action to trigger a new one.
					{
						if (GCD <= 0.0)
						{ // If bloodletter is down, and GCD is up, use a GCD skill
							if ((straighterShot || straightShotDuration <= GCDSpeed(spd)) && (ignoreResources || TP >= 70))
							{ // Make sure we keep straight shot up and use procs
								totalDmg += BardSkillDamage("Straight Shot", wdmg, (int)(dex * (hawkeyeDuration > 0.0 ? 1.15 : 1)), dtr, crit, straighterShot, critmod) * (bloodforbloodDuration > 0.0 ? 1.1 : 1) * (ragingStrikesDuration > 0.0 ? 1.2 : 1);
								straighterShot = false;
								TP -= 70;
								animationDelay = animationDelayMax;
								GCD = GCDSpeed(spd);
								straightShotDuration = 20.0;
								avgGCDs++;
								//autoattackDelay = Math.Max(autoattackDelay, 0.1);
							}
							else if (windBiteDuration <= GCDSpeed(spd) || windBitecritMod < critmod && (ignoreResources || TP >= 80))
							{
								totalDmg += BardSkillDamage("Windbite", wdmg, (int)(dex * (hawkeyeDuration > 0.0 ? 1.15 : 1)), dtr, crit, straighterShot, critmod) * (bloodforbloodDuration > 0.0 ? 1.1 : 1) * (ragingStrikesDuration > 0.0 ? 1.2 : 1);
								windBiteDuration = 18;
								TP -= 80;
								animationDelay = animationDelayMax;
								GCD = GCDSpeed(spd);
								windBitecritMod = critmod;
								windBitedmgMod = 1 * (bloodforbloodDuration > 0.0 ? 1.1 : 1) * (ragingStrikesDuration > 0.0 ? 1.2 : 1);
								windbiteHawkEye = hawkeyeDuration > 0.0;
								avgGCDs++;
								//autoattackDelay = Math.Max(autoattackDelay, 0.1);
							}
							else if (venomBiteDuration <= GCDSpeed(spd) || venomBitecritMod < critmod && (ignoreResources || TP >= 80))
							{
								totalDmg += BardSkillDamage("Venomous Bite", wdmg, (int)(dex * (hawkeyeDuration > 0.0 ? 1.15 : 1)), dtr, crit, straighterShot, critmod) * (bloodforbloodDuration > 0.0 ? 1.1 : 1) * (ragingStrikesDuration > 0.0 ? 1.2 : 1);
								venomBiteDuration = 18;
								TP -= 80;
								animationDelay = animationDelayMax;
								GCD = GCDSpeed(spd);
								venomBitecritMod = critmod;
								venombiteHawkEye = hawkeyeDuration > 0.0;
								venomBitedmgMod = 1 * (bloodforbloodDuration > 0.0 ? 1.1 : 1) * (ragingStrikesDuration > 0.0 ? 1.2 : 1);
								avgGCDs++;
								//autoattackDelay = Math.Max(autoattackDelay, 0.1);
							}
							else if (ignoreResources || TP >= 60)
							{
								totalDmg += BardSkillDamage("Heavy Shot", wdmg, (int)(dex * (hawkeyeDuration > 0.0 ? 1.15 : 1)), dtr, crit, straighterShot, critmod) * (bloodforbloodDuration > 0.0 ? 1.1 : 1) * (ragingStrikesDuration > 0.0 ? 1.2 : 1);
								straighterShot = _rng.Next(10) < 2;
								TP -= 60;
								animationDelay = animationDelayMax;
								GCD = GCDSpeed(spd);
								avgGCDs++;
								//autoattackDelay = Math.Max(autoattackDelay, 0.1);
							}
						}
						if (animationDelay <= 0)
						{
							if (bloodLetterRecast <= 0.0 && GCD >= animationDelayMax && (currentTime + Math.Max(GCD, 0) + animationDelayMax >= nextTick && (currentTime + Math.Max(venomBiteDuration, 0) >= nextTick || currentTime + Math.Max(windBiteDuration, 0) >= nextTick)))
							{ // Prioritize Bloodletters above everything else if DoTs are ticking and we're going to hit a server tick before we can act again following the next GCD
								totalDmg += BardSkillDamage("Bloodletter", wdmg, (int)(dex * (hawkeyeDuration > 0.0 ? 1.15 : 1)), dtr, crit, straighterShot, critmod) * (bloodforbloodDuration > 0.0 ? 1.1 : 1) * (ragingStrikesDuration > 0.0 ? 1.2 : 1);
								bloodLetterRecast = 12.0;
								animationDelay = animationDelayMax;
								//autoattackDelay = Math.Max(autoattackDelay, 0.1);
							}
							else if (InnerReleaseRecast <= 0.0 && GCD >= animationDelayMax)
							{
								InnerReleaseDuration = 15.0;
								InnerReleaseRecast = 60.0;
								animationDelay = animationDelayMax;
								//autoattackDelay = Math.Max(autoattackDelay, 0.1);
							}
							else if (bloodLetterRecast <= 0.0 && straightShotDuration > 0 && GCD >= animationDelayMax)
							{ // Prioritize Bloodletters above other off-GCDs but only if straight shot is up
								totalDmg += BardSkillDamage("Bloodletter", wdmg, (int)(dex * (hawkeyeDuration > 0.0 ? 1.15 : 1)), dtr, crit, straighterShot, critmod) * (bloodforbloodDuration > 0.0 ? 1.1 : 1) * (ragingStrikesDuration > 0.0 ? 1.2 : 1);
								bloodLetterRecast = 12.0;
								animationDelay = animationDelayMax;
								//autoattackDelay = Math.Max(autoattackDelay, 0.1);
							}
							else if (TP <= (1000 - 400 - 60) && InvigorateRecast <= 0 && GCD >= animationDelayMax)
							{
								TP = Math.Min(TP + 400, 1000);
								InvigorateRecast = 120.0;
								animationDelay = animationDelayMax;
								//autoattackDelay = Math.Max(autoattackDelay, 0.1);
							}
							else if (ragingStrikesRecast <= 0.0 && GCD >= animationDelayMax)
							{
								ragingStrikesDuration = 20.0;
								ragingStrikesRecast = 180.0;
								animationDelay = animationDelayMax;
								//autoattackDelay = Math.Max(autoattackDelay, 0.1);
							}
							else if (bloodforbloodRecast <= 0.0 && GCD >= animationDelayMax)
							{
								bloodforbloodDuration = 20.0;
								bloodforbloodRecast = 80.0;
								animationDelay = animationDelayMax; // buffs are faster
								//autoattackDelay = Math.Max(autoattackDelay, 0.1);
							}
							else if (hawkeyeRecast <= 0.0 && GCD >= animationDelayMax)
							{
								hawkeyeDuration = 20.0;
								hawkeyeRecast = 90.0;
								animationDelay = animationDelayMax;
								//autoattackDelay = Math.Max(autoattackDelay, 0.1);
							}
							else if (barrageRecast <= 0.0 && GCD >= animationDelayMax)
							{
								barrageRecast = 90.0;
								barrageDuration = 10.0;
								animationDelay = animationDelayMax;
								//autoattackDelay = Math.Max(autoattackDelay, 0.1);
							}
							else if (flamingArrowRecast <= 0.0 && GCD >= animationDelayMax)
							{
								flamingArrowRecast = 60.0;
								flamingArrowDuration = 30.0;
								flamingArrowCritMod = critmod;
								flamingArrowDmgMod = 1 * (bloodforbloodDuration > 0.0 ? 1.1 : 1) * (ragingStrikesDuration > 0.0 ? 1.2 : 1);
								flamingArrowHawkEye = hawkeyeDuration > 0;
								animationDelay = animationDelayMax;
								//autoattackDelay = Math.Max(autoattackDelay, 0.1);
							}
							else if (bluntArrowRecast <= 0.0 && GCD >= animationDelayMax)
							{
								totalDmg += BardSkillDamage("Blunt Arrow", wdmg, (int)(dex * (hawkeyeDuration > 0.0 ? 1.15 : 1)), dtr, crit, straighterShot, critmod) * (bloodforbloodDuration > 0.0 ? 1.1 : 1) * (ragingStrikesDuration > 0.0 ? 1.2 : 1);
								bluntArrowRecast = 30.0;
								animationDelay = animationDelayMax;
								//autoattackDelay = Math.Max(autoattackDelay, 0.1);
							}
							else if (repellingShotRecast <= 0.0 && GCD >= animationDelayMax)
							{
								totalDmg += BardSkillDamage("Repelling Shot", wdmg, (int)(dex * (hawkeyeDuration > 0.0 ? 1.15 : 1)), dtr, crit, straighterShot, critmod) * (bloodforbloodDuration > 0.0 ? 1.1 : 1) * (ragingStrikesDuration > 0.0 ? 1.2 : 1);
								repellingShotRecast = 30.0;
								animationDelay = animationDelayMax;
								//autoattackDelay = Math.Max(autoattackDelay, 0.1);
							}
						}
					}

					if (currentTime >= nextTick) // Process server ticks
					{
						double windBiteCritProc, windBiteCritChance, venomBiteCritProc, venomBiteCritChance;
						bool windRiverProc, venomRiverProc;
						if (windBiteDuration >= 0)
						{
							totalDmg += BardSkillDamage("WindDoT", wdmg, (int)(dex * (windbiteHawkEye ? 1.15 : 1)), dtr, crit, straighterShot, windBitecritMod) * windBitedmgMod;
							windBiteCritProc = (double)Math.Round((double)_rng.Next(100000) / 1000.0, 4);
							windBiteCritChance = (double)Math.Round(0.0697 * (double)crit - 18.437 + windBitecritMod, 4);
							windRiverProc = riverRNG.Next(2) > 0;
							if (windBiteCritProc <= windBiteCritChance && windRiverProc)
							{
								//if (riverRNG.Next(2) > 0)
								//{
									bloodLetterRecast = -50.0;
								//}
							}
						}
						if (venomBiteDuration >= 0)
						{
							totalDmg += BardSkillDamage("VenomDoT", wdmg, (int)(dex * (venombiteHawkEye ? 1.15 : 1)), dtr, crit, straighterShot, venomBitecritMod) * venomBitedmgMod;
							venomBiteCritProc = (double)Math.Round((double)_rng.Next(100000) / 1000.0, 4);
							venomBiteCritChance = (double)Math.Round(0.0697 * (double)crit - 18.437 + venomBitecritMod, 4);
							venomRiverProc = riverRNG.Next(2) > 0;
							if (venomBiteCritProc <= venomBiteCritChance && venomRiverProc)
							{
								//if (riverRNG.Next(2) > 0)
								//{
									bloodLetterRecast = -50.0;
								//}
							}
						}
						if (bloodLetterRecast <= -50.0)
						{
							_bloodLetterProcs++;
						}
						if (flamingArrowDuration >= 0)
						{
							totalDmg += BardSkillDamage("Flaming Arrow", wdmg, (int)(dex * (flamingArrowHawkEye ? 1.15 : 1)), dtr, crit, straighterShot, flamingArrowCritMod) * flamingArrowDmgMod;
						}
						TP = Math.Min(TP + 60, 1000);
						nextTick = GetNextServerTick(nextTick);
					}

					if (autoattackDelay <= 0.0)
					{
						double autoattackdmg = (wdmg * .2714745 + (int)(dex * (hawkeyeDuration > 0.0 ? 1.15 : 1)) * .1006032 + (dtr - 202) * .0241327 + wdmg * (int)(dex * (hawkeyeDuration > 0.0 ? 1.15 : 1)) * .0036167 + wdmg * (dtr - 202) * .0022597 - 1) * (autoattackDelayMax / 3.0) * (1 + .5 * (0.0697 * crit - 18.437 + critmod) / 100);
						totalDmg += autoattackdmg * (barrageDuration > 0.0 ? 3 : 1) * (bloodforbloodDuration > 0.0 ? 1.1 : 1) * (ragingStrikesDuration > 0.0 ? 1.2 : 1);
						autoattackDelay = autoattackDelayMax;
					}

					windBiteDuration -= 0.001;
					venomBiteDuration -= 0.001;
					InvigorateRecast -= 0.001;
					bloodLetterRecast -= 0.001;
					straightShotDuration -= 0.001;
					animationDelay -= 0.001;
					autoattackDelay -= 0.001;
					GCD -= 0.001;
					InnerReleaseDuration -= 0.001;
					InnerReleaseRecast -= 0.001;
					ragingStrikesDuration -= 0.001;
					ragingStrikesRecast -= 0.001;
					bloodforbloodDuration -= 0.001;
					bloodforbloodRecast -= 0.001;
					barrageDuration -= 0.001;
					barrageRecast -= 0.001;
					hawkeyeDuration -= 0.001;
					hawkeyeRecast -= 0.001;
					repellingShotRecast -= 0.001;
					bluntArrowRecast -= 0.001;
					currentTime += 0.001;
				} while (currentTime <= _simMinutes * 60);
			}
			avgGCDs /= simIterations;
			return totalDmg;
		}

		public enum SpellType
		{
			ThunderDoT,
			Thunder,
			Thunder2,
			Thunder3,
			Fire,
			Fire3,
			Flare,
			Blizzard,
			Blizzard3,
			Scathe,
			None
		}
		public enum BuffType
		{
			AF,
			AF2,
			AF3,
			UI,
			UI2,
			UI3,
			None
		}

		private static double SpellDamage(int wdmg, int intel, int dtr, int crit, SpellType spell, BuffType mode, bool ThunderCloud)
		{
			double potency;
			switch (spell)
			{
				case SpellType.Scathe:
					potency = 120; // average potency given 20% chance trait for +100 potency
					break;
				case SpellType.ThunderDoT:
					potency = 35;
					break;
				case SpellType.Thunder:
					potency = 30;
					break;
				case SpellType.Thunder2:
					potency = 50;
					break;
				case SpellType.Thunder3:
					potency = 60;
					if (ThunderCloud)
					{
						potency += (35 * 8);
					}
					break;
				case SpellType.Fire:
					potency = 170;
					if (mode == BuffType.AF3)
					{
						potency *= 1.8;
					}
					else if (mode == BuffType.AF2)
					{
						potency *= 1.6;
					}
					else if (mode == BuffType.AF)
					{
						potency *= 1.4;
					}
					else if (mode == BuffType.UI3 || mode == BuffType.UI2 || mode == BuffType.UI)
					{
						potency *= 0.5;
					}
					break;
				case SpellType.Fire3:
					potency = 240;
					if (mode == BuffType.AF3)
					{
						potency *= 1.8;
					}
					else if (mode == BuffType.AF2)
					{
						potency *= 1.6;
					}
					else if (mode == BuffType.AF)
					{
						potency *= 1.4;
					}
					else if (mode == BuffType.UI3 || mode == BuffType.UI2 || mode == BuffType.UI)
					{
						potency *= 0.5;
					}
					break;
				case SpellType.Flare:
					potency = 260;
					if (mode == BuffType.AF3)
					{
						potency *= 1.8;
					}
					else if (mode == BuffType.AF2)
					{
						potency *= 1.6;
					}
					else if (mode == BuffType.AF)
					{
						potency *= 1.4;
					}
					else if (mode == BuffType.UI3 || mode == BuffType.UI2 || mode == BuffType.UI)
					{
						potency *= 0.5;
					}
					break;
				case SpellType.Blizzard:
					potency = 170;
					if (mode == BuffType.AF || mode == BuffType.AF3)
					{
						potency *= 0.5;
					}
					break;
				case SpellType.Blizzard3:
					potency = 240;
					if (mode == BuffType.AF || mode == BuffType.AF3)
					{
						potency *= 0.5;
					}
					break;
				default:
					potency = 0;
					break;
			}
			double baseDmg = (wdmg * .2714745 + intel * .1006032 + (dtr - 202) * .0241327 + wdmg * intel * .0036167 + wdmg * (dtr - 202) * .0010800 - 1);
			//                "77 * 0.2714745 + 578 *  0.1006032 + (307 - 202) *0.0241327 + 77 * 578 *    0.0036167 + 77 *   (307 - 202)  0.0010800 - 1"
			//(Potency/100) x [0.00587517 x INT x WD + 0.077076 x INT + 0.074377 x DET]
			//double baseDmg = .00587517 * intel * wdmg + 0.077076 * intel + 0.074377 * dtr;
			//baseDmg *= ((critproc.Next(100000) / 1000.0) < (0.0697 * crit - 18.437)) ? 1.5 : 1;
			baseDmg *= (1 + 0.5 * (0.0697 * crit - 18.437) / 100.0);
			baseDmg *= 1.3;
			double spellDamage = baseDmg * potency / 100.0;
			return spellDamage;
		}

		private static double CastSpeed(int spd, SpellType spell, BuffType mode)
		{
			double castTime;
			switch (spell)
			{
				case SpellType.Flare:
					castTime = CastSpeed(spd, 4.0);
					break;
				case SpellType.Fire3:
				case SpellType.Blizzard3:
				case SpellType.Thunder3:
					castTime = CastSpeed(spd, 3.5);
					break;
				case SpellType.Thunder2:
					castTime = CastSpeed(spd, 3.0);
					break;
				case SpellType.Fire:
				case SpellType.Blizzard:
				case SpellType.Thunder:
					castTime = CastSpeed(spd, 2.5);
					break;
				default:
					castTime = 0.0;
					break;
			}
			if (((spell == SpellType.Fire || spell == SpellType.Fire3 || spell == SpellType.Flare) && mode == BuffType.UI3) || ((spell == SpellType.Blizzard3 || spell == SpellType.Blizzard) && mode == BuffType.AF3))
			{
				castTime /= 2.0;
			}
			castTime = (double)Math.Round(castTime, 3);
			return castTime;
		}
		private static double CastSpeed(int spd, double baseTime)
		{
			return baseTime -= (spd > 344 ? (.001952 * 3 + (spd - (341 + 3)) * .000952) : (spd - 341) * .001952);
		}

		private static int MPCost(SpellType spell, BuffType mode)
		{
			switch (spell)
			{
				case SpellType.Blizzard:
					if (mode == BuffType.AF)
					{
						return 53;
					}
					else if (mode == BuffType.AF3 || mode == BuffType.AF2)
					{
						return 26;
					}
					else
					{
						return 106;
					}
				case SpellType.Blizzard3:
					if (mode == BuffType.AF)
					{
						return 159;
					}
					else if (mode == BuffType.AF3 || mode == BuffType.AF2)
					{
						return 79;
					}
					else
					{
						return 319;
					}
				case SpellType.Fire:
					if (mode == BuffType.AF || mode == BuffType.AF3)
					{
						return 638;
					}
					else if (mode == BuffType.UI)
					{
						return 159;
					}
					else if (mode == BuffType.UI3 || mode == BuffType.UI2)
					{
						return 79;
					}
					else
					{
						return 319;
					}
				case SpellType.Fire3:
					if (mode == BuffType.AF || mode == BuffType.AF3)
					{
						return 1064;
					}
					else if (mode == BuffType.UI)
					{
						return 266;
					}
					else if (mode == BuffType.UI3 || mode == BuffType.UI2)
					{
						return 133;
					}
					else
					{
						return 532;
					}
				case SpellType.Flare:
					return 266;
				case SpellType.Scathe:
				case SpellType.Thunder:
					return 212;
				case SpellType.Thunder2:
					return 319;
				case SpellType.Thunder3:
					return 425;
				default:
					return 0;
			}
		}
		private static double BLMSim(int wdmg, int intel, int crit, int dtr, int spd)
		{
			int avgCasts = 0;
			return BLMSim(wdmg, intel, crit, dtr, spd, out avgCasts);
		}
	}
}
