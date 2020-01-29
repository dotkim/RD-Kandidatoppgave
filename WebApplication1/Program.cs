namespace WebApplication1
{
    public class Program : SelfhostBase
    {
        public static int Main(string[] args)
        {
            return StartService<Startup>(args);
        }
    }
}
