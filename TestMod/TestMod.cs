using Quintessential;

namespace TestMod {

    using PartType = class_139;
    using Permissions = enum_149;
    using BondType = enum_126;
    using BondSite = class_222;
    using AtomTypes = class_175;
    using PartTypes = class_191;

	public class TestMod : QuintessentialMod {

        public static PartType VariantTriplex;

		public override void Load() {}

		public override void PostLoad() {
			Logger.Log("TestMod: Adding part");
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
        }

        public override void Unload() {}
    }
}
