using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Threading;
using System.Diagnostics;

namespace PeriodicTableOfElements
{
    class TableEventEngine
    {
        public static Dictionary<string, string> elementNames = new Dictionary<string, string>
        {
            {"H", "Hydrogen"}, {"He", "Helium"}, {"Li", "Lithium"}, {"Be", "Beryllium"}, {"B", "Boron"},
            {"C", "Carbon"}, {"N", "Nitrogen"}, {"O", "Oxygen"}, {"F", "Fluorine"}, {"Ne", "Neon"},
            {"Na", "Sodium"}, {"Mg", "Magnesium"}, {"Al", "Aluminium"}, {"Si", "Silicon"}, {"P", "Phosphorus"},
            {"S", "Sulfur"}, {"Cl", "Chlorine"}, {"Ar", "Argon"}, {"K", "Potassium"}, {"Ca", "Calcium"},
            {"Sc", "Scandium"}, {"Ti", "Titanium"}, {"V", "Vanadium"}, {"Cr", "Chromium"}, {"Mn", "Manganese"},
            {"Fe", "Iron"}, {"Co", "Cobalt"}, {"Ni", "Nickel"}, {"Cu", "Copper"}, {"Zn", "Zinc"},
            {"Ga", "Gallium"}, {"Ge", "Germanium"}, {"As", "Arsenic"}, {"Se", "Selenium"}, {"Br", "Bromine"},
            {"Kr", "Krypton"}, {"Rb", "Rubidium"}, {"Sr", "Strontium"}, {"Y", "Yttrium"}, {"Zr", "Zirconium"},
            {"Nb", "Niobium"}, {"Mo", "Molybdenum"}, {"Tc", "Technetium"}, {"Ru", "Ruthenium"}, {"Rh", "Rhodium"},
            {"Pd", "Palladium"}, {"Ag", "Silver"}, {"Cd", "Cadmium"}, {"In", "Indium"}, {"Sn", "Tin"},
            {"Sb", "Antimony"}, {"Te", "Tellurium"}, {"I", "Iodine"}, {"Xe", "Xenon"}, {"Cs", "Cesium"},
            {"Ba", "Barium"}, {"La", "Lanthanum"}, {"Ce", "Cerium"}, {"Pr", "Praseodymium"}, {"Nd", "Neodymium"},
            {"Pm", "Promethium"}, {"Sm", "Samarium"}, {"Eu", "Europium"}, {"Gd", "Gadolinium"}, {"Tb", "Terbium"},
            {"Dy", "Dysprosium"}, {"Ho", "Holmium"}, {"Er", "Erbium"}, {"Tm", "Thulium"}, {"Yb", "Ytterbium"},
            {"Lu", "Lutetium"}, {"Hf", "Hafnium"}, {"Ta", "Tantalum"}, {"W", "Tungsten"}, {"Re", "Rhenium"},
            {"Os", "Osmium"}, {"Ir", "Iridium"}, {"Pt", "Platinum"}, {"Au", "Gold"}, {"Hg", "Mercury"},
            {"Tl", "Thallium"}, {"Pb", "Lead"}, {"Bi", "Bismuth"}, {"Po", "Polonium"}, {"At", "Astatine"},
            {"Rn", "Radon"}, {"Fr", "Francium"}, {"Ra", "Radium"}, {"Ac", "Actinium"}, {"Th", "Thorium"},
            {"Pa", "Protactinium"}, {"U", "Uranium"}, {"Np", "Neptunium"}, {"Pu", "Plutonium"}, {"Am", "Americium"},
            {"Cm", "Curium"}, {"Bk", "Berkelium"}, {"Cf", "Californium"}, {"Es", "Einsteinium"}, {"Fm", "Fermium"},
            {"Md", "Mendelevium"}, {"No", "Nobelium"}, {"Lr", "Lawrencium"}, {"Rf", "Rutherfordium"}, {"Db", "Dubnium"},
            {"Sg", "Seaborgium"}, {"Bh", "Bohrium"}, {"Hs", "Hassium"}, {"Mt", "Meitnerium"}, {"Ds", "Darmstadtium"},
            {"Rg", "Roentgenium"}, {"Cn", "Copernicium"}, {"Nh", "Nihonium"}, {"Fl", "Flerovium"}, {"Mc", "Moscovium"},
            {"Lv", "Livermorium"}, {"Ts", "Tennessine"}, {"Og", "Oganesson"}
        };
        public static List<int> keyIndexes = new List<int>();

        public static string chosenKey = "";

        public static void load() {
            for (int i = 0; i < elementNames.Keys.Count; i++) {
                keyIndexes.Add(i);
            }

            generate();
        }

        public static void generate() {
            if (keyIndexes.Count > 0){
                chosenKey = chooseRandomKey();

                Debug.WriteLine("Click on " + elementNames[chosenKey]);
            }
            else {
                Debug.WriteLine("Done");
            }
        }

        public static bool check(string name) {
            return name.Equals(chosenKey);
        }

        public static string chooseRandomKey() {
            Random rand = new Random();

            int randomIndex = rand.Next(keyIndexes.Count);
            int index = keyIndexes[randomIndex];
            keyIndexes.RemoveAt(randomIndex);

            return elementNames.Keys.ElementAt(index);
        }
    }
}
