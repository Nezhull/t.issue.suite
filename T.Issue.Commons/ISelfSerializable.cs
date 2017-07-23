using System.IO;

namespace T.Issue.Commons
{
    public interface ISelfSerializable
    {
        void Serialize(BinaryWriter output);
	
	    void Deserialize(BinaryReader input);
    }
}
