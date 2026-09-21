using System;
using System.Diagnostics;

public class ControlProcesos
{
    public static void FinalizarProceso(int pid)
    {
        try
        {
            Process proceso = Process.GetProcessById(pid);
            proceso.Kill();

            Console.WriteLine($"El proceso con PID {pid} fue finalizado correctamente.");
        }
        catch (ArgumentException)
        {
            Console.WriteLine($"No existe ningún proceso con el PID {pid}.");
        }
        catch (System.ComponentModel.Win32Exception)
        {
            Console.WriteLine($"No se puede finalizar el proceso con PID {pid}. Puede ser un proceso protegido del sistema o no tener permisos suficientes.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ocurrió un error: {ex.Message}");
        }
    }

    public static void IniciarProceso()
    {
        Console.Write("Ingrese el nombre o ruta del ejecutable: ");
        string? ejecutable = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(ejecutable))
        {
            Console.WriteLine("No se ingresó ningún ejecutable.");
            return;
        }

        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = ejecutable,
                UseShellExecute = true
            });

            Console.WriteLine($"Se inició el proceso: {ejecutable}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"No se pudo iniciar el proceso: {ex.Message}");
        }
    }
}