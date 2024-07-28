// See https://aka.ms/new-console-template for more information
class OpenClient
{
    static void Main(string[] args)
    {
	int port = 8080;
	if (args[0] == "port"){
		try 
		{
			port = Int32.Parse(args[1]);
		}
		catch (FormatException) 
		{
			Console.WriteLine("Especificar un puerto válido");
			Environment.Exit(0);
		}
	} else {
		Console.WriteLine("No se especificó el puerto, saliendo...");
		Environment.Exit(0);
	}

	Console.WriteLine("[OK] Abriendo puerto {0}", port);

    }
}
