using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdministradorProcesos
{
    /// <summary>
    /// Módulo de monitoreo de procesos del sistema.
    /// Autor: Gian Marco Escobar Pérez
    /// Rama: feature/modulo-monitoreo
    /// Curso: Sistemas Operativos - UMG
    /// </summary>
    public static class MonitorProcesos
    {
        // Factor de conversión de bytes a megabytes.
        // El .0 fuerza aritmética de punto flotante: si fuera entero,
        // C# haría división entera y los procesos pequeños darían 0.
        private const double BytesPorMB = 1024.0 * 1024.0;

        /// <summary>
        /// Punto de entrada del módulo. Lo llama el menú principal (opción 1).
        /// Obtiene el snapshot de procesos y lo imprime como tabla formateada.
        /// </summary>
        public static void ListarProcesos()
        {
            Console.Clear();
            Console.WriteLine("==========================================================");
            Console.WriteLine("           MONITOR DE PROCESOS EN EJECUCIÓN");
            Console.WriteLine("==========================================================");
            Console.WriteLine();

            List<InfoProceso> procesos = ObtenerProcesos();

            if (procesos.Count == 0)
            {
                Console.WriteLine("No se pudo obtener información de procesos.");
                return;
            }

            // Encabezado. Los números en {campo,-8} definen el ancho de columna:
            // negativo = alineado a la izquierda, positivo = a la derecha.
            Console.WriteLine($"{"PID",-8} {"NOMBRE DEL PROCESO",-34} {"MEMORIA (MB)",14}");
            Console.WriteLine(new string('-', 58));

            foreach (InfoProceso p in procesos)
            {
                // :N2 formatea el double con separador de miles y 2 decimales.
                Console.WriteLine($"{p.Pid,-8} {Truncar(p.Nombre, 34),-34} {p.MemoriaMB,14:N2}");
            }

            Console.WriteLine(new string('-', 58));
            Console.WriteLine();
            Console.WriteLine($"Total de procesos listados : {procesos.Count}");
            Console.WriteLine($"Memoria física total (RAM) : {procesos.Sum(p => p.MemoriaMB):N2} MB");
        }

        /// <summary>
        /// Toma un snapshot de la tabla de procesos del sistema operativo
        /// y lo transforma en una lista de objetos propios (InfoProceso).
        /// </summary>
        public static List<InfoProceso> ObtenerProcesos()
        {
            List<InfoProceso> lista = new List<InfoProceso>();

            // GetProcesses() devuelve una FOTOGRAFÍA del instante actual,
            // no una vista viva de la tabla de procesos del kernel.
            Process[] snapshot = Process.GetProcesses();

            foreach (Process proceso in snapshot)
            {
                try
                {
                    lista.Add(new InfoProceso
                    {
                        Pid = proceso.Id,
                        Nombre = proceso.ProcessName,
                        // WorkingSet64 = bytes residentes en RAM física.
                        MemoriaMB = proceso.WorkingSet64 / BytesPorMB
                    });
                }
                catch (System.ComponentModel.Win32Exception)
                {
                    // Procesos protegidos del sistema: acceso denegado.
                    // Se omiten en lugar de abortar el recorrido completo.
                }
                catch (InvalidOperationException)
                {
                    // El proceso terminó entre el snapshot y la lectura.
                }
                finally
                {
                    // Libera el handle nativo que el objeto Process mantiene abierto.
                    proceso.Dispose();
                }
            }

            // Ordenado de mayor a menor consumo de RAM.
            return lista.OrderByDescending(p => p.MemoriaMB).ToList();
        }

        /// <summary>
        /// Recorta nombres largos para que la tabla no se desalinee.
        /// </summary>
        private static string Truncar(string texto, int maximo)
        {
            if (string.IsNullOrEmpty(texto)) return string.Empty;
            return texto.Length <= maximo ? texto : texto.Substring(0, maximo - 3) + "...";
        }
    }

    /// <summary>
    /// Estructura de datos que representa un proceso ya procesado.
    /// Separar esto del objeto Process permite reutilizar los datos
    /// en otros módulos sin volver a consultar al sistema operativo.
    /// </summary>
    public class InfoProceso
    {
        public int Pid { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public double MemoriaMB { get; set; }
    }
}
