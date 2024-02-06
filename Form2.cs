using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net.Http.Headers;

namespace p65_72_Zifcak_Ondra_Michlik
{
    public partial class Form_Domov : Form
    {
        string idPouzivatela = Form_Login.idPrihlasenehoPouzivatela;

        Random rand = new Random();

        string users_subor = "users2.csv";
        string transactions_subor = "transactions.csv";
        string financie_subor = "financie.csv";
        string users_stavUctu_subor = "users_stavUctu.csv";
        public double stavUctuPouzivatela;
        public string cisloUctuPouzivatela;

        public Form_Domov()
        {
            InitializeComponent();
            panel_Domov.Enabled = true;
            panel_Domov.Visible = true;

            panel_Vklad.Enabled = false;
            panel_Vklad.Visible = false;

            panel_Vyber.Enabled = false;
            panel_Vyber.Visible = false;
        
            panel_Prevod_Na_Ucet.Enabled = false;
            panel_Prevod_Na_Ucet.Visible = false;

            panel_Profil.Enabled = false;
            panel_Profil.Visible = false;

        }

        private void pictureBox_Odhlasenie_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Boli ste úspešne odhlásený");
            this.Hide();
            Form_Login login = new Form_Login();
            login.ShowDialog();
            this.Close();
        }

        private void zistenieCislaUctu()
        {
            StreamReader udajeStavUctu = new StreamReader(users_subor, Encoding.UTF8);
            string riadok;
            while ((riadok = udajeStavUctu.ReadLine()) != null)
            {
                string[] hodnoty = riadok.Split(';');
                if (hodnoty[0].Trim().ToLower() == idPouzivatela.ToString().ToLower())
                {
                    cisloUctuPouzivatela = Convert.ToString(hodnoty[1]);
                }
            }
            udajeStavUctu.Close();
        }

        private void zistenieStavuUctu() { 
            StreamReader udaje = new StreamReader(users_stavUctu_subor, Encoding.UTF8);
            string riadok;
            while((riadok = udaje.ReadLine()) != null) {
                string[] hodnoty = riadok.Split(';');
                if (hodnoty[0].Trim().ToLower() == idPouzivatela.ToString().ToLower()) {
                    stavUctuPouzivatela = Convert.ToDouble(hodnoty[1]);
                }
            }
            udaje.Close();
        }

        private void pictureBox_Vklad_Click(object sender, EventArgs e)
        {
            zistenieStavuUctu();

            this.Text = "Vklad";

            label_Vklad_Stav_Uctu.Text = "Stav účtu:" + stavUctuPouzivatela.ToString() + "€";
            textBox_Vklad.Text = "";
            panel_Domov.Visible = false;
            panel_Vklad.Visible = true;
            panel_Vklad.Enabled = true;
            panel_Domov.Enabled = false;

        }

        private void pictureBox_Vklad_Spat_Click(object sender, EventArgs e)
        {
            this.Text = "Domov";

            textBox_Vklad.Text = "";
            panel_Domov.Visible = true;
            panel_Vklad.Visible = false;
            panel_Vklad.Enabled = false;
            panel_Domov.Enabled = true;
        }

        private void pictureBox_Vyber_Click(object sender, EventArgs e)
        {
            this.Text = "Vyber";

            zistenieStavuUctu();
            label_Vyber_Stav_Uctu.Text = "Stav účtu: " + stavUctuPouzivatela.ToString() + "€";
            textBox_Vyber.Text = "";
            panel_Domov.Visible = false;
            panel_Vyber.Visible = true;
            panel_Vyber.Enabled = true;
            panel_Domov.Enabled = false;
        }

        private void pictureBox_Vyber_Spat_Click(object sender, EventArgs e)
        {
            this.Text = "Domov";

            textBox_Vyber.Text = "";
            panel_Domov.Visible = true;
            panel_Vyber.Visible = false;
            panel_Vyber.Enabled = false;
            panel_Domov.Enabled = true;
        }

        private void pictureBox_Prevod_Na_Ucet_Click(object sender, EventArgs e)
        {
            zistenieStavuUctu();
            zistenieCislaUctu();

            this.Text = "Prevod";

            label_Prevod_Stav_Uctu.Text = "Stav účtu:" + stavUctuPouzivatela.ToString() + "€";

            textBox_Prevod_Cislo_Odosielatel.Text = cisloUctuPouzivatela;
            textBox_Prevod_Cislo_Prijemcu.Text = "";
            textBox_Prevod_Suma.Text = "";

            panel_Domov.Visible = false;
            panel_Prevod_Na_Ucet.Visible = true;
            panel_Prevod_Na_Ucet.Enabled = true;
            panel_Domov.Enabled = false;
        }

        private void pictureBox_Prevod_Spat_Click(object sender, EventArgs e)
        {
            this.Text = "Domov";

            textBox_Prevod_Cislo_Odosielatel.Text = "";
            textBox_Prevod_Cislo_Prijemcu.Text = "";
            textBox_Prevod_Suma.Text = "";

            panel_Domov.Visible = true;
            panel_Prevod_Na_Ucet.Visible = false;
            panel_Prevod_Na_Ucet.Enabled = false;
            panel_Domov.Enabled = true;
        }

        private void pictureBox_Profil_Click(object sender, EventArgs e)
        {
            this.Text = "Profil";

            panel_Domov.Visible = false;
            panel_Profil.Visible = true;
            panel_Profil.Enabled = true;
            panel_Domov.Enabled = false;
        }

        private void pictureBox_Profil_Spat_Click(object sender, EventArgs e)
        {
            this.Text = "Domov";

            panel_Domov.Visible = true;
            panel_Profil.Visible = false;
            panel_Profil.Enabled = false;
            panel_Domov.Enabled = true;
        }

        private void zapisDoSubora(string[] usersFileLine)
        {
            using (StreamWriter stream = new StreamWriter(users_stavUctu_subor))
            {
                for (int j = 0; j < usersFileLine.Length - 1; j++)
                {
                    stream.WriteLine(usersFileLine[j]);
                }
            }
        }

        private void pictureBox_Prevod_Odoslat_Click_1(object sender, EventArgs e)
        {
            string cisloUctuPrijimatela = textBox_Prevod_Cislo_Prijemcu.Text;
            double sumaPrevod = Convert.ToDouble(textBox_Prevod_Suma.Text);

            StreamReader users_file = new StreamReader(users_stavUctu_subor, Encoding.UTF8);
            string[] usersFileLine = users_file.ReadToEnd().Split(new char[] { '\n' });
            users_file.Close();

            for (int i = 0; i < usersFileLine.Length - 1; i++)
            {
                string line = usersFileLine[i];
                string[] lineKusy = line.Split(';');
                string hladaneId = lineKusy[0];
                double stavUctu = Convert.ToDouble(lineKusy[1]);
                if (hladaneId == idPouzivatela)
                {
                    if (!((stavUctu - sumaPrevod) < 0))
                    {
                        stavUctu -= sumaPrevod;
                        string[] parts = usersFileLine[i].Split(';');
                        parts[1] = Convert.ToString(stavUctu);
                        usersFileLine[i] = string.Join(";", parts);

                        zapisDoSubora(usersFileLine);

                        zistenieStavuUctu();
                        label_Prevod_Stav_Uctu.Text = "Stav účtu:" + stavUctuPouzivatela.ToString() + "€";

                        StreamWriter transactions_file = new StreamWriter(transactions_subor, true, Encoding.UTF8);

                        int id = rand.Next(1000, 10000);
                        string idOsoba = idPouzivatela;
                        string odosielatel = cisloUctuPouzivatela;
                        string prijimatel = cisloUctuPrijimatela;
                        string suma = textBox_Prevod_Suma.Text;

                        DateTime dateAndTime = DateTime.Now;
                        string datum = dateAndTime.ToString("dd.MM.yyyy");

                        string udaje_na_zapis = string.Format("{0};{1};{2};{3};{4};{5}", id, idOsoba, odosielatel, prijimatel, suma, datum);
                        transactions_file.WriteLine(udaje_na_zapis);

                        textBox_Prevod_Cislo_Prijemcu.Text = "";
                        textBox_Prevod_Suma.Text = "";

                        MessageBox.Show("Transakcia prebehla úspešne!!!");

                        transactions_file.Close();

                        break;
                    }
                    else
                    {
                        MessageBox.Show("Nemáte dostatok financií na prevod!");
                        break;
                    }
                }
            }

        }

        private void pictureBox_Vklad_Vykonat_Click(object sender, EventArgs e)
        {
            try
            {
                //generovanie ID tranzakcie s aktuálnym dátumom 
                int id_tranzakcie = rand.Next(100000, 1000000);
                string id_osoba = idPouzivatela;

                string suma = textBox_Vklad.Text;

                StreamReader users_file = new StreamReader(users_stavUctu_subor, Encoding.UTF8);
                string[] usersFileLine = users_file.ReadToEnd().Split(new char[] { '\n' });
                users_file.Close();

                for (int i = 0; i < usersFileLine.Length - 1; i++)
                {
                    string line = usersFileLine[i];
                    string[] lineKusy = line.Split(';');
                    string hladaneId = lineKusy[0];
                    double stavUctu = Convert.ToDouble(lineKusy[1]);
                    if (hladaneId == idPouzivatela)
                    {
                        stavUctu += Convert.ToDouble(suma);
                        string[] parts = usersFileLine[i].Split(';');
                        parts[1] = Convert.ToString(stavUctu);
                        usersFileLine[i] = string.Join(";", parts);
                        zapisDoSubora(usersFileLine);

                        //pomoc ChatGPT
                        string zapis = $"{id_tranzakcie},{id_osoba},vklad,{suma},{DateTime.Now:dd.MM.yyyy}";

                        // Zápis do súboru
                        StreamWriter finance = new StreamWriter(financie_subor, true, Encoding.UTF8);
                        finance.WriteLine(zapis);
                        finance.Close();

                        break;
                    }
                }

                // ošetrenie po zápise vkladu
                textBox_Vklad.Text = "";

                //oznámenie o vykonanom vklade
                MessageBox.Show("Vklad bol zaznamenaný.");
                zistenieStavuUctu();
                label_Vklad_Stav_Uctu.Text = "Stav účtu:" + stavUctuPouzivatela.ToString() + "€";
            }
            catch (Exception)
            {
                MessageBox.Show("Musíš zadať sumu!");
            }
        }
    }
}
