using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Diagnostics;

namespace p65_72_Zifcak_Ondra_Michlik
{

    public partial class Form_Login : Form
    {

        // Globalne premenne

        Random rand = new Random();

        string users_subor = "users.csv";

        List<string> zaregistrovane_cisla = new List<string>();
        List<string> zaregistrovane_emaily = new List<string>();
        List<int> zaregistrovane_id = new List<int>();

        public static string idPrihlasenehoPouzivatela;

        // Vytvorenie placeholderov pre register

        private void registerPlaceholder()
        {

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


        // Vytvorenie placeholderu pre textboxy v logine

        private void loginPlaceholder()
        {

            this.ActiveControl = null;

            textBox_Login_Email.Text = "E-mail";
            textBox_Login_Email.ForeColor = System.Drawing.Color.Gray;

            textBox_Login_Pin.Text = "Pin";
            textBox_Login_Pin.ForeColor = System.Drawing.Color.Gray;
        }

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

                if (email == textBox_Login_Email.Text && pin == pinEncryption(textBox_Login_Pin.Text).ToLower())
                {
                    idPrihlasenehoPouzivatela = hodnoty[0];
                    this.Hide();
                    Form_Domov f2 = new Form_Domov();
                    f2.ShowDialog();
                    this.Close();
                }
            }
            MessageBox.Show("Zadal si nespravne udaje!");
        }

        private void pictureBox_Login_Close_Click(object sender, EventArgs e)
        {
            Close();
        }


        // Funkcia na osetrenie vstupu mena a priezviska

        private bool spravneMenoPriezvisko()
        {
            string meno = textBox_Registracia_Meno.Text;
            string priezvisko = textBox_Registracia_Priezvisko.Text;
            meno.ToCharArray();
            priezvisko.ToCharArray();
            for (int i = 0; i < meno.Length; i++)
            {
                if (char.IsDigit(meno[i])) return false;
            }
            for (int j = 0; j < priezvisko.Length; j++)
            {
                if (char.IsDigit(priezvisko[j])) return false;
            }
            if ((meno.Length > 2) && (priezvisko.Length > 2)) return true;
            else return false;
        }

        // Funkcia na osetrenie vstupu telefonneho cisla

        private bool spravneTelefonneCislo()
        {
            string telefonneCislo = textBox_Registracia_Tel_Cislo.Text;
            telefonneCislo.ToCharArray();
            if (telefonneCislo.Length == 10) return true;
            else return false;
        }

        // Funkcia na osetrenie vstupu pre pin

        private bool spravnyPin()
        {
            string pin = textBox_Registracia_Pin.Text;
            pin.ToCharArray();
            if (pin.Length == 4) return true;
            else return false;
        }

        // Jednoducha funkcia na zahashovanie pinu v csv subore
        // Zdroj: https://www.sean-lloyd.com/post/hash-a-string/

        private string pinEncryption(string pin, string salt = "") { 
            
            if(String.IsNullOrEmpty(pin)) return String.Empty;

            // Pouzite SHA256 na zasifrovanie stringu 

            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {

                // Prevedenie pinu na pole typu bajt 
                byte[] textBytes = System.Text.Encoding.UTF8.GetBytes(pin);
                byte[] hashBytes = sha.ComputeHash(textBytes);

                // Prevedenie spat na string + odstranenie "-" a ";" ktore prida funkcia BitConverter
                string hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty).Replace(";", String.Empty);

                return hash;

            }

        }


        // Funkcia na osetrenie vstupu emailu

        private bool spravnyEmail()
        {
            int pocetZavinacov = 0;
            string email = textBox_Registracia_Email.Text;
            email.ToCharArray();
            if (email.Contains('@') && email.Contains('.') && email.Length >= 5)
            {
                for (int i = 0; i < email.Length; i++) { if (email[i] == '@') pocetZavinacov++; }

                if (pocetZavinacov < 2)
                {
                    int zavinacSymbol = email.IndexOf("@");
                    int bodkaSymbol = email.IndexOf(".");
                    if (zavinacSymbol != 0 && email[zavinacSymbol - 1] != '.' && bodkaSymbol != (zavinacSymbol + 1) && bodkaSymbol != (email.Length - 1) && email[bodkaSymbol + 1] != '.') return true;
                    else return false;
                }
                else return false;
            }
            else return false;
        }

        // generovanie cisla uctu
        // funkcia vracia cislo uctu

        private string generaciaUctu()
        {
            rand = new Random();
            string ucet;

            string[] kodyBaniek = new[] { "0900", "1100", "0200", "7500", "6500" };
            int r = rand.Next(0, kodyBaniek.Length);

            string kontrolneCislice = Convert.ToString(rand.Next(0, 10)) + Convert.ToString(rand.Next(0, 10));

            List<string> zakladneCisloUctuList = new List<string>();
            for (int i = 0; i < 10; i++)
            {
                zakladneCisloUctuList.Add(Convert.ToString(rand.Next(0, 10)));
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

            string cislo_uctu = generaciaUctu();
            string meno = textBox_Registracia_Meno.Text;
            string priezvisko = textBox_Registracia_Priezvisko.Text;
            string telefonne_cislo = textBox_Registracia_Tel_Cislo.Text;
            string email = textBox_Registracia_Email.Text;

            string adresa = textBox_Registracia_Adresa.Text;
            string[] adresneUdaje = adresa.Split(',');

            if (adresneUdaje.Length >= 2)
            {
                string adresa_cast = adresneUdaje[0].Trim();
                string mesto_cast = adresneUdaje[1].Trim();

                string pin = textBox_Registracia_Pin.Text;

                string pinHash = pinEncryption(pin);

                StreamWriter users = new StreamWriter(users_subor, true, Encoding.UTF8);

                string udaje_na_zapis = string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9}", id, cislo_uctu, meno, priezvisko, pinHash, telefonne_cislo, email, adresa_cast, mesto_cast, stav_uctu);
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

                MessageBox.Show("Bol si úspešne zaregistrovaný.");
            }
            else
            {
                MessageBox.Show("Adresa musí obsahovať čiarku pre oddelenie adresy a mesta.", "Chyba!");
            }
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

            if (meno != "" && priezvisko != "" && telefonne_cislo != "" && email != "" && adresa != "" && pin != "")
            {
                if (meno != "Meno" && priezvisko != "Priezvisko" && telefonne_cislo != "Telefonne cislo" && email != "E-mail" && adresa != "Adresa" && pin != "Pin")
                {
                    if (spravnyEmail() && spravneTelefonneCislo() && spravneMenoPriezvisko() && spravnyPin())
                    {
                        StreamReader users = new StreamReader(users_subor);
                        string riadok;
                        if (string.IsNullOrEmpty(riadok = users.ReadLine()))
                        {
                            zaregistrovane_cisla.Add(textBox_Registracia_Tel_Cislo.Text);
                            zaregistrovane_emaily.Add(textBox_Registracia_Email.Text);
                            users.Close();
                            zapisanieDoSuboru();
                        }
                        else
                        {
                            string riadok1;
                            while (!string.IsNullOrEmpty(riadok1 = users.ReadLine()))
                            {
                                string[] hodnoty = riadok1.Split(';');
                                string nove_cislo = hodnoty[5].Trim().ToLower();
                                string novy_email = hodnoty[6].Trim().ToLower();
                                zaregistrovane_cisla.Add(nove_cislo);
                                zaregistrovane_emaily.Add(novy_email);
                            }

                            users.Close();

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
                    else if (!spravneMenoPriezvisko()) MessageBox.Show("Zadal si nesprávne meno alebo priezvisko!", "Chyba!");
                    else if (!spravneTelefonneCislo()) MessageBox.Show("Zadal si nesprávne telefónne číslo!", "Chyba!");
                    else if (!spravnyPin()) MessageBox.Show("Zadal si nesprávny pin!", "Chyba!");
                    else if (!spravnyEmail()) MessageBox.Show("Zadal si nesprávny e-mail!", "Chyba!");
                }
                    else MessageBox.Show("Musíš vyplniť všetky údaje!", "Chyba!");
            }
             else MessageBox.Show("Musíš vyplniť všetky údaje!", "Chyba!");
        }

    


        // Osetrenie vstupu na tel. cislo a pin aby tam mohol uzivatel vlozit len cisla

        private void textBox_Registracia_Tel_Cislo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar)) e.Handled = true;
        }

        private void textBox_Registracia_Pin_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar)) e.Handled = true;
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



        // Osetrenie vstupov aby uzivatel nemohol zadat nerealne udaje 
        // a zaroven aby sa udaje zmestili do danych textboxov 
        private void textBox_Registracia_Meno_KeyDown(object sender, KeyEventArgs e)
        {
            if (textBox_Registracia_Meno.Text.Length > 13)
            { 
                if(e.KeyCode != Keys.Back) e.SuppressKeyPress = true;
            }
            if(e.KeyCode == Keys.Enter) e.SuppressKeyPress = true;
        }

        private void textBox_Registracia_Priezvisko_KeyDown(object sender, KeyEventArgs e)
        {
            if (textBox_Registracia_Priezvisko.Text.Length > 13)
            {
                if (e.KeyCode != Keys.Back) e.SuppressKeyPress = true;
            }
            if (e.KeyCode == Keys.Enter) e.SuppressKeyPress = true;
        }

        private void textBox_Registracia_Email_KeyDown(object sender, KeyEventArgs e)
        {
            if (textBox_Registracia_Email.Text.Length > 31)
            {
                if (e.KeyCode != Keys.Back) e.SuppressKeyPress = true;
            }
            if (e.KeyCode == Keys.Enter) e.SuppressKeyPress = true;
        }
    }
}

