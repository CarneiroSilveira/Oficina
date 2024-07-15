using Views;

namespace Oficina_Atecubanos;
using Views;
static class Program
{
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new ViewMenu());
    }
}