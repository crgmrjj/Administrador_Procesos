using System;
using System.ComponentModel;
using System.Diagnostics;

namespace AdministradorProcesos
{
    public class DetalleProcesos
    {
        public static void Ejecutar()
        {
            Console.Write("\nIngrese el PID del proceso: ");
            var entrada = Console.ReadLine();

            if (!int.TryParse(entrada, out int pid))
            {
                Console.WriteLine("PID inválido. Debe ser un número entero.");
                return;
            }

            Process proceso;
            try
            {
                proceso = Process.GetProcessById(pid);
            }
            catch (ArgumentException)
            {
                Console.WriteLine($"No existe ningún proceso activo con PID {pid}.");
                return;
            }

            using (proceso)
            {
                MostrarDetalle(proceso);

                Console.Write("\n¿Desea cambiar la prioridad de este proceso? (s/n): ");
                var respuesta = Console.ReadLine();

                if (respuesta != null && respuesta.Trim().ToLower() == "s")
                {
                    CambiarPrioridad(proceso);
                }
            }
        }

        private static void MostrarDetalle(Process p)
        {
            Console.WriteLine("\n=========== DETALLE DEL PROCESO ===========");
            Console.WriteLine($"Nombre:            {p.ProcessName}");
            Console.WriteLine($"PID:               {p.Id}");
            Console.WriteLine($"Memoria (RAM):     {LeerDato(() => (p.WorkingSet64 / 1024.0 / 1024.0).ToString("F2") + " MB")}");
            Console.WriteLine($"Hilos:             {LeerDato(() => p.Threads.Count.ToString())}");
            Console.WriteLine($"Prioridad (clase): {LeerDato(() => p.PriorityClass.ToString())}");
            Console.WriteLine($"Prioridad base:    {LeerDato(() => p.BasePriority.ToString())}");
            Console.WriteLine($"Hora de inicio:    {LeerDato(() => p.StartTime.ToString("dd/MM/yyyy HH:mm:ss"))}");
            Console.WriteLine($"Tiempo de CPU:     {LeerDato(() => p.TotalProcessorTime.ToString(@"hh\:mm\:ss"))}");
            Console.WriteLine("===========================================");
        }

        // Lee una propiedad protegiendo contra procesos del sistema o ya finalizados.
        private static string LeerDato(Func<string> lectura)
        {
            try
            {
                return lectura();
            }
            catch (Win32Exception)
            {
                return "Acceso denegado (proceso protegido)";
            }
            catch (InvalidOperationException)
            {
                return "El proceso ya finalizó";
            }
        }

        private static void CambiarPrioridad(Process p)
        {
            Console.WriteLine("\nSeleccione la nueva prioridad:");
            Console.WriteLine("1. Baja (Idle)");
            Console.WriteLine("2. Debajo de lo normal (BelowNormal)");
            Console.WriteLine("3. Normal");
            Console.WriteLine("4. Arriba de lo normal (AboveNormal)");
            Console.WriteLine("5. Alta (High)");
            Console.Write("Opción: ");

            ProcessPriorityClass nueva;
            switch (Console.ReadLine())
            {
                case "1": nueva = ProcessPriorityClass.Idle; break;
                case "2": nueva = ProcessPriorityClass.BelowNormal; break;
                case "3": nueva = ProcessPriorityClass.Normal; break;
                case "4": nueva = ProcessPriorityClass.AboveNormal; break;
                case "5": nueva = ProcessPriorityClass.High; break;
                default:
                    Console.WriteLine("Opción no válida. No se cambió la prioridad.");
                    return;
            }

            try
            {
                p.PriorityClass = nueva;
                p.Refresh();
                Console.WriteLine($"Prioridad cambiada correctamente a: {p.PriorityClass}");
            }
            catch (Win32Exception)
            {
                Console.WriteLine("Acceso denegado. Para este proceso ejecute el programa como administrador.");
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("El proceso finalizó antes de poder cambiar su prioridad.");
            }
        }
    }
}