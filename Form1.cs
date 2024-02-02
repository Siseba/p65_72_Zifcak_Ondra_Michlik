using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace p65_72_Zifcak_Ondra_Michlik
{ 

    public partial class Form_Login : Form
    {
        private void registerPlaceholder() {
            

            textBox_Registracia_Meno.Text = "Meno";
            textBox_Registracia_Meno.ForeColor = System.Drawing.Color.Gray;

            textBox_Registracia_Priezvisko.Text = "Priezvisko";
            textBox_Registracia_Priezvisko.ForeColor = System.Drawing.Color.Gray;

            textBox_Registracia_Pin.Text = "Pin";
            textBox_Registracia_Pin.ForeColor = System.Drawing.Color.Gray;

            textBox_Registracia_Tel_Cislo.Text = "Telefonne cislo";
            textBox_Registracia_Tel_Cislo.ForeColor = System.Drawing.Color.Gray;

            textBox_Registracia_Email.Text = "E-mail";
            textBox_Registracia_Email.ForeColor = System.Drawing.Color.Gray;

            textBox_Registracia_Adresa.Text = "Adresa, mesto";
            textBox_Registracia_Adresa.ForeColor = System.Drawing.Color.Gray;

        }

        private void loginPlaceholder() {
            // Vytvorenie placeholderu pre textboxy v logine

            this.ActiveControl = null;

            textBox_Login_Email.Text = "E-mail";
            textBox_Login_Email.ForeColor = System.Drawing.Color.Gray;

            textBox_Login_Pin.Text = "Pin";
            textBox_Login_Pin.ForeColor = System.Drawing.Color.Gray;
        }

        string users_subor = "users.csv";

        List<string> zaregistrovane_cisla = new List<string>();
        List<string> zaregistrovane_emaily = new List<string>();
        List<int> zaregistrovane_id = new List<int>();

        Random rand = new Random();

        public Form_Login()
        {
            InitializeComponent();
            this.ActiveControl = null;
            FileStream file;
            
            if (!File.Exists(users_subor))
            {
                file = File.Create(users_subor);
                file.Close();
            }

            loginPlaceholder();

            // Vypnutie panelu register a zapnutie panelu login - TabOrder fix
            panel_Login.Enabled = true;
            panel_Registracia.Enabled = false;
            
            textBox_Login_Email.TabStop = false;
            textBox_Login_Pin.TabStop = false;
        }
       
        // Vytvorenie placeholderu pre textboxy v registracii 


        // Prepinanie medzi login a register 

        private void pictureBox_Login_Register_Click(object sender, EventArgs e)
        {
            this.ActiveControl = null;

            registerPlaceholder();

            panel_Login.Visible = false;
            panel_Registracia.Visible = true;
            panel_Registracia.Enabled = true;
            panel_Login.Enabled = false;
        }

        private void pictureBox_Registracia_Cancel_Click(object sender, EventArgs e)
        {
            this.ActiveControl = null;

            loginPlaceholder();

            panel_Registracia.Visible = false;
            panel_Login.Visible = true;
            panel_Login.Enabled = true;
            panel_Registracia.Enabled = false;
        }


        // Picture box ( tlacidlo ) na login 

        private void pictureBox_Login_Click(object sender, EventArgs e)
        {
            StreamReader users = new StreamReader("users.csv", Encoding.UTF8);

            string riadok;
            while ((riadok = users.ReadLine()) != null)
            {
                string[] hodnoty = riadok.Split(';');

                string email = hodnoty[6].Trim().ToLower();
                string pin = hodnoty[4].Trim().ToLower();

                if (email == textBox_Login_Email.Text && pin == textBox_Login_Pin.Text)
                { 
                    this.Hide();
                    Form2 f2 = new Form2();
                    f2.ShowDialog();
                    this.Close();
                }
            }
        }

        private void pictureBox_Login_Close_Click(object sender, EventArgs e)
        {
            Close();
        }

        // generovanie cisla uctu
        // funkcia vracia cislo uctu

        private string generaciaUctu()
        {
            rand = new Random();
            string ucet;

            string[] kodyBaniek = new []{ "0900", "1100", "0200", "7500", "6500" };
            int r = rand.Next(0, kodyBaniek.Length);

            string kontrolneCislice = Convert.ToString(rand.Next(0,10)) + Convert.ToString(rand.Next(0,10));

            List<string> zakladneCisloUctuList = new List<string>();
            for (int i = 0; i < 10; i++)
            {
                zakladneCisloUctuList.Add(Convert.ToString(rand.Next(0,10)));
            }
            string zakladneCisloUctu = string.Concat(zakladneCisloUctuList);

            ucet = "SK" + kontrolneCislice + kodyBaniek[r] + "000000" + zakladneCisloUctu;

            return ucet;
        }

        // Funkcia na zapisanie udajov do suboru

        private void zapisanieDoSuboru()
        {

            int id = rand.Next(1000, 10000);
            int stav_uctu = 0;

            string adresa = "hutnicka";
            string mesto = "SNV";
            
            string cislo_uctu = generaciaUctu();
            string meno = textBox_Registracia_Meno.Text;
            string priezvisko = textBox_Registracia_Priezvisko.Text;
            string telefonne_cislo = textBox_Registracia_Tel_Cislo.Text;
            string email = textBox_Registracia_Email.Text;
            
            string pin = textBox_Registracia_Pin.Text;


            StreamWriter users = new StreamWriter(users_subor, true, Encoding.UTF8);

            string udaje_na_zapis = string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9}", id, cislo_uctu, meno, priezvisko, pin, telefonne_cislo, email, adresa, mesto, stav_uctu);
            users.WriteLine(udaje_na_zapis);

            users.Flush();
            users.Close();

            // Vycistenie registracneho formularu po uspesnej registracii

            textBox_Registracia_Meno.Text = "";
            textBox_Registracia_Priezvisko.Text = "";
            textBox_Registracia_Tel_Cislo.Text = "";
            textBox_Registracia_Pin.Text = "";
            textBox_Registracia_Email.Text = "";
            textBox_Registracia_Adresa.Text = "";


            // Prepnutie na panel login po registracii

            this.ActiveControl = null;

            loginPlaceholder();

            panel_Registracia.Visible = false;
            panel_Login.Visible = true;
            panel_Login.Enabled = true;
            panel_Registracia.Enabled = false;

            // Debug

            MessageBox.Show("Uspesne zaregistrovany :D");

        }

        // Picture box ( tlacidlo ) na register 

        private void pictureBox_Registracia_Register_Click(object sender, EventArgs e)
        {

            string meno = textBox_Registracia_Meno.Text;
            string priezvisko = textBox_Registracia_Priezvisko.Text;
            string telefonne_cislo = textBox_Registracia_Tel_Cislo.Text;
            string pin = textBox_Registracia_Pin.Text;
            string email = textBox_Registracia_Email.Text;
            string adresa = textBox_Registracia_Adresa.Text;

            // Precitanie vsetkych udajov v subore s uzivatelmi
            // V pripade ze subor je prazdny => Aktualne udaje sa hned zapisu 
            // V pripade ze sa v subore nachadzaju udaje => Najdene udaje sa zapisu do docastneho listu 
            // pomocou ktoreho sa neskor bude zistovat ci dany uzivatel uz existuje alebo nie :D

            StreamReader users = new StreamReader(users_subor);
            string riadok;
            if ((riadok = users.ReadLine()) == null)
            {
                zaregistrovane_cisla.Add(textBox_Registracia_Tel_Cislo.Text);
                zaregistrovane_emaily.Add(textBox_Registracia_Email.Text);
                users.Close();
                zapisanieDoSuboru();
            }
            else
            {
                string riadok1;
                while ((riadok1 = users.ReadLine()) != null)
                {
                    string[] hodnoty = riadok1.Split(';');
                    string nove_cislo = hodnoty[5].Trim().ToLower();
                    string novy_email = hodnoty[6].Trim().ToLower();
                    zaregistrovane_cisla.Add(nove_cislo);
                    zaregistrovane_emaily.Add(novy_email);
                }

                users.Close();
            }

            
            // Podmienka aby sa uzivatel nemohol zaregistrovat bez zadania vsetkych udajov

            if (meno != "" && priezvisko != "" && telefonne_cislo != ""  && email != "" && adresa != "" && pin != "")
            {
                if (meno != "Meno" && priezvisko != "Priezvisko" && telefonne_cislo != "Telefonne cislo" && email != "E-mail" && adresa != "Adresa" && pin != "Pin")
                    if (zaregistrovane_emaily.Contains(textBox_Registracia_Email.Text))
                    {
                        MessageBox.Show("Ucet s tymto emailom uz existuje D:");
                    }
                    else if (zaregistrovane_cisla.Contains(textBox_Registracia_Tel_Cislo.Text))
                    {
                        MessageBox.Show("Ucet s tymto telefonnym cislom uz existuje D:");
                    }
                    else
                    { 
                        zapisanieDoSuboru();
                    }
            }
        }

        // Osetrenie vstupu na tel. cislo a pin aby tam mohol uzivatel vlozit len cisla

        private void textBox_Registracia_Tel_Cislo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox_Registracia_Pin_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }


        // Placeholdery - registracia

        private void textBox_Registracia_Meno_Click(object sender, EventArgs e)
        {
            if (textBox_Registracia_Meno.Text == "Meno")
            {
                textBox_Registracia_Meno.Text = "";
                textBox_Registracia_Meno.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void textBox_Registracia_Meno_Enter(object sender, EventArgs e)
        {
            if (textBox_Registracia_Meno.Text == "Meno")
            { 
                textBox_Registracia_Meno.Text = "";
                textBox_Registracia_Meno.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void textBox_Registracia_Meno_Leave(object sender, EventArgs e)
        {
            if (textBox_Registracia_Meno.Text == "")
            {
                textBox_Registracia_Meno.Text = "Meno";
                textBox_Registracia_Meno.ForeColor = System.Drawing.Color.Gray;
            }
        }


        private void textBox_Registracia_Priezvisko_Click(object sender, EventArgs e)
        {
            
            if (textBox_Registracia_Priezvisko.Text == "Priezvisko")
            {
                textBox_Registracia_Priezvisko.Text = "";
                textBox_Registracia_Priezvisko.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void textBox_Registracia_Priezvisko_Enter(object sender, EventArgs e)
        {
            bool placeholder = textBox_Registracia_Priezvisko.Text == "Priezvisko";
            if (textBox_Registracia_Meno.Focused == true && placeholder)
            {
                textBox_Registracia_Priezvisko.Text = "";
                textBox_Registracia_Priezvisko.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void textBox_Registracia_Priezvisko_Leave(object sender, EventArgs e)
        {
            if (textBox_Registracia_Priezvisko.Text == "")
            {
                textBox_Registracia_Priezvisko.Text = "Priezvisko";
                textBox_Registracia_Priezvisko.ForeColor = System.Drawing.Color.Gray;
            }

        }

        private void textBox_Registracia_Tel_Cislo_Click(object sender, EventArgs e)
        {
            if (textBox_Registracia_Tel_Cislo.Text == "Telefonne cislo")
            {
                textBox_Registracia_Tel_Cislo.Text = "";
                textBox_Registracia_Tel_Cislo.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void textBox_Registracia_Tel_Cislo_Enter(object sender, EventArgs e)
        {
            if (textBox_Registracia_Tel_Cislo.Text == "Telefonne cislo")
            {
                textBox_Registracia_Tel_Cislo.Text = "";
                textBox_Registracia_Tel_Cislo.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void textBox_Registracia_Tel_Cislo_Leave(object sender, EventArgs e)
        {
            if (textBox_Registracia_Tel_Cislo.Text == "")
            {
                textBox_Registracia_Tel_Cislo.Text = "Telefonne cislo";
                textBox_Registracia_Tel_Cislo.ForeColor = System.Drawing.Color.Gray;
            }
        }

        private void textBox_Registracia_Email_Click(object sender, EventArgs e)
        {
            if (textBox_Registracia_Email.Text == "E-mail")
            {
                textBox_Registracia_Email.Text = "";
                textBox_Registracia_Email.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void textBox_Registracia_Email_Enter(object sender, EventArgs e)
        {
            if (textBox_Registracia_Email.Text == "E-mail")
            {
                textBox_Registracia_Email.Text = "";
                textBox_Registracia_Email.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void textBox_Registracia_Email_Leave(object sender, EventArgs e)
        {
            if (textBox_Registracia_Email.Text == "")
            {
                textBox_Registracia_Email.Text = "E-mail";
                textBox_Registracia_Email.ForeColor = System.Drawing.Color.Gray;
            }
        }

        private void textBox_Registracia_Adresa_Click(object sender, EventArgs e)
        {
            if (textBox_Registracia_Adresa.Text == "Adresa, mesto")
            {
                textBox_Registracia_Adresa.Text = "";
                textBox_Registracia_Adresa.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void textBox_Registracia_Adresa_Enter(object sender, EventArgs e)
        {
            if (textBox_Registracia_Adresa.Text == "Adresa, mesto")
            {
                textBox_Registracia_Adresa.Text = "";
                textBox_Registracia_Adresa.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void textBox_Registracia_Adresa_Leave(object sender, EventArgs e)
        {
            if (textBox_Registracia_Adresa.Text == "")
            {
                textBox_Registracia_Adresa.Text = "Adresa, mesto";
                textBox_Registracia_Adresa.ForeColor = System.Drawing.Color.Gray;
            }
        }

        private void textBox_Registracia_Pin_Click(object sender, EventArgs e)
        {
            if (textBox_Registracia_Pin.Text == "Pin")
            {
                textBox_Registracia_Pin.Text = "";
                textBox_Registracia_Pin.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void textBox_Registracia_Pin_Enter(object sender, EventArgs e)
        {
            if (textBox_Registracia_Pin.Text == "Pin")
            {
                textBox_Registracia_Pin.Text = "";
                textBox_Registracia_Pin.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void textBox_Registracia_Pin_Leave(object sender, EventArgs e)
        {
            if (textBox_Registracia_Pin.Text == "")
            { 
            textBox_Registracia_Pin.Text = "Pin";
                textBox_Registracia_Pin.ForeColor = System.Drawing.Color.Gray;
            }
        }



        // Placeholdery - login

        private void textBox_Login_Email_Click(object sender, EventArgs e)
        {
            textBox_Login_Email.TabStop = true;
            textBox_Login_Pin.TabStop = true;
            if (textBox_Login_Email.Text == "E-mail")
            {
                textBox_Login_Email.Text = "";
                textBox_Login_Email.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void textBox_Login_Email_Enter(object sender, EventArgs e)
        { 
            if (textBox_Login_Email.Text == "E-mail")
            {
                textBox_Login_Email.Text = "";
                textBox_Login_Email.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void textBox_Login_Email_Leave(object sender, EventArgs e)
        {
            if (textBox_Login_Email.Text == "")
            {
                textBox_Login_Email.Text = "E-mail";
                textBox_Login_Email.ForeColor = System.Drawing.Color.Gray;
            }
        }

        private void textBox_Login_Pin_Click(object sender, EventArgs e)
        {
            textBox_Login_Email.TabStop = true;
            textBox_Login_Pin.TabStop = true;
            if (textBox_Login_Pin.Text == "Pin")
            {
                textBox_Login_Pin.Text = "";
                textBox_Login_Pin.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void textBox_Login_Pin_Enter(object sender, EventArgs e)
        {
            if (textBox_Login_Pin.Text == "Pin")
            {
                textBox_Login_Pin.Text = "";
                textBox_Login_Pin.ForeColor = System.Drawing.Color.Black;
                textBox_Login_Pin.PasswordChar = '●';
            }
        }

        private void textBox_Login_Pin_Leave(object sender, EventArgs e)
        {
            if (textBox_Login_Pin.Text == "")
            {
                textBox_Login_Pin.PasswordChar = '\0';
                textBox_Login_Pin.Text = "Pin";
                textBox_Login_Pin.ForeColor = System.Drawing.Color.Gray;
            }
        }
    }
}

