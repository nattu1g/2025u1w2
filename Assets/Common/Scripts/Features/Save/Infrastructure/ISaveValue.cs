namespace Common.Features.Save
{
    public interface ISaveValue
    {
        public string ToJson();
        public void FromJson(string jsonStr);
    }
}
