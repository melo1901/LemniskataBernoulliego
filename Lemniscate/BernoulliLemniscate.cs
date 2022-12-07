using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemniscate
{
    internal class BernoulliLemniscate
    {

        public static void Start()
        {
            bool conditionM = true;         //definicja glownej flagi
            bool valid = false;
            int color = 0, angle = 0;
            ConsoleColor colorForeground = ConsoleColor.White;
            double z = 0, s = 1, a = 0, t = 0;
            char znak = '*';
            while (conditionM)                  //glowna petla
            {
                Console.Clear();
                a = BernoulliLemniscate.validateA(valid, a, z);             //odczytanie i sprawdzenie poprawnosci parametru a
                t = BernoulliLemniscate.validateT(valid, t, z);             //odczytanie i sprawdzenie poprawnosci parametru t
                znak = BernoulliLemniscate.validateZnak(valid, znak);       //odczytanie i sprawdzenie poprawnosci znaku rysujacego funkcje

                BernoulliLemniscate.Color(color, ref colorForeground);      //metoda do wyboru koloru jakim bedziemy rysowac
                BernoulliLemniscate.Rotation(valid, ref angle, z);          //metoda do wyboru rotacji lemniskaty
                BernoulliLemniscate.Draw(valid, a, t, znak, ref colorForeground, angle, s);    //metoda rysujaca funkcje
                conditionM = BernoulliLemniscate.Exit(ref conditionM);      //metoda zakonczenia programu
            }
        }
        private static double validateA(bool valid, double a, double z)
        {
            valid = false;                                                                          //WSZYSTKIE METODY VALIDATE DZIALAJA NA TEJ SAMEJ ZASADZIE
            while (!valid)                                                                          //flaga petli
            {
                Console.WriteLine("Podaj odleglosc srodka do ogniska lemniskaty(min.0):");
                var a_temp = Console.ReadLine();                                                    //input zmiennej tymczasowej, ktora trzeba sprawdzic pod wzgledem formatu
                valid = !string.IsNullOrWhiteSpace(a_temp) &&
                    Double.TryParse(a_temp, out z);                                                //dwa warunki sprawdzajace, czy podana zmienna jest liczba rzeczywista

                if (valid)                                                                        //jesli tak, to przypisujemy te wartosc do zmiennej a
                {
                    a = Convert.ToDouble(a_temp);
                }

                if (!valid)                                                                       //jesli nie, to czyscimy konsole z informacja o bledzie i petla sie powtarza
                {
                    Console.Clear();
                    Console.WriteLine("Podano niepoprawna wartosc, sprobuj ponownie");
                }
            }
            return a;
        }

        private static char validateZnak(bool valid, char znak)
        {
            valid = false;
            while (!valid)
            {
                Console.WriteLine("Wybierz znak, ktorym lemniskata zostanie narysowana (jesli wybierzesz wiecej, to lemniskata zostanie narysowana pierwszym z nich):");
                var znak_temp = Console.ReadLine();
                valid = !string.IsNullOrWhiteSpace(znak_temp);

                if (valid)
                {
                    znak = Convert.ToString(znak_temp)[0];
                }

                if (!valid)
                {
                    Console.Clear();
                    Console.WriteLine("Podano niepoprawna wartosc, sprobuj ponownie");
                }
            }
            Console.Clear();
            return znak;
        }

        private static double validateT(bool valid, double t, double z)
        {
            valid = false;
            while (!valid)
            {
                Console.WriteLine("Podaj rozdzielczosc parametru t (zakres [0,2pi]):");
                var t_temp = Console.ReadLine();
                valid = !string.IsNullOrWhiteSpace(t_temp) &&
                    Double.TryParse(t_temp, out z);

                if (valid)
                {
                    t = Convert.ToDouble(t_temp);
                }
                if (z > 2 * Math.PI || z < 0)
                    valid = false;

                if (!valid)
                {
                    Console.Clear();
                    Console.WriteLine("Prosze wprowadz liczbe w zakresie [0,2pi]");
                }
            }
            return t;
        }

        private static void Color(int color, ref ConsoleColor colorForeground)
        {
            bool colorCondition = true;
            while (colorCondition)                                  //petla do wybierania koloru
            {
                Console.Clear();
                Console.WriteLine("Czy chcesz zmienic kolor tekstu? (T/N)");
                var temp = Console.ReadKey();
                if (temp.Key == ConsoleKey.T)
                {
                    Console.Clear();
                    Console.WriteLine("Wybierz jeden z dostepnych kolorow:\nA.Czerwony        B.Niebieski        C.Zielony");
                    temp = Console.ReadKey();
                    if (temp.Key == ConsoleKey.A)                               //deklaracje odpowiedzi, uwzgledniajac nacisniety klawisz
                    {
                        colorCondition = false;
                        Console.Clear();
                        color = 1;
                    }
                    else if (temp.Key == ConsoleKey.B)
                    {
                        colorCondition = false;
                        Console.Clear();
                        color = 2;
                    }
                    else if (temp.Key == ConsoleKey.C)
                    {
                        colorCondition = false;
                        Console.Clear();
                        color = 3;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Podano niepoprawną literę. Kliknij dowolny klawisz, aby wybrac ponownie.");
                        Console.ReadKey();
                    }
                }

                else if (temp.Key == ConsoleKey.N)
                {
                    colorCondition = false;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Podano niepoprawną literę. Kliknij dowolny klawisz, aby wybrac ponownie.");
                    Console.ReadKey();
                }
            }
            if (color == 1)                                                 //przypisanie do zmiennej ustalonego koloru
            {
                colorForeground = ConsoleColor.Red;
            }
            else if (color == 2)
            {
                colorForeground = ConsoleColor.Blue;
            }
            else if (color == 3)
            {
                colorForeground = ConsoleColor.Green;
            }
            else if (color == 0)
            {
                colorForeground = ConsoleColor.White;
            }
        }

        private static void Draw(bool valid, double a, double t, char znak, ref ConsoleColor colorForeground, int angle, double skala)
        {
            try
            {
                bool conditionD = true;
                Console.Clear();
                double pi = Math.PI, x, y, z = 0;

                while (conditionD)                                                                                      //petla powtarzajaca rysowanie w przypadku zmienienia skali
                {
                    Console.Clear();
                    int centerX = Console.WindowWidth / 2;                                                              //wyznaczenie srodku konsoli (przeciecie osi X i Y)
                    int centerY = Console.WindowHeight / 2;
                    Console.ForegroundColor = colorForeground;                                                          //ustawienie zmienionego wczesniej koloru rysowania funkcji
                    int counter = 0;
                    int[,] matrix = new int[700, 3];                                                                    //utworzenie macierzy 2d
                    for (double i = t; i < pi * 2; i += 0.01)                                                           //glowna petla rysujaca
                    {
                        x = (Math.Sqrt(a) * Math.Cos(i)) / (1 + (Math.Sin(i) * Math.Sin(i))) * skala;                           //uwzglednienie skali i obliczenie wspolrzednej x dla poczatkowego t (z kazda kolejna petla t jest lekko zwiekszane, aby rysowac kolejne punkty)
                        y = (Math.Sqrt(a) * Math.Cos(i)) * Math.Sin((i)) / (1 + (Math.Sin(i)) * Math.Sin(i)) * skala;           //uwzglednienie skali i obliczenie wspolrzednej y dla poczatkowego t (tak jak wyzej)
                        x = (x * Math.Cos(DegreesToRadians(angle))) + (y * Math.Sin(DegreesToRadians(angle)));                  //uwzglednienie katu o jaki funkcja zostala obrocona
                        y = (-x * Math.Sin(DegreesToRadians(angle))) + (y * Math.Cos(DegreesToRadians(angle)));
                        matrix[counter, 0] = (int)Math.Round(i);                                                        //uzupelnienie macierzy parametrem t i wspolrzednymi x, y
                        matrix[counter, 1] = (int)Math.Round(x);
                        matrix[counter, 2] = (int)Math.Round(y);
                        counter++;
                        Console.SetCursorPosition(centerX + (int)Math.Round(x), centerY - (int)Math.Round(y));          //ustawienie kursora w obliczonym punkcie
                        Console.Write(znak);                                                                            //wstawienie wybranego przez uzytkownika znaku
                    }
                    Console.ForegroundColor = ConsoleColor.White;
                    BernoulliLemniscate.ChooseScale(valid, ref skala, z, ref conditionD);

                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            catch (Exception)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Clear();
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("Wprowadzono zbyt duza odleglosc srodka do ogniska lemniskaty dla tego rozmiaru konsoli, sprobuj z mniejsza odlegloscia lub powieksz rozmiar konsoli");
            }
            Console.ReadKey();
        }

        private static bool Exit(ref bool conditionM)
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Jezeli chcesz wyjsc z programu kliknij ESC, aby narysowac kolejna lemniskate kliknij L");    //proste menu wyjscia z programu
            var temp = Console.ReadKey();
            if (temp.Key == ConsoleKey.L)
            {
                conditionM = true;
            }
            if (temp.Key == ConsoleKey.Escape)

                conditionM = false;
            return conditionM;
        }

        private static void Rotation(bool valid, ref int angle, double z)
        {
            valid = false;                                                                                      //metoda rotation dziala na tej samej zasadzie co metody validate
            {
                Console.Clear();
                Console.WriteLine("Podaj kat o jaki obrocic lemniskate:");
                var angle_temp = Console.ReadLine();
                valid = !string.IsNullOrWhiteSpace(angle_temp) &&
                    Double.TryParse(angle_temp, out z);

                if (valid)
                {
                    z = Convert.ToDouble(angle_temp);
                    angle = Convert.ToInt32(z);
                }

                if (!valid)
                {
                    Console.Clear();
                    Console.WriteLine("Podano niepoprawna wartosc, sprobuj ponownie");
                }
            }
        }


        private static void ChooseScale(bool valid, ref double s, double z, ref bool conditionD)
        {
            bool conditionS = true;
            while (conditionS)                                                                          //petla uzywana do poprawnego wybrania odpowiedz na pierwsze pytanie
            {
                Console.SetCursorPosition(0, 0);
                Console.Write(new string(' ', Console.WindowWidth));
                Console.WriteLine("Czy chcesz zmienic skale rysunku? (T/N)");
                var temp = Console.ReadKey();
                if (temp.Key == ConsoleKey.T)
                {
                    valid = false;
                    while (!valid)                                                                     //petla sprawdzajaca, czy wprowadzono liczbe rzeczywista (jak w metodach validate)
                    {
                        Console.Clear();
                        Console.WriteLine("Podaj skale (format skali x:1):");
                        var s_temp = Console.ReadLine();
                        valid = !string.IsNullOrWhiteSpace(s_temp) &&
                            Double.TryParse(s_temp, out z);

                        if (valid && (Convert.ToDouble(s_temp) > 0))
                        {
                            s = Convert.ToDouble(s_temp);
                        }

                        if (!valid)
                        {
                            Console.Clear();
                            Console.WriteLine("Podano niepoprawna wartosc, sprobuj ponownie");
                        }
                    }
                    conditionS = false;
                    conditionD = true;
                }

                else if (temp.Key == ConsoleKey.N)
                {
                    conditionS = false;
                    conditionD = false;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Podano niepoprawną literę. Kliknij dowolny klawisz, aby wybrac ponownie.");
                    Console.ReadKey();
                }
            }
        }
        private static double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }

    }
}

