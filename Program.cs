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
                Console.WriteLine("==========================================================");
                Console.WriteLine("           ADMINISTRADOR DE PROCESOS - C#                 ");
                Console.WriteLine("==========================================================");
                Console.WriteLine("1. Listar procesos activos (Gian)");
                Console.WriteLine("2. Iniciar un nuevo proceso (Eduardo)");
                Console.WriteLine("3. Terminar / Matar un proceso por PID (Eduardo)");
                Console.WriteLine("4. Ver detalle o cambiar prioridad (Ariel - En desarrollo)");
                Console.WriteLine("5. Salir");
                Console.WriteLine("==========================================================");
                Console.Write("Seleccione una opción: ");

                string? opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        // Llamada al módulo de Gian
                        MonitorProcesos.ListarProcesos();
                        break;

                    case "2":
                        // Llamada al módulo de Eduardo (Iniciar)
                        Console.Clear();
                        Console.WriteLine("--- INICIAR PROCESO ---");
                        ControlProcesos.IniciarProceso();
                        break;

                    case "3":
                        // Llamada al módulo de Eduardo (Finalizar)
                        Console.Clear();
                        Console.WriteLine("--- FINALIZAR PROCESO ---");
                        Console.Write("Ingrese el PID del proceso a finalizar: ");
                        if (int.TryParse(Console.ReadLine(), out int pid))
                        {
                            ControlProcesos.FinalizarProceso(pid);
                        }
                        else
                        {
                            Console.WriteLine("PID inválido. Debe ingresar un valor numérico.");
                        }
                        break;

                    case "4":
                        Console.Clear();
                        Console.WriteLine("Módulo de Ariel en desarrollo (prioridades y detalles).");
                        break;

                    case "5":
                        continuar = false;
                        Console.Clear();
                        Console.WriteLine("==========================================================");
                        Console.WriteLine("       Saliendo del Administrador de Procesos...          ");
                        Console.WriteLine("==========================================================");
                        break;

                    default:
                        Console.WriteLine("Opción no válida.");
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
