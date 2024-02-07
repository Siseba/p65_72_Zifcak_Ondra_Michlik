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
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Timers;
using System.Drawing.Drawing2D;

namespace p65_72_Zifcak_Ondra_Michlik
{
    public partial class Form_Domov : Form
    {
        // Deklarovanie globalnych premien

        string idPouzivatela = Form_Login.idPrihlasenehoPouzivatela;

        Random rand = new Random();

        string users_subor = "users.csv";
        string transactions_subor = "transactions.csv";
        string financie_subor = "financie.csv";
        string users_stavUctu_subor = "users_stavUctu.csv";
        public double stavUctuPouzivatela;
        public string cisloUctuPouzivatela;
        string idPrijimatela;

        double celkovePrijmy;
        double celkoveVydaje;

        List<int> zaregistrovane_id = new List<int>();

        public Form_Domov()
        {
            InitializeComponent();

            // Zapnutie / vypnutie panelov podla potreby

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

            panel_Profil_Graf.Visible = false;

            // Vytvorenie potrebnych suborov ak nie su 

            FileStream transactions_subor;
            FileStream financie_subor;

            if (!File.Exists("transactions.csv"))
            {
                transactions_subor = File.Create("transactions.csv");
                transactions_subor.Close();
            }

            if (!File.Exists("financie.csv"))
            {
                financie_subor = File.Create("financie.csv");
                financie_subor.Close();
            }

            
            // Vytovrenie placeholderu 

            prevodPlaceHolder();

        }

        // Placeholder pre textBoxy v prevode 

        private void prevodPlaceHolder() {

            this.ActiveControl = null;

            textBox_Prevod_Cislo_Prijemcu.Text = "Číslo účtu príjemcu";
            textBox_Prevod_Cislo_Prijemcu.ForeColor = System.Drawing.Color.Gray;

            textBox_Prevod_Suma.Text = "Suma";
            textBox_Prevod_Suma.ForeColor  = System.Drawing.Color.Gray;
        }

        // Picture box na odhlasenie pouzivatela

        private void pictureBox_Odhlasenie_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Boli ste úspešne odhlásený");
            this.Hide();
            Form_Login login = new Form_Login();
            login.ShowDialog();
            this.Close();
        }

        // Funkcia na zistenie cisla uctu prihlaseneho pouzivatela
        // Funkcia precita subor s pouzivatelmi, porovna id a ak sa rovnaju vypise cislo uctu pouzivatela

        private void zistenieCislaUctu()
        {
            StreamReader udajeCisloUctu = new StreamReader(users_subor, Encoding.UTF8);
            string riadok;
            while ((riadok = udajeCisloUctu.ReadLine()) != null)
            {
                string[] hodnoty = riadok.Split(';');
                if (hodnoty[0].Trim().ToLower() == idPouzivatela.ToString().ToLower())
                {
                    cisloUctuPouzivatela = Convert.ToString(hodnoty[1]);
                }
            }
            udajeCisloUctu.Close();
        }

        // Funkcia na zistenie stavu uctu prihlaseneho pouzivatela
        // Funkcia precita subor so stavom uctu pouzivatelov a vypise dany stav prihlaseneho pouzivatela

        private void zistenieStavuUctu()
        {
            StreamReader udajeStavUctu = new StreamReader(users_stavUctu_subor, Encoding.UTF8);
            string riadok;
            while ((riadok = udajeStavUctu.ReadLine()) != null)
            {
                string[] hodnoty = riadok.Split(';');
                if (hodnoty[0].Trim().ToLower() == idPouzivatela.ToString().ToLower())
                {
                    stavUctuPouzivatela = Convert.ToDouble(hodnoty[1]);
                }
            }
            udajeStavUctu.Close();
        }

        // Funkcia na vytvorenie unikatnych id 

        private int unikatneID(string subor)
        {

            zaregistrovane_id.Clear();

            StreamReader users = new StreamReader(subor, Encoding.UTF8);
            string riadok;
            if (string.IsNullOrEmpty(riadok = users.ReadLine()))
            {
                users.Close();
                zaregistrovane_id.Add(Convert.ToInt32(1));
                return 1;
            }
            else
            {
                string riadok1;
                while ((riadok1 = users.ReadLine()) != null)
                {
                    string[] hodnoty = riadok1.Split(';');
                    zaregistrovane_id.Add(Convert.ToInt32(hodnoty[0]));
                }

                users.Close();
                int posledneId = zaregistrovane_id.Count();
                return posledneId + 1;
            }

            
        }

        // PictureBoxy pre ovladanie hlavneho menu

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

            prevodPlaceHolder();

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

            celkovePrijmyVydaje();

            label_Profil_Prijem.Text = " + " + celkovePrijmy.ToString() + " €";
            label_Profil_Prijem.ForeColor = System.Drawing.Color.Green;

            label_Profil_Vydaj.Text = " - " + celkoveVydaje.ToString() + " €";
            label_Profil_Vydaj.ForeColor = System.Drawing.Color.Red;

            zobrazenieInformaciiVProfile();

            panel_Domov.Visible = false;
            panel_Profil.Visible = true;
            panel_Profil.Enabled = true;
            panel_Domov.Enabled = false;
            panel_Profil_Graf.Visible = true;
        }

        private void pictureBox_Profil_Spat_Click(object sender, EventArgs e)
        {
            this.Text = "Domov";

            panel_Domov.Visible = true;
            panel_Profil.Visible = false;
            panel_Profil.Enabled = false;
            panel_Domov.Enabled = true;
            panel_Profil_Graf.Visible = false;
        }

        private void pictureBox_Vypis_Uctu_Click(object sender, EventArgs e)
        {
            this.Text = "Vypis";

            panel_Domov.Visible = false;
            panel_VypisUctu.Visible = true;
            panel_VypisUctu.Enabled = true;
            panel_Domov.Enabled = false;

            zistenieCislaUctu();
            vypisUctu();

        }

        private void pictureBox_Vypis_Spat_Click(object sender, EventArgs e)
        {
            this.Text = "Domov";

            panel_Domov.Visible = true;
            panel_VypisUctu.Visible = false;
            panel_VypisUctu.Enabled = false;
            panel_Domov.Enabled = true;
        }

        // Funkcia na zapisanie do subora "users_stavUctu.csv"

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

        // Funkcia na zapisanie prevodu do suboru "transactions.csv"

        private void zapisDoSuboraTransactions(string cisloUctuPrijimatela)
        {
            int id = unikatneID(transactions_subor);
            StreamWriter transactions_file = new StreamWriter(transactions_subor, true, Encoding.UTF8);

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
        }

        // Funkcia na odcitanie penazi z uctu prihlaseneho pouzivatela 
        // a pripocitanie penazi na ucte prijimatela

        private void pictureBox_Prevod_Odoslat_Click_1(object sender, EventArgs e)
        {
            try
            {
                string cisloUctuPrijimatela = textBox_Prevod_Cislo_Prijemcu.Text;
                double sumaPrevod = Convert.ToDouble(textBox_Prevod_Suma.Text);

                StreamReader udajeStavUctu = new StreamReader(users_subor, Encoding.UTF8);
                string riadok;
                while ((riadok = udajeStavUctu.ReadLine()) != null)
                {
                    string[] hodnoty = riadok.Split(';');
                    if (hodnoty[1] == cisloUctuPrijimatela)
                    {
                        idPrijimatela = Convert.ToString(hodnoty[0]);
                    }
                }
                udajeStavUctu.Close();

                if (cisloUctuPrijimatela.Length == 24)
                {
                    if (cisloUctuPouzivatela != textBox_Prevod_Cislo_Prijemcu.Text)
                    {
                        // regex funkcia pomocou chatGPT
                        if (cisloUctuPrijimatela.StartsWith("SK") && Regex.IsMatch(cisloUctuPrijimatela.Substring(2), @"^\d+$"))
                        {
                            if (sumaPrevod >= 5)
                            {
                                StreamReader users_file = new StreamReader(users_stavUctu_subor, Encoding.UTF8);
                                string[] usersFileLine = users_file.ReadToEnd().Split(new char[] { '\n' });
                                users_file.Close();

                                for (int i = 0; i < usersFileLine.Length - 1; i++)
                                {
                                    string line = usersFileLine[i];
                                    string[] lineKusy = line.Split(';');
                                    string hladaneId = lineKusy[0];
                                    double stavUctu = Convert.ToDouble(lineKusy[1]);
                                    if (hladaneId == idPrijimatela)
                                    {
                                        stavUctu += sumaPrevod;
                                        stavUctu = Math.Round(stavUctu, 2);
                                        string[] parts = usersFileLine[i].Split(';');
                                        parts[1] = Convert.ToString(stavUctu);
                                        usersFileLine[i] = string.Join(";", parts);

                                        zapisDoSubora(usersFileLine);

                                        zistenieStavuUctu();
                                        label_Prevod_Stav_Uctu.Text = "Stav účtu:" + stavUctuPouzivatela.ToString() + "€";
                                    }

                                    if (hladaneId == idPouzivatela)
                                    {
                                        if (!((stavUctu - sumaPrevod) < 0))
                                        {
                                            stavUctu -= sumaPrevod;
                                            stavUctu = Math.Round(stavUctu, 2);
                                            string[] parts = usersFileLine[i].Split(';');
                                            parts[1] = Convert.ToString(stavUctu);
                                            usersFileLine[i] = string.Join(";", parts);

                                            zapisDoSubora(usersFileLine);

                                            zistenieStavuUctu();
                                            label_Prevod_Stav_Uctu.Text = "Stav účtu:" + stavUctuPouzivatela.ToString() + "€";

                                            zapisDoSuboraTransactions(cisloUctuPrijimatela);
                                        }
                                        else
                                        {
                                            MessageBox.Show("Nemáte dostatok financií na prevod!");
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Minimálna suma na prevod je 5€");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Účet musí začínať s predponou SK a zvšok musia byť číslice");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Tento druh tranzakcie nie je možný!");
                    }
                }

                   
                else
                {
                    MessageBox.Show("Zadaný účet musí obsahovať 24 znakov");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Musíš vyplniť všetky údaje!!");
            }
        }

        // Funkcia na vklad penazi na ucet prihlaseneho pouzivatela

        private void pictureBox_Vklad_Vykonat_Click(object sender, EventArgs e)
        {
           try
           {
                //generovanie ID tranzakcie s aktuálnym dátumom 
                int id_tranzakcie = unikatneID(financie_subor);
                string id_osoba = idPouzivatela;

                double suma = Convert.ToDouble(textBox_Vklad.Text);

                if (suma <= 0)
                {
                    MessageBox.Show("Musíš zadať viac ako 0 eur");
                }
                else
                {
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
                            stavUctu += suma;
                            stavUctu = Math.Round(stavUctu, 2);
                            string[] parts = usersFileLine[i].Split(';');
                            parts[1] = Convert.ToString(stavUctu);
                            usersFileLine[i] = string.Join(";", parts);
                            zapisDoSubora(usersFileLine);

                            //pomoc ChatGPT
                            string zapis = $"{id_tranzakcie};{id_osoba};vklad;{suma};{DateTime.Now:dd.MM.yyyy}";

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
            }
            catch (Exception)
            {
                MessageBox.Show("Musíš zadať sumu!");
           }
        }

        // Funkcia na vybratie penazi z uctu prihlaseneho pouzivatela

        private void pictureBox_Vyber_Vykonat_Click(object sender, EventArgs e)
        {
            try
            {
                //generovanie ID tranzakcie s aktuálnym dátumom 
                int id_vyber = unikatneID(financie_subor);
                string id_osoba = idPouzivatela;

                double suma = Convert.ToDouble(textBox_Vyber.Text);

                if (suma <= 0)
                {
                    MessageBox.Show("Musíš zadať viac ako 0 eur");
                }
                else
                {
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
                            if (!((stavUctu - suma) < 0))
                            {
                                stavUctu -= Convert.ToDouble(suma);
                                stavUctu = Math.Round(stavUctu, 2);
                                string[] parts = usersFileLine[i].Split(';');
                                parts[1] = Convert.ToString(stavUctu);
                                usersFileLine[i] = string.Join(";", parts);
                                zapisDoSubora(usersFileLine);

                                //pomoc ChatGPT
                                string zapis = $"{id_vyber};{id_osoba};vyber;{suma};{DateTime.Now:dd.MM.yyyy}";

                                // Zápis do súboru
                                StreamWriter finance = new StreamWriter(financie_subor, true, Encoding.UTF8);
                                finance.WriteLine(zapis);
                                finance.Close();

                                break;
                            }
                        }
                    }

                    // ošetrenie po zápise vkladu
                    textBox_Vyber.Text = "";

                    //oznámenie o vykonanom vklade
                    MessageBox.Show("Výber bol zaznamenaný.");
                    zistenieStavuUctu();
                    label_Vyber_Stav_Uctu.Text = "Stav účtu:" + stavUctuPouzivatela.ToString() + "€";
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Musíš zadať sumu!");
            }
        }

        // Funkcia na zobrazenie udajov v profile

        private void zobrazenieInformaciiVProfile()
        {

            zistenieStavuUctu();

            int pocetCislic = stavUctuPouzivatela.ToString().Length;

            StreamReader udaje = new StreamReader(users_subor);
            string riadok;
            while ((riadok = udaje.ReadLine()) != null)
            {
                string[] hodnoty = riadok.Split(';');

                string id = hodnoty[0];

                if (idPouzivatela == id)
                {
                    label_Profil_Zostatok_Na_Ucte.Location = new Point(1256 - pocetCislic * 10, 135);
                    label_Profil_Meno.Text = $"{hodnoty[2]}" + " " + $"{hodnoty[3]}";
                    label_Profil_TelCislo.Text = $"{hodnoty[5]}";
                    label_Profil_Email.Text = $"{hodnoty[6]}";
                    label_Profil_Zostatok_Na_Ucte.Text = "€ " + stavUctuPouzivatela.ToString();
                    label_Profil_Adresa.Text = $"{hodnoty[7]}" + ", " + $" {hodnoty[8]}";
                }

            }
            udaje.Close();

        }

        

        // Funkcia na zistenie celkovych prijimov a vydajov prihlaseneho uzivatela

        private void celkovePrijmyVydaje() {

            celkovePrijmy = 0;
            celkoveVydaje = 0;

            StreamReader udajeVkladVyber = new StreamReader(financie_subor, Encoding.UTF8);
            string riadok;
            while (!string.IsNullOrEmpty(riadok = udajeVkladVyber.ReadLine()))
            {
                string[] hodnoty = riadok.Split(';');

                if (hodnoty[1] == idPouzivatela.ToString())
                {
                    if (hodnoty[2] == "vklad")
                    {
                        celkovePrijmy += Convert.ToDouble(hodnoty[3]);
                    }
                    else if (hodnoty[2] == "vyber")
                    {
                        celkoveVydaje += Convert.ToDouble(hodnoty[3]);
                    }
                }
            }
            udajeVkladVyber.Close();

            zistenieCislaUctu();

            StreamReader udajeTranzakcie = new StreamReader(transactions_subor, Encoding.UTF8);
            string riadok1;
            while (!string.IsNullOrEmpty(riadok1 = udajeTranzakcie.ReadLine()))
            {
                string[] hodnoty1 = riadok1.Split(';');

                if (hodnoty1[2] == cisloUctuPouzivatela && hodnoty1[1] == idPouzivatela.ToString())
                {
                    celkoveVydaje += Convert.ToDouble(hodnoty1[4]);
                }
                else if (hodnoty1[2] != cisloUctuPouzivatela && hodnoty1[3] == cisloUctuPouzivatela)
                {
                    celkovePrijmy += Convert.ToDouble(hodnoty1[4]);
                }
            }
            udajeTranzakcie.Close();

        }


        // Funkcia na vykreslenie grafu prijimov / vydajov v profile

        private void panel_Profil_Graf_Paint(object sender, PaintEventArgs e)
        {
            Graphics kp = e.Graphics;

          
            float maxSirka = 500f;

            double celkovyObrat = (Convert.ToDouble(celkovePrijmy) + Convert.ToDouble(celkoveVydaje));
            float sirkaGrafPrijmyPercenta = (float)((celkovePrijmy) / celkovyObrat) * 100f;
            float sirkaGrafVydajePercenta = (float)((celkoveVydaje) / celkovyObrat) * 100f;

            float sirkaGrafPrijmy = (sirkaGrafPrijmyPercenta / 100 * maxSirka);
            float sirkaGrafVydaje = (sirkaGrafVydajePercenta / 100 * maxSirka);

            kp.FillRectangle(Brushes.White, 10, 10, 10, 240);


            kp.FillRectangle(Brushes.Green, 20, 50,sirkaGrafPrijmy, 60);
            kp.FillRectangle(Brushes.Red, 20, 160, sirkaGrafVydaje, 60);

        }

        // Funkcia na vypisanie prijimov / vydajov / prevodov prihlaseneho uzivatela

        private void vypisUctu()
        {
            StreamReader financie_zistenie_riadkov = new StreamReader(financie_subor, Encoding.UTF8);
            string riadok;
            int data_riadky = 0;
            while ((riadok = financie_zistenie_riadkov.ReadLine()) != null)
            {
                string[] hodnoty = riadok.Split(';');
                if (hodnoty[1] == idPouzivatela)
                {
                    data_riadky += 1;
                }
            }
            financie_zistenie_riadkov.Close();

            StreamReader transactions_zistenie_riadokov = new StreamReader(transactions_subor, Encoding.UTF8);
            while ((riadok = transactions_zistenie_riadokov.ReadLine()) != null)
            {
                string[] hodnoty = riadok.Split(';');
                if (cisloUctuPouzivatela == hodnoty[2] || cisloUctuPouzivatela == hodnoty[3])
                {
                    data_riadky += 1;
                }
            }
            transactions_zistenie_riadokov.Close();

            string[,] data = new string[data_riadky, 5];
            StreamReader financie = new StreamReader(financie_subor, Encoding.UTF8);
            int riadok_pole = 0;
            while ((riadok = financie.ReadLine()) != null)
            {
                string[] hodnoty = riadok.Split(';');
                string hladaneId = hodnoty[1];
                if (hladaneId == idPouzivatela)
                {
                    data[riadok_pole, 0] = hodnoty[2];
                    data[riadok_pole, 1] = cisloUctuPouzivatela;
                    data[riadok_pole, 2] = cisloUctuPouzivatela;
                    if (hodnoty[2] == "vyber")
                    {
                        data[riadok_pole, 3] = "-" + hodnoty[3];
                    }
                    else
                    {
                        data[riadok_pole, 3] = "+" + hodnoty[3];
                    }
                    data[riadok_pole, 4] = hodnoty[4];
                    riadok_pole += 1;
                }
            }
            financie.Close();

            StreamReader transactions = new StreamReader(transactions_subor, Encoding.UTF8);
            while ((riadok = transactions.ReadLine()) != null)
            {
                string[] hodnoty = riadok.Split(';');
                if (hodnoty[3] == cisloUctuPouzivatela)
                {
                    data[riadok_pole, 0] = "prevod";
                    data[riadok_pole, 1] = hodnoty[2];
                    data[riadok_pole, 2] = cisloUctuPouzivatela;
                    data[riadok_pole, 3] = "+" + hodnoty[4];
                    data[riadok_pole, 4] = hodnoty[5];
                    riadok_pole += 1;
                }
                string hladaneId = hodnoty[1];
                if (hladaneId == idPouzivatela)
                {
                    data[riadok_pole, 0] = "prevod";
                    data[riadok_pole, 1] = cisloUctuPouzivatela;
                    data[riadok_pole, 2] = hodnoty[3];
                    if (cisloUctuPouzivatela == hodnoty[2])
                    {
                        data[riadok_pole, 3] = "-" + hodnoty[4];
                    }
                    data[riadok_pole, 4] = hodnoty[5];
                    riadok_pole += 1;
                }
            }
            transactions.Close();

            dataGridView_vypis.Rows.Clear();
            dataGridView_vypis.ColumnCount = 6;
            int data_id = 1;

            for (int i = 0; i < data_riadky; i++)
            {
                string[] data_riadok = new string[6];
                data_riadok[0] = Convert.ToString(data_id);
                data_id += 1;
                for (int j = 1; j < 6; j++)
                {
                    data_riadok[j] = data[i, j - 1];
                }
                dataGridView_vypis.Rows.Add(data_riadok);
            }

            dataGridView_vypis.Sort(dataGridView_vypis.Columns["datum"], ListSortDirection.Descending);
        }

        // Placeholdery pre prevod na ucet

        private void textBox_Prevod_Cislo_Prijemcu_Click(object sender, EventArgs e)
        {
            if (textBox_Prevod_Cislo_Prijemcu.Text == "Číslo účtu príjemcu")
            { 
                textBox_Prevod_Cislo_Prijemcu.Text = "";
                textBox_Prevod_Cislo_Prijemcu.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void textBox_Prevod_Cislo_Prijemcu_Enter(object sender, EventArgs e)
        {
            if (textBox_Prevod_Cislo_Prijemcu.Text == "Číslo účtu príjemcu")
            {
                textBox_Prevod_Cislo_Prijemcu.Text = "";
                textBox_Prevod_Cislo_Prijemcu.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void textBox_Prevod_Cislo_Prijemcu_Leave(object sender, EventArgs e)
        {
            if (textBox_Prevod_Cislo_Prijemcu.Text == "")
            {
                textBox_Prevod_Cislo_Prijemcu.Text = "Číslo účtu príjemcu";
                textBox_Prevod_Cislo_Prijemcu.ForeColor = System.Drawing.Color.Gray;
            }
        }

        private void textBox_Prevod_Suma_Click(object sender, EventArgs e)
        {
            if (textBox_Prevod_Suma.Text == "Suma")
            {
                textBox_Prevod_Suma.Text = "";
                textBox_Prevod_Suma.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void textBox_Prevod_Suma_Enter(object sender, EventArgs e)
        {
            if (textBox_Prevod_Suma.Text == "Suma")
            {
                textBox_Prevod_Suma.Text = "";
                textBox_Prevod_Suma.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void textBox_Prevod_Suma_Leave(object sender, EventArgs e)
        {
            if (textBox_Prevod_Suma.Text == "")
            {
                textBox_Prevod_Suma.Text = "Suma";
                textBox_Prevod_Suma.ForeColor = System.Drawing.Color.Gray;
            }
        }
    }
}
