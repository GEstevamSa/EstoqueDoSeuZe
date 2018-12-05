using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeuZe_Trabalhador
{
    public class Trabalho
    {
        public int balcao;
        public int estoque;
        public int custo;
    }

    public class Grafo
    {
        public int[,] Mat;
    }

    public class SuperZe
    {
        const int MaxCusto = 100;

        public void melhorCaminho(Grafo grafo, Trabalho[] melhorRota,ref int melhorCusto,int[] permutacao)
        {
            int j, k;
            int balc, est;
            int custo;
            int[] proxPedido;

            proxPedido = new int[melhorRota.Length];
            balc = 0;
            est = 0;
            custo = grafo.Mat[balc, est];
            proxPedido[balc] = est;

            for (j = 2; j < melhorRota.Length; j++)
            {
                balc = est;
                balc = permutacao[j];
                custo += grafo.Mat[balc, est];
                proxPedido[balc] = est;
            }

            proxPedido[est] = 0;
            custo += grafo.Mat[est,0];

            if(custo < melhorCusto)
            {
                melhorCusto = custo;
                est = 0;
                for(k = 0; k < melhorRota.Length; k++)
                {
                    balc = est;
                    est = proxPedido[balc];
                    melhorRota[k].balcao = balc;
                    melhorRota[k].estoque = est;
                    melhorRota[k].custo = grafo.Mat[balc, est];
                }
            }
            
        }

        public void permuta(int[] permutacao, Grafo grafo,Trabalho[] melhorRota, ref int melhorCusto,int controle,int k)
        {
            int i;
            permutacao[k] = ++controle;

            if(controle == (melhorRota.Length - 1))
            {
                melhorCaminho(grafo, melhorRota, ref melhorCusto,permutacao);
            }

            else
            {
                for (i = 1; i < melhorRota.Length; i++)
                {
                    if (permutacao[i] == 0)
                    {
                        permuta(permutacao, grafo, melhorRota, ref melhorCusto, controle, i);
                    }
                }
            }
            controle--;
            permutacao[k] = 0;
        }

        public void encontraCaminho(Grafo grafo, List<Trabalho> caminho, ref int custo)
        {
            int i, balc = 0, est = 0, menorCusto, tamanho = caminho.Capacity;
            int[] pos = new int[grafo.Mat.Length];
            List<int> adicionados = new List<int>(grafo.Mat.Length);
            adicionados.Add(0);
            Trabalho trabalho = new Trabalho();
            custo = 0;

            for(i = 0; i < tamanho; i++)
            {
                menorCusto = MaxCusto + 1;
                trabalho.balcao = balc;

                if(i==caminho.Capacity - 1)
                {
                    menorCusto = grafo.Mat[balc, 0];
                    est = 0;
                }

                else
                {
                    for(int j = 0; j< tamanho; j++)
                    {
                        if(j!= balc && !adicionados.Contains(j))
                        {
                            if(grafo.Mat[balc,j] != 0 && grafo.Mat[balc,j]< menorCusto)
                            {
                                menorCusto = grafo.Mat[balc, j];
                                est = j;
                            }
                        }
                    }
                }

                trabalho.estoque = est;
                trabalho.custo = menorCusto;
                adicionados.Add(est);
                custo += menorCusto;
                caminho.Add(trabalho);
                trabalho = new Trabalho();
                balc = est;
            }
        }

        public void montaGrafo(out Grafo grafo, int numPedidos)
        {
            int custo;

            Random random = new Random();
            grafo = new Grafo();
            grafo.Mat = new int[numPedidos, numPedidos];

            for(int i = 0; i < numPedidos; i++)
            {
                for(int j= 0; j < numPedidos; j++)
                {
                    custo = random.Next(MaxCusto) + 1;
                    if (i < j)
                    {
                        grafo.Mat[i, j] = custo;
                    }
                    else
                    {
                        if (i == j)
                        {
                            grafo.Mat[i, j] = 0;
                        }

                        else
                        {
                            grafo.Mat[i, j] = grafo.Mat[j, i];
                        }
                    }
                }
            }

            Console.Write("\nPedidos e custos:\n ");
            for(int i = 0; i < numPedidos; i++)
            {
                Console.Write("\t" + i);
            }
            Console.WriteLine();

            for(int i = 0; i < numPedidos; i++)
            {
                Console.Write(i + "   ");
                for(int j = 0; j < numPedidos; j++)
                {
                    Console.Write("  " + grafo.Mat[i, j]);
                }
                Console.WriteLine();
            }
        }

        public void geraEscolheCaminhos(ref int[] permutacao,Grafo grafo,Trabalho[] melhorRota,out int melhorCusto)
        {
            int controle = -1;
            melhorCusto = int.MaxValue;

            for(int i = 0; i < melhorRota.Length; i++)
            {
                melhorRota[i] = new Trabalho();
            }

            permuta(permutacao, grafo, melhorRota, ref melhorCusto, controle, 1);
        }

        public void imprimeCaminho(int custo, List<Trabalho> melhorCaminho)
        {
            
            Console.WriteLine("\n\nMELHOR CAMINHO PARA A VIAGEM DO SEU ZÉ:");
            Console.WriteLine("\n\n              DE               PARA             CUSTO ");
            foreach (Trabalho trabalho in melhorCaminho)
            {
                Console.Write("              " + trabalho.balcao + "                  " + trabalho.estoque + "                " + trabalho.custo + "\n");
            }

            if(custo > MaxCusto)
            {
                Console.WriteLine("\n\nSeu Zé é fraco não consegue trazer tanto peso de uma só vez!!!");
                Console.WriteLine("Ele só consegue trazer um total de " + MaxCusto + "Kg e voce pediu um total de " + custo + "Kg");
                Console.WriteLine("Tente Novamente quando Seu Zé estiver mais disposto!!");
            }

            else
            {
                Console.WriteLine("\n\nCUSTO MINIMO PARA O TRABALHO DO SEU ZÉ: " + custo + "Kg");
                Console.WriteLine("\n");
            }
            Console.WriteLine("\n");
        }

        public void imprimeMelhorCaminho(int custo,Trabalho[] melhorRota)
        {
            int i;
            Console.WriteLine("\n\nCUSTO MINIMO PARA O TRABALHO DO SEU ZÉ:" + custo + "Kg");
            Console.WriteLine("\n\nMELHOR CAMINHO PARA O TRABALHO DO SEU ZÉ:");
            Console.WriteLine("\n\n              DE               PARA             CUSTO ");
            for (i = 0; i < melhorRota.Length; i++)
            {
                Console.Write("              " + melhorRota[i].balcao + "                  " + melhorRota[i].estoque + "                " + melhorRota[i].custo + "\n");
            }
            Console.WriteLine("\n");
        }

        public void imprimetempo(Stopwatch tempo)
        {
            Console.WriteLine("TEMPO DE EXECUÇÃO");
            Console.WriteLine(tempo.Elapsed.Hours + "horas" + 
                tempo.Elapsed.Seconds + " segundos " + 
                tempo.Elapsed.Milliseconds + " milisegundos");
        }

        public static void Main(string[] args)
        {
            List<Trabalho> caminho;
            int numPedidos, custo = 0;
            SuperZe superZe = new SuperZe();
            Grafo grafo = new Grafo();
            Stopwatch stopwatch = new Stopwatch();
            int[] permutacao;
            Trabalho[] melhorRota;
            int numPedidos2, melhorCusto;
            int aux;
            string resp = null;


            do
            {
                Console.WriteLine("Escolha qual Zé você deseja:" + "\n\n  [Magro] Digite 1  " + "  [Velho] Digite 2  ");
                Console.Write("\n\nResposta:");
                aux = int.Parse(Console.ReadLine());

                if (aux == 1)
                {
                    //Zé Velho
                    Console.Write("\n\nDigite o numero de pedidos e tecle ENTER: ");
                    numPedidos = int.Parse(Console.ReadLine());
                    superZe.montaGrafo(out grafo, numPedidos);
                    caminho = new List<Trabalho>(numPedidos); ;
                    superZe.encontraCaminho(grafo, caminho, ref custo);
                    superZe.imprimeCaminho(custo, caminho);

                    Console.Write("\n\nVocê deseja Continuar? \nSim ou Não\n");
                    resp = Console.ReadLine();
                    Console.ReadKey(true);
                }

                if (aux == 2)
                {
                    //Zé Magrão
                    SuperZe superZe2 = new SuperZe();
                    Grafo grafo2 = new Grafo();

                    Console.Write("\n\nDigite o numero de pedidos e tecle ENTER: ");
                    numPedidos2 = int.Parse(Console.ReadLine());
                    superZe.montaGrafo(out grafo2, numPedidos2);

                    permutacao = new int[numPedidos2];
                    melhorRota = new Trabalho[numPedidos2];

                    caminho = new List<Trabalho>(numPedidos2);
                    superZe2.geraEscolheCaminhos(ref permutacao, grafo2, melhorRota, out melhorCusto);
                    superZe2.imprimeMelhorCaminho(melhorCusto,melhorRota);

                    Console.Write("\n\nVocê deseja Continuar? \nSim ou Não\n");
                    resp = Console.ReadLine();
                    Console.ReadKey(true);
                }

            } while (resp != "Não");

            Environment.Exit(0);
            Console.ReadKey(true);
            
        }

    }
}
