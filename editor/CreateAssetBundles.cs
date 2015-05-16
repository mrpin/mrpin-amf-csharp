using UnityEditor;

public class CreateAssetBundles
{
	[MenuItem ("Assets/Asset Bundle/Build")]
	static void BuildAllAssetBundles ()
	{
		BuildPipeline.BuildAssetBundles ("AssetsBundles");
	}
}