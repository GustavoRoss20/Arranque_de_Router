namespace Arranque_de_Router
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Router router = new Router("CISCO", "0x2104");
            await router.PowerOn();
        }
    }
}