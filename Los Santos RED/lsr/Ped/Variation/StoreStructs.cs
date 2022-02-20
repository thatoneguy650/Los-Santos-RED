//struct OutfitQueryResult
//{
//	alignas(8) uint32_t lockHash;
//	alignas(8) uint32_t uniqueNameHash;
//	alignas(8) uint16_t cost;
//	alignas(8) int32_t numIncludedProps;
//	alignas(8) int32_t numIncludedComponents;
//	alignas(8) int32_t eShopEnum;
//	alignas(8) int32_t eCharacter;
//	alignas(8) char textLabel[64];
//};

//struct OutfitComponentVariantResult
//{
//	alignas(8) uint32_t nameHash;
//	alignas(8) int32_t enumValue;
//	alignas(8) int32_t eCompType;
//};

//struct OutfitPropVariantResult
//{
//	alignas(8) uint32_t nameHash;
//	alignas(8) int32_t enumValue;
//	alignas(8) int32_t eAnchorPoint;
//};
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct ComponentQueryResult
{
	public uint lockHash;
	public uint uniqueNameHash;
	public int locate;
	public int unk1;
	public int textureIndex;
	public int unk2;
	public int eCompType;
	public int unk3;
	public int eCharacter;
	public fixed char textLabel[64];
};


unsafe struct ComponentStoreStruct
{
    public int lockHash;

	public int _pad1;
	public int hash;

	public int _pad2;
	public int locate; // <locate value="..." /> in clothing data

	public int _pad3;
	public int drawable;

	public int _pad4;
	public int texture;

	public int _pad5;
	public int f_5;

	public int _pad6;
	public int componentType;

	public int _pad7;
	public int f_7;

	public int _pad8;
	public int f_8;

	public int _pad9;
	public fixed char gxt[64];
} ;

//struct PropQueryResult
//{
//	alignas(8) uint32_t lockHash;
//	alignas(8) uint32_t uniqueNameHash;
//	alignas(8) int32_t locate;
//	alignas(8) int32_t unk1;
//	alignas(8) int32_t textureIndex;
//	alignas(8) int32_t unk2;
//	alignas(8) int32_t eAnchorPoint;
//	alignas(8) int32_t unk3;
//	alignas(8) int32_t eCharacter;
//	alignas(8) char textLabel[64];
//};