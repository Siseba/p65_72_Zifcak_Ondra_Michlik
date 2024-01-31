using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
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

            textBox_Registracia_Tel_Cislo.Text = "Telefonne cislo";
            textBox_Registracia_Tel_Cislo.ForeColor = System.Drawing.Color.Gray;

            textBox_Registracia_Email.Text = "E-mail";
            textBox_Registracia_Email.ForeColor = System.Drawing.Color.Gray;

            textBox_Registracia_Adresa.Text = "Adresa, mesto";
            textBox_Registracia_Adresa.ForeColor = System.Drawing.Color.Gray;

        }

        private void loginPlaceholder() {
            // Vytvorenie placeholderu pre textboxy v logine

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
            FileStream file;
            
            if (!File.Exists(users_subor))
            {
                file = File.Create(users_subor);
                file.Close();
            }

            loginPlaceholder();
            this.ActiveControl = null;
        }
       
        // Vytvorenie placeholderu pre textboxy v registracii 


        // Prepinanie medzi login a register - Ondra

        private void pictureBox_Login_Register_Click(object sender, EventArgs e)
        {
            this.ActiveControl = null;

            registerPlaceholder();

            panel_Login.Visible = false;
            panel_Registracia.Visible = true;
        }

        private void pictureBox_Registracia_Cancel_Click(object sender, EventArgs e)
        {
            this.ActiveControl = null;
            loginPlaceholder();

            panel_Registracia.Visible = false;
            panel_Login.Visible = true;
        }


        // Picture box ( tlacidlo ) na login - Zifcak

        private void pictureBox_Login_Click(object sender, EventArgs e)
        {
            StreamReader users = new StreamReader("users.csv", Encoding.UTF8);

            string hlavicka = users.ReadLine();

            string riadok;
            while ((riadok = users.ReadLine()) != null)
            {
                string[] hodnoty = riadok.Split(';');

                string email = hodnoty[4].Trim().ToLower();
                string pin = hodnoty[9].Trim().ToLower();

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

        private void zapisanieDoSuboru()
        {

            int id = rand.Next(1000, 10000);
            int cislo_uctu = 121212;
            int stav_uctu = 0;

            string adresa = "hutnicka";
            string mesto = "SNV";
            

            string meno = textBox_Registracia_Meno.Text;
            string priezvisko = textBox_Registracia_Priezvisko.Text;
            string telefonne_cislo = textBox_Registracia_Tel_Cislo.Text;
            string email = textBox_Registracia_Email.Text;
            //string adresa = textBox_Registracia_Adresa.Text;


            StreamWriter users = new StreamWriter(users_subor, true, Encoding.UTF8);

            string udaje_na_zapis = string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8}", id, cislo_uctu, meno, priezvisko, telefonne_cislo, email, adresa, mesto, stav_uctu);
            users.WriteLine(udaje_na_zapis);

            users.Flush();
            users.Close();

            textBox_Registracia_Meno.Text = "";
            textBox_Registracia_Priezvisko.Text = "";
            textBox_Registracia_Tel_Cislo.Text = "";
            textBox_Registracia_Email.Text = "";
            textBox_Registracia_Adresa.Text = "";

            MessageBox.Show("Uspesne zaregistrovany :D");

        }

        // okej

        // Picture box ( tlacidlo ) na register - Ondra

        private void pictureBox_Registracia_Register_Click(object sender, EventArgs e)
        {


            string meno = textBox_Registracia_Meno.Text;
            string priezvisko = textBox_Registracia_Priezvisko.Text;
            string telefonne_cislo = textBox_Registracia_Tel_Cislo.Text;
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
                    string nove_cislo = hodnoty[4].Trim().ToLower();
                    string novy_email = hodnoty[5].Trim().ToLower();
                    zaregistrovane_cisla.Add(nove_cislo);
                    zaregistrovane_emaily.Add(novy_email);
                }

                users.Close();
            }

            
            // Podmienka aby sa uzivatel nemohol zaregistrovat bez zadania vsetkych udajov

            if (meno != "" && priezvisko != "" && telefonne_cislo != ""  && email != "" && adresa != "" )
            {
                if(meno != "Meno" && priezvisko != "Priezvisko" && telefonne_cislo != "Telefonne cislo" && email != "email" && adresa != "adresa")
                if (zaregistrovane_emaily.Contains(textBox_Registracia_Email.Text))
                {
                    MessageBox.Show("Ucet s tymto emailom uz existuje D:");
                }
                else
                {
                    zapisanieDoSuboru();
                }
            }

        }

        // Osetrenie vstupu na tel. cislo aby tam mohol uzivatel vlozit len cisla

        private void textBox_Registracia_Tel_Cislo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        // Placeholdery - registracia

        private void textBox_Registracia_Meno_Click(object sender, EventArgs e)
        {
            bool placeholder = textBox_Registracia_Meno.Text == "Meno";
            if (textBox_Registracia_Meno.Focused && placeholder)
            {
                textBox_Registracia_Meno.Text = "";
                textBox_Registracia_Meno.ForeColor = System.Drawing.Color.Black;
            }

        }

        private void textBox_Registracia_Priezvisko_Click(object sender, EventArgs e)
        {
            bool placeholder = textBox_Registracia_Priezvisko.Text == "Priezvisko";
            bool empty = textBox_Registracia_Priezvisko.Text == "";
            if (textBox_Registracia_Priezvisko.Focused && placeholder)
            {
                textBox_Registracia_Priezvisko.Text = "";
                textBox_Registracia_Priezvisko.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void textBox_Registracia_Tel_Cislo_Click(object sender, EventArgs e)
        {
            bool placeholder = textBox_Registracia_Tel_Cislo.Text == "Telefonne cislo";
            if (textBox_Registracia_Tel_Cislo.Focused && placeholder)
            {
                textBox_Registracia_Tel_Cislo.Text = "";
                textBox_Registracia_Tel_Cislo.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void textBox_Registracia_Email_Click(object sender, EventArgs e)
        {
            bool placeholder = textBox_Registracia_Email.Text == "E-mail";
            if (textBox_Registracia_Email.Focused && placeholder)
            {
                textBox_Registracia_Email.Text = "";
                textBox_Registracia_Email.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void textBox_Registracia_Adresa_Click(object sender, EventArgs e)
        {
            bool placeholder = textBox_Registracia_Adresa.Text == "Adresa, mesto";
            if (textBox_Registracia_Adresa.Focused && placeholder)
            {
                textBox_Registracia_Adresa.Text = "";
                textBox_Registracia_Adresa.ForeColor = System.Drawing.Color.Black;
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

        private void textBox_Registracia_Priezvisko_Leave(object sender, EventArgs e)
        {
            if (textBox_Registracia_Priezvisko.Text == "")
            {
                textBox_Registracia_Priezvisko.Text = "Priezvisko";
                textBox_Registracia_Priezvisko.ForeColor = System.Drawing.Color.Gray;
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

        private void textBox_Registracia_Email_Leave(object sender, EventArgs e)
        {
            if (textBox_Registracia_Email.Text == "")
            {
                textBox_Registracia_Email.Text = "E-mail";
                textBox_Registracia_Email.ForeColor = System.Drawing.Color.Gray;
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

        // Placeholdery - login

        private void textBox_Login_Email_Click(object sender, EventArgs e)
        {
            bool placeholder = textBox_Login_Email.Text == "E-mail";
            if (textBox_Login_Email.Focused && placeholder)
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
            bool placeholder = textBox_Login_Pin.Text == "Pin";
            if (textBox_Login_Pin.Focused && placeholder)
            {
                textBox_Login_Pin.Text = "";
                textBox_Login_Pin.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void textBox_Login_Pin_Leave(object sender, EventArgs e)
        {
            if (textBox_Login_Pin.Text == "")
            { 
                textBox_Login_Pin.Text = "Pin";
                textBox_Login_Pin.ForeColor = System.Drawing.Color.Gray;
            }
        }

    }
}

