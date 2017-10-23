namespace SomeDependencies
{
    public class Convertor : IConvertor
    {
        public string Execute(string input)
        {
            return $"{input}_converted";
        }
    }
}
