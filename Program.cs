using System;

namespace AdministradorProcesos
{
    class Program
    {
        static void Main(string[] args)
        {
            bool continuar = true;

            while (continuar)
            {
                Console.Clear();
                Console.WriteLine("=============================================");
                Console.WriteLine("       ADMINISTRADOR DE PROCESOS en C#       ");
                Console.WriteLine("=============================================");
                Console.WriteLine("1. Listar procesos activos (Gian)");
                Console.WriteLine("2. Iniciar un nuevo proceso (Eduardo)");
                Console.WriteLine("3. Terminar / Matar un proceso por PID (Eduardo)");
                Console.WriteLine("4. Ver detalle o cambiar prioridad (Ariel)");
                Console.WriteLine("5. Salir");
                Console.WriteLine("=============================================");
                Console.Write("Seleccione una opcion: ");

                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        Console.WriteLine("\n[Pendiente: Llamar a MonitorProcesos]");
                        break;
                    case "2":
                        Console.WriteLine("\n[Pendiente: Iniciar proceso con ControlProcesos]");
                        break;
                    case "3":
                        Console.WriteLine("\n[Pendiente: Matar proceso con ControlProcesos]");
                        break;
                    case "4":
                        Console.WriteLine("\n[Pendiente: Detalle con DetalleProcesos]");
                        break;
                    case "5":
                        continuar = false;
                        Console.WriteLine("Saliendo del sistema...");
                        break;
                    default:
                        Console.WriteLine("Opción no válida. Presione cualquier tecla para continuar.");
                        break;
                }

                if (continuar)
                {
                    Console.WriteLine("\nPresione cualquier tecla para volver al menú...");
                    Console.ReadKey();
                }
            }
        }
    }
}
