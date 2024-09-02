namespace Arranque_de_Router
{
    public class Router
    {

        #region Properties

        public string Name { get; set; } = string.Empty;

        public bool IsPowerOn { get; private set; }

        public string ConfigRegister { get; set; } = string.Empty;

        public string IOSLocation { get; private set; } = string.Empty;

        public string ConfigFileLocation { get; private set; } = string.Empty;

        public bool InitialInterfaces { get; private set; } = true;

        private Random random = new Random();

        #endregion

        public Router(string name, string configRegister = "0x2102")
        {
            Name = name;
            IsPowerOn = false;
            ConfigRegister = configRegister;
        }


        private void PrintErrorMessage(string message, string extra = "")
        {
            Console.WriteLine($"\n#########################################\n");
            Console.WriteLine($"Error: {message}.. {extra}\n");
            Console.WriteLine($"#########################################\n");
        }
        public async Task PowerOn()
        {
            Console.WriteLine($"Iniciando el encendido del router...");
            try
            {
                await Post();
                await LoadBootstrap();
                await LoadConfigurationRegister();
                await LoadIOS();
                await LoadConfigFile();
                if (InitialInterfaces)
                {
                    await InitializeInterfaces();
                    Console.WriteLine("Encendido exitoso");
                }

            }
            catch (Exception ex)
            {
                PrintErrorMessage(ex.Message, "Reiniciando router");
                await PowerOn();
            }
        }

        private async Task Post()
        {
            Console.WriteLine("Realizando POST...");
            await Task.Delay(1000);
            var number = random.Next(1, 10);
            if (number == 5)
                throw new Exception("Fallo en el POST");
        }

        private async Task LoadBootstrap()
        {
            Console.WriteLine("Cargando bootstrap...");
            await Task.Delay(1500);

        }

        private async Task LoadConfigurationRegister()
        {
            Console.WriteLine($"Cargando registro de configuración: {ConfigRegister}");
            await Task.Delay(1000);


            IOSLocation = InterpretBootField(ConfigRegister);
            ConfigFileLocation = InterpretConfigFileField(ConfigRegister);

            Console.WriteLine($"Boot field: {ConfigRegister} buscando IOS en: {IOSLocation}");
            Console.WriteLine($"Buscando archivo de configuración en: {ConfigFileLocation}");
        }

        private string InterpretBootField(string bootField)
        {
            switch (bootField)
            {
                case "0x2100":
                    return "ROM";
                case "0x2101":
                    return "ROM";
                case "0x2102":
                case "0x2103":
                    return "Flash";
                case "0x2104":
                    return "TFTP";
                default:
                    return "Flash";
            }
        }

        private string InterpretConfigFileField(string bootField)
        {
            switch (bootField)
            {
                case "0x2100":
                case "0x2101":
                    return "ROM";
                case "0x2102":
                case "0x2103":
                    return "NVRAM";
                case "0x2104":
                    return "TFTP";
                default:
                    return "NVRAM";
            }
        }

        private async Task LoadIOS()
        {
            try
            {
                Console.WriteLine($"Cargando IOS desde {IOSLocation}...");
                await Task.Delay(3000);

                var numberRandom = random.Next(1, 10);
                //var numberRandom = 5;
                if (IOSLocation == "Flash" && numberRandom == 5)
                    throw new Exception("Error al cargar IOS desde Flash");

                if (IOSLocation == "TFTP" && numberRandom == 5)
                    throw new Exception("Error al cargar IOS desde TFTP");

                Console.WriteLine("\nSelf decompressing the image : ");
                Console.WriteLine("########################################################");
                Console.WriteLine("[OK]\n");
            }
            catch (Exception ex)
            {
                PrintErrorMessage(ex.Message);

                if (IOSLocation == "Flash")
                {
                    Console.WriteLine("Intentando cargar IOS desde TFTP...");
                    IOSLocation = "TFTP";
                    await LoadIOS();
                }
                else
                {
                    Console.WriteLine("Entrando en modo de recuperación...");
                    await Task.Delay(3000);
                    Console.WriteLine("Modo de recuperación iniciado. Por favor, cargue el IOS manualmente.");
                    Environment.Exit(1);
                }
            }
        }

        private async Task LoadConfigFile()
        {
            try
            {
                Console.WriteLine($"Cargando archivo de configuración desde {ConfigFileLocation}...");
                await Task.Delay(2000);

                var numberRandom = random.Next(1, 10);
                //var numberRandom = 5;

                if (ConfigFileLocation == "NVRAM" && numberRandom == 5)
                    throw new Exception("Error al cargar archivo de configuración desde NVRAM");

                if (ConfigFileLocation == "TFTP" && numberRandom == 5)
                    throw new Exception("Error al cargar archivo de configuración desde TFTP");

                Console.WriteLine("Archivo de configuración cargado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");

                if (ConfigFileLocation == "NVRAM")
                {
                    Console.WriteLine("Intentando cargar archivo de configuración desde TFTP...");
                    ConfigFileLocation = "TFTP";
                    await LoadConfigFile();
                }
                else
                {
                    Console.WriteLine("Entrando en modo de setup...");
                    await Task.Delay(3000);
                    Console.WriteLine("--- System Configuration Dialog ---");
                    Console.WriteLine("Would you like to enter the initial configuration dialog? [yes/no]:");
                    InitialInterfaces = false;
                }
            }
        }

        private async Task InitializeInterfaces()
        {
            Console.WriteLine("Inicializando interfaces...");
            await Task.Delay(2000);
        }
    }
}
