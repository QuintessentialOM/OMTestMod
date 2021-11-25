using MonoMod.RuntimeDetour;
using MonoMod.Utils;
using Quintessential;
using Quintessential.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace TestMod {

    using PartType = class_139;
    using Permissions = enum_149;
    using BondType = enum_126;
    using BondSite = class_222;
    using AtomTypes = class_175;
    using PartTypes = class_191;

	public class TestMod : QuintessentialMod {

        public static PartType VariantTriplex;
        public static AtomType Aether, Uranium;

        public static class_256 NoSymbol;
        public static class_256 AetherSymbol;

        public static Puzzle AetherTutorial, AetherProduction, EtherealChains, FixatingSuspension, SubtleAlloy;

		public override void Load() {
            Settings = new TestSettings();
			On.Puzzles.method_1285 += AddPuzzles;
			On.JournalVolumes.method_1052 += AddJournalVolume;
			On.class_175.method_248 += AddAtomTypes;
			On.Campaigns.method_828 += AddCampaignChapter;
        }

		private void AddCampaignChapter(On.Campaigns.orig_method_828 orig) {
            orig();
        }

        private void AddAtomTypes(On.class_175.orig_method_248 orig) {
            orig();

            AtomType aether = new AtomType();
            aether.field_2283/*ID*/ = 64; // FIX: sadly, atoms use byte IDs. this will need some change in Quintessential.
            aether.field_2284/*Non-local Name*/ = class_134.method_254("Aether");
            aether.field_2285/*Atomic Name*/ = class_134.method_253("Elemental Aether", string.Empty);
            aether.field_2286/*Local name*/ = class_134.method_253("Aether", string.Empty);
            aether.field_2287/*Symbol*/ = AetherSymbol;
            aether.field_2288/*Shadow*/ = class_235.method_615("textures/atoms/leppa/TestMod/aether_shadow");
            class_229 class229 = new class_229();
            class229.field_1950/*Base*/ = class_238.field_1989.field_81.field_613.field_627;
            class229.field_1951/*Colours*/ = class_235.method_615("textures/atoms/leppa/TestMod/aether_colors");
            class229.field_1952/*Mask*/ = class_238.field_1989.field_81.field_613.field_629;
            class229.field_1953/*Rimlight*/ = class_238.field_1989.field_81.field_613.field_630;
            aether.field_2292 = class229;
            aether.field_2296/*Non-metal?*/ = true;
            aether.QuintAtomType = "TestMod:aether";
            Aether = aether;

            AtomType uranium = new AtomType();
            uranium.field_2283/*ID*/ = 65; // FIX: sadly, atoms use byte IDs. this will need some change in Quintessential.
            uranium.field_2284/*Non-local Name*/ = class_134.method_254("Uranium");
            uranium.field_2285/*Atomic Name*/ = class_134.method_253("Elemental Uranium", string.Empty);
            uranium.field_2286/*Local name*/ = class_134.method_253("Uranium", string.Empty);
            uranium.field_2287/*Symbol*/ = class_235.method_615("textures/atoms/leppa/TestMod/uranium_symbol");
            uranium.field_2288/*Shadow*/ = class_238.field_1989.field_81.field_599;
            class_8 class8_1 = new class_8();
            class8_1.field_13/*Diffuse*/ = class_238.field_1989.field_81.field_577;
            class8_1.field_14/*Lightramp*/ = class_235.method_615("textures/atoms/leppa/TestMod/uranium_lightramp");
            class8_1.field_15/*Rimlight*/ = class_238.field_1989.field_81.field_601;
            uranium.field_2291 = class8_1;
            uranium.field_2294/*Metal?*/ = true;
            uranium.QuintAtomType = "TestMod:uranium";
            Uranium = uranium;

            QApi.AddAtomType(Aether);
            QApi.AddAtomType(Uranium);
        }

		private void AddJournalVolume(On.JournalVolumes.orig_method_1052 orig) {
			orig();
            JournalVolume aetherVolume = new JournalVolume();
            aetherVolume.field_2569 = "Volume Beta, Issue I: An Intangible Touch";
            aetherVolume.field_2570 = "Various alchemists have tried - and failed - to purify the long theorised aether. It's contradictory propeties lead many to believe it could not exist, and the isolation of quintessence seemed like an end to this story. Cutting-edge transmutation engines, however, have found that rare molecules could act like they contain aether, revitilizing study into the field.";
            aetherVolume.field_2571 = new Puzzle[5]
            {
                 AetherTutorial,
                 FixatingSuspension,
                 Puzzles.field_2871,
                 Puzzles.field_2872,
                 Puzzles.field_2873
            };
            Array.Resize(ref JournalVolumes.field_2572, JournalVolumes.field_2572.Length + 1);
            JournalVolumes.field_2572[JournalVolumes.field_2572.Length - 1] = aetherVolume;
        }

		private void AddPuzzles(On.Puzzles.orig_method_1285 orig) {
            orig();
            // aether intro puzzle (aether w/ salt -> salt w/ aether)
            AetherTutorial = new Puzzle();
            AetherTutorial.field_2766 = "TM-P01";
            AetherTutorial.field_2767 = class_134.method_254("Beacon");
            AetherTutorial.field_2768 = "Ash, L.";
            Molecule tutorialInput = new class_160().method_393(class_134.method_254("Shrouded Aether"))
                .method_394(Aether)
                .method_395(1, 0).method_394(AtomTypes.field_1675).method_397()
                .method_395(1, -1).method_394(AtomTypes.field_1675).method_397()
                .method_395(0, -1).method_394(AtomTypes.field_1675).method_397()
                .method_395(-1, 0).method_394(AtomTypes.field_1675).method_397()
                .method_395(-1, 1).method_394(AtomTypes.field_1675).method_397()
                .method_395(0, 1).method_394(AtomTypes.field_1675).method_397();
            Molecule tutorialOutput = new class_160().method_393(class_134.method_254("Beacon"))
                .method_394(AtomTypes.field_1675)
                .method_395(1, 0).method_394(Aether).method_397()
                .method_395(1, -1).method_394(Aether).method_397()
                .method_395(0, -1).method_394(Aether).method_397()
                .method_395(-1, 0).method_394(Aether).method_397()
                .method_395(-1, 1).method_394(Aether).method_397()
                .method_395(0, 1).method_394(Aether).method_397();
            AetherTutorial.field_2770 = new PuzzleInputOutput[] {
                new PuzzleInputOutput(tutorialInput)
            };
            AetherTutorial.field_2771 = new PuzzleInputOutput[] {
                new PuzzleInputOutput(tutorialOutput)
            };
            AetherTutorial.field_2773 = Permissions.CoreTools | Permissions.Disposal;
            // aether production puzzle
            // ethereal chains ()
            // fixating suspension
            FixatingSuspension = new Puzzle();
            FixatingSuspension.field_2766 = "TM-P02";
            FixatingSuspension.field_2767 = class_134.method_254("Fixating Suspension");
            FixatingSuspension.field_2768 = "Ash, L.";
            Molecule water = new class_160().method_393(class_134.method_253("Distilled Water", string.Empty))
                .method_394(AtomTypes.field_1679)
                .method_395(-1, 0).method_394(AtomTypes.field_1675).method_397()
                .method_395(1, -1).method_394(AtomTypes.field_1675);
            Molecule indicator = new class_160().method_393(class_134.method_253("Reaction Indicator", string.Empty))
                .method_394(AtomTypes.field_1675)
                .method_395(-1, 0).method_394(AtomTypes.field_1678).method_397()
                .method_395(1, -1).method_394(AtomTypes.field_1677);
            Molecule suspensionOutput = new class_160().method_393(class_134.method_254("Fixating Suspension"))
                .method_394(AtomTypes.field_1675)
                .method_395(1, 0).method_394(Aether).method_397()
                .method_395(-1, 1).method_394(AtomTypes.field_1679)
                .method_395(-1, 0).method_394(AtomTypes.field_1676)
                .method_395(0, -1).method_394(AtomTypes.field_1675)
                .method_395(-1, 0).method_394(Aether).method_397()
                .method_395(1, -1).method_394(AtomTypes.field_1676)
                .method_395(1, 0).method_394(AtomTypes.field_1679)
                .method_395(0, 1);
            FixatingSuspension.field_2770 = new PuzzleInputOutput[] {
                new PuzzleInputOutput(tutorialInput)
            };
            FixatingSuspension.field_2771 = new PuzzleInputOutput[] {
                new PuzzleInputOutput(suspensionOutput),
                new PuzzleInputOutput(water),
                new PuzzleInputOutput(indicator)
            };
            FixatingSuspension.field_2773 = Permissions.CoreTools | Permissions.BaronWheel | Permissions.Disposal | Permissions.Duplication;
            // subtle alloy
        }

		public override void LoadPuzzleContent() {
			Logger.Log("TestMod: Adding stuff");
            NoSymbol = class_235.method_615("textures/atoms/leppa/TestMod/no_symbol");
            AetherSymbol = class_235.method_615("textures/atoms/leppa/TestMod/aether_symbol");
            // by this point, everything should have Happened and I can start messing with parts
            // let's make a triplex bonder in the shape of a multi-bonder
            PartType varTriplex = new PartType();
			varTriplex./*ID*/field_1528 = "test-mod-variant-triplex";
			varTriplex./*Name*/field_1529 = class_134.method_253("Variant Glyph of Triplex-bonding", string.Empty);
			varTriplex./*Desc*/field_1530 = class_134.method_253("The glyph of triplex-bonding creates three separate types of bonds between fire atoms that, when overlaid, become a triplex bond. This variant takes the shape of a multi-bonder instead of a triangle.", string.Empty);
            varTriplex./*Part Icon*/field_1547 = class_235.method_615("textures/parts/icons/leppa/TestMod/var_triplex_bonder");
            varTriplex./*Hovered Part Icon*/field_1548 = class_235.method_615("textures/parts/icons/leppa/TestMod/var_triplex_bonder_hover");
            varTriplex./*Cost*/field_1531 = 25;
            varTriplex./*Is a Glyph*/field_1539 = true;
            varTriplex./*Glow (Shadow)*/field_1549 = class_238.field_1989.field_97.field_384;
            varTriplex./*Stroke (Outline)*/field_1550 = class_238.field_1989.field_97.field_385;
            varTriplex./*Bonding Sites*/field_1538 = new BondSite[3]
            {
              new BondSite(new HexIndex(0, 0), new HexIndex(-1, 1), BondType.Prisma0, AtomTypes./*Fire*/field_1678),
              new BondSite(new HexIndex(0, 0), new HexIndex(1, 0), BondType.Prisma1, AtomTypes./*Fire*/field_1678),
              new BondSite(new HexIndex(0, 0), new HexIndex(0, -1), BondType.Prisma2, AtomTypes./*Fire*/field_1678)
            };
            varTriplex./*Spaces*/field_1540 = new HexIndex[4]
            {
              new HexIndex(0, 0),
              new HexIndex(-1, 1),
              new HexIndex(1, 0),
              new HexIndex(0, -1)
            };
            varTriplex./*Permissions*/field_1551 = Permissions.PrismaBonder;
            VariantTriplex = varTriplex;

            QApi.AddPartType(VariantTriplex, (part, pos, editor, renderer) => {
                class_256 field1287 = class_238.field_1989.field_90./*speed_bonder_base*/field_213;
                Vector2 vector2 = new Vector2(83f, 119f);
                renderer.method_523(field1287, new Vector2(0.0f, -1f), vector2, 0.0f);
                foreach(HexIndex hexIndex in part.method_1159().field_1540) {
                    // render a ring there
                    renderer.method_530(class_238.field_1989.field_90.field_164 /*bonder_shadow*/, hexIndex, 4f);
                    renderer.method_528(class_238.field_1989.field_90.field_163 /*bonder_ring*/, hexIndex, Vector2.Zero);
                    renderer.method_529(class_238.field_1989.field_90.field_203 /*prismabonder_symbol*/, hexIndex, new Vector2(0.0f, -3f));
                }
                for(int i = 0; i < part.method_1159().field_1540.Length; i++) {
                    HexIndex hexIndex = part.method_1159().field_1540[i];
                    if(hexIndex != new HexIndex(0, 0)) {
                        // render a bonder thingy to it
                        int index = i - 1;
                        float num = new HexRotation(index * 2).ToRadians();
                        renderer.method_522(class_238.field_1989.field_90.field_161 /*bonder_bond*/, new Vector2(-28f, 22f), num);
                        renderer.method_531(class_238.field_1989.field_90.field_254[index] /*prismabond_cylinder_lightmaps[i]*/, class_238.field_1989.field_90.field_166/*bond_cylinder_pattern*/, new HexIndex(0, 0), num);
                    }
                }
            });

            QApi.AddPartTypeToPanel(VariantTriplex, PartTypes.field_1775);

            // we also need to make aether worl
            QApi.RunAfterCycle((sim, first) => {
                if(!first) {
                    List<Molecule> toRemove = new List<Molecule>();
                    var molecules = new DynamicData(sim).Get<List<Molecule>>("field_3823");
                    foreach(var molecule in molecules) {
                        bool hasAether = false, hasNonAether = false;
                        foreach(var atom in molecule.method_1100())
                            if(atom.Value.field_2275.Equals(Aether))
                                hasAether = true;
                            else
                                hasNonAether = true;
                        if(hasAether && !hasNonAether)
                            toRemove.Add(molecule);
                    }
                    foreach(var it in toRemove) {
                        foreach(var atom in it.method_1100()) {
                            var seb = new DynamicData(sim).Get<SolutionEditorBase>("field_3818");
                            //seb.field_3935.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(atom.Key) + new Vector2(147f, 47f), class_238.field_1989.field_90.field_242, 30f, Vector2.Zero, 0.0f));
                            seb.field_3936.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(atom.Key) + new Vector2(80f, 0.0f), class_238.field_1989.field_90.field_240, 30f, Vector2.Zero, 0.0f));
                        }
                        molecules.Remove(it);
                    }
                }
            });
        }

		public override void Unload() {}

		public override void PostLoad() {
            return;
        }

		public override Type SettingsType => typeof(TestSettings);

		public class TestSettings {

            [SettingsLabel("Show Aether Symbol")]
            public bool AetherSymbol = false;

            [SettingsLabel("Do Stuff")]
            public Keybinding Stuff = new Keybinding();
        }

		public override void ApplySettings() {
			base.ApplySettings();
            Aether.field_2287/*Symbol*/ = ((TestSettings)Settings).AetherSymbol ? AetherSymbol : NoSymbol;
        }
	}
}
