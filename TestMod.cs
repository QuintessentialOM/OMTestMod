using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using Quintessential;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace TestMod {

    using PartType = class_206;
    using Permissions = enum_2;
    using BondType = enum_143;
    using BondSite = class_240;
    using AtomTypes = class_263;
    using PartTypes = class_72;

    public class TestMod : QuintessentialMod {

        private static IDetour hook_SolutionEditorBase_method_2000;

        public static PartType VariantTriplex;

		public override void Load() {
			Logger.Log("TestMod: Loading!");
            // this is where hooks go
            IL.SolutionEditorPartsPanel.method_269 += (ILContext ctx) => {
                ILCursor cursor = new ILCursor(ctx);
                if(cursor.TryGotoNext(MoveType.After,
                                      instr => instr.MatchLdsfld("class_72", "field_857"),
                                      instr => true, // too much effort
                                      instr => instr.MatchDup())) {
                    // add a Ldloc.s, Ldsfld, Callvirt, Dup, for VariantTriplex
                    cursor.Emit(OpCodes.Ldloc_S, ctx.Method.Body.Variables[4]);
                    cursor.Emit(OpCodes.Ldsfld, typeof(TestMod).GetField("VariantTriplex"));
                    cursor.EmitDelegate<Action< Action<List<PartTypeForToolbar>, PartType>, List<PartTypeForToolbar>, PartType>>((action, list, type) => {
                        action(list, type);
                    });
                    cursor.Emit(OpCodes.Dup);
                } else {
                    Logger.Log("TestMod: Failed to add Variant Triplex to toolbar!");
                    throw new Exception("Couldn't add Variant Triplex to toolbar!");
                }
            };
            Logger.Log("TestMod: Added IL hook!");
        }

        private delegate void orig_SolutionEditorBase_method_2000(SolutionEditorBase self, Part param_5573, Vector2 param_5574);
        private static void OnSolutionEditorBaseMethod2000(orig_SolutionEditorBase_method_2000 orig, SolutionEditorBase self, Part part, Vector2 pos) {
            orig(self, part, pos);
			class_195 class195 = self.method_1993(part, pos);
			class_273 class273 = new class_273(class195.field_1856, class195.field_1857, Editor.method_926());
			if(part.method_1163() == VariantTriplex) {
				class_66 field1287 = class_227.field_2041.field_920./*speed_bonder_base*/field_1287;
				Vector2 vector2 = new Vector2(83f, 119f);
				class273.method_716(field1287, new Vector2(0.0f, -1f), vector2, 0.0f);
				foreach(HexIndex hexIndex in part.method_1163().field_1909) {
					// render a ring there
					class273.method_723(class_227.field_2041.field_920./*bonder_shadow*/field_1238, hexIndex, 4f);
					class273.method_721(class_227.field_2041.field_920./*bonder_ring*/field_1237, hexIndex, Vector2.Zero);
                    class273.method_722(class_227.field_2041.field_920./*prismabonder_symbol*/field_1277, hexIndex, new Vector2(0.0f, -3f));
                }
				for(int i = 0; i < part.method_1163().field_1909.Length; i++) {
					HexIndex hexIndex = part.method_1163().field_1909[i];
					if(hexIndex != new HexIndex(0, 0)) {
						// render a bonder thingy to it
						int index = i - 1;
						float num = new HexRotation(index * 2).ToRadians();
						class273.method_715(class_227.field_2041.field_920./*bonder_bond*/field_1235, new Vector2(-28f, 22f), num);
						class273.method_724(class_227.field_2041.field_920./*prismabond_cylinder_lightmaps[i]*/field_1328[index], class_227.field_2041.field_920./*bond_cylinder_pattern*/field_1240, new HexIndex(0, 0), num);

						//HexIndex hexIndex2 = field1909[index];
						//float radians = new HexRotation(index * 2).ToRadians();
						//class273.method_724(class_227.field_2041.field_920.field_1328[index], class_227.field_2041.field_920.field_1240, hexIndex2, radians);
					}
				}
			}
		}

		public override void PostLoad() {
			Logger.Log("TestMod: PostLoading!");
            // by this point, everything should have Happened and I can start messing with parts
            // let's make a triplex bonder in the shape of a multi-bonder
            PartType varTriplex = new PartType();
			varTriplex./*ID*/field_1897 = "test-mod-variant-triplex";
			varTriplex./*Name*/field_1898 = class_32.method_112("Variant Glyph of Triplex-bonding", string.Empty);
			varTriplex./*Desc*/field_1899 = class_32.method_112("The glyph of triplex-bonding creates three separate types of bonds between fire atoms that, when overlaid, become a triplex bond. This variant takes the shape of a multi-bonder instead of a triangle.", string.Empty);
            varTriplex./*Part Icon*/field_1916 = class_227.field_2041.field_920.field_1319.field_1406;
            varTriplex./*Hovered Part Icon*/field_1917 = class_227.field_2041.field_920.field_1319.field_1407;
            varTriplex./*Cost*/field_1900 = 25;
            varTriplex./*Not An Arm*/field_1908 = true;
            varTriplex./*Glow (Shadow)*/field_1918 = class_227.field_2041.field_927.field_953;
            varTriplex./*Stroke (Outline)*/field_1919 = class_227.field_2041.field_927.field_954;
            varTriplex./*Bonding Sites*/field_1907 = new BondSite[3]
            {
              new BondSite(new HexIndex(0, 0), new HexIndex(-1, 1), BondType.Prisma0, AtomTypes./*Fire*/field_2184),
              new BondSite(new HexIndex(0, 0), new HexIndex(1, 0), BondType.Prisma1, AtomTypes./*Fire*/field_2184),
              new BondSite(new HexIndex(0, 0), new HexIndex(0, -1), BondType.Prisma2, AtomTypes./*Fire*/field_2184)
            };
            varTriplex./*Spaces*/field_1909 = new HexIndex[4]
            {
              new HexIndex(0, 0),
              new HexIndex(-1, 1),
              new HexIndex(1, 0),
              new HexIndex(0, -1)
            };
            varTriplex./*Permissions*/field_1920 = Permissions.PrismaBonder;
            VariantTriplex = varTriplex;
            Array.Resize(ref PartTypes.field_867, 26);
            PartTypes.field_867[25] = VariantTriplex;

            // since method_2000 is private, hookgen didn't pick it up (sad)
            // gotta add the hook here or it breaks on a static initializer
            hook_SolutionEditorBase_method_2000 = new Hook(
                typeof(SolutionEditorBase).GetMethod("method_2000", BindingFlags.NonPublic | BindingFlags.Instance),
                typeof(TestMod).GetMethod("OnSolutionEditorBaseMethod2000", BindingFlags.NonPublic | BindingFlags.Static)
            );
            Logger.Log("TestMod: Added On hook!");
        }

        public override void Unload() {
			Logger.Log("TestMod: Unloading!");
            hook_SolutionEditorBase_method_2000?.Dispose();
        }
	}
}
