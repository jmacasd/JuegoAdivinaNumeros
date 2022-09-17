using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace JuegoAdivinaNumeros
{
    class Program
    {
        #region Declaraciones

        static string opcionMenu = "";
        static bool opcionValida = false;

        static bool esIntentoExitoso = false;

        static string lineaDePrueba = "1";
        static string verificaLineaDePrueba = "11";

        static List<String> listaDeIntentos;
        static List<ConsoleColor> listaDeColores;

        static Random numeroAleatorio = new Random();

        #endregion Declaraciones

        #region Principal

        static void Main(string[] args)
        {
            Inicializar();

            PantallaDeBienvenida();

            MostrarMenu();

            while (opcionValida == false)
            {
                switch (opcionMenu)
                {
                    case "1":
                        {
                            opcionValida = true;
                            ModoJuegoSolitario();
                        }
                        break;
                    case "2":
                        {
                            opcionValida = true;
                            ModoJuegoVersusMaquina();
                        }
                        break;
                    default:
                        {
                            Console.WriteLine("Opción inválida.");
                            Console.WriteLine("");
                            MostrarMenu();
                        }
                        break;
                }
            }

            Console.WriteLine("Fin.");
            Console.ReadLine();
        }

        #endregion Principal

        #region Metodos

        #region Carga_datos

        static void Inicializar()
        {
            CargarListaDeColores();
        }

        static void CargarListaDeColores()
        {
            listaDeColores = new List<ConsoleColor>();

            Type type = typeof(ConsoleColor);

            foreach (var name in Enum.GetNames(type))
            {
                listaDeColores.Add((ConsoleColor)Enum.Parse(type, name));
            }
        }

        #endregion Carga_datos

        #region Pantallas

        static void PantallaDeBienvenida()
        {
            Console.WriteLine("Bienvenido a Adivina frase de números.");
            Console.WriteLine("Este juego consite en intentar adivinar la siguiente frase de números:");
            Console.WriteLine("");
            //Console.WriteLine("  1       (Número o renglón inicial)");
            //Console.WriteLine("  11      (Este renglón describe el anterior)");
            //Console.WriteLine("  21      (y este otro describe el anterior)");
            //Console.WriteLine("  1211    (continuando...)");
            Console.WriteLine("  1");
            Console.WriteLine("  11");
            Console.WriteLine("  21");
            Console.WriteLine("  1211");
            Console.WriteLine("");
            Console.WriteLine("Este juego tiene dos modalidades: en solitario y versus máquina.");
            Console.WriteLine("");
            Console.WriteLine("Solitario: Comienzas con uno o dos renglones iniciales y deberás continuar la trama de los siguientes renglones.");
            Console.WriteLine("");
            Console.WriteLine("Versus máquina: Podras tener un duelo contra la computadora, tendrás oportunidad de elegir quien iniciará la partida.");
            Console.WriteLine("");
            Console.WriteLine("Diviértete con este sencillo e intrigante juego; deberás mantener paciencia y no equivocarte.");
            Console.WriteLine("");
        }

        static void MostrarMenu()
        {
            Console.WriteLine("Elige la modalidad:");
            Console.WriteLine("");
            Console.WriteLine("  1. Solitario");
            Console.WriteLine("  2. Versus máquina");
            Console.WriteLine("");
            Console.Write("Opción: ");
            opcionMenu = Console.ReadLine();
        }

        #endregion Pantallas

        #region Modo_juego

        static void ModoJuegoSolitario()
        {
            listaDeIntentos = new List<string>();

            Console.Write("Escribe un número entero inicial o en blanco para uno aleatorio:");
            Console.WriteLine("");

            string initialNumber = Console.ReadLine();

            if (initialNumber == "")
            {
                initialNumber = numeroAleatorio.Next(int.MaxValue).ToString();
            }

            listaDeIntentos.Add(initialNumber);

            Console.WriteLine("");
            EscribirLinea(initialNumber, "0B");

            string computedResult = AdivinarSiguienteLinea(initialNumber);

            string attempt = "#";

            while (attempt.Contains("x") == false)
            {
                attempt = Console.ReadLine();

                if (attempt == computedResult)
                {
                    esIntentoExitoso = true;

                    listaDeIntentos.Add(attempt);

                    computedResult = AdivinarSiguienteLinea(attempt);

                    EscribirIntentoUsuario(listaDeIntentos[listaDeIntentos.Count - 1], esIntentoExitoso);
                }
                else
                {
                    esIntentoExitoso = false;

                    listaDeIntentos.Add(attempt);

                    EscribirIntentoUsuario(listaDeIntentos[listaDeIntentos.Count - 1], esIntentoExitoso);
                }
            }
        }

        static void ModoJuegoVersusMaquina()
        {
            listaDeIntentos = new List<string>();

            bool numeroInicialValido = false;
            string numeroInicial = "";
            string turnoAnterior = "";

            // 1. Validar turno inicial entre usuario y maquina

            while (numeroInicialValido == false)
            {
                int semillaAleatoria = (int)(DateTime.Now.ToBinary() / 2);

                // Validar turno inicial entre usuario y maquina

                Console.Write("Escribe un número entero inicial o deja en blanco para uno aleatorio: ");
                numeroInicial = Console.ReadLine();

                if (numeroInicial.Length == 0)
                {
                    numeroAleatorio = new Random(semillaAleatoria);

                    numeroInicial = numeroAleatorio.Next().ToString();

                    numeroInicialValido = true;

                    listaDeIntentos.Add(numeroInicial);

                    Console.WriteLine("");

                    EscribirPrimeraLinea(numeroInicial);

                    turnoAnterior = "maquina";
                }

                try
                {
                    if (numeroInicialValido == true)
                    {
                        continue;
                    }
                    else
                    {
                        int probar = Convert.ToInt32(numeroInicial);

                        if (probar > 0)
                        {
                            numeroInicial = Convert.ToInt32(numeroInicial).ToString();

                            numeroInicialValido = true;

                            listaDeIntentos.Add(numeroInicial);

                            Console.WriteLine("");

                            EscribirPrimeraLinea(numeroInicial);

                            turnoAnterior = "usuario";
                        }
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("");
                }
            }

            bool romperCiclo = false;
            string lineaAdivinada = "";

            // 2. Ejecutar ciclo de usuario versus maquina
            while (romperCiclo == false)
            {
                string ultimaLinea = "";

                if (turnoAnterior == "usuario")
                {
                    Console.WriteLine("");

                    ultimaLinea = listaDeIntentos[listaDeIntentos.Count - 1];

                    lineaAdivinada = AdivinarSiguienteLinea(ultimaLinea);

                    if (lineaAdivinada == AdivinarSiguienteLinea(ultimaLinea))
                    {
                        esIntentoExitoso = true;

                        listaDeIntentos.Add(lineaAdivinada);

                        EscribirIntentoMaquina(lineaAdivinada);

                        turnoAnterior = "maquina";
                    }

                    continue;
                }

                if (turnoAnterior == "maquina")
                {
                    string intento = "#";

                    ultimaLinea = listaDeIntentos[listaDeIntentos.Count - 1];

                    esIntentoExitoso = false;

                    while (esIntentoExitoso == false)
                    {
                        intento = Console.ReadLine();

                        lineaAdivinada = AdivinarSiguienteLinea(ultimaLinea);

                        if (intento == lineaAdivinada)
                        {
                            esIntentoExitoso = true;

                            listaDeIntentos.Add(intento);

                            EscribirIntentoUsuario(intento, esIntentoExitoso);
                        }
                        else
                        {
                            esIntentoExitoso = false;

                            //listaDeIntentos.Add(intento);

                            EscribirIntentoUsuario(intento, esIntentoExitoso);
                        }
                    }

                    turnoAnterior = "usuario";
                }
            }
        }

        #endregion Modo_juego

        #region Escribir

        static void EscribirIntentoUsuario(string mensaje, bool intentoExitoso)
        {
            if (intentoExitoso)
            {
                //WriteLine(message, "0A");
                ReescribirLineaPrevia(mensaje, "0A");
            }
            else
            {
                //WriteLine(message, "0C");
                ReescribirLineaPrevia(mensaje, "0C");
            }
        }

        static void EscribirIntentoMaquina(string mensaje)
        {
            ReescribirLineaPrevia(mensaje, "0B");
        }

        static void EscribirPrimeraLinea(string mensaje)
        {
            ReescribirLineaPrevia(mensaje, "0F");
        }

        static void ReescribirLineaPrevia(string mensaje, string atributoColor)
        {
            int previousLineCursorTop = Console.CursorTop - 1;
            int previousLineCursorLeft = Console.CursorLeft = 0;

            Console.CursorTop = previousLineCursorTop;
            Console.CursorLeft = previousLineCursorLeft;

            EscribirLinea(mensaje, atributoColor);
        }

        static void EscribirLinea(string mensaje, string atributoColor)
        {
            int backColorIndex = int.Parse(atributoColor.Substring(0, 1), System.Globalization.NumberStyles.HexNumber);
            int foreColorIndex = int.Parse(atributoColor.Substring(1, 1), System.Globalization.NumberStyles.HexNumber);

            ConsoleColor previousBackColor = Console.BackgroundColor;
            ConsoleColor previousForeColor = Console.ForegroundColor;

            Console.BackgroundColor = listaDeColores[backColorIndex];
            Console.ForegroundColor = listaDeColores[foreColorIndex];

            Console.WriteLine(mensaje);

            Console.BackgroundColor = previousBackColor;
            Console.ForegroundColor = previousForeColor;
        }

        #endregion

        #region Conteo

        static string AdivinarSiguienteLinea(string lineaActual)
        {
            int indexNumber = -1;
            int numberOfOccurences = 0;

            string resultLine = "";

            indexNumber = (int)Convert.ToDecimal(lineaActual[0].ToString());

            foreach (char c in lineaActual)
            {
                if (indexNumber == (int)Convert.ToDecimal(c.ToString()))
                {
                    numberOfOccurences++;
                }
                else
                {
                    resultLine += numberOfOccurences.ToString() + indexNumber.ToString();
                    indexNumber = (int)Convert.ToDecimal(c.ToString());

                    numberOfOccurences = 1;
                }
            }

            resultLine += numberOfOccurences.ToString() + indexNumber.ToString();

            return resultLine;
        }

        #endregion

        #endregion Metodos
    }

    #region Notas

    /*
     * 1. Agregar comandos para regresar al menú o salir
     * 2. Generar puntuacion
     * 3. Aplicar colores a pantallas
     * 4. Autodimensionar ventana o pantalla completa
     * 5. Cronometrar intentos
     * 6. Historial de intentos con fecha y hora
     * 7. Juego invisible:
     *    >> Para modalidad solitario: Mostrar sólamente el número
     *       inicial, y, para los siguientes, capturar las teclas 
     *       pulsadas evaluándolas tras presionar Intro.
     *    >> Para modalidad versus máquina: Mostrar sólamente el 
     *       número inicial, y , para los siguientes, capturar las 
     *       teclas pulsadas evaluándolas tras presionar Intro.
     *
     * X. Mediablemente coronable
     *       
     */

    #endregion Notas
}
