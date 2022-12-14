using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife {
	internal class Menu {
		string[] list;
		int listsize;
		int current = 0;
		bool stop = false;
		ConsoleKeyInfo key;
		public Menu(string[] arr, int a) {
			list = arr;
			listsize = a;
		}

		public void show() {
			Console.Clear();
			for (int i = 0; i < listsize; i++) {
				if (i == current) Console.WriteLine($" >>>  {list[i]}");
				else Console.WriteLine($"      {list[i]}");
			}
		}

		public void choose() {
			stop = false;
			while (!stop) {
				show();
				key = Console.ReadKey();
				switch (key.Key) {
					case ConsoleKey.UpArrow:
						if (current == 0) current += listsize;
						current--;
						current %= listsize;
						break;
					case ConsoleKey.DownArrow:
						current++;
						current %= listsize;
						break;
					case ConsoleKey.Enter:
						switch (current % listsize) {
							case 0:
								Console.Clear();
								Console.Write("Нажмите любую кнопку...");
								Console.ReadLine();
								Console.CursorVisible = false;
								Console.SetCursorPosition(0, 0);
								GameOfLife frame = new GameOfLife(Console.WindowHeight, Console.WindowWidth, 2);
								while (true) {
									Console.Title = frame.GetIter().ToString();
									frame.Output();
									frame.Iter();
									Console.SetCursorPosition(0, 0);
								}
							case 1:
								Console.Clear();
								Console.WriteLine("	Игра «Жизнь» (англ. Conway's Game of Life) — клеточный автомат, придуманный английским математиком");
								Console.WriteLine("	Джоном Конвеем в 1970 году.\n\n");
								Console.WriteLine("	Место действия этой игры — «вселенная» — это размеченная");
								Console.WriteLine("	на клетки поверхность или плоскость — безграничная,");
								Console.WriteLine("	ограниченная, или замкнутая (в пределе — бесконечная плоскость).\n");
								Console.WriteLine("	Каждая клетка на этой поверхности может находиться в двух состояниях:");
								Console.WriteLine("	быть «живой»(заполненной) или быть «мёртвой»(пустой).");
								Console.WriteLine("	Клетка имеет восемь соседей, окружающих её.\n");
								Console.WriteLine("	Распределение живых клеток в начале игры называется первым поколением.");
								Console.WriteLine("	Каждое следующее поколение рассчитывается на основе предыдущего по таким правилам :");
								Console.WriteLine("	- в пустой (мёртвой) клетке, рядом с которой ровно три живые клетки, зарождается жизнь;");
								Console.WriteLine("	- если у живой клетки есть две или три живые соседки, то эта клетка продолжает жить;");
								Console.WriteLine("	- в противном случае, если соседей меньше двух или больше трёх, клетка умирает.\n");
								Console.WriteLine("	Игра прекращается, если:");
								Console.WriteLine("	- на поле не останется ни одной «живой» клетки");
								Console.WriteLine("	- конфигурация на очередном шаге в точности (без сдвигов и поворотов)");
								Console.WriteLine("	- повторит себя же на одном из более ранних шагов(складывается периодическая конфигурация)");
								Console.WriteLine("	- при очередном шаге ни одна из клеток не меняет своего состояния");
								Console.WriteLine("	  (складывается стабильная конфигурация; предыдущее правило, вырожденное до одного шага назад)\n");
								Console.WriteLine("	Нажмите кнопку чтобы вернуться");
								key = Console.ReadKey();
								break;
							case 2:
								Console.Clear();
								Console.WriteLine("+-----------------------------------------+");
								Console.WriteLine("|   Выполнил: Миняйлов Никита Сергеевич   |");
								Console.WriteLine("+-----------------------------------------+");
								Console.WriteLine("|              Группа МО-211              |");
								Console.WriteLine("+-----------------------------------------+");
								Console.WriteLine("|            Факультет: ФИТиКС            |");
								Console.WriteLine("+-----------------------------------------+");
								Console.WriteLine("|      Преподаватель:  Федотова И.В.      |");
								Console.WriteLine("+-----------------------------------------+");
								key = Console.ReadKey();
								break;
							case 3:
								stop = true;
								break;
						}
						break;
				}
			}
		}
	};
}
