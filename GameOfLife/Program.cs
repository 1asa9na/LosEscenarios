using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace GameOfLife {
    class Program {
        static void Main(string[] args) {
			string[] MainMenu = new string[4];
			MainMenu[0] = "Начать Игру";
			MainMenu[1] = "Описание";
			MainMenu[2] = "Об Авторе";
			MainMenu[3] = "Выход";
			Menu Start = new Menu(MainMenu, 4);
			Start.choose();
        }
    };
}